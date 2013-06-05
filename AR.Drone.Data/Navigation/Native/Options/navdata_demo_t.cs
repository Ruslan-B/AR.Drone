using System.Runtime.InteropServices;
using AR.Drone.Data.Navigation.Native.Math;

namespace AR.Drone.Data.Navigation.Native.Options
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct navdata_demo_t
    {
        public ushort tag;
        public ushort size;
        public uint ctrl_state; // flying state (landed, flying, hovering, etc.) defined in CTRL_STATES and FLYING_STATES enum.
        public uint vbat_flying_percentage; // battery voltage filtered (mV)
        public float theta; // UAV's pitch in milli-degrees
        public float phi; // UAV's roll in milli-degrees
        public float psi; // UAV's yaw in milli-degrees
        public int altitude; // UAV's altitude in centimeters
        public float vx; // UAV's estimated linear velocity
        public float vy; // UAV's estimated linear velocity
        public float vz; // UAV's estimated linear velocity
        public uint num_frames; // streamed frame index - Not used -> To integrate in video stage.
        public matrix33_t detection_camera_rot; // Deprecated! Don't use!
        public vector31_t detection_camera_trans; // Deprecated! Don't use!
        public uint detection_tag_index; // Deprecated! Don't use!
        public uint detection_camera_type; // Type of tag searched in detection
        public matrix33_t drone_camera_rot; // Deprecated! Don't use!
        public vector31_t drone_camera_trans; // Deprecated! Don't use!
    }
}