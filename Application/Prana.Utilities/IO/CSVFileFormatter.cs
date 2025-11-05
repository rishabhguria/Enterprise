using Prana.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Prana.Utilities
{
    public class CSVFileFormatter : IFileFormatter
    {

        #region IFileFormatter Members

        public bool CreateFile(IList list, IList summary, string Location, Dictionary<string, string> columnsWithSpecifiedNames)
        {
            try
            {
                if (list.Count > 0 || columnsWithSpecifiedNames != null)
                {

                    DataSet dsToConvertToCSV;
                    if (columnsWithSpecifiedNames == null)
                    {
                        dsToConvertToCSV = Prana.Utilities.MiscUtilities.GeneralUtilities.CreateDataSetFromCollection(list, null);
                    }
                    else
                    {
                        dsToConvertToCSV = Prana.Utilities.MiscUtilities.GeneralUtilities.CreateDataSetFromCollectionWithHeaders(list, columnsWithSpecifiedNames);

                    }
                    DataSet dssummary = null;
                    if (summary != null)
                    {
                        dssummary = Prana.Utilities.MiscUtilities.GeneralUtilities.CreateDataSetFromCollection(summary, null);
                    }
                    CSVFileHealper.ProduceCSV(dsToConvertToCSV.Tables[0], dssummary, Location, true);
                }
                else
                {
                    throw new Exception("No Data Received So CSV File Cannot Be Created...");
                }
            }
            catch //(Exception e)
            {
                throw;
            }
            return true;
        }

        #endregion
    }
}
