using System;

using AR.Drone.Avionics.Tools;
using AR.Drone.Avionics.Apparatus;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public class Altitude : IntentObtainer
    {
        static readonly float Min = 0.3f;
        static readonly float Max = 200.0f;

        public Altitude(float aValue, float aAgression = Intent.DefaultAgression) : base(aValue, aAgression) { /* Do Nothing */ }

        public override void Contribute(Output aApparatusOutput, ref Input aApparatusInput)
        {
            float __altitude = Arithmetics.KeepInRange(aApparatusOutput.Navigation.Altitude, Min, Max);
            aApparatusInput.Gaz = Arithmetics.KeepInRange(Diff(__altitude) * Agression, Input.Limits.Gaz.Min, Input.Limits.Gaz.Max);
        }
    }
}
