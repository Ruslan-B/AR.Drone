using System;
using AR.Drone.NativeApi.Navdata;
using AR.Drone.Api.Navdata;

namespace AR.Drone.Helpers
{
    public class NavdataHelper
    {
        private const int NavdataHeader = 0x55667788;

        public static unsafe bool TryParse(byte[] data, out NavdataInfo navdataInfo)
        {
            navdataInfo = new NavdataInfo();
            if (data.Length < sizeof (NativeApi.Navdata.Navdata))
                return false;

            //using (var fs = new FileStream(@"d:\navdata.raw", FileMode.Create))
            //    fs.Write(data, 0, data.Length);

            fixed (byte* pData = &data[0])
            {
                NativeApi.Navdata.Navdata navdata = *(NativeApi.Navdata.Navdata*) pData;
                if (navdata.header == NavdataHeader)
                {
                    navdataInfo.State = navdata.ardrone_state;

                    int offset = sizeof (NativeApi.Navdata.Navdata);
                    while (offset < data.Length)
                    {
                        var option = (NavdataOption*) (pData + offset);
                        ProcessOption(option, ref navdataInfo);
                        offset += option->size;
                    }
                    uint checkSum = CalculateChecksum(data);
                    if (navdataInfo.CheckSum.cks == checkSum)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static unsafe void ProcessOption(NavdataOption* option, ref NavdataInfo navdataInfo)
        {
            switch (option->tag)
            {
                case NavdataTag.DEMO:
                    NavdataDemo demo = *(NavdataDemo*) option;
                    navdataInfo.Demo = demo;
                    break;
                case NavdataTag.TIME:
                    NavdataTime time = *(NavdataTime*) option;
                    navdataInfo.Time = time;
                    break;
                case NavdataTag.RAW_MEASURES:
                    break;
                case NavdataTag.PHYS_MEASURES:
                    break;
                case NavdataTag.GYROS_OFFSETS:
                    break;
                case NavdataTag.EULER_ANGLES:
                    NavdataEulerAngles eulerAngles = *(NavdataEulerAngles*) option;
                    navdataInfo.EulerAngles = eulerAngles;
                    break;
                case NavdataTag.REFERENCES:
                    NavdataReferences references = *(NavdataReferences*) option;
                    navdataInfo.References = references;
                    break;
                case NavdataTag.TRIMS:
                    NavdataTrims trims = *(NavdataTrims*) option;
                    navdataInfo.Trims = trims;
                    break;
                case NavdataTag.RC_REFERENCES:
                    NavdataRcReferences rcReferences = *(NavdataRcReferences*) option;
                    navdataInfo.RcReferences = rcReferences;
                    break;
                case NavdataTag.PWM:
                    NavdataPwm pwm = *(NavdataPwm*) option;
                    navdataInfo.PWM = pwm;
                    break;
                case NavdataTag.ALTITUDE:
                    NavdataAltitude altitude = *(NavdataAltitude*) option;
                    navdataInfo.Altitude = altitude;
                    break;
                case NavdataTag.VISION_RAW:
                    break;
                case NavdataTag.VISION_OF:
                    break;
                case NavdataTag.VISION:
                    break;
                case NavdataTag.VISION_PERF:
                    break;
                case NavdataTag.TRACKERS_SEND:
                    break;
                case NavdataTag.VISION_DETECT:
                    break;
                case NavdataTag.WATCHDOG:
                    break;
                case NavdataTag.ADC_DATA_FRAME:
                    break;
                case NavdataTag.VIDEO_STREAM:
                    break;
                case NavdataTag.GAMES:
                    break;
                case NavdataTag.PRESSURE_RAW:
                    break;
                case NavdataTag.MAGNETO:
                    break;
                case NavdataTag.WIND:
                    break;
                case NavdataTag.KALMAN_PRESSURE:
                    break;
                case NavdataTag.HDVIDEO_STREAM:
                    break;
                case NavdataTag.WIFI:
                    NavdataWifi wifi = *(NavdataWifi*) option;
                    navdataInfo.Wifi = wifi;
                    break;
                case NavdataTag.ZIMMU_3000:
                    break;
                case NavdataTag.NUMS:
                    break;
                case NavdataTag.CheckSum:
                    navdataInfo.CheckSum = *(NavdataCheckSum*) option;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static uint CalculateChecksum(byte[] buffer)
        {
            uint checksum = 0;
            for (int i = 0; i < buffer.Length - 8; ++i)
                checksum += buffer[i];
            return checksum;
        }
    }
}