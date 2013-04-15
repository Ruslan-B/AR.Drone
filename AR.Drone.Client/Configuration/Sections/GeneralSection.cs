using System.Runtime.InteropServices;

namespace AR.Drone.Client.Configuration.Sections
{
    [StructLayout(LayoutKind.Sequential)]
    public class GeneralSection
    {
        public readonly ReadOnlyItem<int> ConfigVersion = new ReadOnlyItem<int>("general:num_version_config");
        public readonly ReadOnlyItem<int> MotherboardVersion = new ReadOnlyItem<int>("general:num_version_mb");
        public readonly ReadOnlyItem<string> SoftVersion = new ReadOnlyItem<string>("general:num_version_soft");
        public readonly ReadOnlyItem<string> DroneSerial = new ReadOnlyItem<string>("general:drone_serial");
        public readonly ReadOnlyItem<string> SoftBuildDate = new ReadOnlyItem<string>("general:soft_build_date");
        public readonly ReadOnlyItem<string> Motor1Soft = new ReadOnlyItem<string>("general:motor1_soft");
        public readonly ReadOnlyItem<string> Motor1Hard = new ReadOnlyItem<string>("general:motor1_hard");
        public readonly ReadOnlyItem<string> Motor1Supplier = new ReadOnlyItem<string>("general:motor1_supplier");
        public readonly ReadOnlyItem<string> Motor2Soft = new ReadOnlyItem<string>("general:motor2_soft");
        public readonly ReadOnlyItem<string> Motor2Hard = new ReadOnlyItem<string>("general:motor2_hard");
        public readonly ReadOnlyItem<string> Motor2Supplier = new ReadOnlyItem<string>("general:motor2_supplier");
        public readonly ReadOnlyItem<string> Motor3Soft = new ReadOnlyItem<string>("general:motor3_soft");
        public readonly ReadOnlyItem<string> Motor3Hard = new ReadOnlyItem<string>("general:motor3_hard");
        public readonly ReadOnlyItem<string> Motor3Supplier = new ReadOnlyItem<string>("general:motor3_supplier");
        public readonly ReadOnlyItem<string> Motor4Soft = new ReadOnlyItem<string>("general:motor4_soft");
        public readonly ReadOnlyItem<string> Motor4Hard = new ReadOnlyItem<string>("general:motor4_hard");
        public readonly ReadOnlyItem<string> Motor4Supplier = new ReadOnlyItem<string>("general:motor4_supplier");
        public readonly ReadWriteItem<string> ARDroneName = new ReadWriteItem<string>("general:ardrone_name");
        public readonly ReadOnlyItem<int> FlyingTime = new ReadOnlyItem<int>("general:flying_time");
        public readonly ReadWriteItem<bool> NavdataDemo = new ReadWriteItem<bool>("general:navdata_demo");
        public readonly ReadWriteItem<int> NavdataOptions = new ReadWriteItem<int>("general:navdata_options");
        public readonly ReadWriteItem<int> ComWatchdog = new ReadWriteItem<int>("general:com_watchdog");
        public readonly ReadWriteItem<bool> Video = new ReadWriteItem<bool>("general:video_enable");
        public readonly ReadWriteItem<bool> Vision = new ReadWriteItem<bool>("general:vision_enable");
        public readonly ReadWriteItem<int> BatteryVoltageMin = new ReadWriteItem<int>("general:vbat_min");
        public readonly ReadWriteItem<int> LocalTime = new ReadWriteItem<int>("general:localtime");
    }
}