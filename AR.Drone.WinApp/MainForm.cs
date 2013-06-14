using System;
using System.Drawing;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;
using AR.Drone.Client;
using AR.Drone.Client.Commands;
using AR.Drone.Client.Configuration;
using AR.Drone.Client.Configuration.Sections;
using AR.Drone.Data;
using AR.Drone.Data.Navigation.Native;
using AR.Drone.Media;
using AR.Drone.Video;

namespace AR.Drone.WinApp
{
    public partial class MainForm : Form
    {
        private readonly DroneClient _droneClient;
        private readonly PacketRecorder _packetRecorderWorker;
        private readonly VideoPacketDecoderWorker _videoPacketDecoderWorker;
        private uint _frameNumber;
        private VideoFrame _frame;
        private Bitmap _frameBitmap;
        private NavigationPacket _navigationPacket;

        public MainForm()
        {
            InitializeComponent();

            unsafe
            {
                bool x64 = sizeof(IntPtr) == 8;
                Text += x64 ? " [x64]" : " [x86]";
            }

            _videoPacketDecoderWorker = new VideoPacketDecoderWorker(PixelFormat.BGR24, true, OnVideoPacketDecoded);
            _videoPacketDecoderWorker.Start();

            string path = string.Format("flight_{0:yyyy-MM-dd-HH-mm}.ardrone", DateTime.Now);
            var stream = new FileStream(path, FileMode.OpenOrCreate);
            _packetRecorderWorker = new PacketRecorder(stream);
            _packetRecorderWorker.Start();

            _droneClient = new DroneClient();
            _droneClient.NavigationPacketAcquired += OnNavigationPacketAcquired;
            _droneClient.VideoPacketAcquired += OnVideoPacketAcquired;
            _droneClient.ConfigurationUpdated += OnConfigurationUpdated;
            _droneClient.Active = true;

            tmrStateUpdate.Enabled = true;
            tmrVideoUpdate.Enabled = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            _droneClient.Dispose();
            _videoPacketDecoderWorker.Dispose();
            _packetRecorderWorker.Dispose();

            base.OnClosed(e);
        }

        private void OnNavigationPacketAcquired(NavigationPacket packet)
        {
            //if (_packetRecorderWorker.IsAlive)
            //    _packetRecorderWorker.EnqueuePacket(packet);

            _navigationPacket = packet;
        }

        private void OnVideoPacketAcquired(VideoPacket packet)
        {
            if (_packetRecorderWorker.IsAlive)
                _packetRecorderWorker.EnqueuePacket(packet);
            if (_videoPacketDecoderWorker.IsAlive)
                _videoPacketDecoderWorker.EnqueuePacket(packet);
        }

        private void OnVideoPacketDecoded(VideoFrame frame)
        {
            _frame = frame;
        }

