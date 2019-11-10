namespace Thinning.UI.Helpers.Interfaces
{
    using System.Threading.Tasks;
    using Thinning.Infrastructure.Models;
    using Thinning.UI.ViewModels;

    public interface IAlgorithmTest
    {
        Task<TestResult> ExecuteAsync(
            int algorithmIterations,
            int algorithmsCount,
            string imageFilepath,
            ProgressViewModel progressViewModel);
    }
}
