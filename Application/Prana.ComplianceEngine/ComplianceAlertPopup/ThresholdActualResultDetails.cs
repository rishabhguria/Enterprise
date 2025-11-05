using Prana.LogManager;
using System;
using System.Data;
using System.Windows.Forms;

namespace Prana.ComplianceEngine.ComplianceAlertPopup
{
    public partial class ThresholdActualResultDetails : UserControl
    {
        public ThresholdActualResultDetails()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the InitializeLayout event of the gridViewThresholdActualResult control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void gridViewThresholdActualResult_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.Bands[0].Columns[ComplainceConstants.CONST_FIELD_NAME].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                e.Layout.Bands[0].Columns[ComplainceConstants.CONST_THRESHOLD].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                e.Layout.Bands[0].Columns[ComplainceConstants.CONST_CAPTION_ACTUAL_RESULT].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                e.Layout.Bands[0].Columns[ComplainceConstants.CONST_FIELD_NAME].CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
                e.Layout.Bands[0].Columns[ComplainceConstants.CONST_THRESHOLD].CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
                e.Layout.Bands[0].Columns[ComplainceConstants.CONST_CAPTION_ACTUAL_RESULT].CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
                e.Layout.Bands[0].Columns[ComplainceConstants.CONST_FIELD_NAME].CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
                e.Layout.Bands[0].Columns[ComplainceConstants.CONST_THRESHOLD].CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
                e.Layout.Bands[0].Columns[ComplainceConstants.CONST_CAPTION_ACTUAL_RESULT].CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        public void BindData(DataTable resultTable)
        {
            try
            {
                gridViewThresholdActualResult.DataSource = resultTable;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
