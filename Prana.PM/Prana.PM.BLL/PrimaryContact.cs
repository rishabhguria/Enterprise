using Csla;
using Csla.Validation;
using System;

namespace Prana.PM.BLL
{
    [Serializable]
    public class PrimaryContact : BusinessBase<PrimaryContact>
    {
        #region Constants
        const string CONST_FirstName = "FirstName";
        const string CONST_LastName = "LastName";
        const string CONST_Title = "Title";
        const string CONST_EMail = "EMail";
        const string CONST_WorkNumber = "WorkNumber";
        const string CONST_CellNumber = "CellNumber";
        #endregion

        #region Private fields and Public Properties

        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _title = string.Empty;
        private string _eMail = string.Empty;
        private string _workNumber = string.Empty;
        private string _cellNumber = string.Empty;

        /// <summary>
        /// Gets or sets the name of the first.
        /// </summary>
        /// <value>The name of the first.</value>
        public string FirstName
        {
            get
            {
                return _firstName;

            }
            set
            {
                _firstName = value;
                PropertyHasChanged(CONST_FirstName);
            }
        }

        /// <summary>
        /// Gets or sets the name of the last.
        /// </summary>
        /// <value>The name of the last.</value>
        public string LastName
        {
            get
            {
                return _lastName;

            }
            set
            {
                _lastName = value;
                PropertyHasChanged(CONST_LastName);
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                PropertyHasChanged(CONST_Title);
            }
        }

        /// <summary>
        /// Gets or sets the E mail.
        /// </summary>
        /// <value>The E mail.</value>
        public string EMail
        {
            get
            {
                return _eMail;
            }
            set
            {
                _eMail = value;
                PropertyHasChanged(CONST_EMail);
            }
        }

        /// <summary>
        /// Gets or sets the work number.
        /// </summary>
        /// <value>The work number.</value>
        public string WorkNumber
        {
            get
            {
                return _workNumber;
            }
            set
            {
                _workNumber = value;
                PropertyHasChanged(CONST_WorkNumber);
            }
        }

        /// <summary>
        /// Gets or sets the cell number.
        /// </summary>
        /// <value>The cell number.</value>
        public string CellNumber
        {
            get
            {
                return _cellNumber;
            }
            set
            {
                _cellNumber = value;
                PropertyHasChanged(CONST_CellNumber);
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

        //private int _id;
        /// <summary>
        /// TODO : Can't afford to have this ID property in multiuer environment
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return 0;
        }

        #endregion

        #region Validation Rules

        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_FirstName);
            ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_FirstName, 50));

            ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_LastName);
            ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_LastName, 50));

            ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_Title);
            ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_LastName, 50));

            ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_EMail);
            ValidationRules.AddRule(CommonRules.RegExMatch, new CommonRules.RegExRuleArgs(CONST_EMail, CommonRules.RegExPatterns.Email));

            ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_WorkNumber);
            ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_LastName, 50));

            ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_CellNumber);
            ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_LastName, 50));
        }

        #endregion

    }
}
