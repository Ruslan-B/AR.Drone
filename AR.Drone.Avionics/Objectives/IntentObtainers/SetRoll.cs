using AR.Drone.Avionics.Apparatus;
using AR.Drone.Avionics.Tools;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public class SetRoll : IntentObtainer
    {
        public SetRoll(float aValue) : base(aValue, DefaultAgression)
        {
            /* Do Nothing */
        }

        public override void Contribute(Output aApparatusOutput, ref Input aApparatusInput)
        {
            aApparatusInput.Roll = Arithmetics.KeepInRange(Value, Input.Limits.Roll.Min, Input.Limits.Roll.Max);
        }
    }
}