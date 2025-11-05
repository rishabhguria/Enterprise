using Newtonsoft.Json;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class EquitySwapTradeData : OTCTradeData
    {
        #region Properties
        /// <summary>
        /// EquityLeg Frequency
        /// </summary>
        private string _EquityLeg_Frequency;
        public string EquityLeg_Frequency
        {
            get { return _EquityLeg_Frequency; }
            set { _EquityLeg_Frequency = value; }
        }

        /// <summary>
        /// EquityLeg BulletSwap
        /// </summary>
        private bool equityLeg_BulletSwap;
        public bool EquityLeg_BulletSwap
        {
            get { return equityLeg_BulletSwap; }
            set { equityLeg_BulletSwap = value; }
        }

        /// <summary>
        /// EquityLeg ExcludeDividends
        /// </summary>
        private bool equityLeg_ExcludeDividends;
        public bool EquityLeg_ExcludeDividends
        {
            get { return equityLeg_ExcludeDividends; }
            set { equityLeg_ExcludeDividends = value; }
        }

        /// <summary>
        /// EquityLeg ImpliedCommission
        /// </summary>
        private bool equityLeg_ImpliedCommission;
        public bool EquityLeg_ImpliedCommission
        {
            get { return equityLeg_ImpliedCommission; }
            set { equityLeg_ImpliedCommission = value; }
        }

        /// <summary>
        /// Commission Basis
        /// </summary>
        private string commissionBasis;
        public string CommissionBasis
        {
            get { return commissionBasis; }
            set { commissionBasis = value; }
        }

        /// <summary>
        /// Hard Commission Rate
        /// </summary>
        private double hardCommissionRate;
        public double HardCommissionRate
        {
            get { return hardCommissionRate; }
            set { hardCommissionRate = value; }
        }

        /// <summary>
        /// Soft Commission Rate
        /// </summary>
        private double softCommissionRate;
        public double SoftCommissionRate
        {
            get { return softCommissionRate; }
            set { softCommissionRate = value; }
        }

        /// <summary>
        /// FinanceLeg InterestRate
        /// </summary>
        private double financeLeg_InterestRate;
        public double FinanceLeg_InterestRate
        {
            get { return financeLeg_InterestRate; }
            set { financeLeg_InterestRate = value; }
        }

        /// <summary>
        /// FinanceLeg SpreadBasisPoint
        /// </summary>
        private double financeLeg_SpreadBasisPoint;
        public double FinanceLeg_SpreadBasisPoint
        {
            get { return financeLeg_SpreadBasisPoint; }
            set { financeLeg_SpreadBasisPoint = value; }
        }

        /// <summary>
        /// FinanceLeg FixedRate
        /// </summary>
        private double financeLeg_FixedRate;
        public double FinanceLeg_FixedRate
        {
            get { return financeLeg_FixedRate; }
            set { financeLeg_FixedRate = value; }
        }

        /// <summary>
        /// FinanceLeg Frequency
        /// </summary>
        private string financeLeg_Frequency;
        public string FinanceLeg_Frequency
        {
            get { return financeLeg_Frequency; }
            set { financeLeg_Frequency = value; }
        }

        /// <summary>
        /// FinanceLeg DayCount
        /// </summary>
        private int financeLeg_DayCount;
        public int FinanceLeg_DayCount
        {
            get { return financeLeg_DayCount; }
            set { financeLeg_DayCount = value; }
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


        private DateTime equityLeg_FirstPaymentDate = DateTimeConstants.MinValue;

        public DateTime EquityLeg_FirstPaymentDate
        {
            get { return equityLeg_FirstPaymentDate; }
            set { equityLeg_FirstPaymentDate = value; }
        }

        private DateTime equityLeg_ExpirationDate = DateTimeConstants.MinValue;

        public DateTime EquityLeg_ExpirationDate
        {
            get { return equityLeg_ExpirationDate; }
            set { equityLeg_ExpirationDate = value; }
        }

        private DateTime financeLeg_FirstResetDate = DateTimeConstants.MinValue;

        public DateTime FinanceLeg_FirstResetDate
        {
            get { return financeLeg_FirstResetDate; }
            set { financeLeg_FirstResetDate = value; }
        }

        private DateTime financeLeg_FirstPaymentDate = DateTimeConstants.MinValue;
        public DateTime FinanceLeg_FirstPaymentDate
        {
            get { return financeLeg_FirstPaymentDate; }
            set { financeLeg_FirstPaymentDate = value; }
        }

        private string customFieldJsonString;

        public string CustomFieldJsonString
        {
            get { return customFieldJsonString; }
            set { customFieldJsonString = value; }
        }

        #endregion

        /// <summary>
        /// Constructor EquitySwapTradeData
        /// </summary>
        public EquitySwapTradeData()
        {

        }

        /// <summary>
        /// Constructor EquitySwapTradeData
        /// </summary>
        public EquitySwapTradeData(string swapString)
        {
            try
            {
                string[] externList = swapString.Split(Seperators.SEPERATOR_5);
                InstrumentType = externList[0].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                DaysToSettle = int.Parse(externList[1].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                EffectiveDate = DateTime.Parse(externList[2].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                ISDACounterParty = int.Parse(externList[3].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                EquityLeg_Frequency = externList[4].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                EquityLeg_BulletSwap = Boolean.Parse(externList[5].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                EquityLeg_ImpliedCommission = Boolean.Parse(externList[6].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                EquityLeg_FirstPaymentDate = DateTime.Parse(externList[7].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                EquityLeg_ExpirationDate = DateTime.Parse(externList[8].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                CommissionBasis = externList[9].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                HardCommissionRate = double.Parse(externList[10].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                SoftCommissionRate = double.Parse(externList[11].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_InterestRate = double.Parse(externList[12].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_SpreadBasisPoint = double.Parse(externList[13].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_DayCount = int.Parse(externList[14].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_Frequency = externList[15].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                FinanceLeg_FixedRate = double.Parse(externList[16].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_FirstResetDate = DateTime.Parse(externList[17].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_FirstPaymentDate = DateTime.Parse(externList[18].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                TradeDate = DateTime.Parse(externList[19].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                Symbol = externList[20].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                UniqueIdentifier = externList[21].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                Description = externList[22].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                var customFieldString = externList[23].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                CustomFieldJsonString = customFieldString;
                CustomFields = !string.IsNullOrWhiteSpace(customFieldString) ? JsonConvert.DeserializeObject<List<OTCCustomFields>>(customFieldString) : new List<OTCCustomFields>();
                EquityLeg_ExcludeDividends = Boolean.Parse(externList[24].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw new Exception("swap string not in correct format", ex);
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

            paramsStr.Append(CustomFIXConstants.CUST_TAG_EquityLeg_Frequency);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.EquityLeg_Frequency.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_EquityLeg_BulletSwap);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.EquityLeg_BulletSwap.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_EquityLeg_ImpliedCommission);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.EquityLeg_ImpliedCommission.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_EquityLeg_FirstPaymentDate);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.EquityLeg_FirstPaymentDate.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_EquityLeg_ExpirationDate);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.EquityLeg_ExpirationDate.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_Commission_Basis);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.CommissionBasis.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_Commission_HardCommissionRate);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.HardCommissionRate.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_Commission_SoftCommissionRate);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.SoftCommissionRate.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_FinanceLeg_InterestRate);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.FinanceLeg_InterestRate.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_FinanceLeg_SpreadBasisPoint);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.FinanceLeg_SpreadBasisPoint.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_FinanceLeg_DayCount);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.FinanceLeg_DayCount.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_FinanceLeg_Frequency);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.FinanceLeg_Frequency.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_FinanceLeg_FixedRate);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.FinanceLeg_FixedRate.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_FinanceLeg_FirstResetDate);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.FinanceLeg_FirstResetDate.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_FinanceLeg_FirstPaymentDate);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.FinanceLeg_FirstPaymentDate.ToString());
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

            paramsStr.Append(CustomFIXConstants.CUST_TAG_EquityLeg_ExcludeDividends);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.EquityLeg_ExcludeDividends.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            return paramsStr.ToString();
        }

        public override OTCTradeData Clone()
        {
            try
            {
                EquitySwapTradeData clonedOTCParams = new EquitySwapTradeData()
                {
                    _EquityLeg_Frequency = this._EquityLeg_Frequency,
                    commissionBasis = this.commissionBasis,
                    customFields = this.customFields,
                    DaysToSettle = this.DaysToSettle,
                    Description = this.Description,
                    EffectiveDate = this.EffectiveDate,
                    equityLeg_BulletSwap = this.equityLeg_BulletSwap,
                    equityLeg_ExpirationDate = this.equityLeg_ExpirationDate,
                    equityLeg_FirstPaymentDate = this.equityLeg_FirstPaymentDate,
                    EquityLeg_Frequency = this.EquityLeg_Frequency,
                    equityLeg_ImpliedCommission = this.equityLeg_ImpliedCommission,
                    financeLeg_DayCount = this.financeLeg_DayCount,
                    financeLeg_FirstPaymentDate = this.financeLeg_FirstPaymentDate,
                    financeLeg_FirstResetDate = financeLeg_FirstResetDate,
                    financeLeg_FixedRate = this.financeLeg_FixedRate,
                    financeLeg_Frequency = this.financeLeg_Frequency,
                    financeLeg_InterestRate = this.financeLeg_InterestRate,
                    financeLeg_SpreadBasisPoint = this.financeLeg_SpreadBasisPoint,
                    GroupID = this.GroupID,
                    hardCommissionRate = this.hardCommissionRate,
                    InstrumentType = this.InstrumentType,
                    ISDAContract = this.ISDAContract,
                    ISDACounterParty = this.ISDACounterParty,
                    softCommissionRate = this.softCommissionRate,
                    Symbol = this.Symbol,
                    TradeDate = this.TradeDate,
                    UniqueIdentifier = this.UniqueIdentifier
                };

                return clonedOTCParams;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
