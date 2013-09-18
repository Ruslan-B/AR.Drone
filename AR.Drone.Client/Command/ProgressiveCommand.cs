using AR.Drone.Data;

namespace AR.Drone.Client.Command
{
    public class ProgressCommand : AtCommand
    {
        private readonly FlightMode _mode;
        private readonly float _roll;
        private readonly float _pitch;
        private readonly float _yaw;
        private readonly float _gaz;

        public ProgressCommand(FlightMode mode, float roll, float pitch, float yaw, float gaz)
        {
            _mode = mode;
            _roll = roll;
            _pitch = pitch;
            _yaw = yaw;
            _gaz = gaz;
        }

        protected override string ToAt(int sequenceNumber)
        {
            return string.Format("AT*PCMD={0},{1},{2},{3},{4},{5}\r", sequenceNumber,
                                 (int) _mode,
                                 ConversionHelper.ToInt(_roll),
                                 ConversionHelper.ToInt(_pitch),
                                 ConversionHelper.ToInt(_gaz),
                                 ConversionHelper.ToInt(_yaw));
        }
    }
}