using System;

using AR.Drone.Avionics.Tools;
using AR.Drone.Avionics.Tools.Time;
using AR.Drone.Avionics.Apparatus;

namespace AR.Drone.Avionics.Objectives
{
    /// <summary>
    /// Objective that commands the drone to take off
    /// </summary>
    public class Takeoff : Objective
    {
        private void _create_task() { Add(new IntentObtainers.Takingoff()); }

        public Takeoff(long aDuration) : base(aDuration) { _create_task(); }
        public Takeoff(Expiration aExpiration) : base(aExpiration) { _create_task(); }
    }
}
