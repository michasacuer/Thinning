namespace Thinning.Algorithm
{
    using System;
    using System.Threading.Tasks;
    using Thinning.Common.Consts;
    using Thinning.Infrastructure.Interfaces;

    public class K3M : IK3M
    {
        private K3MConsts consts;

        public K3M()
        {
            this.consts = new K3MConsts();
        }

        public byte[] Execute(byte[] pixels, int stride, int height, int width)
        {
            bool deletion = true;

            var temp = new byte[pixels.Length];
            var result = new byte[pixels.Length];
            Buffer.BlockCopy(pixels, 0, temp, 0, pixels.Length);

            while (deletion)
            {
                deletion = false;

                Parallel.For(1, height - 1, y =>
                {
                    int offset = y * stride;

                    for (int x = 1; x < width - 1; x++)
                    {
                        int positionOfPixel = x + offset;
                        if (pixels[positionOfPixel] == Value.One)
                        {
                            int summary = 0;

                            var stickPixels = new int[8]
                            {
                                temp[positionOfPixel - stride - 1],
                                temp[positionOfPixel - stride],
                                temp[positionOfPixel - stride + 1],
                                temp[positionOfPixel - 1],
                                temp[positionOfPixel + 1],
                                temp[positionOfPixel + stride - 1],
                                temp[positionOfPixel + stride],
                                temp[positionOfPixel + stride + 1],
                            };

                            for (int i = 0; i < stickPixels.Length; i++)
                            {
                                if (stickPixels[i] == Value.One)
                                {
                                    summary += this.consts.CompareList[i];
                                }
                            }

                            if (this.consts.A0.Contains(summary))
                            {
                                pixels[positionOfPixel] = Value.Two;
                            }
                        }
                    }
                });

                Buffer.BlockCopy(pixels, 0, temp, 0, pixels.Length);

                for (int i = 1; i < this.consts.A.Length; i++)
                {
                    Parallel.For(1, height - 1, y =>
                    {
                        int offset = y * stride;

                        for (int x = 1; x < width - 1; x++)
                        {
                            int positionOfPixel = x + offset;
                            if (pixels[positionOfPixel] == Value.Two)
                            {
                                int summary = 0;

                                var stickPixels = new int[8]
                                {
                                    temp[positionOfPixel - stride - 1],
                                    temp[positionOfPixel - stride],
                                    temp[positionOfPixel - stride + 1],
                                    temp[positionOfPixel - 1],
                                    temp[positionOfPixel + 1],
                                    temp[positionOfPixel + stride - 1],
                                    temp[positionOfPixel + stride],
                                    temp[positionOfPixel + stride + 1],
                                };

                                for (int j = 0; j < stickPixels.Length; j++)
                                {
                                    if (stickPixels[j] != Value.Zero)
                                    {
                                        summary += this.consts.CompareList[j];
                                    }
                                }

                                if (this.consts.A[i].Contains(summary))
                                {
                                    pixels[positionOfPixel] = Value.Zero;
                                    deletion = true;
                                }
                            }
                        }
                    });

                    Buffer.BlockCopy(pixels, 0, temp, 0, pixels.Length);
                }

                for (int y = 1; y < height - 1; y++)
                {
                    int offset = y * stride;

                    for (int x = 1; x < width - 1; x++)
                    {
                        int positionOfPixel = x + offset;
                        if (pixels[positionOfPixel] != Value.Zero)
                        {
                            int summary = 0;

                            var stickPixels = new int[8]
                            {
                                        temp[positionOfPixel - stride - 1],
                                        temp[positionOfPixel - stride],
                                        temp[positionOfPixel - stride + 1],
                                        temp[positionOfPixel - 1],
                                        temp[positionOfPixel + 1],
                                        temp[positionOfPixel + stride - 1],
                                        temp[positionOfPixel + stride],
                                        temp[positionOfPixel + stride + 1],
                            };

                            for (int i = 0; i < stickPixels.Length; i++)
                            {
                                if (stickPixels[i] != Value.Zero)
                                {
                                    summary += this.consts.CompareList[i];
                                }
                            }

                            if (this.consts.A0pix.Contains(summary))
                            {
                                pixels[positionOfPixel] = Value.Zero;
                                deletion = true;
                            }
                        }
                    }
                }
            }

            Buffer.BlockCopy(pixels, 0, temp, 0, pixels.Length);

            return pixels;
        }
    }
}
