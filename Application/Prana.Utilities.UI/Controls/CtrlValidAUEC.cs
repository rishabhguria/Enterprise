using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
//using Prana.CommonDataCache;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
//using Prana.Tools;
using Prana.BusinessObjects;

namespace Prana.Utilities
{
    public partial class CtrlValidAUEC : UserControl
    {

        public delegate void AUECSelectedHandler(ValidAUEC validAuec);
        public event AUECSelectedHandler ValidAUECSelected;
        public event EventHandler CloseForm;


        public CtrlValidAUEC()
        {
            InitializeComponent();
        }

        public void SetUP()
        {
            BindAuec();
            BindGrid(dictAuec);

        }

        //Bind all AUECS in the dictionary...
        Dictionary<int, ValidAUEC> dictAuec = new Dictionary<int, ValidAUEC>();
        private void BindAuec()
        {
            try
            {
                CachedDataManager cachedDataManager = CachedDataManager.GetInstance;

                Dictionary<int, string> dictAuecs = cachedDataManager.GetAllAuecs();

                foreach (KeyValuePair<int, string> kvpAuec in dictAuecs)
                {
                    string[] auecdetails = (kvpAuec.Value).Split(',');
                    int auecID = kvpAuec.Key;
                    ValidAUEC auecdetailwise = new ValidAUEC();

                    auecdetailwise.AssetID = int.Parse(auecdetails[0].ToString());
                    auecdetailwise.UnderlyingID = int.Parse(auecdetails[1].ToString());
                    auecdetailwise.ExchangeID = int.Parse(auecdetails[2].ToString());
                    auecdetailwise.CurrencyID = int.Parse(auecdetails[3].ToString());

                    auecdetailwise.Asset = cachedDataManager.GetAssetText(auecdetailwise.AssetID);
                    auecdetailwise.Underlying = cachedDataManager.GetUnderLyingText(auecdetailwise.UnderlyingID);
                    auecdetailwise.Exchange = cachedDataManager.GetExchangeText(auecdetailwise.ExchangeID);
                    auecdetailwise.DefaultCurrency = cachedDataManager.GetCurrencyText(auecdetailwise.CurrencyID);
                    auecdetailwise.ExchangeIdentifier = cachedDataManager.GetAUECText(auecID);


                    dictAuec.Add(auecID, auecdetailwise);

                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BindGrid(Dictionary<int, ValidAUEC> dictAuecDetails)
        {
            grdAuec.DataSource = null;
            List<ValidAUEC> auec = new List<ValidAUEC>();
            foreach (KeyValuePair<int, ValidAUEC> auecDetails in dictAuecDetails)
            {
                auec.Add(auecDetails.Value);
            }
            grdAuec.DataSource = auec;
            SetGridColumns(grdAuec);
            AddCheckBox(grdAuec);
        }

        private void AddCheckBox(UltraGrid grid)
        {
            grid.DisplayLayout.Bands[0].Columns.Add("checkBox", "Select");
            grid.DisplayLayout.Bands[0].Columns["checkBox"].DataType = typeof(bool);
            grid.DisplayLayout.Bands[0].Columns["checkBox"].CellClickAction = CellClickAction.CellSelect;
            grid.DisplayLayout.Bands[0].Columns["checkBox"].Hidden = false;
            grid.DisplayLayout.Bands[0].Columns["checkBox"].Header.VisiblePosition = 0;
            grid.DisplayLayout.Bands[0].Columns["checkBox"].Width = 35;
            grid.DisplayLayout.Bands[0].Columns["checkBox"].AllowRowFiltering = DefaultableBoolean.False;
            grid.DisplayLayout.Bands[0].Columns["checkBox"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

        }

        //private void ValidAUECs_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    _validAuecs = null;
        //    _symbolLookUp.AUEC = null;
        //}

        private void SelectedRow()
        {
            try
            {
                ValidAUEC loadAuec = null;
                if (grdAuec.ActiveRow != null)
                {
                    if (grdAuec.ActiveCell != null && grdAuec.ActiveCell.Column.Key == "checkBox")
                    {
                        loadAuec = (ValidAUEC)grdAuec.ActiveRow.ListObject;
                        //_symbolLookUp.AUEC = loadAuec;
                        grdAuec.ActiveCell = null;
                        if (ValidAUECSelected != null)
                        {
                            ValidAUECSelected(loadAuec);
                        }
                        //this.Hide();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdAuec_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SelectedRow();
            }
        }

        private void grdAuec_KeyDown(object sender, KeyEventArgs e)
        {
            //Press space to select AUEC rather than clicking checkbox by mouse.
            if (e.KeyData.Equals(Keys.Space))
            {
                grdAuec.ActiveCell = grdAuec.ActiveRow.Cells["checkbox"];
                SelectedRow();
            }
        }

        //private void ValidAUECs_Load(object sender, EventArgs e)
        //{
        //    txtSearch.Focus();
        //}

        private void btnSkip_Click(object sender, EventArgs e)
        {
            if (CloseForm != null)
            {
                CloseForm(null,null);
            }

           // this.Close();
        }


        private void btnGetData_Click(object sender, EventArgs e)
        {
            //Search the text in the AUECS..
            string txt = txtSearch.Text.Trim().ToUpperInvariant();
            Dictionary<int, ValidAUEC> dictauecs = new Dictionary<int, ValidAUEC>();
            try
            {
                //Empty textbox will load all AUECS...
                if (txt.Equals(string.Empty))
                {
                    BindGrid(dictAuec);
                }
                else
                {
                    //Text without space search...
                    if (!txt.Contains(" "))
                    {
                        foreach (KeyValuePair<int, ValidAUEC> auecdetails in dictAuec)
                        {
                            if (auecdetails.Value.Asset.ToUpperInvariant().Contains(txt) || auecdetails.Value.Exchange.ToUpperInvariant().Contains(txt) || auecdetails.Value.Underlying.ToUpperInvariant().Contains(txt) || auecdetails.Value.DefaultCurrency.ToUpperInvariant().Contains(txt) || auecdetails.Value.ExchangeIdentifier.ToUpperInvariant().Contains(txt))
                            {
                                dictauecs.Add(auecdetails.Key, auecdetails.Value);
                            }
                        }

                    }
                    else
                    {
                        // Multiple Search like (Equity Nasdaq)....
                        string[] txtDetails = txt.Split(' ');
                        foreach (string text in txtDetails)
                        {
                            foreach (KeyValuePair<int, ValidAUEC> auecdetails in dictAuec)
                            {
                                if (auecdetails.Value.Asset.ToUpperInvariant().Contains(text) || auecdetails.Value.Exchange.ToUpperInvariant().Contains(text) || auecdetails.Value.Underlying.ToUpperInvariant().Contains(text) || auecdetails.Value.DefaultCurrency.ToUpperInvariant().Contains(text) || auecdetails.Value.ExchangeIdentifier.ToUpperInvariant().Contains(text))
                                {
                                    if (!dictauecs.ContainsKey(auecdetails.Key))
                                    {
                                        dictauecs.Add(auecdetails.Key, auecdetails.Value);
                                    }
                                }
                            }
                        }
                    }
                    BindGrid(dictauecs);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetGridColumns(UltraGrid grid)
        {
            ColumnsCollection columns = grid.DisplayLayout.Bands[0].Columns;

            foreach (UltraGridColumn column in columns)
            {
                string caption = column.Header.Caption;
                if (caption.Equals("Asset") || caption.Equals("Underlying") || caption.Equals("Exchange") || caption.Equals("DefaultCurrency") || caption.Equals("ExchangeIdentifier"))
                {
                    column.Hidden = false;
                }
                else
                {
                    column.Hidden = true;
                }

            }
        }

        //public Prana.Tools.SymbolLookUp SaveAuec()
        //{
        //    return _symbolLookUp;
        //}

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData.Equals(Keys.Enter))
            {
                btnGetData_Click(this.btnGetData, e);
                e.Handled = true;
            }
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            txtSearch.SelectAll();
        }

        

        private void CtrlValidAUEC_Load(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }

       
       

     
    }
}
