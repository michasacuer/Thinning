﻿namespace Thinning.Infrastructure
{
    using Autofac;
    using Thinning.Contracts.Algorithm;
    using Thinning.Infrastructure.Interfaces;

    public class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Test>().As<ITest>();
            builder.RegisterType<TestWorker>().As<ITestWorker>();
            builder.RegisterType<ApplicationSetup>().As<IApplicationSetup>();
            builder.RegisterType<ImageConversion>().As<IImageConversion>();

            return builder.Build();
        }
    }
}
