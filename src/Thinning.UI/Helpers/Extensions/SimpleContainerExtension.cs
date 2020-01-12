namespace Thinning.UI.Helpers.Extensions
{
    using Caliburn.Micro;
    using Thinning.Infrastructure;
    using Thinning.Infrastructure.Interfaces;
    using Thinning.UI.Helpers.Interfaces;
    using Thinning.UI.ViewModels;

    public static class SimpleContainerExtension
    {
        public static void InjectInterfaces(this SimpleContainer simpleContainer)
        {
            simpleContainer.Singleton<IWindowManager, WindowManager>();
            simpleContainer.Singleton<IEventAggregator, EventAggregator>();

            simpleContainer.Singleton<MainWindowViewModel, MainWindowViewModel>();
            simpleContainer.Singleton<PerformanceChartViewModel, PerformanceChartViewModel>();

            simpleContainer.PerRequest<ICardContent, CardContent>();
            simpleContainer.PerRequest<IApplicationSetup, ApplicationSetup>();
            simpleContainer.PerRequest<IAlgorithmTest, AlgorithmTest>();
            simpleContainer.PerRequest<IFileDialog, FileDialog>();
            simpleContainer.PerRequest<IImageConversion, ImageConversion>();
            simpleContainer.PerRequest<IHardware, Hardware>();
            simpleContainer.PerRequest<ISystemInfo, SystemInfo>();
            simpleContainer.PerRequest<IWebService, WebService>();
            simpleContainer.PerRequest<IMainWindowViewModelHelper, MainWindowViewModelHelper>();
            simpleContainer.PerRequest<IPerformanceChartViewModelHelper, PerformanceChartViewModelHelper>();
        }
    }
}
