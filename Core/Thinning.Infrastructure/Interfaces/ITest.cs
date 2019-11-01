namespace Thinning.Infrastructure.Interfaces
{
    using System;
    using System.Threading;
    using Thinning.Infrastructure.Models;

    public interface ITest
    {
        TestResult Run(int iterations, string imageFilepath, IProgress<int> progress, CancellationToken cancellationToken);
    }
}
