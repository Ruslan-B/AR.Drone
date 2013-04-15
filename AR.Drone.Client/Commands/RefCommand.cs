namespace AR.Drone.Client.Commands
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

    public enum RefMode
    {
        Default = 1 << 18 | 1 << 20 | 1 << 22 | 1 << 24 | 1 << 28,
        Land = (0 << 9) | Default,
        Takeoff = (1 << 9) | Default,
        Emergency = (1 << 8) | Default
    }
}