using AR.Drone.Avionics.Apparatus;
using AR.Drone.Avionics.Tools;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public abstract class VelocityIntent : IntentObtainer
    {
        private static readonly float Min = -15.0f;
        private static readonly float Max = 15.0f;

        protected VelocityIntent(float aValue, float aAgression = DefaultAgression) : base(aValue, aAgression)
        {
            /* Do Nothing */
        }

        protected float CalculateVelocityManeuver(float aVelocity)
        {
            float velocity = Arithmetics.KeepInRange(aVelocity, Min, Max);
            float diff = Diff(velocity)/2;

            return Arithmetics.KeepInRange(diff*Agression, Input.Limits.Pitch.Min, Input.Limits.Pitch.Max);
        }
    }
}