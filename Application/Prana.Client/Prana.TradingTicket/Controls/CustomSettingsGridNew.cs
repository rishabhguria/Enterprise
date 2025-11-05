using Infragistics.Win.Misc;
using Prana.BusinessObjects;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.TradingTicket
{
    public partial class CustomSettingsGridNew : UserControl
    {

        #region Private Variables
        //private const string FORM_NAME = "CustomSettingsGrid : ";
        private CompanyUser _loginUser;
        DataTable dt = new DataTable("AUECTable");
        private Hashtable _defaultButtonCollection = new Hashtable();
        #endregion

        #region Properties
        private ArrayList _existingNames = new ArrayList();
        public ArrayList ExistingNames
        {
            get { return _existingNames; }
        }

        private Dictionary<int, TradingTicketSettingsCollection> _auecDefinedButtons = new Dictionary<int, TradingTicketSettingsCollection>();
        public Dictionary<int, TradingTicketSettingsCollection> AUECDefinedButtons
        {
            get { return _auecDefinedButtons; }
            set { _auecDefinedButtons = value; }
        }

        #endregion

        private TradingTicketSettingsCollection _ttsettingCollection;


        public CustomSettingsGridNew()
        {
            InitializeComponent();
            if (!CustomThemeHelper.IsDesignMode())
            {
                if (CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                Right7.Appearance.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                Right7.ForeColor = System.Drawing.Color.White;
                Right7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Right7.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                Right7.UseAppStyling = false;
                Right7.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                Left7.Appearance.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                Left7.ForeColor = System.Drawing.Color.White;
                Left7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Left7.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                Left7.UseAppStyling = false;
                Left7.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                Right2.Appearance.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                Right2.ForeColor = System.Drawing.Color.White;
                Right2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Right2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                Right2.UseAppStyling = false;
                Right2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                Right3.Appearance.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                Right3.ForeColor = System.Drawing.Color.White;
                Right3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Right3.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                Right3.UseAppStyling = false;
                Right3.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                Right4.Appearance.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                Right4.ForeColor = System.Drawing.Color.White;
                Right4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Right4.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                Right4.UseAppStyling = false;
                Right4.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                Right5.Appearance.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                Right5.ForeColor = System.Drawing.Color.White;
                Right5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Right5.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                Right5.UseAppStyling = false;
                Right5.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                Right6.Appearance.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                Right6.ForeColor = System.Drawing.Color.White;
                Right6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Right6.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                Right6.UseAppStyling = false;
                Right6.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                Right1.Appearance.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                Right1.ForeColor = System.Drawing.Color.White;
                Right1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Right1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                Right1.UseAppStyling = false;
                Right1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                Left1.Appearance.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                Left1.ForeColor = System.Drawing.Color.White;
                Left1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Left1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                Left1.UseAppStyling = false;
                Left1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                Left2.Appearance.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                Left2.ForeColor = System.Drawing.Color.White;
                Left2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Left2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                Left2.UseAppStyling = false;
                Left2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                Left3.Appearance.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                Left3.ForeColor = System.Drawing.Color.White;
                Left3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Left3.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                Left3.UseAppStyling = false;
                Left3.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                Left4.Appearance.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                Left4.ForeColor = System.Drawing.Color.White;
                Left4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Left4.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                Left4.UseAppStyling = false;
                Left4.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                Left5.Appearance.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                Left5.ForeColor = System.Drawing.Color.White;
                Left5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Left5.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                Left5.UseAppStyling = false;
                Left5.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                Left6.Appearance.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                Left6.ForeColor = System.Drawing.Color.White;
                Left6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Left6.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                Left6.UseAppStyling = false;
                Left6.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void SetUp(CompanyUser LoginUser, TradingTicketSettingsCollection ttsettingCollection)
        {
            try
            {
                _existingNames = new ArrayList();
                _auecDefinedButtons = new Dictionary<int, TradingTicketSettingsCollection>();
                _defaultButtonCollection = new Hashtable();
                dt.Clear();

                _loginUser = LoginUser;
                _ttsettingCollection = ttsettingCollection;
                FillAUECIDButtonsCollection(ttsettingCollection);
                BuildButtonCollection();
                BindAUECCombo();
            }
            catch (Exception)
            {
                bool rethrow = Logger.HandleException(new Exception("Problem drawing Buttons! Please Retry."), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region Initial Methods
        private void FillAUECIDButtonsCollection(TradingTicketSettingsCollection existingDBButtonDefinitions)
        {
            try
            {
                foreach (TradingTicketSettings existingButtonDefinition in existingDBButtonDefinitions)
                {
                    if (_auecDefinedButtons.ContainsKey(existingButtonDefinition.AUECID))
                    {
                        _auecDefinedButtons[existingButtonDefinition.AUECID].Add(existingButtonDefinition);
                        ExistingNames.Add(existingButtonDefinition.Name);
                    }
                    else
                    {
                        TradingTicketSettingsCollection temp = new TradingTicketSettingsCollection();
                        temp.Add(existingButtonDefinition);
                        _auecDefinedButtons.Add(existingButtonDefinition.AUECID, temp);
                        //_auecDefinedButtons[existingButtonDefinition.AUECID].Add(existingButtonDefinition);
                        ExistingNames.Add(existingButtonDefinition.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }
        private void BuildButtonCollection()
        {
            try
            {
                foreach (Control defaultButton in this.Controls["ultraGroupBox1"].Controls["pnlActionButtons"].Controls)
                {
                    if (defaultButton is UltraButton)
                    {
                        defaultButton.Tag = null;
                        defaultButton.Text = "Define";
                        _defaultButtonCollection.Add(defaultButton.Name, defaultButton);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BindAUECCombo()
        {
            try
            {
                dt = new DataTable("AUECTable");
                dt.Columns.Add("AUECID");
                dt.Columns.Add("AUECName");
                dt.Columns.Add("ASSETID");
                dt.Columns.Add("UNDERLYINGID");
                dt.Columns.Add("EXCHANGEID");
                dt.Columns.Add("CURRENCYID");
                AUECs companyUserAUECs = WindsorContainerManager.GetCompanyUserAUECDetails(_loginUser.CompanyUserID);
                foreach (AUEC existingAUEC in companyUserAUECs)
                {
                    DataRow row = dt.NewRow();
                    row["AUECID"] = existingAUEC.AUECID;
                    row["AUECName"] = existingAUEC.Name;
                    row["ASSETID"] = existingAUEC.Asset.AssetID;
                    row["UNDERLYINGID"] = existingAUEC.UnderLying.UnderlyingID;
                    row["EXCHANGEID"] = existingAUEC.Exchange.ExchangeID;
                    row["CURRENCYID"] = existingAUEC.Currency.CurrencyID;
                    dt.Rows.Add(row);
                }
                DataRow nullrow = dt.NewRow();
                nullrow["AUECID"] = int.MinValue;
                nullrow["AUECName"] = ApplicationConstants.C_COMBO_SELECT;
                nullrow["ASSETID"] = int.MinValue;
                nullrow["UNDERLYINGID"] = int.MinValue;
                nullrow["EXCHANGEID"] = int.MinValue;
                nullrow["CURRENCYID"] = int.MinValue;
                dt.Rows.Add(nullrow);
                cmbAUEC.DataSource = null;
                cmbAUEC.DataSource = dt;
                cmbAUEC.DataBind();
                cmbAUEC.DisplayMember = "AUECName";
                cmbAUEC.ValueMember = "AUECID";
                cmbAUEC.Value = int.MinValue;

                foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn col in cmbAUEC.DisplayLayout.Bands[0].Columns)
                {
                    if (col.Header.Caption.ToString().Trim() != "AUECName")
                    {
                        col.Hidden = true;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        internal TradingTicketSettingsCollection GetTicketPreferences()
        {
            return _ttsettingCollection;
        }

        private void cmbAUEC_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbAUEC.DataSource != null)
                {
                    foreach (UltraButton button in _defaultButtonCollection.Values)
                    {
                        if (button.Tag != null)
                        {
                            //TradingTicketSettings newSetting = ((TradingTicketSettings)button.Tag);
                            //TradingTicketSettings previousSetting = _ttsettingCollection.GetTradingTicketByID(newSetting.TicketSettingsID);
                            //if (previousSetting != null)
                            //{
                            //    _ttsettingCollection.Remove(previousSetting);
                            //}
                            //_ttsettingCollection.Add(newSetting);
                            button.Text = "Define";
                            button.Tag = null;
                        }
                    }

                    ReDrawButtons(int.Parse(cmbAUEC.Value.ToString()));
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void ReDrawButtons(int AUECID)
        {
            try
            {
                if (_auecDefinedButtons.ContainsKey(AUECID))
                {
                    TradingTicketSettingsCollection selectedAUECButtonsCollection = _auecDefinedButtons[AUECID];
                    foreach (TradingTicketSettings ttsetting in selectedAUECButtonsCollection)
                    {
                        UltraButton currentButton = (UltraButton)_defaultButtonCollection[ttsetting.ButtonPosition];
                        currentButton.Text = ttsetting.Name;
                        currentButton.Tag = ttsetting;
                        string[] array = ttsetting.ButtonColor.Split(',');
                        currentButton.BackColor = Color.FromArgb(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]));
                    }
                }
                else
                {
                    foreach (Control defaultButtons in _defaultButtonCollection.Values)
                    {
                        if (defaultButtons is UltraButton)
                        {
                            if (defaultButtons.Name.Contains("Left"))
                            {
                                defaultButtons.BackColor = Color.LightGreen;
                            }
                            else
                            {
                                defaultButtons.BackColor = Color.LightCoral;
                            }
                            //defaultButtons.Text = "Define";
                            //defaultButtons.Tag = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void btnDefine_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (cmbAUEC.Value != null && (int.Parse(cmbAUEC.Value.ToString()) != int.MinValue))
                {
                    int auecID = int.Parse(cmbAUEC.SelectedRow.Cells["AUECID"].Value.ToString());
                    int assetID = int.Parse(cmbAUEC.SelectedRow.Cells["ASSETID"].Value.ToString());
                    int underLyingID = int.Parse(cmbAUEC.SelectedRow.Cells["UNDERLYINGID"].Value.ToString());
                    //int exchangeID = cmbAUEC.SelectedRow.Cells["EXCHANGEID"].Value;
                    string buttonPosition = ((UltraButton)sender).Name;
                    DialogResult result;
                    NewTicketCustomSetting newButtonForm = new NewTicketCustomSetting(_loginUser, ExistingNames, auecID, assetID, underLyingID, buttonPosition);

                    if (((UltraButton)sender).Text == "Define")
                    {
                        result = newButtonForm.ShowDialog();
                    }
                    else
                    {
                        newButtonForm.SetTradingTicket((TradingTicketSettings)((UltraButton)sender).Tag);
                        result = newButtonForm.ShowDialog();
                    }

                    if (result == DialogResult.OK)
                    {
                        UltraButton currentButton = (UltraButton)sender;
                        TradingTicketSettings newActionButtonDefinition = newButtonForm.GetTradingTicket();
                        ExistingNames.Add(newActionButtonDefinition.Name);
                        currentButton.Text = newActionButtonDefinition.Name;
                        currentButton.Tag = newActionButtonDefinition;
                        string[] array = newActionButtonDefinition.ButtonColor.Split(',');
                        currentButton.BackColor = Color.FromArgb(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]));
                        if (newActionButtonDefinition.IsHotButton)
                        {
                            currentButton.Appearance.BackColor2 = Color.Yellow;
                        }
                        //currentButton.BackColor = newActionButtonDefinition.ButtonColor;
                        newActionButtonDefinition.ButtonPosition = currentButton.Name;
                        if (!_auecDefinedButtons.ContainsKey(newActionButtonDefinition.AUECID))
                        {
                            TradingTicketSettingsCollection newCollection = new TradingTicketSettingsCollection();
                            newCollection.Add(newActionButtonDefinition);
                            _auecDefinedButtons.Add(newActionButtonDefinition.AUECID, newCollection);
                            _ttsettingCollection.Add(newActionButtonDefinition);
                        }
                        else
                        {
                            if (_auecDefinedButtons[newActionButtonDefinition.AUECID].GetTradingTicketByID(newActionButtonDefinition.TicketSettingsID) == null)
                            {
                                _auecDefinedButtons[newActionButtonDefinition.AUECID].Add(newActionButtonDefinition);
                                _ttsettingCollection.Add(newActionButtonDefinition);
                            }
                            //TradingTicketSettings previousDefinition = _auecDefinedButtons[newActionButtonDefinition.AUECID].GetTradingTicketByID(newActionButtonDefinition.TicketSettingsID);
                            //if (previousDefinition == null)
                            //{

                            //}
                            //else
                            //{
                            //    //_ttsettingCollection.Remove(previousDefinition);
                            //    //_ttsettingCollection.Add(newActionButtonDefinition);

                            //}
                            //if (previousDefinition != null)
                            //{
                            //    _auecDefinedButtons[newActionButtonDefinition.AUECID].Remove(previousDefinition);
                            //}
                        }
                    }
                    newButtonForm.Close();
                }
                else
                {
                    MessageBox.Show("Please select an AUEC first.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
