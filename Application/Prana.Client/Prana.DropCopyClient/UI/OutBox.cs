using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.DropCopyClient
{
    /// <summary>
    /// OutBox 
    /// </summary>
    public partial class OutBox : Form
    {
        #region Private Variables

        //CheckBoxOnHeader_CreationFilter headerCheckBox = new CheckBoxOnHeader_CreationFilter();
        private delegate void SetDisplayCallback(DropCopyOrder order);
        static OutBox _outbox = null;
        DCOCNew _collOutboxOrders = new DCOCNew();
        private bool _alreadyStatred = false;
        #endregion Private Variables

        #region Class Initialization

        private OutBox()
        {
            InitializeComponent();
            BindOrderSide();
        }

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

        #endregion Class Initialization

        #region Public Methods

        /// <summary>
        /// getInstance 
        /// </summary>
        public static OutBox getInstance
        {
            get
            {
                if (_outbox == null)
                {
                    _outbox = new OutBox();
                }
                return _outbox;
            }
        }
        /// <summary>
        /// AddOrders 
        /// </summary>
        public void AddOrders(DropCopyOrder order)
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
                    order.CompanyUserID = PranaDropCopyClient.LoginUser.CompanyUserID;
                    _collOutboxOrders.Add(order);
                    if (this.Visible == false)
                    {
                        Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// ClearOutbox 
        /// </summary>
        public void ClearOutbox()
        {
            _collOutboxOrders.Clear();
            grdBasket.DataSource = null;
            grdBasket.DataSource = _collOutboxOrders;
            grdBasket.DataBind();
        }

        #endregion Public Methods

        #region Private Methods

        private void BindOrderSide()
        {
            try
            {
                Sides orderSides = new Sides();
                orderSides = WindsorContainerManager.GetSides();
                orderSides.Insert(0, new Side(int.MinValue, ApplicationConstants.C_COMBO_SELECT, int.MinValue.ToString()));
                cmbbxSide.DataSource = null;
                cmbbxSide.DataSource = orderSides;
                cmbbxSide.DisplayMember = "Name";
                // valuemember changed to tagvalue as we are identifying sides by that value across the application
                // wherever we are using the filter we can check for cmbside.value to fixconstants.sides...
                cmbbxSide.ValueMember = "TagValue";
                cmbbxSide.Value = int.MinValue.ToString();


                ColumnsCollection columns = cmbbxSide.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {

                    if (column.Key != "Name")
                    {
                        column.Hidden = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }





        #endregion Private Methods

        #region Grid Settings
        /// <summary>
        /// Binds Orders to Grid 
        /// </summary>
        private void BindBasketGrid()
        {
            try
            {

                grdBasket.DataSource = null;
                grdBasket.DataSource = _collOutboxOrders;
                grdBasket.DataBind();

                //AddCheckBoxinGrid(grdBasket);
                //SetNonEditableColumns();

                grdBasket.DisplayLayout.Bands[0].Columns["LimitPrice"].Format = "F2";
                grdBasket.DisplayLayout.Bands[0].Columns["Quantity"].Format = "F0";
                grdBasket.DisplayLayout.Override.CellClickAction = CellClickAction.Edit;

                grdBasket.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_QUANTITY].NullText = "0";
                grdBasket.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADINGACCOUNT_ID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                grdBasket.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SYMBOL].CharacterCasing = CharacterCasing.Upper;

                grdBasket.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;

                #region Column Chooser

                grdBasket.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
                grdBasket.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;


                BandsCollection bands = grdBasket.DisplayLayout.Bands;

                foreach (UltraGridBand innerband in bands)
                {
                    innerband.Columns["Price"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    innerband.Columns["IsAcked"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    innerband.Columns["TradingAccountID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                }


                UltraGridBand band = grdBasket.DisplayLayout.Bands[0];

                foreach (UltraGridColumn column in band.Columns)
                {
                    column.Hidden = true;
                    column.CellAppearance.BackColor = Color.Black;
                    column.CellAppearance.ForeColor = Color.Gray;
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
            columns.Add("Symbol");
            columns.Add("OrderSide");
            columns.Add("ClientOrderID");
            columns.Add("CumQty");
            columns.Add("AvgPrice");
            columns.Add("OrderStatus");
            columns.Add("OrderType");
            columns.Add("Quantity");
            columns.Add("Text");
            return columns;
        }
        /// <summary>
        /// Sets the Editable and Non Editable UltraGrid Coloumns
        /// </summary>
        private void SetNonEditableColumns()
        {
            foreach (UltraGridColumn existingColumn in grdBasket.DisplayLayout.Bands[0].Columns)
            {
                existingColumn.CellActivation = Activation.NoEdit;
            }

            grdBasket.DisplayLayout.Bands[0].Columns[Global.OrderFields.PROPERTY_SYMBOL].CellActivation = Activation.AllowEdit;
            grdBasket.DisplayLayout.Bands[0].Columns["SymbolMap"].CellActivation = Activation.AllowEdit;
            grdBasket.DisplayLayout.Bands[0].Columns[Global.OrderFields.PROPERTY_TRADINGACCOUNT_ID].CellActivation = Activation.AllowEdit;
        }


        //Left for Future Use
        //private void AddCheckBoxinGrid(UltraGrid grid)
        //{
        //    grid.CreationFilter = headerCheckBox;
        //    grid.DisplayLayout.Bands[0].Columns.Add(OrderFields.PROPERTY_CHKBOX, "");
        //    grid.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CHKBOX].DataType = typeof(bool);
        //    headerCheckBox._CLICKED += new CheckBoxOnHeader_CreationFilter.HeaderCheckBoxClickedHandler(headerCheckBox__CLICKED);
        //}

        //void headerCheckBox__CLICKED(object sender, CheckBoxOnHeader_CreationFilter.HeaderCheckBoxEventArgs e)
        //{

        //}

        #endregion

        #region Event Handlers


        private void OutBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
            //this.Hide();
            _outbox = null;
        }


        private void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                UltraGridBand band = grdBasket.DisplayLayout.Bands[0];
                //Clear all filters
                band.ColumnFilters.ClearAllFilters();
                // Set default filters as they define orders visible in a particular blotter
                //Set Filters...add additional conditions
                if (this.cmbbxSide.Value != null)
                {

                    switch (this.cmbbxSide.Value.ToString())
                    {
                        case FIXConstants.SIDE_Buy:
                        case FIXConstants.SIDE_Buy_Closed:
                        case FIXConstants.SIDE_Buy_Open:
                        case FIXConstants.SIDE_BuyMinus:
                        case FIXConstants.SIDE_Cross:
                        case FIXConstants.SIDE_CrossShort:
                        case FIXConstants.SIDE_Sell:
                        case FIXConstants.SIDE_Sell_Closed:
                        case FIXConstants.SIDE_Sell_Open:
                        case FIXConstants.SIDE_SellPlus:
                        case FIXConstants.SIDE_SellShort:
                        case FIXConstants.SIDE_SellShortExempt:
                            band.ColumnFilters[Prana.Global.OrderFields.PROPERTY_ORDER_SIDE].FilterConditions.Add(FilterComparisionOperator.Equals, this.cmbbxSide.Text);
                            break;
                        default:
                            break;
                    }
                }
                if (this.txtBoxSymbol.Text != string.Empty)
                {
                    band.ColumnFilters[Global.OrderFields.PROPERTY_SYMBOL].FilterConditions.Add(FilterComparisionOperator.StartsWith, this.txtBoxSymbol.Text);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtBoxSymbol.Clear();
                cmbbxSide.Value = int.MinValue;
                btnFilter_Click(this, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        #endregion Event Handlers
    }
}