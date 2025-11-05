using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCasesMergerUtililty
{
     class StatusType
    {
        public string Status { get; set; }
        public List<string> Members { get; set; }

        public StatusType()
        {
            Status = "";
            Members = new List<string>();
        }
    }
}
