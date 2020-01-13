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

        private readonly IWebService webService;

        public Hardware(ISystemInfo systemInfo, IWebService webService)
        {
            this.systemInfo = systemInfo;
            this.webService = webService;
        }

        public string GetHardwareInfo()
        {
            string os = this.systemInfo.GetOperativeSystemInfo();
            string cpu = this.systemInfo.GetCpuInfo();
            string gpu = this.systemInfo.GetGpuInfo();
            string memory = this.systemInfo.GetTotalMemory();
            this.webService.UpdatePcInfoStorage(cpu, gpu, memory, os);

            var hardwareInfoList = new List<string>();
            hardwareInfoList.Add("OS: " + os);
            hardwareInfoList.Add("CPU: " + cpu);
            hardwareInfoList.Add("GPU: " + gpu);
            hardwareInfoList.Add("MEMORY: " + memory);

            var sb = new StringBuilder();
            foreach (string info in hardwareInfoList)
            {
                sb.Append(info + Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
