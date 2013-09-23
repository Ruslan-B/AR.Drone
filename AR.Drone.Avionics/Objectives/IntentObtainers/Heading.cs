using System;
using AR.Drone.Avionics.Apparatus;
using AR.Drone.Avionics.Tools;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    public class Heading : IntentObtainer
    {
        public const float Min = -(float) Math.PI;
        public const float Max = +(float) Math.PI;
        private const float Pi2 = (float) (Math.PI*2.0);

        private int _obtainCandidates;

        public Heading(float aValue, float aAgression = DefaultAgression, bool aCanBeObtained = false) :
            base(CorrectRotation(aValue), aAgression, aCanBeObtained)
        {
            _obtainCandidates = 0;
        }

        public override void Contribute(Output aApparatusOutput, ref Input aApparatusInput)
        {
            float heading = CorrectRotation(aApparatusOutput.Navigation.Yaw);
            aApparatusInput.Yaw = Arithmetics.KeepInRange(CorrectRotation((Value) - (heading))*Agression, Input.Limits.Yaw.Min,
                                                          Input.Limits.Yaw.Max);
            
            if (CanBeObtained)
            {
                if (Math.Abs(aApparatusInput.Yaw) < 0.05) _obtainCandidates++;
                else _obtainCandidates = 0;

                if (_obtainCandidates > 3) Obtained = true;
            }
        }

        private static float CorrectRotation(float aValue)
        {
            while (aValue > Max) aValue -= Pi2;
            while (aValue < Min) aValue += Pi2;

            return aValue;
        }
    }
}