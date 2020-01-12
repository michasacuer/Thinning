namespace Thinning.Infrastructure.Models
{
    using System.Collections.Generic;
    using System.Drawing;

    public class TestResult
    {
        public List<byte[]> RawBitmaps { get; set; }

        public List<Bitmap> ResultBitmaps { get; set; }

        public List<List<double>> ResultTimes { get; set; }
    }
}
