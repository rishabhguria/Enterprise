using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.RuleEngine.Core;

namespace Nirvana.RiskManagement
{
    public class TradingAccountFactData : IFactData
    {
        #region IDataMapper Members

        public long ExposureLimit
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public long CalculatedExposureLimit
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion

        public long PositivePnlLimit
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public long NegativePnlLimit
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public long CalculatedPositivePnlLimit
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public long CalculatedNegativePnlLimit
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        #region IDataMapping Members


        public string Id
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion
    }
}
