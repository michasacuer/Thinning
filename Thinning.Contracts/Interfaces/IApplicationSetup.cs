namespace Thinning.Contracts.Interfaces
{
    using System.Collections.Generic;
    using Thinning.Infrastructure.Interfaces;

    public interface IApplicationSetup
    {
        List<string> GetRegisteredAlgorithmNames();

        List<IAlgorithm> GetRegisteredAlgorithmInstances();
    }
}
