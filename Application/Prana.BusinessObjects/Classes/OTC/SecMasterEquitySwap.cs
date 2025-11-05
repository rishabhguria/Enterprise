using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.BusinessObjects
{
    public class SecMasterEquitySwap : SecMasterOTCData
    {
        public SecMasterEquitySwap()
        {

        }

        public int EquitySwapId { get; set; }
        /// <summary>
        /// OTCTemplateID
        /// </summary>
        private int oTCTemplateID;
        public int OTCTemplateID
        {
            get { return oTCTemplateID; }
            set { oTCTemplateID = value; }
        }

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


        public SecMasterEquitySwap(string swapString)
            : this()
        {
            try
            {
                string[] externList = swapString.Split(Seperators.SEPERATOR_5);
                DaysToSettle = int.Parse(externList[0].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                EffectiveDate = DateTime.Parse(externList[1].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                ISDACounterParty = int.Parse(externList[2].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                EquityLeg_Frequency = externList[3].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                EquityLeg_BulletSwap = Boolean.Parse(externList[4].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                EquityLeg_ImpliedCommission = Boolean.Parse(externList[5].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                EquityLeg_FirstPaymentDate = DateTime.Parse(externList[6].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                EquityLeg_ExpirationDate = DateTime.Parse(externList[7].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                CommissionBasis = externList[8].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                HardCommissionRate = double.Parse(externList[9].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                SoftCommissionRate = double.Parse(externList[10].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_InterestRate = double.Parse(externList[11].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_SpreadBasisPoint = double.Parse(externList[12].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_DayCount = int.Parse(externList[13].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_Frequency = externList[14].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                FinanceLeg_FixedRate = double.Parse(externList[15].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_FirstResetDate = DateTime.Parse(externList[16].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                FinanceLeg_FirstPaymentDate = DateTime.Parse(externList[17].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                //CustomFields = DateTime.Parse(externList[11].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
                EquityLeg_ExcludeDividends = Boolean.Parse(externList[18].Split(Seperators.SEPERATOR_6).GetValue(1).ToString());
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

        public override string ToString()
        {
            StringBuilder paramsStr = new StringBuilder();


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

            paramsStr.Append(CustomFIXConstants.CUST_TAG_CustomFields);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.CustomFields.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            paramsStr.Append(CustomFIXConstants.CUST_TAG_EquityLeg_ExcludeDividends);
            paramsStr.Append(Seperators.SEPERATOR_6);
            paramsStr.Append(this.EquityLeg_ExcludeDividends.ToString());
            paramsStr.Append(Seperators.SEPERATOR_5);

            return paramsStr.ToString();
        }


    }
}
