using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Configuration;

namespace Prana.WCFConnectionMgr
{
    public static class PranaServiceHost
    {
        // List of services hosted by application
        static List<ServiceHost> _hostedServicesList = new List<ServiceHost>();


        static ServiceElementCollection _hostedServices = (ServiceElementCollection)((ServicesSection)ConfigurationManager.GetSection("system.serviceModel/services")).Services;
        public static Dictionary<string, ServiceEndpointElement> GetHostedServicesEndpoints()
        {
            Dictionary<string, ServiceEndpointElement> endpoints = new Dictionary<string, ServiceEndpointElement>();

            foreach (ServiceElement serviceElement in _hostedServices)
                foreach (ServiceEndpointElement endpoint in serviceElement.Endpoints)
                    endpoints.Add(endpoint.Name, endpoint);

            return endpoints;
        }

        public static void HostPranaService(Object classObject)
        {
            try
            {
                ServiceHost serviceHost = new ServiceHost(classObject);
                serviceHost.Open();

                _hostedServicesList.Add(serviceHost);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static void HostPranaService(Type classType)
        {
            try
            {
                ServiceHost serviceHost = new ServiceHost(classType);
                serviceHost.Open();

                _hostedServicesList.Add(serviceHost);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static bool IsServiceHosted(string endpointName = "")
        {
            try
            { 
                if (endpointName == string.Empty)
                {
                    string hostedServiceName = Assembly.GetEntryAssembly().ManifestModule.Name.Replace("Prana.", string.Empty).Replace("Host.exe", string.Empty);

                    int orderPort = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("OrderRequestPort"));
                    int hostedPort = GetHostedServicesEndpoints()[hostedServiceName + "EndpointAddress"].Address.Port;

                    var portList = new List<int> { orderPort, hostedPort };

                    if (ProcessPorts.IsPortNumberInUse(portList))
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Please free up the port and restart {0}.", hostedServiceName));
                        Console.WriteLine(string.Format("Please free up the port and restart {0}.", hostedServiceName));
                        return true;
                    }
                }
                else
                {
                    int endpointPort = GetHostedServicesEndpoints()[endpointName].Address.Port;

                    var portList = new List<int> { endpointPort };
                    if (_hostedServicesList.FirstOrDefault(x => x.ChannelDispatchers.FirstOrDefault().Listener.Uri.Port == endpointPort) == null && ProcessPorts.IsPortNumberInUse(portList))
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Please free up the port and restart {0}.", endpointName.Replace("EndpointAddress", string.Empty)));
                        Console.WriteLine(string.Format("Please free up the port and restart {0}.", endpointName.Replace("EndpointAddress", string.Empty)));

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return false;
        }

        public static void CleanUp()
        {
            try
            {
                foreach (ServiceHost host in _hostedServicesList)
                {
                    host.Abort();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
