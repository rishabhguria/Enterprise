using System;
using System.Collections.Generic;
using System.Text;
using Prana.WCFConnectionMgr;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.Interfaces;

namespace ReportingClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ReportingHealper.CreateReportsForEachClient();
                ReportingHealper.test();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

      
    }
}
