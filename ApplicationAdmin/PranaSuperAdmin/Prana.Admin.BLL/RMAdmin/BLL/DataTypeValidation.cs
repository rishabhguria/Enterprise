using System;

namespace Prana.Admin.BLL
{
    public class DataTypeValidation
    {
        #region Constructors

        public DataTypeValidation()
        {

        }

        #endregion Constructors

        #region NumericValidation
        /// <summary>
        /// The method to check the validity of entered data for numeric datatype.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ValidateNumeric(string str)
        {
            bool validate = false;

            System.Text.RegularExpressions.Regex rgnumber = new System.Text.RegularExpressions.Regex(@"^\d+$");

            if (rgnumber.IsMatch(str))
            {
                validate = true;
            }
            return validate;
        }

        public static bool ValidateMaxPermiitedNumeric(string str)
        {
            bool validate = false;

            Int64 enterdValue = Convert.ToInt64(str);
            if (enterdValue < Int64.MaxValue)
            {
                validate = true;
            }
            return validate;
        }
        #endregion NumericValidation


    }
}
