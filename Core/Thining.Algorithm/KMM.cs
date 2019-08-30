namespace Thining.Algorithm
{
    using System.Threading.Tasks;
    using Thining.Common.Consts;

    public class KMM : IAlgorithm
    {
        private const byte Zero = byte.MaxValue;

        private const byte One = byte.MinValue;

        private const byte Two = 32;

        private const byte Three = 64;

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

                        if (pixels[positionOfPixel] == One)
                        {
                            if (pixels[positionOfPixel + 1] == Zero || pixels[positionOfPixel - 1] == Zero ||
                                pixels[positionOfPixel + stride] == Zero || pixels[positionOfPixel - stride] == Zero)
                            {
                                pixels[positionOfPixel] = Two;
                            }
                            else if (pixels[positionOfPixel - stride + 1] == Zero || pixels[positionOfPixel - stride - 1] == Zero ||
                                     pixels[positionOfPixel + stride - 1] == Zero || pixels[positionOfPixel + stride - 1] == Zero)
                            {
                                pixels[positionOfPixel] = Three;
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

                        if (pixels[positionOfPixel] == Two)
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
                                if (stickPixels[i] != Zero)
                                {
                                    summary += this.consts.CompareList[i];
                                }
                            }

                            if (this.consts.DeleteList.Contains(summary))
                            {
                                pixels[positionOfPixel] = Zero;
                                deletion = true;
                            }
                        }
                    }
                });

                int n = 2;

                while (n <= 3)
                {
                    int value = n == 2 ? Two : Three;

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
                                    if (stickPixels[i] != Zero)
                                    {
                                        summary += this.consts.CompareList[i];
                                    }
                                }

                                if (this.consts.DeleteList.Contains(summary))
                                {
                                    pixels[positionOfPixel] = Zero;
                                    deletion = true;
                                }
                                else
                                {
                                    pixels[positionOfPixel] = Zero;
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
