namespace Thinning.UI.ViewModels
{
    using Caliburn.Micro;
    using Thinning.Contracts.Interfaces;
    using Thinning.UI.Helpers;
    using Thinning.UI.Helpers.Interfaces;

    public class MainWindowViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private IKMM kMM;

        private IZhangSuen zhangSuen;

        private ICardContent cardContent;

        public MainWindowViewModel(IKMM kMM, IZhangSuen zhangSuen, ICardContent cardContent)
        {
            this.kMM = kMM;
            this.zhangSuen = zhangSuen;
            this.cardContent = cardContent;

            this.Items.Add(new PerformanceChartViewModel { DisplayName = "K3M" });
            this.Items.Add(new PerformanceChartViewModel { DisplayName = "KMM" });
            this.Items.Add(new PerformanceChartViewModel { DisplayName = "Zhang Suen" });

            this.HardwareInfo = this.cardContent.GetHardwareInfo();
            this.NotifyOfPropertyChange(() => this.HardwareInfo);
        }

        public string BaseImageUrl { get; set; }

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

        public void RunAlghoritms() => throw new System.NotImplementedException();
    }
}
