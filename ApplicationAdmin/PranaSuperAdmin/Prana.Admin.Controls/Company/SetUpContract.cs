using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.IO;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{
    public partial class SetUpContract : UserControl
    {
        Logos companyLogos = null;
        Logos PranaLogo = null;
        const string C_COMBO_SELECT = "- Select -";
        private const string FORM_NAME = "SetUpContract : ";

        // bool _isSetUpContractGridInitialized = false;
        public SetUpContract()
        {
            InitializeComponent();
        }

        #region Grid Column Names

        const string COL_Symbol = "Symbol";
        const string COL_AUECID = "AUECID";
        const string COL_Multiplier = "Multiplier";
        const string COL_ContractMonthName = "ContractMonthName";
        const string COL_ContractMonthID = "ContractMonthID";
        const string COL_CompanyID = "CompanyID";
        const string COL_CompanySetUpContractID = "CompanySetUpContractID";
        const string COL_Description = "Description";

        #endregion Grid Column Names

        public void SetUpControl(int companyID)
        {
            BindCombos();
            SetContractDetails(companyID);
        }

        private void grdSetUpContracts_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //if (bool.Equals(_isSetUpContractGridInitialized, false))
            //{
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

            grdSetUpContracts.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdSetUpContracts.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            //grdSetUpContracts.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdSetUpContracts.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdSetUpContracts.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdSetUpContracts.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdSetUpContracts.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            UltraGridColumn colSymbol = band.Columns[COL_Symbol];
            colSymbol.Header.Caption = "Symbol";
            colSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            colSymbol.SortIndicator = SortIndicator.Disabled;
            colSymbol.Header.VisiblePosition = 1;

            UltraGridColumn colAuecID = band.Columns[COL_AUECID];
            colAuecID.Header.Caption = "AUEC";
            colAuecID.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            colAuecID.Header.VisiblePosition = 2;
            colAuecID.ValueList = drpDownAUEC;
            colAuecID.SortIndicator = SortIndicator.Disabled;
            colAuecID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;

            //colUnderLyingID.SortIndicator = SortIndicator.Disabled;

            UltraGridColumn colMultiplier = band.Columns[COL_Multiplier];
            colMultiplier.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerPositiveWithSpin;
            colMultiplier.Header.Caption = "Contract Size";
            colMultiplier.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
            colMultiplier.SortIndicator = SortIndicator.Disabled;
            colMultiplier.Header.VisiblePosition = 4;

            UltraGridColumn colContractMonthName = band.Columns[COL_ContractMonthName];
            colContractMonthName.Header.Caption = "Contract Month";
            colContractMonthName.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            colContractMonthName.Header.VisiblePosition = 6;
            colContractMonthName.ValueList = drpDownMonthCode;
            colContractMonthName.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            colContractMonthName.Hidden = true;

            UltraGridColumn colContractMonthID = band.Columns[COL_ContractMonthID];
            colContractMonthID.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            colContractMonthID.Header.VisiblePosition = 3;
            colContractMonthID.ValueList = drpDownMonthCode;
            colContractMonthID.SortIndicator = SortIndicator.Disabled;
            colContractMonthID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;

            UltraGridColumn colDescription = band.Columns[COL_Description];
            colDescription.Header.Caption = "Description";
            colDescription.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            colDescription.SortIndicator = SortIndicator.Disabled;
            colDescription.Header.VisiblePosition = 5;

            UltraGridColumn colCompanyID = band.Columns[COL_CompanyID];
            colCompanyID.Hidden = true;

            //UltraGridColumn colContractMonthYear = band.Columns[COL_ContractMonthYear];
            //colContractMonthYear.Header.Caption = "Contract Month Year";
            //colContractMonthYear.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            //colContractMonthYear.Header.VisiblePosition = 6;
            //colContractMonthYear.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
            //colContractMonthYear.MaskInput = "{LOC}mm/yyyy";

            UltraGridColumn colCompanySetUpContractID = band.Columns[COL_CompanySetUpContractID];
            colCompanySetUpContractID.Hidden = true;

            // _isSetUpContractGridInitialized = true;
            //}
        }

        private void BindCombos()
        {
            //Assets assets = AssetManager.GetAssets();
            //Asset asset = new Asset(int.MinValue, C_COMBO_SELECT);
            //assets.Insert(0, asset);

            System.Data.DataTable dtauec = new System.Data.DataTable();
            dtauec.Columns.Add("Data");
            dtauec.Columns.Add("Value");
            object[] row = new object[2];
            row[0] = C_COMBO_SELECT;
            row[1] = int.MinValue;
            dtauec.Rows.Add(row);

            AUECs auecs = AUECManager.GetAUEC();
            if (auecs.Count > 0)
            {
                foreach (AUEC auec in auecs)
                {
                    string Data = auec.AUECString;
                    int Value = auec.AUECID;

                    row[0] = Data;
                    row[1] = Value;
                    dtauec.Rows.Add(row);
                }
            }
            drpDownAUEC.DataSource = null;
            drpDownAUEC.DataSource = dtauec;
            drpDownAUEC.DisplayMember = "Data";
            drpDownAUEC.ValueMember = "Value";
            //drpDownAUEC.Value = int.MinValue;
            Utils.UltraDropDownFilter(drpDownAUEC, "Data");

            //UnderLyings underLyings = AssetManager.GetUnderLyings();
            //UnderLying underLying = new UnderLying(int.MinValue, C_COMBO_SELECT);
            //underLyings.Insert(0, underLying);

            //drpDownUnderLyings.DataSource = underLyings;
            //drpDownUnderLyings.ValueMember = "UnderLyingID";
            //drpDownUnderLyings.DisplayMember = "Name";
            //Utils.UltraDropDownFilter(drpDownUnderLyings, "Name");

            //Exchanges exchanges = ExchangeManager.GetExchanges();
            //Exchange exchange = new Exchange(int.MinValue, C_COMBO_SELECT);
            //exchanges.Insert(0, exchange);

            //drpDownExchanges.DataSource = exchanges;
            //drpDownExchanges.ValueMember = "ExchangeID";
            //drpDownExchanges.DisplayMember = "DisplayName";
            //Utils.UltraDropDownFilter(drpDownExchanges, "DisplayName");

            //List exchanges = ExchangeManager.GetExchanges();
            drpDownMonthCode.DataSource = null;
            drpDownMonthCode.DataSource = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(ContractMonthCode));
            drpDownMonthCode.ValueMember = "Value";
            drpDownMonthCode.DisplayMember = "DisplayText";

            Utils.UltraDropDownFilter(drpDownMonthCode, "DisplayText");
            drpDownMonthCode.Text = "All";
        }

        private SetUpContracts _setUpContracts = new SetUpContracts();
        private void SetContractDetails(int companyID)
        {
            if (companyID > 0)
            {
                SetUpContracts setUpContracts = CompanyManager.GetSetUpContracts(companyID);
                if (setUpContracts.Count > 0)
                {
                    grdSetUpContracts.DataSource = setUpContracts;
                    _setUpContracts = setUpContracts;
                }
                else
                {
                    _setUpContracts = new SetUpContracts();
                    grdSetUpContracts.DataSource = null;
                }

                BindCompanyLogos();
            }
            else
            {
                //SetUpContracts setUpContracts = new SetUpContracts();
                //grdSetUpContracts.DataSource = setUpContracts;
            }
            //Commo
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Prana.Admin.BLL.SetUpContract setUpContract = new Prana.Admin.BLL.SetUpContract();
            setUpContract.AuecID = int.MinValue;
            setUpContract.ContractMonthID = 1;
            //setUpContract.ContractMonthName = "All";
            setUpContract.Description = "";

            _setUpContracts.Add(setUpContract);
            grdSetUpContracts.DataSource = _setUpContracts;
            grdSetUpContracts.DataBind();
            //Assembly assembly = Assembly.GetExecutingAssembly();
            //MessageBox.Show("Assembly Name: ", assembly.FullName.ToString());
            //MessageBox.Show("Assembly Location: ", assembly.Location.ToString());
            //MessageBox.Show("Assembly Name: ", assembly.ReflectionOnly.ToString());

            //Type attrType = typeof(AssemblyDescriptionAttribute);
            //object[] arrs = assembly.GetCustomAttributes(attrType, false);
            //if (arrs.Length > 0)
            //{
            //    AssemblyDescriptionAttribute desc = (AssemblyDescriptionAttribute)arrs[0];
            //}

        }

        public SetUpContracts GetSetUpContractDetails()
        {
            SetUpContracts setUpContracts = new SetUpContracts();
            if (grdSetUpContracts.DataSource != null)
            {
                if (grdSetUpContracts.Rows.Count > 0)
                {
                    int index = 1;
                    // bool isValid = true;
                    foreach (UltraGridRow ultraRow in grdSetUpContracts.Rows)
                    {
                        if (ultraRow.Cells[COL_Symbol].Text.Equals(string.Empty))
                        {
                            MessageBox.Show("Please select the Symbol in row: " + index);
                            // isValid = false;
                            return setUpContracts;
                        }
                        if (int.Parse(ultraRow.Cells[COL_AUECID].Value.ToString()).Equals(int.MinValue))
                        {
                            MessageBox.Show("Please select the AUEC in row: " + index);
                            //isValid = false;
                            return setUpContracts;
                        }
                        index++;
                    }
                    setUpContracts = (SetUpContracts)grdSetUpContracts.DataSource;
                }
            }
            return setUpContracts;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdSetUpContracts.ActiveRow != null)
            {
                int companySetUpContractID = int.Parse(grdSetUpContracts.ActiveRow.Cells[COL_CompanySetUpContractID].Value.ToString());
                if (companySetUpContractID > 0)
                {
                    bool result = CompanyManager.DeleteSetUpContract(companySetUpContractID);
                    if (result == true)
                    {
                        grdSetUpContracts.ActiveRow.Delete(false);
                    }
                }
                else
                {
                    int index = grdSetUpContracts.ActiveRow.Index;
                    grdSetUpContracts.ActiveRow.Delete(false);
                }
            }
        }

        private void grdSetUpContracts_Error(object sender, Infragistics.Win.UltraWinGrid.ErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void BindCompanyLogos()
        {
            companyLogos = GeneralManager.GetCompanyLogos();
            PranaLogo = GeneralManager.GetPranaLogo();

            companyLogos.Insert(0, new Logo(int.MinValue, C_COMBO_SELECT, null));

            //Inserting the - Select - option in the Combo Box at the top.
            cmbCompanyLogo.Refresh();
            cmbCompanyLogo.DataSource = null;
            cmbCompanyLogo.DataSource = companyLogos;
            cmbCompanyLogo.DisplayMember = "LogoName";
            cmbCompanyLogo.ValueMember = "LogoID";
            cmbCompanyLogo.Value = int.MinValue;
            cmbCompanyLogo.DisplayLayout.Bands[0].Columns["LogoID"].Hidden = true;

            ColumnsCollection columns = cmbCompanyLogo.DisplayLayout.Bands[0].Columns;

            foreach (UltraGridColumn column in columns)
            {
                if (column.Key != "LogoName" && column.Key != "LogoImage")
                {
                    column.Hidden = true;
                }
            }
            cmbCompanyLogo.DisplayLayout.Bands[0].ColHeadersVisible = false;

            {
                PranaLogo.Insert(0, new Logo(int.MinValue, C_COMBO_SELECT, null));

                //Inserting the - Select - option in the Combo Box at the top.
                cmbPranaLogo.Refresh();
                cmbPranaLogo.DataSource = null;
                cmbPranaLogo.DataSource = PranaLogo;
                cmbPranaLogo.DisplayMember = "LogoName";
                cmbPranaLogo.ValueMember = "LogoID";
                cmbPranaLogo.Value = int.MinValue;
                cmbPranaLogo.DisplayLayout.Bands[0].Columns["LogoID"].Hidden = true;

                columns = cmbPranaLogo.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "LogoName" && column.Key != "LogoImage")
                    {
                        column.Hidden = true;
                    }
                }
                cmbPranaLogo.DisplayLayout.Bands[0].ColHeadersVisible = false;
            }

        }

        public byte[] m_barrImg = null;
        private string _fullName = string.Empty;
        private string _flagName = string.Empty;
        private string _logoName = string.Empty;
        private void btnUpoadCompanyLogo_Click(object sender, EventArgs e)
        {
            try
            {
                long m_lImageFileLength = long.MinValue;

                openFileDialogCompanyLogo.InitialDirectory = "DeskTop";
                openFileDialogCompanyLogo.Filter = "JPEG Files (*.jpg)|*.jpg";
                if (openFileDialogCompanyLogo.ShowDialog() == DialogResult.OK)
                {

                    string strFn = openFileDialogCompanyLogo.FileName;

                    FileInfo fiImage = new FileInfo(strFn);
                    m_lImageFileLength = fiImage.Length;

                    _fullName = fiImage.FullName;
                    _logoName = fiImage.Name;

                    FileStream fs = new FileStream(strFn, FileMode.Open,
                        FileAccess.Read, FileShare.Read);

                    m_barrImg = new byte[Convert.ToInt32(m_lImageFileLength)];
                    int iBytesRead = fs.Read(m_barrImg, 0,
                        Convert.ToInt32(m_lImageFileLength));
                    fs.Close();

                    companyLogos = (Logos)cmbCompanyLogo.DataSource;
                    int logoCount = companyLogos.Count;
                    if (logoCount > 1)
                    {
                        for (int i = 1; i < logoCount; i++)
                            companyLogos.RemoveAt(i);
                    }

                    companyLogos.Insert(logoCount - 1, new Logo(int.MinValue, _logoName, m_barrImg));
                    BindCompanyLogos(companyLogos);
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            # endregion

            finally
            {
                #region LogEntry
                Logger.LoggerWrite("ExchangeForm_Load",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "ExchangeForm_Load", null);
                #endregion
            }
        }

        private void BindCompanyLogos(Logos companyLogos)
        {
            //Inserting the - Select - option in the Combo Box at the top.
            if (companyLogos.Equals(this.companyLogos))
            {
                cmbCompanyLogo.Refresh();
                cmbCompanyLogo.DataSource = null;
                cmbCompanyLogo.DataSource = companyLogos;
                cmbCompanyLogo.DisplayMember = "LogoName";
                cmbCompanyLogo.ValueMember = "LogoID";
                cmbCompanyLogo.Value = int.MinValue;
                cmbCompanyLogo.DisplayLayout.Bands[0].Columns["LogoID"].Hidden = true;

                ColumnsCollection columns = cmbCompanyLogo.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "LogoName" && column.Key != "LogoImage")
                    {
                        column.Hidden = true;
                    }
                }
                cmbCompanyLogo.DisplayLayout.Bands[0].ColHeadersVisible = false;
            }
            else
            {
                cmbPranaLogo.Refresh();
                //cmbPranaLogo.DisplayLayout.CopyFrom(cmbCompanyLogo.DisplayLayout,PropertyCategories.All);
                cmbPranaLogo.DataSource = null;
                cmbPranaLogo.DataSource = companyLogos;
                cmbPranaLogo.DisplayMember = "LogoName";
                cmbPranaLogo.ValueMember = "LogoID";
                cmbPranaLogo.Value = int.MinValue;
                cmbPranaLogo.DisplayLayout.Bands[0].Columns["LogoID"].Hidden = true;

                ColumnsCollection columns = cmbPranaLogo.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "LogoName" && column.Key != "LogoImage")
                    {
                        column.Hidden = true;
                    }
                }
                cmbPranaLogo.DisplayLayout.Bands[0].ColHeadersVisible = false;
            }


        }


        private void cmbCompanyLogo_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Override.DefaultRowHeight = 20;
            Infragistics.Win.EmbeddableImageRenderer aImageRenderer = new Infragistics.Win.EmbeddableImageRenderer();
            aImageRenderer.DrawBorderShadow = false;
            e.Layout.Bands[0].Columns["LogoImage"].Editor = aImageRenderer;
        }

        public void SaveCompanyLogo()
        {
            int logoID = int.MinValue;
            byte[] logoImage = null;
            Logo companyLogo = new Logo();
            companyLogo.LogoID = int.Parse(cmbCompanyLogo.Value.ToString());
            companyLogo.LogoName = cmbCompanyLogo.Text.ToString();
            if (cmbCompanyLogo.Text.ToString() != "")
            {
                companyLogo.LogoImage = (byte[])cmbCompanyLogo.ActiveRow.Cells["LogoImage"].Value;
            }
            else
            {
                companyLogo.LogoImage = null;
            }
            if (companyLogo.LogoImage != logoImage)
            {
                logoID = GeneralManager.SaveCompanyLogo(companyLogo);
            }
        }

        public void SavePranaLogo()
        {
            int logoID = int.MinValue;
            byte[] logoImage = null;
            Logo companyLogo = new Logo();
            companyLogo.LogoID = int.Parse(cmbPranaLogo.Value.ToString());
            companyLogo.LogoName = cmbPranaLogo.Text.ToString();
            if (cmbPranaLogo.Text.ToString() != "")
            {
                companyLogo.LogoImage = (byte[])cmbPranaLogo.ActiveRow.Cells["LogoImage"].Value;
            }
            else
            {
                companyLogo.LogoImage = null;
            }
            if (companyLogo.LogoImage != logoImage)
            {
                logoID = GeneralManager.SavePranaLogo(companyLogo);
            }
        }

        private void btnUpoadPranaLogo_Click(object sender, EventArgs e)
        {
            try
            {
                long m_lImageFileLength = long.MinValue;

                openFileDialogCompanyLogo.InitialDirectory = "DeskTop";
                openFileDialogCompanyLogo.Filter = "JPEG Files (*.jpg)|*.jpg";
                if (openFileDialogCompanyLogo.ShowDialog() == DialogResult.OK)
                {

                    string strFn = openFileDialogCompanyLogo.FileName;

                    FileInfo fiImage = new FileInfo(strFn);
                    m_lImageFileLength = fiImage.Length;

                    _fullName = fiImage.FullName;
                    _logoName = fiImage.Name;

                    FileStream fs = new FileStream(strFn, FileMode.Open,
                        FileAccess.Read, FileShare.Read);

                    m_barrImg = new byte[Convert.ToInt32(m_lImageFileLength)];
                    int iBytesRead = fs.Read(m_barrImg, 0,
                        Convert.ToInt32(m_lImageFileLength));
                    fs.Close();

                    PranaLogo = (Logos)cmbPranaLogo.DataSource;
                    int logoCount = PranaLogo.Count;


                    if (logoCount > 1)
                    {
                        for (int i = 1; i < logoCount; i++)
                            PranaLogo.RemoveAt(i);
                    }
                    PranaLogo.Insert(logoCount - 1, new Logo(int.MinValue, _logoName, m_barrImg));
                    BindCompanyLogos(PranaLogo);
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            # endregion

            finally
            {
                #region LogEntry
                Logger.LoggerWrite("ExchangeForm_Load",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "ExchangeForm_Load", null);
                #endregion
            }
        }

        private void cmbPranaLogo_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Override.DefaultRowHeight = 20;
            Infragistics.Win.EmbeddableImageRenderer aImageRenderer = new Infragistics.Win.EmbeddableImageRenderer();
            aImageRenderer.DrawBorderShadow = false;
            e.Layout.Bands[0].Columns["LogoImage"].Editor = aImageRenderer;
        }

    }
}

