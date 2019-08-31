namespace Thining.Infrastructure.Image.Preprocessing
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Threading.Tasks;

    public class Binarization
    {
        public Bitmap Execute(Bitmap resultBmp)
        {
            int threshold = this.OtsuValue(resultBmp);

            int pixelBPP = Image.GetPixelFormatSize(resultBmp.PixelFormat) / 8;

            unsafe
            {
                BitmapData bmpData = resultBmp.LockBits(new Rectangle(0, 0, resultBmp.Width, resultBmp.Height), ImageLockMode.ReadWrite, resultBmp.PixelFormat);

                byte* ptr = (byte*)bmpData.Scan0;

                int height = resultBmp.Height;
                int width = resultBmp.Width * pixelBPP;

                Parallel.For(0, height, y =>
                {
                    byte* offset = ptr + (y * bmpData.Stride);

                    for (int x = 0; x < width; x = x + pixelBPP)
                    {
                        byte value = (offset[x] + offset[x + 1] + offset[x + 2]) / 3 > threshold ? byte.MaxValue : byte.MinValue;
                        offset[x] = value;
                        offset[x + 1] = value;
                        offset[x + 2] = value;

                        if (pixelBPP == 4)
                        {
                            offset[x + 3] = 255;
                        }
                    }
                });

                resultBmp.UnlockBits(bmpData);
            }

            return resultBmp;
        }

        private int OtsuValue(Bitmap tempBmp)
        {
            int x;
            int y;
            int[] histogram = new int[256];

            for (y = 0; y < tempBmp.Height; y++)
            {
                for (x = 0; x < tempBmp.Width; x++)
                {
                    Color color = tempBmp.GetPixel(x, y);
                    int pixelValue = (color.R + color.G + color.B) / 3;
                    histogram[pixelValue]++;
                }
            }

            int total = tempBmp.Height * tempBmp.Width;
            float summary = 0;

            for (int i = 0; i < 256; i++)
            {
                summary += i * histogram[i];
            }

            float summary2 = 0;
            x = 0;
            y = 0;

            float max = 0;
            int threshold = 0;

            for (int i = 0; i < 256; i++)
            {
                x += histogram[i];

                if (x == 0)
                {
                    continue;
                }

                y = total - x;

                if (y == 0)
                {
                    break;
                }

                summary2 += i * histogram[i];
                float o = summary2 / x;
                float p = (summary - summary2) / y;

                float between = x * y * (o - p) * (o - p);

                if (between > max)
                {
                    max = between;
                    threshold = i;
                }
            }

            return threshold;
        }
    }
}
