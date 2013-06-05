using System;
using AR.Drone.Data.Helpers;
using AR.Drone.Data.Navigation.Native;

namespace AR.Drone.Data.Navigation
{
    public static class NavdataConverter
    {
        private const float DegreeToRadian = (float) (Math.PI/180.0f);

        public static NavigationData ToNavigationData(NavdataBag navdataBag)
        {
            var navigationData = new NavigationData();

            var ardroneState = (def_ardrone_state_mask_t) navdataBag.ardrone_state;
            UpdateStateUsing(ardroneState, ref navigationData.State);

            var ctrlState = (CTRL_STATES) (navdataBag.demo.ctrl_state >> 0x10);
            UpdateStateUsing(ctrlState, ref navigationData.State);

            navigationData.Yaw = DegreeToRadian*(navdataBag.demo.psi/1000.0f);
            navigationData.Pitch = DegreeToRadian*(navdataBag.demo.theta/1000.0f);
            navigationData.Roll = DegreeToRadian*(navdataBag.demo.phi/1000.0f);

            navigationData.Altitude = navdataBag.demo.altitude/1000.0f;

            navigationData.Time = navdataBag.time.time;

            navigationData.Velocity.X = navdataBag.demo.vx/1000.0f;
            navigationData.Velocity.Y = navdataBag.demo.vy/1000.0f;
            navigationData.Velocity.Z = navdataBag.demo.vz/1000.0f;

            navigationData.Battery.Low = ardroneState.HasFlag(def_ardrone_state_mask_t.ARDRONE_VBAT_LOW);
            navigationData.Battery.Percentage = navdataBag.demo.vbat_flying_percentage;
            navigationData.Battery.Voltage = navdataBag.raw_measures.vbat_raw/1000.0f;

            navigationData.Magneto.Rectified.X = navdataBag.magneto.magneto_rectified.x;
            navigationData.Magneto.Rectified.Y = navdataBag.magneto.magneto_rectified.y;
            navigationData.Magneto.Rectified.Z = navdataBag.magneto.magneto_rectified.z;

            navigationData.Magneto.Offset.X = navdataBag.magneto.magneto_offset.x;
            navigationData.Magneto.Offset.Y = navdataBag.magneto.magneto_offset.y;
            navigationData.Magneto.Offset.Z = navdataBag.magneto.magneto_offset.z;

            navigationData.Video.FrameNumber = navdataBag.video_stream.frame_number;

            navigationData.Wifi.LinkQuality = 1.0f - ConversionHelper.ToSingle(navdataBag.wifi.link_quality);

            return navigationData;
        }

        private static void UpdateStateUsing(def_ardrone_state_mask_t ardroneState, ref NavigationState state)
        {
            if (ardroneState.HasFlag(def_ardrone_state_mask_t.ARDRONE_FLY_MASK))
                state |= NavigationState.Flying;
            else
                state |= NavigationState.Landed;

            if (ardroneState.HasFlag(def_ardrone_state_mask_t.ARDRONE_EMERGENCY_MASK))
                state |= NavigationState.Emergency;

            if (ardroneState.HasFlag(def_ardrone_state_mask_t.ARDRONE_COMMAND_MASK))
                state |= NavigationState.Command;

            if (ardroneState.HasFlag(def_ardrone_state_mask_t.ARDRONE_CONTROL_MASK))
                state |= NavigationState.Control;
        }

        private static void UpdateStateUsing(CTRL_STATES ctrlStates, ref NavigationState state)
        {
            switch (ctrlStates)
            {
                case CTRL_STATES.CTRL_TRANS_TAKEOFF:
                    state |= NavigationState.Takeoff;
                    break;
                case CTRL_STATES.CTRL_TRANS_LANDING:
                    state |= NavigationState.Landing;
                    break;
                case CTRL_STATES.CTRL_HOVERING:
                    state |= NavigationState.Hovering;
                    break;
            }
        }
    }
}