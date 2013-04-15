namespace AR.Drone.Client.Commands
{
    public class ControlCommand : ATCommand
    {
        private readonly ControlMode _controlMode;

        public ControlCommand(ControlMode controlMode)
        {
            _controlMode = controlMode;
        }

        protected override string ToAt(int sequenceNumber)
        {
            return string.Format("AT*CTRL={0},{1},0\r", sequenceNumber, (int) _controlMode);
        }
    }
}