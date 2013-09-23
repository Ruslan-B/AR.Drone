using System;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    // A target value with an expiration functionality 
    public class Intent
    {
        // A value that indicates that the Intent has no target value
        public const float None = Single.NaN;
        // Default agression level value of the intent
        public const float DefaultAgression = 1.0f;

        // Value to aim for (target value)
        public readonly float Value;

        // Determines the agression level of the Intent (a multiplier)
        // NOTE: = 1.0 is neutral, < 1.0 passive, > 1.0 agressive
        public float Agression = DefaultAgression;

        // Create a new Intent, with no expiration, unless provided
        public Intent(float aValue, float aAgression = DefaultAgression)
        {
            Value = aValue;
            Agression = aAgression;
        }

        public float Diff(float aValue)
        {
            return Value - aValue;
        }
    }
}