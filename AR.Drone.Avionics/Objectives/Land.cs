using AR.Drone.Avionics.Tools.Time;

namespace AR.Drone.Avionics.Objectives
{
    /// <summary>
    /// Objective that command the drone to land
    /// </summary>
    public class Land : Objective
    {
        private void CreateTask()
        {
            Add(new IntentObtainers.Landing());
        }

        public Land(long aDuration) : base(aDuration)
        {
            CreateTask();
        }

        public Land(Expiration aExpiration) : base(aExpiration)
        {
            CreateTask();
        }
    }
}