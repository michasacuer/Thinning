namespace Thinning.UI.Helpers
{
    using System.Threading.Tasks;
    using Autofac;
    using Thinning.Contracts;
    using Thinning.Infrastructure.Interfaces;
    using Thinning.Infrastructure.Models;

    public class AlgorithmTest
    {
        public async Task<TestResult> ExecuteAsync(string imageFilepath)
        {
            var container = ContainerConfig.Configure();
            var testResult = new TestResult();

            using (var scope = container.BeginLifetimeScope())
            {
                var test = scope.Resolve<ITest>();
                testResult = await Task.Run(() => test.Run(imageFilepath));
            }

            return testResult;
        }
    }
}
