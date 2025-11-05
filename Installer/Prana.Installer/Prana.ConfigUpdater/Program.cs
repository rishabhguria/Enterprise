using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Xml;

namespace Prana.ConfigUpdater
{
    public enum ApplicationName
    {
        PricingService2,
        PricingService2UI,
        TradeService,
        TradeServiceUI,
        ExpnlService,
        ExpnlServiceUI,
        GreenFieldAllocationService,
        GreenFieldAuthService,
        GreenFieldBlotterDataService,
        GreenFieldCalculationService,
        GreenFieldCommonDataService,
        GreenFieldComplianceAlertsService,
        GreenFieldLiveFeedService,
        GreenFieldSecurityValidationService,
        GreenFieldTradingService,
        GreenFieldWatchlistDataService,
        GreenFieldLayoutService,
        Client,
        Admin,
        Esper,
        RuleMediator,
        LoggingTool,
        BasketCompliance,
        MarketDataUtility
    }
    public enum EnvironmentName
    {
        Production,
        DevelopmentReleaseMode,
        DevelopmentDebugMode,
        DevelopmentCodeCheckout
    }

    public static class Program
    {
        #region Variables
        private static EnvironmentName _environmentName;

        private static string _clientDBName = string.Empty;
        private static string _smDBName = string.Empty;
        private static string _dbServerName = string.Empty;
        private static string _internalIpAddress = string.Empty;
        private static string _portSeriesPrefix = string.Empty;
        private static string _tradeServiceQueueNameSuffix = string.Empty;
        private static string _vHostName = string.Empty;

        private static string _pricingService2_OrderRequestPort = string.Empty;
        private static string _tradeService_OrderRequestPort = string.Empty;
        private static string _expnlService_OrderRequestPort = string.Empty;
        private static string _greenFieldAllocationService_OrderRequestPort = string.Empty;
        private static string _greenFieldAuthService_OrderRequestPort = string.Empty;
        private static string _greenFieldBlotterDataService_OrderRequestPort = string.Empty;
        private static string _greenFieldCalculationService_OrderRequestPort = string.Empty;
        private static string _greenFieldCommonDataService_OrderRequestPort = string.Empty;
        private static string _greenFieldComplianceAlertsService_OrderRequestPort = string.Empty;
        private static string _greenFieldLiveFeedService_OrderRequestPort = string.Empty;
        private static string _greenFieldSecurityValidationService_OrderRequestPort = string.Empty;
        private static string _greenFieldTradingService_OrderRequestPort = string.Empty;
        private static string _greenFieldWatchlistDataService_OrderRequestPort = string.Empty;
        private static string _greenFieldLayoutService_OrderRequestPort = string.Empty;

        private static string _esper_SocketLockPort = string.Empty;
        private static string _ruleMediator_SocketLockPort = string.Empty;
        private static string _loggingTool_SocketLockPort = string.Empty;
        private static string _basketCompliance_SocketLockPort = string.Empty;

        private static string _marketDataServicePort = string.Empty;

        private static string _pricingService2Port = string.Empty;
        private static string _portOfHostedServicesInPricingService2 = string.Empty;

        private static string _tradeServicePort = string.Empty;
        private static string _portOfHostedServicesInTradeService = string.Empty;

        private static string _expnlServicePort = string.Empty;
        private static string _portOfHostedServicesInExpnlService = string.Empty;

        private static string _greenFieldAllocationServicePort = string.Empty;
        private static string _greenFieldAuthServicePort = string.Empty;
        private static string _greenFieldBlotterDataServicePort = string.Empty;
        private static string _greenFieldCalculationServicePort = string.Empty;
        private static string _greenFieldCommonDataServicePort = string.Empty;
        private static string _greenFieldComplianceAlertsServicePort = string.Empty;
        private static string _greenFieldLiveFeedServicePort = string.Empty;
        private static string _greenFieldSecurityValidationServicePort = string.Empty;
        private static string _greenFieldTradingServicePort = string.Empty;
        private static string _greenFieldWatchlistDataServicePort = string.Empty;
        private static string _greenFieldLayoutServicePort = string.Empty;
        #endregion

