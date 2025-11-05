using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.CommonDataCache;
using Prana.Fix.FixDictionary;
using Prana.Global;
using Prana.TradeManager.Extension;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace Prana.DropCopyClient
{
    /// <summary>
    /// InBox 
    /// </summary>
    public partial class InBox : Form
    {
        #region Private Variables
        private delegate void SetDisplayCallback(DropCopyOrder order);
        //List<string> _symbolList = new List<string>();
        CheckBoxOnHeader_CreationFilter headerCheckBox = new CheckBoxOnHeader_CreationFilter();
        static InBox _inbox = null;
        DCOCNew _collInboxOrders = new DCOCNew();
        //private Dictionary<string, UltraGridRow> _rowCollection = new Dictionary<string, UltraGridRow>();
        private Dictionary<string, DropCopyOrder> _clientOrderIds = new Dictionary<string, DropCopyOrder>();
        private bool _alreadyStatred = false;

        private static int _newMessageCount = 0;
        //int _hashCode = 0;
        #endregion Private Variables

        #region Class Initialization

        private InBox()
        {
            InitializeComponent();
            BindTradingAccount();
            //_hashCode = this.GetHashCode();
            //SecMasterCacheManager.GetInstance.Subscribe(_hashCode);
            //SecMasterCacheManager.GetInstance.SecMstrDataResponse += new SecMasterCacheManager.SecurityMasterDataHandler(GetInstance_SecMstrDataResponse);
            timerRefresh.Start();
        }

        //void GetInstance_SecMstrDataResponse(SecMasterBaseObj secMasterObj)
        //{
        //    foreach (DropCopyOrder order in _collInboxOrders)
        //    {
        //        if (order.SymbolMap.ToUpper() == secMasterObj.TickerSymbol)
        //        {
        //            order.AUECID = secMasterObj.AUECID;
        //            order.AssetID = secMasterObj.AssetID;
        //            order.UnderlyingID = secMasterObj.UnderLyingID;
        //            order.ExchangeID = secMasterObj.ExchangeID;                    

        //        }
        //    }
        //}




        #endregion Initialization

        #region Public Methods
        /// <summary>
        /// Init 
        /// </summary>
        public void Init()
        {
            if (!_alreadyStatred)
            {
                BindBasketGrid();
                Show();
                Visible = false;
                _alreadyStatred = true;
            }

        }
        /// <summary>
        /// getInstance 
        /// </summary>
        public static InBox getInstance
        {
            get
            {

                if (_inbox == null)
                {
                    _inbox = new InBox();
                }
                return _inbox;
            }
        }
        /// <summary>
        /// AddOrders 
        /// </summary>
        /// <param name="order"/>
        public void AddOrders(DropCopyOrder order)
        {
            try
            {
                SetDisplayCallback mi = new SetDisplayCallback(AddOrders);
                if (UIValidation.GetInstance().validate(grdBasket))
                {
                    if (grdBasket.InvokeRequired)
                    {
                        Invoke(mi, order);
                    }
                    else
                    {
                        //if (!timerRefreash.Enabled) // for symbol checking
                        //{
                        //    timerRefreash.Start();
                        //}
                        if (this.Visible == false)// to pop up if hidden
                        {
                            Visible = true;
                        }
                        if (_clientOrderIds.ContainsKey(order.ClientOrderID))
                        {
                            DropCopyOrder origOrder = _clientOrderIds[order.ClientOrderID];
                            _newMessageCount--;
                            if (order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Rejected)
                            {
                                origOrder.IsReject = true;
                                origOrder.Text = order.Text;
                            }
                            else
                            {
                                origOrder.IsAcked = true;
                                origOrder.Text = order.Text;
                            }
                        }
                        else
                        {
                            _collInboxOrders.Add(order);
                            _clientOrderIds.Add(order.ClientOrderID, order);
                            order.SymbolMap = order.Symbol;
                            order.CompanyUserID = PranaDropCopyClient.LoginUser.CompanyUserID;
                            _newMessageCount++;
                        }
                        this.Text = "InBox: " + _newMessageCount.ToString() + " New Order Messages";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        #endregion Public Methods

        #region Grid Settings
        /// <summary>
        /// Binds Orders to Grid 
        /// </summary>
        private void BindBasketGrid()
        {
            try
            {
                //grdBasket.DataSource = null;
                grdBasket.DataSource = _collInboxOrders;
                grdBasket.DataBind();
                AddCheckBoxinGrid(grdBasket);
                SetNonEditableColumns();
                grdBasket.DisplayLayout.Bands[0].Columns["LimitPrice"].Format = "F2";
                grdBasket.DisplayLayout.Bands[0].Columns["Quantity"].Format = "F0";
                grdBasket.DisplayLayout.Override.CellClickAction = CellClickAction.Edit;
                grdBasket.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_QUANTITY].NullText = "0";
                grdBasket.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADINGACCOUNT_ID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                grdBasket.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SYMBOL].CharacterCasing = CharacterCasing.Upper;
                grdBasket.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
                grdBasket.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
                #region Column Chooser
                grdBasket.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
                grdBasket.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
                grdBasket.DisplayLayout.Override.HeaderAppearance.BackColor = Color.DimGray;
                BandsCollection bands = grdBasket.DisplayLayout.Bands;
                foreach (UltraGridBand innerband in bands)
                {
                    innerband.Columns["MsgType"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    innerband.Columns["MsgType"].Hidden = true;
                }
                UltraGridBand band = grdBasket.DisplayLayout.Bands[0];

                foreach (UltraGridColumn column in band.Columns)
                {
                    column.Hidden = true;
                }

                List<string> columns = ColumnList();
                int i = 0;
                foreach (string columnName in columns)
                {
                    UltraGridColumn column = band.Columns[columnName];
                    column.Hidden = false;
                    column.Header.VisiblePosition = i;
                    i++;

                }


                #endregion Column Chooser


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private List<string> ColumnList()
        {
            List<string> columns = new List<string>();
            columns.Add("Broker");
            columns.Add("TradingAccountID");
            columns.Add("Symbol");
            columns.Add("SymbolMap");
            columns.Add("ClientOrderID");
            columns.Add("MsgType");
            columns.Add("OrderSide");
            columns.Add("OrderType");
            columns.Add("Price");
            columns.Add("Quantity");
            columns.Add("Text");
            columns.Add("checkBox");
            return columns;

        }

        private void SetNonEditableColumns()
        {
            foreach (UltraGridColumn existingColumn in grdBasket.DisplayLayout.Bands[0].Columns)
            {
                existingColumn.CellActivation = Activation.NoEdit;
            }

            grdBasket.DisplayLayout.Bands[0].Columns["SymbolMap"].CellActivation = Activation.AllowEdit;
            grdBasket.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADINGACCOUNT_ID].CellActivation = Activation.AllowEdit;
            grdBasket.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CHKBOX].CellActivation = Activation.AllowEdit;
        }


        private void BindTradingAccount()
        {
            try
            {

                TradingAccountCollection accounts = new TradingAccountCollection();
                accounts = CachedDataManager.GetInstance.GetUserTradingAccounts();
                cmbBoxTradingAccount.DataSource = null;
                cmbBoxTradingAccount.DataSource = accounts;
                cmbBoxTradingAccount.DisplayMember = "Name";
                cmbBoxTradingAccount.ValueMember = "TradingAccountID";
                cmbBoxTradingAccount.DataBind();
                if (cmbBoxTradingAccount.DataSource != null && grdBasket.DataSource!=null)
                {
                    grdBasket.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADINGACCOUNT_ID].ValueList = cmbBoxTradingAccount;

                    ColumnsCollection columns = cmbBoxTradingAccount.DisplayLayout.Bands[0].Columns;
                    foreach (UltraGridColumn column in columns)
                    {
                        if (column.Key != "Name")
                        {
                            column.Hidden = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void AddCheckBoxinGrid(UltraGrid grid)
        {
            grid.CreationFilter = headerCheckBox;
            grid.DisplayLayout.Bands[0].Columns.Add(OrderFields.PROPERTY_CHKBOX, "");
            grid.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CHKBOX].DataType = typeof(bool);
            headerCheckBox._CLICKED += new CheckBoxOnHeader_CreationFilter.HeaderCheckBoxClickedHandler(headerCheckBox__CLICKED);
        }




        #endregion

        #region Event Handlers



        private void Symbolvalidation(UltraGridRow row)
        {
            string symbolError = "Symbol Can't be Validated";
            // string symbol = row.Cells["Symbol"].Value.ToString().ToUpper().Trim();

            if (row.Cells[OrderFields.PROPERTY_AUEC_ID].Value.ToString() != int.MinValue.ToString())
            {
                if (row.Cells[OrderFields.PROPERTY_TEXT].Value.ToString() == symbolError)
                {
                    row.Cells[OrderFields.PROPERTY_TEXT].Value = string.Empty;
                }
            }
            else
            {
                row.Cells[OrderFields.PROPERTY_TEXT].Value = symbolError;
                row.Cells[OrderFields.PROPERTY_TEXT].Appearance.ForeColor = Color.Red;
            }
        }
        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (TradeManagerExtension.GetInstance().ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
            {
                foreach (UltraGridRow row in grdBasket.Rows)
                {

                    if (row.Cells[OrderFields.PROPERTY_CHKBOX].Value.ToString() == "True" && row.Cells["IsAcked"].Value.ToString().ToUpper() != "TRUE" && row.Cells["IsReject"].Value.ToString().ToUpper() != "TRUE")
                    {
                        DropCopyOrder dropCopyOrder = (DropCopyOrder)row.ListObject;
                        //dropCopyOrder.TransactionTime = DateTime.Now.ToUniversalTime().ToString(Prana.Utilities.DateTimeUtilities.DateTimeConstants.NirvanaDateTimeFormat);
                        //int auecSettlementPeriod = CachedDataManager.GetInstance.GetAUECSettlementPeriod(dropCopyOrder.AUECID);
                        // dropCopyOrder.SettlementDate = Prana.Utilities.DateTimeUtilities.BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(DateTime.Now.ToUniversalTime(), auecSettlementPeriod, dropCopyOrder.AUECID);
                        string validationmsg = ValidateDropCopyOrders(dropCopyOrder);
                        if (validationmsg == string.Empty)
                        {
                            PranaMessage PranaMessage = Transformer.CreatePranaMessageThroughReflection(dropCopyOrder);

                            PranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_PranaMsgType, ((int)OrderFields.PranaMsgTypes.ORDStaged).ToString());
                            PranaMessage.MessageType = CustomFIXConstants.MsgDropCopyAck;
                            PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value = FIXConstants.MSGOrder;
                            TradeManagerExtension.GetInstance().SendMessage(PranaMessage);
                        }
                        else
                        {
                            MessageBox.Show(validationmsg);
                            break;
                        }
                    }
                }
                //foreach(
            }

        }
        /// <summary>
        /// for validating drop copy orders
        /// </summary>
        /// <param name="dropcopyorder"></param>
        /// <returns></returns>
        private string ValidateDropCopyOrders(DropCopyOrder dropcopyorder)
        {
            if (dropcopyorder.SymbolMap == string.Empty)
            {
                return "The Symbol is invalid. Please enter valid Symbol";
            }
            if (dropcopyorder.TradingAccountID == int.MinValue)
            {
                return "The Trading Account ID is invalid. Please enter valid Trading Account ID ";
            }
            if (dropcopyorder.AUECID == int.MinValue)
            {
                return "Symbol could not be validated.";
            }
            else
                return string.Empty;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReject_Click(object sender, EventArgs e)
        {
            if (TradeManagerExtension.GetInstance().ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
            {
                foreach (UltraGridRow row in grdBasket.Rows)
                {

                    if (row.Cells[OrderFields.PROPERTY_CHKBOX].Value.ToString() == "True" && row.Cells["IsAcked"].Value.ToString().ToUpper() != "TRUE" && row.Cells["IsReject"].Value.ToString().ToUpper() != "TRUE")
                    {
                        DropCopyOrder dropCopyOrder = (DropCopyOrder)row.ListObject;

                        //Currently if the order is rejected then there should be some text
                        if (row.Cells["Text"].Value.ToString() == string.Empty)
                        {

                        }

                        Prana.DropCopyClient.UI.RejectionDialog rejectionDialog = new Prana.DropCopyClient.UI.RejectionDialog();
                        rejectionDialog.ShowDialog();
                        if (rejectionDialog.DialogResult == DialogResult.OK)
                        {
                            string[] textboxlines = rejectionDialog.textBox1.Lines;
                            StringBuilder strbuilder = new StringBuilder();
                            foreach (string strline in textboxlines)
                            {
                                strbuilder.Append(strline);
                                strbuilder.Append(" ");
                            }
                            dropCopyOrder.Text = strbuilder.ToString();
                            if (dropCopyOrder.Text == string.Empty)
                            {
                                dropCopyOrder.Text = "Rejected by User";
                            }
                            //dropCopyOrder.Text = "Rejected by User";
                            PranaMessage PranaMessage = Transformer.CreatePranaMessageThroughReflection(dropCopyOrder);
                            PranaMessage.MessageType = CustomFIXConstants.MsgDropCopyReject;

                            TradeManagerExtension.GetInstance().SendMessage(PranaMessage);
                        }

                    }
                }
            }
        }

        private void InBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel=true ;
            //this.Hide();
            timerRefresh.Stop();
            _inbox = null;
        }


        void headerCheckBox__CLICKED(object sender, CheckBoxOnHeader_CreationFilter.HeaderCheckBoxEventArgs e)
        {

        }


        #endregion Event Handlers




        //private void grdBasket_InitializeRow(object sender, InitializeRowEventArgs e)
        //{
        //    ((DropCopyOrder)e.Row.ListObject).RowObject = e.Row;
        //    if (!_rowCollection.ContainsKey(e.Row.Cells[OrderFields.PROPERTY_CLIENTORDERID].Value.ToString()))
        //    {
        //        _rowCollection.Add(e.Row.Cells[OrderFields.PROPERTY_CLIENTORDERID].Value.ToString(), e.Row);
        //    }
        //}
        /// <summary>
        /// ClearInbox 
        /// </summary>
        public void ClearInbox()
        {
            // _rowCollection.Clear();
            _clientOrderIds.Clear();
            _collInboxOrders.Clear();
            //grdBasket.DataSource = null;
            grdBasket.DataSource = _collInboxOrders;
            grdBasket.DataBind();
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            try
            {
                List<string> errorsymbols = new List<string>();
                foreach (DropCopyOrder order in _collInboxOrders)
                {

                    if (order.AUECID == int.MinValue)
                    {
                        errorsymbols.Add(order.SymbolMap.ToUpper());
                    }
                }
                if (errorsymbols.Count > 0)
                {
                    // SecMasterCacheManager.GetInstance.GetSecMasterData(errorsymbols, ApplicationConstants.PranaSymbology, _hashCode);
                }
                //  List<string> errorsymbolList = new List<string>();
                //  List<string> symbolList = GetInCorrectSymbols();
                // Dictionary<string, Level1Data> latestSymbolListData = LiveFeedDataSubscriberClient.GetInstance().GetLiveFeedDataSnapShot(symbolList);
                //if (errorsymbolList.Count == 0)
                //{
                //  //  timerRefreash.Stop();
                //}
                foreach (UltraGridRow row in grdBasket.Rows)
                {

                    row.Appearance.BackColor = Color.Black;

                    if (row.Cells[OrderFields.PROPERTY_ORDER_SIDE].Value.ToString() == "Buy")
                    {
                        row.Appearance.ForeColor = Color.Green;
                    }
                    else if (row.Cells[OrderFields.PROPERTY_ORDER_SIDE].Value.ToString() == "Sell")
                    {
                        row.Appearance.ForeColor = Color.Red;
                    }



                    if (row.Cells["IsAcked"].Value.ToString().ToUpper() == "TRUE" || row.Cells["IsReject"].Value.ToString().ToUpper() == "TRUE")
                    {
                        if (row.Cells["IsAcked"].Value.ToString().ToUpper() == "TRUE")
                        {
                            row.Activation = Activation.Disabled;
                            row.Cells[OrderFields.PROPERTY_CHKBOX].Value = false;

                        }
                        if (row.Cells["IsReject"].Value.ToString().ToUpper() == "TRUE")
                        {
                            row.Activation = Activation.NoEdit;
                            row.Cells[OrderFields.PROPERTY_CHKBOX].Value = false;
                            row.Appearance.BackColor = Color.Red;
                            row.Appearance.ForeColor = Color.White;
                        }
                    }
                    else
                    {
                        Symbolvalidation(row);
                    }
                }
            }
            catch (Exception)
            {
                timerRefresh.Stop();
            }
        }
    }
}