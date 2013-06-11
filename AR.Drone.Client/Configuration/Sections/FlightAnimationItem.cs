using System;

namespace AR.Drone.Client.Configuration.Sections
{
    public class FlightAnimationItem : ReadWriteItem<string>
    {
        public FlightAnimationItem(string key) : base(key)
        {
        }

        public FlightAnimation Animation { get; private set; }
        public int Duration { get; private set; }

        public override bool TryUpdate(string value)
        {
            bool result = base.TryUpdate(value);
            if (result)
            {
                string[] parts = value.Split(',');
                FlightAnimation animation;
                int duration;
                if (parts.Length > 1 && Enum.TryParse(parts[0], out animation) && Enum.TryParse(parts[1], out duration))
                {
                    Animation = animation;
                    Duration = duration;
                }
            }
            return result;
        }


        public FlightAnimationItem Set(FlightAnimation animation)
        {
            return Set(animation, GetDefaultDuration(animation));
        }

        public FlightAnimationItem Set(FlightAnimation animation, int duration)
        {
            Animation = animation;
            Duration = duration;
            Value = string.Format("{0},{1}", (int) Animation, Duration);
            return this;
        }

        public static int GetDefaultDuration(FlightAnimation animation)
        {
            switch (animation)
            {
                case FlightAnimation.PhiM30Deg:
                    return 1000;
                case FlightAnimation.Phi30Deg:
                    return 1000;
                case FlightAnimation.ThetaM30Deg:
                    return 1000;
                case FlightAnimation.Theta30Deg:
                    return 1000;
                case FlightAnimation.Theta20DegYaw200Deg:
                    return 1000;
                case FlightAnimation.Theta20DegYawM200Deg:
                    return 1000;
                case FlightAnimation.Turnaround:
                    return 5000;
                case FlightAnimation.TurnaroundGodown:
                    return 5000;
                case FlightAnimation.YawShake:
                    return 2000;
                case FlightAnimation.YawDance:
                    return 5000;
                case FlightAnimation.PhiDance:
                    return 5000;
                case FlightAnimation.ThetaDance:
                    return 5000;
                case FlightAnimation.VzDance:
                    return 5000;
                case FlightAnimation.Wave:
                    return 5000;
                case FlightAnimation.PhiThetaMixed:
                    return 5000;
                case FlightAnimation.DoublePhiThetaMixed:
                    return 5000;
                case FlightAnimation.FlipAhead:
                    return 15;
                case FlightAnimation.FlipBehind:
                    return 15;
                case FlightAnimation.FlipLeft:
                    return 15;
                case FlightAnimation.FlipRight:
                    return 15;
                case FlightAnimation.Mayday:
                    return 15;
                default:
                    throw new ArgumentOutOfRangeException("animation");
            }
        }
    }
}