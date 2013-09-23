using AR.Drone.Avionics.Apparatus;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public class VelocityY : VelocityIntent
    {
        public VelocityY(float aValue, float aAgression = DefaultAgression) : base(aValue, aAgression)
        {
            /* Do Nothing */
        }

        public override void Contribute(Output aApparatusOutput, ref Input aApparatusInput)
        {
            aApparatusInput.Roll = CalculateVelocityManeuver(aApparatusOutput.Navigation.Velocity.Y);
        }
    }
}