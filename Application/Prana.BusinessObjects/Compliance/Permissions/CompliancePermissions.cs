using Prana.BusinessObjects.Compliance.Enums;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.BusinessObjects.Compliance.Permissions
{
    public class CompliancePermissions
    {
        /// <summary>
        /// Gets or sets the company identifier.
        /// </summary>
        /// <value>
        /// The company identifier.
        /// </value>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the company user identifier.
        /// </summary>
        /// <value>
        /// The company user identifier.
        /// </value>
        public int CompanyUserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is power user.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is power user; otherwise, <c>false</c>.
        /// </value>
        public bool IsPowerUser { get; set; }

        /// <summary>
        /// The compliance UI permissions
        /// </summary>
        public Dictionary<RuleType, ComplianceUIPermissions> complianceUIPermissions = new Dictionary<RuleType, ComplianceUIPermissions>();

        /// <summary>
        /// Gets or sets the rule check permission.
        /// </summary>
        /// <value>
        /// The rule check permission.
        /// </value>
        public RuleCheckPermissions RuleCheckPermission { get; set; }

        /// <summary>
        /// The rule level permission
        /// </summary>
        public List<RuleLevelPermission> RuleLevelPermission = new List<RuleLevelPermission>();

        /// <summary>
        /// Gets or sets the pre rule level permissions.
        /// </summary>
        /// <value>
        /// The pre rule level permissions.
        /// </value>
        public DataTable PreRuleLevelPermissions { get; set; }

        /// <summary>
        /// Gets or sets the post rule level permissions.
        /// </summary>
        /// <value>
        /// The post rule level permissions.
        /// </value>
        public DataTable PostRuleLevelPermissions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating enable Basket Compliance check.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is Enable Basket Compliance Check; otherwise, <c>false</c>.
        /// </value>
        public bool EnableBasketComplianceCheck { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompliancePermissions"/> class.
        /// </summary>
        public CompliancePermissions()
        {
            this.PreRuleLevelPermissions = new DataTable();
            this.RuleCheckPermission = new RuleCheckPermissions();
            this.PostRuleLevelPermissions = new DataTable();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompliancePermissions"/> class.
        /// </summary>
        /// <param name="dr">The dr.</param>
        public CompliancePermissions(DataRow dr)
        {
            try
            {
                this.CompanyId = Convert.ToInt32(dr["CompanyId"]);
                this.CompanyUserId = Convert.ToInt32(dr["UserId"]);
                this.IsPowerUser = Convert.ToBoolean(dr["PowerUser"]);
                this.EnableBasketComplianceCheck = Convert.ToBoolean(dr["EnableBasketComplianceCheck"]);

                this.PreRuleLevelPermissions = new DataTable();
                this.RuleCheckPermission = new RuleCheckPermissions();
                this.PostRuleLevelPermissions = new DataTable();

                this.RuleCheckPermission.IsApplyToManual = Convert.ToBoolean(dr["IsApplyToManual"]);
                this.RuleCheckPermission.IsOverridePermission = Convert.ToBoolean(dr["IsOverridePermission"]);
                this.RuleCheckPermission.IsPreTradeEnabled = Convert.ToBoolean(dr["IsPreTradeCheckEnabled"]);
                this.RuleCheckPermission.IsStaging = Convert.ToBoolean(dr["IsStaging"]);
                this.RuleCheckPermission.IsTrading = Convert.ToBoolean(dr["IsTrading"]);
                this.RuleCheckPermission.DefaultOverRideType = (RuleOverrideType)Enum.Parse(typeof(RuleOverrideType), dr["DefaultRuleOverrideType"].ToString());
                this.RuleCheckPermission.DefaultPrePopUpEnabled = Convert.ToBoolean(dr["DefaultPrePopUp"]);
                this.RuleCheckPermission.DefaultPostPopUpEnabled = Convert.ToBoolean(dr["DefaultPostPopUp"]);

                if (!this.complianceUIPermissions.ContainsKey(RuleType.PreTrade))
                    this.complianceUIPermissions.Add(RuleType.PreTrade, new ComplianceUIPermissions());

                this.complianceUIPermissions[RuleType.PreTrade].IsCreate = Convert.ToBoolean(dr["PreTradeIsCreate"]);
                this.complianceUIPermissions[RuleType.PreTrade].IsRename = Convert.ToBoolean(dr["PreTradeIsRename"]);
                this.complianceUIPermissions[RuleType.PreTrade].IsExport = Convert.ToBoolean(dr["PreTradeIsExport"]);
                this.complianceUIPermissions[RuleType.PreTrade].IsEnable = Convert.ToBoolean(dr["PreTradeIsEnable"]);
                this.complianceUIPermissions[RuleType.PreTrade].IsDelete = Convert.ToBoolean(dr["PreTradeIsDelete"]);
                this.complianceUIPermissions[RuleType.PreTrade].IsImport = Convert.ToBoolean(dr["PreTradeIsImport"]);


                if (!this.complianceUIPermissions.ContainsKey(RuleType.PostTrade))
                    this.complianceUIPermissions.Add(RuleType.PostTrade, new ComplianceUIPermissions());

                this.complianceUIPermissions[RuleType.PostTrade].IsCreate = Convert.ToBoolean(dr["PostTradeIsCreate"]);
                this.complianceUIPermissions[RuleType.PostTrade].IsRename = Convert.ToBoolean(dr["PostTradeIsRename"]);
                this.complianceUIPermissions[RuleType.PostTrade].IsExport = Convert.ToBoolean(dr["PostTradeIsExport"]);
                this.complianceUIPermissions[RuleType.PostTrade].IsEnable = Convert.ToBoolean(dr["PostTradeIsEnable"]);
                this.complianceUIPermissions[RuleType.PostTrade].IsDelete = Convert.ToBoolean(dr["PostTradeIsDelete"]);
                this.complianceUIPermissions[RuleType.PostTrade].IsImport = Convert.ToBoolean(dr["PostTradeIsImport"]);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
