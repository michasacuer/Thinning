namespace Thinning.Algorithm
{
    using System;
    using System.Threading.Tasks;
    using Thinning.Common.Consts;
    using Thinning.Infrastructure.Interfaces;

    public class ZhangSuen : IZhangSuen
    {
        public byte[] Execute(byte[] pixels, int stride, int height, int width)
        {
            bool deletion = true;

            var temp = new byte[pixels.Length];
            var result = new byte[pixels.Length];

            Buffer.BlockCopy(pixels, 0, temp, 0, pixels.Length);

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
                            int p2 = temp[positionOfPixel - stride];
                            int p3 = temp[positionOfPixel - stride + 1];
                            int p4 = temp[positionOfPixel + 1];
                            int p5 = temp[positionOfPixel + stride + 1];
                            int p6 = temp[positionOfPixel + stride];
                            int p7 = temp[positionOfPixel + stride - 1];
                            int p8 = temp[positionOfPixel - 1];
                            int p9 = temp[positionOfPixel - stride - 1];

                            int neighbours = p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9;

                            if (neighbours >= 510 && neighbours <= 1530)
                            {
                                int transitionsToBlack
                                    = (p2 & ~p3) + (p3 & ~p4) + (p4 & ~p5) + (p5 & ~p6) + (p6 & ~p7) + (p7 & ~p8) + (p8 & ~p9) + (p9 & ~p2);

                                if (transitionsToBlack == 255)
                                {
                                    if ((~p2 & ~p4 & ~p8) != -1 && (~p2 & ~p6 & ~p8) != -1)
                                    {
                                        pixels[positionOfPixel] = Value.Zero;
                                        deletion = true;
                                    }
                                }
                            }
                        }
                    }
                });

                Buffer.BlockCopy(pixels, 0, temp, 0, pixels.Length);

                if (!deletion)
                {
                    break;
                }

                Parallel.For(0, height - 1, y =>
                {
                    int offset = y * stride;

                    for (int x = 0; x < width - 1; x++)
                    {
                        int positionOfPixel = x + offset;

                        if (pixels[positionOfPixel] == Value.One)
                        {
                            int p2 = temp[positionOfPixel - stride];
                            int p3 = temp[positionOfPixel - stride + 1];
                            int p4 = temp[positionOfPixel + 1];
                            int p5 = temp[positionOfPixel + stride + 1];
                            int p6 = temp[positionOfPixel + stride];
                            int p7 = temp[positionOfPixel + stride - 1];
                            int p8 = temp[positionOfPixel - 1];
                            int p9 = temp[positionOfPixel - stride - 1];

                            int neighbours = p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9;

                            if (neighbours >= 510 && neighbours <= 1530)
                            {
                                int transitionsToBlack
                                    = (p2 & ~p3) + (p3 & ~p4) + (p4 & ~p5) + (p5 & ~p6) + (p6 & ~p7) + (p7 & ~p8) + (p8 & ~p9) + (p9 & ~p2);

                                if (transitionsToBlack == 255)
                                {
                                    if ((~p2 & ~p4 & ~p6) != -1 && (~p4 & ~p6 & ~p8) != -1)
                                    {
                                        pixels[positionOfPixel] = Value.Zero;
                                        deletion = true;
                                    }
                                }
                            }
                        }
                    }
                });

                Buffer.BlockCopy(pixels, 0, temp, 0, pixels.Length);
            }

            return pixels;
        }
    }
}
