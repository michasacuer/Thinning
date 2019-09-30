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

    public class Test : ITest
    {
        private IK3M k3M;

        private IKMM kMM;

        private IZhangSuen zhangSuen;

        public Test(IK3M k3M, IKMM kMM, IZhangSuen zhangSuen)
        {
            this.k3M = k3M;
            this.kMM = kMM;
            this.zhangSuen = zhangSuen;
        }

        public TestResult Run(string imageFilepath, IProgress<int> progress, CancellationToken cancellationToken)
        {
            int testsCount = 20;
            var bitmap = new Bitmap(imageFilepath);

            bitmap = this.PrepareBitmapToTestRun(bitmap);

            BitmapData bmpData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite,
                bitmap.PixelFormat);

            int bytes = bmpData.Stride * bitmap.Height;

            byte[][] researchSamples = new byte[testsCount * 3][];

            for (int i = 0; i < testsCount * 3; i++)
            {
                researchSamples[i] = new byte[bytes];
                Marshal.Copy(bmpData.Scan0, researchSamples[i], 0, bytes);
            }

            int height = bitmap.Height;
            int width = bitmap.Width;

            var stopwatch = new Stopwatch();

            var k3MResultTimes = new List<double>();
            var kMMResultTimes = new List<double>();
            var zhangSuenResultTimes = new List<double>();
            int sample = 0;
            int progressValue = 1;

            try
            {
                for (int i = 0; i < testsCount; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        bitmap.UnlockBits(bmpData);
                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    stopwatch.Start();
                    researchSamples[sample] = this.k3M.Execute(researchSamples[sample], bmpData.Stride, height, width);
                    stopwatch.Stop();

                    k3MResultTimes.Add((double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000);

                    stopwatch.Reset();
                    sample++;
                    progress.Report(progressValue++);
                }

                for (int i = 0; i < testsCount; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        bitmap.UnlockBits(bmpData);
                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    stopwatch.Start();
                    researchSamples[sample] = this.kMM.Execute(researchSamples[sample], bmpData.Stride, height, width);
                    stopwatch.Stop();

                    kMMResultTimes.Add((double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000);

                    stopwatch.Reset();
                    sample++;
                    progress.Report(progressValue++);
                }

                for (int i = 0; i < testsCount; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        bitmap.UnlockBits(bmpData);
                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    stopwatch.Start();
                    researchSamples[sample] = this.zhangSuen.Execute(researchSamples[sample], bmpData.Stride, height, width);
                    stopwatch.Stop();

                    zhangSuenResultTimes.Add((double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000);

                    stopwatch.Reset();
                    sample++;
                    progress.Report(progressValue++);
                }

                bitmap.UnlockBits(bmpData);

                var testResult = this.ByteArraysToBitmapResults(researchSamples, width, height, bitmap.PixelFormat);

                testResult.K3MResultTimes = k3MResultTimes;
                testResult.KMMResultTimes = kMMResultTimes;
                testResult.ZhangSuenResultTimes = zhangSuenResultTimes;

                return testResult;
            }
            catch (OperationCanceledException)
            {
                return default;
            }
        }

        private Bitmap PrepareBitmapToTestRun(Bitmap bitmap)
        {
            var conversion = new Conversion();

            Thread.Sleep(500);

            bitmap = conversion.Binarize(bitmap);
            bitmap = conversion.Create8bppGreyscaleImage(bitmap);

            return bitmap;
        }

        private TestResult ByteArraysToBitmapResults(byte[][] researchSamples, int width, int height, PixelFormat originPixelFormat)
        {
            var k3MResultBitmap = new Bitmap(width, height);
            var kMMResultBitmap = new Bitmap(width, height);
            var zhangSuenResultBitmap = new Bitmap(width, height);

            BitmapData k3MBmpData = k3MResultBitmap.LockBits(
               new Rectangle(0, 0, k3MResultBitmap.Width, k3MResultBitmap.Height),
               ImageLockMode.ReadWrite,
               originPixelFormat);

            Marshal.Copy(researchSamples[1], 0, k3MBmpData.Scan0, researchSamples[1].Length);
            k3MResultBitmap.UnlockBits(k3MBmpData);

            BitmapData kMMBmpData = kMMResultBitmap.LockBits(
                new Rectangle(0, 0, kMMResultBitmap.Width, kMMResultBitmap.Height),
                ImageLockMode.ReadWrite,
                originPixelFormat);

            Marshal.Copy(researchSamples[21], 0, kMMBmpData.Scan0, researchSamples[21].Length);
            kMMResultBitmap.UnlockBits(kMMBmpData);

            BitmapData zhangSuenBmpData = zhangSuenResultBitmap.LockBits(
                new Rectangle(0, 0, zhangSuenResultBitmap.Width, zhangSuenResultBitmap.Height),
                ImageLockMode.ReadWrite,
                originPixelFormat);

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
