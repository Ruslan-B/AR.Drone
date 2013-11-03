using System;

namespace AR.Drone.Client.Configuration
{

    public class GeneralSection : SectionBase
    {
        public GeneralSection(Settings settings) : base(settings, "general")
        {
        }

        public Int32 ConfigurationVersion
        {
            get { return GetInt32("num_version_config"); }
        }

        public Int32 HardwareVersion
        {
            get { return GetInt32("num_version_mb"); }
        }

        public String FirmwareVersion
        {
            get { return GetString("num_version_soft"); }
        }

        public String DroneSerial
        {
            get { return GetString("drone_serial"); }
        }

        public String FirmwareBuildDate
        {
            get { return GetString("soft_build_date"); }
        }

        public String Motor1Soft
        {
            get { return GetString("motor1_soft"); }
        }

        public String Motor1Hard
        {
            get { return GetString("motor1_hard"); }
        }

        public String Motor1Supplier
        {
            get { return GetString("motor1_supplier"); }
        }

        public String Motor2Soft
        {
            get { return GetString("motor2_soft"); }
        }

        public String Motor2Hard
        {
            get { return GetString("motor2_hard"); }
        }

        public String Motor2Supplier
        {
            get { return GetString("motor2_supplier"); }
        }

        public String Motor3Soft
        {
            get { return GetString("motor3_soft"); }
        }

        public String Motor3Hard
        {
            get { return GetString("motor3_hard"); }
        }

        public String Motor3Supplier
        {
            get { return GetString("motor3_supplier"); }
        }

        public String Motor4Soft
        {
            get { return GetString("motor4_soft"); }
        }

        public String Motor4Hard
        {
            get { return GetString("motor4_hard"); }
        }

        public String Motor4Supplier
        {
            get { return GetString("motor4_supplier"); }
        }

        public String ARDroneName
        {
            get { return GetString("ardrone_name"); }
            set { Set("ardrone_name", value); }
        }

        public Int32 FlyingTime
        {
            get { return GetInt32("flying_time"); }
        }

        public Boolean NavdataDemo
        {
            get { return GetBoolean("navdata_demo"); }
            set { Set("navdata_demo", value); }
        }

        public NavdataOptions NavdataOptions
        {
            get { return GetEnum<NavdataOptions>("navdata_options"); }
            set { SetEnum<NavdataOptions>("navdata_options", value); }
        }

        public Int32 ComWatchdog
        {
            get { return GetInt32("com_watchdog"); }
            set { Set("com_watchdog", value); }
        }

        public Boolean Video
        {
            get { return GetBoolean("video_enable"); }
            set { Set("video_enable", value); }
        }

        public Boolean Vision
        {
            get { return GetBoolean("vision_enable"); }
            set { Set("vision_enable", value); }
        }

        public Int32 BatteryVoltageMin
        {
            get { return GetInt32("vbat_min"); }
            set { Set("vbat_min", value); }
        }

        public Int32 LocalTime
        {
            get { return GetInt32("localtime"); }
            set { Set("localtime", value); }
        }
    }

    public class ControlSection : SectionBase
    {
        public ControlSection(Settings settings) : base(settings, "control")
        {
        }

        public String AccsOffset
        {
            get { return GetString("accs_offset"); }
        }

        public String AccsGains
        {
            get { return GetString("accs_gains"); }
        }

        public String GyrosOffset
        {
            get { return GetString("gyros_offset"); }
        }

        public String GyrosGains
        {
            get { return GetString("gyros_gains"); }
        }

        public String Gyros110Offset
        {
            get { return GetString("gyros110_offset"); }
        }

        public String Gyros110Gains
        {
            get { return GetString("gyros110_gains"); }
        }

        public String MagnetoOffset
        {
            get { return GetString("magneto_offset"); }
        }

        public Single MagnetoRadius
        {
            get { return GetSingle("magneto_radius"); }
        }

        public Single GyroOffsetThrX
        {
            get { return GetSingle("gyro_offset_thr_x"); }
        }

        public Single GyroOffsetThrY
        {
            get { return GetSingle("gyro_offset_thr_y"); }
        }

        public Single GyroOffsetThrZ
        {
            get { return GetSingle("gyro_offset_thr_z"); }
        }

        public Int32 PwmRefGyros
        {
            get { return GetInt32("pwm_ref_gyros"); }
        }

        public Int32 OsctunValue
        {
            get { return GetInt32("osctun_value"); }
        }

        public Boolean OsctunTest
        {
            get { return GetBoolean("osctun_test"); }
        }

        public Int32 ControlLevel
        {
            get { return GetInt32("control_level"); }
            set { Set("control_level", value); }
        }

        public Single EulerAngleMax
        {
            get { return GetSingle("euler_angle_max"); }
            set { Set("euler_angle_max", value); }
        }

