using AR.Drone.Avionics.Tools.Time;

namespace AR.Drone.Avionics.Objectives
{
    /// <summary>
    /// Objective that flat trims the drone
    /// </summary>
    public class Emergency : Objective
    {
        private void CreateTask()
        {
            Add(new IntentObtainers.EmergencySetting(true));
        }

        public Emergency(long aDuration = 0) : base(aDuration)
        {
            CreateTask();
        }

        public Emergency(Expiration aExpiration) : base(aExpiration)
        {
            CreateTask();
        }
    }
}