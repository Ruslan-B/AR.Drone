using System;
using AR.Drone.Command;
using AR.Drone.NativeApi;
using AR.Drone.Navigation;
using AR.Drone.Video;
using AR.Drone.Workers;

namespace AR.Drone
{
    public class DroneController
    {
        private readonly CommandQueueWorker _commandQueueWorker;
        private readonly DroneConfig _config;
        private readonly NavdataAcquisitionWorker _navdataAcquisitionWorker;
        private readonly NavdataParser _navdataParser;
        private readonly NetworkWorker _networkWorker;
        private readonly RecoderWorker _recorderWorker;
        private readonly VideoAcquisitionWorker _videoAcquisitionWorker;
        private readonly VideoDecoderWorker _videoDecoderWorker;
        private readonly Watchdog _watchdog;

        private DroneState _droneState;
        private NavigationData _navigationData;
        private ProgressiveCommand _progressiveCommand;
        private RequestedState _requestedState;

        public DroneController()
        {
            _config = new DroneConfig();
            _droneState = DroneState.Unknown;

            _networkWorker = new NetworkWorker(Config, OnConnectionChanged);
            _navdataAcquisitionWorker = new NavdataAcquisitionWorker(Config, OnNavdataAcquired);
            _commandQueueWorker = new CommandQueueWorker(Config);
            _videoAcquisitionWorker = new VideoAcquisitionWorker(Config, OnFrameAcquired);
            _videoDecoderWorker = new VideoDecoderWorker(OnFrameDecoded);
            _recorderWorker = new RecoderWorker();
            _watchdog = new Watchdog(_networkWorker, _navdataAcquisitionWorker, _commandQueueWorker, _videoAcquisitionWorker, _videoDecoderWorker, _recorderWorker);
            _navdataParser = new NavdataParser();
        }


        public bool Active
        {
            get { return _watchdog.IsAlive; }
            set
            {
                if (value)
                    _watchdog.Start();
                else
                    _watchdog.Stop();
            }
        }

        public DroneConfig Config
        {
            get { return _config; }
        }

        public DroneState DroneState
        {
            get { return _droneState; }
        }

        public NavigationData NavigationData
        {
            get { return _navigationData; }
        }

        public event Action<DroneController, VideoPacket> FrameAcquired;
        public event Action<DroneController, VideoFrame> FrameDecoded;
        public event Action<DroneController, NavigationPacket> NavdataAcquired;
        public event Action<DroneController, NavigationData> NavdataParsed;

        public void Emergency()
        {
            _requestedState = RequestedState.Emergency;
        }

        public void ResetEmergency()
        {
          _commandQueueWorker.Enqueue(new RefCommand(RefMode.Emergency));
          _commandQueueWorker.Enqueue(new RefCommand(RefMode.Land));
        }

        public void Land()
        {
            _requestedState = RequestedState.Landed;
        }

        public void Takeoff()
        {
            _requestedState = RequestedState.Flying;
        }

        public void FlatTrim()
        {
            if (_droneState.HasFlag(DroneState.Flying) == false)
                _commandQueueWorker.Enqueue(new FlatTrimCommand());
        }


        public void Hover()
        {
            _requestedState = RequestedState.Hovering;
        }

        public void Progress(ProgressiveMode mode = ProgressiveMode.Progressive, float roll = 0, float pitch = 0, float yaw = 0, float gaz = 0)
        {
            _requestedState = RequestedState.Progressing;
            _progressiveCommand = new ProgressiveCommand(mode, roll, pitch, yaw, gaz);
        }

        public void SwitchVideoChanell(VideoChannel channel)
        {
            _commandQueueWorker.Enqueue(new ConfigCommand("video:video_channel", channel));
        }

        private void OnNavdataAcquired(NavigationPacket packet)
        {
            if (NavdataAcquired != null)
                NavdataAcquired(this, packet);

            if (_recorderWorker.IsAlive)
                _recorderWorker.EnqueuePacket(packet);

            NavigationData navdata;
            if (_navdataParser.TryParse(packet, out navdata))
                OnNavdataParsed(navdata);
        }

        private void OnNavdataParsed(NavigationData navdata)
        {
            _navigationData = navdata;
            UpdateStateUsing();
            ProcessRequestedState();

            if (NavdataParsed != null)
                NavdataParsed(this, navdata);
        }

        private void OnFrameAcquired(VideoPacket packet)
        {
            if (_videoDecoderWorker.IsAlive)
                _videoDecoderWorker.EnqueuePacket(packet);

            if (_recorderWorker.IsAlive)
                _recorderWorker.EnqueuePacket(packet);

            if (FrameAcquired != null)
                FrameAcquired(this, packet);
        }

