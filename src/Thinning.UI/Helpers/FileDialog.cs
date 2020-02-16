namespace Thinning.UI.Helpers
{
    using Microsoft.Win32;
    using Thinning.UI.Helpers.Interfaces;

    public class FileDialog : IFileDialog
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

        public string GetAlgorithmImplementationFilepath()
        {
            OpenFileDialog openFile = new OpenFileDialog()
            {
                FilterIndex = 1,
            };

            return openFile.ShowDialog() == true ? openFile.FileName : string.Empty;
        }
    }
}
