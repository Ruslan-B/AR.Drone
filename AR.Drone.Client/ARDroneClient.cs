using System;
using System.Collections.Concurrent;
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
        private readonly NavdataAcquisitionWorker _navdataAcquisitionWorker;
        private readonly NetworkWorker _networkWorker;
        private readonly VideoAcquisitionWorker _videoAcquisitionWorker;
        private readonly Watchdog _watchdog;

        private NavigationData _navigationData;
        private RequestedState _requestedState;

        public ARDroneClient()
        {
            _config = new ARDroneConfig();
            _commandQueue = new ConcurrentQueue<ATCommand>();

            _networkWorker = new NetworkWorker(_config, OnConnectionChanged);
            _navdataAcquisitionWorker = new NavdataAcquisitionWorker(_config, OnNavigationPacketAcquired);
            _commandQueueWorker = new CommandQueueWorker(_config, _commandQueue);
            _videoAcquisitionWorker = new VideoAcquisitionWorker(_config, OnVideoPacketAcquired);
            _watchdog = new Watchdog(_networkWorker, _navdataAcquisitionWorker, _commandQueueWorker, _videoAcquisitionWorker);
        }

        public Action<NavigationPacket> NavigationPacketAcquired { get; set; }
        public Action<NavigationData> NavigationDataDecoded { get; set; }
        public Action<VideoPacket> VideoPacketAcquired { get; set; }
        
        public bool Active
        {
            get { return _watchdog.IsAlive; }
            set
            {
                if (value)
                    _watchdog.Start();
                else
                {
                    _watchdog.Stop();
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

        private void ProcessRequestedState()
        {
            switch (_requestedState)
            {
                case RequestedState.None:
                    return;
                case RequestedState.Emergency:
                    if (_navigationData.State.HasFlag(NavigationState.Flying))
                        _commandQueue.Enqueue(new RefCommand(RefMode.Emergency));
                    else
                        _requestedState = RequestedState.None;
                    break;
                case RequestedState.Landed:
                    if (_navigationData.State.HasFlag(NavigationState.Flying) &&
                        _navigationData.State.HasFlag(NavigationState.Landing) == false)
                    {
                        _commandQueue.Enqueue(new RefCommand(RefMode.Land));
                    }
                    else
                        _requestedState = RequestedState.None;
                    break;
                case RequestedState.Flying:
                    if (_navigationData.State.HasFlag(NavigationState.Landed) &&
                        _navigationData.State.HasFlag(NavigationState.Takeoff) == false &&
                        _navigationData.State.HasFlag(NavigationState.Emergency) == false &&
                        _navigationData.Battery.Low == false)
                    {
                        _commandQueue.Enqueue(new RefCommand(RefMode.Takeoff));
                    }
                    else
                        _requestedState = RequestedState.None;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Emergency()
        {
            _requestedState = RequestedState.Emergency;
        }

        public void ResetEmergency()
        {
            _requestedState = RequestedState.None;
            _commandQueue.Enqueue(new RefCommand(RefMode.Emergency));
        }

        public void Land()
        {
            _requestedState = RequestedState.Landed;
        }

        public void Takeoff()
        {
            if (_navigationData.State.HasFlag(NavigationState.Landed))
                _requestedState = RequestedState.Flying;
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
            _networkWorker.Dispose();
            _navdataAcquisitionWorker.Dispose();
            _commandQueueWorker.Dispose();
            _videoAcquisitionWorker.Dispose();
            _watchdog.Dispose();
        }
    }
}