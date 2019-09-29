namespace Thinning.Infrastructure.Interfaces
{
    using Thinning.Infrastructure.Models;

    public interface ITest
    {
        TestResult Run(string imageFilepath);
    }
}
