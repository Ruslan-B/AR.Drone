using System;

using AR.Drone.Avionics.Tools;
using AR.Drone.Avionics.Apparatus;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public class VelocityX : VelocityIntent
    {
        public VelocityX(float aValue, float aAgression = Intent.DefaultAgression) : base(aValue, aAgression) { /* Do Nothing */ }
        public override void Contribute(Apparatus.Output aApparatusOutput, ref Apparatus.Input aApparatusInput)
        {
            aApparatusInput.Pitch = -CalculateVelocityManeuver(aApparatusOutput.Navigation.Velocity.X);
        }
    }
}
