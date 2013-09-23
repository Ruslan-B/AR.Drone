using System;

using AR.Drone.Avionics.Tools;
using AR.Drone.Avionics.Apparatus;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public class Hovering : IntentObtainer
    {
        public Hovering() : base(0.0f) { /* Do Nothing */ }

        public override void Contribute(Output aApparatusOutput, ref Input aApparatusInput)
        {
            aApparatusInput.Command = Apparatus.Input.Type.Hover;
        }
    }
}
