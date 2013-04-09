using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using AR.Drone.Client;
using AR.Drone.Client.Command;
using AR.Drone.Client.NativeApi.Navdata;
using AR.Drone.Client.Navigation;
using AR.Drone.Client.Video;
using AR.Drone.Client.Workers;

namespace AR.Drone.WinApp
{
    public partial class MainForm : Form
    {
        private readonly ARDroneClient _arDroneClient;
        private readonly PacketRecorderWorker _packetRecorderWorker;
        private readonly VideoDecoderWorker _videoDecoderWorker;

        private Image _frameImage;
        private NativeNavdata _nativeNavdata;

        public MainForm()
        {
            InitializeComponent();

            _videoDecoderWorker = new VideoDecoderWorker(OnVideoDecoderOnFrameDecoded);
            _videoDecoderWorker.Start();

            string path = string.Format("ardrone_{0:yyyy-MM-dd-HH-mm}.pack", DateTime.Now);
            _packetRecorderWorker = new PacketRecorderWorker(path);
            _packetRecorderWorker.Start();

            _arDroneClient = new ARDroneClient();
            _arDroneClient.NavigationPacketAcquired += OnNavigationPacketAcquired;
            _arDroneClient.VideoPacketAcquired += OnVideoPacketAcquired;
            _arDroneClient.Active = true;

            tmrStateUpdate.Enabled = true;
            tmrVideoUpdate.Enabled = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            _arDroneClient.Dispose();
            _videoDecoderWorker.Dispose();
            _packetRecorderWorker.Dispose();

            base.OnClosed(e);
        }

        private void OnNavigationPacketAcquired(NavigationPacket packet)
        {
            if (_packetRecorderWorker.IsAlive) _packetRecorderWorker.EnqueuePacket(packet);
            
            UpdateNativeNavdata(packet);
        }

        private void OnVideoPacketAcquired(VideoPacket packet)
        {
            if (_packetRecorderWorker.IsAlive) _packetRecorderWorker.EnqueuePacket(packet);

            _videoDecoderWorker.EnqueuePacket(packet);
        }

        private void UpdateNativeNavdata(NavigationPacket packet)
        {
            NativeNavdata nativeNavdata;
            if (NativeNavdataParser.TryParse(ref packet, out nativeNavdata))
            {
                _nativeNavdata = nativeNavdata;
            }
        }

        private void OnVideoDecoderOnFrameDecoded(VideoFrame frame)
        {
            _frameImage = ARDroneVideoHelper.CreateImageFromFrame(frame);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _arDroneClient.Active = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _arDroneClient.Active = false;
        }

        private void tmrVideoUpdate_Tick(object sender, EventArgs e)
        {
            Image oldImage = pbVideo.Image;
            if (oldImage == _frameImage) return;
            pbVideo.Image = _frameImage;
            if (oldImage != null) oldImage.Dispose();
        }

        private void tmrStateUpdate_Tick(object sender, EventArgs e)
        {
            tvInfo.BeginUpdate();

            TreeNode node = tvInfo.Nodes.GetOrCreate("ClientActive");
            node.Text = string.Format("Client Active: {0}", _arDroneClient.Active);

            node = tvInfo.Nodes.GetOrCreate("Navigation Data");
            DumpBranch(node.Nodes, _arDroneClient.NavigationData);

            TreeNode vativeNode = tvInfo.Nodes.GetOrCreate("Native");

            var ctrl_state = (CTRL_STATES) (_nativeNavdata.demo.ctrl_state >> 0x10);
            node = vativeNode.Nodes.GetOrCreate("ctrl_state");
            node.Text = string.Format("Ctrl State: {0}", ctrl_state);

            var flying_state = (FLYING_STATES) (_nativeNavdata.demo.ctrl_state & 0xffff);
            node = vativeNode.Nodes.GetOrCreate("flying_state");
            node.Text = string.Format("Ctrl State: {0}", flying_state);

            DumpBranch(vativeNode.Nodes, _nativeNavdata);

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
            _arDroneClient.FlatTrim();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _arDroneClient.Takeoff();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _arDroneClient.Land();
        }

        private void btnEmergency_Click(object sender, EventArgs e)
        {
            _arDroneClient.Emergency();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _arDroneClient.ResetEmergency();
        }

        private void btnSwitchCam_Click(object sender, EventArgs e)
        {
            _arDroneClient.SetVideoChannel(VideoChannel.Next);
        }

        private void btnHover_Click(object sender, EventArgs e)
        {
            _arDroneClient.Hover();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            _arDroneClient.Progress(gaz: 0.25f);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            _arDroneClient.Progress(gaz: -0.25f);
        }

        private void btnTurnLeft_Click(object sender, EventArgs e)
        {
            _arDroneClient.Progress(yaw: 0.25f);
        }

        private void btnTurnRight_Click(object sender, EventArgs e)
        {
            _arDroneClient.Progress(yaw: -0.25f);
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            _arDroneClient.Progress(ProgressiveMode.CombinedYaw, roll: 0.05f);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            _arDroneClient.Progress(ProgressiveMode.CombinedYaw, roll: -0.05f);
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            _arDroneClient.Progress(ProgressiveMode.CombinedYaw, pitch: -0.05f);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _arDroneClient.Progress(ProgressiveMode.CombinedYaw, pitch: 0.05f);
        }
    }
}