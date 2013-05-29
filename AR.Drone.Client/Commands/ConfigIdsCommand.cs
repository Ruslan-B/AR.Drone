namespace AR.Drone.Client.Commands
{
    public class ConfigIdsCommand : ATCommand
    {
        private readonly string _sessionId;
        private readonly string _userId;
        private readonly string _applicationId;

        public ConfigIdsCommand(string sessionId, string userId, string applicationId)
        {
            _sessionId = sessionId;
            _userId = userId;
            _applicationId = applicationId;
        }

        protected override string ToAt(int sequenceNumber)
        {
            return string.Format("AT*CONFIG_IDS={0},\"{1}\",\"{2}\",\"{3}\"\r", sequenceNumber, _sessionId, _userId, _applicationId);
        }
    }
}