        public static void Main()
        {
            try
            {
                Console.Title = "Configuration Updater";

                #region Input Data
                Console.WriteLine("Select Environment for which updating config files:");
                Console.WriteLine("\tPress 0 - Production Build");
                Console.WriteLine("\tPress 1 - Development Release Mode Build");
                Console.WriteLine("\tPress 2 - Development Debug Mode Build");
                Console.WriteLine("\tPress any other key - Development Code Checkout");
                Console.Write("\t");

                string environmentNameStr = Console.ReadLine().Trim();
                if (string.IsNullOrWhiteSpace(environmentNameStr) || !(environmentNameStr.Equals("0") || environmentNameStr.Equals("1") || environmentNameStr.Equals("2")))
                {
                    environmentNameStr = string.Empty;
                }

                switch (environmentNameStr)
                {
                    case "0":
                        _environmentName = EnvironmentName.Production;
                        break;
                    case "1":
                        _environmentName = EnvironmentName.DevelopmentReleaseMode;
                        break;
                    case "2":
                        _environmentName = EnvironmentName.DevelopmentDebugMode;
                        break;
                    default:
                        _environmentName = EnvironmentName.DevelopmentCodeCheckout;
                        break;
                }

                Console.Write("Client DB Name (leave blank to skip): ");
                _clientDBName = Console.ReadLine().Trim();

                Console.Write("SM DB Name (leave blank to skip): ");
                _smDBName = Console.ReadLine().Trim();

                Console.Write("DB Server Name (leave blank to skip): ");
                _dbServerName = Console.ReadLine().Trim();

                if (_environmentName == EnvironmentName.Production)
                {
                    Console.Write("Internal IP Address (leave blank to skip): ");
                    _internalIpAddress = Console.ReadLine().Trim();
                }
                else
                {
                    foreach (System.Net.NetworkInformation.NetworkInterface ni in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
                    {
                        var addr = ni.GetIPProperties().GatewayAddresses.FirstOrDefault();
                        if (addr != null)
                        {
                            if (ni.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Ethernet)
                            {
                                foreach (System.Net.NetworkInformation.UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                                {
                                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                    {
                                        _internalIpAddress = ip.Address.ToString();
                                    }
                                }
                            }
                        }
                    }

                    if (string.IsNullOrWhiteSpace(_internalIpAddress))
                    {
                        Console.WriteLine("Unable to retrive IP address from this machine.");
                    }
                }

                Console.Write("Port Series Prefix (leave blank for default => 50): ");
                _portSeriesPrefix = Console.ReadLine().Trim();
                if (string.IsNullOrWhiteSpace(_portSeriesPrefix))
                {
                    _portSeriesPrefix = "50";
                }

                Console.Write("Trade Service Queues Suffix (leave blank to skip): ");
                _tradeServiceQueueNameSuffix = Console.ReadLine().Trim();

                Console.Write("VHost Name (leave blank to skip): ");
                _vHostName = Console.ReadLine().Trim();
                #endregion

                #region Port Series
                _pricingService2_OrderRequestPort = _portSeriesPrefix + "00";
                _tradeService_OrderRequestPort = _portSeriesPrefix + "01";
                _expnlService_OrderRequestPort = _portSeriesPrefix + "02";
                _greenFieldAllocationService_OrderRequestPort = _portSeriesPrefix + "03";
                _greenFieldAuthService_OrderRequestPort = _portSeriesPrefix + "04";
                _greenFieldBlotterDataService_OrderRequestPort = _portSeriesPrefix + "05";
                _greenFieldCalculationService_OrderRequestPort = _portSeriesPrefix + "06";
                _greenFieldCommonDataService_OrderRequestPort = _portSeriesPrefix + "07";
                _greenFieldLiveFeedService_OrderRequestPort = _portSeriesPrefix + "08";
                _greenFieldSecurityValidationService_OrderRequestPort = _portSeriesPrefix + "09";
                _greenFieldTradingService_OrderRequestPort = _portSeriesPrefix + "10";
                _greenFieldWatchlistDataService_OrderRequestPort = _portSeriesPrefix + "11";
                _greenFieldComplianceAlertsService_OrderRequestPort = _portSeriesPrefix + "12";
                _greenFieldLayoutService_OrderRequestPort = _portSeriesPrefix + "13";

                _esper_SocketLockPort = _portSeriesPrefix + "41";
                _ruleMediator_SocketLockPort = _portSeriesPrefix + "42";
                _loggingTool_SocketLockPort = _portSeriesPrefix + "43";
                _basketCompliance_SocketLockPort = _portSeriesPrefix + "44";

                _marketDataServicePort = _portSeriesPrefix + "50";

                _pricingService2Port = _portSeriesPrefix + "51";
                _portOfHostedServicesInPricingService2 = _portSeriesPrefix + "52";

                _tradeServicePort = _portSeriesPrefix + "53";
                _portOfHostedServicesInTradeService = _portSeriesPrefix + "54";

                _expnlServicePort = _portSeriesPrefix + "55";
                _portOfHostedServicesInExpnlService = _portSeriesPrefix + "56";

                _greenFieldAllocationServicePort = _portSeriesPrefix + "57";
                _greenFieldAuthServicePort = _portSeriesPrefix + "58";
                _greenFieldBlotterDataServicePort = _portSeriesPrefix + "59";
                _greenFieldCalculationServicePort = _portSeriesPrefix + "60";
                _greenFieldCommonDataServicePort = _portSeriesPrefix + "61";
                _greenFieldLiveFeedServicePort = _portSeriesPrefix + "62";
                _greenFieldSecurityValidationServicePort = _portSeriesPrefix + "63";
                _greenFieldTradingServicePort = _portSeriesPrefix + "64";
                _greenFieldWatchlistDataServicePort = _portSeriesPrefix + "65";
                _greenFieldComplianceAlertsServicePort = _portSeriesPrefix + "66";
                _greenFieldLayoutServicePort = _portSeriesPrefix + "67";
                #endregion

                #region Config Updation
                if (_environmentName == EnvironmentName.Production)
                {
                    UpdateConfiguration(ApplicationName.PricingService2, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Pricing\\Prana.PricingService2Host.exe.config");
                    UpdateConfiguration(ApplicationName.PricingService2UI, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Pricing\\Prana.PricingService2UI.exe.config");
                    UpdateConfiguration(ApplicationName.TradeService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Server\\Prana.TradeServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.TradeServiceUI, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Server\\Prana.TradeServiceUI.exe.config");
                    UpdateConfiguration(ApplicationName.ExpnlService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Expnl\\Prana.ExpnlServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.ExpnlServiceUI, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Expnl\\Prana.ExpnlServiceUI.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldAllocationService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\GreenField Services\\Allocation Service\\Prana.AllocationServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldAuthService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\GreenField Services\\Auth Service\\Prana.AuthServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldBlotterDataService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\GreenField Services\\Blotter Data Service\\Prana.BlotterDataServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldCalculationService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\GreenField Services\\Calculation Service\\Prana.CalculationServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldCommonDataService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\GreenField Services\\Common Data Service\\Prana.CommonDataServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldComplianceAlertsService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\GreenField Services\\Compliance Alerts Service\\Prana.ComplianceAlertsServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldLiveFeedService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\GreenField Services\\LiveFeed Service\\Prana.LiveFeedServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldSecurityValidationService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\GreenField Services\\Security Validation Service\\Prana.SecurityValidationServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldTradingService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\GreenField Services\\Trading Service\\Prana.TradingServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldWatchlistDataService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\GreenField Services\\Watchlist Data Service\\Prana.WatchlistDataServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldLayoutService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\GreenField Services\\Layout Service\\Prana.LayoutServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.Client, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Client Release\\Prana.exe.config");
                    UpdateConfiguration(ApplicationName.Admin, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Admin Release\\Prana.Admin.exe.config");
                    UpdateConfiguration(ApplicationName.Esper, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\EsperCalculator\\target\\resources\\conf\\prana-esperCalculator-Config.xml");
                    UpdateConfiguration(ApplicationName.RuleMediator, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\RuleEngineMediator\\target\\resources\\conf\\prana-ruleMediator-Config.xml");
                    UpdateConfiguration(ApplicationName.LoggingTool, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\ComplianceLoggingTool\\target\\resources\\conf\\prana-loggingTool-Config.xml");
                    UpdateConfiguration(ApplicationName.BasketCompliance, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\BasketComplianceService\\target\\resources\\conf\\prana-basketCompliance-Config.xml");

                    UpdateMarketDataUtility("net.tcp://localhost:", "[0-9]+", "/MarketDataService", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\MarketDataUtility\\LiveFeedUtility.exe.config");

                    UpdateComplianceBatFile("title 'Esper Calculation Engine - ", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\EsperCalculator\\StartEsperCalculator.bat");
                    UpdateComplianceBatFile("title 'Rule Mediator Engine - ", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\RuleEngineMediator\\StartRuleEngineMediator.bat");
                    UpdateComplianceBatFile("title 'Logging Tool - ", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\ComplianceLoggingTool\\StartLoggingTool.bat");
                    UpdateComplianceBatFile("title 'Basket Compliance Service - ", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\BasketComplianceService\\StartBasketComplianceService.bat");
                }
                else if (_environmentName == EnvironmentName.DevelopmentReleaseMode)
                {
                    UpdateConfiguration(ApplicationName.PricingService2, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.PricingAnalysisModels\\Prana.PricingService2Host\\bin\\Release\\Prana.PricingService2Host.exe.config");
                    UpdateConfiguration(ApplicationName.PricingService2UI, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.PricingAnalysisModels\\Prana.PricingService2UI\\bin\\Release\\Prana.PricingService2UI.exe.config");
                    UpdateConfiguration(ApplicationName.TradeService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.Server\\Prana.TradeServiceHost\\bin\\Release\\Prana.TradeServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.TradeServiceUI, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.Server\\Prana.TradeServiceUI\\bin\\Release\\Prana.TradeServiceUI.exe.config");
                    UpdateConfiguration(ApplicationName.ExpnlService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.ExpnlServiceHost\\bin\\Release\\Prana.ExpnlServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.ExpnlServiceUI, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.ExpnlServiceUI\\bin\\Release\\Prana.ExpnlServiceUI.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldAllocationService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.AllocationServiceHost\\bin\\Release\\Prana.AllocationServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldAuthService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.AuthServiceHost\\bin\\Release\\Prana.AuthServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldBlotterDataService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.BlotterDataServiceHost\\bin\\Release\\Prana.BlotterDataServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldCalculationService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.CalculationServiceHost\\bin\\Release\\Prana.CalculationServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldCommonDataService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.CommonDataServiceHost\\bin\\Release\\Prana.CommonDataServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldComplianceAlertsService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.ComplianceAlertsServiceHost\\bin\\Release\\Prana.ComplianceAlertsServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldLiveFeedService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.LiveFeedServiceHost\\bin\\Release\\Prana.LiveFeedServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldSecurityValidationService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.SecurityValidationServiceHost\\bin\\Release\\Prana.SecurityValidationServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldTradingService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.TradingServiceHost\\bin\\Release\\Prana.TradingServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldWatchlistDataService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.WatchlistDataServiceHost\\bin\\Release\\Prana.WatchlistDataServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldLayoutService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.LayoutServiceHost\\bin\\Release\\Prana.LayoutServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.Client, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.Client\\Prana\\bin\\Release\\Prana.exe.config");
                    UpdateConfiguration(ApplicationName.Admin, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\ApplicationAdmin\\PranaSuperAdmin\\Prana.Admin\\Prana.Admin.exe.config");
                    UpdateConfiguration(ApplicationName.Esper, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-esperCalculator\\Export\\target\\resources\\conf\\prana-esperCalculator-Config.xml");
                    UpdateConfiguration(ApplicationName.RuleMediator, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-ruleEngineMediator\\Export\\target\\resources\\conf\\prana-ruleMediator-Config.xml");
                    UpdateConfiguration(ApplicationName.LoggingTool, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-loggingTool\\Export\\target\\resources\\conf\\prana-loggingTool-Config.xml");
                    UpdateConfiguration(ApplicationName.BasketCompliance, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-basketComplianceService\\Export\\target\\resources\\conf\\prana-basketCompliance-Config.xml");

                    UpdateComplianceBatFile("title 'Esper Calculation Engine - ", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-esperCalculator\\Export\\StartEsperCalculator.bat");
                    UpdateComplianceBatFile("title 'Rule Mediator Engine - ", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-ruleEngineMediator\\Export\\StartRuleEngineMediator.bat");
                    UpdateComplianceBatFile("title 'Logging Tool - ", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-loggingTool\\Export\\StartLoggingTool.bat");
                    UpdateComplianceBatFile("title 'Basket Compliance Service - ", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-basketComplianceService\\Export\\StartBasketComplianceService.bat");
                }
                else if (_environmentName == EnvironmentName.DevelopmentDebugMode)
                {
                    UpdateConfiguration(ApplicationName.PricingService2, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.PricingAnalysisModels\\Prana.PricingService2Host\\bin\\Debug\\Prana.PricingService2Host.exe.config");
                    UpdateConfiguration(ApplicationName.PricingService2UI, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.PricingAnalysisModels\\Prana.PricingService2UI\\bin\\Debug\\Prana.PricingService2UI.exe.config");
                    UpdateConfiguration(ApplicationName.TradeService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.Server\\Prana.TradeServiceHost\\bin\\Debug\\Prana.TradeServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.TradeServiceUI, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.Server\\Prana.TradeServiceUI\\bin\\Debug\\Prana.TradeServiceUI.exe.config");
                    UpdateConfiguration(ApplicationName.ExpnlService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.ExpnlServiceHost\\bin\\Debug\\Prana.ExpnlServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.ExpnlServiceUI, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.ExpnlServiceUI\\bin\\Debug\\Prana.ExpnlServiceUI.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldAllocationService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.AllocationServiceHost\\bin\\Debug\\Prana.AllocationServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldAuthService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.AuthServiceHost\\bin\\Debug\\Prana.AuthServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldBlotterDataService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.BlotterDataServiceHost\\bin\\Debug\\Prana.BlotterDataServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldCalculationService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.CalculationServiceHost\\bin\\Debug\\Prana.CalculationServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldCommonDataService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.CommonDataServiceHost\\bin\\Debug\\Prana.CommonDataServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldComplianceAlertsService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.ComplianceAlertsServiceHost\\bin\\Debug\\Prana.ComplianceAlertsServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldLiveFeedService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.LiveFeedServiceHost\\bin\\Debug\\Prana.LiveFeedServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldSecurityValidationService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.SecurityValidationServiceHost\\bin\\Debug\\Prana.SecurityValidationServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldTradingService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.TradingServiceHost\\bin\\Debug\\Prana.TradingServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldWatchlistDataService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.WatchlistDataServiceHost\\bin\\Debug\\Prana.WatchlistDataServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.GreenFieldLayoutService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.LayoutServiceHost\\bin\\Debug\\Prana.LayoutServiceHost.exe.config");
                    UpdateConfiguration(ApplicationName.Client, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.Client\\Prana\\bin\\Debug\\Prana.exe.config");
                    UpdateConfiguration(ApplicationName.Admin, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\ApplicationAdmin\\PranaSuperAdmin\\Prana.Admin\\Prana.Admin.exe.config");
                    UpdateConfiguration(ApplicationName.Esper, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-esperCalculator\\Export\\target\\resources\\conf\\prana-esperCalculator-Config.xml");
                    UpdateConfiguration(ApplicationName.RuleMediator, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-ruleEngineMediator\\Export\\target\\resources\\conf\\prana-ruleMediator-Config.xml");
                    UpdateConfiguration(ApplicationName.LoggingTool, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-loggingTool\\Export\\target\\resources\\conf\\prana-loggingTool-Config.xml");
                    UpdateConfiguration(ApplicationName.BasketCompliance, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-basketComplianceService\\Export\\target\\resources\\conf\\prana-basketCompliance-Config.xml");

                    UpdateComplianceBatFile("title 'Esper Calculation Engine - ", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-esperCalculator\\Export\\StartEsperCalculator.bat");
                    UpdateComplianceBatFile("title 'Rule Mediator Engine - ", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-ruleEngineMediator\\Export\\StartRuleEngineMediator.bat");
                    UpdateComplianceBatFile("title 'Logging Tool - ", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-loggingTool\\Export\\StartLoggingTool.bat");
                    UpdateComplianceBatFile("title 'Basket Compliance Service - ", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-basketComplianceService\\Export\\StartBasketComplianceService.bat");
                }
                else if (_environmentName == EnvironmentName.DevelopmentCodeCheckout)
                {
                    UpdateConfiguration(ApplicationName.PricingService2, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.PricingAnalysisModels\\Prana.PricingService2Host\\App.config");
                    UpdateConfiguration(ApplicationName.PricingService2UI, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.PricingAnalysisModels\\Prana.PricingService2UI\\App.config");
                    UpdateConfiguration(ApplicationName.TradeService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.Server\\Prana.TradeServiceHost\\App.config");
                    UpdateConfiguration(ApplicationName.TradeServiceUI, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.Server\\Prana.TradeServiceUI\\App.config");
                    UpdateConfiguration(ApplicationName.ExpnlService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.ExpnlServiceHost\\App.config");
                    UpdateConfiguration(ApplicationName.ExpnlServiceUI, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.ExpnlServiceUI\\App.config");
                    UpdateConfiguration(ApplicationName.GreenFieldAllocationService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.AllocationServiceHost\\App.config");
                    UpdateConfiguration(ApplicationName.GreenFieldAuthService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.AuthServiceHost\\App.config");
                    UpdateConfiguration(ApplicationName.GreenFieldBlotterDataService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.BlotterDataServiceHost\\App.config");
                    UpdateConfiguration(ApplicationName.GreenFieldCalculationService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.CalculationServiceHost\\App.config");
                    UpdateConfiguration(ApplicationName.GreenFieldCommonDataService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.CommonDataServiceHost\\App.config");
                    UpdateConfiguration(ApplicationName.GreenFieldComplianceAlertsService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.ComplianceAlertsServiceHost\\App.config");
                    UpdateConfiguration(ApplicationName.GreenFieldLiveFeedService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.LiveFeedServiceHost\\App.config");
                    UpdateConfiguration(ApplicationName.GreenFieldSecurityValidationService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.SecurityValidationServiceHost\\App.config");
                    UpdateConfiguration(ApplicationName.GreenFieldTradingService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.TradingServiceHost\\App.config");
                    UpdateConfiguration(ApplicationName.GreenFieldWatchlistDataService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.WatchlistDataServiceHost\\App.config");
                    UpdateConfiguration(ApplicationName.GreenFieldLayoutService, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Prana.GreenFieldServices\\Prana.LayoutServiceHost\\App.config");
                    UpdateConfiguration(ApplicationName.Client, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Application\\Prana.Client\\Prana\\App.config");
                    UpdateConfiguration(ApplicationName.Admin, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\ApplicationAdmin\\PranaSuperAdmin\\Prana.Admin\\App.config");
                    UpdateConfiguration(ApplicationName.Esper, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-esperCalculator\\src\\main\\resources\\conf\\prana-esperCalculator-Config.xml");
                    UpdateConfiguration(ApplicationName.RuleMediator, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-ruleEngineMediator\\src\\main\\resources\\conf\\prana-ruleMediator-Config.xml");
                    UpdateConfiguration(ApplicationName.LoggingTool, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-loggingTool\\src\\main\\resources\\conf\\prana-loggingTool-Config.xml");
                    UpdateConfiguration(ApplicationName.BasketCompliance, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-basketComplianceService\\src\\main\\resources\\conf\\prana-basketCompliance-Config.xml");

                    UpdateComplianceBatFile("title 'Esper Calculation Engine - ", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-esperCalculator\\StartEsperCalculator.bat");
                    UpdateComplianceBatFile("title 'Rule Mediator Engine - ", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-ruleEngineMediator\\StartRuleEngineMediator.bat");
                    UpdateComplianceBatFile("title 'Logging Tool - ", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-loggingTool\\StartLoggingTool.bat");
                    UpdateComplianceBatFile("title 'Basket Compliance Service - ", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\JavaModules\\prana-basketComplianceService\\StartBasketComplianceService.bat");

                    #region Database Publish Profile Updation
                    UpdatePublishProfile(_clientDBName, _smDBName, _dbServerName, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Database\\Prana.NirvanaClient\\Prana.NirvanaClient.publish.xml");
                    UpdatePublishProfile(_smDBName, null, _dbServerName, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Database\\Prana.SecurityMaster\\Prana.SecurityMaster.publish.xml");
                    #endregion
                }
                #endregion
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.StackTrace);
            }
            finally
            {
                Console.Write("Press any key to exit...");
                Console.ReadKey();
            }
        }

        private static void UpdateMarketDataUtility(string replaceText1, string replaceText2, string replaceText3, string filePath)
        {
            try
            {
                using (TransactionScope updateTransaction = new TransactionScope())
                {
                    if (File.Exists(filePath))
                    {
                        string text = File.ReadAllText(filePath);
                        text = Regex.Replace(text, replaceText1 + replaceText2 + replaceText3, replaceText1 + _marketDataServicePort + replaceText3);
                        File.WriteAllText(filePath, text);
                    }
                    updateTransaction.Complete();
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.StackTrace);
            }
        }

        private static void UpdateComplianceBatFile(string replaceText, string filePath)
        {
            try
            {
                using (TransactionScope updateTransaction = new TransactionScope())
                {
                    if (File.Exists(filePath) && !string.IsNullOrWhiteSpace(_vHostName))
                    {
                        string text = File.ReadAllText(filePath);
                        text = Regex.Replace(text, replaceText + "[A-Za-z0-9]+'", replaceText + _vHostName + "'");
                        File.WriteAllText(filePath, text);
                    }
                    updateTransaction.Complete();
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.StackTrace);
            }
        }

        private static void UpdateConfiguration(ApplicationName applicationName, string configurationPath)
        {
            try
            {
                using (TransactionScope updateTransaction = new TransactionScope())
                {
                    if (File.Exists(configurationPath))
                    {
                        System.Configuration.ConfigXmlDocument configXmlDocument = new System.Configuration.ConfigXmlDocument();
                        configXmlDocument.Load(configurationPath);

                        XmlNode destinationNode = configXmlDocument.SelectSingleNode("//configuration/connectionStrings");
                        if (destinationNode != null)
                        {
                            string patternDBName = @"Database=([^;]+);";
                            string patternDBServerName = @"Server=([^;]+);";
                            Regex regexDBName = new Regex(patternDBName);
                            Regex regexDBServerName = new Regex(patternDBServerName);

                            foreach (XmlNode childNode in destinationNode.ChildNodes)
                            {
                                if (childNode.Attributes != null)
                                {
                                    if (childNode.Attributes["name"].Value == "PranaConnectionString")
                                    {
                                        if (!string.IsNullOrWhiteSpace(_clientDBName))
                                        {
                                            childNode.Attributes["connectionString"].Value = regexDBName.Replace(childNode.Attributes["connectionString"].Value, "Database=" + _clientDBName + ";");
                                        }

                                        if (!string.IsNullOrWhiteSpace(_dbServerName))
                                        {
                                            childNode.Attributes["connectionString"].Value = regexDBServerName.Replace(childNode.Attributes["connectionString"].Value, "Server=" + _dbServerName + ";");
                                        }
                                    }

                                    if (childNode.Attributes["name"].Value == "SMConnectionString")
                                    {
                                        if (!string.IsNullOrWhiteSpace(_smDBName))
                                        {
                                            childNode.Attributes["connectionString"].Value = regexDBName.Replace(childNode.Attributes["connectionString"].Value, "Database=" + _smDBName + ";");
                                        }

                                        if (!string.IsNullOrWhiteSpace(_dbServerName))
                                        {
                                            childNode.Attributes["connectionString"].Value = regexDBServerName.Replace(childNode.Attributes["connectionString"].Value, "Server=" + _dbServerName + ";");
                                        }
                                    }
                                }
                            }
                        }

                        destinationNode = configXmlDocument.SelectSingleNode("//configuration/appSettings");
                        if (destinationNode != null)
                        {
                            foreach (XmlNode childNode in destinationNode.ChildNodes)
                            {
                                if (childNode.Attributes != null)
                                {
                                    if (!string.IsNullOrWhiteSpace(_internalIpAddress))
                                    {
                                        if (childNode.Attributes["key"].Value == "PricingServer")
                                        {
                                            childNode.Attributes["value"].Value = _internalIpAddress + ":" + _pricingService2_OrderRequestPort;
                                        }
                                        if (childNode.Attributes["key"].Value == "TradeServer")
                                        {
                                            childNode.Attributes["value"].Value = _internalIpAddress + ":" + _tradeService_OrderRequestPort;
                                        }
                                        if (childNode.Attributes["key"].Value == "ExpnlServer")
                                        {
                                            childNode.Attributes["value"].Value = _internalIpAddress + ":" + _expnlService_OrderRequestPort;
                                        }
                                    }
                                    else
                                    {
                                        if (childNode.Attributes["key"].Value == "PricingServer")
                                        {
                                            childNode.Attributes["value"].Value = childNode.Attributes["value"].Value.Trim().Split(':')[0] + ":" + _pricingService2_OrderRequestPort;
                                        }
                                        if (childNode.Attributes["key"].Value == "TradeServer")
                                        {
                                            childNode.Attributes["value"].Value = childNode.Attributes["value"].Value.Trim().Split(':')[0] + ":" + _tradeService_OrderRequestPort;
                                        }
                                        if (childNode.Attributes["key"].Value == "ExpnlServer")
                                        {
                                            childNode.Attributes["value"].Value = childNode.Attributes["value"].Value.Trim().Split(':')[0] + ":" + _expnlService_OrderRequestPort;
                                        }
                                    }

                                    if (applicationName == ApplicationName.PricingService2 && childNode.Attributes["key"].Value == "OrderRequestPort")
                                    {
                                        childNode.Attributes["value"].Value = _pricingService2_OrderRequestPort;
                                    }
                                    else if (applicationName == ApplicationName.TradeService && childNode.Attributes["key"].Value == "OrderRequestPort")
                                    {
                                        childNode.Attributes["value"].Value = _tradeService_OrderRequestPort;
                                    }
                                    else if (applicationName == ApplicationName.ExpnlService && childNode.Attributes["key"].Value == "OrderRequestPort")
                                    {
                                        childNode.Attributes["value"].Value = _expnlService_OrderRequestPort;
                                    }
                                    else if (applicationName == ApplicationName.GreenFieldAllocationService && childNode.Attributes["key"].Value == "OrderRequestPort")
                                    {
                                        childNode.Attributes["value"].Value = _greenFieldAllocationService_OrderRequestPort;
                                    }
                                    else if (applicationName == ApplicationName.GreenFieldAuthService && childNode.Attributes["key"].Value == "OrderRequestPort")
                                    {
                                        childNode.Attributes["value"].Value = _greenFieldAuthService_OrderRequestPort;
                                    }
                                    else if (applicationName == ApplicationName.GreenFieldBlotterDataService && childNode.Attributes["key"].Value == "OrderRequestPort")
                                    {
                                        childNode.Attributes["value"].Value = _greenFieldBlotterDataService_OrderRequestPort;
                                    }
                                    else if (applicationName == ApplicationName.GreenFieldCalculationService && childNode.Attributes["key"].Value == "OrderRequestPort")
                                    {
                                        childNode.Attributes["value"].Value = _greenFieldCalculationService_OrderRequestPort;
                                    }
                                    else if (applicationName == ApplicationName.GreenFieldCommonDataService && childNode.Attributes["key"].Value == "OrderRequestPort")
                                    {
                                        childNode.Attributes["value"].Value = _greenFieldCommonDataService_OrderRequestPort;
                                    }
                                    else if (applicationName == ApplicationName.GreenFieldComplianceAlertsService && childNode.Attributes["key"].Value == "OrderRequestPort")
                                    {
                                        childNode.Attributes["value"].Value = _greenFieldComplianceAlertsService_OrderRequestPort;
                                    }
                                    else if (applicationName == ApplicationName.GreenFieldLiveFeedService && childNode.Attributes["key"].Value == "OrderRequestPort")
                                    {
                                        childNode.Attributes["value"].Value = _greenFieldLiveFeedService_OrderRequestPort;
                                    }
                                    else if (applicationName == ApplicationName.GreenFieldSecurityValidationService && childNode.Attributes["key"].Value == "OrderRequestPort")
                                    {
                                        childNode.Attributes["value"].Value = _greenFieldSecurityValidationService_OrderRequestPort;
                                    }
                                    else if (applicationName == ApplicationName.GreenFieldTradingService && childNode.Attributes["key"].Value == "OrderRequestPort")
                                    {
                                        childNode.Attributes["value"].Value = _greenFieldTradingService_OrderRequestPort;
                                    }
                                    else if (applicationName == ApplicationName.GreenFieldWatchlistDataService && childNode.Attributes["key"].Value == "OrderRequestPort")
                                    {
                                        childNode.Attributes["value"].Value = _greenFieldWatchlistDataService_OrderRequestPort;
                                    }
                                    else if (applicationName == ApplicationName.GreenFieldLayoutService && childNode.Attributes["key"].Value == "OrderRequestPort")
                                    {
                                        childNode.Attributes["value"].Value = _greenFieldLayoutService_OrderRequestPort;
                                    }

                                    if (applicationName == ApplicationName.TradeService && !string.IsNullOrWhiteSpace(_tradeServiceQueueNameSuffix) &&
                                        (childNode.Attributes["key"].Value == "DBQUEUE_PATH"
                                        || childNode.Attributes["key"].Value == "ERRORQUEUE_PATH"
                                        || childNode.Attributes["key"].Value == "ConnectionUnAvailable_PATH"
                                        || childNode.Attributes["key"].Value == "CP_SENT_MSGS_PATH"
                                        || childNode.Attributes["key"].Value == "CP_RECEIVED_MSGS_PATH"
                                        || childNode.Attributes["key"].Value == "CLIENT_RECEIVED_PATH"
                                        || childNode.Attributes["key"].Value == "DRP_CPY_ERROR_MSGS_PATH"
                                        || childNode.Attributes["key"].Value == "OLDTRADES_QUEUE_PATH"
                                        || childNode.Attributes["key"].Value == "CASHACTIVITY_QUEUE_PATH"))
                                    {
                                        childNode.Attributes["value"].Value += "_" + _tradeServiceQueueNameSuffix;
                                    }
                                }
                            }
                        }

                        destinationNode = configXmlDocument.SelectSingleNode("//appConfig/appSettings");
                        if (destinationNode != null)
                        {
                            foreach (XmlNode childNode in destinationNode.ChildNodes)
                            {
                                if (childNode.Attributes != null)
                                {
                                    if (childNode.Attributes["key"].Value == "Vhost" && !string.IsNullOrWhiteSpace(_vHostName))
                                    {
                                        childNode.Attributes["value"].Value = _vHostName;
                                    }

                                    if (applicationName == ApplicationName.Esper && childNode.Attributes["key"].Value == "SocketLockPort")
                                    {
                                        childNode.Attributes["value"].Value = _esper_SocketLockPort;
                                    }
                                    else if (applicationName == ApplicationName.RuleMediator && childNode.Attributes["key"].Value == "SocketLockPort")
                                    {
                                        childNode.Attributes["value"].Value = _ruleMediator_SocketLockPort;
                                    }
                                    else if (applicationName == ApplicationName.LoggingTool && childNode.Attributes["key"].Value == "SocketLockPort")
                                    {
                                        childNode.Attributes["value"].Value = _loggingTool_SocketLockPort;
                                    }
                                    else if (applicationName == ApplicationName.BasketCompliance && childNode.Attributes["key"].Value == "SocketLockPort")
                                    {
                                        childNode.Attributes["value"].Value = _basketCompliance_SocketLockPort;
                                    }
                                }
                            }
                        }

                        // Updating port in hosted services
                        XmlNode serviceModelServices = configXmlDocument.SelectSingleNode("//configuration/system.serviceModel/services");
                        if (serviceModelServices != null)
                        {
                            foreach (XmlNode serviceNode in serviceModelServices.ChildNodes)
                                UpdateServiceModelEndpoint(serviceNode.ChildNodes[0]);
                        }

                        // Updating port in consumed services
                        XmlNode serviceModelClients = configXmlDocument.SelectSingleNode("//configuration/system.serviceModel/client");
                        if (serviceModelClients != null)
                        {
                            foreach (XmlNode childNode in serviceModelClients.ChildNodes)
                                UpdateServiceModelEndpoint(childNode);
                        }

                        destinationNode = configXmlDocument.SelectSingleNode("//configuration/Compliance");
                        if (destinationNode != null && !string.IsNullOrWhiteSpace(_vHostName))
                        {
                            foreach (XmlNode childNode in destinationNode.ChildNodes)
                            {
                                if (childNode.Attributes != null)
                                {
                                    if (childNode.Attributes["key"].Value == "VHost")
                                    {
                                        childNode.Attributes["value"].Value = _vHostName;
                                    }
                                }
                            }
                        }

                        configXmlDocument.Save(configurationPath);
                    }
                    updateTransaction.Complete();
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.StackTrace);
            }
        }

        private static void UpdateServiceModelEndpoint(XmlNode endpointNode)
        {
            if (endpointNode.Attributes["name"].Value.StartsWith("Pricing"))
            {
                if (endpointNode.Attributes["name"].Value.Equals("PricingService2EndpointAddress"))
                    ReplacePortInEndpoint(endpointNode, _pricingService2Port);
                else
                    ReplacePortInEndpoint(endpointNode, _portOfHostedServicesInPricingService2);
            }
            else if (endpointNode.Attributes["name"].Value.StartsWith("Trade"))
            {
                if (endpointNode.Attributes["name"].Value.Equals("TradeServiceEndpointAddress"))
                    ReplacePortInEndpoint(endpointNode, _tradeServicePort);
                else
                    ReplacePortInEndpoint(endpointNode, _portOfHostedServicesInTradeService);
            }
            else if (endpointNode.Attributes["name"].Value.StartsWith("Expnl"))
            {
                if (endpointNode.Attributes["name"].Value.Equals("ExpnlServiceEndpointAddress"))
                    ReplacePortInEndpoint(endpointNode, _expnlServicePort);
                else
                    ReplacePortInEndpoint(endpointNode, _portOfHostedServicesInExpnlService);
            }
            else if (endpointNode.Attributes["name"].Value.Equals("MarketDataServiceEndpointAddress"))
            {
                ReplacePortInEndpoint(endpointNode, _marketDataServicePort);
            }
            else if (endpointNode.Attributes["name"].Value.Equals("AllocationServiceEndpointAddress"))
            {
                ReplacePortInEndpoint(endpointNode, _greenFieldAllocationServicePort);
            }
            else if (endpointNode.Attributes["name"].Value.Equals("AuthServiceEndpointAddress"))
            {
                ReplacePortInEndpoint(endpointNode, _greenFieldAuthServicePort);
            }
            else if (endpointNode.Attributes["name"].Value.Equals("BlotterDataServiceEndpointAddress"))
            {
                ReplacePortInEndpoint(endpointNode, _greenFieldBlotterDataServicePort);
            }
            else if (endpointNode.Attributes["name"].Value.Equals("CalculationServiceEndpointAddress"))
            {
                ReplacePortInEndpoint(endpointNode, _greenFieldCalculationServicePort);
            }
            else if (endpointNode.Attributes["name"].Value.Equals("CommonDataServiceEndpointAddress"))
            {
                ReplacePortInEndpoint(endpointNode, _greenFieldCommonDataServicePort);
            }
            else if (endpointNode.Attributes["name"].Value.Equals("ComplianceAlertsServiceEndpointAddress"))
            {
                ReplacePortInEndpoint(endpointNode, _greenFieldComplianceAlertsServicePort);
            }
            else if (endpointNode.Attributes["name"].Value.Equals("LiveFeedServiceEndpointAddress"))
            {
                ReplacePortInEndpoint(endpointNode, _greenFieldLiveFeedServicePort);
            }
            else if (endpointNode.Attributes["name"].Value.Equals("SecurityValidationServiceEndpointAddress"))
            {
                ReplacePortInEndpoint(endpointNode, _greenFieldSecurityValidationServicePort);
            }
            else if (endpointNode.Attributes["name"].Value.Equals("TradingServiceEndpointAddress"))
            {
                ReplacePortInEndpoint(endpointNode, _greenFieldTradingServicePort);
            }
            else if (endpointNode.Attributes["name"].Value.Equals("WatchlistDataServiceEndpointAddress"))
            {
                ReplacePortInEndpoint(endpointNode, _greenFieldWatchlistDataServicePort);
            }
            else if (endpointNode.Attributes["name"].Value.Equals("LayoutServiceEndpointAddress"))
            {
                ReplacePortInEndpoint(endpointNode, _greenFieldLayoutServicePort);
            }
        }

        private static void ReplacePortInEndpoint(XmlNode endpointNode, string port)
        {
            string[] splitAddress = endpointNode.Attributes["address"].Value.Split(':', '/');
            splitAddress[4] = port;

            endpointNode.Attributes["address"].Value = "net.tcp://localhost:";

            for (int i = 4; i < splitAddress.Length; i++)
            {
                endpointNode.Attributes["address"].Value += splitAddress[i];

                if (i != splitAddress.Length - 1)
                    endpointNode.Attributes["address"].Value += '/';
            }
        }

        private static void UpdatePublishProfile(string targetDBName, string sqlCmdVariableValue, string dbServerName, string publishProfileXmlPath)
        {
            try
            {
                if (File.Exists(publishProfileXmlPath))
                {
                    XmlDocument publishProfileXmlDoc = new XmlDocument();
                    publishProfileXmlDoc.Load(publishProfileXmlPath);

                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(publishProfileXmlDoc.NameTable);
                    nsmgr.AddNamespace("msb", "http://schemas.microsoft.com/developer/msbuild/2003");

                    // Use the namespace in the XPath expression to select the node
                    XmlNode targetDatabaseNode = publishProfileXmlDoc.SelectSingleNode("//msb:TargetDatabaseName", nsmgr);

                    if (targetDatabaseNode != null && !string.IsNullOrWhiteSpace(targetDBName))
                    {
                        targetDatabaseNode.InnerText = targetDBName;
                    }

                    XmlNode targetConnectionStringNode = publishProfileXmlDoc.SelectSingleNode("//msb:TargetConnectionString", nsmgr);

                    if (targetConnectionStringNode != null && !string.IsNullOrWhiteSpace(dbServerName))
                    {
                        string patternDBServerName = @"Data Source=([^;]+);";
                        Regex regexDBServerName = new Regex(patternDBServerName);

                        targetConnectionStringNode.InnerText = regexDBServerName.Replace(targetConnectionStringNode.InnerText, "Data Source=" + dbServerName + ";");
                    }

                    XmlNode valueNode = publishProfileXmlDoc.SelectSingleNode("//msb:Value", nsmgr);
                    if (valueNode != null && !string.IsNullOrWhiteSpace(sqlCmdVariableValue))
                    {
                        valueNode.InnerText = sqlCmdVariableValue;
                    }

                    publishProfileXmlDoc.Save(publishProfileXmlPath);
                }
                else
                {
                    Console.WriteLine("Unable to find database publish profile: " + publishProfileXmlPath);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.StackTrace);
            }
        }
    }
}
