namespace Thinning.UI.Helpers
{
    using System.Collections.Generic;
    using static Thinning.UI.Helpers.Storage;

    public class StorageDto
    {
        public static PcInfo PcInfo { get; set; }
        public static List<TestLine> TestLines { get; set; }
        public static List<Image> Images { get; set; }
    }
}
