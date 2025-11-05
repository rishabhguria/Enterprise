using Prana.Interfaces;
///PortfolioReportManager class inside pm.common
using Prana.PM.DAL;
//using Prana.

namespace Prana.InstanceCreator
{
    public class PMInstanceCreator
    {

        static IPMInteraction _instance = null;

        /// <summary>
        /// Gets the instance of IPMInteraction. One can change or add the permission for 
        /// accessing the pm interaction Cache here.
        /// </summary>
        /// <value>The instance.</value>
        public static IPMInteraction Instance
        {
            get
            {
                return GetPMInteractionInstance();
            }
        }

        public static IPMInteraction GetPMInteractionInstance()
        {
            if (_instance == null)
            {
                _instance = PortfolioReportManager.GetInstance();
            }

            return _instance;
        }
    }
}
