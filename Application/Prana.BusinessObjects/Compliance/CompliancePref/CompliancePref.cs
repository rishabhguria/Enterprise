using System;

namespace Prana.BusinessObjects.Compliance.CompliancePref
{
    /// <summary>
    /// Contains all compliance preferences.
    /// Set from Admin
    /// currently only 2 properties
    /// cross import of rules and export path.
    /// </summary>
    public class CompliancePref
    {
        /// <summary>
        /// The pre post cross import allowed
        /// </summary>
        Boolean prePostCrossImportAllowed;

        /// <summary>
        /// Gets or sets a value indicating whether [pre post cross import allowed].
        /// </summary>
        /// <value>
        /// <c>true</c> if [pre post cross import allowed]; otherwise, <c>false</c>.
        /// </value>
        public Boolean PrePostCrossImportAllowed
        {
            get { return prePostCrossImportAllowed; }
            set { prePostCrossImportAllowed = value; }
        }

        /// <summary>
        /// The in market
        /// </summary>
        Boolean inMarket;

        /// <summary>
        /// Gets or sets a value indicating whether [in market].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [in market]; otherwise, <c>false</c>.
        /// </value>
        public Boolean InMarket
        {
            get { return inMarket; }
            set { inMarket = value; }
        }

        /// <summary>
        /// The in stage
        /// </summary>
        Boolean inStage;

        /// <summary>
        /// Gets or sets a value indicating whether [in stage].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [in stage]; otherwise, <c>false</c>.
        /// </value>
        public Boolean InStage
        {
            get { return inStage; }
            set { inStage = value; }
        }

        /// <summary>
        /// The import export path
        /// </summary>
        String importExportPath;

        /// <summary>
        /// Gets or sets the import export path.
        /// </summary>
        /// <value>
        /// The import export path.
        /// </value>
        public String ImportExportPath
        {
            get { return importExportPath; }
            set { importExportPath = value; }
        }

        /// <summary>
        /// The _post in market
        /// </summary>
        Boolean _postInMarket;

        /// <summary>
        /// Gets or sets a value indicating whether [post in market].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [post in market]; otherwise, <c>false</c>.
        /// </value>
        public Boolean PostInMarket
        {
            get { return _postInMarket; }
            set { _postInMarket = value; }
        }

        /// <summary>
        /// The _post in stage
        /// </summary>
        Boolean _postInStage;

        /// <summary>
        /// Gets or sets a value indicating whether [post in stage].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [post in stage]; otherwise, <c>false</c>.
        /// </value>
        public Boolean PostInStage
        {
            get { return _postInStage; }
            set { _postInStage = value; }
        }

        /// <summary>
        /// Gets or sets the post with in market in stage.
        /// </summary>
        /// <value>
        /// The post with in market in stage.
        /// </value>
        public int PostWithInMarketInStage
        {
            get
            {
                if (_postInStage && _postInMarket)
                    return 3;
                else if (_postInMarket)
                    return 2;
                else
                    return 1;
            }
        }

        /// <summary>
        /// Is Basket Compliance Enabled in Company
        /// </summary>
        private bool _isBasketComplianceEnabledCompany;

        /// <summary>
        /// Gets or sets a value indicating whether [Is Basket Compliance Enabled in Company].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [Is Basket Compliance Enabled in Company]; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsBasketComplianceEnabledCompany
        {
            get { return _isBasketComplianceEnabledCompany; }
            set { _isBasketComplianceEnabledCompany = value; }
        }

        /// <summary>
        /// Gets or sets block trades on compliance faliure.
        /// </summary>
        /// <value>
        /// true or false
        /// </value>
        private bool _blockTradeOnComplianceFaliure = true;

        public Boolean BlockTradeOnComplianceFaliure
        {
            get { return _blockTradeOnComplianceFaliure; }
            set { _blockTradeOnComplianceFaliure = value; }
        }


        /// <summary>
        /// The _post in stage
        /// </summary>
        Boolean _stageValueFromField;

        /// <summary>
        /// Gets or sets a value indicating whether [post in stage].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [post in stage]; otherwise, <c>false</c>.
        /// </value>
        public Boolean StageValueFromField
        {
            get { return _stageValueFromField; }
            set { _stageValueFromField = value; }
        }


        /// <summary>
        /// The _post in stage
        /// </summary>
        string _stageValueFromFieldString;

        /// <summary>
        /// Gets or sets a value indicating whether [post in stage].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [post in stage]; otherwise, <c>false</c>.
        /// </value>
        public string StageValueFromFieldString
        {
            get { return _stageValueFromFieldString; }
            set { _stageValueFromFieldString = value; }
        }

    }

}
