using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.RuleEngine.BussinessObjects
{
    public class RulesAsset
    {
        private string _title;

        public string title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _packageName;

        public string packageName
        {
            get { return _packageName; }
            set { _packageName = value; }
        }
        private int _compressionLevelID;

        public int compressionLevelID
        {
            get { return _compressionLevelID; }
            set { _compressionLevelID = value; }
        }
        private string _ruleID;

        public string ruleID
        {
            get { return _ruleID; }
            set { _ruleID = value; }
        }
        private string _description;

        public string description
        {
            get { return _description; }
            set { _description = value; }
        }
        private Metadata _metadata = new Metadata();

        public Metadata metadata
        {
            get { return _metadata; }
            set { _metadata = value; }
        }

        
    }
}
