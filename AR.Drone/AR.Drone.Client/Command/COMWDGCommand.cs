namespace AR.Drone.Command
{
    public class COMWDGCommand : ATCommand
    {
        protected override string ToAt(int sequenceNumber)
        {
            return string.Format("AT*COMWDG={0}\r", sequenceNumber);
        }
    }
}