        private void OnFrameDecoded(VideoFrame frame)
        {
            if (FrameDecoded != null)
                FrameDecoded(this, frame);
        }

        private void OnConnectionChanged(bool connected)
        {
            if (connected == false)
            {
                _droneState = DroneState.Unknown;
            }
        }

        private void SendConfiguration()
        {
            _commandQueueWorker.Enqueue(new ConfigCommand("general:navdata_demo", false));
            _commandQueueWorker.Enqueue(new ConfigCommand("control:altitude_max", 2000));
            _commandQueueWorker.Enqueue(new ControlCommand(ControlMode.NoControlMode));
        }

        private void UpdateStateUsing()
        {
            if (_droneState == DroneState.Unknown)
                SendConfiguration();
            
            // major states
            def_ardrone_state_mask_t ardrone_state = _navigationData.ardrone_state;

            if (ardrone_state.HasFlag(def_ardrone_state_mask_t.ARDRONE_FLY_MASK))
            {
                _droneState |= DroneState.Flying;
                _droneState &= ~DroneState.Landed;
            }
            else
            {
                _droneState &= ~DroneState.Flying;
                _droneState |= DroneState.Landed;
            }

            if (ardrone_state.HasFlag(def_ardrone_state_mask_t.ARDRONE_VBAT_LOW))
            {
                _droneState |= DroneState.BatteryLow;
            }
            else
            {
                _droneState &= ~DroneState.BatteryLow;
            }

            if (ardrone_state.HasFlag(def_ardrone_state_mask_t.ARDRONE_EMERGENCY_MASK))
            {
                _droneState |= DroneState.Emergency;
            }
            else
            {
                _droneState &= ~DroneState.Emergency;
            }

            // process control state
            var ctrl_state = (CTRL_STATES)_navigationData.demo.ctrl_state;

            if (ctrl_state.HasFlag(CTRL_STATES.CTRL_TRANS_TAKEOFF))
            {
                _droneState |= DroneState.Takeoff;
            }
            else
            {
                _droneState &= ~DroneState.Takeoff;
            }

            if (ctrl_state.HasFlag(CTRL_STATES.CTRL_TRANS_LANDING))
            {
                _droneState |= DroneState.Landing;
            }
            else
            {
                _droneState &= ~DroneState.Landing;
            }

            if (ctrl_state.HasFlag(CTRL_STATES.CTRL_HOVERING) &&
                ctrl_state.HasFlag(CTRL_STATES.CTRL_TRANS_GOTOFIX) == false)
            {
                _droneState |= DroneState.Hovering;
            }
            else
            {
                _droneState &= ~DroneState.Hovering;
            }

            if (ctrl_state.HasFlag(CTRL_STATES.CTRL_TRANS_GOTOFIX))
            {
                _droneState |= DroneState.Progress;
            }
            else
            {
                _droneState &= ~DroneState.Progress;
            }
        }


        private void ProcessRequestedState()
        {
            switch (_requestedState)
            {
                case RequestedState.None:
                    // do nothing
                    break;
                case RequestedState.Emergency:
                    if (_droneState.HasFlag(DroneState.Flying))
                    {
                        _commandQueueWorker.Enqueue(new RefCommand(RefMode.Emergency));
                        _commandQueueWorker.Enqueue(new RefCommand(RefMode.Land));
                    }
                    else
                    {
                        _requestedState = RequestedState.None;
                    }
                    break;
                case RequestedState.Landed:
                    if (_droneState.HasFlag(DroneState.Flying))
                    {
                        _commandQueueWorker.Enqueue(new RefCommand(RefMode.Land));
                    }
                    else
                    {
                        _requestedState = RequestedState.None;
                    }
                    break;
                case RequestedState.Flying:
                    if (_droneState.HasFlag(DroneState.Landed) &&
                        _droneState.HasFlag(DroneState.Emergency) == false &&
                        _droneState.HasFlag(DroneState.BatteryLow) == false)
                    {
                        _commandQueueWorker.Enqueue(new RefCommand(RefMode.Takeoff));
                    }
                    else
                    {
                        _requestedState = RequestedState.None;
                    }
                    break;
                case RequestedState.Hovering:
                    if (_droneState.HasFlag(DroneState.Flying))
                    {
                        _commandQueueWorker.Enqueue(new ProgressiveCommand(ProgressiveMode.Progressive, 0, 0, 0, 0));
                        _requestedState = RequestedState.None;
                    }
                    else
                    {
                        _requestedState = RequestedState.None;
                    }
                    break;
                case RequestedState.Progressing:
                    if (_droneState.HasFlag(DroneState.Flying))
                    {
                        _commandQueueWorker.Enqueue(_progressiveCommand);
                        _requestedState = RequestedState.None;
                    }
                    else
                    {
                        _requestedState = RequestedState.None;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}