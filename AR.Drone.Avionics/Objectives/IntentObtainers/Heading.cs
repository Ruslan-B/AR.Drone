using System;

using AR.Drone.Avionics.Tools;
using AR.Drone.Avionics.Apparatus;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public class Heading : IntentObtainer
    {
        public const float Min = -(float)Math.PI;
        public const float Max = +(float)Math.PI;
        private static readonly float _2_pi = (float)(Math.PI * 2.0);
        private int _obtain_candidates;

        public Heading(float aValue, float aAgression = Intent.DefaultAgression, bool aCanBeObtained = false) :
            base(CorrectRotation(aValue), aAgression, aCanBeObtained) { _obtain_candidates = 0; }

        public override void Contribute(Output aApparatusOutput, ref Input aApparatusInput)
        {
            float __heading = CorrectRotation(aApparatusOutput.Navigation.Yaw);
            aApparatusInput.Yaw = Arithmetics.KeepInRange(CorrectRotation((Value /*+ _2_pi*/) - (__heading /*+ _2_pi*/)) * Agression, Input.Limits.Yaw.Min, Input.Limits.Yaw.Max);
            //if (CanBeObtained && Math.Abs(aApparatusInput.Yaw) < 0.05) Obtained = true;
            if (CanBeObtained)
            {
                if (Math.Abs(aApparatusInput.Yaw) < 0.05) _obtain_candidates++;
                else _obtain_candidates = 0;

                if (_obtain_candidates > 3) Obtained = true;
            }
        }

        private static float CorrectRotation(float aValue)
        {
            while (aValue > Max) aValue -= _2_pi;
            while (aValue < Min) aValue += _2_pi;

            return aValue;
        }
    }
}
