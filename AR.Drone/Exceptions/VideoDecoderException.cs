using System;

namespace AR.Drone.Exceptions
{
    public class VideoDecoderException : Exception
    {
        public VideoDecoderException(string message) : base(message)
        {
        }
    }
}