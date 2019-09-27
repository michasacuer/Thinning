namespace Thinning.UI.ViewModels
{
    using System;
    using Caliburn.Micro;
    using LiveCharts;
    using LiveCharts.Wpf;

    public class PerformanceChartViewModel : Screen
    {
        public PerformanceChartViewModel()
        {
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                }
            };

            //also adding values updates and animates the chart automatically
            SeriesCollection[0].Values.Add(48d);

            Labels = new[] { "Maria", "Susan", "Charles", "Frida" };
            Formatter = value => value.ToString("N");
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
    }
}
