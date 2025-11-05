using Csla;
using Csla.Validation;
using System;

namespace Prana.PM.BLL
{
    [Serializable()]
    public class SelectColumnsItem : BusinessBase<SelectColumnsItem>
    {
        public SelectColumnsItem()
        {
            MarkAsChild();
        }

        public SelectColumnsItem(string sourceColumnName, int id, SelectColumnsType type, bool isRequiredinUpload, int tableTypeID)
        {
            _sourceColumnName = sourceColumnName;
            _id = id;
            _type = type;
            _isRequiredInUpload = isRequiredinUpload;
            _tableTypeID = tableTypeID;
        }

        #region Constants

        const string CONST_ID = "ID";
        const string CONST_SourceColumnName = "SourceColumnName";
        const string CONST_Description = "Description";
        const string CONST_Type = "Type";
        const string CONST_SampleValue = "SampleValue";
        const string CONST_Notes = "Notes";
        const string CONST_ColumnSequenceNo = "ColumnSequenceNo";
        #endregion

        //System.Text.RegularExpressions.Regex regMatch = new System.Text.RegularExpressions.Regex("[a-zA-Z0-9_]");
        //System.Text.RegularExpressions.Regex regMatch = new System.Text.RegularExpressions.Regex("[^/W*]");
        //System.Text.RegularExpressions.Regex regMatch = new System.Text.RegularExpressions.Regex(@"[\w\w\w]"); //@"[\w\.@-]"
        //System.Text.RegularExpressions.Regex regMatch = new System.Text.RegularExpressions.Regex("[^~|!@#$%&*()_+`-=[]{};:<>,.?|");
        //System.Text.RegularExpressions.Regex regMatchSourceComumnName = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9_]*$");
        System.Text.RegularExpressions.Regex regMatchColumnSequenceNo = new System.Text.RegularExpressions.Regex("[0-9]");

        #region Public properties and methods

