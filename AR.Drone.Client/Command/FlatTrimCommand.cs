namespace AR.Drone.Client.Command
{
    public class FlatTrimCommand : AtCommand
    {
        public static FlatTrimCommand Default = new FlatTrimCommand();

        private FlatTrimCommand()
        {
        }

        protected override string ToAt(int sequenceNumber)
        {
            return string.Format("AT*FTRIM={0}\r", sequenceNumber);
        }
    }
}