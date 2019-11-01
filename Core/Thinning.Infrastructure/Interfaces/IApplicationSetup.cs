namespace Thinning.Infrastructure.Interfaces
{
    using System.Collections.Generic;

    public interface IApplicationSetup
    {
        List<string> GetRegisteredAlgorithmNames();
    }
}
