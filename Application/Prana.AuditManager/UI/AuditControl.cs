using Infragistics.Win.Misc;
using Prana.AuditManager.Definitions.Data;
using Prana.AuditManager.Definitions.Enum;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana.AuditManager.UI
{
    internal partial class AuditControl : UserControl
    {
        /// <summary>
        /// Constructor to initialize components
        /// </summary>
        public AuditControl()
        {
            InitializeComponent();

            // PranaReleaseViewType release = CachedDataManager.GetInstance.GetPranaReleaseViewType();
            if (CustomThemeHelper.ApplyTheme)
            {
                this.ultraExpandableGroupBoxMain.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.ultraExpandableGroupBoxPanelAudit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.ForeColor = System.Drawing.Color.White;
            }
        }

        Object _lockerObject = new object();
        List<UltraLabel> _labelList = new List<UltraLabel>();
        List<AuditDataDefinition> _auditCache;

        /// <summary>
        /// Funtion to load data from list
        /// </summary>
        /// <param name="dataToLoad"></param>
        public void LoadData(List<AuditDataDefinition> dataToLoad)
        {

            ClearPanelList();
            LoadCache(dataToLoad);

        }

        /// <summary>
        /// Function to show audit data
        /// </summary>
        /// <param name="auditOn"></param>
        public void ShowDataFor(int auditOn)
        {
            ClearPanelList();
            CreateAndLoadPanel(auditOn);
        }

        /// <summary>
        /// Function to load auditCache list
        /// </summary>
        /// <param name="dataToLoad"></param>
        private void LoadCache(List<AuditDataDefinition> dataToLoad)
        {
            lock (_lockerObject)
            {
                this._auditCache = dataToLoad;
            }
        }

        /// <summary>
        /// Function to clear audit panel
        /// </summary>
        private void ClearPanelList()
        {
            lock (_lockerObject)
            {
                foreach (UltraLabel ctrlPnl in _labelList)
                {
                    ultraExpandableGroupBoxPanelAudit.Controls.Remove(ctrlPnl);
                }
                _labelList.Clear();
            }
        }

        /// <summary>
        /// Function to show audit data from auditCache list based on auditOn
        /// </summary>
        /// <param name="auditOn"></param>
        private void CreateAndLoadPanel(int auditDimensionValue)
        {
            try
            {
                lock (_lockerObject)
                {
                    UltraLabel uLabel;
                    if (_auditCache == null)
                        return;

                    var list = _auditCache.Where(audit => audit.AuditDimensionValue == auditDimensionValue).ToList();


                    int preLocationX = 20;
                    int preLocationY = 20;
                    if (list.Count() > 0)
                    {
                        foreach (AuditDataDefinition dataDefinition in list)
                        {
                            uLabel = GenerateNewLabel(dataDefinition, ref preLocationX, ref preLocationY);
                            _labelList.Add(uLabel);
                            ultraExpandableGroupBoxPanelAudit.Controls.Add(uLabel);
                        }
                    }
                    else
                    {
                        ClearPanelList();
                        var defaultList = _auditCache.GroupBy(ac => new
                        {
                            ac.Action
                        })
                                          .Select(ac => new AuditDataDefinition
                                          {
                                              Action = ac.Min(at => at.Action),
                                              ActualAuditTime = ac.Max(at => at.ActualAuditTime),
                                              UserId = ac.Where(at1 => at1.ActualAuditTime == ac.Max(at => at.ActualAuditTime)).Select(at => at.UserId).FirstOrDefault(),
                                              AuditDimensionValue = ac.Where(at1 => at1.ActualAuditTime == ac.Max(at => at.ActualAuditTime)).Select(at => at.AuditDimensionValue).FirstOrDefault()
                                          });

                        foreach (AuditDataDefinition dataDefinition in defaultList)
                        {
                            uLabel = GenerateDefaultLabel(dataDefinition, ref preLocationX, ref preLocationY);
                            _labelList.Add(uLabel);
                            ultraExpandableGroupBoxPanelAudit.Controls.Add(uLabel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Function to show audit data on specified location for selected item
        /// </summary>
        /// <param name="dataDefinition"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private UltraLabel GenerateNewLabel(AuditDataDefinition dataDefinition, ref int x, ref int y)
        {

            //x = x + 100;
            y = y + 15;
            //UltraPanel panel = new UltraPanel();
            UltraLabel ulbAudit = new UltraLabel();
            ulbAudit.Location = new Point(x, y);
            ulbAudit.AutoSize = true;
            //ulbAudit.MaximumSize = new System.Drawing.Size(250, 35);
            //ulbAudit.MinimumSize = new System.Drawing.Size(250, 35);

            //String userName = Cached
            //CommonDataCache.CachedDataManager.GetInstance.GetUserText(dataDefinition.UserId);
            String dimension = GetValueForKeyAndAction(dataDefinition.AuditDimensionValue, dataDefinition.Action);
            ulbAudit.Text = dimension + "  " + SplitActionString(dataDefinition.Action.ToString(), "ActionString") + " by " + CommonDataCache.CachedDataManager.GetInstance.GetUserText(dataDefinition.UserId) + " at " + dataDefinition.ActualAuditTime;

            //panel.ClientArea.Controls.Add(ulbAudit);

            ulbAudit.Margin = new Padding(30, 0, 0, 0);

            //panel.MaximumSize = new System.Drawing.Size(270, 50);
            //panel.MinimumSize = new System.Drawing.Size(270, 50);
            //ulbAudit.AutoSize = true;
            //ulbAudit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

            return ulbAudit;
        }

        /// <summary>
        /// JIRA: CHMW-385
        /// Function to show default audit data on specified location
        /// </summary>
        /// <param name="dataDefinition"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private UltraLabel GenerateDefaultLabel(AuditDataDefinition dataDefinition, ref int x, ref int y)
        {
            //x = x + 100;
            y = y + 15;
            //UltraPanel panel = new UltraPanel();
            UltraLabel ulbAudit = new UltraLabel();
            ulbAudit.Location = new Point(x, y);
            ulbAudit.AutoSize = true;

            String dimension = GetValueForKeyAndAction(dataDefinition.AuditDimensionValue, dataDefinition.Action);
            if (dataDefinition.Action.ToString().Contains("AUEC"))
            {
                ulbAudit.Text = "Last " + dataDefinition.Action.ToString() + " : " + dimension + " by " + CommonDataCache.CachedDataManager.GetInstance.GetUserText(dataDefinition.UserId) + " at " + dataDefinition.ActualAuditTime;
            }
            else
            {
                ulbAudit.Text = "Last " + SplitActionString(dataDefinition.Action.ToString(), "Default") + " : " + dimension + " by " + CommonDataCache.CachedDataManager.GetInstance.GetUserText(dataDefinition.UserId) + " at " + dataDefinition.ActualAuditTime;
            }
            ulbAudit.Margin = new Padding(30, 0, 0, 0);

            return ulbAudit;
        }

        /// <summary>
        ///  JIRA: CHMW-359
        /// Action string splitted to multiple string
        /// </summary>
        /// <param name="actionString"></param>
        /// <returns></returns>
        private String SplitActionString(String actionString, String LabelType)
        {
            #region Action string split
            String fin = "NA";
            try
            {
                String[] action = Regex.Replace(actionString, "([A-Z])", " $1", RegexOptions.Compiled).Trim().Split(' ');
                if (action.Length > 0)
                {
                    fin = action[0];
                    switch (LabelType)
                    {
                        case "ActionString":
                            for (int i = 1; i < action.Length; i++)
                            {
                                fin = action[i];
                            }
                            break;
                        case "Default":
                            for (int i = 1; i < action.Length; i++)
                            {
                                fin += " " + action[i].ToLower();
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return fin;

            #endregion
        }

        /// <summary>
        /// Returns value for key and action
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private String GetValueForKeyAndAction(int key, AuditAction action)
        {
            switch (action)
            {
                case AuditAction.UserCreated:
                case AuditAction.UserUpdated:
                case AuditAction.UserDeleted:
                case AuditAction.UserApproved:
                    return CommonDataCache.CachedDataManager.GetInstance.GetUserText(key);
                case AuditAction.BatchApproved:
                case AuditAction.BatchCreated:
                case AuditAction.BatchDeleted:
                case AuditAction.BatchUpdated:
                    return "";
                case AuditAction.CounterPartyVenueApproved:
                case AuditAction.CounterPartyVenueCreated:
                case AuditAction.CounterPartyVenueDeleted:
                case AuditAction.CounterPartyVenueUpdated:
                    return CommonDataCache.CachedDataManager.GetInstance.GetCounterPartyVenueText(key);
                case AuditAction.AccountApproved:
                case AuditAction.AccountCreated:
                case AuditAction.AccountDeleted:
                case AuditAction.AccountUpdated:
                    return CommonDataCache.CachedDataManager.GetInstance.GetAccountText(key);
                case AuditAction.AccountGroupApproved:
                case AuditAction.AccountGroupCreated:
                case AuditAction.AccountGroupDeleted:
                case AuditAction.AccountGroupUpdated:
                    return CommonDataCache.CachedDataManager.GetInstance.GetAccountGroups(key);
                case AuditAction.MasterFundCreated:
                case AuditAction.MasterFundUpdated:
                case AuditAction.MasterFundApproved:
                case AuditAction.MasterFundDeleted:
                    return CommonDataCache.CachedDataManager.GetInstance.GetMasterFund(key); // returns all master funds which have association with accounts
                case AuditAction.PricingRuleCreated:
                case AuditAction.PricingRuleUpdated:
                case AuditAction.PricingRuleDeleted:
                case AuditAction.PricingRuleApproved:
                    return "";
                case AuditAction.ClientCreated:
                case AuditAction.ClientUpdated:
                case AuditAction.ClientDeleted:
                case AuditAction.ClientApproved:
                    return CommonDataCache.CachedDataManager.GetCompanyText(key);
                case AuditAction.MasterStrategyCreated:
                case AuditAction.MasterStrategyUpdated:
                case AuditAction.MasterStrategyDeleted:
                case AuditAction.MasterStrategyApproved:
                    return CommonDataCache.CachedDataManager.GetInstance.GetMasterStrategy(key);
                case AuditAction.ThirdPartyCreated:
                case AuditAction.ThirdPartyUpdated:
                case AuditAction.ThirdPartyDeleted:
                case AuditAction.ThirdPartyApproved:
                    return CommonDataCache.CachedDataManager.GetInstance.GetThirdPartyNameByID(key);
                case AuditAction.AUECCreated:
                case AuditAction.AUECUpdated:
                case AuditAction.AUECDeleted:
                case AuditAction.AUECApproved:
                    return "";
                case AuditAction.StrategyCreated:
                case AuditAction.StrategyUpdated:
                case AuditAction.StrategyDeleted:
                case AuditAction.StrategyApproved:
                    return CommonDataCache.CachedDataManager.GetInstance.GetStrategyText(key);
                default:
                    return "NA";
            }
        }

    }
}
