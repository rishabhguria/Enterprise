using Lucene.Net.Index;
using Lucene.Net.Store;
using System.IO;

namespace Prana.SecuritySearch.Common
{
    public partial class CommonSearchBase
    {
        private string _IndexingDir;
        public string IndexingDir
        {
            get
            {
                return _IndexingDir;
            }

            set
            {
                _IndexingDir = CommonSearchConstants._parentDirectory + "\\" + value;
            }
        }

        private FSDirectory _directoryTemp;
        public FSDirectory _directory
        {
            get
            {
                if (_directoryTemp == null)
                {
                    var parentDirectory = new DirectoryInfo(CommonSearchConstants._parentDirectory);
                    if (!parentDirectory.Exists)
                        parentDirectory.Create();
                    _directoryTemp = FSDirectory.Open(new DirectoryInfo(_IndexingDir));
                }

                if (IndexWriter.IsLocked(_directoryTemp))
                    IndexWriter.Unlock(_directoryTemp);

                var lockFilePath = Path.Combine(_IndexingDir, "write.lock");

                if (File.Exists(lockFilePath))
                {

                    if (!IsFileLocked(lockFilePath))
                        File.Delete(lockFilePath);
                }

                return _directoryTemp;
            }
        }

        /// <summary>
        /// Is File Locked
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected bool IsFileLocked(string filePath)
        {
            FileStream stream = null;
            FileInfo file = new FileInfo(filePath);
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }
}
