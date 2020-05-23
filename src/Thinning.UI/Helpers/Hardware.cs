namespace Thinning.UI.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Thinning.Infrastructure.Interfaces;
    using Thinning.UI.Helpers.Interfaces;

    public class Hardware : IHardware
    {
        private readonly ISystemInfo systemInfo;

        public Hardware(ISystemInfo systemInfo)
        {
            this.systemInfo = systemInfo;
        }

        public string GetHardwareInfo()
        {
            var hardwareInfoList = new List<string>
            {
                "OS: " + this.systemInfo.GetOperativeSystemInfo(),
                "CPU: " + this.systemInfo.GetCpuInfo(),
                "GPU: " + this.systemInfo.GetGpuInfo(),
                "MEMORY: " + this.systemInfo.GetTotalMemory()
            };

            var sb = new StringBuilder();
            foreach (string info in hardwareInfoList)
            {
                sb.Append(info + Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
