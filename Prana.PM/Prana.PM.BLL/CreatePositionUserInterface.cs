namespace Prana.PM.BLL
{
    public class CreatePositionUserInterface
    {
        private string _template = string.Empty;

        public string Template
        {
            get { return _template; }
            set { _template = value; }
        }



        private string _importMethod = string.Empty;

        public string ImportMethod
        {
            get { return _importMethod; }
            set { _importMethod = value; }
        }

        private string _fileType = string.Empty;

        public string FileType
        {
            get { return _fileType; }
            set { _fileType = value; }
        }

        private OTCPositionList _OTCPositions = new OTCPositionList();

        public OTCPositionList OTCPositions
        {
            get { return _OTCPositions; }
            set { _OTCPositions = value; }
        }





    }
}
