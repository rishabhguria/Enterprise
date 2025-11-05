using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class AlgoStrategyParametersToUpdate
    {
        public string MinValue;
        public string MaxValue;
        public bool Enabled = true;
        public bool Visibility = true;
        public string Default;
        public string Increment;
        public bool Checked = true;
        public string SelectedValue;
        public bool Required = false;
        public string ReuiredGroupName;
        public List<string> ValidateWithOrderProperty;
    }
}
