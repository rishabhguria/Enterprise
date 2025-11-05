using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Prana.ShortLocate.Preferences
{
    public partial class ctrlShortLocatePreferences : UserControl, IPreferences
    {
        public ctrlShortLocatePreferences()
        {
            InitializeComponent();
        }
        public IPreferenceData GetPrefs()
        {
            return ctrlShortLocatePrefDataManager.GetInstance.ShortLocatePreferences;
        }

        public UserControl Reference()
        {
            return this;
        }
        private CompanyUser _user;

        public CompanyUser User
        {
            get { return _user; }
            set { _user = value; }
        }

        public void SetUp(CompanyUser user)
        {
            try
            {
                _user = user;
                ctrlShortLocatePrefDataManager.GetInstance._loggedInUser = user;
                grdDecimalPlaces.DataSource = DeepCopyHelper.Clone(ctrlShortLocatePrefDataManager.ShortLocateUIGridBindingList);
                SetGridLayout();
                DataTable broker = GetBrokers();
                SetBorrowerBrokerDataSource(cmbDefaultBorrowerBroker, broker, "Name", "CounterPartyID");
                ShortLocateUIPreferences preferences = ctrlShortLocatePrefDataManager.GetInstance.GetShortLocatePreferences(user.CompanyUserID);
                if (preferences != null)
                {
                    SetShortLocateInputParameters(preferences);
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


        private void SetShortLocateInputParameters(ShortLocateUIPreferences preferences)
        {
            try
            {
                cmbAlert.Value = preferences.Alert;
                cmbRebateFees.Value = preferences.Rebatefees;
                cmbYTD.Value = preferences.YTD;
                cmbDefaultBorrowerBroker.Value = preferences.DefaultBorrowBroker;
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


        public static DataTable GetBrokers()
        {
            Dictionary<int, string> dictBrokers = CommonDataCache.CachedDataManager.GetInstance.GetAllThirdPartiesWithShortName();
            DataTable dtBroker = new DataTable();
            try
            {
                dtBroker.Columns.Add("CounterPartyID");
                dtBroker.Columns.Add("Name");
                object[] rowBroker = new object[2];
                foreach (KeyValuePair<int, string> keyVal in dictBrokers)
                {
                    rowBroker[0] = keyVal.Key;
                    rowBroker[1] = keyVal.Value;
                    dtBroker.Rows.Add(rowBroker);
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
            return dtBroker;
        }

        public bool Save()
        {
            try
            {
                ShortLocateUIPreferences shortLocatePref = new ShortLocateUIPreferences();
                if (cmbAlert.SelectedItem == null)
                    shortLocatePref.Alert = "No Sound";
                else
                    shortLocatePref.Alert = cmbAlert.SelectedItem.ToString();
                if (cmbDefaultBorrowerBroker.SelectedItem == null)
                    shortLocatePref.DefaultBorrowBroker = "GS";
                else
                    shortLocatePref.DefaultBorrowBroker = cmbDefaultBorrowerBroker.SelectedItem.ToString();
                if (cmbRebateFees.SelectedItem == null)
                    shortLocatePref.Rebatefees = ShortLocateRebateFee.BPS.ToString();
                else
                    shortLocatePref.Rebatefees = cmbRebateFees.SelectedItem.ToString();
                if (cmbYTD.SelectedItem == null)
                    shortLocatePref.YTD = ShortLocateYTD.YTD.ToString();
                else
                    shortLocatePref.YTD = cmbYTD.SelectedItem.ToString();
                grdDecimalPlaces.UpdateData();
                BindingList<ShortLocateUIGridDetails> DecimalPrefList = (BindingList<ShortLocateUIGridDetails>)grdDecimalPlaces.DataSource;
                Dictionary<string, double> dictFieldToDecimal = DecimalPrefList.ToDictionary(pref => pref.FieldName, pref => pref.DecimalPlaces);
                shortLocatePref.RebatefeesDecimal = Math.Round(dictFieldToDecimal["Rebate Fees"]);
                shortLocatePref.TotalAmountDecimal = Math.Round(dictFieldToDecimal["Total Amount"]);
                shortLocatePref.LastPxDecimal = Math.Round(dictFieldToDecimal["Last Px"]);
                ctrlShortLocatePrefDataManager.SaveShortLocatePrefs(shortLocatePref, User.CompanyUserID);
                ctrlShortLocatePrefDataManager.GetInstance.ShortLocatePreferences = shortLocatePref;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return true;
        }
        public void RestoreDefault()
        {

        }
        public string SetModuleActive
        {
            set
            {

            }
        }

        private void SetBorrowerBrokerDataSource(UltraComboEditor ultraComboEditor, object dataSource, string displayMember, string valueMember)
        {
            try
            {
                ultraComboEditor.Value = null;
                ultraComboEditor.DataSource = dataSource;
                ultraComboEditor.DisplayMember = displayMember;
                ultraComboEditor.ValueMember = valueMember;
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

        private void SetGridLayout()
        {
            try
            {
                grdDecimalPlaces.DisplayLayout.Bands[0].Columns["FieldName"].Header.Caption = "Field";
                grdDecimalPlaces.DisplayLayout.Bands[0].Columns["FieldName"].CellActivation = Activation.NoEdit;
                grdDecimalPlaces.DisplayLayout.Bands[0].Columns["DecimalPlaces"].Header.Caption = "DecimalPlaces";
                grdDecimalPlaces.DisplayLayout.Bands[0].Columns["DecimalPlaces"].CellActivation = Activation.AllowEdit;
                grdDecimalPlaces.DisplayLayout.Bands[0].Columns["DecimalPlaces"].MaxValue = 10000;
                grdDecimalPlaces.DisplayLayout.Bands[0].Columns["DecimalPlaces"].MinValue = 0;
                grdDecimalPlaces.DisplayLayout.Bands[0].Columns["DecimalPlaces"].Format = "#,##,###0";
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

    }
}
