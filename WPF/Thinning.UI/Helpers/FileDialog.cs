namespace Thinning.UI.Helpers
{
    using Microsoft.Win32;

    public class FileDialog
    {
        public string GetImageFilepath()
        {
            OpenFileDialog openPicture = new OpenFileDialog()
            {
                Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif",
                FilterIndex = 1,
            };

            return openPicture.ShowDialog() == true ? openPicture.FileName : string.Empty;
        }
    }
}
