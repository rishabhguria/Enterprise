using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.Interfaces
{
    public interface IRiskPreferenceDataManager
    {
        SortedList<int, float> SetInerestRateFromDB(RiskPrefernece riskPreferences);
        Dictionary<string, string> SetPSSymbolMappingFromDB(RiskPrefernece riskPreferences);
        DataTable SetDefaultVolShockAdjFactorFromDB(RiskPrefernece riskPreferences);
        void DeleteInterestRateFromDB(int id);
        string SaveInterestRatesToDB(DataTable dt);
        string SavePSSymbolMappingToDB(DataTable dt);
    }
}
