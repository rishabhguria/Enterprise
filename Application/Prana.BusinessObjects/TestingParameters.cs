using Prana.Global;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class TestingParameters
    {

        string _formName;

        public string FormName
        {
            get { return _formName; }
            set { _formName = value; }
        }

        Dictionary<string, object> _data = new Dictionary<string, object>();

        public Dictionary<string, object> Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public event EventHandler<EventArgs<string>> ParameterAdded = null;

        public void AddParameter(string key, object data)
        {

            if (_data.ContainsKey(key))
            {
                _data[key] = data;
            }
            else
            {
                _data.Add(key, data);
            }
            if (ParameterAdded != null)
            {
                ParameterAdded(this, new EventArgs<string>(key));
            }

        }

    }
}
