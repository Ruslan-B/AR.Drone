using AR.Drone.Data;

namespace AR.Drone.Client.Command
{
    public class ProgressWithMagnetoCommand : AtCommand
    {
        private readonly FlightMode _mode;
        private readonly float _roll;
        private readonly float _pitch;
        private readonly float _yaw;
        private readonly float _gaz;
        private readonly float _psi;
        private readonly float _accuracy;

        public ProgressWithMagnetoCommand(FlightMode mode, float roll, float pitch, float yaw, float gaz, float psi, float accuracy)
        {
            _mode = mode;
            _roll = roll;
            _pitch = pitch;
            _yaw = yaw;
            _gaz = gaz;
            _psi = psi;
            _accuracy = accuracy;
        }

        protected override string ToAt(int sequenceNumber)
        {
            return string.Format("AT*PCMD_MAG={0},{1},{2},{3},{4},{5},{6},{7}\r", sequenceNumber,
                                 (int) _mode,
                                 ConversionHelper.ToInt(_roll),
                                 ConversionHelper.ToInt(_pitch),
                                 ConversionHelper.ToInt(_gaz),
                                 ConversionHelper.ToInt(_yaw),
                                 ConversionHelper.ToInt(_psi),
                                 ConversionHelper.ToInt(_accuracy));
        }
    }
}