namespace Thinning.UI.ViewModels
{
    using Caliburn.Micro;
    using Thinning.Contracts.Interfaces;

    public class MainWindowViewModel : Screen
    {
        private IKMM kMM;

        private IZhangSuen zhangSuen;

        public MainWindowViewModel(IKMM kMM, IZhangSuen zhangSuen)
        {
            this.kMM = kMM;
            this.zhangSuen = zhangSuen;
        }
    }
}
