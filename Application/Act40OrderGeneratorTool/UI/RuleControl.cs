using Infragistics.Win;
using Prana.Utilities.UI.MiscUtilities;
using System;
using System.Data;
using System.Windows.Forms;

namespace Act40OrderGeneratorTool
{
    public partial class RuleControl : UserControl
    {
        public RuleControl()
        {
            InitializeComponent();
        }

        internal void SetUp(Rule rule)
        {
            ValueList filterfields = new ValueList();
            foreach (FilterFields op in Enum.GetValues(typeof(FilterFields)))
            {
                filterfields.ValueListItems.Add(op.ToString());
            }

            //Add filter columns
            ultraGridFilterCondition.DisplayLayout.Bands[0].Columns["Column"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ultraGridFilterCondition.DisplayLayout.Bands[0].Columns["Column"].ValueList = filterfields;
            ultraGridFilterCondition.DisplayLayout.Bands[0].Columns["Column"].DefaultCellValue = filterfields.ValueListItems[0].DisplayText;
            Misc.SetUpComboBox(ultraComboPortfolioLimitField, EnumHelper.ConvertEnumForBindingWithCaption(typeof(FilterFields)));

            ValueList filterOperators = new ValueList();
            foreach (FilterOperators op in Enum.GetValues(typeof(FilterOperators)))
            {
                filterOperators.ValueListItems.Add(op.ToString());
            }

            //Add filter conditions
            ultraGridFilterCondition.DisplayLayout.Bands[0].Columns["Condition"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ultraGridFilterCondition.DisplayLayout.Bands[0].Columns["Condition"].ValueList = filterOperators;
            ultraGridFilterCondition.DisplayLayout.Bands[0].Columns["Condition"].DefaultCellValue = filterOperators.ValueListItems[0].DisplayText;
            ultraGridFilterCondition.DisplayLayout.Bands[0].ColHeadersVisible = false;
            ultraGridFilterCondition.DisplayLayout.Bands[0].Columns["Value"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoubleWithSpin;
            ultraGridFilterCondition.DisplayLayout.Bands[0].Columns["Value"].Format = "nnn,nnn,nnn,nnn,nnn.nn";
            ultraGridFilterCondition.DisplayLayout.Bands[0].Columns["Value"].MaskInput = "nnn,nnn,nnn,nnn,nnn.nn";
            ultraGridFilterCondition.DisplayLayout.Bands[0].Columns["Value"].MaxValue = 999999999999999;
            ultraGridFilterCondition.DisplayLayout.Bands[0].Columns["Value"].MinValue = -999999999999999;

            Misc.SetUpComboBox(ultraComboBookSizeOption, EnumHelper.ConvertEnumForBindingWithCaption(typeof(BookSizeOperation)));

            // BInd the rule to UI
            //r.BookSizeOperation = BookSizeOperation.Percentage;
            ultraNumericEditorBookSize.Value = rule.BookSizeFactor;
            ultraNumericEditorPortfolioLimit.Value = rule.PortfolioLimit;
            //r.LimitingField = (FilterFields)Enum.Parse(typeof(FilterFields), ultraComboPortfolioLimitField.Value.ToString(), true);
            ultraComboPortfolioLimitField.Value = rule.LimitingField;
            ultraGridFilterCondition.DataSource = rule.FilterConditions;
            ultraCheckEditor1.Checked = rule.LimitDestination;
            ultraNumericEditorDestinationLimit.Enabled = ultraCheckEditor1.Checked;
            ultraNumericEditorDestinationLimit.Value = rule.DestinationLimit;
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            ultraGridFilterCondition.DisplayLayout.Bands[0].AddNew();
        }

        private void ultraButton2_Click(object sender, EventArgs e)
        {
            if (ultraGridFilterCondition.ActiveRow != null)
                ultraGridFilterCondition.ActiveRow.Delete(false);
        }

        /// <summary>
        /// Returns the UI as a rule
        /// </summary>
        /// <returns></returns>
        internal Rule GetRule()
        {
            ValidateRule();
            Rule r = new Rule();
            r.BookSizeOperation = (BookSizeOperation)Enum.Parse(typeof(BookSizeOperation), ultraComboBookSizeOption.Value.ToString(), true);
            r.BookSizeFactor = Convert.ToDouble(ultraNumericEditorBookSize.Value);
            r.PortfolioLimit = Convert.ToInt32(ultraNumericEditorPortfolioLimit.Value);
            r.LimitingField = (FilterFields)Enum.Parse(typeof(FilterFields), ultraComboPortfolioLimitField.Value.ToString(), true);
            r.FilterConditions = (ultraGridFilterCondition.DataSource as DataTable);
            r.LimitDestination = ultraCheckEditor1.Checked;
            r.DestinationLimit = Convert.ToDouble(ultraNumericEditorDestinationLimit.Value);
            return r;
        }

        private void ValidateRule()
        {
            try
            {
                foreach (DataRow dr in (ultraGridFilterCondition.DataSource as DataTable).Rows)
                {
                    Convert.ToDouble(dr["Value"].ToString());
                }
            }
            catch
            {
                throw new Exception("Invalid rule");
            }
        }

        private void ultraCheckEditor1_CheckStateChanged(object sender, EventArgs e)
        {
            ultraNumericEditorDestinationLimit.Enabled = ultraCheckEditor1.Checked;
        }
    }
}
