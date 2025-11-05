namespace Prana.ServiceGateway.Models
{
    public class UserDto
    {
        public bool IsSupport { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsAdminOrSupport { get => IsSupport || IsAdmin; }
        public int CompanyUserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
    }
}
