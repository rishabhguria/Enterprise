using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public interface ILiveFeedProvider
    {
        
        DataTable GetLiveFeedData(DataTable symbolInfo);

        Dictionary<string, string> ValidateSymbol(string symbol, string asset);
    }
}
