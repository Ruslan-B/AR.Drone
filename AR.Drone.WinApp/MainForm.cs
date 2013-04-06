using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using AR.Drone.Command;
using AR.Drone.NativeApi;
using AR.Drone.Navigation;
using AR.Drone.Video;

namespace AR.Drone.WinApp
{
    public partial class MainForm : Form
    {
        private readonly ARDroneClient _arDroneClient;
        private uint _currentFrameNumber;
        private VideoFrame _videoFrame;

        public MainForm()
        {
            InitializeComponent();

            _arDroneClient = new ARDroneClient();
            _arDroneClient.VideoFrameDecoded += OnVideoDecoderOnFrameDecoded;
            tmrStateUpdate.Enabled = true;
            tmrVideoUpdate.Enabled = true;
        }

        private void OnVideoDecoderOnFrameDecoded(ARDroneClient client, VideoFrame frame)
        {
            _videoFrame = frame;
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
            if (_videoFrame.FrameNumber == _currentFrameNumber) return;
            _currentFrameNumber = _videoFrame.FrameNumber;
            Image oldImage = pbVideo.Image;
            Bitmap newImage = ARDroneVideoHelper.CreateImageFromFrame(_videoFrame);
            pbVideo.Image = newImage;
            if (oldImage != null) oldImage.Dispose();
        }

        private void tmrStateUpdate_Tick(object sender, EventArgs e)
        {
            tvInfo.BeginUpdate();

            TreeNode node = tvInfo.Nodes.GetOrCreate("ClientActive");
            node.Text = string.Format("Client Active: {0}", _arDroneClient.Active);

            node = tvInfo.Nodes.GetOrCreate("Navigation Data");
            DumpBranch(node.Nodes, _arDroneClient.NavigationData);

            NativeNavdata navdata = _arDroneClient.NativeNavdata;
            
            var vativeNode = tvInfo.Nodes.GetOrCreate("Native");

            var ctrl_state = (CTRL_STATES)(navdata.demo.ctrl_state >> 0x10);
            node = vativeNode.Nodes.GetOrCreate("ctrl_state");
            node.Text = string.Format("Ctrl State: {0}", ctrl_state);

            var flying_state = (FLYING_STATES)(navdata.demo.ctrl_state & 0xffff);
            node = vativeNode.Nodes.GetOrCreate("flying_state");
            node.Text = string.Format("Ctrl State: {0}", flying_state);

            DumpBranch(vativeNode.Nodes, navdata);

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
            _arDroneClient.SetVideoChanell(VideoChannel.Next);
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