namespace Thinning.UI.Helpers
{
    using Autofac;
    using Thinning.Contracts;
    using Thinning.Infrastructure.Interfaces;
    using Thinning.Infrastructure.Models;

    public class AlgorithmTest
    {
        public TestResult Execute(string imageFilepath)
        {
            var container = ContainerConfig.Configure();
            var testResult = new TestResult();

            using (var scope = container.BeginLifetimeScope())
            {
                var test = scope.Resolve<ITest>();
                testResult =     test.Run(imageFilepath);
            }

            return testResult;
        }
    }
}
