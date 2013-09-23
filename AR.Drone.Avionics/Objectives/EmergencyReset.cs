using AR.Drone.Avionics.Tools.Time;

namespace AR.Drone.Avionics.Objectives
{
    /// <summary>
    /// Objective that flat trims the drone
    /// </summary>
    public class EmergencyReset : Objective
    {
        private void CreateTask()
        {
            Add(new IntentObtainers.EmergencyResetting(true));
        }

        public EmergencyReset(long aDuration = 0) : base(aDuration)
        {
            CreateTask();
        }

        public EmergencyReset(Expiration aExpiration) : base(aExpiration)
        {
            CreateTask();
        }
    }
}