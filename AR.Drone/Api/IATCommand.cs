namespace AR.Drone.Api
{
    public interface IATCommand
    {
        string ToAt(int sequenceNumber);
    }
}