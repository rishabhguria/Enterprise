using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Constants;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Windows.Forms;

namespace Prana.PositionManagement
{
    public partial class frmPositionMgmtMain : Form, ISnapShotPositionManagement, IPublishing
    {
        public frmPositionMgmtMain()
        {
            InitializeComponent();
            Disposed += new EventHandler(frmPositionMgmtMain_Disposed);
            CreateSubscriptionServicesProxy();
            GetOpenTaxlots(DateTime.Now);

            CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RISK_MANAGEMENT);
            if (CustomThemeHelper.ApplyTheme)
            {
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
            }
        }

        #region ISnapShotPositionManagement Members
        void frmPositionMgmtMain_Disposed(object sender, EventArgs e)
        {
            try
            {
                if (FormClosedHandler != null)
                {
                    FormClosedHandler(this, e);
                }
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

        public Form Reference()
        {
            return this;
        }

        public event EventHandler FormClosedHandler;

        public void SetUp()
        {
        }

        public event EventHandler LaunchPreferences;
        #endregion

        #region UI Section
        private void BindGrid(UltraGrid grid, GenericBindingList<TaxLot> source)
        {
            try
            {
                if (!this.IsDisposed)
                {
                    grid.DataSource = source;
                    grid.DataBind();

                    UltraGridBand band = grid.DisplayLayout.Bands[0];
                    UltraWinGridUtils.HideColumns(band);
                    band.Columns["Symbol"].Hidden = false;
                    band.Columns["Quantity"].Hidden = false;
                    band.Columns["AvgPrice"].Hidden = false;
                    band.Columns["AUECLocalDate"].Hidden = false;
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

        private void GetOpenTaxlots(DateTime givenDate)
        {
            try
            {
                PositionDataManager.GetInstance().GetOpenPositionsDAL(givenDate);
                PositionDataManager.GetInstance().GetTransactionsDAL(givenDate);
                foreach (TaxLot t in PositionDataManager.GetInstance().OpenTransactions)
                {
                    PositionDataManager.GetInstance().OpenPositions.Add(t);
                }
                BindGrid(grdOpenPositions, PositionDataManager.GetInstance().OpenPositions);
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
        #endregion

        #region Subscribe Section

        private DuplexProxyBase<ISubscription> _proxy;
        private void CreateSubscriptionServicesProxy()
        {
            try
            {
                _proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
                _proxy.Subscribe(Topics.Topic_Allocation, null);
                _proxy.Subscribe(Topics.Topic_Closing, null);
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

        #endregion

        #region IPublishing Members
        public delegate void UIThreadMarshallerPublish(MessageData data, string topic);
        public void Publish(MessageData e, string topicName)
        {
            try
            {
                if (e != null && topicName != null)
                {
                    UIThreadMarshallerPublish mi = new UIThreadMarshallerPublish(Publish);
                    if (UIValidation.GetInstance().validate(this))
                    {
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke(mi, new object[] { e, topicName });
                        }
                        else
                        {
                            Object[] publishDataList = null;
                            if (topicName == Topics.Topic_Allocation || topicName == Topics.Topic_Closing)
                            {
                                publishDataList = (Object[])e.EventData;
                                foreach (Object obj in publishDataList)
                                {
                                    TaxLot taxlot = (TaxLot)obj;

                                    if (taxlot.TaxLotState == ApplicationConstants.TaxLotState.New && taxlot.Level1ID != 0)
                                    {
                                        PositionDataManager.GetInstance().OpenPositions.Add(taxlot);
                                    }
                                    if (taxlot.TaxLotState == ApplicationConstants.TaxLotState.Deleted)
                                    {
                                        PositionDataManager.GetInstance().OpenPositions.Remove(taxlot);
                                    }
                                    if (taxlot.TaxLotState == ApplicationConstants.TaxLotState.Updated)
                                    {
                                        PositionDataManager.GetInstance().OpenPositions.Update(taxlot);
                                    }
                                    BindGrid(grdOpenPositions, PositionDataManager.GetInstance().OpenPositions);
                                }

                            }
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

        public string getReceiverUniqueName()
        {
            return "frmPositionMgmtMain";
        }

        #endregion

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetOpenTaxlots(DateTime.Now);
        }

        private void ultraButtonPreferenecs_Click(object sender, EventArgs e)
        {
            try
            {
                if (LaunchPreferences != null)
                    LaunchPreferences(this, e);
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
    }
}