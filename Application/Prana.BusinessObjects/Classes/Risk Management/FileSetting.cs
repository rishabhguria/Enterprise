using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class FileSetting
    {

        private string _xmlPath;

        public string TempXmlPath
        {
            get { return _xmlPath; }
            set { _xmlPath = value; }
        }


        private string _xsltPath;

        public string XsltPath
        {
            get { return _xsltPath; }
            set { _xsltPath = value; }
        }


        private string _inputFilePath;

        public string InputFilePath
        {
            get { return _inputFilePath; }
            set { _inputFilePath = value; }
        }

        private string _outPutFilePath;

        public string OutPutFilePath
        {
            get { return _outPutFilePath; }
            set { _outPutFilePath = value; }
        }

        private string _XSDPath;

        public string XSDPath
        {
            get { return _XSDPath; }
            set { _XSDPath = value; }
        }

        private string _thirdPartyName;

        public string ThirdPartyName
        {
            get { return _thirdPartyName; }
            set { _thirdPartyName = value; }
        }

        public FileSetting()
        {

        }

        public FileSetting(string xmlPathArg, string xsltPathArg, string inputFilePathArg, string XSDArg, string OutPutFilePathArg, string thirdPartyAgr)
        {
            _xmlPath = xmlPathArg;
            _xsltPath = xsltPathArg;
            _inputFilePath = inputFilePathArg;
            _XSDPath = XSDArg;
            _outPutFilePath = OutPutFilePathArg;
            _thirdPartyName = thirdPartyAgr;
        }
    }
}
