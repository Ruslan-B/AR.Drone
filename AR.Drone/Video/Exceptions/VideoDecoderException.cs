using System;

namespace AR.Drone.Video.Exceptions
{
    public class VideoDecoderException : Exception
    {
        public VideoDecoderException(string message) : base(message)
        {
        }
    }
}