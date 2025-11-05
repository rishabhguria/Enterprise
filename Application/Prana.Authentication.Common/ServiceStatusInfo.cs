using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.Authentication.Common
{
    [Serializable]
    public class ServiceStatusInfo
    {
        private string _errorMessage = string.Empty;
        private bool _status = false;
        public bool Status
        {
            get { return _status; }
            set { _status = value; }
        }
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
    }
}
