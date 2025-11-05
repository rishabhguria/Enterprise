using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.CalculationService.Models
{
    public class RtpnlLayoutInfo
    {
        public OpenFinPageInfo pageInfo { get; set; }
        public List<InternalPageInfo> internalPageInfo { get; set; }
    }

    public class OpenFinPageInfo
    {
        public string pageId { get; set; }
        public string pageLayout { get; set; }
        public string pageName { get; set; }
        public string pageTag { get; set; }
        public string oldPageName { get; set; }
    }

    public class InternalPageInfo
    {
        public string title { get; set; }
        public string oldTitle { get; set; }
        public string description { get; set; }
        public string layout { get; set; }
        public string oldViewName { get; set; }
        public string pageId { get; set; }
        public string pageLayout { get; set; }
        public string pageName { get; set; }
        public string pageTag { get; set; }
        public string oldPageName { get; set; }
        public string viewId { get; set; }
    }
}
