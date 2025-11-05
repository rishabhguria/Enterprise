using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Castle.Windsor;
using Prana.ReportingServices;
using Prana.AutomationHandlers;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.Utilities.UIUtilities;
using Infragistics.Win;
using Prana.BusinessObjects.AppConstants;
using Prana.Utilities.XMLUtilities;

namespace Prana.ReportingServer
{
    public partial class frmAdmin : Form
    {
        IWindsorContainer _container; ReportingServer reportingServerForm ;
        internal void SetContainer(IWindsorContainer container)
        {
            _container = container;
        }
        PranaReportingServices _pranaReportingServices;

        public frmAdmin()
        {
            try
            {
                InitializeComponent();

                //if (reportingServerForm == null && _container != null)
                //    reportingServerForm = (ReportingServer)_container[typeof(ReportingServer)];
                if (_pranaReportingServices == null)
                    _pranaReportingServices = new PranaReportingServices();
                if (dicClientThirdParty == null)
                    initializeDictionary();

                displayCronExpression();
                btnSaveInputSettings.Enabled = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        

        #region Genral Section

        private void btnCreateStructure_Click(object sender, EventArgs e)
        {
            try
            {
                _pranaReportingServices.CreateStructure(udtStructureDate.DateTime);
                MessageBox.Show("Folder Structure Created Successfully For Date:--" + udtStructureDate.DateTime);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnSaveCronExpression_Click(object sender, EventArgs e)
        {
            try
            {
                //To Save In DataBase
                string strCronExpression = txtSeconds.Text + " " + txtMinutes.Text + " " + txtHours.Text
                    + " " + txtDays.Text + " " + txtMonths.Text + " " + txtYears.Text;

                _pranaReportingServices.SaveCronExression(strCronExpression);
                //To Save In Cache--
                if (reportingServerForm == null && _container != null)
                    reportingServerForm = (ReportingServer)_container[typeof(ReportingServer)];
                ReportingServicesDataManager.BaseSettings.CronExpression = strCronExpression;

                //To Reset Sheduler
                reportingServerForm.StartScheduler();
                MessageBox.Show("Sceduler Saved Successfully !");

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void displayCronExpression()
        {
            try
            {
                //To Save In DataBase
                string strCronExpression = ReportingServicesDataManager.BaseSettings.CronExpression;
                string[] arrCronExpression=strCronExpression.Split(new char[]{' '});
                txtSeconds.Text = arrCronExpression[0];
                txtMinutes.Text = arrCronExpression[1];
                txtHours.Text = arrCronExpression[2];
                txtDays.Text = arrCronExpression[3];
                txtMonths.Text = arrCronExpression[4];
                txtYears.Text = arrCronExpression[5];
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        #region FundS Section

        private void btnSyncFunds_Click(object sender, EventArgs e)
        {
            try
            {
                ClearData();
                _pranaReportingServices.ImportFundsFromDifferentClientsDB();
                btnShowFunds_Click(null, null);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }


            }
        }
        private void ClearData()
        {
            grdFunds.DataSource = null;
            grdFunds.Refresh();

        }
        private void btnShowFunds_Click(object sender, EventArgs e)
        {
            try
            {
                ClearData();          
                grdFunds.DataSource = Prana.AutomationHandlers.ClientFunds.getAllFunds();
                grdFunds.DataBind();
                BindGrid(grdFunds);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        

        #endregion

        #region InputSettings Section

        //Key:--ClientName And Value:--ThirdPartyValueList
        Dictionary<string, ValueList> dicClientThirdParty;
        private void initializeDictionary()
        {
            try
            {
                dicClientThirdParty = new Dictionary<string, ValueList>();
                foreach (string clientName in AutomationHandlerDataManager.DicClientThirdParties.Keys)
                {
                    ValueList _currentValueList = new ValueList();
                    foreach (string thirdParty in AutomationHandlerDataManager.DicClientThirdParties[clientName])
                        _currentValueList.ValueListItems.Add(thirdParty.ToString());

                    dicClientThirdParty.Add(clientName.Trim().ToLower(), _currentValueList);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnShowSettings_Click(object sender, EventArgs e)
        {
            try
            {
                ClientSettingsPref ObjectDeSerialize = ReportingServicesDataManager.ImportXMLSettings;
                grdInputSettings.DataSource = ObjectDeSerialize;
                grdInputSettings.DataBind();
                BindGrid(grdInputSettings);

                btnSaveInputSettings.Enabled = true;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            
        }

        private void grdInputSettings_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                ClientRiskPref currentClientRP = new ClientRiskPref();
                string ClientSettingName = grdInputSettings.ActiveRow.Cells["ClientSettingName"].Text;

                //If No Risk Seetings Available Then An Empty Row Will Be Displayed
                if (ReportingServicesDataManager.DicInputSettings[ClientSettingName].RiskPreferences == null)
                    ReportingServicesDataManager.DicInputSettings[ClientSettingName].RiskPreferences = new ClientRiskPref();
                currentClientRP = ReportingServicesDataManager.DicInputSettings[ClientSettingName].RiskPreferences;
                
                //To Bind To Grid 
                List<ClientRiskPref> _lsCurrentClientRP = new List<ClientRiskPref>();
                _lsCurrentClientRP.Add(currentClientRP);
                grdRiskSettings.DataSource = _lsCurrentClientRP;
                grdRiskSettings.DataBind();
                BindGrid(grdRiskSettings);
                grdInputSettings.ActiveRow.Band.Columns["ClientSettingName"].CellActivation = Activation.NoEdit;
                if (ClientSettingName == "NewClientSetting" || grdInputSettings.ActiveRow.Cells["IsNewRow"].ToString() == true.ToString())
                    grdInputSettings.ActiveRow.Band.Columns["ClientSettingName"].CellActivation = Activation.AllowEdit;                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClientSettingsPref ObjectToSerialize = grdInputSettings.DataSource as ClientSettingsPref;
                string _ImportXMLFilePath = ReportingServicesDataManager.getImportXMLFilePath();
                _ImportXMLFilePath += "settings.xml";
                XMLUtilities.SerializeToXMLFile<ClientSettingsPref>(ObjectToSerialize,_ImportXMLFilePath);
                MessageBox.Show("Input Settings Are Saved At Location:-" + _ImportXMLFilePath);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void toolStripMenuItemAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdInputSettings.DataSource == null)
                {
                    btnShowSettings_Click(null, null);
                }
                if (grdInputSettings.DisplayLayout.Override.GroupByRowInitialExpansionState == GroupByRowInitialExpansionState.Collapsed)
                {
                    grdInputSettings.DisplayLayout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;
                } 
               
                //To Add new Blank Row --
                ClientSettings newRow = new ClientSettings();
                newRow.ClientSettingName = "NewClientSetting";
                newRow.IsNewRow = true;

                if (!ReportingServicesDataManager.DicInputSettings.ContainsKey(newRow.ClientSettingName))
                    ReportingServicesDataManager.DicInputSettings.Add(newRow.ClientSettingName, newRow);
                else
                    throw new Exception("ClientSettingName:-- " + newRow.ClientSettingName + " Already Exist!!");                

                ((ClientSettingsPref)grdInputSettings.DataSource).ClientSettingsList.Add(newRow);
                grdInputSettings.DataBind();
                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdInputSettings.ActiveRow != null)
                {
                    //To Delete The Selected Row --
                    ClientSettings deletedObject = (ClientSettings)grdInputSettings.ActiveRow.ListObject;
                    ((ClientSettingsPref)grdInputSettings.DataSource).ClientSettingsList.Remove(deletedObject);
                    grdInputSettings.DataBind();
                    ReportingServicesDataManager.DicInputSettings.Remove(deletedObject.ClientSettingName);
                }                

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdInputSettings_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Header.Caption == "ClientName" && e.Cell.Row.Cells["ClientName"].Value != null)
                {
                    e.Cell.Row.Cells["ThirdPartyNames"].ValueList = dicClientThirdParty[e.Cell.Value.ToString().Trim().ToLower()];
                }
                if (e.Cell.Column.Header.Caption == "ClientSettingName" && e.Cell.Row.Cells["ClientSettingName"].Value != null && e.Cell.DataChanged)
                {
                    //To Udate the Dictionary
                    string originalKey=e.Cell.OriginalValue.ToString();
                    string modifiedKey=e.Cell.Value.ToString();
                    
                    ReportingServicesDataManager.DicInputSettings.Add(modifiedKey, ReportingServicesDataManager.DicInputSettings[originalKey]);
                    ReportingServicesDataManager.DicInputSettings.Remove(originalKey);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdInputSettings_InitializeRow(object sender, InitializeRowEventArgs e)
        {           
            try
            {                
                //To Show ThirdParties According To Each Client
                if(e.Row.Cells["ClientName"].Value!=null)
                    e.Row.Cells["ThirdPartyNames"].ValueList = dicClientThirdParty[e.Row.Cells["ClientName"].Value.ToString().Trim().ToLower()];                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        
        #endregion

        #region GlobalSection

        //To Have data From Enums
        string[] lsImportType, lsFileFormatters, lsInputDataLocationType, lsReportType, lsRiskCalCriteria;
        //To Bind to the grid
        ValueList vlImportType, vlFileFormatters, vlInputDataLocationType, vlReportType, vlClientNames, vlThirdPartyNames, vlRiskCalCriteria;        

        private void BindGrid(UltraGrid grdToBind)
        {
            try
            {
                UltraGridBand band = grdToBind.DisplayLayout.Bands[0];
                if (grdToBind == grdFunds)
                {                    
                    band.Columns["FundID"].Hidden = true;
                    band.Columns["ClientID"].Hidden = true;
                }
                else if (grdToBind == grdInputSettings)
                {
                    List<string> displayColumns = new List<string>(new string[] { "ClientSettingName", "ClientName", "FileFormatter", "InputDataLocationType", 
                        "ReportType", "ImportType", "ThirdPartyNames"});
                    UltraWinGridUtils.SetColumns(displayColumns, grdToBind);

                    #region DropDown Options Section

                    if (lsImportType == null)
                    {
                        vlImportType = new ValueList();
                        lsImportType = Enum.GetNames(typeof(AutomationEnum.ImportTypeEnum));
                        foreach (string _importType in lsImportType)
                            vlImportType.ValueListItems.Add(_importType); 
                    }
                    if (lsFileFormatters == null)
                    {
                        vlFileFormatters = new ValueList();
                        lsFileFormatters = Enum.GetNames(typeof(AutomationEnum.FileFormate));
                        foreach (string _fileFormater in lsFileFormatters)
                            vlFileFormatters.ValueListItems.Add(_fileFormater);
                    }
                    if (lsInputDataLocationType == null)
                    {
                        vlInputDataLocationType = new ValueList();
                        lsInputDataLocationType = Enum.GetNames(typeof(AutomationEnum.InputOutputType));
                        foreach (string _InputDataLocationType in lsInputDataLocationType)
                            vlInputDataLocationType.ValueListItems.Add(_InputDataLocationType);
                    }
                    if (lsReportType == null)
                    {
                        vlReportType = new ValueList();
                        lsReportType = Enum.GetNames(typeof(AutomationEnum.ReprotTypeEnum));
                        foreach (string _reportType in lsReportType)
                            vlReportType.ValueListItems.Add(_reportType);
                    }
                    if (vlClientNames == null)
                    {
                        vlClientNames = new ValueList();
                        foreach (string _clientName in AutomationHandlerDataManager.AllClients.Values)
                            vlClientNames.ValueListItems.Add(_clientName);
                    }
                    if (vlThirdPartyNames == null)
                    {
                        //vlThirdPartyNames = new ValueList();
                        //foreach (string _thirdPartyName in AutomationHandlerDataManager..Values)
                        //    vlClientNames.ValueListItems.Add(_clientName);
                    }

                    band.Columns["FileFormatter"].ValueList = vlFileFormatters;
                    band.Columns["InputDataLocationType"].ValueList = vlInputDataLocationType;
                    band.Columns["ImportType"].ValueList = vlImportType;
                    band.Columns["ReportType"].ValueList = vlReportType;
                    band.Columns["ClientName"].ValueList = vlClientNames;

                    //This will be According to Selected Client
                    //band.Columns["ThirdPartyNames"].ValueList = vlReportType;

                    #endregion                    

                }
              
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

       

        
       
    }
}