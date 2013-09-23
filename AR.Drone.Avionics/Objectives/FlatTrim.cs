using AR.Drone.Avionics.Tools.Time;

namespace AR.Drone.Avionics.Objectives
{
    /// <summary>
    /// Objective that flat trims the drone
    /// </summary>
    public class FlatTrim : Objective
    {
        private void CreateTask()
        {
            Add(new IntentObtainers.FlatTrimming(true));
        }

        public FlatTrim(long aDuration = 0) : base(aDuration)
        {
            CreateTask();
        }

        public FlatTrim(Expiration aExpiration) : base(aExpiration)
        {
            CreateTask();
        }
    }
}