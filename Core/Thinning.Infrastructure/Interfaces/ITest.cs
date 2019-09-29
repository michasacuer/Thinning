namespace Thinning.Infrastructure.Interfaces
{
    using System;
    using Thinning.Infrastructure.Models;

    public interface ITest
    {
        TestResult Run(string imageFilepath, IProgress<int> progress);
    }
}
