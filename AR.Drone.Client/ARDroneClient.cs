using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using AI.Core.System;
using AR.Drone.Client.Command;
using AR.Drone.Client.Navigation;
using AR.Drone.Client.Video;
using AR.Drone.Client.Workers;

namespace AR.Drone.Client
{
    public class ARDroneClient : DisposableBase
    {
        private readonly ConcurrentQueue<ATCommand> _commandQueue;

        private readonly CommandQueueWorker _commandQueueWorker;
        private readonly ARDroneConfig _config;
        private readonly ConfigAcquisitionWorker _configAcquisitionWorker;
        private readonly NavdataAcquisitionWorker _navdataAcquisitionWorker;
        private readonly NetworkWorker _networkWorker;
        private readonly VideoAcquisitionWorker _videoAcquisitionWorker;
        private readonly WatchdogWorker _watchdogWorker;

        private NavigationData _navigationData;
        private Request _request;

        public ARDroneClient()
        {
            _config = new ARDroneConfig();
            _commandQueue = new ConcurrentQueue<ATCommand>();

            _networkWorker = new NetworkWorker(_config, OnConnectionChanged);
            _commandQueueWorker = new CommandQueueWorker(_config, _commandQueue);
            _navdataAcquisitionWorker = new NavdataAcquisitionWorker(_config, OnNavigationPacketAcquired);
            _videoAcquisitionWorker = new VideoAcquisitionWorker(_config, OnVideoPacketAcquired);
            _configAcquisitionWorker = new ConfigAcquisitionWorker(_config, OnConfigurationPacketAcquired);
            _watchdogWorker = new WatchdogWorker(_networkWorker, _navdataAcquisitionWorker, _commandQueueWorker, _videoAcquisitionWorker);
        }

        public Action<NavigationPacket> NavigationPacketAcquired { get; set; }
        public Action<NavigationData> NavigationDataDecoded { get; set; }
        public Action<VideoPacket> VideoPacketAcquired { get; set; }

        public bool Active
        {
            get { return _watchdogWorker.IsAlive; }
            set
            {
                if (value)
                    _watchdogWorker.Start();
                else
                {
                    _watchdogWorker.Stop();
                    RecreateNavigationData();
                }
            }
        }

        public ARDroneConfig Config
        {
            get { return _config; }
        }

        public NavigationData NavigationData
        {
            get { return _navigationData; }
        }

        private void SendInitialConfiguration()
        {
            _commandQueue.Enqueue(new ConfigCommand("general:navdata_demo", false));
            _commandQueue.Enqueue(new ControlCommand(ControlMode.NoControlMode));
            _request = Request.Configuration;
        }

        private void RecreateNavigationData()
        {
            _navigationData = new NavigationData();
        }

        private void OnConnectionChanged(bool connected)
        {
            if (connected == false)
            {
                RecreateNavigationData();
            }
        }

        private void OnNavigationPacketAcquired(NavigationPacket packet)
        {
            if (_navigationData.State == NavigationState.Unknown)
                SendInitialConfiguration();

            if (NavigationPacketAcquired != null)
                NavigationPacketAcquired(packet);

            NativeNavdata nativeNavdata;
            if (NativeNavdataParser.TryParse(ref packet, out nativeNavdata))
            {
                NavigationData navigationData = nativeNavdata.ToNavigationData();
                OnNavigationDataDecoded(navigationData);
            }
        }

        private void OnNavigationDataDecoded(NavigationData navigationData)
        {
            _navigationData = navigationData;

            ProcessRequestedState();

            if (NavigationDataDecoded != null)
                NavigationDataDecoded(navigationData);
        }

        private void OnVideoPacketAcquired(VideoPacket packet)
        {
            if (VideoPacketAcquired != null)
                VideoPacketAcquired(packet);
        }

        private void OnConfigurationPacketAcquired(ConfigurationPacket packet)
        {
            string config = Encoding.ASCII.GetString(packet.Data);
            Trace.TraceInformation(config);
        }

        private void ProcessRequestedState()
        {
            switch (_request)
            {
                case Request.None:
                    return;
                case Request.Emergency:
                    if (_navigationData.State.HasFlag(NavigationState.Flying))
                        _commandQueue.Enqueue(new RefCommand(RefMode.Emergency));
                    else
                        _request = Request.None;
                    break;
                case Request.Land:
                    if (_navigationData.State.HasFlag(NavigationState.Flying) &&
                        _navigationData.State.HasFlag(NavigationState.Landing) == false)
                    {
                        _commandQueue.Enqueue(new RefCommand(RefMode.Land));
                    }
                    else
                        _request = Request.None;
                    break;
                case Request.Fly:
                    if (_navigationData.State.HasFlag(NavigationState.Landed) &&
                        _navigationData.State.HasFlag(NavigationState.Takeoff) == false &&
                        _navigationData.State.HasFlag(NavigationState.Emergency) == false &&
                        _navigationData.Battery.Low == false)
                    {
                        _commandQueue.Enqueue(new RefCommand(RefMode.Takeoff));
                    }
                    else
                        _request = Request.None;
                    break;
                case Request.Configuration:
                    _configAcquisitionWorker.Start();
                    if (_navigationData.State.HasFlag(NavigationState.Command))
                    {
                        _commandQueue.Enqueue(new ControlCommand(ControlMode.AckControlMode));
                        break;
                    }
                    
                    _commandQueue.Enqueue(new ControlCommand(ControlMode.CfgGetControlMode));
                    _request = Request.None;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Emergency()
        {
            _request = Request.Emergency;
        }

        public void ResetEmergency()
        {
            _request = Request.None;
            _commandQueue.Enqueue(new RefCommand(RefMode.Emergency));
        }

        public void RequestConfiguration()
        {
            _request = Request.Configuration;
        }

        public void Land()
        {
            _request = Request.Land;
        }

        public void Takeoff()
        {
            if (_navigationData.State.HasFlag(NavigationState.Landed))
                _request = Request.Fly;
        }

        public void FlatTrim()
        {
            if (_navigationData.State.HasFlag(NavigationState.Landed))
                _commandQueue.Enqueue(new FlatTrimCommand());
        }

        public void Hover()
        {
            if (_navigationData.State.HasFlag(NavigationState.Flying))
                _commandQueue.Enqueue(new ProgressiveCommand(ProgressiveMode.Progressive, 0, 0, 0, 0));
        }

        public void Progress(ProgressiveMode mode = ProgressiveMode.Progressive, float roll = 0, float pitch = 0, float yaw = 0, float gaz = 0)
        {
            if (_navigationData.State.HasFlag(NavigationState.Flying))
                _commandQueue.Enqueue(new ProgressiveCommand(mode, roll, pitch, yaw, gaz));
        }

        public void SetVideoChannel(VideoChannel channel)
        {
            _commandQueue.Enqueue(new ConfigCommand("video:video_channel", channel));
        }

        protected override void DisposeOverride()
        {
            _configAcquisitionWorker.Dispose();
            _networkWorker.Dispose();
            _navdataAcquisitionWorker.Dispose();
            _commandQueueWorker.Dispose();
            _videoAcquisitionWorker.Dispose();
            _watchdogWorker.Dispose();
        }

        internal enum Request
        {
            None,
            Land,
            Fly,
            Emergency,
            Configuration
        }
    }
}