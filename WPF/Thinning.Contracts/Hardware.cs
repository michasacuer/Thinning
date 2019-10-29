namespace Thinning.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Thinning.Infrastructure;

    public class Hardware
    {
        public string GetHardwareInfo()
        {
            var systemInfo = new SystemInfo();
            var hardwareInfoList = new List<string>();

            hardwareInfoList.Add("OS: " + systemInfo.GetOperativeSystemInfo());
            hardwareInfoList.Add("CPU: " + systemInfo.GetCpuInfo());
            hardwareInfoList.Add("GPU: " + systemInfo.GetGpuInfo());
            hardwareInfoList.Add("MEM: " + systemInfo.GetTotalMemory());

            var sb = new StringBuilder();

            foreach (string info in hardwareInfoList)
            {
                sb.Append(info + Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
