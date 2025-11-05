using Prana.BusinessObjects.AppConstants;
//using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class MatchingRule
    {
        string name = "Matching Criteria :: ";
        Dictionary<string, RuleParameters> rulefieldCollection = new Dictionary<string, RuleParameters>();
        Dictionary<string, RuleParameters> _includedRulefieldCollection = new Dictionary<string, RuleParameters>();
        public MatchingRule(XmlNode xmlNode)
        {
            try
            {
                XmlNodeList xmlNodes = xmlNode.SelectNodes("Parameter");
                foreach (XmlNode xmlNodeMatchingRule in xmlNodes)
                {
                    RuleParameters ruleParams = new RuleParameters();

                    string field = xmlNodeMatchingRule.Attributes["Name"].Value.ToString();
                    int type = int.Parse(xmlNodeMatchingRule.Attributes["Type"].Value.ToString());
                    double tolerance = new double();
                    int roundDigits = new int();
                    // http://jira.nirvanasolutions.com:8080/browse/CHMW-1358
                    // Modified by Ankit Gupta on 29 Aug, 2014
                    double AbsDiff = new double();
                    bool isIncluded = new bool();
                    bool isRoundOff = new bool();
                    bool isIntegral = new bool();
                    bool isPercentMatch = new bool();
                    bool isAbsoluteMatch = new bool();
                    bool ismismatchExactReconColumn = new bool();
                    if (xmlNodeMatchingRule.Attributes["ErrorTolerance"] != null)
                    {
                        tolerance = double.Parse(xmlNodeMatchingRule.Attributes["ErrorTolerance"].Value.ToString());
                    }
                    if (xmlNodeMatchingRule.Attributes["IsIncluded"] != null)
                    {
                        isIncluded = bool.Parse(xmlNodeMatchingRule.Attributes["IsIncluded"].Value.ToString());
                    }
                    if (xmlNodeMatchingRule.Attributes["IsRoundOff"] != null)
                    {
                        isRoundOff = bool.Parse(xmlNodeMatchingRule.Attributes["IsRoundOff"].Value.ToString());
                    }
                    if (xmlNodeMatchingRule.Attributes["RoundDigits"] != null)
                    {
                        roundDigits = int.Parse(xmlNodeMatchingRule.Attributes["RoundDigits"].Value.ToString());
                    }
                    if (xmlNodeMatchingRule.Attributes["IsIntegral"] != null)
                    {
                        isIntegral = bool.Parse(xmlNodeMatchingRule.Attributes["IsIntegral"].Value.ToString());
                    }
                    if (xmlNodeMatchingRule.Attributes["IsPercentMatch"] != null)
                    {
                        isPercentMatch = bool.Parse(xmlNodeMatchingRule.Attributes["IsPercentMatch"].Value.ToString());
                    }
                    if (xmlNodeMatchingRule.Attributes["IsAbsoluteMatch"] != null)
                    {
                        isAbsoluteMatch = bool.Parse(xmlNodeMatchingRule.Attributes["IsAbsoluteMatch"].Value.ToString());
                    }
                    if (xmlNodeMatchingRule.Attributes["AbsoluteDifference"] != null)
                    {
                        AbsDiff = double.Parse(xmlNodeMatchingRule.Attributes["AbsoluteDifference"].Value.ToString());
                    }

                    if (xmlNodeMatchingRule.Attributes["mismatchExactReconColumn"] != null)
                    {
                        ismismatchExactReconColumn = bool.Parse(xmlNodeMatchingRule.Attributes["mismatchExactReconColumn"].Value.ToString());
                    }

                    ruleParams.FieldName = field;
                    ruleParams.Type = (ComparisionType)type;
                    ruleParams.ErrorTolerance = tolerance;
                    ruleParams.RoundDigits = roundDigits;
                    ruleParams.IsIncluded = isIncluded;
                    ruleParams.IsRoundOff = isRoundOff;
                    ruleParams.IsIntegralMatch = isIntegral;
                    ruleParams.IsPercentMatch = isPercentMatch;
                    ruleParams.IsAbsoluteDifference = isAbsoluteMatch;
                    ruleParams.AbsDiff = AbsDiff;
                    ruleParams.MismatchExactReconColumn = ismismatchExactReconColumn;
                    Add(field, ruleParams);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private MatchingRule()
        {

        }

        private static MatchingRule _matchingRule;
        /// <summary>
        /// Singleton instance of DataAccess class
        /// </summary>
        /// <returns></returns>
        public static MatchingRule GetInstance()
        {
            if (_matchingRule == null)
            {
                _matchingRule = new MatchingRule();
            }
            return _matchingRule;
        }

        public void Add(string field, RuleParameters ruleParameteres)
        {
            try
            {
                rulefieldCollection.Add(field, ruleParameteres);
                if (ruleParameteres.IsIncluded)
                    _includedRulefieldCollection.Add(field, ruleParameteres);
                name += "," + field + "  " + ruleParameteres.Type.ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        //private IReconServices _reconServices;

        //public IReconServices ReconServices
        //{
        //    get { return _reconServices; }
        //    set { _reconServices = value; }
        //}

        //validate selected matching rules and check that that columns exist in both Nirvana and PB data
        public StringBuilder ValidateMatchingRules(DataTable dtAppDataToMatch, DataTable dtPBDataToMatch)
        {
            StringBuilder ReconErrorMessage = new StringBuilder();
            try
            {
                //ComparisonFields is the list of string type which contains all the matching rules which are checked
                foreach (string matchingRule in ComparisonFields)
                {
                    if (!dtAppDataToMatch.Columns.Contains(matchingRule))
                        ReconErrorMessage.Append("Matching Rule " + matchingRule + " not available in Nirvana Data \n");
                    if (!dtPBDataToMatch.Columns.Contains(matchingRule))
                        ReconErrorMessage.Append("Matching Rule " + matchingRule + " not available in PB Data \n");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return ReconErrorMessage;
        }

        public Dictionary<string, RuleParameters> GetRuleFieldCollection()
        {
            return rulefieldCollection;
        }

        public Dictionary<string, RuleParameters> GetInculdedRuleFieldCollection()
        {
            return _includedRulefieldCollection;
        }

        #region Commented
        //public Result IsRulePassed(DataRow row1, DataRow row2)
        //{
        //    Result allCondiditionMatched = new Result();
        //    try
        //    {
        //    allCondiditionMatched.IsPassed = true;
        //    allCondiditionMatched.Text = String.Empty;
        //    allCondiditionMatched.MisMatchType = String.Empty;

        //    // The item with tolerance should be only one and last in the rule list
        //    // TODO: ENHANCEMENT REQUIRED !!!
        //    foreach (KeyValuePair<string, RuleParameters> ruleItem in rulefieldCollection)
        //    {
        //        if (ruleItem.Value.IsIncluded)
        //        {
        //            string var1 = row1[ruleItem.Key].ToString().Trim().ToUpper();
        //            string var2 = row2[ruleItem.Key].ToString().Trim().ToUpper();
        //            DateTime dateValue;
        //            double doubleValue;
        //            //Narendra Kumar Jangir 2013 Mar 07
        //            //DateTime.TryParse would be true for date
        //            //double.TryParse is used to hadle doubles(like 1.18) which can be parsed in date
        //            //http://jira.nirvanasolutions.com:8080/browse/LAZARD-6
        //            if ((DateTime.TryParse(var1, out dateValue)) && (!double.TryParse(var1, out doubleValue)))
        //            {
        //                var1 = dateValue.Date.ToShortDateString();
        //            }

        //            if ((DateTime.TryParse(var2, out dateValue)) && (!double.TryParse(var2, out doubleValue)))
        //            {
        //                var2 = dateValue.Date.ToShortDateString();
        //            }
        //            Result result = new Result();
        //            result.Text = string.Empty;
        //            switch (ruleItem.Value.Type)
        //            {
        //                case ComparisionType.Exact:
        //                    result.IsPassed = ComparingLogic.ExactMatch(var1, var2);
        //                    break;
        //                case ComparisionType.Partial:
        //                    result.IsPassed = ComparingLogic.PartialMatch(var1, var2);
        //                    break;
        //                case ComparisionType.Numeric:
        //                    result = ComparingLogic.NumericMatch(ref var1, ref var2, ruleItem.Value);
        //                    row1[ruleItem.Key] = var1;
        //                    row2[ruleItem.Key] = var2;
        //                    if (result.IsPassed == true && result.Text != String.Empty)
        //                    {
        //                        if (allCondiditionMatched.Text == String.Empty)
        //                        {
        //                            allCondiditionMatched.Text = result.Text;
        //                        }
        //                        else
        //                        {
        //                            allCondiditionMatched.Text += ", " + result.Text;
        //                        }

        //                        allCondiditionMatched.MisMatchType = result.MisMatchType;
        //                    }
        //                    break;
        //            }

        //            if (!result.IsPassed)
        //            {
        //                allCondiditionMatched.IsPassed = false;
        //                allCondiditionMatched.Text = String.Empty;
        //                allCondiditionMatched.MisMatchType = string.Empty;
        //                break;
        //            }


        //        }
        //    }
        //    if (allCondiditionMatched.IsPassed && allCondiditionMatched.MisMatchType == String.Empty)
        //    {
        //        allCondiditionMatched.MisMatchType = "Exactly Matched";
        //    }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return allCondiditionMatched;
        //}
        //public Result IsRulePartiallyPassed(DataRow row1, DataRow row2)
        //{

        //    string text = String.Empty;
        //    //  List<string> unmatchedRules = new List<string>();
        //    Result allUnmatchedRules = new Result();
        //    allUnmatchedRules.MisMatchType = string.Empty;
        //    allUnmatchedRules.Text = string.Empty;

        //    foreach (KeyValuePair<string, RuleParameters> ruleItem in rulefieldCollection)
        //    {
        //        if (ruleItem.Value.IsIncluded)
        //        {
        //            string var1 = string.Empty;
        //            string var2 = string.Empty;
        //            if (row1.Table.Columns.Contains(ruleItem.Key))
        //                var1 = row1[ruleItem.Key].ToString().Trim().ToUpper();   
        //            if (row2.Table.Columns.Contains(ruleItem.Key))
        //                var2 = row2[ruleItem.Key].ToString().Trim().ToUpper();

        //            Result result = new Result();
        //            result.IsPassed = false;
        //            result.Text = String.Empty;

        //            switch (ruleItem.Value.Type)
        //            {
        //                case ComparisionType.Exact:
        //                    result.IsPassed = ComparingLogic.ExactMatch(var1, var2);
        //                    break;
        //                case ComparisionType.Partial:
        //                    result.IsPassed = ComparingLogic.PartialMatch(var1, var2);
        //                    break;
        //                case ComparisionType.Numeric:
        //                    result = ComparingLogic.NumericMatch(ref var1, ref var2, ruleItem.Value);
        //                    break;
        //            }
        //            if (!result.IsPassed)
        //            {
        //                if (ruleItem.Key != "Symbol")
        //                {

        //                    if (allUnmatchedRules.MisMatchType == string.Empty)
        //                    {
        //                        if (ruleItem.Key == "AccountName")
        //                        {
        //                            allUnmatchedRules.MisMatchType = "Allocation Mismatch";
        //                        }
        //                        else
        //                        {
        //                            allUnmatchedRules.MisMatchType = ruleItem.Key + ' ' + "Mismatch";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        allUnmatchedRules.MisMatchType = "Multiple Mismatches";
        //                    }
        //                    if (allUnmatchedRules.Text != string.Empty)
        //                    {
        //                        allUnmatchedRules.Text += ',';
        //                    }
        //                    allUnmatchedRules.Text += ruleItem.Key + ' ';
        //                }
        //            }
        //        }
        //    }

        //    if (allUnmatchedRules.Text != string.Empty)
        //    {
        //        allUnmatchedRules.Text += "Mismatch with PB";
        //    }
        //    return allUnmatchedRules;

        //}
        #endregion

        public string Name
        {
            get
            {
                return name;
            }
        }
        public DataTable Data
        {
            get
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Field");
                dt.Columns.Add("MatchingType");
                foreach (KeyValuePair<string, RuleParameters> item in rulefieldCollection)
                {
                    dt.Rows.Add(new object[] { item.Key, item.Value.Type });
                }
                return dt;
            }
        }
        public List<string> Fields
        {
            get
            {
                List<string> fields = new List<string>();
                foreach (KeyValuePair<string, RuleParameters> item in rulefieldCollection)
                {
                    fields.Add(item.Key);
                }
                return fields;
            }
        }

        public List<string> ComparisonFields
        {
            get
            {

                List<string> comparisonFields = new List<string>();

                foreach (KeyValuePair<string, RuleParameters> item in rulefieldCollection)
                {
                    if (item.Value.IsIncluded)
                    {
                        comparisonFields.Add(item.Key);
                    }
                }
                return comparisonFields;
            }

        }

        // This dictionary gives the Exact mismatch Recon columns & their type

        public Dictionary<String, ComparisionType> ExactmismatchReconFields
        {
            get
            {

                Dictionary<String, ComparisionType> ExactMismatchcompareFields = new Dictionary<String, ComparisionType>();

                foreach (KeyValuePair<string, RuleParameters> item in rulefieldCollection)
                {
                    if (item.Value.IsIncluded && item.Value.MismatchExactReconColumn)
                    {
                        ExactMismatchcompareFields.Add(item.Key, item.Value.Type);
                    }
                }
                return ExactMismatchcompareFields;
            }

        }
        public List<string> NumericFields
        {
            get
            {
                List<string> NumericFields = new List<string>();
                foreach (KeyValuePair<string, RuleParameters> item in rulefieldCollection)
                {
                    //if (item.Value.IsIncluded && item.Value.Type.Equals(ComparisionType.Numeric))
                    //{
                    if (item.Value.Type.Equals(ComparisionType.Numeric))
                    {
                        NumericFields.Add(item.Key);
                    }
                    //}
                }
                return NumericFields;
            }
        }
    }

    public struct Result
    {
        private bool _isPassed;

        public bool IsPassed
        {
            get { return _isPassed; }
            set { _isPassed = value; }
        }

        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private string _misMatchType;

        public string MisMatchType
        {
            get { return _misMatchType; }
            set { _misMatchType = value; }
        }

        private ToleranceType _toleranceType;

        public ToleranceType ToleranceType
        {
            get { return _toleranceType; }
            set { _toleranceType = value; }
        }

        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1358
        // Modified by Ankit Gupta on 29 Aug, 2014
        private double _toleranceValue;

        public double ToleranceValue
        {
            get { return _toleranceValue; }
            set { _toleranceValue = value; }
        }

        private ReconStatus _reconStaus;
        public ReconStatus ReconStaus
        {
            get { return _reconStaus; }
            set { _reconStaus = value; }
        }

        private string _columnName;
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
    }
    [Serializable]
    public struct RuleParameters
    {
        private string _fieldName;
        private ComparisionType _type;
        private double _errorTolerance;
        private int _roundDigits;
        private bool _isRoundOff;
        private bool _isIntegralMatch;
        private bool _isIncluded;
        private bool _isPercentMatch;
        private bool _isAbsoluteDifference;
        private bool _mismatchExactReconColumn;

        public bool MismatchExactReconColumn
        {
            get { return _mismatchExactReconColumn; }
            set { _mismatchExactReconColumn = value; }
        }
        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1358
        // Modified by Ankit Gupta on 29 Aug, 2014
        private double _absDiff;
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        public ComparisionType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public double ErrorTolerance
        {
            get { return _errorTolerance; }
            set { _errorTolerance = value; }
        }

        public int RoundDigits
        {
            get { return _roundDigits; }
            set { _roundDigits = value; }
        }

        public bool IsRoundOff
        {
            get { return _isRoundOff; }
            set { _isRoundOff = value; }
        }

        public bool IsIntegralMatch
        {
            get { return _isIntegralMatch; }
            set { _isIntegralMatch = value; }
        }

        public bool IsIncluded
        {
            get { return _isIncluded; }
            set { _isIncluded = value; }
        }

        public bool IsPercentMatch
        {
            get { return _isPercentMatch; }
            set { _isPercentMatch = value; }
        }

        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1358
        // Modified by Ankit Gupta on 29 Aug, 2014		
        public double AbsDiff
        {
            get { return _absDiff; }
            set { _absDiff = value; }
        }

        public bool IsAbsoluteDifference
        {
            get { return _isAbsoluteDifference; }
            set { _isAbsoluteDifference = value; }
        }
    }
}
