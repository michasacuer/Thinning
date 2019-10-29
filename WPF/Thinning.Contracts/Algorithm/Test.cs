namespace Thinning.Contracts.Algorithm
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

        public TestResult Run(string imageFilepath, IProgress<int> progress, CancellationToken cancellationToken)
        {
            int iterations = 20;
            int stride;

            var bitmap = this.worker.PrepareBitmapToTestRun(new Bitmap(imageFilepath));
            var testSamples = this.worker.PrepareTestSamples(bitmap, iterations, out stride);
            var timesTestResult = this.worker.RunAllAlgorithmsTestInterations(bitmap, stride, testSamples, iterations, progress, cancellationToken);

            if (timesTestResult != null)
            {
                var bitmapsTestResult = this.worker.ByteArraysToBitmapResults(testSamples, bitmap);

                return new TestResult(timesTestResult, bitmapsTestResult);
            }
            else
            {
                return null;
            }
        }
    }
}
