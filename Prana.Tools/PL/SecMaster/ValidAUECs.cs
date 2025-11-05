using ExportGridsData;
using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class ValidAUECs : Form, IExportGridData
    {
        private static ValidAUECs _validAuecs = null;
        public ValidAUEC _selectedAUEC = new ValidAUEC();
        static object locker = new object();

        //CtrlValidAUEC ctrlValidAUEC1 = null;


        public static ValidAUECs GetInstance()
        {
            lock (locker)
            {
                if (_validAuecs == null)
                {
                    _validAuecs = new ValidAUECs();
                }
                else
                {
                    _validAuecs.ctrlValidAUEC1.ClearPreviousColumnFilters();
                }
            }
            return _validAuecs;
        }

        public ValidAUECs()
        {
            try
            {
                InitializeComponent();
                SetUp();
                InstanceManager.RegisterInstance(this);
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

            //BindAuec();
            //BindGrid(dictAuec);
        }


        private void WireEvents()
        {
            try
            {
                this.ctrlValidAUEC1.ValidAUECSelected += ctrlValidAUEC1_ValidAUECSelected;
                this.ctrlValidAUEC1.CloseForm += new EventHandler(ctrlValidAUEC1_CloseForm);
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



        void ctrlValidAUEC1_CloseForm(object sender, EventArgs e)
        {
            this.Close();
        }

        void ctrlValidAUEC1_ValidAUECSelected(object sender, EventArgs e)
        {
            try
            {
                ListEventAargs listEventAargs = e as ListEventAargs;
                if (listEventAargs != null)
                {
                    ValidAUEC validAUEC = listEventAargs.argsObject as ValidAUEC;
                    _selectedAUEC = validAUEC;
                    this.Hide();
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


        ////Bind all AUECS in the dictionary...
        //Dictionary<int, ValidAUEC> dictAuec = new Dictionary<int, ValidAUEC>();
        //private void BindAuec()
        //{
        //    try
        //    {
        //        CachedDataManager cachedDataManager = CachedDataManager.GetInstance;


        private void ValidAUECs_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                _validAuecs = null;
                // _symbolLookUp.AUEC = null;
                UnWireEvents();
                InstanceManager.ReleaseInstance(typeof(ValidAUECs));
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

        private void UnWireEvents()
        {
            try
            {
                this.ctrlValidAUEC1.CloseForm -= new EventHandler(ctrlValidAUEC1_CloseForm);
                this.ctrlValidAUEC1.ValidAUECSelected -= ctrlValidAUEC1_ValidAUECSelected;
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

        private void ValidAUECs_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_SYMBOL_LOOKUP);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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

        private void SetUp()
        {
            try
            {
                ctrlValidAUEC1.SetUP();
                WireEvents();
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


        // On UI load search data by exchange - omshiv, NOv 2013
        /// <summary>
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="exchangeIdentifier"></param>
        internal void SearchAUECOnLoad(string exchangeIdentifier)
        {
            try
            {
                ctrlValidAUEC1.SearchAUECOnLoad(exchangeIdentifier);
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

        /// <summary>
        /// used to Export Data for automation
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="WindowName"></param>
        /// <param name="tabName"></param>
        /// <param name="filePath"></param>
        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            this.ctrlValidAUEC1.ExportGridData(filePath);
        }
    }
}