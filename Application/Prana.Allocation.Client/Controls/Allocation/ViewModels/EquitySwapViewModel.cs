
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.SM.OTC;
using Prana.Utilities.DateTimeUtilities;
using System;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Allocation.ViewModels
{
    //public enum PeriodFrequency
    //{
    //    Monthly = 1,
    //    Quarterly = 2,
    //    Semi_Annually = 3,
    //    Annually = 4,
    //    AT_Maturity = 5
    //}

    ///// <summary>
    ///// Rate Type
    ///// </summary>
    //public enum RateType
    //{
    //    LIBOR_30_Day = 1,
    //    LIBOR_3_Month = 2,
    //    LIBOR_6_Month = 3,
    //    LIBOR_12_Month = 4,
    //    Safex_Overnight = 5,
    //    Fixed = 6
    //}

    ///// <summary>
    ///// Commision Type
    ///// </summary>
    //public enum CommisionType
    //{
    //    Shares = 1,
    //    Notional = 2,
    //    Contract = 3,
    //    FlatAmount = 4

    //}

    ///// <summary>
    ///// Commision Type
    ///// </summary>
    //public enum DayCount
    //{
    //    ACT_30_360 = 1,
    //    ACT_ACT = 2,
    //    ACT_360 = 3,
    //    ACT_30_365 = 4,
    //    ACT_365 = 5
    //}



    //public enum DataTypes
    //{
    //    String = 1,
    //    Numeric = 2,
    //    Date = 3,
    //    Bool = 4
    //}



    //public enum InstrumentType
    //{
    //    EquitySwap = 1,
    //    CFD = 2
    //}


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

        private Visibility isTradeView = Visibility.Visible;
        public Visibility IsTradeView
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


        internal void SetTradeView(bool isTradeView)
        {
            if (!isTradeView)
                IsTradeView = Visibility.Collapsed;
            else
                IsTradeView = Visibility.Visible;
        }

        internal void SetData(EquitySwapTradeData equitySwapData)
        {
            try
            {

                var periodFrequency = PeriodFrequency.Monthly;
                Enum.TryParse(equitySwapData.InstrumentType, out periodFrequency);

                var FinanceLegFrequency = PeriodFrequency.Monthly;
                Enum.TryParse(equitySwapData.FinanceLeg_Frequency, out FinanceLegFrequency);

                var rateType = RateType.Fixed;
                Enum.TryParse(equitySwapData.FinanceLeg_InterestRate.ToString(), out rateType);

                var dayCount = DayCount.ACT_30_360;
                Enum.TryParse(equitySwapData.FinanceLeg_DayCount.ToString(), out dayCount);

                var commisionType = CommisionType.Contract;
                Enum.TryParse(equitySwapData.FinanceLeg_DayCount.ToString(), out commisionType);

                EquitySwap.CommissionBasis = commisionType;
                EquitySwap.EquityLeg_BulletSwap = equitySwapData.EquityLeg_BulletSwap;
                EquitySwap.EquityLeg_ExcludeDividends = equitySwapData.EquityLeg_ExcludeDividends;
                EquitySwap.EquityLeg_ImpliedCommission = equitySwapData.EquityLeg_ImpliedCommission;
                EquitySwap.FinanceLeg_DayCount = dayCount;
                EquitySwap.FinanceLeg_FixedRate = equitySwapData.FinanceLeg_FixedRate;
                EquitySwap.FinanceLeg_Frequency = FinanceLegFrequency;
                EquitySwap.FinanceLeg_InterestRate = rateType;
                EquitySwap.FinanceLeg_SpreadBasisPoint = equitySwapData.FinanceLeg_SpreadBasisPoint;
                EquitySwap.HardCommissionRate = equitySwapData.HardCommissionRate;
                EquitySwap.SelectedEquityLeg_Frequency = periodFrequency;
                EquitySwap.SoftCommissionRate = equitySwapData.SoftCommissionRate;
                EquitySwap.EquityLeg_FirstPaymentDate = equitySwapData.EquityLeg_FirstPaymentDate;
                EquitySwap.EquityLeg_ExpirationDate = equitySwapData.EquityLeg_ExpirationDate;
                EquitySwap.FinanceLeg_FirstPaymentDate = equitySwapData.FinanceLeg_FirstPaymentDate;
                EquitySwap.FinanceLeg_FirstResetDate = equitySwapData.FinanceLeg_FirstResetDate;

                if (isTradeView.Equals(Visibility.Visible))
                {
                    EquitySwap.EquityLeg_FirstPaymentDate = equitySwapData.EquityLeg_FirstPaymentDate;
                    EquitySwap.EquityLeg_ExpirationDate = equitySwapData.EquityLeg_ExpirationDate;
                    EquitySwap.FinanceLeg_FirstPaymentDate = equitySwapData.FinanceLeg_FirstPaymentDate;
                    EquitySwap.FinanceLeg_FirstResetDate = equitySwapData.FinanceLeg_FirstResetDate;
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
                        date = DateTime.Parse(BusinessDayCalculator.GetInstance().GetLastBusinessDateOfMonth(1, effectiveDate));
                        break;

                    case PeriodFrequency.Quarterly:
                        date = DateTime.Parse(BusinessDayCalculator.GetInstance().GetLastBusinessDateOfQuarter(1, effectiveDate));
                        break;

                    case PeriodFrequency.Annually:
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
    }
}
