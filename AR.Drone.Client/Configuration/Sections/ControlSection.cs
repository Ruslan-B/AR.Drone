using System;
using System.Runtime.InteropServices;

namespace AR.Drone.Client.Configuration.Sections
{
    [StructLayout(LayoutKind.Sequential)]
    public class ControlSection
    {
        public readonly ReadOnlyItem<string> accs_offset = new ReadOnlyItem<string>("control:accs_offset");
        public readonly ReadOnlyItem<string> accs_gains = new ReadOnlyItem<string>("control:accs_gains");
        public readonly ReadOnlyItem<string> gyros_offset = new ReadOnlyItem<string>("control:gyros_offset");
        public readonly ReadOnlyItem<string> gyros_gains = new ReadOnlyItem<string>("control:gyros_gains");
        public readonly ReadOnlyItem<string> gyros110_offset = new ReadOnlyItem<string>("control:gyros110_offset");
        public readonly ReadOnlyItem<string> gyros110_gains = new ReadOnlyItem<string>("control:gyros110_gains");
        public readonly ReadOnlyItem<string> magneto_offset = new ReadOnlyItem<string>("control:magneto_offset");
        public readonly ReadOnlyItem<float> magneto_radius = new ReadOnlyItem<float>("control:magneto_radius");
        public readonly ReadOnlyItem<float> gyro_offset_thr_x = new ReadOnlyItem<float>("control:gyro_offset_thr_x");
        public readonly ReadOnlyItem<float> gyro_offset_thr_y = new ReadOnlyItem<float>("control:gyro_offset_thr_y");
        public readonly ReadOnlyItem<float> gyro_offset_thr_z = new ReadOnlyItem<float>("control:gyro_offset_thr_z");
        public readonly ReadOnlyItem<int> pwm_ref_gyros = new ReadOnlyItem<int>("control:pwm_ref_gyros");
        public readonly ReadOnlyItem<int> osctun_value = new ReadOnlyItem<int>("control:osctun_value");
        public readonly ReadOnlyItem<bool> osctun_test = new ReadOnlyItem<bool>("control:osctun_test");
        public readonly ReadWriteItem<int> control_level = new ReadWriteItem<int>("control:control_level");
        public readonly ReadWriteItem<float> euler_angle_max = new ReadWriteItem<float>("control:euler_angle_max");
        public readonly ReadWriteItem<int> altitude_max = new ReadWriteItem<int>("control:altitude_max");
        public readonly ReadWriteItem<int> altitude_min = new ReadWriteItem<int>("control:altitude_min");
        public readonly ReadWriteItem<float> control_iphone_tilt = new ReadWriteItem<float>("control:control_iphone_tilt");
        public readonly ReadWriteItem<float> control_vz_max = new ReadWriteItem<float>("control:control_vz_max");
        public readonly ReadWriteItem<float> control_yaw = new ReadWriteItem<float>("control:control_yaw");
        public readonly ReadWriteItem<bool> outdoor = new ReadWriteItem<bool>("control:outdoor");
        public readonly ReadWriteItem<bool> flight_without_shell = new ReadWriteItem<bool>("control:flight_without_shell");
        [Obsolete] public readonly ReadOnlyItem<bool> autonomous_flight = new ReadOnlyItem<bool>("control:autonomous_flight");
        public readonly ReadWriteItem<bool> manual_trim = new ReadWriteItem<bool>("control:manual_trim");
        public readonly ReadWriteItem<float> indoor_euler_angle_max = new ReadWriteItem<float>("control:indoor_euler_angle_max");
        public readonly ReadWriteItem<float> indoor_control_vz_max = new ReadWriteItem<float>("control:indoor_control_vz_max");
        public readonly ReadWriteItem<float> indoor_control_yaw = new ReadWriteItem<float>("control:indoor_control_yaw");
        public readonly ReadWriteItem<float> outdoor_euler_angle_max = new ReadWriteItem<float>("control:outdoor_euler_angle_max");
        public readonly ReadWriteItem<float> outdoor_control_vz_max = new ReadWriteItem<float>("control:outdoor_control_vz_max");
        public readonly ReadWriteItem<float> outdoor_control_yaw = new ReadWriteItem<float>("control:outdoor_control_yaw");
        public readonly ReadWriteItem<int> flying_mode = new ReadWriteItem<int>("control:flying_mode");
        public readonly ReadWriteItem<int> hovering_range = new ReadWriteItem<int>("control:hovering_range");
        public readonly ReadWriteItem<string> flight_anim = new ReadWriteItem<string>("control:flight_anim");
    }
}