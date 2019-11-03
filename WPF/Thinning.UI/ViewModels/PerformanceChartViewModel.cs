namespace Thinning.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Caliburn.Micro;
    using LiveCharts;
    using LiveCharts.Wpf;
    using Thinning.UI.Helpers.Interfaces;

    public class PerformanceChartViewModel : Screen
    {
        private readonly IPerformanceChartViewModelHelper helper;

        public PerformanceChartViewModel()
        {
        }

        public PerformanceChartViewModel(
            IPerformanceChartViewModelHelper helper,
            List<double> values,
            double maxValue,
            string displayName)
        {
            this.helper = helper;
            this.helper.SetReferenceToViewModel(this);

            var times = new ChartValues<double>(values);
            this.DisplayName = displayName;

            this.SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Time (Ms)",
                    Values = times,
                },
            };

            this.helper.PrepareChart(times, maxValue);
        }

        public SeriesCollection SeriesCollection { get; set; }

        public string[] Labels { get; set; }

        public Func<double, string> Formatter { get; set; }

        public double MaxValue { get; set; } = 0;
    }
}
