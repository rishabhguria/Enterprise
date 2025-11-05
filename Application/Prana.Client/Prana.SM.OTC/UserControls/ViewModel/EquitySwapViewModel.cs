using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.Utilities.DateTimeUtilities;
using System;

namespace Prana.SM.OTC
{
    public enum PeriodFrequency
    {
        Monthly = 1,
        Quarterly = 2,
        Semi_Annually = 3,
        Annually = 4,
        AT_Maturity = 5
    }

    /// <summary>
    /// Rate Type
    /// </summary>
    public enum RateType
    {
        LIBOR_30_Day = 1,
        LIBOR_3_Month = 2,
        LIBOR_6_Month = 3,
        LIBOR_12_Month = 4,
        Safex_Overnight = 5,
        Fixed = 6
    }

    /// <summary>
    /// Commision Type
    /// </summary>
    public enum CommisionType
    {
        Shares = 1,
        Notional = 2,
        Contract = 3,
        FlatAmount = 4

    }

    /// <summary>
    /// Commision Type
    /// </summary>
    public enum DayCount
    {
        ACT_30_360 = 1,
        ACT_ACT = 2,
        ACT_360 = 3,
        ACT_30_365 = 4,
        ACT_365 = 5
    }

    public class EquitySwapViewModel : BindableBase
    {
        /// <summary>
        /// Sec Master EquitySwap
        /// </summary>
        private SecMasterEquitySwapModel _equitySwap;
        public SecMasterEquitySwapModel EquitySwap
        {
            get { return _equitySwap; }
            set
            {
                _equitySwap = value;
                OnPropertyChanged();
            }
        }

        private string isTradeView = "Collapsed";

        public string IsTradeView
        {
            get { return isTradeView; }
            set
            {
                isTradeView = value;
                OnPropertyChanged("IsTradeView");
            }
        }

        /// <summary>
        /// EquitySwap ViewModel
        /// </summary>
        public EquitySwapViewModel()
        {
            EquitySwap = new SecMasterEquitySwapModel();
        }


        internal void SetData(SecMasterEquitySwap secMasterEquitySwap, DateTime effectiveDate)
        {
            try
            {

                var periodFrequency = PeriodFrequency.Monthly;
                Enum.TryParse(secMasterEquitySwap.EquityLeg_Frequency, out periodFrequency);

                var FinanceLegFrequency = PeriodFrequency.Monthly;
                Enum.TryParse(secMasterEquitySwap.FinanceLeg_Frequency, out FinanceLegFrequency);

                var rateType = RateType.Fixed;
                Enum.TryParse(secMasterEquitySwap.FinanceLeg_InterestRate.ToString(), out rateType);

                var dayCount = DayCount.ACT_30_360;
                Enum.TryParse(secMasterEquitySwap.FinanceLeg_DayCount.ToString(), out dayCount);

                var commisionType = CommisionType.Contract;
                Enum.TryParse(secMasterEquitySwap.FinanceLeg_DayCount.ToString(), out commisionType);

                EquitySwap.CommissionBasis = commisionType;
                EquitySwap.EquityLeg_BulletSwap = secMasterEquitySwap.EquityLeg_BulletSwap;
                EquitySwap.EquityLeg_ExcludeDividends = secMasterEquitySwap.EquityLeg_ExcludeDividends;
                EquitySwap.EquityLeg_ImpliedCommission = secMasterEquitySwap.EquityLeg_ImpliedCommission;
                EquitySwap.FinanceLeg_DayCount = dayCount;
                EquitySwap.FinanceLeg_FixedRate = secMasterEquitySwap.FinanceLeg_FixedRate;
                EquitySwap.FinanceLeg_Frequency = FinanceLegFrequency;
                EquitySwap.FinanceLeg_InterestRate = rateType;
                EquitySwap.FinanceLeg_SpreadBasisPoint = secMasterEquitySwap.FinanceLeg_SpreadBasisPoint;
                EquitySwap.HardCommissionRate = secMasterEquitySwap.HardCommissionRate;
                EquitySwap.SelectedEquityLeg_Frequency = periodFrequency;
                EquitySwap.SoftCommissionRate = secMasterEquitySwap.SoftCommissionRate;
                EquitySwap.EquityLeg_FirstPaymentDate = secMasterEquitySwap.EquityLeg_FirstPaymentDate;
                EquitySwap.EquityLeg_ExpirationDate = secMasterEquitySwap.EquityLeg_ExpirationDate;
                EquitySwap.FinanceLeg_FirstPaymentDate = secMasterEquitySwap.FinanceLeg_FirstPaymentDate;
                EquitySwap.FinanceLeg_FirstResetDate = secMasterEquitySwap.FinanceLeg_FirstResetDate;

                if (!isTradeView.Equals("Collapsed"))
                {
                    EquitySwap.EquityLeg_FirstPaymentDate = GetAdjustedDate(effectiveDate, EquitySwap.SelectedEquityLeg_Frequency);
                    EquitySwap.EquityLeg_ExpirationDate = GetAdjustedDate(effectiveDate, EquitySwap.SelectedEquityLeg_Frequency);
                    EquitySwap.FinanceLeg_FirstPaymentDate = GetAdjustedDate(effectiveDate, EquitySwap.FinanceLeg_Frequency);
                    EquitySwap.FinanceLeg_FirstResetDate = GetAdjustedDate(effectiveDate, EquitySwap.FinanceLeg_Frequency);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get Adjusted Date
        /// </summary>
        /// <param name="tradeDate"></param>
        /// <param name="daysToSettle"></param>
        /// <param name="periodFrequency"></param>
        /// <returns></returns>
        private DateTime GetAdjustedDate(DateTime effectiveDate, PeriodFrequency periodFrequency)
        {
            try
            {
                DateTime date = effectiveDate;

                switch (periodFrequency)
                {

                    case PeriodFrequency.Monthly:
                        effectiveDate = effectiveDate.AddMonths(1);
                        date = DateTime.Parse(BusinessDayCalculator.GetInstance().GetLastBusinessDateOfMonth(1, effectiveDate));
                        break;

                    case PeriodFrequency.Quarterly:
                        effectiveDate = effectiveDate.AddMonths(3);
                        date = DateTime.Parse(BusinessDayCalculator.GetInstance().GetLastBusinessDateOfQuarter(1, effectiveDate));
                        break;

                    case PeriodFrequency.Annually:
                        effectiveDate = effectiveDate.AddYears(1);
                        date = DateTime.Parse(BusinessDayCalculator.GetInstance().GetLastBusinessDateOfYear(1, effectiveDate));
                        break;

                }
                return date;
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal void SetTradeView(bool isTradeView)
        {
            if (!isTradeView)
                IsTradeView = "Collapsed";
            else
                IsTradeView = "Visible";

        }
    }
}
