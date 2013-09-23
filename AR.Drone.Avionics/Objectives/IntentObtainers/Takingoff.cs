using System;

using AR.Drone.Avionics.Tools;
using AR.Drone.Avionics.Apparatus;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public class Takingoff : IntentObtainer
    {
        public Takingoff() : base(0.0f) { /* Do Nothing */ }

        public override void Contribute(Apparatus.Output aApparatusOutput, ref Apparatus.Input aApparatusInput)
        {

            aApparatusInput.Command = Apparatus.Input.Type.Takeoff;
        }
    }
}
