namespace Thinning.Contracts.Algorithm
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Threading;
    using Thinning.Contracts.Interfaces;
    using Thinning.Infrastructure.Interfaces;
    using Thinning.Infrastructure.Models;

    public class TestWorker : ITestWorker
    {
        private readonly List<IAlgorithm> algorithms;

        private readonly IApplicationSetup applicationSetup;

        private readonly IImageConversion imageConversion;

        public TestWorker(IApplicationSetup applicationSetup, IImageConversion imageConversion)
        {
            this.applicationSetup = applicationSetup;
            this.algorithms = this.applicationSetup.GetRegisteredAlgorithmInstances();
            this.imageConversion = imageConversion;
        }

        public Bitmap PrepareBitmapToTestRun(Bitmap bitmap)
        {
            Thread.Sleep(500);

            bitmap = this.imageConversion.Binarize(bitmap);
            bitmap = this.imageConversion.Create8bppGreyscaleImage(bitmap);

            return bitmap;
        }

        public byte[][] PrepareTestSamples(Bitmap bitmap, int iterations, int algorithmsCount, out int stride)
        {
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite,
                bitmap.PixelFormat);

            int pixelsCount = bitmapData.Stride * bitmap.Height;
            byte[][] testSamples = new byte[iterations * algorithmsCount][];

            for (int i = 0; i < iterations * algorithmsCount; i++)
            {
                testSamples[i] = new byte[pixelsCount];
                Marshal.Copy(bitmapData.Scan0, testSamples[i], 0, pixelsCount);
            }

            stride = bitmapData.Stride;
            bitmap.UnlockBits(bitmapData);

            return testSamples;
        }

        public TestResult RunAllAlgorithmsTestInterations(
            Bitmap bitmap,
            int stride,
            byte[][] testSamples,
            int iterations,
            IProgress<int> progress,
            CancellationToken cancellationToken)
        {
            var resultTimes = new List<List<double>>();

            int sample = 0;
            int progressValue = 1;
            int algorithmCount = 0;

            foreach (var algorithm in this.algorithms)
            {
                resultTimes.Add(new List<double>());

                for (int i = 0; i < iterations; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return null;
                    }

                    double executionTime;
                    testSamples[sample] = algorithm.Execute(testSamples[sample], stride, bitmap.Height, bitmap.Width, out executionTime);
                    resultTimes[algorithmCount].Add(executionTime);

                    sample++;
                    progress.Report(progressValue++);
                }

                algorithmCount++;
            }

            return new TestResult
            {
                ResultTimes = resultTimes,
            };
        }

        public TestResult ByteArraysToBitmapResults(TestResult resultTimes, int iterations, byte[][] researchSamples, Bitmap bitmap)
        {
            var resultBitmaps = new List<Bitmap>();
            foreach (var timesList in resultTimes.ResultTimes)
            {
                resultBitmaps.Add(new Bitmap(bitmap.Width, bitmap.Height));
            }

            var sample = 0;

            foreach (var bmp in resultBitmaps)
            {
                var bitmapData = bmp.LockBits(
                    new Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.ReadWrite,
                    bitmap.PixelFormat);

                Marshal.Copy(researchSamples[sample], 0, bitmapData.Scan0, researchSamples[sample].Length);
                bmp.UnlockBits(bitmapData);

                sample += iterations;
            }

            return new TestResult
            {
                ResultTimes = resultTimes.ResultTimes,
                ResultBitmaps = resultBitmaps,
            };
        }
    }
}
