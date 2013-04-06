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
            return string.Format("AT*PCMD={0},{1},{2},{3},{4},{5}\r", sequenceNumber, (int) _mode, ToInt(_roll), ToInt(_pitch), ToInt(_gaz), ToInt(_yaw));
        }

        private int ToInt(float value)
        {
            int result = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
            return result;
        }
    }
}