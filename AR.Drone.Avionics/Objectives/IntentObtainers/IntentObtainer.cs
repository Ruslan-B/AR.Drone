using AR.Drone.Avionics.Apparatus;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    // Unification of Intent and Obtainer. (See Intent as data, while obtainer is an action)
    public abstract class IntentObtainer : Intent, IEarlyObtainer
    {
        protected IntentObtainer(float aValue, float aAgression = DefaultAgression, bool aCanBeObained = false) : base(aValue, aAgression)
        {
            CanBeObtained = aCanBeObained;
            Obtained = false;
        }

        public bool CanBeObtained { get; private set; }
        public bool Obtained { get; protected set; }

        public abstract void Contribute(Output aApparatusOutput, ref Input aApparatusInput);
    }
}