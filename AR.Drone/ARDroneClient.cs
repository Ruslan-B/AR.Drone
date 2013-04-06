using System;
using System.Collections.Concurrent;
using AR.Drone.Command;
using AR.Drone.Common;
using AR.Drone.Navigation;
using AR.Drone.Video;
using AR.Drone.Workers;

namespace AR.Drone
{
    public class ARDroneClient : DisposableBase
    {
        private readonly ConcurrentQueue<ATCommand> _commandQueue;

        private readonly CommandQueueWorker _commandQueueWorker;
        private readonly ARDroneConfig _config;
        private readonly NavdataAcquisitionWorker _navdataAcquisitionWorker;
        private readonly NetworkWorker _networkWorker;
        private readonly RecoderWorker _recorderWorker;
        private readonly VideoAcquisitionWorker _videoAcquisitionWorker;
        private readonly VideoDecoderWorker _videoDecoderWorker;
        private readonly Watchdog _watchdog;

        private NativeNavdata _nativeNavdata;
        private NavigationData _navigationData;
        private RequestedState _requestedState;

        public ARDroneClient()
        {
            _config = new ARDroneConfig();
            _commandQueue = new ConcurrentQueue<ATCommand>();

            _networkWorker = new NetworkWorker(Config, OnConnectionChanged);
            _navdataAcquisitionWorker = new NavdataAcquisitionWorker(Config, OnNavigationPacketAcquired);
            _commandQueueWorker = new CommandQueueWorker(Config, _commandQueue);
            _videoAcquisitionWorker = new VideoAcquisitionWorker(Config, OnVideoPacketAcquired);
            _videoDecoderWorker = new VideoDecoderWorker(OnFrameDecoded);
            _recorderWorker = new RecoderWorker();
            _watchdog = new Watchdog(_networkWorker, _navdataAcquisitionWorker, _commandQueueWorker, _videoAcquisitionWorker, _videoDecoderWorker, _recorderWorker);
        }

        public Action<ARDroneClient, NavigationPacket> NavigationPacketAcquired { get; set; }
        public Action<ARDroneClient, NavigationData> NavigationDataDecoded { get; set; }
        public Action<ARDroneClient, VideoPacket> VideoPacketAcquired { get; set; }
        public Action<ARDroneClient, VideoFrame> VideoFrameDecoded { get; set; }

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
                    ResetNavigationState();
                }
            }
        }

        public ARDroneConfig Config
        {
            get { return _config; }
        }

        // todo remove this property and field
        public NativeNavdata NativeNavdata
        {
            get { return _nativeNavdata; }
        }

        public NavigationData NavigationData
        {
            get { return _navigationData; }
        }

        private void SendConfiguration()
        {
            _commandQueue.Enqueue(new ConfigCommand("general:navdata_demo", false));
            _commandQueue.Enqueue(new ConfigCommand("control:altitude_max", 2000));
            _commandQueue.Enqueue(new ControlCommand(ControlMode.NoControlMode));
        }

        private void ResetNavigationState()
        {
            _nativeNavdata = new NativeNavdata();
            _navigationData = new NavigationData();
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
            if (_navigationData.State.HasFlag(DroneState.Landed))
                _requestedState = RequestedState.Flying;
        }

        public void FlatTrim()
        {
            if (_navigationData.State.HasFlag(DroneState.Landed))
                _commandQueue.Enqueue(new FlatTrimCommand());
        }

        public void Hover()
        {
            if (_navigationData.State.HasFlag(DroneState.Flying))
                _commandQueue.Enqueue(new ProgressiveCommand(ProgressiveMode.Progressive, 0, 0, 0, 0));
        }

        public void Progress(ProgressiveMode mode = ProgressiveMode.Progressive, float roll = 0, float pitch = 0, float yaw = 0, float gaz = 0)
        {
            if (_navigationData.State.HasFlag(DroneState.Flying))
                _commandQueue.Enqueue(new ProgressiveCommand(mode, roll, pitch, yaw, gaz));
        }

        public void SetVideoChanell(VideoChannel channel)
        {
            _commandQueue.Enqueue(new ConfigCommand("video:video_channel", channel));
        }

        private void OnNavigationPacketAcquired(NavigationPacket packet)
        {
            if (_navigationData.State == DroneState.Unknown)
                SendConfiguration();

            if (_recorderWorker.IsAlive)
                _recorderWorker.EnqueuePacket(packet);

            if (NavigationPacketAcquired != null)
                NavigationPacketAcquired(this, packet);

            NativeNavdata nativeNavdata;
            if (NativeNavdataParser.TryParse(ref packet, out nativeNavdata))
            {
                _nativeNavdata = nativeNavdata;

                NavigationData navigationData = nativeNavdata.ToNavigationData();
                OnNavigationDataDecoded(navigationData);
            }
        }

        private void OnNavigationDataDecoded(NavigationData navigationData)
        {
            _navigationData = navigationData;

            ProcessRequestedState();

            if (NavigationDataDecoded != null)
                NavigationDataDecoded(this, navigationData);
        }

        private void OnVideoPacketAcquired(VideoPacket packet)
        {
            if (_recorderWorker.IsAlive)
                _recorderWorker.EnqueuePacket(packet);

            if (_videoDecoderWorker.IsAlive)
                _videoDecoderWorker.EnqueuePacket(packet);

            if (VideoPacketAcquired != null)
                VideoPacketAcquired(this, packet);
        }

        private void OnFrameDecoded(VideoFrame frame)
        {
            if (VideoFrameDecoded != null)
                VideoFrameDecoded(this, frame);
        }

        private void OnConnectionChanged(bool connected)
        {
            if (connected == false)
            {
                ResetNavigationState();
            }
        }

        private void ProcessRequestedState()
        {
            switch (_requestedState)
            {
                case RequestedState.None:
                    return;
                case RequestedState.Emergency:
                    if (_navigationData.State.HasFlag(DroneState.Flying))
                        _commandQueue.Enqueue(new RefCommand(RefMode.Emergency));
                    else
                        _requestedState = RequestedState.None;
                    break;
                case RequestedState.Landed:
                    if (_navigationData.State.HasFlag(DroneState.Flying) &&
                        _navigationData.State.HasFlag(DroneState.Landing) == false)
                    {
                        _commandQueue.Enqueue(new RefCommand(RefMode.Land));
                    }
                    else
                        _requestedState = RequestedState.None;
                    break;
                case RequestedState.Flying:
                    if (_navigationData.State.HasFlag(DroneState.Landed) &&
                        _navigationData.State.HasFlag(DroneState.Takeoff) == false &&
                        _navigationData.State.HasFlag(DroneState.Emergency) == false &&
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

        protected override void DisposeOverride()
        {
            _networkWorker.Dispose();
            _navdataAcquisitionWorker.Dispose();
            _commandQueueWorker.Dispose();
            _videoAcquisitionWorker.Dispose();
            _videoDecoderWorker.Dispose();
            _recorderWorker.Dispose();
            _watchdog.Dispose();
        }
    }
}