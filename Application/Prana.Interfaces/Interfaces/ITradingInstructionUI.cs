namespace Prana.Interfaces
{
    public interface ITradingInstructionUI
    {
        System.Windows.Forms.Form Reference();
        Prana.BusinessObjects.CompanyUser LoginUser
        {
            get;
            set;
        }
    }
}
