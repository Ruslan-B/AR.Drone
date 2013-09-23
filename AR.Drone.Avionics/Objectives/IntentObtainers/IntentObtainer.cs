using System;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    // Unification of Intent and Obtainer. (See Intent as data, while obtainer is an action)
    public abstract class IntentObtainer : Intent, EarlyObtainer
    {
        public bool CanBeObtained { get; private set; }
        public bool Obtained { get; protected set; }

        public IntentObtainer(float aValue, float aAgression = Intent.DefaultAgression, bool aCanBeObained = false) : base(aValue, aAgression)
        {
            CanBeObtained = aCanBeObained;
            Obtained = false;
        }

        public abstract void Contribute(Apparatus.Output aApparatusOutput, ref Apparatus.Input aApparatusInput);
    }
}
