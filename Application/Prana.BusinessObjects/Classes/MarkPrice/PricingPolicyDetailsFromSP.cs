namespace Prana.BusinessObjects
{
    public class PricingPolicyDetailsFromSP
    {
        public int AccountID { get; set; }
        public string Symbol { get; set; }
        public string PricingField { get; set; }

        public PricingPolicyDetailsFromSP()
        {
            AccountID = int.MinValue;
            Symbol = string.Empty;
            PricingField = string.Empty;
        }
    }
}
