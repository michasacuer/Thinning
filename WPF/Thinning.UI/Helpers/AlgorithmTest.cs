namespace Thinning.UI.Helpers
{
    using System;
    using System.Threading;
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

            using (var scope = container.BeginLifetimeScope())
            {
                var test = scope.Resolve<ITest>();
                progressViewModel.CancellationToken = new CancellationTokenSource();

                IProgress<int> progress = new Progress<int>((i) =>
                {
                    if (i <= 20)
                    {
                        progressViewModel.TaskInfo = "K3M Executing";
                    }
                    else if (i > 20 && i < 40)
                    {
                        progressViewModel.TaskInfo = "KMM Executing";
                    }
                    else
                    {
                        progressViewModel.TaskInfo = "ZhagnSuen Executing";
                    }

                    progressViewModel.ProgressValue = i;
                    progressViewModel.NotifyOfPropertyChange(() => progressViewModel.ProgressValue);
                    progressViewModel.NotifyOfPropertyChange(() => progressViewModel.TaskInfo);
                });

                var testResult = await Task.Run(
                    () => test.Run(
                    imageFilepath,
                    progress,
                    progressViewModel.CancellationToken.Token),
                    progressViewModel.CancellationToken.Token);

                await progressViewModel.TryCloseAsync();

                return testResult;
            }
        }
    }
}
