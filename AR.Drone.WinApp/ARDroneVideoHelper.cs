using System;
using System.Drawing;
using System.Drawing.Imaging;
using AR.Drone.Client.Video;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using VideoPixelFormat = AR.Drone.Client.Video.PixelFormat;

namespace AR.Drone.WinApp
{
    public static class ARDroneVideoHelper
    {
        public static PixelFormat ConvertPixelFormat(VideoPixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case VideoPixelFormat.Gray8:
                    return PixelFormat.Format8bppIndexed;
                case VideoPixelFormat.BGR24:
                    return PixelFormat.Format24bppRgb;
                case VideoPixelFormat.RGB24:
                    throw new NotSupportedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static unsafe Bitmap CreateImageFromFrame(VideoFrame frame)
        {
            fixed (void* pData = &frame.Data[0, 0, 0])
            {
                PixelFormat pixelFormat = ConvertPixelFormat(frame.PixelFormat);
                int height = frame.Data.GetLength(0);
                int width = frame.Data.GetLength(1);
                int depth = frame.Data.GetLength(2);
                var bitmap = new Bitmap(width, height, width*depth, pixelFormat, new IntPtr(pData));
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