using Csla;
using Csla.Validation;
using System;
//


namespace Prana.PM.BLL
{
    [Serializable()]
    public class AddressDetails : BusinessBase<AddressDetails>
    {
        //public delegate bool CountryRuleHandler(object target, RuleArgs e);
        //private RuleArgs _rArgs = new RuleArgs(CONST_CountryId);

        //private RuleHandler _handler;
        //private string _ruleName = string.Empty;

        //private object _target;

        //public void AddRule(RuleHandler handler, RuleArgs e);


        #region Private Members
        private string _address1 = string.Empty;
        private string _address2 = string.Empty;
        private int _countryId;
        private int _stateId;
        private string _zip = string.Empty;
        private string _workNumber = string.Empty;
        private string _faxNumber = string.Empty;
        #endregion

        #region Constants
        //const string CONST_AddressDetails = "AddressDetails";
        const string CONST_Address1 = "Address1";
        const string CONST_Address2 = "Address2";
        const string CONST_CountryId = "CountryId";
        const string CONST_StateId = "StateId";
        const string CONST_Zip = "Zip";
        const string CONST_WorkNumber = "WorkNumber";
        const string CONST_FaxNumber = "FaxNumber";


        #endregion

        #region Public Properties
        public string Address1
        {
            get { return _address1; }
            set
            {
                _address1 = value;
                PropertyHasChanged(CONST_Address1);
            }
        }

        public string Address2
        {
            get { return _address2; }
            set
            {
                _address2 = value;
                //PropertyHasChanged(CONST_Address2);
            }
        }

        public int CountryId
        {
            get { return _countryId; }
            set
            {
                _countryId = value;
                PropertyHasChanged(CONST_CountryId);
            }
        }

        public int StateId
        {
            get { return _stateId; }
            set
            {
                _stateId = value;
                PropertyHasChanged(CONST_StateId);
            }
        }

        public string Zip
        {
            get { return _zip; }
            set
            {
                _zip = value;
                PropertyHasChanged(CONST_Zip);
            }

        }

        public string WorkNumber
        {
            get { return _workNumber; }
            set
            {
                _workNumber = value;
                PropertyHasChanged(CONST_WorkNumber);
            }
        }

        public string FaxNumber
        {
            get { return _faxNumber; }
            set
            {
                _faxNumber = value;
                PropertyHasChanged(CONST_FaxNumber);
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

        // private int _id;
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
            ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_Address1);
            ValidationRules.AddRule(Csla.Validation.CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_Address1, 100));

