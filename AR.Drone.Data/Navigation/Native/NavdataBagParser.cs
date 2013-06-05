using System;
using AR.Drone.Data.Navigation.Native.Options;

namespace AR.Drone.Data.Navigation.Native
{
    public class NavdataBagParser
    {
        private const int NavdataHeader = 0x55667788;

        public static unsafe bool TryParse(ref NavigationPacket packet, out NavdataBag navigationData)
        {
            byte[] data = packet.Data;
            navigationData = new NavdataBag();

            if (data.Length < sizeof (navdata_t))
                return false;

            fixed (byte* pData = &data[0])
            {
                navdata_t navdata = *(navdata_t*) pData;
                if (navdata.header == NavdataHeader)
                {
                    navigationData.ardrone_state = navdata.ardrone_state;

                    int offset = sizeof (navdata_t);
                    while (offset < data.Length)
                    {
                        var option = (navdata_option_t*) (pData + offset);
                        ProcessOption(option, ref navigationData);
                        offset += option->size;
                    }
                    uint dataCheckSum = CalculateChecksum(data);
                    if (navigationData.cks.cks == dataCheckSum)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static unsafe void ProcessOption(navdata_option_t* option, ref NavdataBag navigationData)
        {
            var tag = (navdata_tag_t) option->tag;
            switch (tag)
            {
                case navdata_tag_t.NAVDATA_DEMO_TAG:
                    navigationData.demo = *(navdata_demo_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_TIME_TAG:
                    navigationData.time = *(navdata_time_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_RAW_MEASURES_TAG:
                    navigationData.raw_measures = *(navdata_raw_measures_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_PHYS_MEASURES_TAG:
                    navigationData.phys_measures = *(navdata_phys_measures_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_GYROS_OFFSETS_TAG:
                    navigationData.gyros_offsets = *(navdata_gyros_offsets_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_EULER_ANGLES_TAG:
                    navigationData.euler_angles = *(navdata_euler_angles_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_REFERENCES_TAG:
                    navigationData.references = *(navdata_references_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_TRIMS_TAG:
                    navigationData.trims = *(navdata_trims_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_RC_REFERENCES_TAG:
                    navigationData.rc_references = *(navdata_rc_references_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_PWM_TAG:
                    navigationData.pwm = *(navdata_pwm_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_ALTITUDE_TAG:
                    navigationData.altitude = *(navdata_altitude_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_VISION_RAW_TAG:
                    navigationData.vision_raw = *(navdata_vision_raw_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_VISION_OF_TAG:
                    navigationData.vision_of_tag = *(navdata_vision_of_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_VISION_TAG:
                    navigationData.vision = *(navdata_vision_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_VISION_PERF_TAG:
                    navigationData.vision_perf = *(navdata_vision_perf_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_TRACKERS_SEND_TAG:
                    navigationData.trackers_send = *(navdata_trackers_send_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_VISION_DETECT_TAG:
                    navigationData.vision_detect = *(navdata_vision_detect_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_WATCHDOG_TAG:
                    navigationData.watchdog = *(navdata_watchdog_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_ADC_DATA_FRAME_TAG:
                    navigationData.adc_data_frame = *(navdata_adc_data_frame_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_VIDEO_STREAM_TAG:
                    navigationData.video_stream = *(navdata_video_stream_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_GAMES_TAG:
                    navigationData.games = *(navdata_games_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_PRESSURE_RAW_TAG:
                    navigationData.pressure_raw = *(navdata_pressure_raw_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_MAGNETO_TAG:
                    navigationData.magneto = *(navdata_magneto_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_WIND_TAG:
                    navigationData.wind_speed = *(navdata_wind_speed_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_KALMAN_PRESSURE_TAG:
                    navigationData.kalman_pressure = *(navdata_kalman_pressure_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_HDVIDEO_STREAM_TAG:
                    navigationData.hdvideo_stream = *(navdata_hdvideo_stream_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_WIFI_TAG:
                    navigationData.wifi = *(navdata_wifi_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_ZIMMU_3000_TAG:
                    // do nothing
                    break;
                case navdata_tag_t.NAVDATA_NUM_TAGS:
                    // do nothing
                    break;
                case navdata_tag_t.NAVDATA_CKS_TAG:
                    navigationData.cks = *(navdata_cks_t*) option;
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