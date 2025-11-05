using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Infragistics.Win.UltraWinGrid;

namespace Prana.RuleEngine
{
    public partial class PortfolioGrpControl : System.Windows.Forms.UserControl
    {
        public PortfolioGrpControl()
        {
            InitializeComponent();
            SetGridColumns();
        }

        private void SetGridColumns()
        {
            try
            {

                DataTable dt = new DataTable("Source");
                dt.Columns.Add("Fund");
                dt.Columns.Add("Symbol");
                dt.Columns.Add("Global");
                dt.Columns.Add("Taxlot");
                dt.Columns.Add("AssetClass");
                dt.Columns.Add("Underlying");
                dt.Columns.Add("Exchange");
                dt.Columns.Add("Currency");
                dt.Columns.Add("TradeCurrency");
               
                dt.Columns.Add("UDAAssetClass");
                dt.Columns.Add("SecurityType");
                dt.Columns.Add("Sector");
                dt.Columns.Add("Subsector");
                dt.Columns.Add("Country");
                dt.Columns.Add("MasterFund");
                dt.Columns.Add("Strategy");
                dt.Columns.Add("MasterStrategy");
                dt.Columns.Add("PositionSide");
                dt.Columns.Add("PrimeBroker");
                dt.Columns.Add("ExecutingBroker ");
                dt.Columns.Add("OrderSide");
                GridPortfolioGrp.DataSource = dt;
                GridPortfolioGrp.DataBind();
               // GridPortfolioGrp.Enabled = false;
              
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void GridPortfolioGrp_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            


        }

        internal void SetCompressionViewInGrid(string Compression)
        {
            try
            {
                GridPortfolioGrp.DisplayLayout.Bands[0].SortedColumns.Clear();
                if (Compression != "")
                {
                    String[] list = Compression.Split(' ');
                    foreach(String item in list)
                    {
                        GridPortfolioGrp.DisplayLayout.Bands[0].SortedColumns.Add(item, false, true);
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        internal void ClearGrouping()
        {
            GridPortfolioGrp.DisplayLayout.Bands[0].SortedColumns.Clear();
        }

    }
}
