namespace Thinning.UI.ViewModels
{
    using System.Threading;
    using Caliburn.Micro;

    public class ProgressViewModel : Screen
    {
        public ProgressViewModel(int iterations, int algorithmsCount)
        {
            this.MaximumValue = iterations * algorithmsCount;
        }

        public CancellationTokenSource CancellationToken { get; set; }

        public int ProgressValue { get; set; } = 0;

        public int MaximumValue { get; set; } = 60;

        public string TaskInfo { get; set; }

        public void CancelTest() => this.CancellationToken.Cancel();
    }
}
