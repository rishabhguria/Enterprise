using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class PerformanceParameters
    {
        public PerformanceParameters(string availableMemory, string cpuUsage)
        {
            _availableMemory = availableMemory;
            _cpuUsage = cpuUsage;
        }
        private string _availableMemory;

        public string AvailableMemory
        {
            get { return _availableMemory; }
        }
        private string _cpuUsage;

        public string CPUusage
        {
            get { return _cpuUsage; }
        }

    }
}
