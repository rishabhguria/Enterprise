using Csla;
using Csla.Validation;
using Prana.BusinessObjects.PositionManagement;
using System;

namespace Prana.PM.BLL
{
    [Serializable()]
    public class DataSource : BusinessBase<DataSource>
    {

        #region Constants
        const string CONST_StatusID = "StatusID";
        const string CONST_TypeID = "TypeID";
        #endregion

        #region Private members And Public Properties

        private ThirdPartyNameID _dataSourceNameID;

        /// <summary>
        /// Gets or sets the data source name ID.
        /// </summary>
        /// <value>The data source name ID.</value>
        public ThirdPartyNameID DataSourceNameID
        {
            get
            {
                if (_dataSourceNameID == null)
                {
                    _dataSourceNameID = new ThirdPartyNameID();
                }
                return _dataSourceNameID;
            }
            set
            {
                _dataSourceNameID = value;
                PropertyHasChanged();
            }
        }

        private int _status;

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public int StatusID
        {
            get { return _status; }
            set
            {
                _status = value;
                PropertyHasChanged(CONST_StatusID);
            }
        }


        private int _typeID;

        /// <summary>
        /// Gets or sets the type ID.
        /// </summary>
        /// <value>The type ID.</value>
        public int TypeID
        {
            get
            {
                return _typeID;
            }
            set
            {
                _typeID = value;
                PropertyHasChanged(CONST_TypeID);
            }
        }

        private string _type = string.Empty;

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get
            {
                return _type;

            }
            set
            {
                _type = value;
                PropertyHasChanged();
            }
        }


        private AddressDetails _addressDetails;

        /// <summary>
        /// Gets or sets the data source address details.
        /// </summary>
        /// <value>The data source address details.</value>
        public AddressDetails DataSourceAddressDetails
        {
            get
            {
                if (_addressDetails == null)
                {
                    _addressDetails = new AddressDetails();
                }
                return _addressDetails;
            }
            set
            {
                _addressDetails = value;
                PropertyHasChanged();
            }
        }


        private PrimaryContact _primaryContact;

        /// <summary>
        /// Gets or sets the primary contact.
        /// </summary>
        /// <value>The primary contact.</value>
        public PrimaryContact PrimaryContact
        {
            get
            {
                if (_primaryContact == null)
                {
                    _primaryContact = new PrimaryContact();
                }
                return _primaryContact;
            }
            set
            {
                _primaryContact = value;
                PropertyHasChanged();
            }
        }
        #region  Retrieve


        #endregion

        #endregion

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value><c>true</c> if this instance is dirty; otherwise, <c>false</c>.</value>
        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }


        protected override object GetIdValue()
        {
            return DataSourceNameID.ID;
        }

        /// <summary>
        /// Adds the business rules.
        /// </summary>
        protected override void AddBusinessRules()
        {
            //ValidationRules.AddRule(CommonRules.MinValue<int>, new CommonRules.MinValueRuleArgs<int>(CONST_StatusID, 1));
            //ValidationRules.AddRule(CommonRules.MinValue<int>, new CommonRules.MinValueRuleArgs<int>(CONST_TypeID, 1));
            ValidationRules.AddRule(MinValueRuleArgs.TypeRequired, CONST_TypeID);
            ValidationRules.AddRule(MinValueRuleArgs.StatusRequired, CONST_StatusID);
        }

        public class MinValueRuleArgs : RuleArgs
        {
            public MinValueRuleArgs(string validation)
                : base(validation)
            {
            }

            public static bool StatusRequired(object target, RuleArgs e)
            {
                if (((DataSource)target).StatusID <= 0)
                {
                    e.Description = "Status required";
                    return false;
                }
                else
                {
                    return true;
                }
            }

            public static bool TypeRequired(object target, RuleArgs e)
            {
                if (((DataSource)target).TypeID <= 0)
                {
                    e.Description = "Type required";
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }

    }


}
