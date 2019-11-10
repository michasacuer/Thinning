namespace Thinning.UI.Helpers
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Thinning.Contracts.Interfaces;
    using Thinning.Infrastructure.Interfaces;
    using Thinning.Infrastructure.Models;
    using Thinning.UI.Helpers.Interfaces;
    using Thinning.UI.ViewModels;

    public class MainWindowViewModelHelper : IMainWindowViewModelHelper
    {
        private readonly ICardContent cardContent;

        private readonly IApplicationSetup applicationSetup;

        private readonly IFileDialog fileDialog;

        private readonly IAlgorithmTest algorithmTest;

        private readonly IWindowManager windowManager;

        private readonly IImageConversion imageConversion;

        private readonly IPerformanceChartViewModelHelper performanceChartHelper;

        private MainWindowViewModel mainWindowViewModel;

        public MainWindowViewModelHelper(
            ICardContent cardContent,
            IApplicationSetup applicationSetup,
            IWindowManager windowManager,
            IAlgorithmTest algorithmTest,
            IFileDialog fileDialog,
            IImageConversion imageConversion,
            IPerformanceChartViewModelHelper performanceChartHelper)
        {
            this.cardContent = cardContent;
            this.applicationSetup = applicationSetup;
            this.windowManager = windowManager;
            this.fileDialog = fileDialog;
            this.algorithmTest = algorithmTest;
            this.imageConversion = imageConversion;
            this.performanceChartHelper = performanceChartHelper;
        }

        public void SetReferenceToViewModel(MainWindowViewModel mainWindowViewModel) =>
            this.mainWindowViewModel = mainWindowViewModel;

        public void LoadImage()
        {
            string filepath = this.fileDialog.GetImageFilepath();
            if (!filepath.Equals(string.Empty))
            {
                this.mainWindowViewModel.BaseImageUrl = filepath;
                this.mainWindowViewModel.NotifyOfPropertyChange(() => this.mainWindowViewModel.BaseImageUrl);

                this.mainWindowViewModel.ImageInfo = this.cardContent.GetImageInfo(this.mainWindowViewModel.BaseImageUrl);
                this.mainWindowViewModel.NotifyOfPropertyChange(() => this.mainWindowViewModel.ImageInfo);

                this.mainWindowViewModel.IsRunButtonsEnabled = true;
                this.mainWindowViewModel.NotifyOfPropertyChange(() => this.mainWindowViewModel.IsRunButtonsEnabled);
            }
        }

        public async void RunAlgorithms()
        {
            this.mainWindowViewModel.IsButtonsEnabled = false;
            this.mainWindowViewModel.NotifyOfPropertyChange(() => this.mainWindowViewModel.IsButtonsEnabled);

            var testResult = await this.ExecuteTests();
            if (testResult != null)
            {
                this.AttachResultsToAlgorithms(testResult);
            }

            this.mainWindowViewModel.IsButtonsEnabled = true;
            this.mainWindowViewModel.NotifyOfPropertyChange(() => this.mainWindowViewModel.IsButtonsEnabled);
        }

        public void SetTabsForPerformanceCharts()
        {
            var algorithmNames = this.applicationSetup.GetRegisteredAlgorithmNames();
            algorithmNames.ForEach(name => this.mainWindowViewModel.Items.Add(new PerformanceChartViewModel { DisplayName = name }));
        }

        public void SetHardwareInfo()
        {
            this.mainWindowViewModel.HardwareInfo = this.cardContent.GetHardwareInfo();
            this.mainWindowViewModel.NotifyOfPropertyChange(() => this.mainWindowViewModel.HardwareInfo);
        }

        private async Task<TestResult> ExecuteTests()
        {
            int iterations = this.mainWindowViewModel.SelectedIterationsCount;
            int algorithmsCount = this.mainWindowViewModel.Items.Count;

            var progressViewModel = new ProgressViewModel(iterations, algorithmsCount);
            await this.windowManager.ShowWindowAsync(progressViewModel, null, null);

            return await this.algorithmTest.ExecuteAsync(
                iterations, algorithmsCount, this.mainWindowViewModel.BaseImageUrl, progressViewModel);
        }

        private void AttachResultsToAlgorithms(TestResult testResult)
        {
            this.mainWindowViewModel.Images = new ObservableCollection<ImageLabelViewStructure>();
            double maxValue = this.GetMaxValueFromResultTimes(testResult.ResultTimes);

            int algorithmCount = 0;
            foreach (var timesList in testResult.ResultTimes)
            {
                this.mainWindowViewModel.Items[algorithmCount] = new PerformanceChartViewModel(
                    this.performanceChartHelper,
                    timesList,
                    maxValue,
                    this.mainWindowViewModel.Items[algorithmCount].DisplayName);

                this.mainWindowViewModel.Images.Add(new ImageLabelViewStructure
                {
                    Image = this.imageConversion.BitmapToBitmapImage(testResult.ResultBitmaps[algorithmCount]),
                    Label = this.mainWindowViewModel.Items[algorithmCount].DisplayName,
                });

                algorithmCount++;
            }

            this.mainWindowViewModel.NotifyOfPropertyChange(() => this.mainWindowViewModel.Images);
            this.mainWindowViewModel.NotifyOfPropertyChange(() => this.mainWindowViewModel.Items);
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
