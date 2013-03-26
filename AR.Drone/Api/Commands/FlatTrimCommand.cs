namespace AR.Drone.Api.Commands
{
    public class FlatTrimCommand : IATCommand
    {
        public string ToAt(int sequenceNumber)
        {
            return string.Format("AT*FTRIM={0}\r", sequenceNumber);
        }
    }
}