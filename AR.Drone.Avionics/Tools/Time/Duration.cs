using System;

namespace AR.Drone.Avionics.Tools.Time
{
    // Represents a time duration in milliseconds
    public class Duration
    {
        public readonly long Period;

        // Set duration period in milliseconds
        public Duration(long aPeriod)
        {
            Period = aPeriod;
        }

        // When would the period end, elapsing time right now
        public DateTime Ends()
        {
            return new DateTime(DateTime.Now.Ticks + Period*10000);
        }

        // When would the period end, elapsing time from a given point
        public DateTime Ends(DateTime aTime)
        {
            return new DateTime(aTime.Ticks + Period*10000);
        }

        // Returns period as DateTime class
        public DateTime AsDateTime()
        {
            return new DateTime(Period*10000);
        }
    }
}