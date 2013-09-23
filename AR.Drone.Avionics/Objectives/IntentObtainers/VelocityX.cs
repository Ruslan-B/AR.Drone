using AR.Drone.Avionics.Apparatus;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public class VelocityX : VelocityIntent
    {
        public VelocityX(float aValue, float aAgression = DefaultAgression) : base(aValue, aAgression)
        {
            /* Do Nothing */
        }

        public override void Contribute(Output aApparatusOutput, ref Input aApparatusInput)
        {
            aApparatusInput.Pitch = -CalculateVelocityManeuver(aApparatusOutput.Navigation.Velocity.X);
        }
    }
}