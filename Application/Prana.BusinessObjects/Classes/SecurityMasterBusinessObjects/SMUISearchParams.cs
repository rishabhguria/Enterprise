using System;

namespace Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects
{
    // Save search parameter of SM UI 
    [Serializable]
    public class SMUISearchParams
    {
        private String searchCriteria;

        public String SearchCriteria
        {
            get { return searchCriteria; }
            set { searchCriteria = value; }
        }

        private String matchOn;

        public String MatchOn
        {
            get { return matchOn; }
            set { matchOn = value; }
        }
        private string enteredText;

        public string EnteredText
        {
            get { return enteredText; }
            set { enteredText = value; }
        }
        private string sMColumnsView;

        public string SMColumnsView
        {
            get { return sMColumnsView; }
            set { sMColumnsView = value; }
        }

        private int searchType;

        public int SearchType
        {
            get { return searchType; }
            set { searchType = value; }
        }
        private Boolean searchUnApprovedSec;

        public Boolean SearchUnApprovedSec
        {
            get { return searchUnApprovedSec; }
            set { searchUnApprovedSec = value; }
        }


    }
}
