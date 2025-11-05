using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Prana.PM.BLL
{
    /// <summary>
    /// This class is the root class to store the column related preferences based on 
    /// different custom views
    /// </summary>
    [XmlRoot("CustomViewPreferences")]
    [Serializable]
    public class CustomViewPreferences : IDisposable
    {
        [XmlElement("SplitterPosition", typeof(int))]
        public int SplitterPosition;

        [XmlElement("FilterDetails", typeof(GridColumnFilterDetails))]
        public GridColumnFilterDetails FilterDetails;

        [XmlArray("SelectedColumnsCollection"), XmlArrayItem("Column", typeof(PreferenceGridColumn))]
        public List<PreferenceGridColumn> SelectedColumnsCollection;

        [XmlArray("GroupByColumnsCollection"), XmlArrayItem("GroupByColumn", typeof(string))]
        public List<string> GroupByColumnsCollection;

        [XmlElement("IsDashboardVisible", typeof(bool))]
        public bool IsDashboardVisible;

        public CustomViewPreferences()
        {
            SelectedColumnsCollection = new List<PreferenceGridColumn>();
            GroupByColumnsCollection = new List<string>();
            SplitterPosition = int.MinValue;
            FilterDetails = new GridColumnFilterDetails();
        }

        private bool _isSaved = false;

        [XmlIgnore]
        public bool IsSaved
        {
            get { return _isSaved; }
            set { _isSaved = value; }
        }

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    if (SelectedColumnsCollection != null)
                    {
                        foreach (PreferenceGridColumn obj in SelectedColumnsCollection)
                        {
                            obj.Dispose();
                        }
                        SelectedColumnsCollection.Clear();
                        SelectedColumnsCollection = null;
                    }
                    if (GroupByColumnsCollection != null)
                        GroupByColumnsCollection.Clear();
                    GroupByColumnsCollection = null;
                    FilterDetails.Dispose();
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
        }
        #endregion
    }
}
