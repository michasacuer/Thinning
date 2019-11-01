namespace Thinning.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Media;
    using Caliburn.Micro;
    using Thinning.Infrastructure;
    using Thinning.Infrastructure.Interfaces;
    using Thinning.UI.Helpers;
    using Thinning.UI.Helpers.Interfaces;

    public class MainWindowViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private ICardContent cardContent;

        private IApplicationSetup applicationSetup;

        private IWindowManager windowManager;

        public MainWindowViewModel(
            ICardContent cardContent,
            IApplicationSetup applicationSetup,
            IWindowManager windowManager)
        {
            this.cardContent = cardContent;
            this.applicationSetup = applicationSetup;
            this.windowManager = windowManager;

            var algorithmNames = this.applicationSetup.GetRegisteredAlgorithmNames();
            this.Images = new ObservableCollection<Tuple<string, ImageSource>>();

            foreach (var algorithm in algorithmNames)
            {
                this.Items.Add(new PerformanceChartViewModel { DisplayName = algorithm });
            }

            this.HardwareInfo = this.cardContent.GetHardwareInfo();
            this.NotifyOfPropertyChange(() => this.HardwareInfo);
        }

        public string BaseImageUrl { get; set; }

        public ObservableCollection<Tuple<string, ImageSource>> Images { get; set; }

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

            var progressViewModel = new ProgressViewModel();
            await this.windowManager.ShowWindowAsync(progressViewModel, null, null);

            var algorithmTest = new AlgorithmTest();
            var testResult = await algorithmTest.ExecuteAsync(this.BaseImageUrl, progressViewModel);

            if (testResult != null)
            {
                var conversion = new ImageConversion();

                //this.K3MAlgorithmResult = conversion.BitmapToBitmapImage(testResult.K3MBitmapResult);
                //this.NotifyOfPropertyChange(() => this.K3MAlgorithmResult);
                //
                //this.KMMAlgorithmResult = conversion.BitmapToBitmapImage(testResult.KMMBitmapResult);
                //this.NotifyOfPropertyChange(() => this.KMMAlgorithmResult);
                //
                //this.ZhangSuenAlgorithmResult = conversion.BitmapToBitmapImage(testResult.ZhangSuenBitmapResult);
                //this.NotifyOfPropertyChange(() => this.ZhangSuenAlgorithmResult);

                this.Images.Add(Tuple.Create("ddd", (ImageSource)conversion.BitmapToBitmapImage(testResult.K3MBitmapResult)));
                this.Images.Add(Tuple.Create("ddd2", (ImageSource)conversion.BitmapToBitmapImage(testResult.KMMBitmapResult)));
                this.Images.Add(Tuple.Create("ddd3", (ImageSource)conversion.BitmapToBitmapImage(testResult.ZhangSuenBitmapResult)));
                this.NotifyOfPropertyChange(() => this.Images);

                this.Items[0] = new PerformanceChartViewModel(testResult.K3MResultTimes, "K3M");
                this.Items[1] = new PerformanceChartViewModel(testResult.KMMResultTimes, "KMM");
                this.Items[2] = new PerformanceChartViewModel(testResult.ZhangSuenResultTimes, "Zhang Suen");
                this.NotifyOfPropertyChange(() => this.Items);
            }

            this.IsButtonsEnabled = true;
            this.NotifyOfPropertyChange(() => this.IsButtonsEnabled);
        }
    }
}
