using System;

using AR.Drone.Avionics.Tools;
using AR.Drone.Avionics.Tools.Time;
using AR.Drone.Avionics.Apparatus;

namespace AR.Drone.Avionics.Objectives
{
    /// <summary>
    /// Objective that commands the drone to hover
    /// </summary>
    public class Hover : Objective
    {
        private void _create_task() { Add(new IntentObtainers.Hovering()); }

        public Hover(long aDuration) : base(aDuration) { _create_task(); }
        public Hover(Expiration aExpiration) : base(aExpiration) { _create_task(); }
    }
}
