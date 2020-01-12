namespace Thinning.Infrastructure.Models
{
    using System.Collections.Generic;

    public class Storage
    {
        public class PcInfo
        {
            public string Cpu { get; set; }
            public string Gpu { get; set; }
            public string Os { get; set; }
            public string Memory { get; set; }
        }

        public class TestLine
        {
            public string AlgorithmName { get; set; }
            public int Iterations { get; set; }
            public List<TestRun> AlgorithmTestRuns { get; set; }
        }

        public class Image
        {
            public string AlgorithmName { get; set; }
            public int OriginalWidth { get; set; }
            public int OriginalHeight { get; set; }
            public int OriginalBpp { get; set; }
            public bool TestImage { get; set; }
            public byte[] ImageContent { get; set; }
        }

        public class TestRun
        {
            public double Time { get; set; }
            public int RunCount { get; set; }
        }
    }
}
