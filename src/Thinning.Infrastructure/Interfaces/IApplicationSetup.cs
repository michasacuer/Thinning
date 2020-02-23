namespace Thinning.Infrastructure.Interfaces
{
    using System.Collections.Generic;
    using Thinning.Algorithm.Interfaces;

    public interface IApplicationSetup
    {
        List<string> GetRegisteredAlgorithmNames();

        List<IAlgorithm> GetRegisteredAlgorithmInstances();

        bool TryUploadCSharClassAlgorithm(string filepath);
    }
}