        public Int32 AltitudeMax
        {
            get { return GetInt32("altitude_max"); }
            set { Set("altitude_max", value); }
        }

        public Int32 AltitudeMin
        {
            get { return GetInt32("altitude_min"); }
            set { Set("altitude_min", value); }
        }

        public Single ControliPhoneTilt
        {
            get { return GetSingle("control_iphone_tilt"); }
            set { Set("control_iphone_tilt", value); }
        }

        public Single ControlVzMax
        {
            get { return GetSingle("control_vz_max"); }
            set { Set("control_vz_max", value); }
        }

        public Single ControlYaw
        {
            get { return GetSingle("control_yaw"); }
            set { Set("control_yaw", value); }
        }

        public Boolean Outdoor
        {
            get { return GetBoolean("outdoor"); }
            set { Set("outdoor", value); }
        }

        public Boolean FlightWithoutShell
        {
            get { return GetBoolean("flight_without_shell"); }
            set { Set("flight_without_shell", value); }
        }

        public Boolean AutonomousFlight
        {
            get { return GetBoolean("autonomous_flight"); }
        }

        public Boolean ManualTrim
        {
            get { return GetBoolean("manual_trim"); }
            set { Set("manual_trim", value); }
        }

        public Single IndoorEulerAngleMax
        {
            get { return GetSingle("indoor_euler_angle_max"); }
            set { Set("indoor_euler_angle_max", value); }
        }

        public Single IndoorControlVzMax
        {
            get { return GetSingle("indoor_control_vz_max"); }
            set { Set("indoor_control_vz_max", value); }
        }

        public Single IndoorControlYaw
        {
            get { return GetSingle("indoor_control_yaw"); }
            set { Set("indoor_control_yaw", value); }
        }

        public Single OutdoorEulerAngleMax
        {
            get { return GetSingle("outdoor_euler_angle_max"); }
            set { Set("outdoor_euler_angle_max", value); }
        }

        public Single OutdoorControlVzMax
        {
            get { return GetSingle("outdoor_control_vz_max"); }
            set { Set("outdoor_control_vz_max", value); }
        }

        public Single OutdoorControlYaw
        {
            get { return GetSingle("outdoor_control_yaw"); }
            set { Set("outdoor_control_yaw", value); }
        }

        public Int32 FlyingMode
        {
            get { return GetInt32("flying_mode"); }
            set { Set("flying_mode", value); }
        }

        public Int32 HoveringRange
        {
            get { return GetInt32("hovering_range"); }
            set { Set("hovering_range", value); }
        }

        public FlightAnimation FlightAnimation
        {
            get { return GetFlightAnimation("flight_anim"); }
            set { Set("flight_anim", value); }
        }
    }

    public class NetworkSection : SectionBase
    {
        public NetworkSection(Settings settings) : base(settings, "network")
        {
        }

        public String SsidSinglePlayer
        {
            get { return GetString("ssid_single_player"); }
            set { Set("ssid_single_player", value); }
        }

        public String SsidMultiPlayer
        {
            get { return GetString("ssid_multi_player"); }
            set { Set("ssid_multi_player", value); }
        }

        public Int32 WifiMode
        {
            get { return GetInt32("wifi_mode"); }
            set { Set("wifi_mode", value); }
        }

        public Int32 WifiRate
        {
            get { return GetInt32("wifi_rate"); }
            set { Set("wifi_rate", value); }
        }

        public String OwnerMac
        {
            get { return GetString("owner_mac"); }
            set { Set("owner_mac", value); }
        }
    }

    public class PicSection : SectionBase
    {
        public PicSection(Settings settings) : base(settings, "pic")
        {
        }

        public Int32 UltrasoundFreq
        {
            get { return GetInt32("ultrasound_freq"); }
            set { Set("ultrasound_freq", value); }
        }

        public Int32 UltrasoundWatchdog
        {
            get { return GetInt32("ultrasound_watchdog"); }
            set { Set("ultrasound_watchdog", value); }
        }

        public Int32 Version
        {
            get { return GetInt32("pic_version"); }
        }
    }

    public class VideoSection : SectionBase
    {
        public VideoSection(Settings settings) : base(settings, "video")
        {
        }

        public Int32 CamifFps
        {
            get { return GetInt32("camif_fps"); }
        }

        public Int32 CodecFps
        {
            get { return GetInt32("codec_fps"); }
            set { Set("codec_fps", value); }
        }

        public Int32 CamifBuffers
        {
            get { return GetInt32("camif_buffers"); }
        }

        public Int32 Trackers
        {
            get { return GetInt32("num_trackers"); }
        }

        public VideoCodecType Codec
        {
            get { return GetEnum<VideoCodecType>("video_codec"); }
            set { SetEnum<VideoCodecType>("video_codec", value); }
        }

        public Int32 Slices
        {
            get { return GetInt32("video_slices"); }
            set { Set("video_slices", value); }
        }

