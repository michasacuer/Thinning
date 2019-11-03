namespace Thinning.UI.Helpers.Interfaces
{
    using Thinning.UI.ViewModels;

    public interface IMainWindowViewModelHelper
    {
        void SetReferenceToMainWindow(MainWindowViewModel mainWindowViewModel);

        void SetHardwareInfo();

        void SetTabsForPerformanceCharts();

        void LoadImage();

        void RunAlgorithms();
    }
}
