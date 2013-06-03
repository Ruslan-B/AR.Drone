namespace AR.Drone.Client
{
    internal enum StateRequest
    {
        None,
        Initialization,
        Configuration,
        Land,
        Fly,
        Emergency,
        ResetEmergency
    }
}