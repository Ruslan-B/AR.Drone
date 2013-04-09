using System.Runtime.InteropServices;

namespace AR.Drone.Client.Navigation
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NavigationData
    {
        public NavigationState State;
        public float Yaw; // psi - Z
        public float Pitch; // theta - Y
        public float Roll; // phi - X
        public float Altitude;
        public Vector3 Velocity;
        public Battery Battery;
        public uint Time;
        public Wifi Wifi;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Wifi
    {
        public float LinkQuality;

        public override string ToString()
        {
            return string.Format("{{LinkQuality:{0}}}", LinkQuality);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Battery
    {
        public bool Low;
        public float Percentage;
        public float Voltage;


        public override string ToString()
        {
            return string.Format("{{Low:{0} Percentage:{1} Voltage:{2}}}", Low, Percentage, Voltage);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3
    {
        public float X;
        public float Y;
        public float Z;


        public override string ToString()
        {
            return string.Format("{{X:{0} Y:{1} Z:{2}}}", X, Y, Z);
        }
    }
}