using Castle.Windsor;

namespace Prana.CoreService.Interfaces
{
    public interface IPranaServiceCommon
    {
        //Initialize service
        System.Threading.Tasks.Task<bool> InitialiseService(IWindsorContainer container);

        //Perform last minute clean up tasks before server component gets closed. Keep it tidy and light.
        void CleanUp();
    }
}