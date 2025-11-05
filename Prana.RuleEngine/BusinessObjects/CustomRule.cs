using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.RuleEngine.BusinessObjects
{
    class CustomRule
    {
        private String _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
        private Boolean _enabled;
        public Boolean Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        private Boolean _isDeleted;
        public Boolean IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; }
        }
        private String _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
       
        private String _compressionLevel;
        public string CompressionLevel
        {
            get { return _compressionLevel; }
            set { _compressionLevel = value; }
        }

        private String _ruleId;
        public string RuleId
        {
            get { return _ruleId; }
            set { _ruleId = value; }
        }

        private String _ruleType;
        public string RuleType
        {
            get { return _ruleType; }
            set { _ruleType = value; }
        }
        private String _htmlPath;
        public string HtmlPath
        {
            get { return _htmlPath; }
            set { _htmlPath = value; }
        }

    }
}
