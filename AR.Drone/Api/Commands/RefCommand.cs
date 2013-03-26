namespace AR.Drone.Api.Commands
{
    public class RefCommand : IATCommand
    {
        private readonly RefMode _refMode;

        public RefCommand(RefMode refMode)
        {
            _refMode = refMode;
        }

        public string ToAt(int sequenceNumber)
        {
            return string.Format("AT*REF={0},{1}\r", sequenceNumber, (int) _refMode);
        }
    }
}