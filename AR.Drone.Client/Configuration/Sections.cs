
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AR.Drone.Client.Configuration
{

	public class GeneralSection : SectionBase
	{
		public GeneralSection(DroneConfiguration configuration) : base(configuration, "general") {}

		public Int32 ConfigVersion
		{
			get { return GetInt32("num_version_config"); }
		}

		public Int32 MotherboardVersion
		{
			get { return GetInt32("num_version_mb"); }
		}

		public String SoftVersion
		{
			get { return GetString("num_version_soft"); }
		}

		public String DroneSerial
		{
			get { return GetString("drone_serial"); }
		}

		public String SoftBuildDate
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

		public Int32 NavdataOptions
		{
			get { return GetInt32("navdata_options"); }
			set { Set("navdata_options", value); }
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
		public ControlSection(DroneConfiguration configuration) : base(configuration, "control") {}

		public String accs_offset
		{
			get { return GetString("accs_offset"); }
		}

		public String accs_gains
		{
			get { return GetString("accs_gains"); }
		}

		public String gyros_offset
		{
			get { return GetString("gyros_offset"); }
		}

		public String gyros_gains
		{
			get { return GetString("gyros_gains"); }
		}

		public String gyros110_offset
		{
			get { return GetString("gyros110_offset"); }
		}

		public String gyros110_gains
		{
			get { return GetString("gyros110_gains"); }
		}

		public String magneto_offset
		{
			get { return GetString("magneto_offset"); }
		}

		public Single magneto_radius
		{
			get { return GetSingle("magneto_radius"); }
		}

		public Single gyro_offset_thr_x
		{
			get { return GetSingle("gyro_offset_thr_x"); }
		}

		public Single gyro_offset_thr_y
		{
			get { return GetSingle("gyro_offset_thr_y"); }
		}

		public Single gyro_offset_thr_z
		{
			get { return GetSingle("gyro_offset_thr_z"); }
		}

		public Int32 pwm_ref_gyros
		{
			get { return GetInt32("pwm_ref_gyros"); }
		}

		public Int32 osctun_value
		{
			get { return GetInt32("osctun_value"); }
		}

		public Boolean osctun_test
		{
			get { return GetBoolean("osctun_test"); }
		}

		public Int32 control_level
		{
			get { return GetInt32("control_level"); }
			set { Set("control_level", value); }
		}

		public Single euler_angle_max
		{
			get { return GetSingle("euler_angle_max"); }
			set { Set("euler_angle_max", value); }
		}

		public Int32 altitude_max
		{
			get { return GetInt32("altitude_max"); }
			set { Set("altitude_max", value); }
		}

		public Int32 altitude_min
		{
			get { return GetInt32("altitude_min"); }
			set { Set("altitude_min", value); }
		}

		public Single control_iphone_tilt
		{
			get { return GetSingle("control_iphone_tilt"); }
			set { Set("control_iphone_tilt", value); }
		}

		public Single control_vz_max
		{
			get { return GetSingle("control_vz_max"); }
			set { Set("control_vz_max", value); }
		}

		public Single control_yaw
		{
			get { return GetSingle("control_yaw"); }
			set { Set("control_yaw", value); }
		}

		public Boolean outdoor
		{
			get { return GetBoolean("outdoor"); }
			set { Set("outdoor", value); }
		}

		public Boolean flight_without_shell
		{
			get { return GetBoolean("flight_without_shell"); }
			set { Set("flight_without_shell", value); }
		}

		public Boolean autonomous_flight
		{
			get { return GetBoolean("autonomous_flight"); }
		}

		public Boolean manual_trim
		{
			get { return GetBoolean("manual_trim"); }
			set { Set("manual_trim", value); }
		}

		public Single indoor_euler_angle_max
		{
			get { return GetSingle("indoor_euler_angle_max"); }
			set { Set("indoor_euler_angle_max", value); }
		}

		public Single indoor_control_vz_max
		{
			get { return GetSingle("indoor_control_vz_max"); }
			set { Set("indoor_control_vz_max", value); }
		}

		public Single indoor_control_yaw
		{
			get { return GetSingle("indoor_control_yaw"); }
			set { Set("indoor_control_yaw", value); }
		}

		public Single outdoor_euler_angle_max
		{
			get { return GetSingle("outdoor_euler_angle_max"); }
			set { Set("outdoor_euler_angle_max", value); }
		}

		public Single outdoor_control_vz_max
		{
			get { return GetSingle("outdoor_control_vz_max"); }
			set { Set("outdoor_control_vz_max", value); }
		}

		public Single outdoor_control_yaw
		{
			get { return GetSingle("outdoor_control_yaw"); }
			set { Set("outdoor_control_yaw", value); }
		}

		public Int32 flying_mode
		{
			get { return GetInt32("flying_mode"); }
			set { Set("flying_mode", value); }
		}

		public Int32 hovering_range
		{
			get { return GetInt32("hovering_range"); }
			set { Set("hovering_range", value); }
		}
	}

	public class NetworkSection : SectionBase
	{
		public NetworkSection(DroneConfiguration configuration) : base(configuration, "network") {}

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
		public PicSection(DroneConfiguration configuration) : base(configuration, "pic") {}

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
		public VideoSection(DroneConfiguration configuration) : base(configuration, "video") {}

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
		public LedsSection(DroneConfiguration configuration) : base(configuration, "leds") {}

		public String Animation
		{
			get { return GetString("leds_anim"); }
			set { Set("leds_anim", value); }
		}
	}

	public class DetectSection : SectionBase
	{
		public DetectSection(DroneConfiguration configuration) : base(configuration, "detect") {}

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

		public Int32 DetectType
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
		public SyslogSection(DroneConfiguration configuration) : base(configuration, "syslog") {}

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
		public UserboxSection(DroneConfiguration configuration) : base(configuration, "userbox") {}

		public String UserboxCmd
		{
			get { return GetString("userbox_cmd"); }
			set { Set("userbox_cmd", value); }
		}
	}

	public class GpsSection : SectionBase
	{
		public GpsSection(DroneConfiguration configuration) : base(configuration, "gps") {}

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
		public CustomSection(DroneConfiguration configuration) : base(configuration, "custom") {}

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
