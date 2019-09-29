namespace Thinning.Infrastructure.Models
{
    using System.Collections.Generic;
    using System.Drawing;

    public class TestResult
    {
        public Bitmap KMMBitmapResult { get; set; }

        public Bitmap ZhangSuenBitmapResult { get; set; }

        public List<double> KMMResultTimes { get; set; }

        public List<double> ZhangSuenResultTimes { get; set; }
    }
}
