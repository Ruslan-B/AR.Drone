using System;

namespace AR.Drone.Command
{
    public class ProgressiveCommand : ATCommand
    {
        private readonly float _gaz;
        private readonly ProgressiveMode _mode;
        private readonly float _pitch;
        private readonly float _roll;
        private readonly float _yaw;


        public ProgressiveCommand(ProgressiveMode mode, float roll, float pitch, float yaw, float gaz)
        {
            _mode = mode;
            _roll = roll;
            _pitch = pitch;
            _yaw = yaw;
            _gaz = gaz;
        }

        protected override string ToAt(int sequenceNumber)
        {
            return string.Format("AT*PCMD={0},{1},{2},{3},{4},{5}\r", sequenceNumber, (int) _mode, Normalize(_roll), Normalize(_pitch), Normalize(_gaz), Normalize(_yaw));
        }

        private int Normalize(float value)
        {
            int resultingValue;
            unsafe
            {
                value = (Math.Abs(value) > 1) ? 1 : value;
                resultingValue = *(int*) (&value);
            }
            return resultingValue;
        }
    }

    public enum ProgressiveMode
    {
        Progressive = 0,
        CombinedYaw = 1,
        AbsoluteControl = 2,
    }
}