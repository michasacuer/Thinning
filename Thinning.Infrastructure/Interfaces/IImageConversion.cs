namespace Thinning.Infrastructure.Interfaces
{
    using System.Drawing;
    using System.Windows.Media.Imaging;

    public interface IImageConversion
    {
        Bitmap Binarize(Bitmap bitmap);

        BitmapImage BitmapToBitmapImage(Bitmap bitmap);

        Bitmap CreateNonIndexedImage(Bitmap bitmap);

        Bitmap Create8bppGreyscaleImage(Bitmap bitmap);
    }
}
