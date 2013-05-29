namespace AR.Drone.Client.Commands
{
    public class CalibrateCommand : ATCommand
    {
        private readonly Device _device;

        public CalibrateCommand(Device device)
        {
            _device = device;
        }

        protected override string ToAt(int sequenceNumber)
        {
            return string.Format("AT*CALIB={0},{1},0\r", sequenceNumber, (int) _device);
        }
    }
}