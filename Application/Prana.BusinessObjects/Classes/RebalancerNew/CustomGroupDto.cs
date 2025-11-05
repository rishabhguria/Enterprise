using System.Collections.Generic;

namespace Prana.BusinessObjects.Classes.RebalancerNew
{
    public class CustomGroupDto
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public List<int> FundGroupMapping { get; set; }
    }
}
