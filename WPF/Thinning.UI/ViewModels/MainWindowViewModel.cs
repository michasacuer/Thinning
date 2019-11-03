namespace Thinning.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using Caliburn.Micro;
    using Thinning.Infrastructure.Interfaces;
    using Thinning.Infrastructure.Models;
    using Thinning.UI.Helpers.Interfaces;

    public class MainWindowViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private readonly ICardContent cardContent;

        private readonly IApplicationSetup applicationSetup;

        private readonly IFileDialog fileDialog;

        private readonly IAlgorithmTest algorithmTest;

        private readonly IWindowManager windowManager;

        private readonly IImageConversion imageConversion;

        public MainWindowViewModel(
            ICardContent cardContent,
            IApplicationSetup applicationSetup,
            IWindowManager windowManager,
            IAlgorithmTest algorithmTest,
            IFileDialog fileDialog,
            IImageConversion imageConversion)
        {
            this.cardContent = cardContent;
            this.applicationSetup = applicationSetup;
            this.windowManager = windowManager;
            this.fileDialog = fileDialog;
            this.algorithmTest = algorithmTest;
            this.imageConversion = imageConversion;

            this.SetTabsForPerformanceCharts();

            this.HardwareInfo = this.cardContent.GetHardwareInfo();
            this.NotifyOfPropertyChange(() => this.HardwareInfo);
        }

        public string BaseImageUrl { get; set; }

        public ObservableCollection<Tuple<string, ImageSource>> Images { get; set; }

        public bool IsButtonsEnabled { get; set; } = true;

        public bool IsRunButtonsEnabled { get; set; } = false;

        public string HardwareInfo { get; set; }

        public string ImageInfo { get; set; }

        public int SelectedIterationsCount { get; set; }

        public List<int> IterationsList { get; set; } = new List<int>(new int[] { 10, 20, 30, 40, 50 });

        public void LoadImage()
        {
            string filepath = this.fileDialog.GetImageFilepath();
            if (!filepath.Equals(string.Empty))
            {
                this.BaseImageUrl = filepath;
                this.NotifyOfPropertyChange(() => this.BaseImageUrl);

                this.ImageInfo = this.cardContent.GetImageInfo(this.BaseImageUrl);
                this.NotifyOfPropertyChange(() => this.ImageInfo);

                this.IsRunButtonsEnabled = true;
                this.NotifyOfPropertyChange(() => this.IsRunButtonsEnabled);
            }
        }

        public async void RunAlgorithms()
        {
            this.IsButtonsEnabled = false;
            this.NotifyOfPropertyChange(() => this.IsButtonsEnabled);

            var testResult = await this.ExecuteTests();
            if (testResult != null)
            {
                this.AttachResultsToAlgorithms(testResult);
            }

            this.IsButtonsEnabled = true;
            this.NotifyOfPropertyChange(() => this.IsButtonsEnabled);
        }

        private void SetTabsForPerformanceCharts()
        {
            var algorithmNames = this.applicationSetup.GetRegisteredAlgorithmNames();
            algorithmNames.ForEach(name => this.Items.Add(new PerformanceChartViewModel { DisplayName = name }));
        }

        private async Task<TestResult> ExecuteTests()
        {
            int iterations = this.SelectedIterationsCount;
            int algorithmsCount = this.Items.Count;

            var progressViewModel = new ProgressViewModel(iterations,  algorithmsCount);
            await this.windowManager.ShowWindowAsync(progressViewModel, null, null);

            return await this.algorithmTest.ExecuteAsync(iterations, algorithmsCount, this.BaseImageUrl, progressViewModel);
        }

        private void AttachResultsToAlgorithms(TestResult testResult)
        {
            int algorithmCount = 0;

            this.Images = new ObservableCollection<Tuple<string, ImageSource>>();

            foreach (var timesList in testResult.ResultTimes)
            {
                this.Items[algorithmCount] = new PerformanceChartViewModel(timesList, this.Items[algorithmCount].DisplayName);
                this.Images.Add(Tuple.Create(
                    this.Items[algorithmCount].DisplayName,
                    (ImageSource)this.imageConversion.BitmapToBitmapImage(testResult.ResultBitmaps[algorithmCount])));

                algorithmCount++;
            }

            this.NotifyOfPropertyChange(() => this.Images);
            this.NotifyOfPropertyChange(() => this.Items);
        }
    }
}
