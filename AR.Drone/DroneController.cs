using System;
using System.Threading;
using AR.Drone.Api.Commands;
using AR.Drone.NativeApi;
using AR.Drone.Workers;
using AR.Drone.Api.Navdata;
using AR.Drone.Api.Video;
using AR.Drone.Common;

namespace AR.Drone
{
    public class DroneController : WorkerBase
    {
        private readonly CommandQueue _commandQueue;
        private readonly DroneConfig _config;
        private readonly NavdataAcquisition _navdataAcquisition;
        private readonly VideoAcquisition _videoAcquisition;
        private readonly WirelessWorker _wirelessWorker;
        private NavdataInfo _navInfo;
        private DroneControllerState _state;

        public DroneController()
        {
            _config = new DroneConfig();
            _state = DroneControllerState.None;

            _wirelessWorker = new WirelessWorker(Config);
            _navdataAcquisition = new NavdataAcquisition(Config, OnNavInfoUpdated);
            _commandQueue = new CommandQueue(Config);
            _videoAcquisition = new VideoAcquisition(Config, OnVideoFrameAcquired);
        }


        public DroneConfig Config
        {
            get { return _config; }
        }

        public DroneControllerState State
        {
            get { return _state; }
        }

        public NavdataInfo NavInfo
        {
            get { return _navInfo; }
        }

        public event Action<DroneController, VideoPacket> VideoFrameAcquired;

        public void Emergency()
        {
            _commandQueue.Flush();
            _state |= DroneControllerState.Emergency;
            _state &= ~DroneControllerState.Landing;
            _state &= ~DroneControllerState.Takeoff;
            _commandQueue.Enqueue(new RefCommand(RefMode.Emergency));
        }

        public void Land()
        {
            _state |= DroneControllerState.Landing;
            _state &= ~DroneControllerState.Takeoff;
            _state &= ~DroneControllerState.Emergency;
            _commandQueue.Enqueue(new RefCommand(RefMode.Land));
        }

        public void Takeoff()
        {
            _state |= DroneControllerState.Takeoff;
            _state &= ~DroneControllerState.Landing;
            _state &= ~DroneControllerState.Emergency;
            _commandQueue.Enqueue(new RefCommand(RefMode.Takeoff));
        }

        public void FlatTrim()
        {
            if (_state.HasFlag(DroneControllerState.Flying) == false)
                _commandQueue.Enqueue(new FlatTrimCommand());
        }


        public void Hover()
        {
            Move();
        }

        public void Move(ProgressiveMode mode = ProgressiveMode.Progressive, float roll = 0, float pitch = 0, float yaw = 0, float gaz = 0)
        {
            _commandQueue.Enqueue(new ProgressiveCommand(mode, roll, pitch, yaw, gaz));
        }

        public void SwitchVideoChanell(VideoChannel channel)
        {
            _commandQueue.Enqueue(new ConfigCommand("video:video_channel", channel));
        }

        protected override void Loop(CancellationToken token)
        {
            _wirelessWorker.Start();

            while (token.IsCancellationRequested == false)
            {
                if (_wirelessWorker.IsAlive == false)
                {
                    _wirelessWorker.Start();
                }
                else if (_wirelessWorker.IsConnected)
                {
                    if (_navdataAcquisition.IsAlive == false ||
                        _commandQueue.IsAlive == false)
                    {
                        if (_commandQueue.IsAlive) _commandQueue.Stop();
                        if (_navdataAcquisition.IsAlive) _navdataAcquisition.Stop();

                        _commandQueue.Start();
                        _navdataAcquisition.Start();
                    }
                    if (_videoAcquisition.IsAlive == false)
                    {
                        _videoAcquisition.Start();
                    }
                }
                Thread.Sleep(100);
            }
            _wirelessWorker.Stop();
            _navdataAcquisition.Stop();
            _commandQueue.Stop();
            _videoAcquisition.Stop();
        }

        private void OnVideoFrameAcquired(VideoPacket packet)
        {
            if (VideoFrameAcquired != null)
                VideoFrameAcquired(this, packet);
        }

        private void OnNavInfoUpdated(NavdataInfo info)
        {
            _navInfo = info;
            ARDroneState droneState = _navInfo.State;

            UpdateState(droneState);
            ProcessRequestedState();
        }

        private void UpdateState(ARDroneState droneState)
        {
            // process drone flying state
            if (droneState.HasFlag(ARDroneState.Flying))
            {
                _state |= DroneControllerState.Flying;
                _state &= ~DroneControllerState.Landed;
            }
            else
            {
                if (_state == DroneControllerState.None)
                {
                    // if not flying then send fisrt time initialization and configuration
                    _commandQueue.Enqueue(new ConfigCommand("general:navdata_demo", false));
                    _commandQueue.Enqueue(new ConfigCommand("control:altitude_max", 2000));
                    _commandQueue.Enqueue(new ControlCommand(ControlMode.NoControlMode));
                }
                _state |= DroneControllerState.Landed;
                _state &= ~DroneControllerState.Flying;
            }
        }

        private void ProcessRequestedState()
        {
            // process controller state
            if (_state.HasFlag(DroneControllerState.Emergency)) // emergency requested
            {
                // send emergency till drone not in emergency state
                if (_state.HasFlag(DroneControllerState.Flying))
                {
                    _commandQueue.Enqueue(new RefCommand(RefMode.Emergency));
                }
            }
            else if (_state.HasFlag(DroneControllerState.Landing)) // landing requested
            {
                // land till flying
                if (_state.HasFlag(DroneControllerState.Flying))
                    _commandQueue.Enqueue(new RefCommand(RefMode.Land));
                else
                    _state &= ~DroneControllerState.Landing;
            }
            else if (_state.HasFlag(DroneControllerState.Takeoff)) // takeoff requested
            {
                // takeoff till landed
                if (_state.HasFlag(DroneControllerState.Landed))
                    _commandQueue.Enqueue(new RefCommand(RefMode.Takeoff));
                else
                    _state &= ~DroneControllerState.Takeoff;
            }
        }
    }
}