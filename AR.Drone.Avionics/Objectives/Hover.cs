using AR.Drone.Avionics.Tools.Time;

namespace AR.Drone.Avionics.Objectives
{
    /// <summary>
    /// Objective that commands the drone to hover
    /// </summary>
    public class Hover : Objective
    {
        private void CreateTask()
        {
            Add(new IntentObtainers.Hovering());
        }

        public Hover(long aDuration) : base(aDuration)
        {
            CreateTask();
        }

        public Hover(Expiration aExpiration) : base(aExpiration)
        {
            CreateTask();
        }
    }
}