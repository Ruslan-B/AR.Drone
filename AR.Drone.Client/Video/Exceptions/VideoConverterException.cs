using System;

namespace AR.Drone.Client.Video.Exceptions
{
    [Serializable]
    public class VideoConverterException : Exception
    {
        public VideoConverterException(string message)
            : base(message)
        {
        }
    }
}