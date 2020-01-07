namespace Thinning.UI.Helpers.Interfaces
{
    using LiveCharts;
    using Thinning.UI.ViewModels;

    public interface IPerformanceChartViewModelHelper : IViewModel<PerformanceChartViewModel>
    {
        void PrepareChart(ChartValues<double> times, double maxValue);
    }
}
