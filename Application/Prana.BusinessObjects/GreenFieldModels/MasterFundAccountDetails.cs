using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.BusinessObjects.GreenFieldModels
{
    public class MasterFundAccountDetails
    {
        public List<AccountDto> AccountList { get; set; } = new List<AccountDto> { };
        public string MasterFundOrGroupName { get; set; }
        public int? MasterFundOrGroupId { get; set; }
        public bool IsCustomGroup { get; set; }
        public bool HasAllAccountsHavePermission { get; set; }
    }

    /// <summary>
    ///This is the account to pass to other services, we dont use Account business object as it has extra fiedl,which we dont want in UI
    /// </summary>
    public class AccountDto
    {
        public AccountDto(string name, int accountId)
        {
            Name = name;
            AccountId = accountId;
        }

        public string Name { get; set; }
        public int AccountId { get; set; }
    }
}
