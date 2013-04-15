namespace AR.Drone.Client.Configuration
{
    public interface IConfigurationItem
    {
        string Key { get; }
        object Value { get; }
        bool TryUpdate(string value);
    }
}