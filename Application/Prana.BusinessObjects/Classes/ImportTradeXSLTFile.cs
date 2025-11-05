using System;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    public class ImportTradeXSLTFile
    {
        private string _emsSource;

        public string EMSSource
        {
            get { return _emsSource; }
            set { _emsSource = value; }
        }

        private int _importSourceID;

        [Browsable(false)]
        public int ImportSourceID
        {
            get { return _importSourceID; }
            set { _importSourceID = value; }
        }

        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        private int _fileID;

        [Browsable(false)]
        public int FileID
        {
            get { return _fileID; }
            set { _fileID = value; }
        }

        private byte[] _binaryData;

        [Browsable(false)]
        public byte[] BinaryData
        {
            get { return _binaryData; }
            set { _binaryData = value; }
        }

        private DateTime _lastSaveTime;

        [Browsable(false)]
        public DateTime LastSaveTime
        {
            get { return _lastSaveTime; }
            set { _lastSaveTime = value; }
        }


    }
}
