using System.Runtime.InteropServices;
using AR.Drone.NativeApi.Math;

namespace AR.Drone.NativeApi.Navdata
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct NavdataDemo
    {
        public NavdataTag tag;
        public ushort size;

        public uint ctrl_state; // todo: flying state (landed, flying, hovering, etc.) defined in CTRL_STATES enum.
        public uint vbat_flying_percentage; // battery voltage filtered (mV)

        public float theta; // UAV's pitch in milli-degrees
        public float phi; // UAV's roll in milli-degrees
        public float psi; // UAV's yaw in milli-degrees

        public int altitude; // UAV's altitude in centimeters

        public float vx; // UAV's estimated linear velocity
        public float vy; // UAV's estimated linear velocity
        public float vz; // UAV's estimated linear velocity

        public uint num_frames; // streamed frame index - Not used -> To integrate in video stage.

        public Matrix33 detection_camera_rot; // Deprecated! Don't use!
        public Vector3 detection_camera_trans; // Deprecated! Don't use!
        public uint detection_tag_index; // Deprecated! Don't use!

        public uint detection_camera_type; // Type of tag searched in detection

        public Matrix33 drone_camera_rot; // Deprecated! Don't use!
        public Vector3 drone_camera_trans; // Deprecated! Don't use!
    }

    public enum CTRL_STATES
    {
        CTRL_DEFAULT,
        CTRL_INIT,
        CTRL_LANDED,
        CTRL_FLYING,
        CTRL_HOVERING,
        CTRL_TEST,
        CTRL_TRANS_TAKEOFF,
        CTRL_TRANS_GOTOFIX,
        CTRL_TRANS_LANDING,
        CTRL_STATES
    }
}