using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.SM.OTC;
using Prana.Utilities.DateTimeUtilities;
using System;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Allocation.ViewModels
{
    public class ConvertibleBondViewModel : BindableBase
    {

        private SecMasterConvertibleBondModel _ConvertibleBondView;
        public SecMasterConvertibleBondModel ConvertibleBondView
        {
            get { return _ConvertibleBondView; }
            set
            {
                _ConvertibleBondView = value;
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
                if (value.Equals("Visible"))
                { ConvertibleBondView.IsTradeView = Visibility.Visible; }
                else
                    ConvertibleBondView.IsTradeView = Visibility.Collapsed;
                OnPropertyChanged("IsTradeView");


            }
        }
        public ConvertibleBondViewModel()
        {
            ConvertibleBondView = new SecMasterConvertibleBondModel();
        }


        internal void SetData(ConvertibleBondTradeData secMasterOTCData)
        {
            try
            {
                var finance_DayCount = DayCount.ACT_30_360;
                Enum.TryParse(secMasterOTCData.FinanceLeg_DayCount.ToString(), out finance_DayCount);

                var finance_Commisionbais = CommisionType.Contract;
                Enum.TryParse(secMasterOTCData.Commission_Basis.ToString(), out finance_Commisionbais);

                var finance_interesrRatebenchmark = PeriodFrequency.Monthly;
                Enum.TryParse(secMasterOTCData.FinanceLeg_IRBenchMark.ToString(), out finance_interesrRatebenchmark);

                var finance_CouponFreq = PeriodFrequency.Monthly;
                Enum.TryParse(secMasterOTCData.FinanceLeg_CouponFreq.ToString(), out finance_CouponFreq);

                ConvertibleBondView.EquityLeg_ConversionRatio = secMasterOTCData.EquityLeg_ConversionRatio;
                ConvertibleBondView.FinanceLeg_ZeroCoupon = secMasterOTCData.FinanceLeg_ZeroCoupon;
                ConvertibleBondView.FinanceLeg_IRBenchMark = finance_interesrRatebenchmark;
                ConvertibleBondView.FinanceLeg_FXRate = secMasterOTCData.FinanceLeg_FXRate;
                ConvertibleBondView.FinanceLeg_SBPoint = secMasterOTCData.FinanceLeg_SBPoint;
                ConvertibleBondView.FinanceLeg_DayCount = finance_DayCount;
                ConvertibleBondView.FinanceLeg_CouponFreq = finance_CouponFreq;
                ConvertibleBondView.Commission_Basis = finance_Commisionbais;
                ConvertibleBondView.Commission_HardCommRate = secMasterOTCData.Commission_HardCommRate;
                ConvertibleBondView.Commission_SoftCommRate = secMasterOTCData.Commission_SoftCommRate;

                ConvertibleBondView.EquityLeg_ConversionDate = secMasterOTCData.EquityLeg_ConversionDate;
                ConvertibleBondView.EquityLeg_ConversionPrice = secMasterOTCData.EquityLeg_ConversionPrice;
                ConvertibleBondView.FinanceLeg_FirstResetDate = secMasterOTCData.FinanceLeg_FirstResetDate;
                ConvertibleBondView.FinanceLeg_FirstPaymentDate = secMasterOTCData.FinanceLeg_FirstPaymentDate;
                ConvertibleBondView.FinanceLeg_ParValue = secMasterOTCData.FinanceLeg_ParValue;


                //if (!isTradeView.Equals("Collapsed"))
                //{
                //    ConvertibleBondView.FinanceLeg_FirstResetDate = GetAdjustedDate(effectiveDate, ConvertibleBondView.FinanceLeg_CouponFreq);
                //    ConvertibleBondView.FinanceLeg_FirstPaymentDate = GetAdjustedDate(effectiveDate, ConvertibleBondView.FinanceLeg_CouponFreq);
                //}

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

    }
}
