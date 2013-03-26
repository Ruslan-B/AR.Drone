namespace AR.Drone.NativeApi.Video
{
    public enum VideoEncapsulationFrameType : byte
    {
        Unknnown,
        IDR,
        I,
        P,
        Headers,
    }
}