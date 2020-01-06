namespace Thinning.Contracts.Interfaces
{
    public interface ISystemInfo
    {
        string GetOperativeSystemInfo();

        string GetCpuInfo();

        string GetGpuInfo();

        string GetTotalMemory();
    }
}
