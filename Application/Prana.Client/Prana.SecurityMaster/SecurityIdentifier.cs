using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.SecurityMaster
{
    public class SecurityIdentifier
    {
        #region  private varibles
      
        private string _ISIN = string.Empty;
        private string _CUSIP = string.Empty;
        private string _SEDOL = string.Empty;
        private string _RIC = string.Empty;
        private string _PranaSymbol = string.Empty;
        private string _CompanyName = string.Empty;

        #endregion  private varibles

        public SecurityIdentifier()
        {

        }
     
        public string ISIN
        {
            get { return _ISIN; }
            set { _ISIN = value; }
        }      

        public string CUSIP
        {
            get { return _CUSIP; }
            set { _CUSIP = value; }
        }       

        public string SEDOL
        {
            get { return _SEDOL; }
            set { _SEDOL = value; }
        }      

        public string  RIC
        {
            get { return _RIC; }
            set { _RIC = value; }
        }

        private string _BBCode = string.Empty;

        public string  BBCode
        {
            get { return _BBCode; }
            set { _BBCode = value; }
        }   

        public string  PranaSymbol
        {
            get { return _PranaSymbol; }
            set { _PranaSymbol = value; }
        }     

        public string  CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value; }
        }

    }
}
