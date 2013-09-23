using System;

using AR.Drone.Avionics.Tools;
using AR.Drone.Avionics.Apparatus;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public abstract class VelocityIntent : IntentObtainer
    {
        static readonly float Min = -15.0f;
        static readonly float Max = 15.0f;

        protected float CalculateVelocityManeuver(float aVelocity)
        {
            float __velocity = Arithmetics.KeepInRange(aVelocity, Min, Max);
            float __diff = Diff(__velocity) / 2;
            /*bool __neg = __diff < 0;

            __diff = (float)Math.Pow(__diff, 2);
            if (__neg) __diff = -__diff;*/

            return Arithmetics.KeepInRange(__diff * Agression, Input.Limits.Pitch.Min, Input.Limits.Pitch.Max);
        }

        public VelocityIntent(float aValue, float aAgression = Intent.DefaultAgression) : base(aValue, aAgression) { /* Do Nothing */ }
    }
}
