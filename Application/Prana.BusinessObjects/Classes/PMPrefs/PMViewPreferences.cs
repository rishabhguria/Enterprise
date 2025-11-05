using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// This class is the root class to store the column related preferences based on 
    /// different custom views
    /// </summary>

    [Serializable]
    public class PMViewPreferences
    {

        public string TabName;

        public List<string> SelectedDynamicColumnsCollection;


        public List<string> GroupByDynamicColumnsCollection;

        public PMViewPreferences()
        {
            try
            {

                TabName = string.Empty;
                SelectedDynamicColumnsCollection = new List<string>();
                GroupByDynamicColumnsCollection = new List<string>();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private List<string> _getAllColumns;

        public List<string> GetAllColumns
        {
            get
            {
                try
                {
                    if (_getAllColumns == null)
                    {
                        _getAllColumns = new List<string>();
                        //This is needed in all the cases
                        _getAllColumns.Add("HasBeenSentToUser");
                        _getAllColumns.Add("ID");
                        if (SelectedDynamicColumnsCollection.Count > 0)
                            _getAllColumns.AddRange(SelectedDynamicColumnsCollection);
                        if (GroupByDynamicColumnsCollection.Count > 0)
                        {
                            foreach (string groupByColumn in GroupByDynamicColumnsCollection)
                            {
                                if (!_getAllColumns.Contains(groupByColumn))
                                    _getAllColumns.Add(groupByColumn);
                            }
                        }
                    }
                    return _getAllColumns;
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                    if (rethrow)
                    {
                        throw;
                    }
                }
                return null;
            }
        }

        public void UpdateColumn(string columnName, ExPNLPreferenceMsgType MsgType)
        {
            try
            {
                switch (MsgType)
                {
                    case ExPNLPreferenceMsgType.SelectedColumnAdded:
                        if (!SelectedDynamicColumnsCollection.Contains(columnName))
                            SelectedDynamicColumnsCollection.Add(columnName);
                        break;
                    case ExPNLPreferenceMsgType.SelectedColumnDeleted:
                        if (SelectedDynamicColumnsCollection.Contains(columnName))
                            SelectedDynamicColumnsCollection.Remove(columnName);
                        break;

                    case ExPNLPreferenceMsgType.GroupByColumnAdded:
                        if (!GroupByDynamicColumnsCollection.Contains(columnName))
                            GroupByDynamicColumnsCollection.Add(columnName);
                        break;
                    case ExPNLPreferenceMsgType.GroupByColumnDeleted:
                        if (GroupByDynamicColumnsCollection.Contains(columnName))
                            GroupByDynamicColumnsCollection.Remove(columnName);
                        break;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            //So that it can be reinitialized
            _getAllColumns = null;
        }
    }
}
