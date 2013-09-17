using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using AR.Drone.Client;
using AR.Drone.Client.Commands;
using AR.Drone.Client.Configuration;
using AR.Drone.Data;
using AR.Drone.Data.Navigation;
using AR.Drone.Data.Navigation.Native;
using AR.Drone.Infrastructure;
using AR.Drone.Media;
using AR.Drone.Video;

namespace AR.Drone.WinApp
{
    public partial class MainForm : Form
    {
        private readonly DroneClient _droneClient;
        private readonly List<PlayerForm> _playerForms;
        private readonly VideoPacketDecoderWorker _videoPacketDecoderWorker;
        private DroneConfiguration _configuration;
        private VideoFrame _frame;
        private Bitmap _frameBitmap;
        private uint _frameNumber;
        private NavigationData _navigationData;
        private NavigationPacket _navigationPacket;
        private PacketRecorder _packetRecorderWorker;
        private FileStream _recorderStream;

        public MainForm()
        {
            InitializeComponent();

            _videoPacketDecoderWorker = new VideoPacketDecoderWorker(PixelFormat.BGR24, true, OnVideoPacketDecoded);
            _videoPacketDecoderWorker.Start();

            _droneClient = new DroneClient();
            _droneClient.NavigationPacketAcquired += OnNavigationPacketAcquired;
            _droneClient.VideoPacketAcquired += OnVideoPacketAcquired;
            _droneClient.NavigationDataAcquired += data => _navigationData = data;

            tmrStateUpdate.Enabled = true;
            tmrVideoUpdate.Enabled = true;

            _playerForms = new List<PlayerForm>();

            _videoPacketDecoderWorker.UnhandledException += UnhandledException;
        }

