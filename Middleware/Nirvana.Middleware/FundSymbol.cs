using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using Nirvana.Middleware.Linq;

namespace Nirvana.Middleware
{
    public class FundSymbol
    {
        public int FundID { get; set; }

        public string FundName { get; set; }

        public Fund Fund
        {
            get
            {
                return new Fund { FundID = FundID, FundName = FundName };
            }
            set
            {
                if (value != null)
                {
                    FundID = value.FundID;
                    FundName = value.FundName;
                }
            }
        }

        public string Symbol { get; set; }

        public static IEnumerable<FundSymbol> GetFundSymbols(DateTime _FromDate, DateTime _ToDate)
        {
            using (NirvanaDataContext db = new NirvanaDataContext())
            {
                List<FundSymbol> _Results = db.P_MW_GetFundIDSymbol(_FromDate, _ToDate).ToList();
                return _Results;
            }
        }

        public override string ToString()
        {
            XElement symbolElement;

            symbolElement = new XElement("FundIDSymbol",
            new XElement("FundID", this.Fund.FundID),
            new XElement("Fund", this.Fund.FundName),
            new XElement("Symbol", this.Symbol));

            return symbolElement.ToString();
        }
    }
}
