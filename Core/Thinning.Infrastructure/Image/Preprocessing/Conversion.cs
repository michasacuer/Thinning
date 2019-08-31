namespace Thining.Infrastructure.Image.Preprocessing
{
    using System.Drawing;
    using Accord.Imaging.Filters;

    public class Conversion
    {
        public Bitmap Create8bppGreyscaleImage(Bitmap bitmap) => Grayscale.CommonAlgorithms.BT709.Apply(bitmap);
    }
}
