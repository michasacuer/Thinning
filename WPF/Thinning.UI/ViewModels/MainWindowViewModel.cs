namespace Thinning.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using Caliburn.Micro;
    using Thinning.Infrastructure.Interfaces;
    using Thinning.Infrastructure.Models;
    using Thinning.UI.Helpers;
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

        public ObservableCollection<ImageLabelViewStructure> Images { get; set; }

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
            this.Images = new ObservableCollection<ImageLabelViewStructure>();
            double maxValue = this.GetMaxValueFromResultTimes(testResult.ResultTimes);

            int algorithmCount = 0;
            foreach (var timesList in testResult.ResultTimes)
            {
                this.Items[algorithmCount] = new PerformanceChartViewModel(timesList, maxValue, this.Items[algorithmCount].DisplayName);
                this.Images.Add(new ImageLabelViewStructure
                {
                    Image = this.imageConversion.BitmapToBitmapImage(testResult.ResultBitmaps[algorithmCount]),
                    Label = this.Items[algorithmCount].DisplayName,
                });

                algorithmCount++;
            }

            this.NotifyOfPropertyChange(() => this.Images);
            this.NotifyOfPropertyChange(() => this.Items);
        }

        private double GetMaxValueFromResultTimes(List<List<double>> times)
        {
            var topValues = new List<double>();
            foreach (var timesList in times)
            {
                var sortedList = new List<double>(timesList).OrderByDescending(t => t).ToList();
                topValues.Add(sortedList.First());
            }

            topValues = topValues.OrderByDescending(t => t).ToList();

            return topValues.First();
        }
    }
}
