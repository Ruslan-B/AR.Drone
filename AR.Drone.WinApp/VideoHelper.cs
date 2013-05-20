using System;
using System.Drawing;
using System.Drawing.Imaging;
using AR.Drone.Client.Video;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using VideoPixelFormat = AR.Drone.Client.Video.PixelFormat;
using System.Runtime.InteropServices;

namespace AR.Drone.WinApp
{
    public static class VideoHelper
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
            PixelFormat pixelFormat = ConvertPixelFormat(frame.PixelFormat);
            var bitmap = new Bitmap(frame.Width, frame.Height, pixelFormat);
            if (pixelFormat == PixelFormat.Format8bppIndexed)
            {
                ColorPalette palette = bitmap.Palette;
                for (int i = 0; i < palette.Entries.Length; i++)
                {
                    palette.Entries[i] = Color.FromArgb(i, i, i);
                }
                bitmap.Palette = palette;
            }
            Rectangle rect = new Rectangle(0, 0, frame.Width, frame.Height);
            BitmapData data = bitmap.LockBits(rect, ImageLockMode.WriteOnly, pixelFormat);
            Marshal.Copy(frame.Data, 0, data.Scan0, frame.Data.Length);
            bitmap.UnlockBits(data);
            return bitmap;
        }
    }
}