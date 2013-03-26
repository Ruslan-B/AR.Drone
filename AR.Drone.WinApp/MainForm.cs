using System;
using System.Text;
using System.Windows.Forms;
using AR.Drone.Api.Commands;
using AR.Drone.NativeApi.Navdata;
using AR.Drone.Workers;
using AR.Drone.Api.Navdata;
using AR.Drone.Api.Video;

namespace AR.Drone.WinApp
{
    public partial class MainForm : Form
    {
        private readonly DroneController _droneController;
        private readonly VideoDecoder _videoDecoder;

        public MainForm()
        {
            InitializeComponent();

            _videoDecoder = new VideoDecoder();
            _videoDecoder.FrameDecoded += OnVideoDecoderOnFrameDecoded;

            _droneController = new DroneController();
            _droneController.VideoFrameAcquired += (c, f) => _videoDecoder.EnqueuePacket(f);

            tmrStateUpdate.Enabled = true;
        }

        private void OnVideoDecoderOnFrameDecoded(VideoDecoder decoder, VideoFrame frame)
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
            pbVideo.Image = null;
            _videoDecoder.Start();
            _droneController.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _droneController.Stop();
            _videoDecoder.Stop();
        }

        private void tmrStateUpdate_Tick(object sender, EventArgs e)
        {
            var sbState = new StringBuilder();
            sbState.AppendFormat("Alive: {0}\r", _droneController.IsAlive);
            
            sbState.AppendFormat("Controller State: {0}\r", _droneController.State);
            
            NavdataInfo navdataInfo = _droneController.NavInfo;
            sbState.AppendFormat("Navdata: {0}\r", navdataInfo.State);
            
            NavdataDemo demo = navdataInfo.Demo;
            sbState.Append("Demo data:\r");
            sbState.AppendFormat("State: {0}\r", demo.ctrl_state);
            sbState.AppendFormat("VBat: {0}\r", demo.vbat_flying_percentage);
            sbState.AppendFormat("UAV theta: {0:##0.000} phi: {1:##0.000} psi: {2:##0.000}\r", demo.theta, demo.phi, demo.psi);
            sbState.AppendFormat("Altitude: {0}\r", demo.altitude);
            sbState.AppendFormat("Velocity: vx: {0:##0.000} vy: {1:##0.000} yz: {2:##0.000}\r", demo.vx, demo.vy, demo.vz);
            sbState.AppendLine();

            NavdataAltitude altitude =  navdataInfo.Altitude;
            sbState.Append("Altitude:\r");
            sbState.AppendFormat("altitude_vision: {0}\r", altitude.altitude_vision);
            sbState.AppendFormat("altitude_vz: {0}\r", altitude.altitude_vz);
            sbState.AppendFormat("altitude_ref: {0}\r", altitude.altitude_ref);
            sbState.AppendFormat("altitude_raw: {0}\r", altitude.altitude_raw);
            sbState.AppendFormat("obs_accZ: {0}\r", altitude.obs_accZ);
            sbState.AppendFormat("obs_alt: {0}\r", altitude.obs_accZ);
            sbState.AppendFormat("obs_x: {0:0.00} {1:0.00} {2:0.00}\r", altitude.obs_x.x, altitude.obs_x.y, altitude.obs_x.z);
            sbState.AppendFormat("obs_x: {0}\r", altitude.obs_state);
            sbState.AppendFormat("est_vb: {0:0.00} {1:0.00}\r", altitude.est_vb.x, altitude.est_vb.y);
            sbState.AppendFormat("est_state: {0}\r", altitude.est_state);
            sbState.AppendLine();

            NavdataTrims trims = navdataInfo.Trims;
            sbState.Append("Trims:\r");
            sbState.AppendFormat("angular_rates_trim_r: {0}\r", trims.angular_rates_trim_r);
            sbState.AppendFormat("angular_rates_trim_r: {0}\r", trims.euler_angles_trim_theta);
            sbState.AppendFormat("angular_rates_trim_r: {0}\r", trims.euler_angles_trim_phi);
            sbState.AppendLine();

            lState.Text = sbState.ToString();
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
            _droneController.Move(gaz: 0.1f);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            _droneController.Move(gaz: -0.1f);
        }

        private void btnTurnLeft_Click(object sender, EventArgs e)
        {
            _droneController.Move(yaw: 0.1f);
        }

        private void btnTurnRight_Click(object sender, EventArgs e)
        {
            _droneController.Move(yaw: -0.1f);
        }

    }
}