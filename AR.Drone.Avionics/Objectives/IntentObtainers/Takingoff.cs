using AR.Drone.Avionics.Apparatus;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public class Takingoff : IntentObtainer
    {
        public Takingoff() : base(0.0f)
        {
            /* Do Nothing */
        }

        public override void Contribute(Output aApparatusOutput, ref Input aApparatusInput)
        {
            aApparatusInput.Command = Input.Type.Takeoff;
        }
    }
}