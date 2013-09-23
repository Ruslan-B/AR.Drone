using AR.Drone.Avionics.Apparatus;
using AR.Drone.Avionics.Tools;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public class SetYaw : IntentObtainer
    {
        public SetYaw(float aValue) : base(aValue, DefaultAgression)
        {
            /* Do Nothing */
        }

        public override void Contribute(Output aApparatusOutput, ref Input aApparatusInput)
        {
            aApparatusInput.Yaw = Arithmetics.KeepInRange(Value, Input.Limits.Yaw.Min, Input.Limits.Yaw.Max);
        }
    }
}