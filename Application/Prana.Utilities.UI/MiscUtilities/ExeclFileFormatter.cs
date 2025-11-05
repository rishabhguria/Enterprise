//using ExcelLibrary;
//using Prana.Interfaces;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data;


//namespace Prana.Utilities.UI.MiscUtilities
//{
//    public class ExeclFileFormatter : IFileFormatter
//    {
//        #region IFileFormatter Members

//        public bool CreateFile(IList list, IList summary, string Location, Dictionary<string, string> columnsWithSpecifiedNames)
//        {
//            try
//            {

//                //  Preferences userPreferences = XMLUtilities.XMLUtilities.DeserializeFromXMLFile<Preferences>(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\settings.xml");

//                if (list.Count > 0)
//                {
//                    DataSet dsToConvertToExcel = Prana.Utilities.MiscUtilities.GeneralUtilities.CreateDataSetFromCollectionWithHeaders(list, columnsWithSpecifiedNames);
//                    DataSet dsSummary = Prana.Utilities.MiscUtilities.GeneralUtilities.CreateDataSetFromCollection(summary, null);
//                    DataSetHelper.CreateWorkbook(Location, dsToConvertToExcel, dsSummary);
//                }
//                else
//                {
//                    throw new Exception("No Data Recieved So Execl File Cannot Be Created..");
//                }
//            }
//            catch//(Exception e)
//            {
//                throw;
//            }
//            return true;
//        }

//        #endregion

//        static ExeclFileFormatter()
//        {
//            //colums.Clear();
//            //RiskPreferences userPreferences = DeserializeFromXMLFile<RiskPreferences>();
//            //foreach (string column in userPreferences.Columns)
//            //    if (!colums.Contains(column))
//            //        colums.Add(column);
//        }


//    }
//}
