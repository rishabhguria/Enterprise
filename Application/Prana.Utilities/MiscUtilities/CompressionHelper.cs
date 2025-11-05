using Prana.LogManager;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Prana.Utilities.MiscUtilities
{
    /// <summary>
    /// http://www.csharphelp.com/archives4/archive689.html
    /// The strings need to be longer than 3400 characters; otherwise the compression rate is not good enough. 
    /// </summary>
    public class CompressionHelper
    {
        public static string Zip(string text, byte[] byteArray)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                text = null;
                MemoryStream ms = new MemoryStream();
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    zip.Write(buffer, 0, buffer.Length);
                }
                int length = buffer.Length;
                buffer = null;
                ms.Position = 0;

                byte[] compressed = new byte[ms.Length];
                ms.Read(compressed, 0, compressed.Length);
                ms = null;
                byteArray = new byte[compressed.Length + 4];
                System.Buffer.BlockCopy(compressed, 0, byteArray, 4, compressed.Length);
                compressed = null;
                System.Buffer.BlockCopy(BitConverter.GetBytes(length), 0, byteArray, 0, 4);
                return Convert.ToBase64String(byteArray);
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
            return null;
        }

        public static string UnZip(string compressedText, byte[] gzBuffer)
        {
            try
            {
                gzBuffer = Convert.FromBase64String(compressedText);
                compressedText = null;
                using (MemoryStream ms = new MemoryStream())
                {

                    int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                    ms.Write(gzBuffer, 4, gzBuffer.Length - 4);
                    gzBuffer = null;
                    ms.Position = 0;
                    gzBuffer = new byte[msgLength];
                    GZipStream zip = new GZipStream(ms, CompressionMode.Decompress);
                    zip.Read(gzBuffer, 0, gzBuffer.Length);
                    return Encoding.UTF8.GetString(gzBuffer);
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
            return null;
        }

    }
}
