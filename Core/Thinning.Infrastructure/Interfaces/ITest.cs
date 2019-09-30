namespace Thinning.Infrastructure.Interfaces
{
    using System;
    using System.Threading;
    using Thinning.Infrastructure.Models;

    public interface ITest
    {
        TestResult Run(string imageFilepath, IProgress<int> progress, CancellationToken cancellationToken);
    }
}
