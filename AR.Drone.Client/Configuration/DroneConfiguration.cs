using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AR.Drone.Client.Configuration
{
    [StructLayout(LayoutKind.Sequential)]
    public class DroneConfiguration
    {
        public static readonly string DefaultApplicationId = "default";
        public static readonly string DefaultUserId = "default";

        public static string NewSessionId()
        {
            return Guid.NewGuid().ToString("N");
        }

        private readonly string _applicationId;
        private readonly string _userId;
        private readonly string _sessionId;

        private readonly Dictionary<string, IConfigurationItem> _items;
        private readonly ConcurrentQueue<ATCommand> _queue;


        public readonly GeneralSection General;
        public readonly ControlSection Control;
        public readonly NetworkSection Network;
        public readonly PicSection Pic;
        public readonly VideoSection Video;
        public readonly LedsSection Leds;
        public readonly DetectSection Detect;
        public readonly SyslogSection Syslog;
        public readonly UserboxSection Userbox;
        public readonly GpsSection Gps;
        public readonly CustomSection Custom;

        public DroneConfiguration(string applicationId, string userId, string sessionId)
        {
            _applicationId = applicationId;
            _userId = userId;
            _sessionId = sessionId;

            _items = new Dictionary<string, IConfigurationItem>();
            _queue = new ConcurrentQueue<ATCommand>();

            General = new GeneralSection(this);
            Control = new ControlSection(this);
            Network = new NetworkSection(this);
            Pic = new PicSection(this);
            Video = new VideoSection(this);
            Leds = new LedsSection(this);
            Detect = new DetectSection(this);
            Syslog = new SyslogSection(this);
            Userbox = new UserboxSection(this);
            Gps = new GpsSection(this);
            Custom = new CustomSection(this);
        }
        
        public DroneConfiguration(string application, string user)
            : this(application, user, NewSessionId())
        {
        }

        public DroneConfiguration(string application)
            : this(application, DefaultUserId, NewSessionId())
        {
        }

        public DroneConfiguration()
            : this(DefaultApplicationId, DefaultUserId, NewSessionId())
        {
        }
        
        public Dictionary<string, IConfigurationItem> Items
        {
            get { return _items; }
        }

        public void Enqueue(ATCommand command)
        {
            _queue.Enqueue(command);
        }

        public void SendTo(DroneClient client)
        {
            ATCommand command;
            while (_queue.TryDequeue(out command)) client.Send(command);
        }

        public class GeneralSection : SectionBase
        {
            private readonly ActiveItem<bool> _navdataDemo;
            private readonly ActiveItem<int> _navdataOptions;
            private readonly ActiveItem<int> _comWatchdog;
            private readonly ActiveItem<bool> _video;
            private readonly ActiveItem<bool> _vision;
            private readonly ActiveItem<int> _batteryVoltageMin;
            private readonly ActiveItem<int> _localTime;

            public GeneralSection(DroneConfiguration configuration)
                : base(configuration, "general")
            {
                _navdataDemo = new ActiveItem<bool>(this, "navdata_demo");
                _navdataOptions = new ActiveItem<int>(this, "navdata_options");
                _comWatchdog = new ActiveItem<int>(this, "com_watchdog");
                _video = new ActiveItem<bool>(this, "video_enable");
                _vision = new ActiveItem<bool>(this, "vision_enable");
                _batteryVoltageMin = new ActiveItem<int>(this, "vbat_min");
                _localTime = new ActiveItem<int>(this, "localtime");
            }

            public int ConfigVersion
            {
                get { return GetInt32("num_version_config"); }
            }

            public int MotherboardVersion
            {
                get { return GetInt32("num_version_mb"); }
            }

            public string SoftVersion
            {
                get { return GetString("num_version_soft"); }
            }

            public string DroneSerial
            {
                get { return GetString("drone_serial"); }
            }

            public string SoftBuildDate
            {
                get { return GetString("soft_build_date"); }
            }

            public string Motor1Soft
            {
                get { return GetString("motor1_soft"); }
            }

            public string Motor1Hard
            {
                get { return GetString("motor1_hard"); }
            }

            public string Motor1Supplier
            {
                get { return GetString("motor1_supplier"); }
            }

            public string Motor2Soft
            {
                get { return GetString("motor2_soft"); }
            }

            public string Motor2Hard
            {
                get { return GetString("motor2_hard"); }
            }

            public string Motor2Supplier
            {
                get { return GetString("motor2_supplier"); }
            }

            public string Motor3Soft
            {
                get { return GetString("motor3_soft"); }
            }

            public string Motor3Hard
            {
                get { return GetString("motor3_hard"); }
            }

            public string Motor3Supplier
            {
                get { return GetString("motor3_supplier"); }
            }

            public string Motor4Soft
            {
                get { return GetString("motor4_soft"); }
            }

            public string Motor4Hard
            {
                get { return GetString("motor4_hard"); }
            }

            public string Motor4Supplier
            {
                get { return GetString("motor4_supplier"); }
            }

            public string ArDroneName
            {
                get { return GetString("ardrone_name"); }
                set { Set("ardrone_name", value); }
            }

            public int FlyingTime
            {
                get { return GetInt32("flying_time"); }
            }

            public ActiveItem<bool> NavdataDemo
            {
                get { return _navdataDemo; }
            }

            public ActiveItem<int> NavdataOptions
            {
                get { return _navdataOptions; }
            }

            public ActiveItem<int> ComWatchdog
            {
                get { return _comWatchdog; }
            }

            public ActiveItem<bool> Video
            {
                get { return _video; }
            }

            public ActiveItem<bool> Vision
            {
                get { return _vision; }
            }

            public ActiveItem<int> BatteryVoltageMin
            {
                get { return _batteryVoltageMin; }
            }

            public ActiveItem<int> LocalTime
            {
                get { return _localTime; }
            }
        }

        public class ControlSection : SectionBase
        {
            public readonly ReadOnlyItem<string> accs_offset;
            public readonly ReadOnlyItem<string> accs_gains;
            public readonly ReadOnlyItem<string> gyros_offset;
            public readonly ReadOnlyItem<string> gyros_gains;
            public readonly ReadOnlyItem<string> gyros110_offset;
            public readonly ReadOnlyItem<string> gyros110_gains;
            public readonly ReadOnlyItem<string> magneto_offset;
            public readonly ReadOnlyItem<float> magneto_radius;
            public readonly ReadOnlyItem<float> gyro_offset_thr_x;
            public readonly ReadOnlyItem<float> gyro_offset_thr_y;
            public readonly ReadOnlyItem<float> gyro_offset_thr_z;
            public readonly ReadOnlyItem<int> pwm_ref_gyros;
            public readonly ReadOnlyItem<int> osctun_value;
            public readonly ReadOnlyItem<bool> osctun_test;
            public readonly ReadOnlyItem<int> control_level;
            public readonly ActiveItem<float> euler_angle_max;
            public readonly ActiveItem<int> altitude_max;
            public readonly ActiveItem<int> altitude_min;
            public readonly ActiveItem<float> control_iphone_tilt;
            public readonly ActiveItem<float> control_vz_max;
            public readonly ActiveItem<float> control_yaw;
            public readonly ActiveItem<bool> outdoor;
            public readonly ActiveItem<bool> flight_without_shell;
            public readonly ReadOnlyItem<bool> autonomous_flight;
            public readonly ActiveItem<bool> manual_trim;
            public readonly ActiveItem<float> indoor_euler_angle_max;
            public readonly ActiveItem<float> indoor_control_vz_max;
            public readonly ActiveItem<float> indoor_control_yaw;
            public readonly ActiveItem<float> outdoor_euler_angle_max;
            public readonly ActiveItem<float> outdoor_control_vz_max;
            public readonly ActiveItem<float> outdoor_control_yaw;
            public readonly ActiveItem<int> flying_mode;
            public readonly ActiveItem<int> hovering_range;
            public readonly FlightAnimationItem flight_anim;

            public ControlSection(DroneConfiguration configuration)
                : base(configuration, "control")
            {
                accs_offset = new ReadOnlyItem<string>(this, "accs_offset");
                accs_gains = new ReadOnlyItem<string>(this, "accs_gains");
                gyros_offset = new ReadOnlyItem<string>(this, "gyros_offset");
                gyros_gains = new ReadOnlyItem<string>(this, "gyros_gains");
                gyros110_offset = new ReadOnlyItem<string>(this, "gyros110_offset");
                gyros110_gains = new ReadOnlyItem<string>(this, "gyros110_gains");
                magneto_offset = new ReadOnlyItem<string>(this, "magneto_offset");
                magneto_radius = new ReadOnlyItem<float>(this, "magneto_radius");
                gyro_offset_thr_x = new ReadOnlyItem<float>(this, "gyro_offset_thr_x");
                gyro_offset_thr_y = new ReadOnlyItem<float>(this, "gyro_offset_thr_y");
                gyro_offset_thr_z = new ReadOnlyItem<float>(this, "gyro_offset_thr_z");
                pwm_ref_gyros = new ReadOnlyItem<int>(this, "pwm_ref_gyros");
                osctun_value = new ReadOnlyItem<int>(this, "osctun_value");
                osctun_test = new ReadOnlyItem<bool>(this, "osctun_test");
                control_level = new ActiveItem<int>(this, "control_level");
                euler_angle_max = new ActiveItem<float>(this, "euler_angle_max");
                altitude_max = new ActiveItem<int>(this, "altitude_max");
                altitude_min = new ActiveItem<int>(this, "altitude_min");
                control_iphone_tilt = new ActiveItem<float>(this, "control_iphone_tilt");
                control_vz_max = new ActiveItem<float>(this, "control_vz_max");
                control_yaw = new ActiveItem<float>(this, "control_yaw");
                outdoor = new ActiveItem<bool>(this, "outdoor");
                flight_without_shell = new ActiveItem<bool>(this, "flight_without_shell");
                autonomous_flight = new ReadOnlyItem<bool>(this, "autonomous_flight"); // obsolete
                manual_trim = new ActiveItem<bool>(this, "manual_trim");
                indoor_euler_angle_max = new ActiveItem<float>(this, "indoor_euler_angle_max");
                indoor_control_vz_max = new ActiveItem<float>(this, "indoor_control_vz_max");
                indoor_control_yaw = new ActiveItem<float>(this, "indoor_control_yaw");
                outdoor_euler_angle_max = new ActiveItem<float>(this, "outdoor_euler_angle_max");
                outdoor_control_vz_max = new ActiveItem<float>(this, "outdoor_control_vz_max");
                outdoor_control_yaw = new ActiveItem<float>(this, "outdoor_control_yaw");
                flying_mode = new ActiveItem<int>(this, "flying_mode");
                hovering_range = new ActiveItem<int>(this, "hovering_range");
                flight_anim = new FlightAnimationItem(this, "flight_anim");
            }
        }

        public class NetworkSection : SectionBase
        {
            public readonly ActiveItem<string> SsidSinglePlayer;
            public readonly ActiveItem<string> SsidMultiPlayer;
            public readonly ActiveItem<int> WifiMode;
            public readonly ActiveItem<int> WifiRate;
            public readonly ActiveItem<string> OwnerMac;

            public NetworkSection(DroneConfiguration configuration)
                : base(configuration, "network")
            {
                SsidSinglePlayer = new ActiveItem<string>(this, "ssid_single_player");
                SsidMultiPlayer = new ActiveItem<string>(this, "ssid_multi_player");
                WifiMode = new ActiveItem<int>(this, "wifi_mode");
                WifiRate = new ActiveItem<int>(this, "wifi_rate");
                OwnerMac = new ActiveItem<string>(this, "owner_mac");
            }
        }

        public class PicSection : SectionBase
        {
            public readonly ActiveItem<int> UltrasoundFreq;
            public readonly ActiveItem<int> UltrasoundWatchdog;
            public readonly ReadOnlyItem<int> Version;

            public PicSection(DroneConfiguration configuration)
                : base(configuration, "pic")
            {
                UltrasoundFreq = new ActiveItem<int>(this, "ultrasound_freq");
                UltrasoundWatchdog = new ActiveItem<int>(this, "ultrasound_watchdog");
                Version = new ReadOnlyItem<int>(this, "pic_version");
            }
        }

        public class VideoSection : SectionBase
        {
            public readonly ReadOnlyItem<int> CamifFps;
            public readonly ActiveItem<int> CodecFps;
            public readonly ReadOnlyItem<int> CamifBuffers;
            public readonly ReadOnlyItem<int> Trackers;
            public readonly ActiveItem<VideoCodecType> Codec;
            public readonly ActiveItem<int> Slices;
            public readonly ActiveItem<int> LiveSocket;
            public readonly ReadOnlyItem<int> StorageSpace;
            public readonly ActiveItem<int> Bitrate;
            public readonly ActiveItem<int> MaxBitrate;
            public readonly ActiveItem<VideoBitrateControlMode> BitrateCtrlMode;
            public readonly ActiveItem<int> BitrateStorage;
            public readonly ActiveItem<VideoChannelType> Channel;
            public readonly ActiveItem<bool> OnUsb;
            public readonly ActiveItem<int> FileIndex;

            public VideoSection(DroneConfiguration configuration)
                : base(configuration, "video")
            {
                CamifFps = new ReadOnlyItem<int>(this, "camif_fps");
                CodecFps = new ActiveItem<int>(this, "codec_fps");
                CamifBuffers = new ReadOnlyItem<int>(this, "camif_buffers");
                Trackers = new ReadOnlyItem<int>(this, "num_trackers");
                Codec = new ActiveItem<VideoCodecType>(this, "video_codec");
                Slices = new ActiveItem<int>(this, "video_slices");
                LiveSocket = new ActiveItem<int>(this, "video_live_socket");
                StorageSpace = new ReadOnlyItem<int>(this, "video_storage_space");
                Bitrate = new ActiveItem<int>(this, "bitrate");
                MaxBitrate = new ActiveItem<int>(this, "max_bitrate");
                BitrateCtrlMode = new ActiveItem<VideoBitrateControlMode>(this, "bitrate_ctrl_mode");
                BitrateStorage = new ActiveItem<int>(this, "bitrate_storage");
                Channel = new ActiveItem<VideoChannelType>(this, "video_channel");
                OnUsb = new ActiveItem<bool>(this, "video_on_usb");
                FileIndex = new ActiveItem<int>(this, "video_file_index");
            }
        }

        public class LedsSection : SectionBase
        {
            public readonly ActiveItem<string> Animation;

            public LedsSection(DroneConfiguration configuration)
                : base(configuration, "leds")
            {
                Animation = new ActiveItem<string>(this, "leds_anim");
            }
        }

        public class DetectSection : SectionBase
        {
            public readonly ActiveItem<int> EnemyColors;
            public readonly ActiveItem<int> GroundstripeColors;
            public readonly ActiveItem<int> EnemyWithoutShell;
            public readonly ActiveItem<int> DetectType;
            public readonly ActiveItem<int> DetectionsSelectH;
            public readonly ActiveItem<int> DetectionsSelectVHsync;
            public readonly ActiveItem<int> DetectionsSelectV;

            public DetectSection(DroneConfiguration configuration)
                : base(configuration, "detect")
            {
                EnemyColors = new ActiveItem<int>(this, "enemy_colors");
                GroundstripeColors = new ActiveItem<int>(this, "groundstripe_colors");
                EnemyWithoutShell = new ActiveItem<int>(this, "enemy_without_shell");
                DetectType = new ActiveItem<int>(this, "detect_type");
                DetectionsSelectH = new ActiveItem<int>(this, "detections_select_h");
                DetectionsSelectVHsync = new ActiveItem<int>(this, "detections_select_v_hsync");
                DetectionsSelectV = new ActiveItem<int>(this, "detections_select_v");
            }
        }

        public class SyslogSection : SectionBase
        {
            public readonly ActiveItem<int> Output;
            public readonly ActiveItem<int> MaxSize;
            public readonly ActiveItem<int> NbFiles;

            public SyslogSection(DroneConfiguration configuration)
                : base(configuration, "syslog")
            {
                Output = new ActiveItem<int>(this, "output");
                MaxSize = new ActiveItem<int>(this, "max_size");
                NbFiles = new ActiveItem<int>(this, "nb_files");
            }
        }

        public class UserboxSection : SectionBase
        {
            public readonly ActiveItem<string> UserboxCmd;

            public UserboxSection(DroneConfiguration configuration)
                : base(configuration, "userbox")
            {
                UserboxCmd = new ActiveItem<string>(this, "userbox_cmd");
            }
        }

        public class GpsSection : SectionBase
        {
            public readonly ReadOnlyItem<double> Latitude;
            public readonly ReadOnlyItem<double> Longitude;
            public readonly ReadOnlyItem<double> Altitude;

            public GpsSection(DroneConfiguration configuration)
                : base(configuration, "gps")
            {
                Latitude = new ReadOnlyItem<double>(this, "latitude");
                Longitude = new ReadOnlyItem<double>(this, "longitude");
                Altitude = new ReadOnlyItem<double>(this, "altitude");
            }
        }

        public class CustomSection : SectionBase
        {
            public readonly ReadOnlyItem<string> ApplicationId;
            public readonly ReadOnlyItem<string> ApplicationDescription;
            public readonly ReadOnlyItem<string> ProfileId;
            public readonly ReadOnlyItem<string> ProfileDescription;
            public readonly ReadOnlyItem<string> SessionId;
            public readonly ReadOnlyItem<string> SessionDescription;

            public CustomSection(DroneConfiguration configuration)
                : base(configuration, "custom")
            {
                ApplicationId = new ReadOnlyItem<string>(this, "application_id");
                ApplicationDescription = new ReadOnlyItem<string>(this, "application_desc");
                ProfileId = new ReadOnlyItem<string>(this, "profile_id");
                ProfileDescription = new ReadOnlyItem<string>(this, "profile_desc");
                SessionId = new ReadOnlyItem<string>(this, "session_id");
                SessionDescription = new ReadOnlyItem<string>(this, "session_desc");
            }
        }
    }
}