namespace Thinning.UI.Helpers.Interfaces
{
    using Thinning.UI.ViewModels;

    public interface IMainWindowViewModelHelper : IViewModel<MainWindowViewModel>
    {
        void SetHardwareInfo();

        void SetTabsForPerformanceCharts();

        void LoadImage();

        void RunAlgorithms();

        void UploadAlgorithm();
    }
}
