using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using System.Xml.Serialization;
using Prana.Interfaces;
using System.Data;


namespace Prana.PM.Client.UI
{
  [Serializable]
    public class ClosingPreferences:IPreferenceData
    {
        private AccountingMethods _accountingMethods = new AccountingMethods();
       
        [XmlIgnore]
        public AccountingMethods AccountingMethods
        {
            set { _accountingMethods = value; }
            get { return _accountingMethods; }
        }

        private bool _isShortWithBuyandBTC = false;


        public bool IsShortWithBuyandBTC
        {
            get { return _isShortWithBuyandBTC; }
            set { _isShortWithBuyandBTC = value; }
        }

        private bool _overrideGlobal = false;

        public bool  OverrideGlobal
        {
            get { return _overrideGlobal; }
            set { _overrideGlobal = value; }
        }


        private int _globalClosingAlgo;

        public int GlobalClosingAlgo
        {
            get { return _globalClosingAlgo; }
            set { _globalClosingAlgo = value; }
        }

	
    }
}
