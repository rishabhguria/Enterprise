namespace Prana.Interfaces
{
    /// <summary>
    /// Its a interface for NewsStory module.
    /// </summary>
    public interface IBlotterReports
    {
        System.Windows.Forms.Form Reference();
        Prana.BusinessObjects.CompanyUser LoginUser
        {
            get;
            set;
        }
    }

}
