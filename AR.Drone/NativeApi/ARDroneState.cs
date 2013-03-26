using System;

namespace AR.Drone.NativeApi
{
    [Flags]
    public enum ARDroneState : uint
    {
        Flying = 1U << 0, // FLY : (0) ardrone is landed, (1) ardrone is flying
        Video = 1U << 1, // VIDEO : (0) video disable, (1) video enable
        Vision = 1U << 2, // VISION : (0) vision disable, (1) vision enable
        Control = 1U << 3, // CONTROL ALGO : (0) euler angles control, (1) angular speed control
        Altitude = 1U << 4, // ALTITUDE CONTROL ALGO : (0) altitude control inactive (1) altitude control active
        UserFeedbackStart = 1U << 5, // USER feedback : Start button state
        Command = 1U << 6, // Control command ACK : (0) None, (1) one received
        Camera = 1U << 7, // CAMERA : (0) camera not ready, (1) Camera ready
        Travelling = 1U << 8, // Travelling : (0) disable, (1) enable
        UsbKeyReady = 1U << 9, // USB key : (0) usb key not ready, (1) usb key ready
        NavdataDemo = 1U << 10, // Navdata demo : (0) All navdata, (1) only navdata demo
        NavdataBootstrap = 1U << 11, // Navdata bootstrap : (0) options sent in all or demo mode, (1) no navdata options sent
        MotorsProblem = 1U << 12, // Motors status : (0) Ok, (1) Motors problem
        CommunicationLost = 1U << 13, // Communication Lost : (1) com problem, (0) Com is ok
        SoftwareFault = 1U << 14, // Software fault detected - user should land as quick as possible (1)
        VBatLow = 1U << 15, // VBat low : (1) too low, (0) Ok
        UserEmergencyLanding = 1U << 16, // User Emergency Landing : (1) User EL is ON, (0) User EL is OFF
        TimerElapsed = 1U << 17, // Timer elapsed : (1) elapsed, (0) not elapsed
        MagnetoNeedsCalib = 1U << 18, // Magnetometer calibration state : (0) Ok, no calibration needed, (1) not ok, calibration needed
        AnglesOutOfRange = 1U << 19, // Angles : (0) Ok, (1) out of range
        Wind = 1U << 20, // WIND : (0) ok, (1) Too much wind
        UltrasoundDeaf = 1U << 21, // Ultrasonic sensor : (0) Ok, (1) deaf
        CutoutDetected = 1U << 22, // Cutout system detection : (0) Not detected, (1) detected
        PicVersionIsOk = 1U << 23, // PIC Version number OK : (0) a bad version number, (1) version number is OK
        ATCodecThreadOn = 1U << 24, // ATCodec thread ON : (0) thread OFF (1) thread ON
        NavdataThreadOn = 1U << 25, // Navdata thread ON : (0) thread OFF (1) thread ON
        VideoThreadOn = 1U << 26, // Video thread ON : (0) thread OFF (1) thread ON
        ACQThreadOn = 1U << 27, // Acquisition thread ON : (0) thread OFF (1) thread ON
        CtrlWatchdog = 1U << 28, // CTRL watchdog : (1) delay in control execution (> 5ms), (0) control is well scheduled
        AdcWatchdog = 1U << 29, // ADC Watchdog : (1) delay in uart2 dsr (> 5ms), (0) uart2 is good
        ComWatchdog = 1U << 30, // Communication Watchdog : (1) com problem, (0) Com is ok
        Emergency = 1U << 31 // Emergency landing : (0) no emergency, (1) emergency
    }
}