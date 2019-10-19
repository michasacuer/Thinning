namespace Thinning.Contracts
{
    using Autofac;
    using Thinning.Algorithm;
    using Thinning.Contracts.Algorithm;
    using Thinning.Infrastructure.Interfaces;
    using Thinning.Infrastructure.Interfaces.Algorithms;

    public class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Test>().As<ITest>();
            builder.RegisterType<TestWorker>().As<ITestWorker>();

            builder.RegisterType<KMM>().As<IKMM>();
            builder.RegisterType<ZhangSuen>().As<IZhangSuen>();
            builder.RegisterType<K3M>().As<IK3M>();

            return builder.Build();
        }
    }
}
