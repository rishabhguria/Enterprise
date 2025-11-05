using System;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// The class represents pricing rule
    /// Each object of the class represents a part or complete pricing rule
    /// </summary>
    public class PricingRule
    {
        #region Properties
        public int RuleID { get; set; }
        public int AccountID { get; set; }
        public int AssetClassID { get; set; }
        public int ExchangeID { get; set; }
        public int PricingDataTypeID { get; set; }
        public int SourceID { get; set; }
        public int SecondarySourceID { get; set; }
        public int CompanyID { get; set; }
        public string SecondarySource { get; set; }
        public bool IsPricingPolicy { get; set; }
        public int PricingPolicyID { get; set; }
        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public PricingRule()
        {
            RuleID = int.MinValue;
            AccountID = int.MinValue;
            AssetClassID = int.MinValue;
            ExchangeID = int.MinValue;
            PricingDataTypeID = int.MinValue;
            SourceID = int.MinValue;
            SecondarySourceID = int.MinValue;
            CompanyID = int.MinValue;
        }

        /// <summary>
        /// <Method to Create the object and fill it with the data
        /// </summary>
        /// <param name="ruleID">ID of the rule</param>
        /// <param name="accountID">ID of the account</param>
        /// <param name="assetClassID">ID of the asset class</param>
        /// <param name="exchangeID">ID of the exchange</param>
        /// <param name="pricingTypeID">ID of the pricing type</param>
        /// <param name="sourceID">ID of the source</param>
        /// <param name="secSourceID">ID of the secondary source</param>
        /// <param name="companyID">ID of the company</param>
        /// <returns>The object with the details</returns>
        public PricingRule FillData(int ruleID, int accountID, int assetClassID, int exchangeID, int pricingTypeID, int sourceID, int secSourceID, int companyID, String SecondarySource)
        {
            //PricingRule objRule = new PricingRule();
            this.RuleID = ruleID;
            this.AccountID = accountID;
            this.AssetClassID = assetClassID;
            this.ExchangeID = exchangeID;
            this.PricingDataTypeID = pricingTypeID;
            this.SourceID = sourceID;
            this.SecondarySourceID = secSourceID;
            this.CompanyID = companyID;
            this.SecondarySource = SecondarySource;

            return this;
        }


    }
}
