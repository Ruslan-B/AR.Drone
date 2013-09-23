using System;

using AR.Drone.Avionics.Tools;
using AR.Drone.Avionics.Tools.Time;
using AR.Drone.Avionics.Apparatus;

namespace AR.Drone.Avionics.Objectives
{
    /// <summary>
    /// Objective that flat trims the drone
    /// </summary>
    public class Emergency : Objective
    {
        private void _create_task() { Add(new IntentObtainers.EmergencySetting(true)); }

        public Emergency(long aDuration = 0) : base(aDuration) { _create_task(); }
        public Emergency(Expiration aExpiration) : base(aExpiration) { _create_task(); }
    }
}
