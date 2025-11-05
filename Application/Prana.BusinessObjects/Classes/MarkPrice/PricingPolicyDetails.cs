using System;

namespace Prana.BusinessObjects
{
    public class PricePolicyDetails
    {
        public int PricingID { get; set; }
        public bool IsActive { get; set; }
        public string PolicyName { get; set; }
        public string SPName { get; set; }
        public bool IsFileAvailable { get; set; }
        public string FilePath { get; set; }
        public string FolderPath { get; set; }


        public PricePolicyDetails()
        {
            PricingID = int.MinValue;
            IsActive = false;
            PolicyName = String.Empty;
            SPName = String.Empty;
            IsFileAvailable = false;
            FilePath = String.Empty;
            FolderPath = String.Empty;
        }
    }
}
