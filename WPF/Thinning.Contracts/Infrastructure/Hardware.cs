namespace Thinning.Contracts.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using Thinning.Infrastructure.Computer;

    public class Hardware
    {
        public string GetHardwareInfo()
        {
            var systemInfo = new SystemInfo();
            string hardwareInfo = string.Empty;
            var hardwareInfoList = new List<string>();

            hardwareInfoList.Add("OS: " + systemInfo.GetOperativeSystemInfo());
            hardwareInfoList.Add("CPU: " + systemInfo.GetCpuInfo());
            hardwareInfoList.Add("GPU: " + systemInfo.GetGpuInfo());
            hardwareInfoList.Add("MEM: " + systemInfo.GetTotalMemory());

            foreach (string info in hardwareInfoList)
            {
                hardwareInfo += info;
                hardwareInfo += Environment.NewLine;
            }

            return hardwareInfo;
        }
    }
}
