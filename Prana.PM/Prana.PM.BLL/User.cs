using Csla;
using Csla.Validation;
using System;

namespace Prana.PM.BLL
{
    [Serializable()]
    public class User : BusinessBase<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
            MarkAsChild();
        }

        #region Constants
        const string CONST_Password = "Password";
        const string CONST_CompanyUserID = "CompanyUserID";
        const string CONST_ConfirmPassword = "ConfirmPassword";
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User(int id, string name)
        {

            this.CompanyUserID = id;
            this.UserName = name;
        }



        private string _userName = string.Empty;

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                PropertyHasChanged();
            }
        }



        private string _adminFirstName = string.Empty;

        public string AdminFirstName
        {
            get { return _adminFirstName; }
            set
            {
                _adminFirstName = value;
                PropertyHasChanged();
            }
        }

        private string _adminLastName = string.Empty;

        public string AdminLastName
        {
            get { return _adminLastName; }
            set
            {
                _adminLastName = value;
                PropertyHasChanged();
            }
        }



        private string _adminTitle = string.Empty;

        public string AdminTitle
        {
            get { return _adminTitle; }
            set
            {
                _adminTitle = value;
                PropertyHasChanged();
            }
        }

        private string _userID = string.Empty;

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public string ID
        {
            get { return _userID; }
            set
            {
                _userID = value;
                PropertyHasChanged();
            }
        }

        private int _companyUserID;

        /// <summary>
        /// Gets or sets the company user ID.
        /// </summary>
        /// <value>The company user ID.</value>
        public int CompanyUserID
        {
            get { return _companyUserID; }
            set
            {
                _companyUserID = value;
                PropertyHasChanged(CONST_CompanyUserID);
            }
        }


        private string _password = string.Empty;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                PropertyHasChanged(CONST_Password);
            }
        }

        private string _confirmPassword = string.Empty;

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                PropertyHasChanged(CONST_ConfirmPassword);
            }
        }


        private string _adminEmail = string.Empty;

        public string AdminEmail
        {
            get { return _adminEmail; }
            set
            {
                _adminEmail = value;
                PropertyHasChanged();
            }
        }

        private string _adminWorkNumber = string.Empty;

        public string AdminWorkNumber
        {
            get { return _adminWorkNumber; }
            set
            {
                _adminWorkNumber = value;
                PropertyHasChanged();
            }
        }

        private string _adminCellNumber = string.Empty;

        public string AdminCellNumber
        {
            get { return _adminCellNumber; }
            set
            {
                _adminCellNumber = value;
                PropertyHasChanged();
            }
        }

        private string _adminPagerNumber = string.Empty;

        public string AdminPagerNumber
        {
            get { return _adminPagerNumber; }
            set
            {
                _adminPagerNumber = value;
                PropertyHasChanged();
            }
        }

        private string _adminHomeNumber = string.Empty;

        public string AdminHomeNumber
        {
            get { return _adminHomeNumber; }
            set
            {
                _adminHomeNumber = value;
                PropertyHasChanged();
            }
        }

        private string _adminFaxNumber = string.Empty;

        public string AdminFaxNumber
        {
            get { return _adminFaxNumber; }
            set
            {
                _adminFaxNumber = value;
                PropertyHasChanged();
            }
        }

        public override string ToString()
        {
            return UserName;
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
            return _userID;
        }

        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(CommonRules.StringRequired, CONST_Password);
            ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_Password, 50));

            //ValidationRules.AddRule(CommonRules.MinValue<int>, new CommonRules.MinValueRuleArgs<int>(CONST_CompanyUserID, 1));
            //ValidationRules.AddRule(CommonRules.MinValue<int>, new CommonRules.MinValueRuleArgs<int>(CONST_CompanyUserID, 1));

            //ValidationRules.AddRule(CommonRules.RegExMatch, new Csla.Validation.CommonRules.RegExRuleArgs(CONST_SourceItemName, regMatchSourceItemName));
            ValidationRules.AddRule(CustomClass.UserRequired, CONST_CompanyUserID);
            ValidationRules.AddRule(CustomClass.MatchPassword, CONST_ConfirmPassword);
        }

        public class CustomClass : RuleArgs
        {
            public CustomClass(string validation)
                : base(validation)
            {
            }

            public static bool UserRequired(object target, RuleArgs e)
            {
                User finalTarget = target as User;
                if (finalTarget != null)
                {
                    if (finalTarget.CompanyUserID <= 0)
                    {
                        e.Description = "User required";
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

            public static bool MatchPassword(object target, RuleArgs e)
            {
                User finalTarget = target as User;
                if (finalTarget != null)
                {
                    if (finalTarget.Password.Equals(finalTarget.ConfirmPassword))
                    {
                        return true;
                    }
                    else
                    {
                        e.Description = "Confirm password does not match the given password";
                        return false;
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
