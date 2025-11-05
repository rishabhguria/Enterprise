using System;
using System.IO;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class FileMetaData
    {
        public FileMetaData()
        {
        }
        private string _encryptedFileName;
        public string EncryptedFileName
        {
            get { return _encryptedFileName; }
            set { _encryptedFileName = value; }
        }


        private string _originalFileName;
        public string OriginalFileName
        {
            get { return _originalFileName; }
            set { _originalFileName = value; }
        }

        private int _noOFColumns;
        public int NoOFColumns
        {
            get { return _noOFColumns; }
            set { _noOFColumns = value; }
        }

        private int _noOfRows;
        public int NoOfRows
        {
            get { return _noOfRows; }
            set { _noOfRows = value; }
        }


        private FileAttributes _attributes;
        public FileAttributes Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        private DateTime _creationTimeUtc;
        public DateTime CreationTimeUtc
        {
            get { return _creationTimeUtc; }
            set { _creationTimeUtc = value; }
        }


        private string _directoryName;
        public string DirectoryName
        {
            get { return _directoryName; }
            set { _directoryName = value; }
        }

        private bool _exists;
        public bool Exists
        {
            get { return _exists; }
            set { _exists = value; }
        }

        private string _extension;
        public string Extension
        {
            get { return _extension; }
            set { _extension = value; }
        }

        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }

        private bool _isReadOnly;
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; }
        }

        private DateTime _lastAccessTimeUtc;
        public DateTime LastAccessTimeUtc
        {
            get { return _lastAccessTimeUtc; }
            set { _lastAccessTimeUtc = value; }
        }

        private DateTime _lastWriteTimeUtc;
        public DateTime LastWriteTimeUtc
        {
            get { return _lastWriteTimeUtc; }
            set { _lastWriteTimeUtc = value; }
        }

        private long _length;
        public long Length
        {
            get { return _length; }
            set { _length = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _size;
        public string Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public string GetSize()
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (_length == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(_length);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(_length) * num).ToString() + suf[place];
        }
    }
}
