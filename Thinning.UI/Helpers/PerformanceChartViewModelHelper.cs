namespace Thinning.UI.Helpers
{
    using LiveCharts;
    using Thinning.UI.Helpers.Interfaces;
    using Thinning.UI.ViewModels;

    public class PerformanceChartViewModelHelper : IPerformanceChartViewModelHelper
    {
        private PerformanceChartViewModel performanceChartViewModel;

        public void SetReferenceToViewModel(PerformanceChartViewModel performanceChartViewModel) =>
            this.performanceChartViewModel = performanceChartViewModel;

        public void PrepareChart(ChartValues<double> times, double maxValue)
        {
            var labels = new string[times.Count];

            for (int i = 0; i < times.Count; i++)
            {
                labels[i] = $"Run {i + 1}";
            }

            this.performanceChartViewModel.Labels = labels;
            this.performanceChartViewModel.Formatter = value => value.ToString("N");
            this.performanceChartViewModel.MaxValue = maxValue;
        }
    }
}
