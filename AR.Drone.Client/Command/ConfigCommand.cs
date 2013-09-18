namespace AR.Drone.Client.Command
{
    public class ConfigCommand : AtCommand
    {
        private readonly string _key;
        private readonly string _value;

        public ConfigCommand(string key, string value)
        {
            _key = key;
            _value = value;
        }

        protected override string ToAt(int sequenceNumber)
        {
            return string.Format("AT*CONFIG={0},\"{1}\",\"{2}\"\r", sequenceNumber, _key, _value);
        }
    }
}