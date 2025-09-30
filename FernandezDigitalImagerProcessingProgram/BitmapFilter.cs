using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageProcessingFilters
{
    public class BitmapFilter
    {
        public static bool Conv3x3(Bitmap b, ConvMatrix m)
        {
            if (0 == m.Factor) return false;

            Bitmap bSrc = (Bitmap)b.Clone();

            BitmapData bmData = b.LockBits(
                new Rectangle(0, 0, b.Width, b.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);

            BitmapData bmSrc = bSrc.LockBits(
                new Rectangle(0, 0, bSrc.Width, bSrc.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            IntPtr Scan0 = bmData.Scan0;
            IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width - 2;
                int nHeight = b.Height - 2;

                int nPixel;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        for (int c = 0; c < 3; c++) // process BGR separately
                        {
                            nPixel =
                                (pSrc[(2 + y) * stride + (2 + x) * 3 + c] * m.BottomRight) +
                                (pSrc[(2 + y) * stride + (1 + x) * 3 + c] * m.BottomMid) +
                                (pSrc[(2 + y) * stride + (0 + x) * 3 + c] * m.BottomLeft) +
                                (pSrc[(1 + y) * stride + (2 + x) * 3 + c] * m.MidRight) +
                                (pSrc[(1 + y) * stride + (1 + x) * 3 + c] * m.Pixel) +
                                (pSrc[(1 + y) * stride + (0 + x) * 3 + c] * m.MidLeft) +
                                (pSrc[(0 + y) * stride + (2 + x) * 3 + c] * m.TopRight) +
                                (pSrc[(0 + y) * stride + (1 + x) * 3 + c] * m.TopMid) +
                                (pSrc[(0 + y) * stride + (0 + x) * 3 + c] * m.TopLeft);

                            nPixel = nPixel / m.Factor + m.Offset;

                            nPixel = Math.Min(Math.Max(nPixel, 0), 255);

                            p[(1 + y) * stride + (1 + x) * 3 + c] = (byte)nPixel;
                        }
                    }
                }
            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);

            return true;
        }
    }
}
