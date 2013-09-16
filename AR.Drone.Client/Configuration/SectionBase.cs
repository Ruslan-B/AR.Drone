using System;

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

        protected int GetInt32(string index) { throw new NotImplementedException(); }
        protected string GetString(string index) { throw new NotImplementedException(); }

        protected void Set(string index, string value)
        {
            throw new NotImplementedException();
        }
    }

    public class Int32Indexer
    {

    }

    public interface IIndexer<T>
    {
        T this[string index]  { get; set; }
    }

}