        private void OnConfigurationUpdated(DroneConfiguration configuration)
        {
            if (configuration.Video.Codec != VideoCodecType.H264_360P ||
                configuration.Video.BitrateCtrlMode != VideoBitrateControlMode.Dynamic)
            {
                _droneClient.Send(configuration.Video.Codec.Set(VideoCodecType.H264_360P).ToCommand());
                _droneClient.Send(configuration.Video.BitrateCtrlMode.Set(VideoBitrateControlMode.Dynamic).ToCommand());
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _droneClient.Active = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _droneClient.Active = false;
        }

        private void tmrVideoUpdate_Tick(object sender, EventArgs e)
        {
            if (_frame == null || _frameNumber == _frame.Number)
                return;
            _frameNumber = _frame.Number;
                
            if (_frameBitmap == null)
                _frameBitmap = VideoHelper.CreateBitmap(ref _frame);
            else
                VideoHelper.UpdateBitmap(ref _frameBitmap, ref _frame);

            pbVideo.Image = _frameBitmap;
        }

        private void tmrStateUpdate_Tick(object sender, EventArgs e)
        {
            tvInfo.BeginUpdate();

            TreeNode node = tvInfo.Nodes.GetOrCreate("ClientActive");
            node.Text = string.Format("Client Active: {0}", _droneClient.Active);

            node = tvInfo.Nodes.GetOrCreate("Navigation Data");
            DumpBranch(node.Nodes, _droneClient.NavigationData);

            node = tvInfo.Nodes.GetOrCreate("Configuration");
            DumpBranch(node.Nodes, _droneClient.Configuration);

            TreeNode vativeNode = tvInfo.Nodes.GetOrCreate("Native");

            NavdataBag navdataBag;
            if (_navigationPacket.Data != null && NavdataBagParser.TryParse(ref _navigationPacket, out navdataBag))
            {
                var ctrl_state = (CTRL_STATES)(navdataBag.demo.ctrl_state >> 0x10);
                node = vativeNode.Nodes.GetOrCreate("ctrl_state");
                node.Text = string.Format("Ctrl State: {0}", ctrl_state);

                var flying_state = (FLYING_STATES)(navdataBag.demo.ctrl_state & 0xffff);
                node = vativeNode.Nodes.GetOrCreate("flying_state");
                node.Text = string.Format("Ctrl State: {0}", flying_state);

                DumpBranch(vativeNode.Nodes, navdataBag);
            }
            tvInfo.EndUpdate();
        }

        private void DumpBranch(TreeNodeCollection nodes, object o)
        {
            Type type = o.GetType();
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo fieldInfo in fields)
            {
                TreeNode node = nodes.GetOrCreate(fieldInfo.Name);
                object fieldValue = fieldInfo.GetValue(o);

                if (fieldValue == null)
                    node.Text = node.Name + ": null";
                else if (fieldValue is IConfigurationItem)
                    node.Text = node.Name + ": " + ((IConfigurationItem)fieldValue).Value;
                else
                {
                    Type fieldType = fieldInfo.FieldType;
                    if (fieldType.Namespace.StartsWith("System") || fieldType.IsEnum)
                        node.Text = node.Name + ": " + fieldValue;
                    else
                        DumpBranch(node.Nodes, fieldValue);
                }
            }
        }

        private void btnFlatTrim_Click(object sender, EventArgs e)
        {
            _droneClient.Send(_droneClient.Configuration.Control.flight_anim.Set(FlightAnimation.FlipLeft, 15).ToCommand());
            _droneClient.FlatTrim();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _droneClient.Takeoff();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _droneClient.Land();
        }

        private void btnEmergency_Click(object sender, EventArgs e)
        {
            _droneClient.Emergency();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _droneClient.ResetEmergency();
        }

        private void btnSwitchCam_Click(object sender, EventArgs e)
        {
            DroneConfiguration configuration = _droneClient.Configuration;
            ATCommand command = configuration.Video.Channel.Set(VideoChannelType.Next).ToCommand();
            _droneClient.Send(command);
        }

        private void Test(Expression<Action<DroneConfiguration>> x)
        {
        }

        private void btnHover_Click(object sender, EventArgs e)
        {
            _droneClient.Hover();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            _droneClient.Progress(FlightMode.Progressive, gaz: 0.25f);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            _droneClient.Progress(FlightMode.Progressive, gaz: -0.25f);
        }

        private void btnTurnLeft_Click(object sender, EventArgs e)
        {
            _droneClient.Progress(FlightMode.Progressive, yaw: 0.25f);
        }

        private void btnTurnRight_Click(object sender, EventArgs e)
        {
            _droneClient.Progress(FlightMode.Progressive, yaw: -0.25f);
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            _droneClient.Progress(FlightMode.Progressive, roll: -0.05f);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            _droneClient.Progress(FlightMode.Progressive, roll: 0.05f);
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            _droneClient.Progress(FlightMode.Progressive, pitch: -0.05f);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _droneClient.Progress(FlightMode.Progressive, pitch: 0.05f);
        }

        private void btnReadConfig_Click(object sender, EventArgs e)
        {
            _droneClient.RequestConfiguration();
        }
    }
}