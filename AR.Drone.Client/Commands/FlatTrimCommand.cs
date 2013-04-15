namespace AR.Drone.Client.Commands
{
    public class FlatTrimCommand : ATCommand
    {
        protected override string ToAt(int sequenceNumber)
        {
            return string.Format("AT*FTRIM={0}\r", sequenceNumber);
        }
    }
}