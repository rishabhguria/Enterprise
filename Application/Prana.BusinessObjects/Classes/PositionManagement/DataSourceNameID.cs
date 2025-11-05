using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using Csla;
using Csla.Validation;

namespace Prana.BusinessObjects.PositionManagement
{
    [Serializable(),System.Runtime.InteropServices.ComVisible(false)]
    public class DataSourceNameID : BusinessBase<DataSourceNameID>
    {

        #region Constants
        const string CONST_FullName = "FullName";
        const string CONST_ShortName = "ShortName";
        const string CONST_DataSourceID = "ID";
        #endregion

        #region contructor

        ///TODO : there can be some problem while defining other constructors than the default constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceNameID"/> class.
        /// </summary>
        public DataSourceNameID()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceNameID"/> class.
        /// </summary>
        /// <param name="dataSourceID">The data source ID.</param>
        /// <param name="fullName">The full name.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DataSourceNameID(int dataSourceID, string fullName)
        {
            ID = dataSourceID;
            FullName = fullName;
            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceNameID"/> class.
        /// </summary>
        /// <param name="dataSourceID">The data source ID.</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="shortName">Name of the short.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DataSourceNameID(int dataSourceID, string fullName, string shortName) : this(dataSourceID, fullName)
        {
            ShortName = shortName;
        }

        #endregion

        #region members and properties

        private int _ID ; //= int.MinValue;

        /// <summary>
        /// Gets or sets the company ID.
        /// </summary>
        /// <value>The ID.</value>
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
                PropertyHasChanged(CONST_DataSourceID);
            }
        }

        private string _fullname = string.Empty;

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>The name.</value>
        public string FullName
        {
            get
            {
                return _fullname;
            }
            set
            {
                _fullname = value;
                PropertyHasChanged(CONST_FullName);
            }
        }

        private string _shortname = string.Empty;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string ShortName
        {
            get
            {
                return _shortname;
            }
            set
            {
                _shortname = value;
                PropertyHasChanged(CONST_ShortName);
            }
        }
        #endregion

        public override string ToString()
        {
            return _shortname;
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
            return _ID;
        }

        protected override void AddBusinessRules()
        {
            System.Text.RegularExpressions.Regex noWhiteSpacePattern = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9_]*$");
            ValidationRules.AddRule(CommonRules.RegExMatch, new Csla.Validation.CommonRules.RegExRuleArgs(CONST_ShortName, noWhiteSpacePattern));
            
            ValidationRules.AddRule(CommonRules.StringRequired, CONST_ShortName);
            


            //ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_SourceItemName, 2));

            ValidationRules.AddRule(CommonRules.StringRequired, CONST_FullName);
            ValidationRules.AddRule(CustomClass.DataSourceRequired, CONST_DataSourceID);
        }

        public class CustomClass : RuleArgs
        {
            public CustomClass(string validation)
                : base(validation)
            {
            }

            public static bool DataSourceRequired(object target, RuleArgs e)
            {
                DataSourceNameID finalTarget = target as DataSourceNameID;
                if (finalTarget != null)
                {
                    if (finalTarget.ID <= 0)
                    {
                        e.Description = "Data Source required";
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

    }
}
