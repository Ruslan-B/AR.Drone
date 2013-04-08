using System;

namespace AR.Drone.Video.Exceptions
{
    [Serializable]
    public class VideoDecoderException : Exception
    {
        public VideoDecoderException(string message) : base(message)
        {
        }
    }
}