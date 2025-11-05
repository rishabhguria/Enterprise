using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.ComponentModel;
using System.Windows.Forms;
namespace Prana.Blotter
{
    public partial class CtrlTradingInstruction : UserControl
    {

        BindingList<TradingInstruction> _tradeInstList = TradeManager.TradeInstructionCollection.GetInstance().TradingInstructionColleciton;
        private CompanyUser _loginUser;

        public Prana.BusinessObjects.CompanyUser LoginUser
        {
            set { _loginUser = value; }
        }

        private bool _isGridInitialized = false;

        #region Desk Trade Grid Columns
        const string CAPTION_DeskID = "ID";
        const string CAPTION_Side = "Side";
        const string CAPTION_Symbol = "Symbol";
        const string CAPTION_Quantity = "Quantity";
        const string CAPTION_Instructions = "Instructions";
        const string CAPTION_SideID = "SideID";
        const string CAPTION_UserID = "UserID";
        const string CAPTION_User = "User";
        const string CAPTION_TradingAcc = "TradingAcc";
        const string CAPTION_Select = "Select";
        const string CAPTION_Status = "Status";
        #endregion

        public CtrlTradingInstruction()
        {

            InitializeComponent();
        }

        /// <summary>
        /// Handles the InitializeLayout event of the grdDeskTrades control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdDeskTrades_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                if (bool.Equals(_isGridInitialized, false))
                {
                    UltraGridBand _gridBandDeskTrades = grdDeskTrades.DisplayLayout.Bands[0];
                    SetGridColumns(_gridBandDeskTrades);
                }
                _isGridInitialized = true;
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

        ValueList _sideList = new ValueList();


        /// <summary>
        /// Sets the grid columns.
        /// </summary>
        /// <param name="gridBandDeskTrades"></param>/param>
        private void SetGridColumns(UltraGridBand gridBandDeskTrades)
        {
            UltraGridColumn colSymbol = gridBandDeskTrades.Columns[CAPTION_Symbol];
            colSymbol.Width = 50;
            colSymbol.Header.Caption = "Symbol";
            colSymbol.Header.VisiblePosition = 1;
            colSymbol.CellActivation = Activation.NoEdit;

            UltraGridColumn colQuantity = gridBandDeskTrades.Columns[CAPTION_Quantity];
            colQuantity.Width = 65;
            colQuantity.Header.Caption = "Quantity";
            colQuantity.Header.VisiblePosition = 2;
            colQuantity.CellActivation = Activation.NoEdit;

            _sideList.ValueListItems.Add(Prana.BusinessObjects.FIXConstants.SIDE_Buy, "BUY");
            _sideList.ValueListItems.Add(Prana.BusinessObjects.FIXConstants.SIDE_Buy_Closed, "BUY TO COVER");
            _sideList.ValueListItems.Add(Prana.BusinessObjects.FIXConstants.SIDE_Sell, "SELL");
            _sideList.ValueListItems.Add(Prana.BusinessObjects.FIXConstants.SIDE_SellShort, "SELL SHORT");

            UltraGridColumn ColSide = gridBandDeskTrades.Columns[CAPTION_Side];
            ColSide.Width = 65;
            ColSide.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ColSide.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.OnCellActivate;
            ColSide.ValueList = _sideList;
            ColSide.Header.Caption = "Side";
            ColSide.Header.VisiblePosition = 3;
            ColSide.CellActivation = Activation.NoEdit;

            UltraGridColumn colInstructions = gridBandDeskTrades.Columns[CAPTION_Instructions];
            colInstructions.Width = 80;
            colInstructions.Header.Caption = "Instructions";
            colInstructions.Header.VisiblePosition = 4;
            // colInstructions.CellActivation = Activation.NoEdit;

            //UltraGridColumn colTradingAcc = gridBandDeskTrades.Columns[CAPTION_TradingAcc];
            //colTradingAcc.CellActivation = Activation.NoEdit;
            //colTradingAcc.Hidden = true;

            //UltraGridColumn ColUser = gridBandDeskTrades.Columns[CAPTION_User];
            //ColUser.CellActivation = Activation.NoEdit;

            UltraGridColumn ColStatus = gridBandDeskTrades.Columns[CAPTION_Status];
            ColStatus.CellActivation = Activation.NoEdit;

            UltraGridColumn colSelect = gridBandDeskTrades.Columns["IsSelected"];
            colSelect.Header.Caption = CAPTION_Select;
            colSelect.CellActivation = Activation.AllowEdit;
            //UltraGridColumn colUserID = gridBandDeskTrades.Columns[CAPTION_UserID];
            //colUserID.Hidden = true;
        }

