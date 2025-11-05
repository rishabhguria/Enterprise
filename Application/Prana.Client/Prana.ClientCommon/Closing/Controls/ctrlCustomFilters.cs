using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.ClientCommon
{
    public partial class ctrlCustomFilters : UserControl
    {


        const string COL_COLUMNNAME = "ColumnName";
        const string COL_CONDITIONTYPE = "ConditionOperatorType";
        const string COL_CONDITIONVALUE = "ConditionValue";

        //   const string COL_ISCOMMASEPARATED = "IsCommaSeparated";


        public ctrlCustomFilters()
        {
            try
            {
                InitializeComponent();
                this.InitializeUI();
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



        private void InitializeUI()
        {
            try
            {
                this.ultraGrid1.UseAppStyling = false;
                _conditionWrapperList.Add(new CustomCondition());
                this.ultraGrid1.SyncWithCurrencyManager = false;
                this.ultraGrid1.Rows.ExpandAll(true);
                this.ultraGrid1.DataSource = _conditionWrapperList;
                this.ultraGrid1.DisplayLayout.Bands[0].Columns[COL_COLUMNNAME].Header.Caption = "Condition Column";
                this.ultraGrid1.DisplayLayout.Bands[0].Columns[COL_CONDITIONTYPE].Header.Caption = "Operator Type";
                // this.ultraGrid1.DisplayLayout.Bands[0].Columns[COL_CONDITIONVALUE].Header.Caption = "Value";

                this.ultraGrid1.DisplayLayout.Bands[0].Columns[COL_COLUMNNAME].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                this.ultraGrid1.DisplayLayout.Bands[0].Columns[COL_CONDITIONTYPE].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ultraGrid1.DisplayLayout.Override.SupportDataErrorInfo = SupportDataErrorInfo.RowsAndCells;
                BindColumnTypes();

                BindConditionTypes();
                //  HideColumns();

                if (this.ultraGrid1.Rows.Count > 0)
                    this.ultraGrid1.Rows[0].Activate();
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

        //private void HideColumns()
        //{
        //  //  this.ultraGrid1.DisplayLayout.Bands[0].Columns[COL_CONDITIONVALUE].Hidden = true;
        //   // this.ultraGrid1.DisplayLayout.Bands[0].Columns[COL_ISCOMMASEPARATED].Hidden = true;
        //}

        private void BindConditionTypes()
        {
            try
            {
                UltraGridColumn colConditionType = ultraGrid1.DisplayLayout.Bands[0].Columns[COL_CONDITIONTYPE];
                colConditionType.ValueList = GetOperatorsValueList();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            //colConditionType.ValueList = conditionTypes;
        }

        private void BindColumnTypes()
        {
            try
            {
                ValueList columnTypes = new ValueList();
                UltraGridColumn colColumnType = ultraGrid1.DisplayLayout.Bands[0].Columns[COL_COLUMNNAME];
                columnTypes.ValueListItems.Add("Symbol", "Symbol");
                columnTypes.ValueListItems.Add("UnderLyingSymbol", "UnderLying");
                columnTypes.ValueListItems.Add("SEDOLSymbol", "Sedol");
                columnTypes.ValueListItems.Add("BloombergSymbol", "Bloomberg");
                colColumnType.ValueList = columnTypes;

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

        public static Infragistics.Win.ValueList GetOperatorsValueList()
        {
            ValueList valueList = null;
            try
            {
                valueList = new ValueList();
                valueList.DisplayStyle = ValueListDisplayStyle.DisplayTextAndPicture;
                valueList.DropDownResizeHandleStyle = DropDownResizeHandleStyle.None;
                Prana.BusinessObjects.AppConstants.EnumDescriptionAttribute.ConditionOperator[] conditionOperatorArray = (Prana.BusinessObjects.AppConstants.EnumDescriptionAttribute.ConditionOperator[])Enum.GetValues(typeof(Prana.BusinessObjects.AppConstants.EnumDescriptionAttribute.ConditionOperator));
                for (int index = 0; index < conditionOperatorArray.Length; ++index)
                {
                    string str = ((object)conditionOperatorArray[index]).ToString();
                    valueList.ValueListItems.Add((object)conditionOperatorArray[index], str);
                }
                return valueList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return valueList;
        }


        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridLayout layout = e.Layout;
                UltraGridOverride @override = layout.Override;
                layout.AutoFitStyle = AutoFitStyle.ExtendLastColumn;
                layout.InterBandSpacing = 0;
                layout.ScrollBounds = ScrollBounds.ScrollToFill;
                layout.RowConnectorStyle = RowConnectorStyle.None;
                layout.ScrollBounds = ScrollBounds.ScrollToFill;
                layout.ScrollStyle = ScrollStyle.Immediate;
                layout.MaxColScrollRegions = 1;
                layout.MaxRowScrollRegions = 1;
                @override.RowSelectors = DefaultableBoolean.False;
                @override.RowSizing = RowSizing.AutoFree;
                @override.SummaryFooterCaptionVisible = DefaultableBoolean.False;
                @override.SummaryFooterSpacingAfter = 0;
                @override.SummaryFooterSpacingBefore = 0;
                @override.TipStyleScroll = TipStyle.Hide;
                @override.MinRowHeight = 25;
                @override.AllowColMoving = AllowColMoving.NotAllowed;
                @override.AllowColSizing = AllowColSizing.None;
                // e.Layout.Bands[0].Columns[COL_CONDITIONVALUE].CellAppearance.TextHAlign = HAlign.Left;
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

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _conditionWrapperList.Add(new CustomCondition());
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UltraGridRow activeRow = this.ultraGrid1.ActiveRow;
            if (activeRow == null)
                return;
            UltraGridRow ultraGridRow = !activeRow.HasPrevSibling(false, true) ? (!activeRow.HasNextSibling(false, true) ? activeRow.ParentRow : activeRow.GetSibling(SiblingRow.Next, false, false, false)) : activeRow.GetSibling(SiblingRow.Previous, false, false, false);
            activeRow.Delete(false);
            if (ultraGridRow != null)
                ultraGridRow.Activate();
        }

        //private void ultraGrid1_Enter(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (MouseButtons.Left == (Control.MouseButtons & MouseButtons.Left))
        //            return;
        //        UltraGrid ultraGrid = (UltraGrid)sender;
        //        if (ultraGrid.Rows.Count == 0)
        //            return;
        //        UltraGridRow ultraGridRow = ultraGrid.ActiveRow ?? ultraGrid.Rows[0];
        //        if (ultraGrid.ActiveCell == null)
        //            ultraGridRow.Cells["CustomCondition"].Activate();
        //        ultraGrid.PerformAction(UltraGridAction.EnterEditMode, false, false);
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




        ///// <summary>
        ///// Processes a command key.
        ///// 
        ///// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if ((keyData) == Keys.Delete)
                {
                    Control activeControl = this.ActiveControl;
                    for (ContainerControl containerControl = activeControl as ContainerControl; containerControl != null; containerControl = activeControl as ContainerControl)
                        activeControl = containerControl.ActiveControl;
                    if (activeControl == null || activeControl.Name != "OperatorConditionValueEditor" && activeControl.Parent.Name != "OperatorConditionValueEditor")
                    {
                        this.deleteToolStripMenuItem_Click((object)this, EventArgs.Empty);
                        return true;
                    }
                }
                else if ((keyData) == Keys.Insert)
                {
                    Control activeControl = this.ActiveControl;
                    for (ContainerControl containerControl = activeControl as ContainerControl; containerControl != null; containerControl = activeControl as ContainerControl)
                        activeControl = containerControl.ActiveControl;
                    if (activeControl == null || activeControl.Name != "OperatorConditionValueEditor" && activeControl.Parent.Name != "OperatorConditionValueEditor")
                    {
                        this.addToolStripMenuItem_Click((object)this, EventArgs.Empty);
                        return true;
                    }
                }
                return base.ProcessCmdKey(ref msg, keyData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        private List<CustomCondition> GetUpdatedConditionList()
        {
            List<CustomCondition> list = null;
            try
            {
                list = new List<CustomCondition>();
                //_dictClosingPreferences.Clear();
                foreach (UltraGridRow row in ultraGrid1.Rows)
                {
                    CustomCondition customCondition = row.ListObject as CustomCondition;

                    list.Add(customCondition);


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

            return list;
        }


        BindingList<CustomCondition> _conditionWrapperList = new BindingList<CustomCondition>();
        private void BindData(List<CustomCondition> conditions)
        {
            try
            {
                this._conditionWrapperList.Clear();

                //  List<CustomCondition> listConditions = new List<CustomCondition>();
                foreach (CustomCondition condition in conditions)
                {
                    _conditionWrapperList.Add(condition);

                }

                this.ultraGrid1.Refresh();
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


        public void LoadDataForTemplate(ClosingTemplate template)
        {
            try
            {
                List<CustomCondition> conditionList = new List<CustomCondition>();
                foreach (KeyValuePair<string, List<CustomCondition>> kp in template.DictCustomConditions)
                {
                    conditionList.AddRange(kp.Value);
                }
                //_dictClosingPreferences = template.DictCustomConditions;
                BindData(conditionList);
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

        public void UpdateDataForTemplate(ClosingTemplate closingTemplate)
        {
            try
            {
                closingTemplate.DictCustomConditions.Clear();
                List<CustomCondition> updatedConditions = GetUpdatedConditionList();
                foreach (CustomCondition customCondition in updatedConditions)
                {

                    string columnName = customCondition.ColumnName;
                    if (!closingTemplate.DictCustomConditions.ContainsKey(columnName))
                    {

                        List<CustomCondition> conditionListNew = new List<CustomCondition>();
                        conditionListNew.Add(customCondition);
                        closingTemplate.DictCustomConditions.Add(columnName, conditionListNew);
                    }
                    else
                    {
                        closingTemplate.DictCustomConditions[columnName].Add(customCondition);
                    }
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

        private void ultraGrid1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                Point pt = ultraGrid1.PointToScreen(e.Location);
                if (e.Button == MouseButtons.Right)
                {
                    contextMenuStrip.Show(pt);
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


        internal void LoadCustomFilters(Prana.BusinessObjects.ClosingTemplate template)
        {
            try
            {
                LoadDataForTemplate(template);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        internal void UpdateCustomFilters(Prana.BusinessObjects.ClosingTemplate closingTemplate)
        {
            try
            {

                UpdateDataForTemplate(closingTemplate);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
