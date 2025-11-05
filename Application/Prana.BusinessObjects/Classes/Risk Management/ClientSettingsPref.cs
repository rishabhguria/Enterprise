using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ClientSettingsPref
    {
        [NonSerialized]
        Dictionary<string, ClientSettings> _dict = new Dictionary<string, ClientSettings>();

        private List<ClientSettings> _clientSettingsList;

        public List<ClientSettings> ClientSettingsList
        {
            get
            {
                return _clientSettingsList;
            }
            set { _clientSettingsList = value; }
        }
        public List<ClientSettings> getClientSettings()
        {
            List<ClientSettings> settings = new List<ClientSettings>();
            foreach (ClientSettings setting in _clientSettingsList)
            {

                settings.Add((ClientSettings)setting);
            }
            return settings;

        }

        public ClientSettings getClientSettings(string clientSettingName)
        {
            if (_dict.ContainsKey(clientSettingName))
                return (ClientSettings)_dict[clientSettingName].Clone();
            return null;
        }

        public List<string> getNames()
        {
            if (_clientSettingsList != null)
                return new List<string>(_dict.Keys);
            return null;
        }

        public void LoadDictionary()
        {
            try
            {
                if (_dict.Count == 0 && _clientSettingsList.Count != 0)
                {
                    foreach (ClientSettings _clientSettings in _clientSettingsList)
                        _dict.Add(_clientSettings.ClientSettingName, _clientSettings);

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
