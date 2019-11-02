namespace Thinning.Infrastructure.Interfaces
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;
    using Accord.Imaging.Filters;

    public interface IImageConversion
    {
        Bitmap Binarize(Bitmap bitmap);

        BitmapImage BitmapToBitmapImage(Bitmap bitmap);

        Bitmap CreateNonIndexedImage(Bitmap bitmap);

        Bitmap Create8bppGreyscaleImage(Bitmap bitmap);
    }
}
