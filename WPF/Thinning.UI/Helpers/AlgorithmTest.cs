namespace Thinning.UI.Helpers
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using Thinning.Contracts;
    using Thinning.Infrastructure.Interfaces;
    using Thinning.Infrastructure.Models;
    using Thinning.UI.ViewModels;

    public class AlgorithmTest
    {
        public async Task<TestResult> ExecuteAsync(string imageFilepath, ProgressViewModel progressViewModel)
        {
            var container = ContainerConfig.Configure();
            var testResult = new TestResult();

            using (var scope = container.BeginLifetimeScope())
            {
                var test = scope.Resolve<ITest>();

                IProgress<int> progress = new Progress<int>((i) =>
                {
                    progressViewModel.ProgressValue = i;
                    progressViewModel.NotifyOfPropertyChange(() => progressViewModel.ProgressValue);
                });

                testResult = await Task.Run(() => test.Run(imageFilepath, progress));
            }

            await progressViewModel.TryCloseAsync();

            return testResult;
        }
    }
}
