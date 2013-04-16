using System;
using System.Collections.Concurrent;
using AI.Core.System;
using AR.Drone.Client.Commands;
using AR.Drone.Client.Configuration;
using AR.Drone.Client.Navigation;
using AR.Drone.Client.Packets;
using AR.Drone.Client.Workers;
using AR.Drone.Client.Workers.Acquisition;

namespace AR.Drone.Client
{
    public class DroneClient : DisposableBase
    {
        private readonly ConcurrentQueue<ATCommand> _commandQueue;

        private readonly CommandQueueWorker _commandQueueWorker;
        private readonly DroneConfiguration _configuration;
        private readonly ConfigurationAcquisitionWorker _configurationAcquisitionWorker;
        private readonly NavdataAcquisitionWorker _navdataAcquisitionWorker;
        private readonly NetworkWorker _networkWorker;
        private readonly VideoAcquisitionWorker _videoAcquisitionWorker;
        private readonly WatchdogWorker _watchdogWorker;

        private NavigationData _navigationData;
        private RequestedState _requestedState;

        public DroneClient()
        {
            _configuration = new DroneConfiguration();
            _commandQueue = new ConcurrentQueue<ATCommand>();

            _networkWorker = new NetworkWorker(_configuration, OnConnectionChanged);
            _commandQueueWorker = new CommandQueueWorker(_configuration, _commandQueue);
            _navdataAcquisitionWorker = new NavdataAcquisitionWorker(_configuration, OnNavigationPacketAcquired);
            _videoAcquisitionWorker = new VideoAcquisitionWorker(_configuration, OnVideoPacketAcquired);
            _configurationAcquisitionWorker = new ConfigurationAcquisitionWorker(_configuration, OnConfigurationPacketAcquired);
            _watchdogWorker = new WatchdogWorker(_networkWorker, _navdataAcquisitionWorker, _commandQueueWorker, _videoAcquisitionWorker);
        }

        public Action<NavigationPacket> NavigationPacketAcquired { get; set; }
        public Action<NavigationData> NavigationDataUpdated { get; set; }
        public Action<VideoPacket> VideoPacketAcquired { get; set; }
        public Action<ConfigurationPacket> ConfigurationPacketAcquired { get; set; }
        public Action<DroneConfiguration> ConfigurationUpdated { get; set; }

        private void ResetNavigationData()
        {
            _navigationData = new NavigationData();
        }

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
                    ResetNavigationData();
                }
            }
        }

        public DroneConfiguration Configuration
        {
            get { return _configuration; }
        }

        public NavigationData NavigationData
        {
            get { return _navigationData; }
        }

        private void OnConnectionChanged(bool connected)
        {
            if (connected == false)
            {
                ResetNavigationData();
            }
        }

        private void OnNavigationPacketAcquired(NavigationPacket packet)
        {
            if (_navigationData.State == NavigationState.Unknown)
                _requestedState = RequestedState.Initialize;

            if (NavigationPacketAcquired != null)
                NavigationPacketAcquired(packet);

            UpdateNavigationData(packet);
        }

        private void UpdateNavigationData(NavigationPacket packet)
        {
            NavigationData navigationData;
            if (NavigationPacketParser.TryParse(ref packet, out navigationData))
            {
                _navigationData = navigationData;

                ProcessRequestedState();

                if (NavigationDataUpdated != null)
                    NavigationDataUpdated(_navigationData);
            }
        }

        private void OnVideoPacketAcquired(VideoPacket packet)
        {
            if (VideoPacketAcquired != null)
                VideoPacketAcquired(packet);
        }

        private void OnConfigurationPacketAcquired(ConfigurationPacket packet)
        {
            if (ConfigurationPacketAcquired != null)
                ConfigurationPacketAcquired(packet);

            if (ConfigurationPacketParser.TryUpdate(_configuration, packet))
            {
                if (ConfigurationUpdated != null)
                    ConfigurationUpdated(_configuration);
            }
        }

        private void ProcessRequestedState()
        {
            switch (_requestedState)
            {
                case RequestedState.None:
                    return;
                case RequestedState.Initialize:
                    ATCommand cmdNavdataDemo = _configuration.General.NavdataDemo.Set(false).ToCommand();
                    _commandQueue.Enqueue(cmdNavdataDemo);
                    _commandQueue.Enqueue(new ControlCommand(ControlMode.NoControlMode));
                    _requestedState = RequestedState.GetConfiguration;
                    return;
                case RequestedState.GetConfiguration:
                    _configurationAcquisitionWorker.Start();
                    if (_navigationData.State.HasFlag(NavigationState.Command))
                    {
                        _commandQueue.Enqueue(new ControlCommand(ControlMode.AckControlMode));
                    }
                    else
                    {
                        _commandQueue.Enqueue(new ControlCommand(ControlMode.CfgGetControlMode));
                        _requestedState = RequestedState.None;
                    }
                    break;
                case RequestedState.Emergency:
                    if (_navigationData.State.HasFlag(NavigationState.Flying))
                        _commandQueue.Enqueue(new RefCommand(RefMode.Emergency));
                    else
                        _requestedState = RequestedState.None;
                    break;
                case RequestedState.ResetEmergency:
                    _commandQueue.Enqueue(new RefCommand(RefMode.Emergency));
                    _requestedState = RequestedState.None;
                    break;
                case RequestedState.Land:
                    if (_navigationData.State.HasFlag(NavigationState.Flying) &&
                        _navigationData.State.HasFlag(NavigationState.Landing) == false)
                    {
                        _commandQueue.Enqueue(new RefCommand(RefMode.Land));
                    }
                    else
                        _requestedState = RequestedState.None;
                    break;
                case RequestedState.Fly:
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
            _requestedState = RequestedState.ResetEmergency;
        }

        public void RequestConfiguration()
        {
            _requestedState = RequestedState.GetConfiguration;
        }

        public void Land()
        {
            _requestedState = RequestedState.Land;
        }

        public void Takeoff()
        {
            if (_navigationData.State.HasFlag(NavigationState.Landed))
                _requestedState = RequestedState.Fly;
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

        public void Progress(ProgressiveMode mode, float roll = 0, float pitch = 0, float yaw = 0, float gaz = 0)
        {
            if (_navigationData.State.HasFlag(NavigationState.Flying))
                _commandQueue.Enqueue(new ProgressiveCommand(mode, roll, pitch, yaw, gaz));
        }

        public void Send(ATCommand command)
        {
            _commandQueue.Enqueue(command);
        }

        protected override void DisposeOverride()
        {
            _configurationAcquisitionWorker.Dispose();
            _networkWorker.Dispose();
            _navdataAcquisitionWorker.Dispose();
            _commandQueueWorker.Dispose();
            _videoAcquisitionWorker.Dispose();
            _watchdogWorker.Dispose();
        }

        internal enum RequestedState
        {
            None,
            Initialize,
            GetConfiguration,
            Land,
            Fly,
            Emergency,
            ResetEmergency
        }
    }
}