        /// <summary>
        /// Inits the control.
        /// </summary>
        internal void InitControl()
        {
            try
            {
                grdDeskTrades.DataSource = null;
                grdDeskTrades.DataSource = _tradeInstList;
                grdDeskTrades.DataBind();
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
        /// Handles the Click event of the btnReject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (TradingInstruction tradInst in _tradeInstList)
                {
                    bool sendToServer = false;
                    if (tradInst.IsSelected && tradInst.Status == TradingInstructionEnums.TradingInstStatus.ActionPending)
                    {
                        tradInst.Status = TradingInstructionEnums.TradingInstStatus.Rejected;

                        switch (tradInst.MsgType)
                        {
                            case PranaMessageConstants.MSGTradingInstInternal:
                                tradInst.MsgType = PranaMessageConstants.MSGTradingInstInternalAccept;
                                //Set the new target user id = old sender user id
                                tradInst.TargetUserID = tradInst.SenderUserID;
                                sendToServer = true;
                                break;
                            case PranaMessageConstants.MSGTradingInstClient:
                                //No ack to be sent to the server in case a client msg is rejected. 
                                tradInst.MsgType = PranaMessageConstants.MSGTradingInstClientAccept;
                                sendToServer = false;
                                break;
                        }
                        if (sendToServer)
                        {
                            TradeManager.TradeManager.GetInstance().SendTradingInstructionAccept();
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
        /// Handles the Click event of the btnAccept control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (TradingInstruction tradInst in _tradeInstList)
                {
                    if (tradInst.IsSelected && tradInst.Status == TradingInstructionEnums.TradingInstStatus.ActionPending)
                    {
                        bool sendToServer = false;
                        tradInst.Status = TradingInstructionEnums.TradingInstStatus.Accepted;
                        switch (tradInst.MsgType)
                        {
                            case PranaMessageConstants.MSGTradingInstInternal:
                                tradInst.MsgType = PranaMessageConstants.MSGTradingInstInternalAccept;
                                tradInst.TargetUserID = tradInst.SenderUserID;
                                tradInst.UserID = _loginUser.CompanyUserID;
                                sendToServer = true;
                                break;
                            case PranaMessageConstants.MSGTradingInstClient:
                                tradInst.MsgType = PranaMessageConstants.MSGTradingInstClientAccept;
                                if (tradInst.UserID == int.MinValue)
                                {
                                    tradInst.UserID = _loginUser.CompanyUserID;
                                    sendToServer = true;
                                    //                              tradInst.User = _loginUser.ShortName;
                                }
                                break;
                        }
                        //Set the new target user id = old sender user id
                        if (sendToServer)
                        {
                            TradeManager.TradeManager.GetInstance().SendTradingInstructionAccept();
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
        /// Handles the Click event of the grdDeskTrades control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void grdDeskTrades_Click(object sender, EventArgs e)
        {

        }

        private void grdDeskTrades_CellChange(object sender, CellEventArgs e)
        {
            // Check if the column is Select All
            if (e.Cell.Column.Header.Caption == "Select")
            {
                grdDeskTrades.UpdateData();
                //bool isChecked = Convert.ToBoolean(e.Cell.Value);
                // TradingInstruction selectedInst = e.Cell.Row.ListObject as TradingInstruction;
                // if (selectedInst != null)
                // {
                //     _selectedList.Add(selectedInst);
                // }
            }

        }

        private void CtrlTradingInstruction_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_BLOTTER_NEW);
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
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
                btnReject.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnReject.ForeColor = System.Drawing.Color.White;
                btnReject.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnReject.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnReject.UseAppStyling = false;
                btnReject.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnAccept.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnAccept.ForeColor = System.Drawing.Color.White;
                btnAccept.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAccept.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAccept.UseAppStyling = false;
                btnAccept.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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


    }
}
