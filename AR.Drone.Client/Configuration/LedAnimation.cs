using System;
using AR.Drone.Data;

namespace AR.Drone.Client
{
    public struct LedAnimation
    {
        public LedAnimation(LedAnimationType type, float frequency, int duration) 
            : this()
        {
            Type = type;
            Frequency = frequency;
            Duration = duration;
        }

        public LedAnimationType Type { get; private set; }
        public float Frequency { get; private set; }
        public int Duration { get; private set; }

        public static LedAnimation Parse(string value)
        {
            string[] parts = value.Split(',');
            var animation = new LedAnimation();
            LedAnimationType type;
            int duration;
            int ifrequency;
            if (parts.Length > 2 && Enum.TryParse(parts[0], out type) && int.TryParse(parts[1], out ifrequency)
                && int.TryParse(parts[2], out duration))
            {
                animation.Type = type;
                animation.Frequency = ConversionHelper.ToSingle(ifrequency);
                animation.Duration = duration;
            }
            return animation;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", (int) Type, ConversionHelper.ToInt(Frequency), Duration);
        }
    }
}

