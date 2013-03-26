namespace AR.Drone.Api.Commands
{
    public class COMWDGCommand : IATCommand
    {
        public string ToAt(int sequenceNumber)
        {
            return string.Format("AT*COMWDG={0}\r", sequenceNumber);
        }
    }
}