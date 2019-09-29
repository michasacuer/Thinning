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
        private IKMM kMM;

        private IZhangSuen zhangSuen;

        public Test(IKMM kMM, IZhangSuen zhangSuen)
        {
            this.kMM = kMM;
            this.zhangSuen = zhangSuen;
        }

        public TestResult Run(string imageFilepath, IProgress<int> progress)
        {
            var bitmap = new Bitmap(imageFilepath);

            bitmap = this.PrepareBitmapToTestRun(bitmap);

            BitmapData bmpData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite,
                bitmap.PixelFormat);

            int bytes = bmpData.Stride * bitmap.Height;

            byte[][] researchSamples = new byte[30][];

            for (int i = 0; i < 30; i++)
            {
                researchSamples[i] = new byte[bytes];
                Marshal.Copy(bmpData.Scan0, researchSamples[i], 0, bytes);
            }

            int height = bitmap.Height;
            int width = bitmap.Width;

            var stopwatch = new Stopwatch();

            var kMMResultTimes = new List<double>();
            var zhangSuenResultTimes = new List<double>();
            int sample = 0;
            int progressValue = 1;

            for (int i = 0; i < 10; i++)
            {
                stopwatch.Start();
                researchSamples[sample] = this.kMM.Execute(researchSamples[sample], bmpData.Stride, height, width);
                stopwatch.Stop();

                kMMResultTimes.Add((double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000);

                stopwatch.Reset();
                sample++;
                progress.Report(progressValue += 4);
            }

            for (int i = 0; i < 10; i++)
            {
                stopwatch.Start();
                researchSamples[sample] = this.zhangSuen.Execute(researchSamples[sample], bmpData.Stride, height, width);
                stopwatch.Stop();

                zhangSuenResultTimes.Add((double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000);

                stopwatch.Reset();
                sample++;
                progress.Report(progressValue += 4);
            }

            bitmap.UnlockBits(bmpData);

            var kMMResultBitmap = new Bitmap(width, height);
            var zhangSuenResultBitmap = new Bitmap(width, height);

            BitmapData kMMBmpData = kMMResultBitmap.LockBits(
                new Rectangle(0, 0, kMMResultBitmap.Width, kMMResultBitmap.Height),
                ImageLockMode.ReadWrite,
                bitmap.PixelFormat);

            Marshal.Copy(researchSamples[0], 0, kMMBmpData.Scan0, researchSamples[0].Length);
            kMMResultBitmap.UnlockBits(kMMBmpData);

            BitmapData zhangSuenBmpData = zhangSuenResultBitmap.LockBits(
                new Rectangle(0, 0, zhangSuenResultBitmap.Width, zhangSuenResultBitmap.Height),
                ImageLockMode.ReadWrite,
                bitmap.PixelFormat);

            Marshal.Copy(researchSamples[10], 0, zhangSuenBmpData.Scan0, researchSamples[10].Length);
            zhangSuenResultBitmap.UnlockBits(zhangSuenBmpData);

            progress.Report(progressValue += 50);

            return new TestResult
            {
                KMMBitmapResult = kMMResultBitmap,
                ZhangSuenBitmapResult = zhangSuenResultBitmap,
                KMMResultTimes = kMMResultTimes,
                ZhangSuenResultTimes = zhangSuenResultTimes,
            };
        }

        private Bitmap PrepareBitmapToTestRun(Bitmap bitmap)
        {
            var conversion = new Conversion();

            Thread.Sleep(500);

            bitmap = conversion.CreateNonIndexedImage(bitmap);
            bitmap = conversion.Binarize(bitmap);
            bitmap = conversion.Create8bppGreyscaleImage(bitmap);

            return bitmap;
        }
    }
}
