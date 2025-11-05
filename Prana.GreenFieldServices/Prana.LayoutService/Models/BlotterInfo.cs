using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.LayoutService.Models
{
    public class BlotterTabInfo
    {
       public string viewId { get; set; }
       public string tabName { get; set; }
    }

    public class BlotterTabRenameInfo
    {
        public string viewId { get; set;}

        public CustomTabsDetails customTabsDetails { get; set; }
    }

    public class CustomTabsDetails
    {
        public Dictionary<string, string> tabtoRename { get; set; }
    }
}
