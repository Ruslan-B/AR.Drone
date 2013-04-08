using System;

namespace AR.Drone.Client.Video.Exceptions
{
    [Serializable]
    public class VideoDecoderException : Exception
    {
        public VideoDecoderException(string message) : base(message)
        {
        }
    }
}