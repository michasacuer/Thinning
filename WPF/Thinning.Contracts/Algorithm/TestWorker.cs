namespace Thinning.Contracts.Algorithm
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Threading;
    using Thinning.Infrastructure.Image.Preprocessing;
    using Thinning.Infrastructure.Interfaces;
    using Thinning.Infrastructure.Models;

    public class TestWorker : ITestWorker
    {
        private readonly List<IAlgorithm> algorithms;

        public TestWorker(IK3M k3M, IKMM kMM, IZhangSuen zhangSuen)
        {
            this.algorithms = new List<IAlgorithm>(new IAlgorithm[] { k3M, kMM, zhangSuen });
        }

        public Bitmap PrepareBitmapToTestRun(Bitmap bitmap)
        {
            Thread.Sleep(500);

            var conversion = new Conversion();
            bitmap = conversion.Binarize(bitmap);
            bitmap = conversion.Create8bppGreyscaleImage(bitmap);

            return bitmap;
        }

        public byte[][] PrepareTestSamples(Bitmap bitmap, int iterations, out BitmapData bitmapData)
        {
            bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite,
                bitmap.PixelFormat);

            int pixelsCount = bitmapData.Stride * bitmap.Height;
            byte[][] testSamples = new byte[iterations * 3][];

            for (int i = 0; i < iterations * 3; i++)
            {
                testSamples[i] = new byte[pixelsCount];
                Marshal.Copy(bitmapData.Scan0, testSamples[i], 0, pixelsCount);
            }

            return testSamples;
        }

        public TestResult RunAllAlgorithmsTestInterations(
            Bitmap bitmap,
            BitmapData bitmapData,
            byte[][] testSamples,
            int iterations,
            IProgress<int> progress,
            CancellationToken cancellationToken)
        {
            var k3MResultTimes = new List<double>();
            var kMMResultTimes = new List<double>();
            var zhangSuenResultTimes = new List<double>();

            int sample = 0;
            int progressValue = 1;
            int whichAlgorithm = 0;

            var stopwatch = new Stopwatch();

            try
            {
                foreach (var algorithm in this.algorithms)
                {
                    for (int i = 0; i < iterations; i++)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            bitmap.UnlockBits(bitmapData);
                            cancellationToken.ThrowIfCancellationRequested();
                        }

                        stopwatch.Start();
                        testSamples[sample] = algorithm.Execute(testSamples[sample], bitmapData.Stride, bitmap.Height, bitmap.Width);
                        stopwatch.Stop();

                        switch (whichAlgorithm)
                        {
                            case 0:
                                k3MResultTimes.Add((double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000);
                                break;
                            case 1:
                                kMMResultTimes.Add((double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000);
                                break;
                            case 2:
                                zhangSuenResultTimes.Add((double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000);
                                break;
                        }

                        stopwatch.Reset();
                        sample++;
                        progress.Report(progressValue++);
                    }

                    whichAlgorithm++;
                }

                return new TestResult
                {
                    K3MResultTimes = k3MResultTimes,
                    KMMResultTimes = kMMResultTimes,
                    ZhangSuenResultTimes = zhangSuenResultTimes,
                };
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        public TestResult ByteArraysToBitmapResults(byte[][] researchSamples, Bitmap bitmap)
        {
            var k3MResultBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            var kMMResultBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            var zhangSuenResultBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            BitmapData k3MBmpData = k3MResultBitmap.LockBits(
               new Rectangle(0, 0, k3MResultBitmap.Width, k3MResultBitmap.Height),
               ImageLockMode.ReadWrite,
               bitmap.PixelFormat);

            Marshal.Copy(researchSamples[1], 0, k3MBmpData.Scan0, researchSamples[1].Length);
            k3MResultBitmap.UnlockBits(k3MBmpData);

            BitmapData kMMBmpData = kMMResultBitmap.LockBits(
                new Rectangle(0, 0, kMMResultBitmap.Width, kMMResultBitmap.Height),
                ImageLockMode.ReadWrite,
                bitmap.PixelFormat);

            Marshal.Copy(researchSamples[21], 0, kMMBmpData.Scan0, researchSamples[21].Length);
            kMMResultBitmap.UnlockBits(kMMBmpData);

            BitmapData zhangSuenBmpData = zhangSuenResultBitmap.LockBits(
                new Rectangle(0, 0, zhangSuenResultBitmap.Width, zhangSuenResultBitmap.Height),
                ImageLockMode.ReadWrite,
                bitmap.PixelFormat);

            Marshal.Copy(researchSamples[41], 0, zhangSuenBmpData.Scan0, researchSamples[41].Length);
            zhangSuenResultBitmap.UnlockBits(zhangSuenBmpData);

            return new TestResult
            {
                K3MBitmapResult = k3MResultBitmap,
                KMMBitmapResult = kMMResultBitmap,
                ZhangSuenBitmapResult = zhangSuenResultBitmap,
            };
        }
    }
}
