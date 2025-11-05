using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.Utilities
{
    public class DataTableExpressionHelper
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customColumns"></param>
        public static void ConvertUGExpressionToDTExpression(Dictionary<string, string> customColumns)
        {
            try
            {
                if (customColumns != null)
                {
                    //replace if "if" clause exist in formula expression exist
                    ReplaceStringInFormula(customColumns, "if(", "iif(");
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
        }
        private static void ReplaceStringInFormula(Dictionary<string, string> customColumns, string str1, string str2)
        {
            try
            {
                customColumns.Keys.ToList().ForEach
                    (customColumn =>
                    {
                        if (customColumns[customColumn] != null)
                        {
                            if (customColumns[customColumn].IndexOf(str1, StringComparison.InvariantCultureIgnoreCase) >= 0)
                            {
                                customColumns[customColumn] = customColumns[customColumn].ToString().ToLower().Replace(str1, str2);
                                //customColumns[customColumn] = Regex.Replace(customColumns[customColumn], str1, str2, RegexOptions.IgnoreCase);
                                //customColumns[customColumn] = Regex.Replace(customColumns[customColumn], str1, str2, RegexOptions.IgnoreCase);
                            }
                        }
                    }
                );
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



    }
}
