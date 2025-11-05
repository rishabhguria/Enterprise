using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace System
{
    public static class SerializeHash
    {
        public static byte[] ComputeHash<T>(this T _Item)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, _Item);
                using (MD5 md5 = MD5.Create())
                {
                    return md5.ComputeHash(stream.ToArray());
                }
            }
        }
    }
}
