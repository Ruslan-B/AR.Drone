using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AR.Drone.Client.Configuration
{
    [StructLayout(LayoutKind.Sequential)]
    public class DroneConfiguration
    {
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

        private readonly Dictionary<string, IConfigurationItem> _items;
        private readonly ConcurrentQueue<ATCommand> _queue;

        public DroneConfiguration()
        {
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

        public Dictionary<string, IConfigurationItem> Items
        {
            get { return _items; }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class GeneralSection : SectionBase
        {
            public readonly dynamic ConfigVersion;
            public readonly dynamic MotherboardVersion;
            public readonly dynamic SoftVersion;
            public readonly dynamic DroneSerial;
            public readonly dynamic SoftBuildDate;
            public readonly dynamic Motor1Soft;
            public readonly dynamic Motor1Hard;
            public readonly dynamic Motor1Supplier;
            public readonly dynamic Motor2Soft;
            public readonly dynamic Motor2Hard;
            public readonly dynamic Motor2Supplier;
            public readonly dynamic Motor3Soft;
            public readonly dynamic Motor3Hard;
            public readonly dynamic Motor3Supplier;
            public readonly dynamic Motor4Soft;
            public readonly dynamic Motor4Hard;
            public readonly dynamic Motor4Supplier;
            public readonly dynamic ARDroneName;
            public readonly dynamic FlyingTime;
            public readonly dynamic NavdataDemo;
            public readonly dynamic NavdataOptions;
            public readonly dynamic ComWatchdog;
            public readonly dynamic Video;
            public readonly dynamic Vision;
            public readonly dynamic BatteryVoltageMin;
            public readonly dynamic LocalTime;

            public GeneralSection(DroneConfiguration configuration)
                : base(configuration, "general")
            {
                ConfigVersion = new ReadOnlyItem<int>(this, "num_version_config");
                MotherboardVersion = new ReadOnlyItem<int>(this, "num_version_mb");
                SoftVersion = new ReadOnlyItem<string>(this, "num_version_soft");
                DroneSerial = new ReadOnlyItem<string>(this, "drone_serial");
                SoftBuildDate = new ReadOnlyItem<string>(this, "soft_build_date");
                Motor1Soft = new ReadOnlyItem<string>(this, "motor1_soft");
                Motor1Hard = new ReadOnlyItem<string>(this, "motor1_hard");
                Motor1Supplier = new ReadOnlyItem<string>(this, "motor1_supplier");
                Motor2Soft = new ReadOnlyItem<string>(this, "motor2_soft");
                Motor2Hard = new ReadOnlyItem<string>(this, "motor2_hard");
                Motor2Supplier = new ReadOnlyItem<string>(this, "motor2_supplier");
                Motor3Soft = new ReadOnlyItem<string>(this, "motor3_soft");
                Motor3Hard = new ReadOnlyItem<string>(this, "motor3_hard");
                Motor3Supplier = new ReadOnlyItem<string>(this, "motor3_supplier");
                Motor4Soft = new ReadOnlyItem<string>(this, "motor4_soft");
                Motor4Hard = new ReadOnlyItem<string>(this, "motor4_hard");
                Motor4Supplier = new ReadOnlyItem<string>(this, "motor4_supplier");
                ARDroneName = new ActiveItem<string>(this, "ardrone_name");
                FlyingTime = new ReadOnlyItem<int>(this, "flying_time");
                NavdataDemo = new ActiveItem<bool>(this, "navdata_demo");
                NavdataOptions = new ActiveItem<int>(this, "navdata_options");
                ComWatchdog = new ActiveItem<int>(this, "com_watchdog");
                Video = new ActiveItem<bool>(this, "video_enable");
                Vision = new ActiveItem<bool>(this, "vision_enable");
                BatteryVoltageMin = new ActiveItem<int>(this, "vbat_min");
                LocalTime = new ActiveItem<int>(this, "localtime");
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class ControlSection : SectionBase
        {
            public readonly dynamic accs_offset;
            public readonly dynamic accs_gains;
            public readonly dynamic gyros_offset;
            public readonly dynamic gyros_gains;
            public readonly dynamic gyros110_offset;
            public readonly dynamic gyros110_gains;
            public readonly dynamic magneto_offset;
            public readonly dynamic magneto_radius;
            public readonly dynamic gyro_offset_thr_x;
            public readonly dynamic gyro_offset_thr_y;
            public readonly dynamic gyro_offset_thr_z;
            public readonly dynamic pwm_ref_gyros;
            public readonly dynamic osctun_value;
            public readonly dynamic osctun_test;
            public readonly dynamic control_level;
            public readonly dynamic euler_angle_max;
            public readonly dynamic altitude_max;
            public readonly dynamic altitude_min;
            public readonly dynamic control_iphone_tilt;
            public readonly dynamic control_vz_max;
            public readonly dynamic control_yaw;
            public readonly dynamic outdoor;
            public readonly dynamic flight_without_shell;
            [Obsolete] public readonly dynamic autonomous_flight;
            public readonly dynamic manual_trim;
            public readonly dynamic indoor_euler_angle_max;
            public readonly dynamic indoor_control_vz_max;
            public readonly dynamic indoor_control_yaw;
            public readonly dynamic outdoor_euler_angle_max;
            public readonly dynamic outdoor_control_vz_max;
            public readonly dynamic outdoor_control_yaw;
            public readonly dynamic flying_mode;
            public readonly dynamic hovering_range;
            public readonly dynamic flight_anim;

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
                autonomous_flight = new ReadOnlyItem<bool>(this, "autonomous_flight");
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

        [StructLayout(LayoutKind.Sequential)]
        public class NetworkSection : SectionBase
        {
            public readonly dynamic SsidSinglePlayer;
            public readonly dynamic SsidMultiPlayer;
            public readonly dynamic WifiMode;
            public readonly dynamic WifiRate;
            public readonly dynamic OwnerMac;

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

        [StructLayout(LayoutKind.Sequential)]
        public class PicSection : SectionBase
        {
            public readonly dynamic UltrasoundFreq;
            public readonly dynamic UltrasoundWatchdog;
            public readonly dynamic Version;

            public PicSection(DroneConfiguration configuration)
                : base(configuration, "pic")
            {
                UltrasoundFreq = new ActiveItem<int>(this, "ultrasound_freq");
                UltrasoundWatchdog = new ActiveItem<int>(this, "ultrasound_watchdog");
                Version = new ReadOnlyItem<int>(this, "pic_version");
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class VideoSection : SectionBase
        {
            public readonly dynamic CamifFps;
            public readonly dynamic CodecFps;
            public readonly dynamic CamifBuffers;
            public readonly dynamic Trackers;
            public readonly dynamic Codec;
            public readonly dynamic Slices;
            public readonly dynamic LiveSocket;
            public readonly dynamic StorageSpace;
            public readonly dynamic Bitrate;
            public readonly dynamic MaxBitrate;
            public readonly dynamic BitrateCtrlMode;
            public readonly dynamic BitrateStorage;
            public readonly dynamic Channel;
            public readonly dynamic OnUsb;
            public readonly dynamic FileIndex;

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

        [StructLayout(LayoutKind.Sequential)]
        public class LedsSection : SectionBase
        {
            public readonly dynamic Animation;

            public LedsSection(DroneConfiguration configuration)
                : base(configuration, "leds")
            {
                Animation = new ActiveItem<string>(this, "leds_anim");
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class DetectSection : SectionBase
        {
            public readonly dynamic EnemyColors;
            public readonly dynamic GroundstripeColors;
            public readonly dynamic EnemyWithoutShell;
            public readonly dynamic DetectType;
            public readonly dynamic DetectionsSelectH;
            public readonly dynamic DetectionsSelectVHsync;
            public readonly dynamic DetectionsSelectV;

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

        [StructLayout(LayoutKind.Sequential)]
        public class SyslogSection : SectionBase
        {
            public readonly dynamic Output;
            public readonly dynamic MaxSize;
            public readonly dynamic NbFiles;

            public SyslogSection(DroneConfiguration configuration)
                : base(configuration, "syslog")
            {
                Output = new ActiveItem<int>(this, "output");
                MaxSize = new ActiveItem<int>(this, "max_size");
                NbFiles = new ActiveItem<int>(this, "nb_files");
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class UserboxSection : SectionBase
        {
            public readonly dynamic UserboxCmd;

            public UserboxSection(DroneConfiguration configuration)
                : base(configuration, "userbox")
            {
                UserboxCmd = new ActiveItem<string>(this, "userbox:userbox_cmd");
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class GpsSection : SectionBase
        {
            public readonly dynamic Latitude;
            public readonly dynamic Longitude;
            public readonly dynamic Altitude;

            public GpsSection(DroneConfiguration configuration)
                : base(configuration, "gps")
            {
                Latitude = new ReadOnlyItem<double>(this, "latitude");
                Longitude = new ReadOnlyItem<double>(this, "longitude");
                Altitude = new ReadOnlyItem<double>(this, "altitude");
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class CustomSection : SectionBase
        {
            public readonly dynamic ApplicationId;
            public readonly dynamic ApplicationDescription;
            public readonly dynamic ProfileId;
            public readonly dynamic ProfileDescription;
            public readonly dynamic SessionId;
            public readonly dynamic SessionDescription;

            public CustomSection(DroneConfiguration configuration)
                : base(configuration, "custom")
            {
                ApplicationId = new ReadOnlyItem<string>(this, "custom:application_id");
                ApplicationDescription = new ReadOnlyItem<string>(this, "custom:application_desc");
                ProfileId = new ReadOnlyItem<string>(this, "custom:profile_id");
                ProfileDescription = new ReadOnlyItem<string>(this, "custom:profile_desc");
                SessionId = new ReadOnlyItem<string>(this, "custom:session_id");
                SessionDescription = new ReadOnlyItem<string>(this, "custom:session_desc");
            }
        }

        public void Enqueue(ATCommand command)
        {
            _queue.Enqueue(command);
        }

        public void SendTo(DroneClient client)
        {
            ATCommand command;
            while (_queue.TryDequeue(out command))
            {
                client.Send(command);
            }
        }
    }
}