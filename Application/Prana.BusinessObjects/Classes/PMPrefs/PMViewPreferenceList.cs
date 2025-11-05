using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.BusinessObjects
{
    [Serializable, System.Runtime.InteropServices.ComVisible(false)]
    public class PMViewPreferencesList : Dictionary<string, PMViewPreferences>
    {
        private string _selectedView;

        public string SelectedView
        {
            get { return _selectedView; }
            set
            {
                _selectedView = value;
            }
        }
        private List<string> _defaultColumns = null;

        public List<string> DefaultColumns
        {
            get
            {
                if (_defaultColumns == null)
                {
                    _defaultColumns = new List<string>();
                    //This is needed in all the cases
                    _defaultColumns.Add("HasBeenSentToUser");
                    _defaultColumns.Add("ID"); return _defaultColumns;
                }
                return _defaultColumns;
            }
        }

        public List<string> DynamicColumnsToUpdate
        {

            get
            {
                try
                {
                    if (_selectedView != null && base.ContainsKey(_selectedView) && base[_selectedView].GetAllColumns.Count > 0)
                    {
                        return base[_selectedView].GetAllColumns;
                    }
                    else
                    {
                        return DefaultColumns;
                    }
                }
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
                return new List<string>();
            }
        }

        public PMViewPreferencesList()
        {

        }

        protected PMViewPreferencesList(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }

        public PMViewPreferencesList(string strValues)
        {
            try
            {
                string[] PMViewPrefList = strValues.Split(Seperators.SEPERATOR_5);
                this.SelectedView = PMViewPrefList[1];
                string[] lsPMViewPref = PMViewPrefList[0].Split(Seperators.SEPERATOR_4);
                foreach (string strPref in lsPMViewPref)
                {
                    if (!string.IsNullOrEmpty(strPref))
                    {
                        PMViewPreferences pmViewPrefObj = new PMViewPreferences();
                        string[] propPMViewPref = strPref.Split(Seperators.SEPERATOR_3);
                        pmViewPrefObj.TabName = propPMViewPref[0];
                        if (!string.IsNullOrEmpty(propPMViewPref[1]))
                        {
                            pmViewPrefObj.SelectedDynamicColumnsCollection = new List<string>((IEnumerable<string>)propPMViewPref[1].Split(','));
                        }
                        if (!string.IsNullOrEmpty(propPMViewPref[2]))
                        {
                            pmViewPrefObj.GroupByDynamicColumnsCollection = new List<string>((IEnumerable<string>)propPMViewPref[2].Split(','));
                        }
                        this.Add(pmViewPrefObj.TabName, pmViewPrefObj);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder msg = new StringBuilder();
            try
            {
                foreach (PMViewPreferences pmvp in base.Values)
                {
                    msg.Append(pmvp.TabName);

                    //SelectedDynamicColumn seperator                               
                    msg.Append(Seperators.SEPERATOR_3);
                    if (pmvp.SelectedDynamicColumnsCollection != null && pmvp.SelectedDynamicColumnsCollection.Count > 0)
                    {
                        foreach (string column in pmvp.SelectedDynamicColumnsCollection)
                            msg.Append(column).Append(Seperators.SEPERATOR_8);

                        msg.Length--;
                    }
                    //Groupbycolumn_dynamic_Seperator
                    msg.Append(Seperators.SEPERATOR_3);
                    if (pmvp.GroupByDynamicColumnsCollection != null && pmvp.GroupByDynamicColumnsCollection.Count > 0)
                    {
                        foreach (string grpColumn in pmvp.GroupByDynamicColumnsCollection)
                            msg.Append(grpColumn).Append(Seperators.SEPERATOR_8);

                        msg.Length--;
                    }
                    msg.Append(Seperators.SEPERATOR_4);
                }
                if (msg.Length > 0)
                    msg.Length--;

                msg.Append(Seperators.SEPERATOR_5);
                msg.Append(_selectedView);
                return msg.ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return msg.ToString();
        }

        public void UpdateColumn(string columnName, ExPNLPreferenceMsgType MsgType)
        {
            try
            {
                if (_selectedView != null && base.ContainsKey(_selectedView))
                    base[_selectedView].UpdateColumn(columnName, MsgType);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}