namespace Thinning.UI.ViewModels
{
    using System.Windows.Media;
    using Caliburn.Micro;
    using Thinning.Infrastructure.Image.Preprocessing;
    using Thinning.UI.Helpers;
    using Thinning.UI.Helpers.Interfaces;

    public class MainWindowViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private ICardContent cardContent;

        public MainWindowViewModel(ICardContent cardContent)
        {
            this.cardContent = cardContent;

            this.Items.Add(new PerformanceChartViewModel { DisplayName = "KMM" });
            this.Items.Add(new PerformanceChartViewModel { DisplayName = "Zhang Suen" });

            this.HardwareInfo = this.cardContent.GetHardwareInfo();
            this.NotifyOfPropertyChange(() => this.HardwareInfo);
        }

        public string BaseImageUrl { get; set; }

        public ImageSource KMMAlgorithmResult { get; set; }

        public ImageSource ZhangSuenAlgorithmResult { get; set; }

        public bool IsButtonsEnabled { get; set; } = true;

        public string HardwareInfo { get; set; }

        public string ImageInfo { get; set; }

        public void LoadImage()
        {
            var fileDialog = new FileDialog();

            this.BaseImageUrl = fileDialog.GetImageFilepath();
            this.NotifyOfPropertyChange(() => this.BaseImageUrl);

            this.ImageInfo = this.cardContent.GetImageInfo(this.BaseImageUrl);
            this.NotifyOfPropertyChange(() => this.ImageInfo);
        }

        public async void RunAlgorithms()
        {
            this.IsButtonsEnabled = false;
            this.NotifyOfPropertyChange(() => this.IsButtonsEnabled);

            var algorithmTest = new AlgorithmTest();
            var testResult = await algorithmTest.ExecuteAsync(this.BaseImageUrl);

            var conversion = new Conversion();

            this.KMMAlgorithmResult = conversion.BitmapToBitmapImage(testResult.KMMBitmapResult);
            this.NotifyOfPropertyChange(() => this.KMMAlgorithmResult);

            this.ZhangSuenAlgorithmResult = conversion.BitmapToBitmapImage(testResult.ZhangSuenBitmapResult);
            this.NotifyOfPropertyChange(() => this.ZhangSuenAlgorithmResult);

            this.Items[0] = new PerformanceChartViewModel(testResult.KMMResultTimes, "KMM");
            this.Items[1] = new PerformanceChartViewModel(testResult.ZhangSuenResultTimes, "Zhang Suen");
            this.NotifyOfPropertyChange(() => this.Items);

            this.IsButtonsEnabled = true;
            this.NotifyOfPropertyChange(() => this.IsButtonsEnabled);
        }
    }
}
