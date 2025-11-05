using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.AccessBridgeApp
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = true)]
    public class AccessBridgeService : IAccessBridgeService
    {
        public void SendMessage(string commandType, string message)
        {
            try
            {
                switch (commandType)
                {
                    case CameronConstants.buttonCommand:
                        SimulatorAccessBridge.instance.ClickButton(message);
                        break;
                    case CameronConstants.menuButtonCommand:
                        SimulatorAccessBridge.instance.ClickMenuButton(message);
                        break;
                    case CameronConstants.bottomGridCommand:
                        SimulatorAccessBridge.instance.ClickBottomGridField(message);
                        break;
                    case CameronConstants.gridCommand:
                        SimulatorAccessBridge.instance.ClickGridRow(message);
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }
    }
}
