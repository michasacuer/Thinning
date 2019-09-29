namespace Thinning.Algorithm
{
    using System.Threading.Tasks;
    using Thinning.Common.Consts;
    using Thinning.Infrastructure.Interfaces;

    public class KMM : IKMM
    {
        private KMMConsts consts;

        public KMM()
        {
            this.consts = new KMMConsts();
        }

        public byte[] Execute(byte[] pixels, int stride, int height, int width)
        {
            bool deletion = true;

            while (deletion)
            {
                deletion = false;

                Parallel.For(0, height - 1, y =>
                {
                    int offset = y * stride;

                    for (int x = 0; x < width - 1; x++)
                    {
                        int positionOfPixel = x + offset;

                        if (pixels[positionOfPixel] == Value.One)
                        {
                            if (pixels[positionOfPixel + 1] == Value.Zero || pixels[positionOfPixel - 1] == Value.Zero ||
                                pixels[positionOfPixel + stride] == Value.Zero || pixels[positionOfPixel - stride] == Value.Zero)
                            {
                                pixels[positionOfPixel] = Value.Two;
                            }
                            else if (pixels[positionOfPixel - stride + 1] == Value.Zero || pixels[positionOfPixel - stride - 1] == Value.Zero ||
                                     pixels[positionOfPixel + stride - 1] == Value.Zero || pixels[positionOfPixel + stride - 1] == Value.Zero)
                            {
                                pixels[positionOfPixel] = Value.Three;
                            }
                        }
                    }
                });

                Parallel.For(0, height, y =>
                {
                    int offset = y * stride;

                    for (int x = 0; x < width; x++)
                    {
                        int positionOfPixel = x + offset;

                        if (pixels[positionOfPixel] == Value.Two)
                        {
                            int summary = 0;

                            var stickPixels = new int[8]
                            {
                                pixels[positionOfPixel - stride - 1],
                                pixels[positionOfPixel - stride],
                                pixels[positionOfPixel - stride + 1],
                                pixels[positionOfPixel - 1],
                                pixels[positionOfPixel + 1],
                                pixels[positionOfPixel + stride - 1],
                                pixels[positionOfPixel + stride],
                                pixels[positionOfPixel + stride + 1],
                            };

                            for (int i = 0; i < stickPixels.Length; i++)
                            {
                                if (stickPixels[i] != Value.Zero)
                                {
                                    summary += this.consts.CompareList[i];
                                }
                            }

                            if (this.consts.DeleteList.Contains(summary))
                            {
                                pixels[positionOfPixel] = Value.Zero;
                                deletion = true;
                            }
                        }
                    }
                });

                int n = 2;

                while (n <= 3)
                {
                    int value = n == 2 ? Value.Two : Value.Three;

                    Parallel.For(0, height, y =>
                    {
                        int offset = y * stride;

                        for (int x = 0; x < width; x++)
                        {
                            int positionOfPixel = x + offset;

                            if (pixels[positionOfPixel] == value)
                            {
                                int summary = 0;

                                var stickPixels = new int[8]
                                {
                                    pixels[positionOfPixel - stride - 1],
                                    pixels[positionOfPixel - stride],
                                    pixels[positionOfPixel - stride + 1],
                                    pixels[positionOfPixel - 1],
                                    pixels[positionOfPixel + 1],
                                    pixels[positionOfPixel + stride - 1],
                                    pixels[positionOfPixel + stride],
                                    pixels[positionOfPixel + stride + 1],
                                };

                                for (int i = 0; i < stickPixels.Length; i++)
                                {
                                    if (stickPixels[i] != Value.Zero)
                                    {
                                        summary += this.consts.CompareList[i];
                                    }
                                }

                                if (this.consts.DeleteList.Contains(summary))
                                {
                                    pixels[positionOfPixel] = Value.Zero;
                                    deletion = true;
                                }
                                else
                                {
                                    pixels[positionOfPixel] = Value.One;
                                }
                            }
                        }
                    });

                    n++;
                }
            }

            return pixels;
        }
    }
}
