namespace AR.Drone.Client.Command
{
    public class CalibrateCommand : AtCommand
    {
        public static CalibrateCommand Magnetometer = new CalibrateCommand(Device.Magnetometer);

        private readonly Device _device;

        private CalibrateCommand(Device device)
        {
            _device = device;
        }

        protected override string ToAt(int sequenceNumber)
        {
            return string.Format("AT*CALIB={0},{1}\r", sequenceNumber, (int) _device);
        }
    }
}