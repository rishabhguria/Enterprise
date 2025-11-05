using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Utilities
{
    public static class SqlParser
    {
        /// <summary>
        /// This function is useful for making dynamic sql querries based on custom conditions..
        /// </summary>
        /// <returns>
        /// returns the dynamic sql statement  to be plugged after "Where" clause 
        /// </returns>
        public static string GetDynamicConditionQuerry(Dictionary<string, List<CustomCondition>> dictCustomConditions)
        {
            StringBuilder filterCondition = new StringBuilder();

            string defaultValue = string.Format("{0}{1}", "AND", "()");

            try
            {
                foreach (KeyValuePair<string, List<CustomCondition>> kp in dictCustomConditions)
                {

                    if (!kp.Key.Equals("-Select-"))
                    {
                        if (string.IsNullOrEmpty(filterCondition.ToString()))
                        {
                            filterCondition.Append(string.Format("{0}{1}", "AND", "("));
                        }
                        else
                        {
                            filterCondition.Append(string.Format("{0}{1}{2}", ")", "AND", "("));
                        }

                        int i = 0;
                        string likeConditionWithOr = string.Format("{0}{1}{2}", " OR ", kp.Key, " LIKE ");
                        string likeConditionWithoutOr = string.Format("{0}{1}", kp.Key, " LIKE ");

                        string NotlikeConditionWithAND = string.Format("{0}{1}{2}", " AND ", kp.Key, " NOT LIKE ");
                        string NotlikeConditionWithoutAND = string.Format("{0}{1}", kp.Key, " NOT LIKE ");

                        foreach (CustomCondition condition in kp.Value)
                        {
                            if (!string.IsNullOrEmpty(condition.CompareValue))
                            {

                                List<string> listConditionValues = GeneralUtilities.GetListFromString(condition.compareValue, ',');
                                foreach (string conditionValue in listConditionValues)
                                {
                                    switch (condition.ConditionOperatorType)
                                    {
                                        case Prana.BusinessObjects.AppConstants.EnumDescriptionAttribute.ConditionOperator.Equals:

                                            if (i != 0)
                                            {
                                                filterCondition.Append(likeConditionWithOr);
                                            }
                                            else
                                            {
                                                filterCondition.Append(likeConditionWithoutOr);
                                            }
                                            filterCondition.Append(string.Format("{0}{1}{2}", "'", conditionValue, "'"));

                                            break;

                                        case Prana.BusinessObjects.AppConstants.EnumDescriptionAttribute.ConditionOperator.NotEquals:
                                            if (i != 0)
                                            {

                                                filterCondition.Append(NotlikeConditionWithAND);
                                            }
                                            else
                                            {
                                                filterCondition.Append(NotlikeConditionWithoutAND);

                                            }
                                            //filterCondition.Append(Seperators.SEPERATOR_5);
                                            filterCondition.Append(string.Format("{0}{1}{2}", "'", conditionValue, "'"));

                                            break;


                                        case Prana.BusinessObjects.AppConstants.EnumDescriptionAttribute.ConditionOperator.Like:


                                            if (i != 0)
                                            {

                                                filterCondition.Append(likeConditionWithOr);
                                            }
                                            else
                                            {

                                                filterCondition.Append(likeConditionWithoutOr);


                                            }

                                            filterCondition.Append(string.Format("{0}{1}{2}{3}", "'", conditionValue, "%", "'"));

                                            break;

                                        case Prana.BusinessObjects.AppConstants.EnumDescriptionAttribute.ConditionOperator.NotLike:

                                            if (i != 0)
                                            {

                                                filterCondition.Append(NotlikeConditionWithAND);
                                            }
                                            else
                                            {
                                                filterCondition.Append(NotlikeConditionWithoutAND);

                                            }

                                            filterCondition.Append(string.Format("{0}{1}{2}{3}", "'", conditionValue, "%", "'"));

                                            break;

                                        case Prana.BusinessObjects.AppConstants.EnumDescriptionAttribute.ConditionOperator.StartsWith:

                                            if (i != 0)
                                            {
                                                filterCondition.Append(likeConditionWithOr);
                                            }
                                            else
                                            {
                                                filterCondition.Append(likeConditionWithoutOr);

                                            }

                                            filterCondition.Append(string.Format("{0}{1}{2}{3}", "'", conditionValue, "%", "'"));

                                            break;

                                        case Prana.BusinessObjects.AppConstants.EnumDescriptionAttribute.ConditionOperator.DoesNotStartWith:
                                            if (i != 0)
                                            {

                                                filterCondition.Append(NotlikeConditionWithAND);
                                            }
                                            else
                                            {
                                                filterCondition.Append(NotlikeConditionWithoutAND);

                                            }
                                            // filterCondition.Append(Seperators.SEPERATOR_5);
                                            filterCondition.Append(string.Format("{0}{1}{2}{3}", "'", conditionValue, "%", "'"));
                                            // filterCondition.Append(",");
                                            break;


                                        case Prana.BusinessObjects.AppConstants.EnumDescriptionAttribute.ConditionOperator.EndsWith:

                                            if (i != 0)
                                            {
                                                filterCondition.Append(likeConditionWithOr);
                                            }
                                            else
                                            {
                                                filterCondition.Append(likeConditionWithoutOr);

                                            }
                                            // filterCondition.Append(Seperators.SEPERATOR_5);
                                            filterCondition.Append(string.Format("{0}{1}{2}{3}", "'", "%", conditionValue, "'"));
                                            //  filterCondition.Append(",");

                                            break;
                                        case Prana.BusinessObjects.AppConstants.EnumDescriptionAttribute.ConditionOperator.DoesNotEndWith:

                                            if (i != 0)
                                            {
                                                filterCondition.Append(NotlikeConditionWithAND);
                                            }
                                            else
                                            {
                                                filterCondition.Append(NotlikeConditionWithoutAND);

                                            }
                                            //  filterCondition.Append(Seperators.SEPERATOR_5);
                                            filterCondition.Append(string.Format("{0}{1}{2}{3}", "'", "%", conditionValue, "'"));
                                            //  filterCondition.Append(",");

                                            break;

                                        case Prana.BusinessObjects.AppConstants.EnumDescriptionAttribute.ConditionOperator.Contains:

                                            if (i != 0)
                                            {
                                                filterCondition.Append(likeConditionWithOr);
                                            }
                                            else
                                            {
                                                filterCondition.Append(likeConditionWithoutOr);

                                            }
                                            //  filterCondition.Append(Seperators.SEPERATOR_5);
                                            filterCondition.Append(string.Format("{0}{1}{2}{3}{4}", "'", "%", conditionValue, "%", "'"));
                                            //  filterCondition.Append(",");
                                            break;


                                        case Prana.BusinessObjects.AppConstants.EnumDescriptionAttribute.ConditionOperator.DoesNotContain:
                                            if (i != 0)
                                            {
                                                filterCondition.Append(NotlikeConditionWithAND);
                                            }
                                            else
                                            {
                                                filterCondition.Append(NotlikeConditionWithoutAND);

                                            }
                                            //  filterCondition.Append(Seperators.SEPERATOR_5);
                                            filterCondition.Append(string.Format("{0}{1}{2}{3}{4}", "'", "%", conditionValue, "%", "'"));
                                            //  filterCondition.Append(",");

                                            break;
                                        default:
                                            break;
                                    }

                                    i++;


                                }

                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(filterCondition.ToString()))
                {
                    filterCondition.Append(")");

                }

            }


            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            if (!filterCondition.ToString().Equals(defaultValue))
            {
                return filterCondition.ToString();
            }
            else
                return string.Empty;
        }
    }

}