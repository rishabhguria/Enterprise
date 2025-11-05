using System;

namespace Prana.InstallerUtilities.Classes
{
    public class InstallationDetails
    {
        private Guid? _updateCode;
        public Guid? UpdateCode
        {
            get { return _updateCode; }
            set { _updateCode = value; }
        }

        private Guid _productCode;
        public Guid ProductCode
        {
            get { return _productCode; }
            set { _productCode = value; }
        }

        private String _path;
        public String Path
        {
            get { return _path; }
            set { _path = value; }
        }

        private String _name;
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}