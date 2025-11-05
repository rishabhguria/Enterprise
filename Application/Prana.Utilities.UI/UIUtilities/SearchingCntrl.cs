using Infragistics.Win;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public partial class SearchingCntrl : UserControl
    {
        //public delegate void RemoveConditionEventHandler(String ControlName);
        //public event RemoveConditionEventHandler RemoveConditionEvent;
        public event EventHandler<EventArgs<string>> RemoveConditionEvent;

        //public delegate void GetColumnTypeEventHander(String SearchParam, object o);
        //public event GetColumnTypeEventHander GetColumnTypeReq;
        public event EventHandler<EventArgs<string>> GetColumnTypeReq;

        String _fieldType = string.Empty;
        public SearchingCntrl()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnRemoveCondition.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRemoveCondition.ForeColor = System.Drawing.Color.White;
                btnRemoveCondition.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRemoveCondition.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRemoveCondition.UseAppStyling = false;
                btnRemoveCondition.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void btnRemoveCondition_Click(object sender, EventArgs e)
        {
            try
            {
                if (RemoveConditionEvent != null)
                {
                    RemoveConditionEvent(this, new EventArgs<string>(this.Name));
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

        public void SetUp(List<EnumerationValue> valueList)
        {
            try
            {
                cmbbxSearchCriteria.DataSource = valueList;
                cmbbxSearchCriteria.DataBind();
                cmbbxSearchCriteria.DisplayMember = "DisplayText";
                cmbbxSearchCriteria.ValueMember = "Value";
                //cmbBxSearchCriteria.Value = SecMasterConstants.SearchCriteria.CompanyName;
                cmbbxSearchCriteria.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;

                cmbBxIntMachOn.DataSource = EnumHelper.ConvertEnumForBindingWithCaption(typeof(SecMasterConstants.SearchIntMatchOn));
                cmbBxIntMachOn.DataBind();
                cmbBxIntMachOn.DisplayMember = "DisplayText";
                cmbBxIntMachOn.ValueMember = "Value";
                cmbBxIntMachOn.Value = SecMasterConstants.SearchIntMatchOn.EqualTo;
                cmbBxIntMachOn.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;

                cmbBxTruFalse.DataSource = EnumHelper.ConvertEnumForBindingWithCaption(typeof(SecMasterConstants.EnumTrueFalse));
                cmbBxTruFalse.DataBind();
                cmbBxTruFalse.DisplayMember = "DisplayText";
                cmbBxTruFalse.ValueMember = "Value";
                cmbBxTruFalse.Value = SecMasterConstants.EnumTrueFalse.True;
                cmbBxTruFalse.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;

                cmbbxMatchOn.DataSource = EnumHelper.ConvertEnumForBindingWithCaption(typeof(SecMasterConstants.SearchMatchOn));
                cmbbxMatchOn.DataBind();
                cmbbxMatchOn.DisplayMember = "DisplayText";
                cmbbxMatchOn.ValueMember = "Value";
                cmbbxMatchOn.Value = SecMasterConstants.SearchMatchOn.Contains;
                cmbbxMatchOn.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;

                cmbBxAndOr.DataSource = EnumHelper.ConvertEnumForBindingWithCaption(typeof(SecMasterConstants.SearchAndOr));
                cmbBxAndOr.DataBind();
                cmbBxAndOr.DisplayMember = "DisplayText";
                cmbBxAndOr.ValueMember = "Value";
                cmbBxAndOr.Value = SecMasterConstants.SearchAndOr.Or;
                cmbBxAndOr.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);

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

        public SearchCondition GetSearchCondition()
        {
            SearchCondition condition = new SearchCondition();

            try
            {
                if (cmbbxSearchCriteria.Text != "")
                {
                    condition.AndOr = cmbBxAndOr.Text;
                    condition.Operator = " = ";
                    condition.FieldName = cmbbxSearchCriteria.Value.ToString();

                    if (_fieldType != null)
                    {
                        switch (_fieldType)
                        {
                            case "Enum":
                                condition.FieldValue = cmbBxValueLists.Value.ToString();
                                break;

                            case "String":
                                GetValueNOperatorForSTRING(condition);
                                break;

                            case "DateTime":
                                condition.FieldValue = dateTimeCntrl.Value.ToString();
                                condition.Operator = GetOperatorForNumeric();
                                break;

                            case "Int32":
                            case "Int64":
                            case "Double":
                            case "Single":
                            case "Decimal":
                                condition.FieldValue = numEditor.Value.ToString();
                                condition.Operator = GetOperatorForNumeric();
                                break;

                            case "Boolean":
                                condition.FieldValue = cmbBxTruFalse.Text;
                                break;

                            default:
                                condition.FieldValue = txtbxInput.Text.Trim();
                                break;

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
            return condition;
        }

        private void GetValueNOperatorForSTRING(SearchCondition condition)
        {
            String text = txtbxInput.Text.Trim();
            //condition.Operator = " like ";
            condition.FieldValue = text;
            if (cmbbxMatchOn.Value.ToString().Equals(SecMasterConstants.SearchMatchOn.Contains.ToString()))
            {
                condition.FieldValue = "%" + text + "%";
                condition.Operator = " like ";
            }
            else if (cmbbxMatchOn.Value.ToString().Equals(SecMasterConstants.SearchMatchOn.StartsWith.ToString()))
            {
                condition.FieldValue = text + "%";
                condition.Operator = " like ";
            }
        }

        private string GetOperatorForNumeric()
        {
            String oprtr = " = ";
            SecMasterConstants.SearchIntMatchOn MatchOn = (SecMasterConstants.SearchIntMatchOn)Enum.Parse(typeof(SecMasterConstants.SearchIntMatchOn), cmbBxIntMachOn.Value.ToString());
            switch (MatchOn)
            {
                case SecMasterConstants.SearchIntMatchOn.EqualTo:
                    oprtr = " = ";
                    break;
                case SecMasterConstants.SearchIntMatchOn.NotEqualTo:
                    oprtr = " != ";
                    break;
                case SecMasterConstants.SearchIntMatchOn.greaterThan:
                    oprtr = " > ";
                    break;
                case SecMasterConstants.SearchIntMatchOn.LessThan:
                    oprtr = " < ";
                    break;
                case SecMasterConstants.SearchIntMatchOn.greaterThanOrEqualTo:
                    oprtr = " >= ";
                    break;
                case SecMasterConstants.SearchIntMatchOn.LessThanOrEqualTo:
                    oprtr = " <= ";
                    break;

            }
            return oprtr;
        }

        private void cmbbxSearchCriteria_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (GetColumnTypeReq != null)
                {
                    GetColumnTypeReq(this, new EventArgs<string>(cmbbxSearchCriteria.Value.ToString()));
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

        /// <summary>
        /// Set Condition Controls based on field type  like string, Int, Enum
        /// </summary>
        /// <param name="FieldType"></param>
        public void SetConditionControls(String FieldType, ValueList valueList)
        {
            cmbBxValueLists.Visible = false;
            cmbbxMatchOn.Visible = false;
            lblEqual.Visible = false;
            txtbxInput.Visible = false;
            cmbBxTruFalse.Visible = false;
            dateTimeCntrl.Visible = false;
            cmbBxIntMachOn.Visible = false;
            numEditor.Visible = false;
            _fieldType = FieldType;

            try
            {
                switch (FieldType)
                {
                    case "Enum":

                        cmbBxValueLists.DataSource = valueList.ValueListItems;
                        cmbBxValueLists.DataBind();
                        cmbBxValueLists.DisplayMember = "DisplayText";
                        cmbBxValueLists.ValueMember = "DataValue";
                        cmbBxValueLists.DisplayLayout.Bands[0].Columns["DisplayText"].Width = cmbBxValueLists.Width;
                        cmbBxValueLists.DisplayLayout.Bands[0].Columns["DataValue"].Hidden = true;
                        cmbBxValueLists.DisplayLayout.Bands[0].Columns["Appearance"].Hidden = true;
                        cmbBxValueLists.DisplayLayout.Bands[0].Columns["CheckState"].Hidden = true;
                        cmbBxValueLists.DisplayLayout.Bands[0].Columns["Tag"].Hidden = true;
                        cmbBxValueLists.Value = int.MinValue;
                        cmbBxValueLists.Visible = true;
                        lblEqual.Visible = true;

                        break;

                    case "String":
                        txtbxInput.Visible = true;
                        cmbbxMatchOn.Visible = true;
                        break;

                    case "DateTime":
                        dateTimeCntrl.Visible = true;
                        cmbBxIntMachOn.Visible = true;
                        break;

                    case "Int32":
                    case "Int64":
                    case "Double":
                    case "Single":
                    case "Decimal":
                        numEditor.Visible = true;
                        cmbBxIntMachOn.Visible = true;
                        break;

                    case "Boolean":
                        lblEqual.Visible = true;
                        cmbBxTruFalse.Visible = true;
                        break;

                    default:
                        txtbxInput.Visible = true;
                        cmbbxMatchOn.Visible = true;
                        break;

                }

                cmbBxAndOr.Visible = true;
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

        //Added by: sachin mishra
        //Purpose: Added tooltip http://jira.nirvanasolutions.com:8080/browse/CHMW-2273

        private void cmbbxSearchCriteria_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            if (e.Row.Cells.Exists("DisplayText"))
            {
                e.Row.ToolTipText = e.Row.Cells["DisplayText"].Text;
            }
        }
        //Added by: sachin mishra
        //Purpose: Added tooltip http://jira.nirvanasolutions.com:8080/browse/CHMW-2273

        /// <summary>
        /// Added by: sachin mishra
        /// Purpose: Added tooltip http://jira.nirvanasolutions.com:8080/browse/CHMW-2273
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBxAndOr_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            if (e.Row.Cells.Exists("DisplayText"))
            {
                e.Row.ToolTipText = e.Row.Cells["DisplayText"].Text;
            }
        }

        /// <summary>
        /// Added by: sachin mishra
        /// Purpose: Added tooltip http://jira.nirvanasolutions.com:8080/browse/CHMW-2273
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbbxMatchOn_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            if (e.Row.Cells.Exists("DisplayText"))
            {
                e.Row.ToolTipText = e.Row.Cells["DisplayText"].Text;
            }
        }

        /// <summary>
        /// Sets the control to desired value
        /// </summary>
        /// <param name="fieldType"></param>
        /// <param name="valueList"></param>
        /// <param name="andOr"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="operation"></param>
        public void PutValues(String fieldType, ValueList valueList, string andOr, string fieldName, object fieldValue)
        {
            SetConditionControls(fieldType, valueList);

            this.cmbBxAndOr.Value = andOr;
            this.cmbbxSearchCriteria.Value = fieldName;
            this.cmbBxValueLists.Value = fieldValue;
        }
    }
}
