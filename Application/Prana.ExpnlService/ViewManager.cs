using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ExpnlService
{
    public class ViewManager
    {
        private Dictionary<string, PMViewPreferencesList> _dicUserPMPreference = new Dictionary<string, PMViewPreferencesList>();

        public Dictionary<string, PMViewPreferencesList> DicUserPMPreference
        {
            get { return _dicUserPMPreference; }
            set { _dicUserPMPreference = value; }
        }

        static ViewManager _viewManager;
        public static ViewManager GetInstance()
        {
            if (_viewManager == null)
            {
                _viewManager = new ViewManager();
            }
            return _viewManager;
        }

        public void HandlePMPreferences(string userID, ExPNLPreferenceMsgType prefMSGType, string subMessage)
        {
            try
            {
                switch (prefMSGType)
                {
                    case ExPNLPreferenceMsgType.NewPreferences:

                        PMViewPreferencesList pmViewPrefList = new PMViewPreferencesList(subMessage);

                        if (_dicUserPMPreference.ContainsKey(userID))
                            _dicUserPMPreference[userID] = pmViewPrefList;
                        else
                            _dicUserPMPreference.Add(userID, pmViewPrefList);
                        break;

                    case ExPNLPreferenceMsgType.CustomViewAdded:
                        string[] str = subMessage.Split(',');
                        if (_dicUserPMPreference.ContainsKey(userID) && !string.IsNullOrEmpty(str[0]) && !string.IsNullOrEmpty(str[1]) && _dicUserPMPreference[userID].ContainsKey(str[1]) && !_dicUserPMPreference[userID].ContainsKey(str[0]))
                        {
                            _dicUserPMPreference[userID].Add(str[0], _dicUserPMPreference[userID][str[1]]);
                            _dicUserPMPreference[userID].SelectedView = str[0];
                        }
                        break;

                    case ExPNLPreferenceMsgType.CustomViewDeleted:
                        str = subMessage.Split(',');
                        if (_dicUserPMPreference.ContainsKey(userID) && string.IsNullOrEmpty(str[0]) && string.IsNullOrEmpty(str[1]) && _dicUserPMPreference[userID].ContainsKey(str[1]) && _dicUserPMPreference[userID].ContainsKey(str[0]))
                        {
                            _dicUserPMPreference[userID].Remove(str[0]);
                            _dicUserPMPreference[userID].SelectedView = str[1];
                        }
                        break;

                    case ExPNLPreferenceMsgType.GroupByColumnAdded:
                    case ExPNLPreferenceMsgType.GroupByColumnDeleted:

                    case ExPNLPreferenceMsgType.SelectedColumnAdded:
                    case ExPNLPreferenceMsgType.SelectedColumnDeleted:
                        if (_dicUserPMPreference.ContainsKey(userID))
                            _dicUserPMPreference[userID].UpdateColumn(subMessage, prefMSGType);
                        break;
                    case ExPNLPreferenceMsgType.SelectedViewChanged:
                        if (_dicUserPMPreference.ContainsKey(userID) && _dicUserPMPreference[userID].ContainsKey(subMessage))
                            _dicUserPMPreference[userID].SelectedView = subMessage;
                        break;
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
    }
}