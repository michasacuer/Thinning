namespace Thinning.Infrastructure.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Thinning.Infrastructure.Models;
    
    public interface IWebService
    {
        void UpdatePcInfoStorage(string cpu, string gpu, string memory, string os);

        void UpdateStorage(List<string> algorithmNames);

        void UpdateStorage(TestResult testResul, string baseImageFilepath);

        Task<bool> PublishResults();
    }
}
