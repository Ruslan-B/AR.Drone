using System;

namespace AR.Drone.NativeApi
{
    public class NavdataParser
    {
        private const int NavdataHeader = 0x55667788;

        public static unsafe bool TryParse(byte[] data, out RawNavdata rawNavdata)
        {
            rawNavdata = new RawNavdata();

            if (data.Length < sizeof (navdata_t))
                return false;

            fixed (byte* pData = &data[0])
            {
                navdata_t navdata_t = *(navdata_t*) pData;
                if (navdata_t.header == NavdataHeader)
                {
                    rawNavdata.ardrone_state = (def_ardrone_state_mask_t) navdata_t.ardrone_state;

                    int offset = sizeof (navdata_t);
                    while (offset < data.Length)
                    {
                        var option = (navdata_option_t*) (pData + offset);
                        ProcessOption(option, ref rawNavdata);
                        offset += option->size;
                    }
                    uint dataCheckSum = CalculateChecksum(data);
                    if (rawNavdata.cks.cks == dataCheckSum)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static unsafe void ProcessOption(navdata_option_t* option, ref RawNavdata rawNavdata)
        {
            var tag = (navdata_tag_t) option->tag;
            switch (tag)
            {
                case navdata_tag_t.NAVDATA_DEMO_TAG:
                    rawNavdata.demo = *(navdata_demo_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_TIME_TAG:
                    rawNavdata.time = *(navdata_time_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_RAW_MEASURES_TAG:
                    rawNavdata.raw_measures = *(navdata_raw_measures_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_PHYS_MEASURES_TAG:
                    rawNavdata.phys_measures = *(navdata_phys_measures_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_GYROS_OFFSETS_TAG:
                    rawNavdata.gyros_offsets = *(navdata_gyros_offsets_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_EULER_ANGLES_TAG:
                    rawNavdata.euler_angles = *(navdata_euler_angles_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_REFERENCES_TAG:
                    rawNavdata.references = *(navdata_references_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_TRIMS_TAG:
                    rawNavdata.trims = *(navdata_trims_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_RC_REFERENCES_TAG:
                    rawNavdata.rc_references = *(navdata_rc_references_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_PWM_TAG:
                    rawNavdata.pwm = *(navdata_pwm_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_ALTITUDE_TAG:
                    rawNavdata.altitude = *(navdata_altitude_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_VISION_RAW_TAG:
                    rawNavdata.vision_raw = *(navdata_vision_raw_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_VISION_OF_TAG:
                    rawNavdata.vision_of_tag = *(navdata_vision_of_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_VISION_TAG:
                    rawNavdata.vision = *(navdata_vision_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_VISION_PERF_TAG:
                    rawNavdata.vision_perf = *(navdata_vision_perf_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_TRACKERS_SEND_TAG:
                    rawNavdata.trackers_send = *(navdata_trackers_send_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_VISION_DETECT_TAG:
                    rawNavdata.vision_detect = *(navdata_vision_detect_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_WATCHDOG_TAG:
                    rawNavdata.watchdog = *(navdata_watchdog_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_ADC_DATA_FRAME_TAG:
                    rawNavdata.adc_data_frame = *(navdata_adc_data_frame_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_VIDEO_STREAM_TAG:
                    rawNavdata.video_stream = *(navdata_video_stream_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_GAMES_TAG:
                    rawNavdata.games = *(navdata_games_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_PRESSURE_RAW_TAG:
                    rawNavdata.pressure_raw = *(navdata_pressure_raw_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_MAGNETO_TAG:
                    rawNavdata.magneto = *(navdata_magneto_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_WIND_TAG:
                    rawNavdata.wind_speed = *(navdata_wind_speed_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_KALMAN_PRESSURE_TAG:
                    rawNavdata.kalman_pressure = *(navdata_kalman_pressure_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_HDVIDEO_STREAM_TAG:
                    rawNavdata.hdvideo_stream = *(navdata_hdvideo_stream_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_WIFI_TAG:
                    rawNavdata.wifi = *(navdata_wifi_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_ZIMMU_3000_TAG:
                    rawNavdata.zimmu_3000 = *(navdata_zimmu_3000_t*) option;
                    break;
                case navdata_tag_t.NAVDATA_NUM_TAGS:
                    break;
                case navdata_tag_t.NAVDATA_CKS_TAG:
                    rawNavdata.cks = *(navdata_cks_t*) option;
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