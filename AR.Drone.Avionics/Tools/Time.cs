using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AR.Drone.Avionics.Tools.Time
{
    // Represents a time duration in milliseconds
    public class Duration
    {
        public readonly long Period;

        // Set duration period in milliseconds
        public Duration(long aPeriod) { Period = aPeriod; }

        // When would the period end, elapsing time right now
        public DateTime Ends() { return new DateTime(DateTime.Now.Ticks + Period * 10000); }

        // When would the period end, elapsing time from a given point
        public DateTime Ends(DateTime aTime) { return new DateTime(aTime.Ticks + Period * 10000); }

        // Returns period as DateTime class
        public DateTime AsDateTime() { return new DateTime(Period * 10000); }
    }

    // Represets expiration at some point of time
    public class Expiration
    {
        // Static object that never expires
        public static readonly Expiration Never = new Expiration();

        private bool _elapsed = false;
        private Expiration() { new Duration(long.MaxValue); Time = DateTime.MaxValue; }

        // How long does the object have, before it expires
        public Duration Duration { get; private set; }

        // Absolute time of object expiration, if not elapsed, set to DateTime.Max (never expires)
        public DateTime Time { get; private set; }

        // Is the expiration elapsed
        public bool Elapsed {
            get { return _elapsed; }
            set { if (value) Elapse(); }
        }

        // Create a new expiration object, with a specified duration
        // By default, expiration time counter is not elapsed
        public Expiration(Duration aDuration, bool aElapse = false)
        {
            Duration = aDuration;
            if (aElapse) Elapsed = true;
            Time = DateTime.MaxValue;
        }

        public Expiration(long aDuration, bool aElapse = false) : this(new Duration(aDuration), aElapse) { /* Do Nothing */ }

        // Elapse time to expiration, based on provided period
        public void Elapse()
        {
            if (Elapsed) return;
            Time = Duration.Ends();
            _elapsed = true;
        }

        // Return true if object has expired
        public bool IsOverdue { get { return DateTime.Now >= Time; } }

        // Reset the expiration period, making the object valid again the the 'Duration' time provided
        public void Reset() { if (Elapsed) Time = Duration.Ends(); }
    }
}
