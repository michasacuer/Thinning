namespace Thinning.Infrastructure.Interfaces
{
    public interface ISystemInfo
    {
        string GetOperativeSystemInfo();

        string GetCpuInfo();

        string GetGpuInfo();

        string GetTotalMemory();
    }
}
