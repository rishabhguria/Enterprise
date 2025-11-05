using System.Collections.Generic;
using System.Xml.Linq;

namespace Nirvana.Middleware
{
    public class FundEqualityComparer : IEqualityComparer<Fund>
    {
        public bool Equals(Fund x, Fund y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else
                return x.FundID == y.FundID;
        }

        public int GetHashCode(Fund obj)
        {
            if (obj == null)
                return 0;
            else
                return obj.FundID.GetHashCode();
        }
    }

    public class Fund
    {
        public int FundID { get; set; }

        public string FundName { get; set; }

        public override string ToString()
        {
            XElement fundElement;

            fundElement = new XElement("FundIDSymbol",
            new XElement("FundID", this.FundID),
            new XElement("Fund", this.FundName));

            return fundElement.ToString();
        }
    }
}
