namespace Thinning.UI.ViewModels
{
    using System.Threading;
    using Caliburn.Micro;

    public class ProgressViewModel : Screen
    {
        public CancellationTokenSource CancellationToken { get; set; }

        public int ProgressValue { get; set; } = 0;

        public string TaskInfo { get; set; }

        public void CancelTest() => this.CancellationToken.Cancel();
    }
}
