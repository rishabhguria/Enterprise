namespace Prana.ServiceGateway.Models
{
    public class CompanyUser
    {
        public int? CompanyUserID { get; set; }
        public int? CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string ShortName { get; set; }
        public string Title { get; set; }
        public string MailingAddress { get; set; }
        public string EMail { get; set; }
        public string TelephoneWork { get; set; }
        public string TelephoneHome { get; set; }
        public string TelephoneMobile { get; set; }
        public string Fax { get; set; }
        public string LoginID { get; set; }
        public List<TradingAccount> TradingAccounts { get; set; }
        public List<string> MarketDataTypes { get; set; }
        public string FactSetUsernameAndSerialNumber { get; set; }
        public bool? IsFactSetSupportUser { get; set; }
        public string MarketDataAccessIPAddresses { get; set; }
        public string ActivUsername { get; set; }
        public string ActivPassword { get; set; }
        public bool? HasPowerBIAccess { get; set; }
        public string SapiUsername { get; set; }
    }

    //Before updating/removing please check the references
    //in frontend code
    public class LoginUserDto
    {
        public int CompanyUserId { get; set; }
        public string ErrorMessage { get; set; }
        public string token { get; set; }
        public int? AuthenticationType { get; set; }
        public CompanyUser CompanyUser { get; set; }
        public string TouchOtk { get; set; }
        public string CorrelationId { get; set; }
        public int? CompanyMarketDataProvider { get; set; }
        public int? CompanyFactSetContractType { get; set; }
        public bool IsMarketDataBlocked { get; set; }
        public string TouchUrl{ get; set; }
    }
    public class StatusForLoginDto
    {
        public string ErrorMessage { get; set; }
        public bool? Status { get; set; }
    }
    public class TradingAccount
    {
        public int? TradingAccountID { get; set; }
        public string Name { get; set; }
    }


}
