namespace Thinning.Contracts.Interfaces
{
    using System;
    using System.Drawing;
    using System.Threading;
    using Thinning.Infrastructure.Models;

    public interface ITestWorker
    {
        Bitmap PrepareBitmapToTestRun(Bitmap bitmap);

        byte[][] PrepareTestSamples(Bitmap bitmap, int iterations, int algorithmsCount, out int stride);

        TestResult RunAllAlgorithmsTestInterations(
            Bitmap bitmap,
            int stride,
            byte[][] testSamples,
            int iterations,
            IProgress<int> progress,
            CancellationToken cancellationToken);

        TestResult ByteArraysToBitmapResults(TestResult resultTimes, int iterations, byte[][] testSamples, Bitmap bitmap);
    }
}
