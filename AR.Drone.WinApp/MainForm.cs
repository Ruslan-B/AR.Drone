using System;
using System.Reflection;
using System.Windows.Forms;
using AR.Drone.Api.Commands;
using AR.Drone.Api.Video;
using AR.Drone.NativeApi;

namespace AR.Drone.WinApp
{
    public partial class MainForm : Form
    {
        private readonly DroneController _droneController;

        public MainForm()
        {
            InitializeComponent();

            _droneController = new DroneController();
            _droneController.FrameDecoded += OnVideoDecoderOnFrameDecoded;
            tmrStateUpdate.Enabled = true;
        }

        private void OnVideoDecoderOnFrameDecoded(DroneController controller, VideoFrame frame)
        {
            this.ExecuteOnUIThread(() =>
                {
                    if (pbVideo.Image != null)
                    {
                        pbVideo.Image.Dispose();
                        pbVideo.Image = null;
                    }
                    pbVideo.Image = ARDroneVideoHelper.CreateImageFromFrame(frame);
                });
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _droneController.Active = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _droneController.Active = false;
        }

        private void tmrStateUpdate_Tick(object sender, EventArgs e)
        {
            tvInfo.BeginUpdate();

            TreeNode activeNode = tvInfo.Nodes.GetOrCreate("Active");
            activeNode.Text = string.Format("Active: {0}", _droneController.Active);

            TreeNode stateNode = tvInfo.Nodes.GetOrCreate("State");
            stateNode.Text = string.Format("State: {0}", _droneController.DroneState);

            TreeNode rawNavdataNode = tvInfo.Nodes.GetOrCreate("RawNavdata");
            RawNavdata rawNavdata = _droneController.RawNavdata;
            DumpBranch(rawNavdataNode, rawNavdata);

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
            _droneController.Progress(gaz: 0.1f);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            _droneController.Progress(gaz: -0.1f);
        }

        private void btnTurnLeft_Click(object sender, EventArgs e)
        {
            _droneController.Progress(yaw: 0.2f);
        }

        private void btnTurnRight_Click(object sender, EventArgs e)
        {
            _droneController.Progress(yaw: -0.2f);
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            _droneController.Progress(ProgressiveMode.CombinedYaw, roll: 0.01f);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            _droneController.Progress(ProgressiveMode.CombinedYaw, roll: -0.01f);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _droneController.ResetEmergency();
        }
    }
}