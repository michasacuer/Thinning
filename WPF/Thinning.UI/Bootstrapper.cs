namespace Thinning.UI
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using Caliburn.Micro;
    using Thinning.Algorithm;
    using Thinning.Contracts.Interfaces;
    using Thinning.UI.ViewModels;

    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer simpleContainer = new SimpleContainer();

        public Bootstrapper()
        {
            this.Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            this.DisplayRootViewFor<MainWindowViewModel>();
        }

        protected override void Configure()
        {
            this.simpleContainer.Singleton<IWindowManager, WindowManager>();
            this.simpleContainer.Singleton<IEventAggregator, EventAggregator>();

            this.simpleContainer.PerRequest<MainWindowViewModel, MainWindowViewModel>();

            this.simpleContainer.PerRequest<IKMM, KMM>();
            this.simpleContainer.PerRequest<IZhangSuen, ZhangSuen>();

            base.Configure();
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = this.simpleContainer.GetInstance(service, key);
            if (instance != null)
            {
                return instance;
            }

            throw new InvalidOperationException("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return this.simpleContainer.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            this.simpleContainer.BuildUp(instance);
        }
    }
}
