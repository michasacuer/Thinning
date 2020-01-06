namespace Thinning.Algorithm
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Thinning.Algorithm.Consts;
    using Thinning.Algorithm.Interfaces;

    public class KMM : IKMM
    {
        private KMMConsts consts;

        private Stopwatch stopwatch;

        public KMM()
        {
            this.stopwatch = new Stopwatch();
            this.consts = new KMMConsts();
        }

        public byte[] Execute(byte[] pixels, int stride, int height, int width, out double executionTime)
        {
            bool deletion = true;

            while (deletion)
            {
                this.stopwatch.Start();

                deletion = false;

                Parallel.For(1, height - 1, y =>
                {
                    int offset = y * stride;

                    for (int x = 1; x < width - 1; x++)
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

                Parallel.For(1, height, y =>
                {
                    int offset = y * stride;

                    for (int x = 1; x < width; x++)
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

                            if (this.consts.DeleteFourList.Contains(summary))
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

                    Parallel.For(1, height, y =>
                    {
                        int offset = y * stride;

                        for (int x = 1; x < width; x++)
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

            this.stopwatch.Stop();
            executionTime = (double)this.stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000;
            this.stopwatch.Restart();

            return pixels;
        }
    }
}
