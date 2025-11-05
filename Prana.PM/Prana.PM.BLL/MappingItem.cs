using Csla;
using Csla.Validation;
using System;
using System.ComponentModel;

namespace Prana.PM.BLL
{
    public delegate bool SourceItemNameRule(object sender, EventArgs e);
    [Serializable()]
    public class MappingItem : BusinessBase<MappingItem>
    {
        //public delegate bool SourceItemNameRule(object sender, EventArgs e);

        #region Constants
        const string CONST_SourceItemName = "SourceItemName";
        const string CONST_SourceItemFullName = "SourceItemFullName";

        const string CONST_ApplicationItemId = "ApplicationItemId";
        const string CONST_SourceItemID = "SourceItemID";
        #endregion

        //System.Text.RegularExpressions.Regex regMatchSourceItemName = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9_]*$");
        //System.Text.RegularExpressions.Regex regMatchColumnSequenceNo = new System.Text.RegularExpressions.Regex("[0-9]");

        public MappingItem()
        {
            MarkAsChild();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MappingItem(string sourceItemName, string applicationItemName)
        {
            this.SourceItemName = sourceItemName;
            this.ApplicationItemName = applicationItemName;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MappingItem(string sourceItemName, string applicationItemName, string applicationItemFullName)
        {
            this.SourceItemName = sourceItemName;
            this.ApplicationItemName = applicationItemName;
            this.ApplicationItemFullName = applicationItemFullName;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MappingItem(string sourceItemName, string applicationItemName, string applicationItemFullName, bool Lock)
        {
            this.SourceItemName = sourceItemName;
            this.ApplicationItemName = applicationItemName;
            this.ApplicationItemFullName = applicationItemFullName;
            this.Lock = Lock;
        }

        private int _sourceItemID;
        /// <summary>
        /// Gets or sets the source item ID.
        /// </summary>
        /// <value>The source item ID.</value>
        public int SourceItemID
        {
            get { return _sourceItemID; }
            set
            {
                _sourceItemID = value;
                PropertyHasChanged(CONST_SourceItemID);
            }
        }


        private string _sourceItemName = string.Empty;

        /// <summary>
        /// Gets or sets the name of the source item.
        /// </summary>
        /// <value>The name of the source item.</value>
        public string SourceItemName
        {
            get
            {
                CanReadProperty(true);
                return _sourceItemName;
            }
            set
            {
                //_sourceItemName = value;
                CanWriteProperty(true);
                if (value == null)
                {
                    value = string.Empty;
                }
                //if (!_sourceItemName.Equals(value))
                //{
                //AddBusinessRules();
                _sourceItemName = value;
                PropertyHasChanged(CONST_SourceItemName);
                //ValidationRules.CheckRules(CONST_SourceItemName);
                //} 

                //PropertyHasChanged();
                //
                //ApplyEdit();
            }
        }

        private string _sourceItemFullName;

        /// <summary>
        /// Gets or sets the full name of the source item.
        /// </summary>
        /// <value>The full name of the source item.</value>
        public string SourceItemFullName
        {
            get { return _sourceItemFullName; }
            set
            {
                _sourceItemFullName = value;
                PropertyHasChanged();
                ValidationRules.CheckRules(CONST_SourceItemFullName);
            }
        }

        private int _applicationItemId;
        //[Browsable(false)]
        public int ApplicationItemId
        {
            get { return _applicationItemId; }
            set
            {
                _applicationItemId = value;
                //PropertyHasChanged(CONST_ApplicationItemId);
                //PropertyHasChanged(CONST_ApplicationColumnTypeID);
            }
        }


        private string _applicationItemName;

        /// <summary>
        /// Gets or sets the name of the application item.
        /// </summary>
        /// <value>The name of the application item.</value>
        public string ApplicationItemName
        {
            get { return _applicationItemName; }
            set
            {
                _applicationItemName = value;
                //PropertyHasChanged(CONST_ApplicationItemName);
            }
        }

        //To Do: if required add SourceItemFullName, as of now it's not required so ignoring it!
        private string _applicationItemFullName;

        /// <summary>
        /// Gets or sets the full name of the application item.
        /// </summary>
        /// <value>The full name of the application item.</value>
        public string ApplicationItemFullName
        {
            get { return _applicationItemFullName; }
            set
            {
                _applicationItemFullName = value;
                //PropertyHasChanged();
            }
        }

        private int _applicationColumnTypeID;
        /// <summary>
        /// Gets or sets the Application Column Type ID.
        /// </summary>
        /// <value>The Application Coulumn Type ID.</value>
        [Browsable(false)]
        public int ApplicationColumnTypeID
        {
            get { return _applicationColumnTypeID; }
            set
            {
                _applicationColumnTypeID = value;
                //PropertyHasChanged(CONST_ApplicationColumnTypeID);
                PropertyHasChanged(CONST_ApplicationItemId);
            }
        }

        private int _sourceColumnTypeID;
        /// <summary>
        /// Gets or sets the Source Column Type ID.
        /// </summary>
        /// <value>The Source Column Type ID.</value>
        [Browsable(false)]
        public int SourceColumnTypeID
        {
            get { return _sourceColumnTypeID; }
            set
            {
                _sourceColumnTypeID = value;
                //PropertyHasChanged(CONST_SourceColItemID);
            }
        }

        private bool _lock;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MappingItem"/> is lock.
        /// </summary>
        /// <value><c>true</c> if lock; otherwise, <c>false</c>.</value>
        public bool Lock
        {
            get { return _lock; }
            set
            {
                _lock = value;
                PropertyHasChanged();
            }

        }

        public override bool IsValid
        {
            get
            {
                return base.IsValid;
            }
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
            return _sourceItemID;
        }


        #region Validation Rules

        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_SourceItemName);
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_SourceItemName, 6));
            ValidationRules.AddRule(CommonRules.StringRequired, CONST_SourceItemName);
            ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_SourceItemName, 50));
            //ValidationRules.AddRule(SourceItemNameRule, CONST_SourceItemName);

            //ValidationRules.AddRule(CommonRules.MinValue<int>, new CommonRules.MinValueRuleArgs<int>(CONST_ApplicationItemId, 1));
            ValidationRules.AddRule(CommonRules.MinValue<int>, new CommonRules.MinValueRuleArgs<int>(CONST_SourceItemID, 1));

            //ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_SourceItemFullName);
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_SourceItemFullName, 50));

            //ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_ApplicationItemName);
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_ApplicationItemName, 50));

            //ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_ApplicationItemFullName);
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_ApplicationItemFullName, 50));

            //ValidationRules.AddRule(CommonRules.RegExMatch, new Csla.Validation.CommonRules.RegExRuleArgs(CONST_SourceItemName, regMatchSourceItemName));
            //ValidationRules.AddRule(CustomClass.SourceItemNameCheck, CONST_SourceItemName);
            //ValidationRules.AddRule(CustomClass.MapColumnCompatibility, CONST_ApplicationColumnTypeID);
            ValidationRules.AddRule(CustomClass.MapColumnCompatibility, CONST_ApplicationItemId);
        }

        public class CustomClass : RuleArgs
        {
            static System.Text.RegularExpressions.Regex regMatchSourceItemName = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9_]*$");
            public CustomClass(string validation)
                : base(validation)
            {
            }

            public static bool SourceItemNameCheck(object target, RuleArgs e)
            {
                MappingItem finalTarget = target as MappingItem;
                if (finalTarget != null)
                {
                    if (!regMatchSourceItemName.IsMatch(finalTarget.SourceItemName))
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

            public static bool MapColumnCompatibility(object target, RuleArgs e)
            {
                MappingItem finalTarget = target as MappingItem;
                if (finalTarget != null)
                {
                    if (finalTarget.SourceColumnTypeID != finalTarget.ApplicationColumnTypeID)
                    {
                        e.Description = "Source Column Type and Application Column Type does not match. Please map valid columns.";
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


        }

        #endregion

    }
}
