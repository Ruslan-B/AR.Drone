using System;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public interface Obtainer
    {
        void Contribute(Apparatus.Output aApparatusOutput, ref Apparatus.Input aApparatusInput);
    }
}
