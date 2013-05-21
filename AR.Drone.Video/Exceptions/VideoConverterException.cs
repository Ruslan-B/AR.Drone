using System;

namespace AR.Drone.Video.Exceptions
{
    public class VideoConverterException : Exception
    {
        public VideoConverterException(string message)
            : base(message)
        {
        }
    }
}