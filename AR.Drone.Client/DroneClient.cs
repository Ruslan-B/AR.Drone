using System;
using System.Collections.Concurrent;
using AI.Core.System;
using AR.Drone.Client.Acquisition.Configuration;
using AR.Drone.Client.Acquisition.Navigation;
using AR.Drone.Client.Acquisition.Video;
using AR.Drone.Client.Commands;
using AR.Drone.Client.Configuration;
using AR.Drone.Data;

namespace AR.Drone.Client
{
    public class DroneClient : DisposableBase
    {
        private readonly ConcurrentQueue<ATCommand> _commandQueue;
        private readonly CommandSender _commandSender;
        private readonly DroneConfiguration _configuration;
        private readonly ConfigurationAcquisition _configurationAcquisition;
        private readonly NavdataAcquisition _navdataAcquisition;
        private readonly VideoAcquisition _videoAcquisition;
        private readonly Watchdog _watchdog;
        private bool _initializationRequested;
        private NavigationData _navigationData;
        private RequestedState _requestedState;

        public DroneClient()
        {
            _configuration = new DroneConfiguration();
            _commandQueue = new ConcurrentQueue<ATCommand>();

            _commandSender = new CommandSender(_configuration, _commandQueue);
            _navdataAcquisition = new NavdataAcquisition(_configuration, OnNavdataPacketAcquired, OnNavdataAcquisitionStopped);
            _videoAcquisition = new VideoAcquisition(_configuration, OnVideoPacketAcquired);
            _configurationAcquisition = new ConfigurationAcquisition(_configuration, OnConfigurationPacketAcquired);
            _watchdog = new Watchdog(_navdataAcquisition, _commandSender, _videoAcquisition);
        }

        public Action<NavigationPacket> NavigationPacketAcquired { get; set; }

        public Action<NavigationData> NavigationDataUpdated { get; set; }

        public Action<VideoPacket> VideoPacketAcquired { get; set; }

        public Action<ConfigurationPacket> ConfigurationPacketAcquired { get; set; }

        public Action<DroneConfiguration> ConfigurationUpdated { get; set; }

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
                }
            }
        }

        public bool IsConnected
        {
            get { return _navdataAcquisition.IsAcquiring; }
        }

        public DroneConfiguration Configuration
        {
            get { return _configuration; }
        }

        public NavigationData NavigationData
        {
            get { return _navigationData; }
        }

        private void OnNavdataAcquisitionStopped()
        {
            _initializationRequested = false;
        }

        private void OnNavdataPacketAcquired(NavigationPacket packet)
        {
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

                if (_initializationRequested == false)
                {
                    _requestedState = RequestedState.Initialize;
                    _initializationRequested = true;
                }

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
                    _commandQueue.Flush();
                    _commandQueue.Enqueue(cmdNavdataDemo);
                    _commandQueue.Enqueue(new ControlCommand(ControlMode.NoControlMode));
                    _requestedState = RequestedState.GetConfiguration;
                    return;
                case RequestedState.GetConfiguration:
                    _configurationAcquisition.Start();
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
            _configurationAcquisition.Dispose();
            _navdataAcquisition.Dispose();
            _commandSender.Dispose();
            _videoAcquisition.Dispose();
            _watchdog.Dispose();
        }

        internal enum RequestedState
        {
            None
,
            Initialize
,
            GetConfiguration
,
            Land
,
            Fly
,
            Emergency
,
            ResetEmergency
        }
    }
}