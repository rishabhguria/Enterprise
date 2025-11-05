using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.RuleEngine.Core;

namespace Nirvana.RiskManagement
{
    public class UserFactData : IFactData
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

        #endregion

        #region IDataMapper Members


        #endregion

        #region IDataMapper Members


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

        public long MaximumPnlLoss
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public long MaximumSizePerBasket
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}
