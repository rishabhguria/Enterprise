using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.PM.Client.UI
{
    public partial class CtrlAppearance : UserControl
    {
        public CtrlAppearance()
        {
            InitializeComponent();
        }

        bool _isRowColorChanged = false;
        public bool IsRowColorChanged
        {
            get { return _isRowColorChanged; }
            set { _isRowColorChanged = value; }
        }

        public void SavePMAppearances(PMAppearances pmAppearance)
        {
            pmAppearance.FontSizeGrid = Convert.ToDecimal(numericUpDownFontSize.Value.ToString());
            pmAppearance.FontSizeDashboard = Convert.ToDecimal(numericUpDownDashboardFont.Value.ToString());
            pmAppearance.ForeColor1 = cpForeLevel1.Color.ToArgb();
            pmAppearance.ForeColor2 = cpForeLevel2.Color.ToArgb();
            pmAppearance.ForeColor3 = cpForeLevel3.Color.ToArgb();
            pmAppearance.ForeColor4 = cpForeLevel4.Color.ToArgb();
            pmAppearance.BackColor1 = cpBackLevel1.Color.ToArgb();
            pmAppearance.BackColor2 = cpBackLevel2.Color.ToArgb();
            pmAppearance.BackColor3 = cpBackLevel3.Color.ToArgb();
            pmAppearance.BackColor4 = cpBackLevel4.Color.ToArgb();
            pmAppearance.SummaryColor = cpSummaryColor.Color.ToArgb();
            pmAppearance.AlternateColor = cpAlternate.Color.ToArgb();
            pmAppearance.RowBgColor = cpRowbgColor.Color.ToArgb();
            pmAppearance.ShowGridLines = GridLineCheck.Checked;
            pmAppearance.WrapHeader = chkboxWrapHeader.Checked;
            pmAppearance.RowSelectorBackColor = cpRowSelectorBack.Color.ToArgb();
            pmAppearance.RowSelectorForColor = cpRowSelectorFore.Color.ToArgb();
            pmAppearance.ShowGridLinesbyGroup = GroupbyGridLineCheck.Checked;
            pmAppearance.ShowNegativeValuesWithBrackets = chkNegativeWithBrackets.Checked;

            pmAppearance.IsDefaultRowBackColor = !chkbxRowBackColor.Checked;
            pmAppearance.IsDefaultAlternateRowColor = !chkbxAlternateRowColor.Checked;
            pmAppearance.IsDefaultDashboardFontSize = !chkbxDashboardFontSize.Checked;
            pmAppearance.IsDefaultGridFontSize = !chkbxGridFontSize.Checked;
            pmAppearance.IsDefaultGroupingColor = !chkbxDefaultGroupColor.Checked;

            if (rbOrderSide.Checked)
            {
                pmAppearance.OrderSideBuyColor = cpBuyPositive.Color.ToArgb();
                pmAppearance.OrderSideSellColor = cpSellNegative.Color.ToArgb();
                pmAppearance.RowColorbasis = rbOrderSide.Tag.ToString();
            }
            if (rbDayPnl.Checked)
            {
                pmAppearance.DayPnlPositiveColor = cpBuyPositive.Color.ToArgb();
                pmAppearance.DayPnlNegativeColor = cpSellNegative.Color.ToArgb();
                pmAppearance.RowColorbasis = rbDayPnl.Tag.ToString();
            }
            if (rbNone.Checked)
            {
                pmAppearance.RowColorbasis = rbNone.Tag.ToString();
            }
            if (rbDefault.Checked)
            {
                pmAppearance.RowColorbasis = rbDefault.Tag.ToString();
            }
        }

        public void InitializeControl(PMAppearances pmAppearance)
        {
            numericUpDownFontSize.Value = pmAppearance.FontSizeGrid;
            numericUpDownDashboardFont.Value = pmAppearance.FontSizeDashboard;
            cpForeLevel1.Color = Color.FromArgb(pmAppearance.ForeColor1);
            cpForeLevel2.Color = Color.FromArgb(pmAppearance.ForeColor2);
            cpForeLevel3.Color = Color.FromArgb(pmAppearance.ForeColor3);
            cpForeLevel4.Color = Color.FromArgb(pmAppearance.ForeColor4);
            cpBackLevel1.Color = Color.FromArgb(pmAppearance.BackColor1);
            cpBackLevel2.Color = Color.FromArgb(pmAppearance.BackColor2);
            cpBackLevel3.Color = Color.FromArgb(pmAppearance.BackColor3);
            cpBackLevel4.Color = Color.FromArgb(pmAppearance.BackColor4);
            cpSummaryColor.Color = Color.FromArgb(pmAppearance.SummaryColor);
            cpRowSelectorBack.Color = Color.FromArgb(pmAppearance.RowSelectorBackColor);
            cpRowSelectorFore.Color = Color.FromArgb(pmAppearance.RowSelectorForColor);
            cpAlternate.Color = Color.FromArgb(pmAppearance.AlternateColor);
            cpRowbgColor.Color = Color.FromArgb(pmAppearance.RowBgColor);
            if (pmAppearance.RowColorbasis.Equals(rbOrderSide.Tag.ToString()))
            {
                rbOrderSide.Checked = true;
                cpSellNegative.Color = Color.FromArgb(pmAppearance.OrderSideSellColor);
                cpBuyPositive.Color = Color.FromArgb(pmAppearance.OrderSideBuyColor);
            }
            else if (pmAppearance.RowColorbasis.Equals(rbDayPnl.Tag.ToString()))
            {
                rbDayPnl.Checked = true;
                cpSellNegative.Color = Color.FromArgb(pmAppearance.DayPnlNegativeColor);
                cpBuyPositive.Color = Color.FromArgb(pmAppearance.DayPnlPositiveColor);
            }
            else if (pmAppearance.RowColorbasis.Equals(rbNone.Tag.ToString()))
            {
                rbNone.Checked = true;
            }
            else if (pmAppearance.RowColorbasis.Equals(rbDefault.Tag.ToString()))
            {
                rbDefault.Checked = true;
            }
            GridLineCheck.Checked = pmAppearance.ShowGridLines;
            chkboxWrapHeader.Checked = pmAppearance.WrapHeader;
            GroupbyGridLineCheck.Checked = pmAppearance.ShowGridLinesbyGroup;
            chkNegativeWithBrackets.Checked = pmAppearance.ShowNegativeValuesWithBrackets;

            chkbxRowBackColor.Checked = !pmAppearance.IsDefaultRowBackColor;
            chkbxAlternateRowColor.Checked = !pmAppearance.IsDefaultAlternateRowColor;
            chkbxDashboardFontSize.Checked = !pmAppearance.IsDefaultDashboardFontSize;
            chkbxGridFontSize.Checked = !pmAppearance.IsDefaultGridFontSize;
            chkbxDefaultGroupColor.Checked = !pmAppearance.IsDefaultGroupingColor;

            cpRowbgColor.Enabled = !pmAppearance.IsDefaultRowBackColor;
            cpAlternate.Enabled = !pmAppearance.IsDefaultAlternateRowColor;
            numericUpDownDashboardFont.Enabled = !pmAppearance.IsDefaultDashboardFontSize;
            numericUpDownFontSize.Enabled = !pmAppearance.IsDefaultGridFontSize;

            cpForeLevel1.Enabled = !pmAppearance.IsDefaultGroupingColor;
            cpForeLevel2.Enabled = !pmAppearance.IsDefaultGroupingColor;
            cpForeLevel3.Enabled = !pmAppearance.IsDefaultGroupingColor;
            cpForeLevel4.Enabled = !pmAppearance.IsDefaultGroupingColor;
            cpBackLevel1.Enabled = !pmAppearance.IsDefaultGroupingColor;
            cpBackLevel2.Enabled = !pmAppearance.IsDefaultGroupingColor;
            cpBackLevel3.Enabled = !pmAppearance.IsDefaultGroupingColor;
            cpBackLevel4.Enabled = !pmAppearance.IsDefaultGroupingColor;
            cpSummaryColor.Enabled = !pmAppearance.IsDefaultGroupingColor;
            cpRowSelectorBack.Enabled = !pmAppearance.IsDefaultGroupingColor;
            cpRowSelectorFore.Enabled = !pmAppearance.IsDefaultGroupingColor;


            numericUpDownDashboardFont.Refresh();
            numericUpDownFontSize.Refresh();
        }

        private void rbOrderSide_CheckedChanged(object sender, EventArgs e)
        {
            _isRowColorChanged = true;
            label6.Text = "Sell";
            label7.Text = "Buy";
            cpBuyPositive.Visible = true;
            cpSellNegative.Visible = true;
        }

        private void rbDayPnl_CheckedChanged(object sender, EventArgs e)
        {
            _isRowColorChanged = true;
            label6.Text = "Negative";
            label7.Text = "Positive";
            cpBuyPositive.Visible = true;
            cpSellNegative.Visible = true;
        }

        private void rbNone_CheckedChanged(object sender, EventArgs e)
        {
            _isRowColorChanged = true;
            label6.Text = "";
            label7.Text = "";
            cpBuyPositive.Visible = false;
            cpSellNegative.Visible = false;
        }

        private void cpBuyPositive_ColorChanged(object sender, EventArgs e)
        {
            _isRowColorChanged = true;
        }

        private void cpSellNegative_ColorChanged(object sender, EventArgs e)
        {
            _isRowColorChanged = true;
        }

        private void chkbxRowBackColor_CheckedChanged(object sender, EventArgs e)
        {
            cpRowbgColor.Enabled = chkbxRowBackColor.Checked;

        }

        private void chkbxAlternateRowColor_CheckedChanged(object sender, EventArgs e)
        {
            cpAlternate.Enabled = chkbxAlternateRowColor.Checked;

        }

        private void chkbxGridFontSize_CheckedChanged(object sender, EventArgs e)
        {

            numericUpDownFontSize.Enabled = chkbxGridFontSize.Checked;
        }

        private void chkbxDashboardFontSize_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownDashboardFont.Enabled = chkbxDashboardFontSize.Checked;
        }

        private void chkbxDefaultGroupColor_CheckedChanged(object sender, EventArgs e)
        {
            cpForeLevel1.Enabled = chkbxDefaultGroupColor.Checked;
            cpForeLevel2.Enabled = chkbxDefaultGroupColor.Checked;
            cpForeLevel3.Enabled = chkbxDefaultGroupColor.Checked;
            cpForeLevel4.Enabled = chkbxDefaultGroupColor.Checked;
            cpBackLevel1.Enabled = chkbxDefaultGroupColor.Checked;
            cpBackLevel2.Enabled = chkbxDefaultGroupColor.Checked;
            cpBackLevel3.Enabled = chkbxDefaultGroupColor.Checked;
            cpBackLevel4.Enabled = chkbxDefaultGroupColor.Checked;
            cpSummaryColor.Enabled = chkbxDefaultGroupColor.Checked;
            cpRowSelectorBack.Enabled = chkbxDefaultGroupColor.Checked;
            cpRowSelectorFore.Enabled = chkbxDefaultGroupColor.Checked;
        }
    }
}
