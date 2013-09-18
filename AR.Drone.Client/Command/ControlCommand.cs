namespace AR.Drone.Client.Command
{
    public class ControlCommand : AtCommand
    {
        public static ControlCommand AckControlMode = new ControlCommand(ControlMode.AckControlMode);
        public static ControlCommand CfgGetControlMode = new ControlCommand(ControlMode.CfgGetControlMode);

        private readonly ControlMode _controlMode;

        private ControlCommand(ControlMode controlMode)
        {
            _controlMode = controlMode;
        }

        protected override string ToAt(int sequenceNumber)
        {
            return string.Format("AT*CTRL={0},{1},0\r", sequenceNumber, (int) _controlMode);
        }
    }
}