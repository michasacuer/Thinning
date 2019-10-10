namespace Thinning.Infrastructure.Models
{
    using System.Collections.Generic;
    using System.Drawing;

    public class TestResult
    {
        public TestResult()
        {
        }

        public TestResult(TestResult timesTestsResults, TestResult bitmapsTestResult)
        {
            K3MBitmapResult = bitmapsTestResult.K3MBitmapResult;
            KMMBitmapResult = bitmapsTestResult.KMMBitmapResult;
            ZhangSuenBitmapResult = bitmapsTestResult.ZhangSuenBitmapResult;
            K3MResultTimes = timesTestsResults.K3MResultTimes;
            KMMResultTimes = timesTestsResults.KMMResultTimes;
            ZhangSuenResultTimes = timesTestsResults.ZhangSuenResultTimes;
        }

        public Bitmap K3MBitmapResult { get; set; }

        public Bitmap KMMBitmapResult { get; set; }

        public Bitmap ZhangSuenBitmapResult { get; set; }

        public List<double> KMMResultTimes { get; set; }

        public List<double> K3MResultTimes { get; set; }

        public List<double> ZhangSuenResultTimes { get; set; }
    }
}
