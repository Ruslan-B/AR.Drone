using System.Runtime.InteropServices;
using AR.Drone.Data.Navigation.Native.Options;

namespace AR.Drone.Data.Navigation.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NavdataBag
    {
        public uint ardrone_state;
        public navdata_demo_t demo;
        public navdata_time_t time;
        public navdata_raw_measures_t raw_measures;
        public navdata_phys_measures_t phys_measures;
        public navdata_gyros_offsets_t gyros_offsets;
        public navdata_euler_angles_t euler_angles;
        public navdata_references_t references;
        public navdata_trims_t trims;
        public navdata_rc_references_t rc_references;
        public navdata_pwm_t pwm;
        public navdata_altitude_t altitude;
        public navdata_vision_raw_t vision_raw;
        public navdata_vision_of_t vision_of_tag;
        public navdata_vision_t vision;
        public navdata_vision_perf_t vision_perf;
        public navdata_trackers_send_t trackers_send;
        public navdata_vision_detect_t vision_detect;
        public navdata_watchdog_t watchdog;
        public navdata_adc_data_frame_t adc_data_frame;
        public navdata_video_stream_t video_stream;
        public navdata_games_t games;
        public navdata_pressure_raw_t pressure_raw;
        public navdata_magneto_t magneto;
        public navdata_wind_speed_t wind_speed;
        public navdata_kalman_pressure_t kalman_pressure;
        public navdata_hdvideo_stream_t hdvideo_stream;
        public navdata_wifi_t wifi;
        public navdata_cks_t cks;
    }
}