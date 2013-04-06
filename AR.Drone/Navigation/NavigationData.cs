using System.Runtime.InteropServices;

namespace AR.Drone.Navigation
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NavigationData
    {
        public DroneState State;
        public float Yaw; // psi
        public float Pitch; // theta
        public float Roll; // phi
        public float Altitude;
        public Vector3 Velocity;
        public Battery Battery;
        public Wifi Wifi;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Wifi
    {
        public float LinkQuality;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Battery
    {
        public bool Low;
        public float Percentage;
        public float Voltage;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3
    {
        public float X;
        public float Y;
        public float Z;
    }
}