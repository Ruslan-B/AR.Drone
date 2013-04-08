namespace AR.Drone.Command
{
    public class RefCommand : ATCommand
    {
        private readonly RefMode _refMode;

        public RefCommand(RefMode refMode)
        {
            _refMode = refMode;
        }

        protected override string ToAt(int sequenceNumber)
        {
            return string.Format("AT*REF={0},{1}\r", sequenceNumber, (int) _refMode);
        }
    }
}