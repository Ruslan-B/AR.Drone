using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using AR.Drone.Command;
using AR.Drone.Navigation;
using AR.Drone.Video;

namespace AR.Drone.WinApp
{
    public partial class MainForm : Form
    {
        private readonly DroneController _droneController;
        private uint _currentFrameNumber;
        private VideoFrame _videoFrame;

        public MainForm()
        {
            InitializeComponent();

            _droneController = new DroneController();
            _droneController.FrameDecoded += OnVideoDecoderOnFrameDecoded;
            tmrStateUpdate.Enabled = true;
            tmrVideoUpdate.Enabled = true;
        }

        private void OnVideoDecoderOnFrameDecoded(DroneController controller, VideoFrame frame)
        {
            _videoFrame = frame;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _droneController.Active = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _droneController.Active = false;
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

            TreeNode activeNode = tvInfo.Nodes.GetOrCreate("Active");
            activeNode.Text = string.Format("Active: {0}", _droneController.Active);

            TreeNode stateNode = tvInfo.Nodes.GetOrCreate("State");
            stateNode.Text = string.Format("State: {0}", _droneController.DroneState);

            TreeNode navdataNode = tvInfo.Nodes.GetOrCreate("NavigationData");
            NavigationData navigationData = _droneController.NavigationData;
            DumpBranch(navdataNode, navigationData);

            tvInfo.EndUpdate();
        }

        private void DumpBranch(TreeNode node, object value)
        {
            if (value == null)
            {
                node.Text = node.Name + ": null";
                return;
            }

            Type type = value.GetType();

            if (type.Namespace.StartsWith("System") || type.IsEnum)
            {
                node.Text = node.Name + ": " + value;
                return;
            }

            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo fieldInfo in fields)
            {
                TreeNode fieldNode = node.Nodes.GetOrCreate(fieldInfo.Name);
                object fieldValue = fieldInfo.GetValue(value);
                DumpBranch(fieldNode, fieldValue);
            }
        }

        private void btnFlatTrim_Click(object sender, EventArgs e)
        {
            _droneController.FlatTrim();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _droneController.Takeoff();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _droneController.Land();
        }

        private void btnEmergency_Click(object sender, EventArgs e)
        {
            _droneController.Emergency();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _droneController.ResetEmergency();
        }

        private void btnSwitchCam_Click(object sender, EventArgs e)
        {
            _droneController.SwitchVideoChanell(VideoChannel.Next);
        }

        private void btnHover_Click(object sender, EventArgs e)
        {
            _droneController.Hover();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            _droneController.Progress(gaz: 0.2f);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            _droneController.Progress(gaz: -0.2f);
        }

        private void btnTurnLeft_Click(object sender, EventArgs e)
        {
            _droneController.Progress(yaw: 0.5f);
        }

        private void btnTurnRight_Click(object sender, EventArgs e)
        {
            _droneController.Progress(yaw: -0.5f);
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            _droneController.Progress(roll: 0.2f);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            _droneController.Progress(roll: -0.2f);
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            _droneController.Progress(pitch: 0.2f);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _droneController.Progress(pitch: -0.2f);
        }
    }
}