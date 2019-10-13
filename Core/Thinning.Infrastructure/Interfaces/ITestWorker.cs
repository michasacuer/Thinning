namespace Thinning.Infrastructure.Interfaces
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Threading;
    using Thinning.Infrastructure.Models;

    public interface ITestWorker
    {
        Bitmap PrepareBitmapToTestRun(Bitmap bitmap);

        byte[][] PrepareTestSamples(Bitmap bitmap, int iterations, out BitmapData bitmapData);

        TestResult RunAllAlgorithmsTestInterations(
            Bitmap bitmap,
            BitmapData bitmapData,
            byte[][] testSamples,
            int iterations,
            IProgress<int> progress,
            CancellationToken cancellationToken);

        TestResult ByteArraysToBitmapResults(byte[][] testSamples, Bitmap bitmap);
    }
}
