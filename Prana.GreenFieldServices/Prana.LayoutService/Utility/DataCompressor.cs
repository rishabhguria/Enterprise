using System.IO;
using System.IO.Compression;
using System.Text;

namespace Prana.LayoutService.Utility
{
    public class DataCompressor
    {
        /// <summary>
        /// Takes string data and returns compressed byte[] stream .
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] CompressData(string data)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    using (var writer = new StreamWriter(gzipStream, Encoding.UTF8))
                    {
                        writer.Write(data);
                    }
                }
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Takes compressed byte[] data and decompress it and return in string format
        /// </summary>
        /// <param name="compressedData"></param>
        /// <returns></returns>
        public static string DecompressData(byte[] compressedData)
        {
            using (var compressedStream = new MemoryStream(compressedData))
            {
                using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    using (var reader = new StreamReader(gzipStream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}
