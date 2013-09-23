using AR.Drone.Avionics.Apparatus;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public class FlatTrimming : IntentObtainer
    {
        public FlatTrimming(bool aCanBeOntained = false) : base(0.0f, DefaultAgression, aCanBeOntained)
        {
            /* Do Nothing */
        }

        public override void Contribute(Output aApparatusOutput, ref Input aApparatusInput)
        {
            aApparatusInput.Command = Input.Type.FlatTrim;
            Obtained = true;
        }
    }
}