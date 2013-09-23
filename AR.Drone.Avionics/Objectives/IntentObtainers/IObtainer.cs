using AR.Drone.Avionics.Apparatus;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public interface IObtainer
    {
        void Contribute(Output aApparatusOutput, ref Input aApparatusInput);
    }
}