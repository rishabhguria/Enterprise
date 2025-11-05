using Csla;
using Csla.Validation;
using Prana.BusinessObjects.AppConstants;
using System;
using System.ComponentModel;

namespace Prana.PM.BLL
{
    [Serializable()]
    public class ReconSetup : BusinessBase<ReconSetup>
    {
        #region Constants

        const string CONST_ThirdPartyID = "ThirdPartyID";
        const string CONST_ReconTypeID = "ReconTypeID";
        const string CONST_ReconType = "ReconType";
        const string CONST_FormatType = "FormatType";
        const string CONST_XSLTName = "XSLTName";

        #endregion


        public ReconSetup()
        {
            MarkAsChild();
        }

        private int _reconThirdPartyID;

        [Browsable(false)]
        public int ReconThirdPartyID
        {
            get { return _reconThirdPartyID; }
            set { _reconThirdPartyID = value; }
        }


        private int _thirdPartyID;

        public int ThirdPartyID
        {
            get { return _thirdPartyID; }
            set
            {
                _thirdPartyID = value;
                PropertyHasChanged(CONST_ThirdPartyID);
            }

        }


        private int _reconTypeID;

        public int ReconTypeID
        {
            get { return _reconTypeID; }
            set
            {
                _reconTypeID = value;
                _reconType = (ReconType)_reconTypeID;
                PropertyHasChanged(CONST_ReconTypeID);
            }
        }

        private ReconType _reconType;

        public ReconType ReconType
        {
            get { return _reconType; }
            set
            {
                _reconType = value;
                _reconTypeID = (int)_reconType;
                PropertyHasChanged(CONST_ReconType);
            }
        }

        private string _formatType;

        public string FormatType
        {
            get { return _formatType; }
            set
            {
                _formatType = value;
                PropertyHasChanged(CONST_FormatType);
            }
        }

        private int _xsltID;

        [Browsable(false)]
        public int XSLTID
        {
            get { return _xsltID; }
            set { _xsltID = value; }
        }

        private string _xsltName;

        public string XSLTName
        {
            get { return _xsltName; }
            set
            {
                _xsltName = value;
                PropertyHasChanged(CONST_XSLTName);
            }
        }

        private byte[] _xsltData;

        [Browsable(false)]
        public byte[] XLSTData
        {
            get { return _xsltData; }
            set { _xsltData = value; }
        }


        protected override object GetIdValue()
        {
            return _reconThirdPartyID;
        }

        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        #region Validation Rules

        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_FormatType, 50));
            ValidationRules.AddRule(CustomClass.ThirdPartyIDCheck, CONST_ThirdPartyID);
            ValidationRules.AddRule(CustomClass.XSLTNameCheck, CONST_XSLTName);
            ValidationRules.AddRule(CustomClass.FormatTypeCheck, CONST_FormatType);
            ValidationRules.AddRule(CustomClass.RepeatSetupEntryCheck, CONST_FormatType);
        }

        public class CustomClass : RuleArgs
        {
            public CustomClass(string validation) : base(validation)
            {

            }

            public static bool ThirdPartyIDCheck(object target, RuleArgs e)
            {
                ReconSetup finalTarget = target as ReconSetup;
                if (finalTarget != null)
                {
                    if (finalTarget.ThirdPartyID <= 0)
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

            public static bool XSLTNameCheck(object target, RuleArgs e)
            {
                ReconSetup finalTarget = target as ReconSetup;
                if (finalTarget != null)
                {
                    if (finalTarget.XSLTName == string.Empty)
                    {
                        e.Description = "Choose XSLT File";
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

            public static bool FormatTypeCheck(object target, RuleArgs e)
            {
                ReconSetup finalTarget = target as ReconSetup;
                if (finalTarget != null)
                {
                    if (finalTarget.FormatType == string.Empty)
                    {
                        e.Description = "Type in a Format Type Name";
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

            public static bool RepeatSetupEntryCheck(object target, RuleArgs e)
            {
                ReconSetup sourceTarget = target as ReconSetup;
                ReconSetups reconSetups = (ReconSetups)sourceTarget.Parent;
                if (sourceTarget != null && reconSetups != null)
                {
                    int i = 0;
                    foreach (ReconSetup reconSetup in reconSetups)
                    {
                        if ((string.Compare(reconSetup.FormatType.Trim(), sourceTarget.FormatType.Trim(), true) == 0)
                            && reconSetup.ThirdPartyID == sourceTarget.ThirdPartyID
                            && reconSetup.ReconTypeID == sourceTarget.ReconTypeID)
                            i++;
                        if (i > 1)
                        {
                            e.Description = "Format Type already exists ";
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

        #endregion
    }
}
