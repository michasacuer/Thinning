namespace Thinning.UI.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Caliburn.Micro;
    using Thinning.UI.Helpers;
    using Thinning.UI.Helpers.Interfaces;

    public class MainWindowViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private readonly IMainWindowViewModelHelper helper;

        public MainWindowViewModel(IMainWindowViewModelHelper helper)
        {
            this.helper = helper;

            this.helper.SetReferenceToViewModel(this);
            this.helper.SetTabsForPerformanceCharts();
            this.helper.SetHardwareInfo();
        }

        public string BaseImageUrl { get; set; }

        public ObservableCollection<ImageLabelViewStructure> Images { get; set; }

        public bool IsButtonsEnabled { get; set; } = true;

        public bool IsRunButtonsEnabled { get; set; } = false;

        public string HardwareInfo { get; set; }

        public string ImageInfo { get; set; }

        public int SelectedIterationsCount { get; set; }

        public List<int> IterationsList { get; set; } = new List<int>(new int[] { 10, 20, 30, 40, 50, 70, 80 });

        public void LoadImage() => this.helper.LoadImage();

        public async void RunAlgorithms() => this.helper.RunAlgorithms();
    }
}
