namespace AR.Drone.Client.Command
{
    internal class ComWdgCommand : AtCommand
    {
        public static ComWdgCommand Default = new ComWdgCommand();

        private ComWdgCommand()
        {
        }

        protected override string ToAt(int sequenceNumber)
        {
            return string.Format("AT*COMWDG={0}\r", sequenceNumber);
        }
    }
}