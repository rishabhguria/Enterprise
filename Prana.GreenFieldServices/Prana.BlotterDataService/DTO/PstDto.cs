using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.BlotterDataService.DTO
{
    public class PSTResponseDTO
    {
        public PTTAllocDetailsRequest pttRequestObject { get; set; } = new PTTAllocDetailsRequest();
        public List<PTTResponseObject> pttResponseObjects { get; set; } = new List<PTTResponseObject>();
    }
    public class PSTRequestDTO
    {
        public int allocationPrefID { get; set;}
        public string symbol { get; set;}
        public string OrderSideId { get; set; }
    }
}
