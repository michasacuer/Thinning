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
        private readonly int algorithmIterations;

        public AlgorithmTest(int algorithmIterations)
        {
            this.algorithmIterations = algorithmIterations;
        }

        public async Task<TestResult> ExecuteAsync(string imageFilepath, ProgressViewModel progressViewModel)
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var test = scope.Resolve<ITest>();
                progressViewModel.CancellationToken = new CancellationTokenSource();

                int i = 0;
                int whichAlgorithm = 1;

                IProgress<int> progress = new Progress<int>((progressValue) =>
                {
                    if (i < this.algorithmIterations)
                    {
                        progressViewModel.TaskInfo = $"Algorithm number {whichAlgorithm} executing...";
                        i++;
                    }
                    else
                    {
                        i = 0;
                        whichAlgorithm++;
                    }

                    progressViewModel.ProgressValue = progressValue;
                    progressViewModel.NotifyOfPropertyChange(() => progressViewModel.ProgressValue);
                    progressViewModel.NotifyOfPropertyChange(() => progressViewModel.TaskInfo);
                });

                var testResult = await Task.Run(
                    () => test.Run(
                        this.algorithmIterations,
                        imageFilepath,
                        progress,
                        progressViewModel.CancellationToken.Token), progressViewModel.CancellationToken.Token);

                await progressViewModel.TryCloseAsync();

                return testResult;
            }
        }
    }
}
