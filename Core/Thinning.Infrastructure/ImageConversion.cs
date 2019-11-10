namespace Thinning.Infrastructure
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;
    using Accord.Imaging.Filters;
    using Thinning.Infrastructure.Interfaces;

    public class ImageConversion : IImageConversion
    {
        public Bitmap Create8bppGreyscaleImage(Bitmap bitmap) => Grayscale.CommonAlgorithms.BT709.Apply(bitmap);

        public Bitmap CreateNonIndexedImage(Bitmap bitmap)
        {
            Bitmap resultBmp = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(resultBmp))
            {
                graphics.DrawImage(bitmap, 0, 0);
            }

            return resultBmp;
        }

        public BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public Bitmap Binarize(Bitmap bitmap)
        {
            int threshold = this.CalculateOtsuValue(bitmap);

            int pixelBPP = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;

            unsafe
            {
                BitmapData bmpData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadWrite,
                    bitmap.PixelFormat);

                byte* ptr = (byte*)bmpData.Scan0;

                int height = bitmap.Height;
                int width = bitmap.Width * pixelBPP;

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

                bitmap.UnlockBits(bmpData);
            }

            return bitmap;
        }

        private int CalculateOtsuValue(Bitmap tempBmp)
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
