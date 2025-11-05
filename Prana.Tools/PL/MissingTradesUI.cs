using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Fix.FixDictionary;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class MissingTradesUI : Form, IPluggableTools, ILaunchForm
    {
        public delegate void SMQMsgInvokeDelegate(object sender, EventArgs<QueueMessage> e);
        PranaBinaryFormatter binaryFormatter = new PranaBinaryFormatter();
        public event EventHandler LaunchForm;
        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public MissingTradesUI()
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
        /// <summary>
        /// Get Missing Trades Frm Server
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void GetMissingTradesFrmServer()
        {
            try
            {
                if (_postTradeServices != null && _postTradeServices.IsConnected)
                {
                    QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_GET_MISSING_TRADES, string.Empty);
                    qMsg.RequestID = System.Guid.NewGuid().ToString();

                    _postTradeServices.SendMessage(qMsg);
                    toolStripStatusLabel1.Text = "Getting missing trades....";
                    btnMissingTrades.Enabled = false;


                }
                else
                {
                    toolStripStatusLabel1.Text = "Please Connect to Server.";
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

        #region IPluggableTools Members
        /// <summary>
        /// Set up on load
        /// 
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public void SetUP()
        {
            try
            {

                if (_postTradeServices != null && _postTradeServices.IsConnected)
                {
                    _postTradeServices.Connected += new EventHandler(_postTradeServices_Connected);
                    _postTradeServices.Disconnected += new EventHandler(_postTradeServices_Disconnected);
                    // _postTradeServices.MessageReceived += new MsgReceivedDelegate(_postTradeServices_MessageReceived);

                    _postTradeServices.MissingTradesRecieved += new EventHandler<EventArgs<QueueMessage>>(_postTradeServices_MissingTradesRecieved);
                    //new MsgReceivedDelegate(_postTradeServices_MissingTradesRecieved);

                }
                else
                {
                    toolStripStatusLabel1.Text = SecMasterConstants.MSG_ServerNotConnected;

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
        /// Handling of Missiing trades response from Server
        /// </summary>
        /// <param name="message"></param>
        void _postTradeServices_MissingTradesRecieved(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        SMQMsgInvokeDelegate invokeDelegate = new SMQMsgInvokeDelegate(_postTradeServices_MissingTradesRecieved);
                        this.BeginInvoke(invokeDelegate, new object[] { this, e });
                    }
                    else
                    {
                        this.btnMissingTrades.Enabled = true;
                        this.btnMissingTrades.Text = "Refresh Data";
                        List<PranaMessage> pranaMsgList = binaryFormatter.DeSerialize(e.Value.Message.ToString()) as List<PranaMessage>; ;

                        if (pranaMsgList != null)
                        {
                            BindMessageGrid(pranaMsgList);
                            //TODO
                            //toolStripProgressBar1
                            toolStripStatusLabel1.Text = "Missing Trades count: " + pranaMsgList.Count;
                        }
                        else
                        {
                            if (_postTradeServices != null)
                                toolStripStatusLabel1.Text = "Problem in getting Missing trades.";
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
        /// Bind missing trades to grid
        /// 
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="msgs"></param>
        private void BindMessageGrid(List<PranaMessage> msgs)
        {
            try
            {

                OrderCollection clonedOrders = new OrderCollection();
                foreach (PranaMessage msg in msgs)
                {
                    Order order = Transformer.CreateOrder(msg);
                    Prana.CommonDataCache.NameValueFiller.FillNameDetailsOfOrder(order);
                    clonedOrders.Add(order);
                }
                grdMessages.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.None;
                grdMessages.DataSource = null;
                grdMessages.DataSource = clonedOrders;
                grdMessages.DataBind();
                if (grdMessages.DisplayLayout.Bands[0].Columns.Count > 0)
                {
                    grdMessages.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
                    ShowDisplayColumns(grdMessages);
                    if (grdMessages.DisplayLayout.Bands[0].Columns.Exists("ActivSymbol"))
                        grdMessages.DisplayLayout.Bands[0].Columns["ActivSymbol"].Header.Caption = "ACTIV Symbol";
                    if (grdMessages.DisplayLayout.Bands[0].Columns.Exists("FactSetSymbol"))
                        this.grdMessages.DisplayLayout.Bands[0].Columns["FactSetSymbol"].Header.Caption = "FactSet Symbol";
                }

                String path = Application.StartupPath + "\\FixMssingTrades.xml";
                if (File.Exists(path))
                {
                    grdMessages.DisplayLayout.LoadFromXml(path);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
        }

        /// <summary>
        /// Hide/ Show requiered columns
        /// </summary>
        /// <param name="grdMessages"></param>
        private void ShowDisplayColumns(Infragistics.Win.UltraWinGrid.UltraGrid grdMessages)
        {
            try
            {
                string[] ColumnNames = Enum.GetNames(typeof(OrderFields.ServerDisplayColumns));
                int i = 0;
                foreach (UltraGridColumn column in grdMessages.DisplayLayout.Bands[0].Columns)
                {
                    column.Hidden = true;
                }
                foreach (string columnName in ColumnNames)
                {
                    UltraGridColumn column = grdMessages.DisplayLayout.Bands[0].Columns[columnName];
                    column.Hidden = false;
                    column.Header.VisiblePosition = i;
                    i++;
                }
                // Chnaging the caption to the Broker, PRANA-13231
                if (grdMessages.DisplayLayout.Bands[0].Columns.Exists("CounterPartyName"))
                    grdMessages.DisplayLayout.Bands[0].Columns["CounterPartyName"].Header.Caption = ApplicationConstants.CONST_BROKER;
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
        /// <summary>
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void UnwireEvents()
        {
            try
            {
                if (_postTradeServices != null)
                {
                    _postTradeServices.Connected -= new EventHandler(_postTradeServices_Connected);
                    _postTradeServices.Disconnected -= new EventHandler(_postTradeServices_Disconnected);
                    _postTradeServices.MissingTradesRecieved -= new EventHandler<EventArgs<QueueMessage>>(_postTradeServices_MissingTradesRecieved);
                    //new MsgReceivedDelegate(_postTradeServices_MissingTradesRecieved);
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

        void _postTradeServices_Disconnected(object sender, EventArgs e)
        {
            _postTradeServices.IsConnected = false;
        }

        void _postTradeServices_Connected(object sender, EventArgs e)
        {
            _postTradeServices.IsConnected = true;
        }

        public Form Reference()
        {
            return this;
        }

        public event EventHandler PluggableToolsClosed;

        ISecurityMasterServices _securityMaster = null;
        public ISecurityMasterServices SecurityMaster
        {
            set { _securityMaster = value; }
        }

        IPostTradeServices _postTradeServices = null;
        public IPostTradeServices PostTradeServices
        {
            set { _postTradeServices = value; }
        }

        public IPricingAnalysis PricingAnalysis
        {
            set {; }
        }

        #endregion

        private void MissingTradesUI_FormClosed(object sender, FormClosedEventArgs e)
        {

            try
            {
                if (PluggableToolsClosed != null)
                {
                    PluggableToolsClosed(this, EventArgs.Empty);
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
        /// Handling get Missing Trades btn click
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMissingTrades_Click(object sender, EventArgs e)
        {
            try
            {
                GetMissingTradesFrmServer();
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
        /// Handling btn click to add symbol through Symbol lookupUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addSymbolManuallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdMessages.ActiveRow != null)
                {
                    Order PranaOrder = grdMessages.ActiveRow.ListObject as Order;
                    if (PranaOrder != null)
                    {
                        ListEventAargs args = new ListEventAargs();
                        Dictionary<String, String> argDict = new Dictionary<string, string>();
                        SecMasterUIObj secMasterUI = new SecMasterUIObj();

                        secMasterUI.BloombergSymbol = PranaOrder.BloombergSymbol;
                        secMasterUI.TickerSymbol = PranaOrder.Symbol.Trim();
                        //  secMasterUI.StrikePrice = PranaOrder.StrikePrice;
                        //secMasterUI.PutOrCall = PranaOrder.PutOrCall;
                        secMasterUI.SedolSymbol = PranaOrder.SEDOLSymbol;
                        if (PranaOrder.ContractMultiplier != 0)
                            secMasterUI.Multiplier = PranaOrder.ContractMultiplier;
                        // secMasterUI.Delta = (float)PranaOrder.Delta;
                        secMasterUI.LongName = PranaOrder.Description;
                        //  secMasterUI. = PranaOrder.ExchangeName;
                        secMasterUI.IDCOOptionSymbol = PranaOrder.IDCOSymbol;
                        secMasterUI.ISINSymbol = PranaOrder.ISINSymbol;
                        //  secMasterUI.LongName = PranaOrder.


                        // Added secMaster UI object to args by this we can send sec master values to symbol lookup UI. - omshiv Nov, 2013
                        argDict.Add("SecMaster", binaryFormatter.Serialize(secMasterUI));
                        // argDict.Add("SearchCriteria", SecMasterConstants.SearchCriteria.Ticker.ToString());

                        //  argDict.Add("Symbol", PranaOrder.Symbol.Trim());

                        argDict.Add("Action", SecMasterConstants.SecurityActions.ADD.ToString());
                        args.listOfValues.Add(ApplicationConstants.CONST_SYMBOL_LOOKUP);
                        args.argsObject = argDict;

                        if (LaunchForm != null)
                        {
                            LaunchForm(this, args);
                        }
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "Please select trade row to add symbol!";

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

        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                String filePath = Application.StartupPath + "\\FixMssingTrades.xml";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                grdMessages.DisplayLayout.SaveAsXml(filePath);

                toolStripStatusLabel1.Text = "Layout has been Saved!";
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

        private void sendAlertForTradesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exportDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Saving to Excel file. This launches the Save dialog for the user to select the Save Path
                if (grdMessages.Rows.Count > 0)
                {
                    CreateExcel(Prana.Utilities.UI.MiscUtilities.ExcelUtilities.FindSavePathForExcel());
                }
                else
                    toolStripStatusLabel1.Text = "No data to Export";

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
            finally
            {
                //Any cleanup code
                this.Cursor = Cursors.Default;
            }
        }
        private void CreateExcel(String filepath)
        {
            try
            {
                if (filepath != null)
                {

                    grdExclExporter.Export(grdMessages, filepath);
                    MessageBox.Show("Grid data successfully downloaded to " + filepath, "Nirvana Excel Exporter", MessageBoxButtons.OK, MessageBoxIcon.Information);
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



        private void grdMessages_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    Point mousePoint = new Point(e.X, e.Y);
                    UIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(mousePoint);
                    UltraGridCell cell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
                    if (cell != null)
                    {
                        cell.Row.Activate();
                    }
                }
            }
            catch
            {
                //Do Nothing as user can try again
            }
        }
        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MissingTradesUI_Load(object sender, EventArgs e)
        {
            try
            {
                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLE_NAME_MISSING_TRADES);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                    this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    this.statusStrip1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripStatusLabel1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripStatusLabel1.Font = new Font("Century Gothic", 9F);
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

        private void SetButtonsColor()
        {
            try
            {
                btnMissingTrades.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnMissingTrades.ForeColor = System.Drawing.Color.White;
                btnMissingTrades.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnMissingTrades.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnMissingTrades.UseAppStyling = false;
                btnMissingTrades.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void grdMessages_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {

            try
            {

                e.Cancel = true;
                if (grdMessages.DataSource != null)
                {
                    (this.FindForm()).AddCustomColumnChooser(this.grdMessages);
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

        private void grdMessages_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();

        }
    }
}