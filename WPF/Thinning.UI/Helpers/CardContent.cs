namespace Thinning.UI.Helpers
{
    using System;
    using System.Drawing;
    using System.IO;
    using Thinning.Infrastructure.Interfaces;
    using Thinning.UI.Helpers.Interfaces;

    public class CardContent : ICardContent
    {
        private readonly IHardware hardware;

        public CardContent(IHardware hardware)
        {
            this.hardware = hardware;
        }

        public string GetImageInfo(string imageFilepath)
        {
            try
            {
                var image = Image.FromFile(imageFilepath);
                string filename = Path.GetFileName(imageFilepath);
                string imageInfo = "Image name: " + filename + Environment.NewLine +
                    "Width (px): " + image.Width + " Height (px): " + image.Height;

                return imageInfo;
            }
            catch (ArgumentException)
            {
                return string.Empty;
            }
        }

        public string GetHardwareInfo() => this.hardware.GetHardwareInfo();
    }
}
