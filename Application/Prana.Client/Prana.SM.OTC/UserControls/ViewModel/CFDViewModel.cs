using Prana.BusinessObjects;
using System;

namespace Prana.SM.OTC
{
    public class CFDViewModel : BindableBase
    {

        private SecMasterCFDModel _CFDData;
        public SecMasterCFDModel CFDData
        {
            get { return _CFDData; }
            set
            {
                _CFDData = value;
                OnPropertyChanged();
            }
        }


        public CFDViewModel()
        {
            CFDData = new SecMasterCFDModel();
        }


        internal void SetData(SecMasterCFDData secMasterOTCData)
        {
            try
            {
                var collateral_DayCount = DayCount.ACT_30_360;
                Enum.TryParse(secMasterOTCData.Collateral_DayCount.ToString(), out collateral_DayCount);

                var finance_DayCount = DayCount.ACT_30_360;
                Enum.TryParse(secMasterOTCData.Finance_DayCount.ToString(), out finance_DayCount);

                var finance_Commisionbais = CommisionType.Contract;
                Enum.TryParse(secMasterOTCData.CFD_Commissionbasis.ToString(), out finance_Commisionbais);

                var finance_interesrRatebenchmark = RateType.Fixed;
                Enum.TryParse(secMasterOTCData.Finance_InteresrRatebenchmark.ToString(), out finance_interesrRatebenchmark);


                CFDData.Collateral_DayCount = collateral_DayCount;
                CFDData.Collateral_Margin = secMasterOTCData.Collateral_Margin;
                CFDData.Collateral_Rate = secMasterOTCData.Collateral_Rate;
                CFDData.Finance_InteresrRatebenchmark = finance_interesrRatebenchmark;
                CFDData.Finance_Fixedrate = secMasterOTCData.Finance_Fixedrate;
                CFDData.Finance_DayCount = finance_DayCount;
                CFDData.Finance_SpreadBP = secMasterOTCData.Finance_SpreadBP;
                CFDData.Finance_ScriptlendingFee = secMasterOTCData.Finance_ScriptlendingFee;
                CFDData.CFD_Commissionbasis = finance_Commisionbais;
                CFDData.CFD_HardCommRate = secMasterOTCData.CFD_HardCommRate;
                CFDData.CFD_SoftCommRate = secMasterOTCData.CFD_SoftCommRate;
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal void SetData(SecMasterCFDData cfdData, DateTime effectiveDate)
        {

        }
    }
}