        private int _id;
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        //[Browsable(false)]
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                PropertyHasChanged(CONST_ID);
            }
        }

        private string _sourceColumnName = string.Empty;

        /// <summary>
        /// Gets or sets the name of the source column.
        /// </summary>
        /// <value>The name of the source column.</value>
        public string SourceColumnName
        {
            get { return _sourceColumnName; }
            set
            {
                //_sourceColumnName = value;
                //PropertyHasChanged(CONST_SourceColumnName);

                CanWriteProperty(true);
                if (value == null)
                {
                    value = string.Empty;
                }
                _sourceColumnName = value;
                PropertyHasChanged(CONST_SourceColumnName);
            }
        }

        private string _description = string.Empty;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                PropertyHasChanged(CONST_Description);
            }
        }

        private SelectColumnsType _type = SelectColumnsType.Text;

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public SelectColumnsType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                //PropertyHasChanged(CONST_Type);
                //Deliberately called sample value property changed as this property changed has to check 
                //whether Sample value entered is as per the latest value changed for column type.
                PropertyHasChanged(CONST_SampleValue);
            }
        }

        private string _sampleValue = string.Empty;

        public string SampleValue
        {
            get { return _sampleValue; }
            set
            {
                _sampleValue = value;
                PropertyHasChanged(CONST_SampleValue);
            }
        }

        private string _notes = string.Empty;

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                PropertyHasChanged(CONST_Notes);
            }
        }

        //ColumnSequenceNo

        private int _columnSequenceNo;

        /// <summary>
        /// Added Rajat (24 Nov 2006)
        /// Gets or sets the column sequence no.
        /// </summary>
        /// <value>The column sequence no.</value>
        public int ColumnSequenceNo
        {
            get { return _columnSequenceNo; }
            set
            {
                _columnSequenceNo = value;
                PropertyHasChanged(CONST_ColumnSequenceNo);
            }
        }

        private bool _isRequiredInUpload = false;

        /// <summary>
        /// Added Rajat (24 Nov 2005)
        /// Gets or sets a value indicating whether this instance is required in upload.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is required in upload; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequiredInUpload
        {
            get { return _isRequiredInUpload; }
            set
            {
                _isRequiredInUpload = value;
                PropertyHasChanged();
            }
        }

        private int _tableTypeID;

        /// <summary>
        /// Gets or sets the table type ID.
        /// </summary>
        /// <value>The data table type ID.</value>
        public int TableTypeID
        {
            get
            {
                return _tableTypeID;
            }
            set
            {
                _tableTypeID = value;
                PropertyHasChanged();
            }
        }


        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        /// <summary>
        /// TODO : Can't afford to have this ID property in multiuer environment
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return _id;
        }
        #endregion



        #region Validation Rules

        protected override void AddBusinessRules()
        {
            //System.Text.RegularExpressions.Regex regMatchNew = new System.Text.RegularExpressions.Regex(@"[\w\w\w]"); //@"[\w\.@-]"
            //string regMatchNew1 = "";
            //regMatchNew1 = regMatch.Replace(CONST_SourceColumnName, @"[^\w\.@-]", "");

            ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_SourceColumnName);
            ValidationRules.AddRule(Csla.Validation.CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_SourceColumnName, 50));
            ValidationRules.AddRule(Csla.Validation.CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_SampleValue, 100));
            ValidationRules.AddRule(Csla.Validation.CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_Description, 200));
            ValidationRules.AddRule(Csla.Validation.CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_Notes, 200));

            //ValidationRules.AddRule(CommonRules.RegExMatch, new Csla.Validation.CommonRules.RegExRuleArgs(CONST_SourceColumnName, regMatchSourceComumnName));

            //ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_ColumnSequenceNo);
            ValidationRules.AddRule(CommonRules.RegExMatch, new Csla.Validation.CommonRules.RegExRuleArgs(CONST_ColumnSequenceNo, regMatchColumnSequenceNo));

            ValidationRules.AddRule(RegularExpressionRuleArgs.SourceColumnNameCheck, CONST_SourceColumnName);
            ValidationRules.AddRule(RegularExpressionRuleArgs.SampleValueValidation, CONST_SampleValue);
            ValidationRules.AddRule(RegularExpressionRuleArgs.SampleValueValidation, CONST_Type);
            ValidationRules.AddRule(RegularExpressionRuleArgs.SourceColumnRepeatNameCheck, CONST_SourceColumnName);
        }

        #endregion

        public class RegularExpressionRuleArgs : RuleArgs
        {
            static System.Text.RegularExpressions.Regex regMatchSourceColumnName = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9_]*$");
            public RegularExpressionRuleArgs(string validation)
                : base(validation)
            {
            }

            public static bool SourceColumnNameCheck(object target, RuleArgs e)
            {
                SelectColumnsItem finalTarget = target as SelectColumnsItem;
                if (finalTarget != null)
                {

                    if (!regMatchSourceColumnName.IsMatch(finalTarget.SourceColumnName))
                    {
                        e.Description = "Source Name not valid";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            public static bool SampleValueValidation(object target, RuleArgs e)
            {
                bool isValid = false;
                SelectColumnsItem finalTarget = target as SelectColumnsItem;
                if (finalTarget != null)
                {
                    switch (finalTarget.Type)
                    {
                        case SelectColumnsType.Integer:
                            if (IsInteger(finalTarget.SampleValue))
                            {
                                isValid = true;
                            }
                            else
                            {
                                isValid = false;
                            }
                            break;
                        case SelectColumnsType.Decimal:
                            if (IsDecimal(finalTarget.SampleValue))
                            {
                                isValid = true;
                            }
                            else
                            {
                                isValid = false;
                            }
                            break;
                        case SelectColumnsType.Text:
                            if (IsText(finalTarget.SampleValue))
                            {
                                isValid = true;
                            }
                            else
                            {
                                isValid = false;
                            }
                            break;
                        case SelectColumnsType.Boolean:
                            if (IsBool(finalTarget.SampleValue))
                            {
                                isValid = true;
                            }
                            else
                            {
                                isValid = false;
                            }
                            break;
                        case SelectColumnsType.DateTime:
                            if (IsDateTime(finalTarget.SampleValue))
                            {
                                isValid = true;
                            }
                            else
                            {
                                isValid = false;
                            }
                            break;
                        default:
                            isValid = false;
                            break;
                    }
                    //if(finalTarget

                    if (!isValid)
                    {
                        e.Description = "Sample value is not valid as per the type choosen for the column";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            public static bool IsInteger(object value)
            {
                try
                {
                    Convert.ToInt32(value.ToString());
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            public static bool IsDecimal(object value)
            {
                try
                {
                    Convert.ToDouble(value.ToString());
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            public static bool IsDateTime(object value)
            {
                try
                {
                    Convert.ToDateTime(value.ToString());
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            public static bool IsText(object value)
            {
                try
                {
                    Convert.ToString(value.ToString());
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            public static bool IsBool(object value)
            {
                try
                {
                    Convert.ToBoolean(value.ToString());
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            public static bool SourceColumnRepeatNameCheck(object target, RuleArgs e)
            {
                SelectColumnsItem sourceTarget = target as SelectColumnsItem;
                //if (sourceTarget.Parent != null)
                //{
                //    sourceTarget.Parent.RemoveChild(sourceTarget);
                //}
                SelectColumnsItemList selectColumnsItemList = (SelectColumnsItemList)sourceTarget.Parent;
                SelectColumnsItemList selectColumnsItemListClone = new SelectColumnsItemList(); ;

                if (selectColumnsItemList != null)
                {
                    selectColumnsItemListClone = selectColumnsItemList.Clone();
                    int index = 0;
                    //if(selectColumnsItemListClone.Contains(sourceTarget))
                    //{
                    //    int index = selectColumnsItemListClone.IndexOf(sourceTarget);
                    //    selectColumnsItemListClone.Remove(sourceTarget);
                    //}
                    //Have to use the follwoing link as the "selectColumnsItemListClone.Remove(sourceTarget)" was not working
                    //as the reason I think is that the indexes in the clone list gets reverse or changed from the orignal list.
                    foreach (SelectColumnsItem selectColumnsItem in selectColumnsItemListClone)
                    {
                        if (selectColumnsItem.SourceColumnName == sourceTarget.SourceColumnName)
                        {
                            break;
                        }
                        index++;
                    }
                    selectColumnsItemListClone.RemoveAt(index);
                }
                if (sourceTarget != null && selectColumnsItemList != null)
                {
                    //sourceTarget.Parent.RemoveChild(sourceTarget);
                    foreach (SelectColumnsItem selectColumnsItem in selectColumnsItemListClone)
                    {
                        if (string.Compare(selectColumnsItem._sourceColumnName.Trim(), sourceTarget._sourceColumnName.Trim(), true) == 0)
                        //if (!selectColumnsItem.Equals(sourceTarget) && (selectColumnsItem._sourceColumnName.Trim().ToUpper() == sourceTarget._sourceColumnName.Trim().ToUpper()))
                        {
                            e.Description = "Column name entered can not be same as the other source column name present.";
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return true;
                }
            }

        }

    }
}
