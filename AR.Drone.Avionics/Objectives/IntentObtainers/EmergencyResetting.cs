using AR.Drone.Avionics.Apparatus;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public class EmergencyResetting : IntentObtainer
    {
        public EmergencyResetting(bool aCanBeOntained = false) : base(0.0f, DefaultAgression, aCanBeOntained)
        {
            /* Do Nothing */
        }

        public override void Contribute(Output aApparatusOutput, ref Input aApparatusInput)
        {
            aApparatusInput.Command = Input.Type.ResetEmergency;
            Obtained = true;
        }
    }
}