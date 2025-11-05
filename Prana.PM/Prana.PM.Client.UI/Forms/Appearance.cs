using Prana.BusinessObjects;
using Prana.ExposurePnlCache;
using Prana.Global;
using Prana.LogManager;
using Prana.PM.Client.UI.Classes;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Data;
using System.Windows.Forms;
namespace Prana.PM.Client.UI
{
    public partial class Appearance : Form
    {
        public Appearance()
        {
            InitializeComponent();
        }

        public event EventHandler<EventArgs<bool, bool>> PrefsUpdated;
        public event EventHandler<EventArgs<bool>> UpdateDashboard;

        CompanyUser _loginUser = null;
        public CompanyUser LoginUser
        {
            get { return _loginUser; }
            set { _loginUser = value; }
        }

        private void Appearance_Load(object sender, EventArgs e)
        {
            try
            {
                PMAppearances pMAppearances = PMAppearanceManager.PMAppearance;
                ctrlAppearance1.InitializeControl(pMAppearances);
                ctrlFormat1.Setup();

                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RISK_MANAGEMENT);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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

        private void SetButtonsColor()
        {
            try
            {
                btnDefault.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnDefault.ForeColor = System.Drawing.Color.White;
                btnDefault.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnDefault.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnDefault.UseAppStyling = false;
                btnDefault.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void btnDefault_Click(object sender, EventArgs e)
        {
            try
            {
                PMAppearanceManager.RestoreDefaults();
                PMAppearances pMAppearances = PMAppearanceManager.PMAppearance;
                PMAppearanceManager.GetDictionaryForDecimalPlaces(pMAppearances);
                ctrlAppearance1.InitializeControl(pMAppearances);
                ctrlFormat1.Setup();
                RestoreDefaultConsolidatedDashboard();
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
        private void RestoreDefaultConsolidatedDashboard()
        {
            try
            {
                DataTable dataTableDashboard = new DataTable();
                CustomizableCardGrid card = new CustomizableCardGrid();
                card.Setup();
                ExposureAndPnlOrderSummary expnlOrderSummary = new ExposureAndPnlOrderSummary();
                dataTableDashboard = CommonHelper.GetOrderSummaryTableFromObject(expnlOrderSummary);
                card.RestoreDefaultDashBoard(dataTableDashboard, Application.StartupPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME + "\\" + _loginUser.CompanyUserID + "\\" + "MainDashboard.xml");
                if (UpdateDashboard != null)
                {
                    UpdateDashboard(this, new EventArgs<bool>(false));
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
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                PMAppearances pMAppearances = PMAppearanceManager.PMAppearance;
                ctrlAppearance1.SavePMAppearances(pMAppearances);
                ctrlFormat1.SaveFormat(pMAppearances);
                if (tabAppearance.Tabs["tabViewsSettings"].Selected)
                {
                    if ((ctrlCopyView.comboCopyFrom.SelectedIndex != -1 && ctrlCopyView.comboCopyTo.GetNoOfCheckedItems() > 0))
                    {
                        ctrlCopyView.CopyViewToTabs();

                    }
                    else
                    {
                        ctrlCopyView.labelStatus.Appearance.ForeColor = System.Drawing.Color.Red;
                        ctrlCopyView.labelStatus.Text = "Tab(s) not selected";
                    }
                    ctrlCopyView.SetDefaultView(pMAppearances);
                }
                PMAppearanceManager.SavePreferences(pMAppearances);
                if (tabAppearance.Tabs["tabViewsSettings"].Selected)
                {
                    if ((ctrlCopyView.comboCopyFrom.SelectedIndex != -1 && ctrlCopyView.comboCopyTo.GetNoOfCheckedItems() > 0))
                    {
                        if (PrefsUpdated != null)
                        {
                            PrefsUpdated(this, new EventArgs<bool, bool>(ctrlAppearance1.IsRowColorChanged, tabAppearance.Tabs["tabViewsSettings"].Selected));
                        }
                    }
                }
                else
                {
                    if (PrefsUpdated != null)
                    {
                        PrefsUpdated(this, new EventArgs<bool, bool>(ctrlAppearance1.IsRowColorChanged, tabAppearance.Tabs["tabViewsSettings"].Selected));
                    }
                }
                lblDataSaved.Text = "Preferences Updated" + DateTime.Now.ToString();
                ctrlAppearance1.IsRowColorChanged = false;
                if (UpdateDashboard != null)
                    UpdateDashboard(this, new EventArgs<bool>(true));
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



        private void tabAppearance_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                PMAppearances pMAppearances = PMAppearanceManager.PMAppearance;
                if (e.Tab.Key.Equals("tabColorSettings"))
                {
                    btnDefault.Visible = true;
                    this.btnSave.Location = new System.Drawing.Point(221, 7);
                    ctrlAppearance1.InitializeControl(pMAppearances);

                }

                if (e.Tab.Key.Equals("tabFormat"))
                {
                    btnDefault.Visible = true;
                    this.btnSave.Location = new System.Drawing.Point(221, 7);
                }
                if (e.Tab.Key.Equals("tabViewsSettings"))
                {
                    btnDefault.Visible = false;
                    this.btnSave.Location = new System.Drawing.Point(150, 7);
                    ctrlCopyView.Setup(_loginUser);
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