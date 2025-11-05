using Csla;
using Csla.Validation;
using System;

namespace Prana.BusinessObjects.PositionManagement
{
    [Serializable(), System.Runtime.InteropServices.ComVisible(false)]
    public class CompanyNameID : BusinessBase<CompanyNameID>
    {
        #region Constants

        const string CONST_ID = "ID";
        const string CONST_FullName = "FullName";
        const string CONST_ShortName = "ShortName";

        #endregion

        public CompanyNameID()
        {

        }

        public CompanyNameID(int companyID, string fullName)
        {
            this.ID = companyID;
            this.FullName = fullName;
        }

        public CompanyNameID(int companyID, string fullName, string shortName) : this(companyID, fullName)
        {
            this.ShortName = shortName;
        }


        #region members and properties

        private int _ID;

        /// <summary>
        /// Gets or sets the company ID.
        /// </summary>
        /// <value>The ID.</value>

        //[Browsable(false)]
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
                PropertyHasChanged(CONST_ID);
            }
        }

        private string _fullname = string.Empty;

        /// <summary>
        /// Gets or sets the name.
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

        private string _shortName = string.Empty;

        /// <summary>
        /// Gets or sets the Short Name of the company.
        /// </summary>
        /// <value>Short Name of the company.</value>
        public string ShortName
        {
            get { return _shortName; }
            set
            {
                _shortName = value;
                PropertyHasChanged(CONST_ShortName);
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return _shortName;
        }



        #endregion


        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        protected override object GetIdValue()
        {
            return _ID;
        }

        #region Validation Rules

        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_ShortName);
            ValidationRules.AddRule(Csla.Validation.CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_ShortName, 5));
            ValidationRules.AddRule(Csla.Validation.CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_FullName, 50));
        }

        #endregion

    }
}
