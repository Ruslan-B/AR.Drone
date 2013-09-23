using AR.Drone.Avionics.Apparatus;
using AR.Drone.Avionics.Tools;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public class SetGaz : IntentObtainer
    {
        public SetGaz(float aValue) : base(aValue, DefaultAgression)
        {
            /* Do Nothing */
        }

        public override void Contribute(Output aApparatusOutput, ref Input aApparatusInput)
        {
            aApparatusInput.Gaz = Arithmetics.KeepInRange(Value, Input.Limits.Gaz.Min, Input.Limits.Gaz.Max);
        }
    }
}