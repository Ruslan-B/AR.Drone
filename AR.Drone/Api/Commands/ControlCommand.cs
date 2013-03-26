namespace AR.Drone.Api.Commands
{
    public class ControlCommand : IATCommand
    {
        private readonly ControlMode _controlMode;

        public ControlCommand(ControlMode controlMode)
        {
            _controlMode = controlMode;
        }

        public string ToAt(int sequenceNumber)
        {
            return string.Format("AT*CTRL={0},{1},0\r", sequenceNumber, (int) _controlMode);
        }
    }
}