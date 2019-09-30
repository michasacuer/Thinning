namespace Thinning.UI.ViewModels
{
    using Caliburn.Micro;

    public class ProgressViewModel : Screen
    {
        public int ProgressValue { get; set; } = 0;

        public string TaskInfo { get; set; }
    }
}