        private void UnhandledException(object sender, Exception exception)
        {
            MessageBox.Show(exception.ToString(), "Unhandled Exception (Ctrl+C)", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text += Environment.Is64BitProcess ? " [64-bit]" : " [32-bit]";
        }

        protected override void OnClosed(EventArgs e)
        {
            StopRecording();

            _droneClient.Dispose();
            _videoPacketDecoderWorker.Dispose();

            base.OnClosed(e);
        }

        private void OnNavigationPacketAcquired(NavigationPacket packet)
        {
            if (_packetRecorderWorker != null && _packetRecorderWorker.IsAlive)
                _packetRecorderWorker.EnqueuePacket(packet);

            _navigationPacket = packet;
        }

        private void OnVideoPacketAcquired(VideoPacket packet)
        {
            if (_packetRecorderWorker != null && _packetRecorderWorker.IsAlive)
                _packetRecorderWorker.EnqueuePacket(packet);
            if (_videoPacketDecoderWorker.IsAlive)
                _videoPacketDecoderWorker.EnqueuePacket(packet);
        }

        private void OnVideoPacketDecoded(VideoFrame frame)
        {
            _frame = frame;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _droneClient.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _droneClient.Stop();
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
            node.Text = string.Format("Client Active: {0}", _droneClient.IsActive);

            node = tvInfo.Nodes.GetOrCreate("Navigation Data");
            if (_navigationData != null) DumpBranch(node.Nodes, _navigationData);

            node = tvInfo.Nodes.GetOrCreate("Configuration");
            if (_configuration != null) DumpBranch(node.Nodes, _configuration);

            TreeNode vativeNode = tvInfo.Nodes.GetOrCreate("Native");

            NavdataBag navdataBag;
            if (_navigationPacket.Data != null && NavdataBagParser.TryParse(ref _navigationPacket, out navdataBag))
            {
                var ctrl_state = (CTRL_STATES) (navdataBag.demo.ctrl_state >> 0x10);
                node = vativeNode.Nodes.GetOrCreate("ctrl_state");
                node.Text = string.Format("Ctrl State: {0}", ctrl_state);

                var flying_state = (FLYING_STATES) (navdataBag.demo.ctrl_state & 0xffff);
                node = vativeNode.Nodes.GetOrCreate("flying_state");
                node.Text = string.Format("Ctrl State: {0}", flying_state);

                DumpBranch(vativeNode.Nodes, navdataBag);
            }
            tvInfo.EndUpdate();
        }

        private void DumpBranch(TreeNodeCollection nodes, object o)
        {
            Type objectType = o.GetType();
            foreach (FieldInfo fieldInfo in objectType.GetFields())
            {
                TreeNode node = nodes.GetOrCreate(fieldInfo.Name);
                object value = fieldInfo.GetValue(o);

                if (value == null)
                    node.Text = node.Name + ": null";
                else
                {
                    Type valueType = fieldInfo.FieldType;
                    if (valueType.Namespace.StartsWith("System") || valueType.IsEnum)
                        node.Text = node.Name + ": " + value;
                    else
                        DumpBranch(node.Nodes, value);
                }
            }

            foreach (PropertyInfo propertyInfo in objectType.GetProperties())
            {
                TreeNode node = nodes.GetOrCreate(propertyInfo.Name);
                object value = propertyInfo.GetValue(o, null);

                if (value == null)
                    node.Text = node.Name + ": null";
                else
                {
                    Type valueType = propertyInfo.PropertyType;
                    if (valueType.Namespace.StartsWith("System") || valueType.IsEnum)
                        node.Text = node.Name + ": " + value;
                    else
                        DumpBranch(node.Nodes, value);
                }
            }
        }

        private void btnFlatTrim_Click(object sender, EventArgs e)
        {
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
            DroneConfiguration configuration = new DroneConfiguration();
            configuration.Video.Channel = VideoChannelType.Next;
            configuration.SendChanges(_droneClient);
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
            Task<DroneConfiguration> configurationTask = _droneClient.GetConfigurationTask();
            configurationTask.ContinueWith(delegate(Task<DroneConfiguration> task)
                {
                    if (task.Exception != null)
                    {
                        Trace.TraceWarning("Get configuration task is faulted with exception: {0}", task.Exception.InnerException.Message);
                        return;
                    }

                    _configuration = task.Result;
                });
            configurationTask.Start();
        }

        private void btnSendConfig_Click(object sender, EventArgs e)
        {
            DroneConfiguration configuration = _configuration ?? new DroneConfiguration();

            if (string.IsNullOrEmpty(configuration.Custom.SessionId))
            {
                // set new session, application and profile

                configuration.Custom.SessionId = DroneConfiguration.NewId();
                configuration.SendChanges(_droneClient);
                System.Threading.Thread.Sleep(200);

                configuration.Custom.ApplicationId = DroneConfiguration.NewId();
                configuration.SendChanges(_droneClient);
                System.Threading.Thread.Sleep(200);

                configuration.Custom.ProfileId = DroneConfiguration.NewId();
                configuration.SendChanges(_droneClient);
                System.Threading.Thread.Sleep(200);
            }

            configuration.Video.BitrateCtrlMode = VideoBitrateControlMode.Dynamic;
            configuration.Video.Codec = VideoCodecType.H264_360P;
            configuration.Video.Bitrate = 1000;
            configuration.Video.MaxBitrate = 2000;

            //send all changes in one pice
            configuration.SendChanges(_droneClient);
        }

        private void StopRecording()
        {
            if (_packetRecorderWorker != null)
            {
                _packetRecorderWorker.Stop();
                _packetRecorderWorker.Join();
                _packetRecorderWorker = null;
            }
            if (_recorderStream != null)
            {
                _recorderStream.Dispose();
                _recorderStream = null;
            }
        }

        private void btnStartRecording_Click(object sender, EventArgs e)
        {
            string path = string.Format("flight_{0:yyyy_MM_dd_HH_mm}.ardrone", DateTime.Now);

            using (var dialog = new SaveFileDialog {DefaultExt = ".ardrone", Filter = "AR.Drone track files (*.ardrone)|*.ardrone", FileName = path})
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    StopRecording();

                    _recorderStream = new FileStream(dialog.FileName, FileMode.OpenOrCreate);
                    _packetRecorderWorker = new PacketRecorder(_recorderStream);
                    _packetRecorderWorker.Start();
                }
            }
        }

        private void btnStopRecording_Click(object sender, EventArgs e)
        {
            StopRecording();
        }

        private void btnReplay_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog {DefaultExt = ".ardrone", Filter = "AR.Drone track files (*.ardrone)|*.ardrone"})
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    StopRecording();

                    var playerForm = new PlayerForm {FileName = dialog.FileName};
                    playerForm.Closed += (o, args) => _playerForms.Remove(o as PlayerForm);
                    _playerForms.Add(playerForm);
                    playerForm.Show(this);
                }
            }
        }
    }
}