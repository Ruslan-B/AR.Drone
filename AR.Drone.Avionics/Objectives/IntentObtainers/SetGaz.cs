using System;

using AR.Drone.Avionics.Tools;
using AR.Drone.Avionics.Apparatus;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public class SetGaz : IntentObtainer
    {
        public SetGaz(float aValue) : base(aValue, Intent.DefaultAgression) { /* Do Nothing */ }

        public override void Contribute(Output aApparatusOutput, ref Input aApparatusInput)
        {
            aApparatusInput.Gaz = Arithmetics.KeepInRange(Value, Input.Limits.Gaz.Min, Input.Limits.Gaz.Max);
        }
    }
}
