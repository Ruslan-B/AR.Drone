using System;
using System.Collections.Concurrent;
using AR.Drone.Client.Acquisition;
using AR.Drone.Data;
using AR.Drone.Data.Navigation;
using AR.Drone.Infrastructure;
using AR.Drone.Client.Commands;
using AR.Drone.Client.Configuration;

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
        private StateRequest _stateRequest;

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

        public void Send(ATCommand command)
        {
            _commandQueue.Enqueue(command);
        }

        private void OnNavdataAcquisitionStopped()
        {
            _initializationRequested = false;
            _videoAcquisition.Stop();
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


                ProcessTransition();

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

        private void ProcessTransition()
        {
            if (_initializationRequested == false)
            {
                _initializationRequested = true;
                _stateRequest = StateRequest.Initialization;
            }

            switch (_stateRequest)
            {
                case StateRequest.None:
                    return;
                case StateRequest.Initialization:
                    _commandQueue.Flush();
                    ATCommand cmdNavdataDemo = _configuration.General.NavdataDemo.Set(false).ToCommand();
                    Send(cmdNavdataDemo);
                    Send(new ControlCommand(ControlMode.NoControlMode));
                    _stateRequest = StateRequest.Configuration;
                    return;
                case StateRequest.Configuration:
                    _configurationAcquisition.Start();
                    if (_navigationData.State.HasFlag(NavigationState.Command))
                    {
                        Send(new ControlCommand(ControlMode.AckControlMode));
                    }
                    else
                    {
                        Send(new ControlCommand(ControlMode.CfgGetControlMode));
                        _stateRequest = StateRequest.None;
                    }
                    break;
                case StateRequest.Emergency:
                    if (_navigationData.State.HasFlag(NavigationState.Flying))
                        Send(new RefCommand(RefMode.Emergency));
                    else
                        _stateRequest = StateRequest.None;
                    break;
                case StateRequest.ResetEmergency:
                    Send(new RefCommand(RefMode.Emergency));
                    _stateRequest = StateRequest.None;
                    break;
                case StateRequest.Land:
                    if (_navigationData.State.HasFlag(NavigationState.Flying) &&
                        _navigationData.State.HasFlag(NavigationState.Landing) == false)
                    {
                        Send(new RefCommand(RefMode.Land));
                    }
                    else
                        _stateRequest = StateRequest.None;
                    break;
                case StateRequest.Fly:
                    if (_navigationData.State.HasFlag(NavigationState.Landed) &&
                        _navigationData.State.HasFlag(NavigationState.Takeoff) == false &&
                        _navigationData.State.HasFlag(NavigationState.Emergency) == false &&
                        _navigationData.Battery.Low == false)
                    {
                        Send(new RefCommand(RefMode.Takeoff));
                    }
                    else
                        _stateRequest = StateRequest.None;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Emergency()
        {
            _stateRequest = StateRequest.Emergency;
        }

        public void ResetEmergency()
        {
            _stateRequest = StateRequest.ResetEmergency;
        }

        public void RequestConfiguration()
        {
            _stateRequest = StateRequest.Configuration;
        }

        public void Land()
        {
            _stateRequest = StateRequest.Land;
        }

        public void Takeoff()
        {
            if (_navigationData.State.HasFlag(NavigationState.Landed))
                _stateRequest = StateRequest.Fly;
        }

        public void FlatTrim()
        {
            if (_navigationData.State.HasFlag(NavigationState.Landed))
                Send(new FlatTrimCommand());
        }

        public void Hover()
        {
            if (_navigationData.State.HasFlag(NavigationState.Flying))
                Send(new ProgressiveCommand(ProgressiveMode.Progressive, 0, 0, 0, 0));
        }

        public void Progress(ProgressiveMode mode, float roll = 0, float pitch = 0, float yaw = 0, float gaz = 0)
        {
            if (_navigationData.State.HasFlag(NavigationState.Flying))
                Send(new ProgressiveCommand(mode, roll, pitch, yaw, gaz));
        }

        protected override void DisposeOverride()
        {
            _configurationAcquisition.Dispose();
            _navdataAcquisition.Dispose();
            _commandSender.Dispose();
            _videoAcquisition.Dispose();
            _watchdog.Dispose();
        }
    }
}