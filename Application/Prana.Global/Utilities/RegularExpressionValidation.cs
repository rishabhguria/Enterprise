using System;
using System.Text.RegularExpressions;

namespace Prana.Global.Utilities
{
    /// <summary>
    /// Summary description for RegularExpressionValidation.
    /// </summary>
    public class RegularExpressionValidation
    {


        public static bool IsPositiveInteger(String strNumber)
        {
            Regex objNotIntPattern = new Regex("[^0-9-]");
            Regex objIntPattern = new Regex("^[0-9]+$|^[0-9]+$");
            return !objNotIntPattern.IsMatch(strNumber) && objIntPattern.IsMatch(strNumber);
        }
        public static bool IsNaturalNumber(String strNumber)
        {
            Regex objNotNaturalPattern = new Regex("[^0-9]");
            Regex objNaturalPattern = new Regex("0*[1-9][0-9]*");
            return !objNotNaturalPattern.IsMatch(strNumber) &&
                objNaturalPattern.IsMatch(strNumber);
        }

        // Function to test for Positive Integers with zero inclusive  

        public static bool IsWholeNumber(String strNumber)
        {
            Regex objNotWholePattern = new Regex("[^0-9]");
            return !objNotWholePattern.IsMatch(strNumber);
        }

        // Function to Test for Integers both Positive & Negative  

        public static bool IsInteger(String strNumber)
        {
            Regex objNotIntPattern = new Regex("[^0-9-]");
            Regex objIntPattern = new Regex("^-[0-9]+$|^[0-9]+$");
            return !objNotIntPattern.IsMatch(strNumber) && objIntPattern.IsMatch(strNumber);
        }

        // Function to Test for Positive Number both Integer & Real 

        public static bool IsPositiveNumber(String strNumber)
        {
            Regex objNotPositivePattern = new Regex("[^0-9.]");
            Regex objPositivePattern = new Regex("^[.][0-9]+$|[0-9]*[.]*[0-9]+$");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            return !objNotPositivePattern.IsMatch(strNumber) &&
                objPositivePattern.IsMatch(strNumber) &&
                !objTwoDotPattern.IsMatch(strNumber);
        }

        // Function to test whether the string is valid number or not
        //		public static bool IsNumber(String strNumber)
        //		{
        //			Regex objNotNumberPattern=new Regex("[^0-9.-]");
        //			Regex objTwoDotPattern=new Regex("[0-9]*[.][0-9]*[.][0-9]*");
        //			Regex objTwoMinusPattern=new Regex("[0-9]*[-][0-9]*[-][0-9]*");
        //			String strValidRealPattern="^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
        //			String strValidIntegerPattern="^([-]|[0-9])[0-9]*$";
        //			Regex objNumberPattern =new Regex("(" + strValidRealPattern +")|(" + strValidIntegerPattern + ")");
        //			return !objNotNumberPattern.IsMatch(strNumber) &&
        //				!objTwoDotPattern.IsMatch(strNumber) &&
        //				!objTwoMinusPattern.IsMatch(strNumber) &&
        //				objNumberPattern.IsMatch(strNumber);
        //		}  
        public static bool IsNumber(String strNumber)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]|([0-9]*[.][-]+[0-9*])");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");

            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]*$";
            //			String strValidRealPattern="^([-]|[.]|[-.]|[0-9])[0-9]*";

            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");
            return !objNotNumberPattern.IsMatch(strNumber) &&
                !objTwoDotPattern.IsMatch(strNumber) &&
                !objTwoMinusPattern.IsMatch(strNumber) &&
                objNumberPattern.IsMatch(strNumber);
        }
        // Function To test for Alphabets. 

        public static bool IsAlpha(String strToCheck)
        {
            Regex objAlphaPattern = new Regex("[^a-zA-Z]");
            return !objAlphaPattern.IsMatch(strToCheck);
        }
        // Function to Check for AlphaNumeric.
        public static bool IsAlphaNumeric(String strToCheck)
        {
            Regex objAlphaNumericPattern = new Regex("[^a-zA-Z0-9]");
            return !objAlphaNumericPattern.IsMatch(strToCheck);
        }
        // function to check for Valid(those allowed) Order side mapping
        public static bool IsSign(string strToCheck)
        {
            Regex objInPattern = new Regex("[- + * ]+$|[- + *]+$");
            Regex objNotInPattern = new Regex("[^- +  *]");
            return objInPattern.IsMatch(strToCheck) && !objNotInPattern.IsMatch(strToCheck);
        }


    }


}
