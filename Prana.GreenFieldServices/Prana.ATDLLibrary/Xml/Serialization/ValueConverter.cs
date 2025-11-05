using Prana.ATDLLibrary.Diagnostics.Exceptions;
using Prana.ATDLLibrary.Fix;
using Prana.ATDLLibrary.Resources;
using System;
using System.Globalization;
using ThrowHelper = Prana.ATDLLibrary.Diagnostics.ThrowHelper;

namespace Prana.ATDLLibrary.Xml.Serialization
{
    public class ValueConverter
    {
        public static readonly string ExceptionContext = typeof(ValueConverter).Name;

        public static T ConvertTo<T>(string value)
        {
            return (T)ConvertTo(value, typeof(T));
        }

        public static object ConvertTo(string value, System.Type targetType)
        {
            switch (targetType.FullName)
            {
                case "System.String":
                    return value;

                case "System.Char":
                    if (value.Length == 1)
                        return Convert.ToChar(value);
                    else
                        throw ThrowHelper.New<InvalidFieldValueException>(ExceptionContext, ErrorMessages.InvalidCharValue, value);

                case "System.Boolean":
                    return bool.Parse(value);

                case "System.Int32":
                    return Convert.ToInt32(value, CultureInfo.InvariantCulture);

                case "System.Decimal":
                    return Convert.ToDecimal(value, CultureInfo.InvariantCulture);

                case "System.DateTime":
                    {
                        DateTime result;

                        if (!FixDateTime.TryParse(value, CultureInfo.InvariantCulture, out result))
                            throw ThrowHelper.New<InvalidFieldValueException>(ExceptionContext, ErrorMessages.InvalidDateOrTimeValue, value);

                        return result;
                    }

                case "Prana.ATDLLibrary.Fix.FixTag":
                    return new FixTag(Convert.ToInt32(value, CultureInfo.InvariantCulture));

                default:
                    if (targetType.FullName.StartsWith("Prana.ATDLLibrary.Model.Controls.InitValue"))
                        return value;
                    else
                        throw ThrowHelper.New<InternalErrorException>(ExceptionContext, InternalErrors.UnrecognisedAttributeType, targetType.FullName);
            }
        }
    }
}
