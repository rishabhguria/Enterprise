using Newtonsoft.Json;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Prana.BusinessObjects
{

    public class ConvertibleBondTradeData : OTCTradeData
    {
        #region Properties


        private int oTCTemplateID;
        public int OTCTemplateID
        {
            get { return oTCTemplateID; }
            set { oTCTemplateID = value; }
        }

        /// <summary>
        /// Collateral Margin
        /// </summary>
        private double _equityLeg_ConversionRatio;
        public double EquityLeg_ConversionRatio
        {
            get { return _equityLeg_ConversionRatio; }
            set { _equityLeg_ConversionRatio = value; }
        }
        /// <summary>
        /// Collateral Rate
        /// </summary>
        private bool _financeLeg_ZeroCoupon;
        public bool FinanceLeg_ZeroCoupon
        {
            get { return _financeLeg_ZeroCoupon; }
            set { _financeLeg_ZeroCoupon = value; }
        }

        /// <summary>
        /// Collateral DayCount
        /// </summary>
        private int _financeLeg_IRBenchMark;
        public int FinanceLeg_IRBenchMark
        {
            get { return _financeLeg_IRBenchMark; }
            set { _financeLeg_IRBenchMark = value; }
        }



        /// <summary>
        /// Fianace InteresrRatebenchmark
        /// </summary>
        private double _financeLeg_FXRate;
        public double FinanceLeg_FXRate
        {
            get { return _financeLeg_FXRate; }
            set { _financeLeg_FXRate = value; }
        }


        /// <summary>
        /// Fianace Fixedrate
        /// </summary>
        private double _financeLeg_SBPoint;
        public double FinanceLeg_SBPoint
        {
            get { return _financeLeg_SBPoint; }
            set { _financeLeg_SBPoint = value; }
        }


        /// <summary>
        /// fianace DayCount
        /// </summary>
        private int _financeLeg_DayCount;
        public int FinanceLeg_DayCount
        {
            get { return _financeLeg_DayCount; }
            set { _financeLeg_DayCount = value; }
        }


        /// <summary>
        /// fianace ScriptlendingFee
        /// </summary>
        private int _financeLeg_CouponFreq;
        public int FinanceLeg_CouponFreq
        {
            get { return _financeLeg_CouponFreq; }
            set { _financeLeg_CouponFreq = value; }
        }


        /// <summary>
        ///  Commissionbasis
        /// </summary>
        private int _commissionbasis;
        public int Commission_Basis
        {
            get { return _commissionbasis; }
            set { _commissionbasis = value; }
        }


        /// <summary>
        ///  HardCommRate
        /// </summary>
        private double _commission_HardCommRate;
        public double Commission_HardCommRate
        {
            get { return _commission_HardCommRate; }
            set { _commission_HardCommRate = value; }
        }


        /// <summary>
        /// fianace ScriptlendingFee
        /// </summary>
        private double _commission_SoftCommRate;
        public double Commission_SoftCommRate
        {
            get { return _commission_SoftCommRate; }
            set { _commission_SoftCommRate = value; }
        }



        /// <summary>
        /// EquityLeg Price
        /// </summary>
        private double _equityLeg_ConversionPrice;
        public double EquityLeg_ConversionPrice
        {
            get { return _equityLeg_ConversionPrice; }
            set
            {
                _equityLeg_ConversionPrice = value;
            }

        }


        /// <summary>
        /// Collateral Margin
        /// </summary>
        private DateTime _equityLeg_ConversionDate;
        public DateTime EquityLeg_ConversionDate
        {
            get { return _equityLeg_ConversionDate; }
            set
            {
                _equityLeg_ConversionDate = value;

            }
        }


        private DateTime _financeLeg_FirstPaymentDate = DateTimeConstants.MinValue;
        public DateTime FinanceLeg_FirstPaymentDate
        {
            get { return _financeLeg_FirstPaymentDate; }
            set
            {
                _financeLeg_FirstPaymentDate = value;
            }
        }



        private DateTime _financeLeg_FirstResetDate = DateTimeConstants.MinValue;
        public DateTime FinanceLeg_FirstResetDate
        {
            get { return _financeLeg_FirstResetDate; }
            set
            {
                _financeLeg_FirstResetDate = value;

            }
        }


        private string _financeLeg_ParValue = string.Empty;
        public string FinanceLeg_ParValue
        {
            get { return _financeLeg_ParValue; }
            set
            {
                _financeLeg_ParValue = value;
            }
        }
        /// <summary>
        /// CustomFields
        /// </summary>
        private List<OTCCustomFields> customFields = new List<OTCCustomFields>();
        [XmlIgnore]
        public List<OTCCustomFields> CustomFields
        {
            get { return customFields; }
            set { customFields = value; }
        }


        private string customFieldJsonString;

        public string CustomFieldJsonString
        {
            get { return customFieldJsonString; }
            set { customFieldJsonString = value; }
        }


        private string _sedol = string.Empty;
        public string Sedol
        {
            get { return _sedol; }
            set
            {
                _sedol = value;
            }
        }

        private string _isin = string.Empty;
        public string Isin
        {
            get { return _isin; }
            set
            {
                _isin = value;
            }
        }
        private string _cusip = string.Empty;
        public string Cusip
        {
            get { return _cusip; }
            set
            {
                _cusip = value;
            }
        }

        private string _currency = string.Empty;
        public string Currency
        {
            get { return _currency; }
            set
            {
                _currency = value;
            }
        }


        #endregion

        /// <summary>
        /// Constructor EquitySwapTradeData
        /// </summary>
        public ConvertibleBondTradeData()
        {

        }

        /// <summary>
        /// Constructor EquitySwapTradeData
        /// </summary>
        public ConvertibleBondTradeData(string swapString)
        {
            try
            {
                string[] externList = swapString.Split(Seperators.SEPERATOR_5);
                InstrumentType = externList[0].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                DaysToSettle = int.Parse(externList[1].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                EffectiveDate = DateTime.Parse(externList[2].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                ISDACounterParty = int.Parse(externList[3].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                TradeDate = DateTime.Parse(externList[4].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                Symbol = externList[5].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                UniqueIdentifier = externList[6].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                Description = externList[7].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                var customFieldString = externList[8].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                CustomFieldJsonString = customFieldString;
                CustomFields = !string.IsNullOrWhiteSpace(customFieldString) ? JsonConvert.DeserializeObject<List<OTCCustomFields>>(customFieldString) : new List<OTCCustomFields>();
                EquityLeg_ConversionPrice = double.Parse(externList[9].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                EquityLeg_ConversionRatio = double.Parse(externList[10].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                EquityLeg_ConversionDate = DateTime.Parse(externList[11].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_ZeroCoupon = Boolean.Parse(externList[12].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_IRBenchMark = int.Parse(externList[13].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_FXRate = double.Parse(externList[14].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_SBPoint = double.Parse(externList[15].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_DayCount = int.Parse(externList[16].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_CouponFreq = int.Parse(externList[17].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_FirstResetDate = DateTime.Parse(externList[18].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_FirstPaymentDate = DateTime.Parse(externList[19].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_ParValue = (externList[20].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                Commission_Basis = int.Parse(externList[21].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                Commission_HardCommRate = double.Parse(externList[22].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                Commission_SoftCommRate = double.Parse(externList[23].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                Sedol = (externList[24].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                Isin = (externList[25].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                Cusip = (externList[26].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                Currency = (externList[27].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw new Exception("Convertible Bond string not in correct format", ex);
                }
            }

        }



        /// <summary>
        /// To String
        /// </summary>
        public override string ToString()
        {
            StringBuilder paramsStr = new StringBuilder();

            paramsStr.Append(CustomFIXConstants.CUST_TAG_InstrumentTypeID);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.InstrumentType.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_DaysToSettle);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.DaysToSettle.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_EffectiveDate);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.EffectiveDate.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_ISDA_CounterParty);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.ISDACounterParty.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_TradeDate);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.TradeDate.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_Symbol);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.Symbol);
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_UniqueIdentifier);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.UniqueIdentifier);
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_Description);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.Description);
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_CustomFields);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(JsonConvert.SerializeObject(this.CustomFields));
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_EquityLeg_ConversionPrice);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.EquityLeg_ConversionPrice.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_EquityLeg_ConversionRatio);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.EquityLeg_ConversionRatio.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_EquityLeg_ConversionDate);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.EquityLeg_ConversionDate.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_FinanceLeg_ZeroCoupon);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.FinanceLeg_ZeroCoupon.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_FinanceLeg_IRBenchMark);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.FinanceLeg_IRBenchMark.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_FinanceLeg_FXRate);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.FinanceLeg_FXRate.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_FinanceLeg_SBPoint);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.FinanceLeg_SBPoint.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_FinanceLeg_DayCount);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.FinanceLeg_DayCount.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_FinanceLeg_CouponFreq);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.FinanceLeg_CouponFreq.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_FinanceLeg_FirstResetDate);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.FinanceLeg_FirstResetDate.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_FinanceLeg_FirstPaymentDate);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.FinanceLeg_FirstPaymentDate.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_FinanceLeg_ParValue);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.FinanceLeg_ParValue.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_Commission_Basis);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.Commission_Basis.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_Commission_HardCommissionRate);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.Commission_HardCommRate.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_Commission_SoftCommissionRate);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.Commission_SoftCommRate.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_Sedol);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.Sedol.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_Isin);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.Isin.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_Cusip);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.Cusip.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_Currency);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.Currency.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);


            return paramsStr.ToString();
        }

        public override OTCTradeData Clone()
        {
            try
            {
                ConvertibleBondTradeData clonedConvertibleBondParams = new ConvertibleBondTradeData()
                {
                    customFields = this.customFields,
                    DaysToSettle = this.DaysToSettle,
                    Description = this.Description,
                    EffectiveDate = this.EffectiveDate,
                    GroupID = this.GroupID,
                    InstrumentType = this.InstrumentType,
                    ISDAContract = this.ISDAContract,
                    ISDACounterParty = this.ISDACounterParty,
                    Symbol = this.Symbol,
                    TradeDate = this.TradeDate,
                    UniqueIdentifier = this.UniqueIdentifier,
                    _equityLeg_ConversionPrice = this._equityLeg_ConversionPrice,
                    _equityLeg_ConversionRatio = this._equityLeg_ConversionRatio,
                    _equityLeg_ConversionDate = this._equityLeg_ConversionDate,
                    _financeLeg_ZeroCoupon = this._financeLeg_ZeroCoupon,
                    _financeLeg_IRBenchMark = this._financeLeg_IRBenchMark,
                    _financeLeg_FXRate = this._financeLeg_FXRate,
                    _financeLeg_DayCount = this._financeLeg_DayCount,
                    _financeLeg_SBPoint = this._financeLeg_SBPoint,
                    _financeLeg_CouponFreq = this._financeLeg_CouponFreq,
                    _financeLeg_ParValue = this._financeLeg_ParValue,
                    _financeLeg_FirstPaymentDate = this._financeLeg_FirstPaymentDate,
                    _financeLeg_FirstResetDate = this._financeLeg_FirstResetDate,
                    _commissionbasis = this._commissionbasis,
                    _commission_HardCommRate = this.Commission_HardCommRate,
                    _commission_SoftCommRate = this.Commission_SoftCommRate,
                    _sedol = this._sedol,
                    _isin = this.Isin,
                    _cusip = this._cusip,
                    _currency = this._currency,
                };

                return clonedConvertibleBondParams;
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
