using Newtonsoft.Json;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Prana.BusinessObjects
{
    public class CFDTradeData : OTCTradeData
    {

        /// <summary>
        /// Collateral Margin
        /// </summary>
        private double collateral_Margin;
        public double Collateral_Margin
        {
            get { return collateral_Margin; }
            set { collateral_Margin = value; }
        }
        /// <summary>
        /// Collateral Rate
        /// </summary>
        private double collateral_Rate;
        public double Collateral_Rate
        {
            get { return collateral_Rate; }
            set { collateral_Rate = value; }
        }

        /// <summary>
        /// Collateral DayCount
        /// </summary>
        private int collateral_DayCount;
        public int Collateral_DayCount
        {
            get { return collateral_DayCount; }
            set { collateral_DayCount = value; }
        }

        /// <summary>
        /// Fianace InteresrRatebenchmark
        /// </summary>
        private int financeLeg_InteresrRate;
        public int FinanceLeg_InterestRate
        {
            get { return financeLeg_InteresrRate; }
            set { financeLeg_InteresrRate = value; }
        }



        /// <summary>
        /// Fianace InteresrRatebenchmark
        /// </summary>
        private double financeLeg_FixedRate;
        public double FinanceLeg_FixedRate
        {
            get { return financeLeg_FixedRate; }
            set { financeLeg_FixedRate = value; }
        }


        /// <summary>
        /// Fianace Fixedrate
        /// </summary>
        private double financeLeg_SpreadBasisPoint;
        public double FinanceLeg_SpreadBasisPoint
        {
            get { return financeLeg_SpreadBasisPoint; }
            set { financeLeg_SpreadBasisPoint = value; }
        }


        /// <summary>
        /// fianace DayCount
        /// </summary>
        private int financeLeg_DayCount;
        public int FinanceLeg_DayCount
        {
            get { return financeLeg_DayCount; }
            set { financeLeg_DayCount = value; }
        }


        /// <summary>
        /// fianace ScriptlendingFee
        /// </summary>
        private float financeLeg_ScriptlendingFee;
        public float FinanceLeg_ScriptlendingFee
        {
            get { return financeLeg_ScriptlendingFee; }
            set { financeLeg_ScriptlendingFee = value; }
        }


        /// <summary>
        /// CFD Commissionbasis
        /// </summary>
        private string commissionBasis;
        public string CommissionBasis
        {
            get { return commissionBasis; }
            set { commissionBasis = value; }
        }


        /// <summary>
        /// CFD HardCommRate
        /// </summary>
        private double hardCommissionRate;
        public double HardCommissionRate
        {
            get { return hardCommissionRate; }
            set { hardCommissionRate = value; }
        }


        /// <summary>
        /// fianace ScriptlendingFee
        /// </summary>
        private double softCommissionRate;
        public double SoftCommissionRate
        {
            get { return softCommissionRate; }
            set { softCommissionRate = value; }
        }

        private string customFieldJsonString;

        public string CustomFieldJsonString
        {
            get { return customFieldJsonString; }
            set { customFieldJsonString = value; }
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




        public CFDTradeData()
        {
        }

        public CFDTradeData(string swapString)
        {
            try
            {
                string[] externList = swapString.Split(Seperators.SEPERATOR_5);
                InstrumentType = externList[0].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                DaysToSettle = int.Parse(externList[1].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                EffectiveDate = DateTime.Parse(externList[2].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                ISDACounterParty = int.Parse(externList[3].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                CommissionBasis = externList[4].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                HardCommissionRate = float.Parse(externList[5].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                SoftCommissionRate = float.Parse(externList[6].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_InterestRate = int.Parse(externList[7].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_SpreadBasisPoint = float.Parse(externList[8].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_DayCount = int.Parse(externList[9].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_FixedRate = float.Parse(externList[10].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                collateral_DayCount = int.Parse(externList[11].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                collateral_Margin = float.Parse(externList[12].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                collateral_Rate = float.Parse(externList[13].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                TradeDate = DateTime.Parse(externList[14].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                Symbol = externList[15].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                UniqueIdentifier = externList[16].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                Description = externList[17].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                var customFieldString = externList[18].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                CustomFieldJsonString = customFieldString;
                CustomFields = !string.IsNullOrWhiteSpace(customFieldString) ? JsonConvert.DeserializeObject<List<OTCCustomFields>>(customFieldString) : new List<OTCCustomFields>();

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

            paramsStr.Append(CustomFIXConstants.CUST_TAG_FinanceLeg_FixedRate);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.FinanceLeg_FixedRate.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_Collateral_DayCount);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.Collateral_DayCount.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_Collateral_Margin);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.Collateral_Margin.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_Collateral_Rate);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.Collateral_Rate.ToString());
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


            return paramsStr.ToString();
        }

        public override OTCTradeData Clone()
        {
            try
            {
                CFDTradeData clonedOTCParams = new CFDTradeData()
                {

                    customFields = this.customFields,
                    DaysToSettle = this.DaysToSettle,
                    Description = this.Description,
                    EffectiveDate = this.EffectiveDate,
                    collateral_DayCount = this.collateral_DayCount,
                    collateral_Margin = this.collateral_Margin,
                    collateral_Rate = this.collateral_Rate,
                    financeLeg_DayCount = this.financeLeg_DayCount,
                    financeLeg_SpreadBasisPoint = this.financeLeg_SpreadBasisPoint,
                    financeLeg_FixedRate = this.financeLeg_FixedRate,
                    financeLeg_ScriptlendingFee = this.financeLeg_ScriptlendingFee,
                    GroupID = this.GroupID,
                    commissionBasis = this.commissionBasis,
                    hardCommissionRate = this.hardCommissionRate,
                    softCommissionRate = this.softCommissionRate,
                    InstrumentType = this.InstrumentType,
                    ISDAContract = this.ISDAContract,
                    ISDACounterParty = this.ISDACounterParty,
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
