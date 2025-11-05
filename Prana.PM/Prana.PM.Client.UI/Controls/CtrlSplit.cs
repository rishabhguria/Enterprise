using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDataCache;
using Prana.Global;
//using Prana.PostTrade;
using Prana.LogManager;
using System;
using System.Windows.Forms;


namespace Prana.PM.Client.UI.Controls
{
    public partial class CtrlSplit : UserControl
    {
        public CtrlSplit()
        {
            InitializeComponent();
        }

        AllocatedTradesList _bindedList = null;
        double _baseQty = 0.0;
        const string CAPTION_OpenQty = "OpenQty";
        const string CAPTION_OrderSide = "Side";
        const string CAPTION_OrderSideID = "SideID";
        const string CAPTION_ID = "ID";
        double _baseOpenCommissionandFees = 0.0;

        public void SetGridDataSource(AllocatedTradesList allTradelist)
        {
            _bindedList = allTradelist;
            _baseQty = allTradelist[0].OpenQty;
            _baseOpenCommissionandFees = allTradelist[0].OpenTotalCommissionandFees;

            if (_bindedList != null && _bindedList.Count == 1)
            {
                //AllocatedTrade newTaxlots = ClosingManager.Instance.AddTaxlot(_bindedList[0]);

                //_bindedList.Add(newTaxlots);
            }

            grdSplit.DataSource = _bindedList;

        }
        public void SetGridDataSourceForModify(AllocatedTradesList allTradelist)
        {
            _bindedList = allTradelist;
            //  _bindedList[0].ParentRowPk = _bindedList[0].TaxlotPk;

            grdSplit.DataSource = _bindedList;
            grdSplit.Rows[0].Hidden = true;

        }


        private ValueList _buysides = new ValueList();
        private ValueList _sellsides = new ValueList();
        private void BindSides()
        {
            _buysides.ValueListItems.Add(FIXConstants.SIDE_Buy, "Buy");
            _buysides.ValueListItems.Add(FIXConstants.SIDE_Buy_Closed, "Buy to Close");
            _buysides.ValueListItems.Add(FIXConstants.SIDE_Buy_Open, "Buy to Open");
            _sellsides.ValueListItems.Add(FIXConstants.SIDE_Sell, "Sell");
            _sellsides.ValueListItems.Add(FIXConstants.SIDE_Sell_Closed, "Sell to Close");
            _sellsides.ValueListItems.Add(FIXConstants.SIDE_Sell_Open, "Sell to Open");
            _sellsides.ValueListItems.Add(FIXConstants.SIDE_SellShort, "Sell Short");

        }
        //private void btnAddTaxlot_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (_bindedList != null && _bindedList.Count==1)
        //        {
        //            AllocatedTrade newTaxlots = ClosingManager.Instance.AddTaxlot(_bindedList[0]);

        //            _bindedList.Add(newTaxlots);
        //        }
        //        else 
        //        {
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }

        //    }


        //}
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (AllocatedTrade taxlots in _bindedList)
                {
                    taxlots.AUECLocalCloseDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(taxlots.AUECID));
                    taxlots.TimeOfSaveUTC = DateTime.UtcNow;
                }

                if (this != null)
                {
                    this.FindForm().Close();
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


        private void grdSplit_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            if (e.Row != null && !e.ReInitialize)
            {

                AllocatedTrade taxlot = (AllocatedTrade)e.Row.ListObject;

                if (taxlot.SideID == FIXConstants.SIDE_Buy || taxlot.SideID == FIXConstants.SIDE_Buy_Closed || taxlot.SideID == FIXConstants.SIDE_Buy_Open || taxlot.SideID == FIXConstants.SIDE_Buy_Cover)
                {
                    e.Row.Cells[CAPTION_OrderSideID].ValueList = _buysides;

                }
                else
                {
                    e.Row.Cells[CAPTION_OrderSideID].ValueList = _sellsides;
                }
                //for the 0th position taxlot we should not allow edit
                if (taxlot.ParentRowPk == 0)
                {
                    e.Row.Cells[CAPTION_OrderSideID].Activation = Activation.NoEdit;
                    e.Row.Cells[CAPTION_OpenQty].Activation = Activation.NoEdit;
                }
                else
                {
                    e.Row.Cells[CAPTION_OrderSideID].Column.CellActivation = Activation.AllowEdit;
                    e.Row.Cells[CAPTION_OrderSideID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    if (_bindedList.Count > 1)
                    {
                        e.Row.Cells[CAPTION_OpenQty].Column.CellActivation = Activation.AllowEdit;
                    }

                }

            }
            //else
            //{

            //    e.Row.Activation = Activation.NoEdit;


            //}
        }





        private void grdSplit_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            foreach (UltraGridColumn column in e.Layout.Bands[0].Columns)
            {
                column.Hidden = true;
                column.CellActivation = Activation.NoEdit;
            }
            e.Layout.Bands[0].Columns[OrderFields.PROPERTY_SYMBOL].Hidden = false;
            e.Layout.Bands[0].Columns[CAPTION_OpenQty].Hidden = false;
            e.Layout.Bands[0].Columns[CAPTION_OrderSideID].Hidden = false;
            e.Layout.Bands[0].Columns[CAPTION_OrderSideID].Header.Caption = CAPTION_OrderSide;
            e.Layout.Bands[0].Columns[CAPTION_ID].Hidden = false;
            BindSides();


        }

        private void grdSplit_CellChange(object sender, CellEventArgs e)
        {
            grdSplit.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdSplit_InitializeRow);
            if (e.Cell.Column.Key == CAPTION_OpenQty)
            {
                if (Convert.ToInt32(e.Cell.Text.ToString()) >= _baseQty || Convert.ToDouble(e.Cell.Text.ToString()) <= 0)
                {
                    e.Cell.CancelUpdate();
                    _bindedList[0].OpenQty = _baseQty;
                }
                else
                {
                    if (_bindedList[1] != null)
                    {
                        _bindedList[1].OpenTotalCommissionandFees = _baseOpenCommissionandFees * (Convert.ToDouble(e.Cell.Text) / _baseQty);
                        //_bindedList[1].ClosedTotalCommissionandFees = _bindedList[0].ClosedTotalCommissionandFees * (Convert.ToDouble(e.Cell.Text) / _baseQty);
                    }
                    _bindedList[0].OpenQty = _baseQty - Convert.ToInt32(e.Cell.Text.ToString());
                    _bindedList[0].OpenTotalCommissionandFees = _baseOpenCommissionandFees * (_bindedList[0].OpenQty / _baseQty);
                    //_bindedList[0].ClosedTotalCommissionandFees = _bindedList[0].ClosedTotalCommissionandFees * (_bindedList[0].OpenQty / _baseQty);


                }

            }


            if (e.Cell.Column.Header.Caption == CAPTION_OrderSide)
            {
                AllocatedTrade taxlot = (AllocatedTrade)e.Cell.Row.ListObject;
                taxlot.Side = e.Cell.Text;
            }
            grdSplit.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdSplit_InitializeRow);


        }



    }
}
