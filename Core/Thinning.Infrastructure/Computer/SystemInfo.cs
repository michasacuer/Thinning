namespace Thinning.Infrastructure.Computer
{
    using System;
    using System.Collections.Generic;
    using System.Management;
    using Microsoft.VisualBasic.Devices;

    public class SystemInfo
    {
        public string GetOperativeSystemInfo()
        {
            string caption = string.Empty;
            string version = string.Empty;
            var searcher = new ManagementObjectSearcher("Select * From Win32_OperatingSystem");
            var searcherList = searcher.Get();

            foreach (ManagementObject item in searcherList)
            {
                caption = item["Caption"].ToString();
                version = item["Version"].ToString();
            }

            return caption + " Ver. " + version;
        }

        public string GetCpuInfo()
        {
            string cpuName = string.Empty;
            var searcher = new ManagementObjectSearcher("Select * From Win32_processor");
            var searcherList = searcher.Get();

            foreach (ManagementObject item in searcherList)
            {
                cpuName = item["Name"].ToString();
            }

            return cpuName;
        }

        public string GetGpuInfo()
        {
            string gpuName = string.Empty;
            var searcher = new ManagementObjectSearcher("Select * From Win32_VideoController");
            var searcherList = searcher.Get();

            foreach (ManagementObject item in searcherList)
            {
                gpuName = item["Name"].ToString();
            }

            return gpuName;
        }

        public string GetTotalMemory()
        {
            var computerInfo = new ComputerInfo();
            ulong memory = ulong.Parse(computerInfo.TotalPhysicalMemory.ToString());
            return ((memory / (1024 * 1024)) + " MB").ToString();
        }
    }
}
