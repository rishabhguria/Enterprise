using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;

namespace Prana.AutomationHandlers
{
    public class ColumnManager
    {
        static ColumnManager()
        {
            if (ListOfIEColumns == null || DicRRTypeColumns == null || DicRRTypeColumnID == null)
                InitializeDictionaries();
        }

        #region private properties Section

        private static GenericBindingList<InternalExternalColumnsStruct> _lsIEColumns;

        private static GenericBindingList<InternalExternalColumnsStruct> ListOfIEColumns
        {
            get { return _lsIEColumns; }
            set { _lsIEColumns = value; }
        }


        //It will Contain all the Record from T_RiskReportTypeColumnMapping 
        private static Dictionary<string,List<int>> _dicRRTypeColumnID;

        private static Dictionary<string, List<int>> DicRRTypeColumnID
        {
            get { return _dicRRTypeColumnID; }
            set { _dicRRTypeColumnID = value; }
        }

        //it will contain the Dictionary of internal name and external name(of Columns) for each Type
        private static Dictionary<string, Dictionary<string, string>> _dicRRTypeColumns;

        public static Dictionary<string, Dictionary<string, string>> DicRRTypeColumns
        {
            get { return _dicRRTypeColumns; }
            set { _dicRRTypeColumns = value; }
        }

        
        #endregion

        public static void InitializeDictionaries()
        {
            try
            {
                ListOfIEColumns = AutomationHandlerDataManager.getInternalExternalColumnsFromDataBase();
                DicRRTypeColumnID = AutomationHandlerDataManager.getColumnRTypeMappingDictionary();
                CreateReportTypeColumnDictionary();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }


        }

        private static void CreateReportTypeColumnDictionary()
        {
            try
            {
                if (ListOfIEColumns != null && DicRRTypeColumnID != null)
                {
                    DicRRTypeColumns = new Dictionary<string, Dictionary<string, string>>(); 

                    foreach (string RiskReportType in DicRRTypeColumnID.Keys)
                    {
                        List<int> lsColumnIds = DicRRTypeColumnID[RiskReportType];
                        foreach (int columnID in lsColumnIds)
                        {
                            InternalExternalColumnsStruct _internalExternalColumnsStruct = ListOfIEColumns.GetItem(columnID.ToString());
                            string internalColumnName = _internalExternalColumnsStruct.InternalColumnName;
                            string externalColumnName = _internalExternalColumnsStruct.ExternalColumnName;

                            if (!DicRRTypeColumns.ContainsKey(RiskReportType))
                            {
                                Dictionary<string, string> IENameDictionary = new Dictionary<string, string>();
                                IENameDictionary.Add(internalColumnName, externalColumnName);
                                DicRRTypeColumns.Add(RiskReportType, IENameDictionary);
                            }
                            else
                                DicRRTypeColumns[RiskReportType].Add(internalColumnName, externalColumnName);
                        }                       
                    }

                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static void Refresh()
        {
            try
            {
                InitializeDictionaries();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

    }
}
