using System;

using AR.Drone.Avionics.Tools;
using AR.Drone.Avionics.Tools.Time;
using AR.Drone.Avionics.Apparatus;

namespace AR.Drone.Avionics.Objectives
{
    /// <summary>
    /// Objective that command the drone to land
    /// </summary>
    public class Land : Objective
    {
        private void _create_task() { Add(new IntentObtainers.Landing()); }

        public Land(long aDuration) : base(aDuration) { _create_task(); }
        public Land(Expiration aExpiration) : base(aExpiration) { _create_task(); }
    }
}