        public Int32 LiveSocket
        {
            get { return GetInt32("video_live_socket"); }
            set { Set("video_live_socket", value); }
        }

        public Int32 StorageSpace
        {
            get { return GetInt32("video_storage_space"); }
        }

        public Int32 Bitrate
        {
            get { return GetInt32("bitrate"); }
            set { Set("bitrate", value); }
        }

        public Int32 MaxBitrate
        {
            get { return GetInt32("max_bitrate"); }
            set { Set("max_bitrate", value); }
        }

        public VideoBitrateControlMode BitrateCtrlMode
        {
            get { return GetEnum<VideoBitrateControlMode>("bitrate_ctrl_mode"); }
            set { SetEnum<VideoBitrateControlMode>("bitrate_ctrl_mode", value); }
        }

        public Int32 BitrateStorage
        {
            get { return GetInt32("bitrate_storage"); }
            set { Set("bitrate_storage", value); }
        }

        public VideoChannelType Channel
        {
            get { return GetEnum<VideoChannelType>("video_channel"); }
            set { SetEnum<VideoChannelType>("video_channel", value); }
        }

        public Boolean OnUsb
        {
            get { return GetBoolean("video_on_usb"); }
            set { Set("video_on_usb", value); }
        }

        public Int32 FileIndex
        {
            get { return GetInt32("video_file_index"); }
            set { Set("video_file_index", value); }
        }
    }

    public class LedsSection : SectionBase
    {
        public LedsSection(Settings settings) : base(settings, "leds")
        {
        }

        public LedAnimation LedAnimation
        {
            get { return GetLedAnimation("leds_anim"); }
            set { Set("leds_anim", value); }
        }
    }

    public class DetectSection : SectionBase
    {
        public DetectSection(Settings settings) : base(settings, "detect")
        {
        }

        public Int32 EnemyColors
        {
            get { return GetInt32("enemy_colors"); }
            set { Set("enemy_colors", value); }
        }

        public Int32 GroundstripeColors
        {
            get { return GetInt32("groundstripe_colors"); }
            set { Set("groundstripe_colors", value); }
        }

        public Int32 EnemyWithoutShell
        {
            get { return GetInt32("enemy_without_shell"); }
            set { Set("enemy_without_shell", value); }
        }

        public Int32 Type
        {
            get { return GetInt32("detect_type"); }
            set { Set("detect_type", value); }
        }

        public Int32 DetectionsSelectH
        {
            get { return GetInt32("detections_select_h"); }
            set { Set("detections_select_h", value); }
        }

        public Int32 DetectionsSelectVHsync
        {
            get { return GetInt32("detections_select_v_hsync"); }
            set { Set("detections_select_v_hsync", value); }
        }

        public Int32 DetectionsSelectV
        {
            get { return GetInt32("detections_select_v"); }
            set { Set("detections_select_v", value); }
        }
    }

    public class SyslogSection : SectionBase
    {
        public SyslogSection(Settings settings) : base(settings, "syslog")
        {
        }

        public Int32 Output
        {
            get { return GetInt32("output"); }
            set { Set("output", value); }
        }

        public Int32 MaxSize
        {
            get { return GetInt32("max_size"); }
            set { Set("max_size", value); }
        }

        public Int32 NbFiles
        {
            get { return GetInt32("nb_files"); }
            set { Set("nb_files", value); }
        }
    }

    public class UserboxSection : SectionBase
    {
        public UserboxSection(Settings settings) : base(settings, "userbox")
        {
        }

        public UserboxCommand Command
        {
            get { return GetUserboxCommand("userbox_cmd"); }
            set { Set("userbox_cmd", value); }
        }
    }

    public class GpsSection : SectionBase
    {
        public GpsSection(Settings settings) : base(settings, "gps")
        {
        }

        public Double Latitude
        {
            get { return GetDouble("latitude"); }
        }

        public Double Longitude
        {
            get { return GetDouble("longitude"); }
        }

        public Double Altitude
        {
            get { return GetDouble("altitude"); }
        }
    }

    public class CustomSection : SectionBase
    {
        public CustomSection(Settings settings) : base(settings, "custom")
        {
        }

        public String ApplicationId
        {
            get { return GetString("application_id"); }
            set { Set("application_id", value); }
        }

        public String ApplicationDescription
        {
            get { return GetString("application_desc"); }
            set { Set("application_desc", value); }
        }

        public String ProfileId
        {
            get { return GetString("profile_id"); }
            set { Set("profile_id", value); }
        }

        public String ProfileDescription
        {
            get { return GetString("profile_desc"); }
            set { Set("profile_desc", value); }
        }

        public String SessionId
        {
            get { return GetString("session_id"); }
            set { Set("session_id", value); }
        }

        public String SessionDescription
        {
            get { return GetString("session_desc"); }
            set { Set("session_desc", value); }
        }
    }
}
