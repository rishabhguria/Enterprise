using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
namespace Prana.ClientPreferences
{
    /// <summary>
    /// Summary description for PreferencesUniversalSettingsCollection.
    /// </summary>
    public class PreferencesUniversalSettingsCollection
    {
        public PreferencesUniversalSettingsCollection()
        {

        }
        Dictionary<string, PreferencesUniversalSettings> _prefsCollection = new Dictionary<string, PreferencesUniversalSettings>();
        public void Add(PreferencesUniversalSettings pref)
        {
            string prefID = pref.PrefID;
            if (_prefsCollection.ContainsKey(prefID))
            {
                _prefsCollection[prefID] = pref;
            }
            else
            {
                _prefsCollection.Add(prefID, pref);
            }
        }
        public void Remove(PreferencesUniversalSettings pref)
        {
            _prefsCollection.Remove(pref.PrefID);
        }

        public PreferencesUniversalSettings GetPref(int assetID, int underlyingID, int counterPartyID, int venueID)
        {
            String ID = IDGenerator.GetAUCVID(assetID, underlyingID, counterPartyID, venueID);
            if (_prefsCollection.ContainsKey(ID))
            {
                return _prefsCollection[ID];
            }

            else
            {
                return null;
            }
        }
        public PreferencesUniversalSettings GetDefaultPref(int assetID, int underlyingID)
        {

            PreferencesUniversalSettings defaultPref = null;
            foreach (KeyValuePair<string, PreferencesUniversalSettings> prefpair in _prefsCollection)
            {
                if (prefpair.Value.AssetID.ToString() == assetID.ToString() && prefpair.Value.UnderlyingID.ToString() == underlyingID.ToString() && prefpair.Value.IsDefaultCV)
                {
                    defaultPref = prefpair.Value;
                    break;
                }
            }
            return defaultPref;
        }
        public int Count
        {
            get { return _prefsCollection.Count; }
        }
        public Dictionary<string, PreferencesUniversalSettings> PrefsCollection
        {
            get { return _prefsCollection; }
        }
        public void RemoveDefaultCV(string assetID, string underLyingID)
        {
            foreach (KeyValuePair<string, PreferencesUniversalSettings> prefKeyValue in _prefsCollection)
            {
                if (prefKeyValue.Value.AssetID.Equals(assetID) && prefKeyValue.Value.UnderlyingID.Equals(underLyingID))
                {
                    prefKeyValue.Value.IsDefaultCV = false;
                }
            }
        }
    }
}
