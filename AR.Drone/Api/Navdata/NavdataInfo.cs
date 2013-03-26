using AR.Drone.NativeApi;
using AR.Drone.NativeApi.Navdata;

namespace AR.Drone.Api.Navdata
{
    public struct NavdataInfo
    {
        public ARDroneState State;
        public NavdataDemo Demo;
        public NavdataTime Time;
        public NavdataEulerAngles EulerAngles;
        public NavdataReferences References;
        public NavdataTrims Trims;
        public NavdataRcReferences RcReferences;
        public NavdataPwm PWM;
        public NavdataAltitude Altitude;
        public NavdataWifi Wifi;
        public NavdataCheckSum CheckSum;
    }
}