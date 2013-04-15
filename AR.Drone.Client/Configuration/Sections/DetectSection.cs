using System.Runtime.InteropServices;

namespace AR.Drone.Client.Configuration.Sections
{
    [StructLayout(LayoutKind.Sequential)]
    public class DetectSection
    {
        public readonly ReadWriteItem<int> EnemyColors = new ReadWriteItem<int>("detect:enemy_colors");
        public readonly ReadWriteItem<int> GroundstripeColors = new ReadWriteItem<int>("detect:groundstripe_colors");
        public readonly ReadWriteItem<int> EnemyWithoutShell = new ReadWriteItem<int>("detect:enemy_without_shell");
        public readonly ReadWriteItem<int> DetectType = new ReadWriteItem<int>("detect:detect_type");
        public readonly ReadWriteItem<int> DetectionsSelectH = new ReadWriteItem<int>("detect:detections_select_h");
        public readonly ReadWriteItem<int> DetectionsSelectVHsync = new ReadWriteItem<int>("detect:detections_select_v_hsync");
        public readonly ReadWriteItem<int> DetectionsSelectV = new ReadWriteItem<int>("detect:detections_select_v");
    }
}