using System;
using System.Diagnostics;

namespace Prana.MonitoringProcessor
{
    class PerformanceHelper : IDisposable
    {
        protected PerformanceCounter cpuCounter;
        protected PerformanceCounter ramCounter;

        /*
First you have to create the 2 performance counters
using the System.Diagnostics.PerformanceCounter class.
*/


        public PerformanceHelper()
        {
            cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }
        /*
        Call this method every time you need to know
        the current cpu usage.
        */

        public string GetCurrentCpuUsage()
        {
            return cpuCounter.NextValue() + "%";
        }

        /*
        Call this method every time you need to get
        the amount of the available RAM in Mb
        */
        public string GetAvailableRAM()
        {
            return ramCounter.NextValue() + "Mb";
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                cpuCounter.Dispose();
                ramCounter.Dispose();
            }
        }
    }
}
