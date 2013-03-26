using System;
using System.Drawing;
using System.Drawing.Imaging;
using AR.Drone.Api.Video;

namespace AR.Drone.WinApp
{
    public static class ARDroneVideoHelper
    {
        public static PixelFormat ConvertPixelFormat(VideoFramePixelFormat inputPixelFormat)
        {
            switch (inputPixelFormat)
            {
                case VideoFramePixelFormat.Gray8:
                    return PixelFormat.Format8bppIndexed;
                case VideoFramePixelFormat.RGB24:
                    return PixelFormat.Format24bppRgb;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static unsafe Bitmap CreateImageFromFrame(VideoFrame frame)
        {
            fixed (void* pData = &frame.Data[0, 0, 0])
            {
                PixelFormat pixelFormat = ConvertPixelFormat(frame.PixelFormat);
                var bitmap = new Bitmap(frame.Width, frame.Height, frame.Width*frame.Depth, pixelFormat, new IntPtr(pData));
                if (pixelFormat == PixelFormat.Format8bppIndexed)
                {
                    ColorPalette palette = bitmap.Palette;
                    for (int i = 0; i < palette.Entries.Length; i++)
                    {
                        palette.Entries[i] = Color.FromArgb(i, i, i);
                    }
                    bitmap.Palette = palette;
                }
                return bitmap;
            }
        }
    }
}