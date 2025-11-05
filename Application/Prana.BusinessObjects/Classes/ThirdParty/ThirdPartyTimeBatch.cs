using System;
using System.ComponentModel;

namespace Prana.BusinessObjects.Classes.ThirdParty
{
    public class ThirdPartyTimeBatch
    {
        [Browsable(false)]
        public int ID { get; set; }

        public DateTime BatchRunTime { get; set; }

        [Browsable(false)]
        public bool IsPaused { get; set; }
    }
}
