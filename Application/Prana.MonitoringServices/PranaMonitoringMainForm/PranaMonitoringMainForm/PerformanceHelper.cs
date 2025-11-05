using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Prana.MonitoringServices
{
    class PerformanceHelper
    {
        protected PerformanceCounter cpuCounter;
        protected PerformanceCounter ramCounter;

        public void SetValues()
        {
            cpuCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            ramCounter = new PerformanceCounter("Memory", "Available MBytes");

        }

        public string GetCurrentCpuUsage()
        {
           return  cpuCounter.NextValue() + "%";
        }

        public string GetAvailableRAM()
        {
          return   ramCounter.NextValue() + "Mb";
        }
    }
}
