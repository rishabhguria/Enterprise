using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.ClientPreferences;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.TradeManager.Extension;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Blotter
{
    /// <summary>
    /// Summary description for AddFills.
    /// </summary>
    public class AddFillsControl : System.Windows.Forms.UserControl
    {
        /// <summary>
        /// The form name
        /// </summary>
        private const string FORM_NAME = "Add Fills Control : ";

        /// <summary>
        /// The ultra grid1
        /// </summary>
        private PranaUltraGrid ultraGrid1;

        /// <summary>
        /// The components
        /// </summary>
        private IContainer components;

        /// <summary>
        /// The BTN save
        /// </summary>
        private Infragistics.Win.Misc.UltraButton btnSave;

        /// <summary>
        /// The amount
        /// </summary>
        double amount = double.Epsilon;

        /// <summary>
        /// The sum
        /// </summary>
        double sum = double.Epsilon;

        /// <summary>
        /// The average price
        /// </summary>
        double avgPrice = double.Epsilon;

        /// <summary>
        /// ChangeAvgPrice 
        /// </summary>
        public event EventHandler ChangeAvgPrice = null;

        /// <summary>
        /// SendFillsToServer 
        /// </summary>
        public event EventHandler SendFillsToServer;

        /// <summary>
        /// The rc
        /// </summary>
        Infragistics.Shared.ResourceCustomizer rc;

        /// <summary>
        /// The order collection
        /// </summary>
        OrderBindingList _orderCollection;

        /// <summary>
        /// The order
        /// </summary>
        OrderSingle order;

        /// <summary>
        /// The ultra panel1
        /// </summary>
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;

        /// <summary>
        /// _manualOrderID 
        /// </summary>
        public string _manualOrderID = String.Empty;

        /// <summary>
        /// AddFillsControl 
        /// </summary>
        public AddFillsControl()
        {
            rc = Infragistics.Win.UltraWinGrid.Resources.Customizer;
            _orderCollection = new OrderBindingList();
            order = new OrderSingle();
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            // TODO: Add any initialization after the InitializeComponent call

        }

        #region Dispose
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (ultraGrid1 != null)
                {
                    ultraGrid1.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (ultraPanel1 != null)
                {
                    ultraPanel1.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddFillsControl));
            this.ultraGrid1 = new PranaUltraGrid();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).BeginInit();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraGrid1
            // 
            this.ultraGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraGrid1.DisplayLayout.AddNewBox.Prompt = "Add New Fill";
            appearance1.BackColor = System.Drawing.Color.Black;
            this.ultraGrid1.DisplayLayout.Appearance = appearance1;
            this.ultraGrid1.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            appearance2.TextHAlignAsString = "Center";
            this.ultraGrid1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGrid1.DisplayLayout.GroupByBox.Hidden = true;
            this.ultraGrid1.DisplayLayout.InterBandSpacing = 1;
            this.ultraGrid1.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraGrid1.DisplayLayout.MaxRowScrollRegions = 1;
            appearance3.BackColor = System.Drawing.Color.Gold;
            appearance3.BorderColor = System.Drawing.Color.Black;
            appearance3.ForeColor = System.Drawing.Color.Black;
            this.ultraGrid1.DisplayLayout.Override.ActiveRowAppearance = appearance3;
            this.ultraGrid1.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.ultraGrid1.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.ultraGrid1.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGrid1.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGrid1.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGrid1.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGrid1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            this.ultraGrid1.DisplayLayout.Override.CellPadding = 0;
            this.ultraGrid1.DisplayLayout.Override.DefaultColWidth = 50;
            this.ultraGrid1.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ultraGrid1.DisplayLayout.Override.HeaderAppearance = appearance4;
            this.ultraGrid1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraGrid1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance5.BackColor = System.Drawing.Color.Black;
            appearance5.ForeColor = System.Drawing.Color.OrangeRed;
            this.ultraGrid1.DisplayLayout.Override.RowAlternateAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            appearance6.ForeColor = System.Drawing.Color.Lime;
            this.ultraGrid1.DisplayLayout.Override.RowAppearance = appearance6;
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ultraGrid1.DisplayLayout.Override.RowSelectorAppearance = appearance7;
            this.ultraGrid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGrid1.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.None;
            this.ultraGrid1.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.ultraGrid1.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            this.ultraGrid1.DisplayLayout.Override.SpecialRowSeparator = ((Infragistics.Win.UltraWinGrid.SpecialRowSeparator)(((Infragistics.Win.UltraWinGrid.SpecialRowSeparator.TemplateAddRow | Infragistics.Win.UltraWinGrid.SpecialRowSeparator.FixedRows)
            | Infragistics.Win.UltraWinGrid.SpecialRowSeparator.SummaryRow)));
            appearance8.BackColor = System.Drawing.SystemColors.Info;
            this.ultraGrid1.DisplayLayout.Override.SpecialRowSeparatorAppearance = appearance8;
            this.ultraGrid1.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ultraGrid1.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            scrollBarLook1.Appearance = appearance9;
            this.ultraGrid1.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.ultraGrid1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraGrid1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraGrid1.DisplayLayout.UseFixedHeaders = true;
            this.ultraGrid1.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.ultraGrid1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraGrid1.Location = new System.Drawing.Point(2, 2);
            this.ultraGrid1.Name = "ultraGrid1";
            this.ultraGrid1.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.ultraGrid1.Size = new System.Drawing.Size(446, 172);
            this.ultraGrid1.TabIndex = 0;
            this.ultraGrid1.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChange;
            this.ultraGrid1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGrid1.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ultraGrid1_AfterCellUpdate);
            this.ultraGrid1.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGrid1_InitializeLayout);
            this.ultraGrid1.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ultraGrid1_AfterRowInsert);
            this.ultraGrid1.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ultraGrid1_CellChange);
            this.ultraGrid1.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ultraGrid1_BeforeRowInsert);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance12.FontData.SizeInPoints = 10F;
            this.btnSave.Appearance = appearance12;
            this.btnSave.BackColorInternal = System.Drawing.Color.Transparent;
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(189, 178);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.btnSave);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraGrid1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(452, 206);
            this.ultraPanel1.TabIndex = 2;
            // 
            // AddFillsControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.ultraPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Name = "AddFillsControl";
            this.Size = new System.Drawing.Size(452, 206);
            this.Load += new System.EventHandler(this.AddFillsControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).EndInit();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// BindData 
        /// </summary>
        /// <param name="_order"/>
        public void BindData(OrderSingle _order)
        {
            try
            {
                order = _order;
                _manualOrderID = _order.ParentClOrderID;
                _orderCollection = ManualFillsManager.GetInstance().GetBlotterNewManualFills(_order);
                ultraGrid1.DisplayLayout.MaxBandDepth = 2;
                ultraGrid1.DataSource = _orderCollection;
                CustomizeGrid();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Customizes the grid.
        /// </summary>
        private void CustomizeGrid()
        {
            try
            {
                LoadDefaultLayout();
                SetColumnHeadersandActivation();
                SetOtherProperties();
                SetDefaultValue(order);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Loads the default layout.
        /// </summary>
        private void LoadDefaultLayout()
        {
            try
            {
                ImmutableList<string> defaultColumns = (new List<string>
                        {
                             OrderFields.PROPERTY_LAST_SHARES
                            ,OrderFields.PROPERTY_LASTPRICE
                        }).ToImmutableList();
                ColumnsCollection columns = this.ultraGrid1.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (!defaultColumns.Contains(column.Header.Caption))
                        column.Hidden = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the default value.
        /// We want that the newly added fills should be added for the last order. 
        /// </summary>
        /// <param name="order">The order.</param>
        private void SetDefaultValue(OrderSingle order)
        {
            try
            {
                DateTime dt = DateTime.MinValue;
                if (order.TransactionTime != null)
                    dt = Convert.ToDateTime(order.TransactionTime);
                ultraGrid1.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRANSACTION_TIME].DefaultCellValue = dt.Date.AddHours(12);
                ultraGrid1.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CANCEL_ORDER_ID].DefaultCellValue = order.ClOrderID;
                ultraGrid1.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_LASTPRICE].DefaultCellValue = order.Price;
                ultraGrid1.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ORDER_STATUS].DefaultCellValue = order.OrderStatusTagValue;
                ultraGrid1.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_FXRATE].DefaultCellValue = order.FXRate;
                ultraGrid1.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR].DefaultCellValue = order.FXConversionMethodOperator;
                ultraGrid1.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].DefaultCellValue = order.SettlementCurrencyID;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the column headers.
        /// </summary>
        private void SetColumnHeadersandActivation()
        {
            try
            {
                //set fill quantity, price and fx rate columns to be editable
                ultraGrid1.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_LASTPRICE].CellActivation = Activation.AllowEdit;
                ultraGrid1.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_LAST_SHARES].CellActivation = Activation.AllowEdit;

                //set captions for columns
                ultraGrid1.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_LASTPRICE].Header.Caption = BlotterConstants.CAPTION_LAST_FILL_PRICE_LOCAL;
                ultraGrid1.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_LAST_SHARES].Header.Caption = OrderFields.CAPTION_FILL;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the other properties.
        /// </summary>
        private void SetOtherProperties()
        {
            try
            {
                rc.SetCustomizedString("DataErrorCellUpdateUnableToUpdateValue", "Field empty or too big!");
                ultraGrid1.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_LAST_SHARES].Format = ApplicationConstants.FORMAT_QTY;
                ultraGrid1.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_LASTPRICE].Format = ApplicationConstants.FORMAT_AVGPRICE;
                ultraGrid1.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_NOTIONALVALUE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                ultraGrid1.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_NOTIONALVALUEBASE].Format = ApplicationConstants.FORMAT_COSTBASIS;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the InitializeLayout event of the ultraGrid1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void ultraGrid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                // Add-Row Feature Related Settings
                // --------------------------------------------------------------------------------
                // To enable the add-row functionality set the AllowAddNew. If you set the property 
                // to FixedAddRowOnTop or FixedAddRowOnBottom then the add-row will be fixed in the
                // root band. It won't scroll out of view as rows are scrolled.
                //
                e.Layout.Override.AllowAddNew = AllowAddNew.TemplateOnTopWithTabRepeat;

                // Set the appearance for template add-rows. Template add-rows are the 
                // add-row templates that are displayed with each rows collection.
                //
                e.Layout.Override.TemplateAddRowAppearance.BackColor = Color.FromArgb(245, 250, 255);
                e.Layout.Override.TemplateAddRowAppearance.ForeColor = SystemColors.GrayText;

                // Once  the user modifies the contents of a template add-row, it becomes
                // an add-row and the AddRowAppearance gets applied to such rows.
                //
                e.Layout.Override.AddRowAppearance.BackColor = Color.LightYellow;
                e.Layout.Override.AddRowAppearance.ForeColor = Color.Blue;

                // You can set the SpecialRowSeparator to a value with TemplateAddRow flag
                // turned on to display a separator ui element after the add-row. By default
                // UltraGrid displays a separator element if AllowAddNew is either
                // FixedAddRowOnTop or FixedAddRowOnBottom. For scrolling add-rows you have to
                // set the SpecialRowSeparator explicitly. You can also control the appearance
                // of the separator using the SpecialRowSeparatorAppearance proeprty.
                //
                e.Layout.Override.SpecialRowSeparator = SpecialRowSeparator.TemplateAddRow;
                e.Layout.Override.SpecialRowSeparatorAppearance.BackColor = SystemColors.Control;

                // You can display a prompt in the add-row by setting the TemplateAddRowPrompt 
                // proeprty. By default UltraGrid does not display any add-row prompt.
                //
                e.Layout.Override.TemplateAddRowPrompt = "Click here to add a new fill...";

                // You can control the appearance of the prompt using the Override's
                // TemplateAddRowPromptAppearance property. By default the prompt is
                // transparent. You can make it non-transparent by setting the appearance'
                // BackColorAlpha property or by setting the BackColor to a desired value.
                //
                e.Layout.Override.TemplateAddRowPromptAppearance.ForeColor = Color.Maroon;
                e.Layout.Override.TemplateAddRowPromptAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;

                // By default the prompt is displayed across multiple cells. You can confine
                // the prompt a particular cell by setting the SpecialRowPromptField property
                // of the band to the key of the column that you want to display the prompt in.
                //
                //e.Layout.Bands[0].SpecialRowPromptField = e.Layout.Bands[0].Columns[0].Key;


                // Other miscellaneous settings
                // --------------------------------------------------------------------------------
                // Set the scroll style to immediate so the rows get scrolled immediately
                // when the vertical scrollbar thumb is dragged.
                //
                e.Layout.ScrollStyle = ScrollStyle.Immediate;

                // ScrollBounds of ScrollToFill will prevent the user from scrolling the
                // grid further down once the last row becomes fully visible.
                //
                e.Layout.ScrollBounds = ScrollBounds.ScrollToFill;
                // --------------------------------------------------------------------------------
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the BeforeRowInsert event of the ultraGrid1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BeforeRowInsertEventArgs"/> instance containing the event data.</param>
        private void ultraGrid1_BeforeRowInsert(object sender, BeforeRowInsertEventArgs e)
        {
            try
            {
                OrderSingle _order = new OrderSingle();
                _order.LastPrice = order.Price;
                DateTime dt = DateTime.Now.ToUniversalTime();
                _order.TransactionTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dt, CachedDataManager.GetInstance.GetAUECTimeZone(order.AUECID));
                DateTime orderDate = _order.TransactionTime.Date;
                _order.TransactionTime = orderDate.AddHours(12);
                _order.LastShares = 0;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the AfterRowInsert event of the ultraGrid1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        private void ultraGrid1_AfterRowInsert(object sender, RowEventArgs e)
        {
            try
            {
                foreach (UltraGridRow row in ultraGrid1.Rows)
                {
                    if (Convert.ToDouble(row.Cells["LastShares"].Value) == double.MinValue)
                    {
                        row.Cells["LastShares"].Value = 0.0;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }

        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.Exception">Executed quantity exceeds target Quantity!</exception>
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!CachedDataManager.GetInstance.ValidateNAVLockDate(order.AUECLocalDate))
                {
                    MessageBox.Show("The date you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                        + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //UltraGridRow addedRow = this.ultraGrid1.DisplayLayout.Bands[0].AddNew( );
                if (!TradeManagerExtension.GetInstance().CheckServerStatus())
                {
                    return;
                }
                TradingTicketPrefManager.GetInstance.Initialise(Prana.BusinessObjects.AppConstants.TradingTicketPreferenceType.Company, CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, CommonDataCache.CachedDataManager.GetInstance.GetCompanyID());
                TradingTicketPrefManager.GetInstance.GetPreferenceBindingData(false, false);
                bool? isShowTargetQTY = TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.IsShowTargetQTY;

                amount = double.Epsilon;
                sum = 0.0;
                avgPrice = 0.0;
                OrderBindingList fillCollection = new OrderBindingList();
                fillCollection = (OrderBindingList)ultraGrid1.DataSource;

                if (fillCollection.Count > 0)
                {
                    OrderSingle _testorder = fillCollection[fillCollection.Count - 1]; // row where user adds new fill
                    if (_testorder.LastShares == 0.0 && _testorder.LastPrice == 0.0) // if no fill added
                    {
                        fillCollection.Remove(_testorder); // remove it from collection to b saved in DB.
                    }
                }

                OrderBindingList saveCollection = new OrderBindingList();
                foreach (OrderSingle order1 in fillCollection)
                {
                    if (order1.LastShares != double.MinValue && order1.LastShares != 0.0)
                    {
                        saveCollection.Add(order1);
                    }
                }

                if (saveCollection.Count == 0)
                {
                    OrderSingle newFill = saveCollection.AddNew() as OrderSingle;
                    newFill.ClOrderID = order.ClOrderID;// lastClOrderID;
                    //newFill.TransactionTime = DateTime.Now.ToUniversalTime().GetDateTimeFormats()[105].Remove(4, 1).Remove(6, 1).Replace('T', '-');
                    newFill.TransactionTime = DateTime.Now.ToUniversalTime();
                    saveCollection.Add(newFill);
                }
                foreach (OrderSingle checksumOrder in saveCollection)
                {
                    checksumOrder.AUECID = order.AUECID;
                    if (checksumOrder.LastShares != double.Epsilon && checksumOrder.LastShares != 0.0)
                    {
                        sum += Convert.ToDouble(checksumOrder.LastShares);
                    }
                    if (sum > Convert.ToDouble(order.Quantity))
                    {
                        if (isShowTargetQTY.HasValue && isShowTargetQTY.Value)
                        {
                            MessageBox.Show("The Executed Qty is greater than Target Qty!!! Please Check.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        }
                        else
                        {
                            MessageBox.Show("Executed quantity cannot be greater than the combined remaining and working quantity.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        }
                        return;
                    }
                }
                sum = 0.0;

                OrderSingle manualAck = new OrderSingle();
                manualAck.ClOrderID = order.ClOrderID;
                manualAck.MsgType = FIXConstants.MSGExecutionReport;
                manualAck.OrderID = order.OrderID;
                manualAck.PranaMsgType = order.PranaMsgType;
                manualAck.ParentClOrderID = order.ParentClOrderID;
                manualAck.Symbol = order.Symbol;
                manualAck.Quantity = order.Quantity;
                manualAck.OrderTypeTagValue = order.OrderTypeTagValue;
                manualAck.OrderSideTagValue = order.OrderSideTagValue;
                manualAck.OrderStatusTagValue = FIXConstants.ORDSTATUS_New;

                if (SendFillsToServer != null)
                {
                    SendFillsToServer(manualAck, EventArgs.Empty);
                }


                //ManualFillsManager.GetInstance().DeleteBlotterNewOldFills(_manualOrderID);
                foreach (OrderSingle fill in saveCollection)
                {
                    ManualFillsHelper.FillDetails(order, fill, ref sum, ref amount, ref avgPrice);
                    string errorMsg = ManualFillsHelper.SetOrderStatus(fill);
                    if (errorMsg != null && !string.IsNullOrEmpty(errorMsg))
                        throw new Exception(errorMsg);

                    if (fill.LastShares != double.Epsilon)
                    {
                        if (SendFillsToServer != null)
                        {
                            SendFillsToServer(fill, EventArgs.Empty);
                        }
                    }
                }
                string sumAndPrice = String.Empty;
                sumAndPrice = sum + "," + avgPrice;
                if (ChangeAvgPrice != null)
                {
                    ChangeAvgPrice(sumAndPrice, EventArgs.Empty);
                }
                //			return (sum.ToString() + "," + amount.ToString());  
                this.ParentForm.Close();

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Handles the Load event of the AddFillsControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void AddFillsControl_Load(object sender, EventArgs e)
        {
            try
            {
                // CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_BLOTTER_NEW);
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the color of the buttons.
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the CellChange event of the ultraGrid1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellEventArgs"/> instance containing the event data.</param>
        private void ultraGrid1_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key.Equals(OrderFields.PROPERTY_LASTPRICE) && SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.AveragePrice)
                {
                    MessageBox.Show(this, "This is a auto calculate field and will update on change in dependent column", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cell.CancelUpdate();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Event used to update Principal Amount(Local & Base) on the Add Fill UI, PRANA-11696
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrid1_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key.Equals(OrderFields.PROPERTY_LASTPRICE) || e.Cell.Column.Key.Equals(OrderFields.PROPERTY_LAST_SHARES))
                {
                    e.Cell.Row.Cells[OrderFields.PROPERTY_NOTIONALVALUE].Value = Convert.ToDouble(e.Cell.Row.Cells[OrderFields.PROPERTY_LAST_SHARES].Value) * Convert.ToDouble(e.Cell.Row.Cells[OrderFields.PROPERTY_LASTPRICE].Value);

                    if ((e.Cell.Row.Cells[OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR].Value).ToString() == "M")
                    {
                        e.Cell.Row.Cells[OrderFields.PROPERTY_NOTIONALVALUEBASE].Value = Convert.ToDouble(e.Cell.Row.Cells[OrderFields.PROPERTY_LAST_SHARES].Value) * Convert.ToDouble(e.Cell.Row.Cells[OrderFields.PROPERTY_LASTPRICE].Value) * Convert.ToDouble(e.Cell.Row.Cells[OrderFields.PROPERTY_FXRATE].Value);
                    }
                    else
                    {
                        e.Cell.Row.Cells[OrderFields.PROPERTY_NOTIONALVALUEBASE].Value = (Convert.ToDouble(e.Cell.Row.Cells[OrderFields.PROPERTY_LAST_SHARES].Value) * Convert.ToDouble(e.Cell.Row.Cells[OrderFields.PROPERTY_LASTPRICE].Value)) / Convert.ToDouble(e.Cell.Row.Cells[OrderFields.PROPERTY_FXRATE].Value);
                    }

                }
                #region  https://jira.nirvanasolutions.com:8443/browse/CI-3643
                e.Cell.Row.Cells[OrderFields.PROPERTY_CANCEL_ORDER_ID].Value = order.ClOrderID;
                #endregion


            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
    }
}
