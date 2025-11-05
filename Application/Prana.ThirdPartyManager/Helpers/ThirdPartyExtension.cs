namespace System
{
    public static class Ext
    {
        public static string ShortDateStr(DateTime date)
        {
            const string format = "MM/dd/yyyy";

            string value = date.ToShortDateString();
            if (string.IsNullOrEmpty(value)) return value;
            return Convert.ToDateTime(value).ToString(format);
        }
        public static string ShortDateStr(string value)
        {
            const string format = "MM/dd/yyyy";
            if (string.IsNullOrEmpty(value)) return value;

            return Convert.ToDateTime(value).ToString(format);
        }
        public static bool IsNull(string value)
        {
            return string.IsNullOrEmpty(value);
        }
        public static bool IsNotNull(string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}
