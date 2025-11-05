using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.PricingService2UI.FactSet
{
    public partial class FactSetMonitoringUI : Form
    {
        FactSetProperties _factSetProperties = null;

        public FactSetMonitoringUI()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private async void ultraButtonSubscribedSymbols_Click(object sender, EventArgs e)
        {
            try
            {
                List<FactSetSymbolResponse> listFactSetSymbolResponse = await PricingService2Manager.PricingService2Manager.GetInstance.GetAllFactSetSymbolInformation();

                if (listFactSetSymbolResponse != null)
                {
                    ultraGridSubscribedSymbols.DataSource = listFactSetSymbolResponse;
                    ultraGridSubscribedSymbols.DataBind();
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private async void refreshFactSetSymbolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ultraGridSubscribedSymbols.ActiveRow != null)
                {
                    string tickerSymbol = ((FactSetSymbolResponse)ultraGridSubscribedSymbols.ActiveRow.ListObject).TickerSymbol;

                    await PricingService2Manager.PricingService2Manager.GetInstance.DeleteAdvisedSymbol(tickerSymbol);
                    await PricingService2Manager.PricingService2Manager.GetInstance.RefreshFactSetSymbolInformation(tickerSymbol);
                    ultraButtonSubscribedSymbols_Click(null, null);

                    foreach (UltraGridRow row in ultraGridSubscribedSymbols.Rows)
                    {
                        if (Convert.ToString(row.Cells["TickerSymbol"].Value).Equals(tickerSymbol))
                        {
                            ultraGridSubscribedSymbols.ActiveRow = row;
                            break;
                        }
                    }

                    toolStripStatusLabel.Text = "[" + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss") + "] Data Refreshed.";
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ultraGridSubscribedSymbols.ActiveRow != null)
                {
                    FactSetSymbolResponse factSetSymbolResponse = ((FactSetSymbolResponse)ultraGridSubscribedSymbols.ActiveRow.ListObject);

                    Clipboard.SetText(string.Format("Ticker Symbol: {0}, FactSet Symbol: {1}", factSetSymbolResponse.TickerSymbol, factSetSymbolResponse.FactSetSymbol));

                    toolStripStatusLabel.Text = "[" + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss") + "] Data Copied.";
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private void ultraButtonConnectionProperties_Click(object sender, EventArgs e)
        {
            try
            {
                _factSetProperties = new FactSetProperties();
                _factSetProperties.CredentialsUpdated += factSetProperties_CredentialsUpdated;
                _factSetProperties.FormClosed += factSetProperties_FormClosed;
                _factSetProperties.ShowDialog();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void factSetProperties_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                _factSetProperties.CredentialsUpdated -= factSetProperties_CredentialsUpdated;
                _factSetProperties.FormClosed -= factSetProperties_FormClosed;
                _factSetProperties = null;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void factSetProperties_CredentialsUpdated(object sender, EventArgs<List<string>> e)
        {
            try
            {
                PricingService2Manager.PricingService2Manager.GetInstance.RestartLiveFeed().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ultraGridSubscribedSymbols_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    System.Drawing.Point mousePoint = new System.Drawing.Point(e.X, e.Y);
                    UIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(mousePoint);
                    if (element == null)
                    {
                        ultraGridSubscribedSymbols.ActiveRow = null;
                    }

                    UltraGridCell cell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
                    if (cell != null)
                    {
                        cell.Row.Activate();
                        cell.Row.Selected = true;
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
    }
}
