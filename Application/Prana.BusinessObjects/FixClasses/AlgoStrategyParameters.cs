using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class AlgoStrategyParameters
    {
        //Structure containing Parameters
        public string ControlType;
        public string ControlCategory;
        public string ChildParameterID;
        //public AlgoStrategyParametersToUpdate _algoStrategyParametersToUpdate;
        public Dictionary<string, Dictionary<string, AlgoStrategyParametersToUpdate>> _algoStrategyParametersToUpdate;
        public List<string> InnerControlNames;
        public List<string> InnerControlValues;
        public int Xpos;
        public int Ypos;
        public string Type;
        public string ReplaceVisible;

        public List<string> IDs;

        public List<string> Names;

        public Dictionary<string, string> IDtoNameMapping;
        public List<string> ValidateWith;
        public List<string> ValidateWithOrderProperty;
        public string DefaultValue;
        public bool IsRequired = false;
        public string Format = string.Empty;
        public bool SendOnReplace = true;
        public Dictionary<string, List<string>> ValueWiseRegionAvailabilty = new Dictionary<string, List<string>>();
        public Dictionary<string, string> RegionWiseDefaultValues = new Dictionary<string, string>();
        public Dictionary<string, string> CustomAttributesDict = new Dictionary<string, string>();
        public List<string> AvailableInRegions;
        public List<string> RequiredInRegions;
        public List<string> EnableInRegions;
        public bool IsEnabled = true;
        public string ReuiredGroupName = string.Empty;
    }
}
