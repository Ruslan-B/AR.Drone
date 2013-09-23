using AR.Drone.Avionics.Tools.Time;

namespace AR.Drone.Avionics.Objectives
{
    /// <summary>
    /// Objective that commands the drone to take off
    /// </summary>
    public class Takeoff : Objective
    {
        private void CreateTask()
        {
            Add(new IntentObtainers.Takingoff());
        }

        public Takeoff(long aDuration) : base(aDuration)
        {
            CreateTask();
        }

        public Takeoff(Expiration aExpiration) : base(aExpiration)
        {
            CreateTask();
        }
    }
}