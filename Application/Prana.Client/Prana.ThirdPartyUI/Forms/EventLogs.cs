using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects.Classes.ThirdParty.Tables;
using Prana.ClientCommon;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;

namespace Prana.ThirdPartyUI.Forms
{
    public partial class EventLogs : Form
    {
        #region Column Titles
        private const string COLUMN_TRANSMISSION_TIME = "TransmissionTime";
        private const string COLUMN_MSG_DESCRIPTION = "MsgDescription";
        private const string COLUMN_MSG_DIRECTION = "MsgDirection";
        private const string COLUMN_ALLOC_ID = "AllocId";
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogs"/> class.
        /// </summary>
        public EventLogs()
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
        /// Set the theme and appearance of EventLogs form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EventLogs_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRANA_TRADING_TICKET);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, "Events Log", CustomThemeHelper.UsedFont);
                CustomThemeHelper.SetThemeProperties(grdEventLog as UltraGrid, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLE_NAME_THIRD_PARTY_CUSTOM);
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
        /// Handles InitializeLayout event of grdEventLog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdEventLog_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = grdEventLog.DisplayLayout.Bands[0];

                band.Columns[COLUMN_TRANSMISSION_TIME].Header.Caption = "EventTime";
                band.Columns[COLUMN_TRANSMISSION_TIME].Header.VisiblePosition = 1;
                band.Columns[COLUMN_TRANSMISSION_TIME].CellActivation = Activation.NoEdit;

                band.Columns[COLUMN_MSG_DESCRIPTION].Header.Caption = "Description";
                band.Columns[COLUMN_MSG_DESCRIPTION].Header.VisiblePosition = 2;
                band.Columns[COLUMN_MSG_DESCRIPTION].CellActivation = Activation.NoEdit;

                band.Columns[COLUMN_MSG_DIRECTION].Header.Caption = "Direction";
                band.Columns[COLUMN_MSG_DIRECTION].Header.VisiblePosition = 3;
                band.Columns[COLUMN_MSG_DIRECTION].CellActivation = Activation.NoEdit;

                band.Columns[COLUMN_ALLOC_ID].Header.Caption = "AllocationId";
                band.Columns[COLUMN_ALLOC_ID].Header.VisiblePosition = 4;
                band.Columns[COLUMN_ALLOC_ID].CellActivation = Activation.NoEdit;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Event handler for the Click event of the grdEventLog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdEventLog_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                if (grdEventLog.ActiveRow != null)
                {
                    ThirdPartyBatchEventData eventData = (ThirdPartyBatchEventData)grdEventLog.ActiveRow.ListObject;
                    if (eventData != null)
                        txtFixMessage.Text = eventData.FixMsg;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This method is to load the grid data
        /// </summary>
        /// <param name="blockId"></param>
        public void LoadData(int blockId)
        {
            try
            {
                grdEventLog.DataSource = ThirdPartyClientManager.ServiceInnerChannel.GetThirdPartyBatchEventData(blockId);
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
