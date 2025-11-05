using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using System;


namespace Prana.BusinessObjects
{
    public class Data : EventArgs
    {
        private SymbolData _info;

        public SymbolData Info
        {
            get
            {
                return _info;
            }
            set
            {
                _info = value;
            }
        }

        //added by omshiv
        private SecMasterBaseObj _secMasterBaseObj;
        public SecMasterBaseObj SecMasterData
        {
            get
            {
                return _secMasterBaseObj;
            }
            set
            {
                _secMasterBaseObj = value;
            }
        }


    }
}
