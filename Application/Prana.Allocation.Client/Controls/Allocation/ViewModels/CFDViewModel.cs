using Prana.BusinessObjects;
using Prana.SM.OTC;
using System;

namespace Prana.Allocation.Client.Controls.Allocation.ViewModels
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

        internal void SetData(CFDTradeData secMasterOTCData)
        {
            try
            {
                var collateral_DayCount = DayCount.ACT_30_360;
                Enum.TryParse(secMasterOTCData.Collateral_DayCount.ToString(), out collateral_DayCount);

                var finance_DayCount = DayCount.ACT_30_360;
                Enum.TryParse(secMasterOTCData.FinanceLeg_DayCount.ToString(), out finance_DayCount);

                var finance_Commisionbais = CommisionType.Contract;
                Enum.TryParse(secMasterOTCData.CommissionBasis.ToString(), out finance_Commisionbais);

                var finance_interesrRatebenchmark = RateType.Fixed;
                Enum.TryParse(secMasterOTCData.FinanceLeg_InterestRate.ToString(), out finance_interesrRatebenchmark);


                CFDData.Collateral_DayCount = collateral_DayCount;
                CFDData.Collateral_Margin = secMasterOTCData.Collateral_Margin;
                CFDData.Collateral_Rate = secMasterOTCData.Collateral_Rate;
                CFDData.Finance_InteresrRatebenchmark = finance_interesrRatebenchmark;
                CFDData.Finance_Fixedrate = secMasterOTCData.FinanceLeg_FixedRate;
                CFDData.Finance_DayCount = finance_DayCount;
                CFDData.Finance_SpreadBP = secMasterOTCData.FinanceLeg_SpreadBasisPoint;
                CFDData.Finance_ScriptlendingFee = secMasterOTCData.FinanceLeg_ScriptlendingFee;
                CFDData.CFD_Commissionbasis = finance_Commisionbais;
                CFDData.CFD_HardCommRate = secMasterOTCData.HardCommissionRate;
                CFDData.CFD_SoftCommRate = secMasterOTCData.SoftCommissionRate;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