            //ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_Address2);
            ValidationRules.AddRule(Csla.Validation.CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_Address2, 100));

            ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_Zip);

            ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_WorkNumber);

            //ValidationRules.AddRule(CommonRules.MinValue<int>, new CommonRules.MinValueRuleArgs<int>(CONST_CountryId, 1));
            //ValidationRules.AddRule(CommonRules.MinValue<int>, new CommonRules.MinValueRuleArgs<int>(CONST_StateId, 1));
            //ValidationRules.AddRule(CommonRules.MinValue<int>, new CommonRules.MinValueRuleArgs<int>(CONST_StatateId, 1));
            //ValidationRules.AddRule(CommonRules.MinValue<int>, new CommonRules.MinValueRuleArgs<int>(CONST_StateId, 1));


            //ValidationRules.AddRule(CountryRule, CONST_CountryId);
            //ValidationRules.AddRule(CommonRules.MinValueRuleArgs<int>, new MyRuleArgs(), CONST_CountryId);
            //ValidationRules.AddRule(CountryRequired, , CONST_CountryId);
            //ValidationRules.AddRule(new CountryRuleHandler(CountryRequired(addressDetails, rArgs)), CONST_CountryId);
            //ValidationRules.AddRule(new RuleHandler(CommonRules.ma , new CommonRules

            //ValidationRules.AddRule(new RuleHandler((CommonRules.MinValue<int>), new MyRuleArgs.MinValueRuleArgs(CountryId, 0)));
            //ValidationRules.AddRule(CommonRules.MinValue<int>, new MyRuleArgs(CONST_CountryId));
            //ValidationRules.AddRule(CommonRules.MinValue<int>, (MyRuleArgs.MinValueRuleArgs(_target, _rArgs)).ToString());
            //ValidationRules.AddRule(CommonRules.MinValue<int>, new CommonRules.MinValueRuleArgs<int>(CONST_CountryId, 1));
            //ValidationRules.AddRule(CommonRules.MinValue<int>, MinValueRuleArgs.MinValue(_countryId, _rArgs).ToString());
            //ValidationRules.AddRule(MinValueRuleArgs.MinValue , new MinValueRuleArgs);
            //ValidationRules.AddRule(new RuleHandler(CountryRequired11(CountryRequired11(_target, _ruleArgs)), CONST_CountryId), CONST_CountryId);
            ValidationRules.AddRule(MinValueRuleArgs.CountryRequired, CONST_CountryId);
            ValidationRules.AddRule(MinValueRuleArgs.StateRequired, CONST_StateId);
        }

        #endregion

        public class MinValueRuleArgs : RuleArgs
        {

            //private string mPropertyDesc = "";
            //private string mPropertyName = "";
            //private RuleSeverity mRuleSeverity = RuleSeverity.Information;
            //private bool mProcessing = false;

            //[NonSerialized()]
            //private object _target;
            //private RuleHandler _handler;
            //private string _ruleName = string.Empty;
            //private RuleArgs _ruleArgs;


            //private Dictionary<string, List<RuleMethod>> _rulesList;
            //public void AddRule(RuleHandler handler, RuleArgs args);
            //public void AddRule(RuleHandler handler, string propertyName);

            //public void AddRule(RuleHandler handler, string propertyName)
            //{
            //    // get the list of rules for the property
            //    List<RuleMethod> list = GetRulesForProperty(propertyName);
            //    // we have the list, add our new rule
            //    list.Add(new RuleMethod(_target, handler, propertyName));
            //}
            //public void AddRule(RuleHandler handler, RuleArgs ruleArgs)
            //{
            //    //// get the list of rules for the property
            //    //List<RuleMethod> list = GetRulesForProperty(propertyName);
            //    //// we have the list, add our new rule
            //    //list.Add(new RuleMethod(_target, handler, propertyName));
            //}
            //BB
            //internal void ValidationRules(object businessObject)
            //{
            //    SetTarget(businessObject);
            //}
            //internal void SetTarget(object businessObject)
            //{
            //    _target = businessObject;
            //}




            //public string PropertyDesc
            //{
            //    get
            //    {
            //        return mPropertyDesc;
            //    }
            //    set
            //    {
            //        mPropertyDesc = value;
            //    }
            //}
            //public string MPropertyName
            //{
            //    get
            //    {
            //        return mPropertyName;
            //    }
            //    set
            //    {
            //        mPropertyName = value;
            //    }
            //}
            //public RuleSeverity MRuleSeverity
            //{
            //    get
            //    {
            //        return mRuleSeverity;
            //    }
            //    set
            //    {
            //        mRuleSeverity = value;
            //    }
            //}
            //public bool Processing
            //{
            //    get
            //    {
            //        return mProcessing;
            //    }
            //    set
            //    {
            //        mProcessing = value;
            //    }
            //}

            public MinValueRuleArgs(string validation)
                : base(validation)
            {
            }

            //public void NewRule(string propertyName, string propertyDescr)
            //{
            //    this.NewRule(propertyName, "Please select some country");
            //    mPropertyName = propertyName;
            //    mPropertyDesc = propertyDescr;
            //}

            //public void NewRule(string propertyName, RuleSeverity ruleSeverity, string propertyDescr)
            //{
            //    this.NewRule(propertyName, "Please select some country");
            //    mPropertyName = propertyName;
            //    mRuleSeverity = ruleSeverity;
            //    mPropertyDesc = propertyDescr;
            //}

            //public void NewRule(string propertyName, RuleSeverity ruleSeverity, bool stopProcessing, string propertyDescr)
            //{
            //    this.NewRule(propertyName, "Please select some country");
            //    mPropertyName = propertyName;
            //    mRuleSeverity = ruleSeverity;
            //    mProcessing = stopProcessing;
            //    mPropertyDesc = propertyDescr;
            //}

            //public override string ToString()
            //{
            //    return base.ToString() + "!" + mPropertyDesc.ToString();
            //}

            //public static bool MinValue(object target, RuleArgs e);
            public static bool CountryRequired(object target, RuleArgs e)
            {
                //if (int.Parse(target.ToString()) <= 0)
                if (((AddressDetails)target).CountryId <= 0)
                {
                    e.Description = "Country required";
                    return false;
                }
                else
                {
                    return true;
                }
            }

            public static bool StateRequired(object target, RuleArgs e)
            {
                if (((AddressDetails)target).StateId <= 0)
                {
                    e.Description = "State required";
                    return false;
                }
                else
                {
                    return true;
                }
            }

            //public static RuleArgs MinValue(int countryID, int minValue)
            //{
            //    if (countryID <= 0)
            //    {
            //        string mPropertyDesc = "Country required!";
            //        return ;
            //    }
            //    else
            //    {
            //        return "";
            //    }
            //}

            //public static bool MinValueRuleArgs(object target, RuleArgs e)
            //{
            //    if (((AddressDetails)target).CountryId <= 0)
            //    {
            //        e.Description = "Country required!";
            //        return false;
            //    }
            //    else
            //    {
            //        return true;
            //    }
            //}
        }


        //internal void ValidationRules(object businessObject)
        //{
        //    SetTarget(businessObject);
        //}
        //internal void SetTarget(object businessObject)
        //{
        //    _target = businessObject;
        //}


        //private Dictionary<string, List<RuleMethod>> _rulesList;

        //public void AddRule(RuleHandler handler, string propertyName)
        //{
        //    List<RuleMethod> list = GetRulesForProperty(propertyName);
        //    list.Add(new RuleMethod(_target, handler, propertyName));
        //}


    }
}
