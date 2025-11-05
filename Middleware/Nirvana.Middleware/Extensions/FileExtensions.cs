using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace System.IO
{
    /// <summary>
    /// File IO Extensions
    /// </summary>
    /// <remarks></remarks>
    public static class FileExtensions
    {

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool Delete(string file)
        {
            try
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                    return true;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
