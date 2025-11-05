
using System;

namespace Prana.Utilities.IO
{


    public class StringByteConversion
    {
        static System.Text.Encoding _utf8 = System.Text.Encoding.UTF8;
        public static byte[] GetBytes(String msg)
        {
            return _utf8.GetBytes(msg);
        }
        public static string GetString(byte[] data)
        {
            return System.Text.Encoding.UTF8.GetString(data, 0, data.Length);

        }
    }
}
