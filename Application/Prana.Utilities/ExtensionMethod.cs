using System;

namespace Prana.Utilities
{
    public static class ExtensionMethod
    {
        public static decimal ToDecimal(this double value)
        {
            double d = value + 1;
            decimal dd = Decimal.Parse(d.ToString());
            dd = dd - 1;
            return dd;
        }

        public static decimal ToDecimal(this double? value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            double d = (double)value + 1;
            decimal dd = Decimal.Parse(d.ToString());
            dd = dd - 1;
            return dd;
        }
    }
}
