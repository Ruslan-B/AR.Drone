namespace AR.Drone.Client.Configuration
{
    public class NetworkConfiguration
    {
        public NetworkConfiguration(string hostname)
        {
            DroneHostname = hostname;
        }

        public string DroneHostname { get; private set; }
    }
}