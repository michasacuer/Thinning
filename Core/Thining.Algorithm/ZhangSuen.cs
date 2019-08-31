namespace Thining.Algorithm
{
    using System;
    using System.Threading.Tasks;
    using Thining.Common.Consts;

    public class ZhangSuen : IAlgorithm
    {
        public byte[] Execute(byte[] pixels, int stride, int height, int width)
        {
            bool deletion = true;

            var temp = new byte[pixels.Length];
            var result = new byte[pixels.Length];

            while (deletion)
            {
                Array.Copy(pixels, temp, pixels.Length);

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

                if (!deletion)
                {
                    break;
                }

                Array.Copy(pixels, temp, pixels.Length);

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

                Array.Copy(pixels, temp, pixels.Length);
            }

            return pixels;
        }
    }
}
