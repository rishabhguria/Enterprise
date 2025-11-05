using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class RepeatingFixField
    {
        public string Tag { get; set; }
        public bool IsRepeatingTag { get; set; }
        public Dictionary<string, RepeatingFixField> RepeatingFixFields { get; set; } = new Dictionary<string, RepeatingFixField>();
    }
}
