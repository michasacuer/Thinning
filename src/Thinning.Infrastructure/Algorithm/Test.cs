﻿namespace Thinning.Contracts.Algorithm
{
    using System;
    using System.Drawing;
    using System.Threading;
    using Thinning.Infrastructure.Interfaces;
    using Thinning.Infrastructure.Models;

    public class Test : ITest
    {
        private readonly ITestWorker worker;

        public Test(ITestWorker worker)
        {
            this.worker = worker;
        }

        public TestResult Run(
            int iterations,
            int algorthmsCount,
            string imageFilepath,
            IProgress<int> progress,
            CancellationToken cancellationToken)
        {
            var bitmap = this.worker.PrepareBitmapToTestRun(new Bitmap(imageFilepath));
            var testSamples = this.worker.PrepareTestSamples(bitmap, iterations, algorthmsCount, out int stride);
            var timesTestResult = this.worker.RunAllAlgorithmsTestInterations(
                    bitmap, stride, testSamples, iterations, progress, cancellationToken);

            if (timesTestResult != null)
            {
                var testResult = this.worker.ByteArraysToBitmapResults(timesTestResult, iterations,  testSamples, bitmap);

                return testResult;
            }

            return null;
        }
    }
}
