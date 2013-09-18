using System;

namespace AR.Drone.Client.Configuration
{
    [Flags]
    public enum NavdataOptions
    {
        None = 0,
        Demo = 1 << 0,
        Time = 1 << 1,
        RawMeasures = 1 << 2,
        PhysMeasures = 1 << 3,
        GyrosOffsets = 1 << 4,
        EulerAngles = 1 << 5,
        References = 1 << 6,
        Trims = 1 << 7,
        RcReferences = 1 << 8,
        Pwm = 1 << 9,
        Altitude = 1 << 10,
        VisionRaw = 1 << 1,
        VisionOf = 1 << 12,
        Vision = 1 << 13,
        VisionPerf = 1 << 14,
        TrackersSend = 1 << 15,
        VisionDetect = 1 << 16,
        Watchdog = 1 << 17,
        AdcDataFrame = 1 << 18,
        VideoStream = 1 << 19,
        Games = 1 << 20,
        PressureRaw = 1 << 21,
        Magneto = 1 << 22,
        Wind = 1 << 23,
        KalmanPressure = 1 << 24,
        HDVideoStream = 1 << 25,
        WiFi = 1 << 26,
        Zimmu3000 = 1 << 27,
        Nums = 1 << 28,
        All = (1 << 29) - 1
    }
}
