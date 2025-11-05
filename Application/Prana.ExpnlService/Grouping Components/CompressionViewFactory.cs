using Prana.ExpnlService.Grouping_Components.Views_and_SummaryCalculators;
using Prana.Interfaces;
using Prana.LogManager;
using System;

namespace Prana.ExpnlService
{
    public class CompressionViewFactory
    {
        private static CompressionViewFactory _viewfactoryInstance;
        public const string TAXLOT_COMPRESSION = "Taxlot";
        public const string ACCOUNTSYMBOL_COMPRESSION = "Account_Symbol";

        static CompressionViewFactory()
        {
            _viewfactoryInstance = new CompressionViewFactory();
        }

        public static CompressionViewFactory GetInstance()
        {
            return _viewfactoryInstance;
        }

        /// <summary>
        /// Get view corresponding to view Name.
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public IGroupingComponent GetView(string viewName)
        {
            IGroupingComponent currentView = null;

            try
            {
                switch (viewName)
                {
                    case TAXLOT_COMPRESSION:
                        currentView = new Taxlot_View();
                        break;

                    case ACCOUNTSYMBOL_COMPRESSION:
                        currentView = new Account_Symbol_View();
                        break;

                    default:
                        currentView = new Taxlot_View();
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return currentView;
        }
    }
}