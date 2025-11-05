namespace Prana.Interfaces
{
    public interface IRebalancer
    {
        object Reference();

        void SetUp();

        ISecurityMasterServices SecurityMaster { get; set; }
    }
}
