using AR.Drone.Avionics.Apparatus;
using AR.Drone.Avionics.Tools;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public class Altitude : IntentObtainer
    {
        private static readonly float Min = 0.3f;
        private static readonly float Max = 200.0f;

        public Altitude(float aValue, float aAgression = DefaultAgression) : base(aValue, aAgression)
        {
            /* Do Nothing */
        }

        public override void Contribute(Output aApparatusOutput, ref Input aApparatusInput)
        {
            float altitude = Arithmetics.KeepInRange(aApparatusOutput.Navigation.Altitude, Min, Max);
            aApparatusInput.Gaz = Arithmetics.KeepInRange(Diff(altitude)*Agression, Input.Limits.Gaz.Min, Input.Limits.Gaz.Max);
        }
    }
}