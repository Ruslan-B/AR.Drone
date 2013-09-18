using System;

namespace AR.Drone.Client.Configuration
{
    public struct FlightAnimation
    {
        public FlightAnimation(FlightAnimationType type, int duration) 
            : this()
        {
            Type = type;
            Duration = duration;
        }

        public FlightAnimation(FlightAnimationType type) 
            : this (type, GetDefaultDuration(type))
        {
        }

        public FlightAnimationType Type { get; private set; }
        public int Duration { get; private set; }

        public static FlightAnimation Parse(string value)
        {
            string[] parts = value.Split(',');
            var animation = new FlightAnimation();
            FlightAnimationType type;
            int duration;
            if (parts.Length > 1 && Enum.TryParse(parts[0], out type) && int.TryParse(parts[1], out duration))
            {
                animation.Type = type;
                animation.Duration = duration;
            }
            return animation;
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", (int) Type, Duration);
        }

        public static int GetDefaultDuration(FlightAnimationType type)
        {
            switch (type)
            {
                case FlightAnimationType.PhiM30Deg:
                    return 1000;
                case FlightAnimationType.Phi30Deg:
                    return 1000;
                case FlightAnimationType.ThetaM30Deg:
                    return 1000;
                case FlightAnimationType.Theta30Deg:
                    return 1000;
                case FlightAnimationType.Theta20DegYaw200Deg:
                    return 1000;
                case FlightAnimationType.Theta20DegYawM200Deg:
                    return 1000;
                case FlightAnimationType.Turnaround:
                    return 5000;
                case FlightAnimationType.TurnaroundGodown:
                    return 5000;
                case FlightAnimationType.YawShake:
                    return 2000;
                case FlightAnimationType.YawDance:
                    return 5000;
                case FlightAnimationType.PhiDance:
                    return 5000;
                case FlightAnimationType.ThetaDance:
                    return 5000;
                case FlightAnimationType.VzDance:
                    return 5000;
                case FlightAnimationType.Wave:
                    return 5000;
                case FlightAnimationType.PhiThetaMixed:
                    return 5000;
                case FlightAnimationType.DoublePhiThetaMixed:
                    return 5000;
                case FlightAnimationType.FlipAhead:
                    return 15;
                case FlightAnimationType.FlipBehind:
                    return 15;
                case FlightAnimationType.FlipLeft:
                    return 15;
                case FlightAnimationType.FlipRight:
                    return 15;
                case FlightAnimationType.Mayday:
                    return 15;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }
    }
}