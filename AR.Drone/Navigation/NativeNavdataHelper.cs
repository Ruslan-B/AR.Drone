using System;
using AR.Drone.NativeApi;

namespace AR.Drone.Navigation
{
    public static class NativeNavdataHelper
    {
        public static NavigationData ToNavigationData(this NativeNavdata nativeNavdata)
        {
            var navigationData = new NavigationData();

            def_ardrone_state_mask_t ardroneState = nativeNavdata.ardrone_state;
            UpdateStateUsing(ardroneState, ref navigationData.State);

            var ctrlState = (CTRL_STATES) (nativeNavdata.demo.ctrl_state >> 0x10);
            UpdateStateUsing(ctrlState, ref navigationData.State);

            navigationData.Yaw = nativeNavdata.demo.psi/1000.0f;
            navigationData.Pitch = nativeNavdata.demo.theta/1000.0f;
            navigationData.Roll = nativeNavdata.demo.phi/1000.0f;

            navigationData.Altitude = nativeNavdata.demo.altitude/100.0f;

            navigationData.Velocity.X = nativeNavdata.demo.vx/100.0f;
            navigationData.Velocity.Y = nativeNavdata.demo.vy/100.0f;
            navigationData.Velocity.Z = nativeNavdata.demo.vz/100.0f;

            navigationData.Battery.Low = ardroneState.HasFlag(def_ardrone_state_mask_t.ARDRONE_VBAT_LOW);
            navigationData.Battery.Percentage = nativeNavdata.demo.vbat_flying_percentage;
            navigationData.Battery.Voltage = nativeNavdata.raw_measures.vbat_raw/100.0f;

            navigationData.Wifi.LinkQuality = 1.0f - ToSingle(nativeNavdata.wifi.link_quality);

            return navigationData;
        }

        private static void UpdateStateUsing(def_ardrone_state_mask_t ardroneState, ref DroneState state)
        {
            if (ardroneState.HasFlag(def_ardrone_state_mask_t.ARDRONE_FLY_MASK))
                state |= DroneState.Flying;
            else
                state |= DroneState.Landed;

            if (ardroneState.HasFlag(def_ardrone_state_mask_t.ARDRONE_EMERGENCY_MASK))
                state |= DroneState.Emergency;
        }

        private static void UpdateStateUsing(CTRL_STATES ctrlStates, ref DroneState state)
        {
            switch (ctrlStates)
            {
                case CTRL_STATES.CTRL_TRANS_TAKEOFF:
                    state |= DroneState.Takeoff;
                    break;
                case CTRL_STATES.CTRL_TRANS_LANDING:
                    state |= DroneState.Landing;
                    break;
                case CTRL_STATES.CTRL_HOVERING:
                    state |= DroneState.Hovering;
                    break;
            }
        }

        private static float ToSingle(uint value)
        {
            float result = BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
            return result;
        }
    }
}