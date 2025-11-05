namespace Prana.BusinessObjects
{
    /// <summary>
    /// Object of this class will contain the properties of Dynamically loading 
    /// modules like Blotter, LiveFeed etc.
    /// </summary>
    public class DynamicClass
    {
        string msLocation = string.Empty; //Will contain location of Module assembly from which class has to be loaded.
        string msType = string.Empty; //Will contain class/form name which has to be loaded from the specified location.
        string msPrefControlType = string.Empty; //Will contain class/form name which has to be loaded from the specified location.
        string msDescription = string.Empty; //Description if required. 				
        public DynamicClass(string location, string type, string prefControlType, string description)
        {
            msLocation = location;
            msType = type;
            msPrefControlType = prefControlType;
            msDescription = description;
        }

        public string Location
        {
            get { return msLocation; }
            set { msLocation = value; }
        }

        public string Type
        {
            set { msType = value; }
            get { return msType; }
        }

        /// <summary>
        /// Added to add the entry for the preference control in respective assembly
        /// </summary>
        public string PrefControlType
        {
            set { msPrefControlType = value; }
            get { return msPrefControlType; }
        }

        public string Description
        {
            set { msDescription = value; }
            get { return msDescription; }
        }
    }
}
