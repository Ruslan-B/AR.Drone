using AR.Drone.Avionics.Apparatus;
using AR.Drone.Avionics.Tools;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public class SetPitch : IntentObtainer
    {
        public SetPitch(float aValue) : base(aValue, DefaultAgression)
        {
            /* Do Nothing */
        }

        public override void Contribute(Output aApparatusOutput, ref Input aApparatusInput)
        {
            aApparatusInput.Pitch = Arithmetics.KeepInRange(Value, Input.Limits.Pitch.Min, Input.Limits.Pitch.Max);
        }
    }
}