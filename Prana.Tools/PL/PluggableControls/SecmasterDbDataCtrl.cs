using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Fix.FixDictionary;
using Prana.LogManager;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class SecMasterDbDataCtrl : UserControl
    {
        DataTable dtDataBaseMessages = new DataTable();
        public event EventHandler DataReloaded;
        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public SecMasterDbDataCtrl()
        {
            try
            {
                InitializeComponent();
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

        List<string> _gridColumnList = new List<string>();
        public List<string> DisplayedColumnList
        {
            set
            {
                _gridColumnList = value;
            }
        }
        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        private void LoadComboBoxes()
        {
            try
            {
                cmbbxCountry.DataSource = PranaDataManager.GetAllCountries();
                cmbbxCountry.DisplayMember = "Name";
                cmbbxCountry.ValueMember = "ID";
                cmbbxCountry.DisplayLayout.Bands[0].Columns["ID"].Hidden = true;
                cmbbxCountry.DisplayLayout.Bands[0].Columns["ShortName"].Hidden = true;
                cmbbxCountry.DataBind();
                cmbbxCountry.Value = int.MinValue;
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
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetDataFromDB_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                int countryID = int.Parse(cmbbxCountry.Value.ToString());
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetSecMasterData_ALL", new object[] { countryID }, "SMConnectionString");
                dtDataBaseMessages = ds.Tables[0];
                NewUtilities.AddPrimaryKey(dtDataBaseMessages);
                //listDataBaseMsgs = Transformer.CreatePranaMessages(dtDataBaseMessages);
                grdDataBase.DataSource = dtDataBaseMessages;
                grdDataBase.DataBind();
                if (DataReloaded != null)
                {
                    DataReloaded(null, null);
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


        public DataTable Data
        {
            get
            {
                dtDataBaseMessages = NewUtilities.GetSelectedRows(grdDataBase, null);
                return dtDataBaseMessages;
            }
            set { dtDataBaseMessages = value; }
        }
        /// <summary>
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        public void ExportToExcel()
        {
            try
            {
                ExcelAndPrintUtilities excelUtils = new ExcelAndPrintUtilities();
                excelUtils.ExportToExcel(grdDataBase);
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
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="filenameData"></param>
        /// <param name="name"></param>
        public void SetUp(string name)
        {
            try
            {
                grdDataBase.DisplayLayout.Override.RowAppearance.BackColor = Color.Black;
                grdDataBase.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.Green;
                grdDataBase.DisplayLayout.Override.RowAppearance.ForeColor = Color.LightGray;
                ultraExpandableGroupBox2.Text = name;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                this.Name = name;
                LoadComboBoxes();

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
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        public void BindData()
        {
            try
            {
                try
                {
                    grdDataBase.DataSource = dtDataBaseMessages;
                    dtDataBaseMessages.TableName = "Comparision";
                    Utilities.UI.UIUtilities.UltraWinGridUtils.SetColumns(_gridColumnList, grdDataBase);
                    grdDataBase.DataBind();
                    if (grdDataBase.DisplayLayout.Bands[0].Columns.Exists("RowID"))
                    {
                        grdDataBase.DisplayLayout.Bands[0].Columns["RowID"].Hidden = true;
                    }
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
        public UserControl Control
        {
            get { return this; }
        }
        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveDuplicate_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> tags = new List<string>();
                tags.Add(CustomFIXConstants.CUST_TAG_TickerSymbol);
                tags.Add(CustomFIXConstants.CUST_TAG_ReutersSymbol);
                List<PranaMessage> listDataBaseMsgs = Transformer.CreatePranaMessages(dtDataBaseMessages);
                List<PranaMessage> duplicatemsgs = NewUtilities.RemoveDuplicateRows(listDataBaseMsgs, tags);
                dtDataBaseMessages = Transformer.CreateDataTable(listDataBaseMsgs);
                BindData();
                DataTable dupliCteMsgs = Transformer.CreateDataTable(duplicatemsgs);
                DuplicateDataForm form = new DuplicateDataForm(dupliCteMsgs);
                form.Show();
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

        public new bool Validate()
        {
            try
            {
                return true;
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
            return false;



        }
        public void Reload()
        { }

        #region IPluggableUserControl Members


        //public event EventHandler FilterChanged;

        //public void ApplyFilters(object sender, EventArgs e)
        //{

        //}

        //public void ValueSelected(object sender, EventArgs e)
        //{

        //}
        //public string GetSelectedValue(int type)
        //{
        //    return "SecurityMaster";
        //}
        //public event EventHandler SelectedValueChanged;

        #endregion

        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void grdDataBase_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdDataBase);
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

        private void grdDataBase_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

    }
}
