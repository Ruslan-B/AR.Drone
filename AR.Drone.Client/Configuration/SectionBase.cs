namespace AR.Drone.Client.Configuration
{
    public class SectionBase
    {
        private readonly DroneConfiguration _configuration;
        private readonly string _name;

        public SectionBase(DroneConfiguration configuration, string name)
        {
            _configuration = configuration;
            _name = name;
        }

        public DroneConfiguration Configuration
        {
            get { return _configuration; }
        }

        public string Name
        {
            get { return _name; }
        }
    }
}