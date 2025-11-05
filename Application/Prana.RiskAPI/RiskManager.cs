using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Prana.RiskServer
{
    //Bharat Kumar Jangir (18 July 2014)
    //Multiple Users Calculating Risk Issue
    //http://jira.nirvanasolutions.com:8080/browse/PRANA-4451
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class RiskManager : IRiskServices
    {
        static List<string> _inValidSymbols = new List<string>();
        static List<string> _validSymbols = new List<string>();
        object _lockerObject = new object();
        static RiskAPI_Enterprise.RiskAPI riskAPI = new RiskAPI_Enterprise.RiskAPI();
        static bool _isConnected = false;
        static bool _riskAPISampleRateSliding = false;

        public RiskManager()
        {
            try
            {
                RiskPreferenceManager.RefreshInterestRate();
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

        #region Private Methods
        private void TryConnect()
        {
            try
            {
                string RiskAPIKey = System.Configuration.ConfigurationManager.AppSettings["RiskAPIKey"].ToString();
                string RiskAPIUserID = System.Configuration.ConfigurationManager.AppSettings["RiskAPIUserID"].ToString();
                string RiskAPIPwd = System.Configuration.ConfigurationManager.AppSettings["RiskAPIPwd"].ToString();
                double RiskAPITimeout = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["RiskAPITimeout"].ToString());
                string RiskAPIProxyAddress = System.Configuration.ConfigurationManager.AppSettings["RiskAPIProxyAddress"].ToString();
                string RiskAPIProxyUsername = System.Configuration.ConfigurationManager.AppSettings["RiskAPIProxyUsername"].ToString();
                string RiskAPIProxyPassword = System.Configuration.ConfigurationManager.AppSettings["RiskAPIProxyPassword"].ToString();
                bool RiskAPIExceptionsEnabled = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["RiskAPIExceptionsEnabled"].ToString());
                _riskAPISampleRateSliding = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["RiskAPISampleRateSliding"].ToString());
                _isConnected = riskAPI.RiskAPI_Login(RiskAPIKey, RiskAPIUserID, RiskAPIPwd, RiskAPITimeout, RiskAPIProxyAddress, RiskAPIProxyUsername, RiskAPIProxyPassword);

                //Enable/Disable Portfolio Science Exceptions
                riskAPI.setThrowExceptions(RiskAPIExceptionsEnabled);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void IsConnected()
        {
            if (!_isConnected)
            {
                TryConnect();

                if (!_isConnected)
                {
                    throw new Exception("Risk Server not connected to Portfolio Science.");
                }
            }
        }

        private void SetCommonPreferences()
        {
            try
            {
                riskAPI.setFX(RiskPreferenceManager.RiskPrefernece.RiskCalculationCurrency.Trim().ToUpper());
                riskAPI.setSampleRate(RiskPreferenceManager.RiskPrefernece.DaysBwDataPoints, _riskAPISampleRateSliding);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private PranaRequestCarrier CheckSymbolValidity(PranaRequestCarrier riskObjcollection)
        {
            lock (_lockerObject)
            {
                PranaRequestCarrier validRiskObjCollection = riskObjcollection.Clone();
                try
                {
                    List<string> lstRequestSymbols = riskObjcollection.GetIndividualPranaRequestCarrierContainer().PSSymbols;

                    // remove Valid Symbols
                    foreach (string validSymbol in _validSymbols)
                    {
                        if (lstRequestSymbols.Contains(validSymbol))
                        {
                            lstRequestSymbols.Remove(validSymbol);
                        }
                    }

                    // remove InValid Symbol
                    foreach (string inValidSymbol in _inValidSymbols)
                    {
                        if (lstRequestSymbols.Contains(inValidSymbol))
                        {
                            lstRequestSymbols.Remove(inValidSymbol);
                        }
                    }

                    if (lstRequestSymbols.Count != 0)
                    {
                        DateTime TimeIn = DateTime.Now;
                        string[] resultSymbols = (string[])riskAPI.RiskAPI_CheckSymbols(GeneralUtilities.GetStringFromList(lstRequestSymbols, ','));
                        DateTime TimeOut = DateTime.Now;
                        foreach (string inValidSymbol in resultSymbols)
                        {
                            _inValidSymbols.Add(inValidSymbol);
                            lstRequestSymbols.Remove(inValidSymbol);
                        }

                        #region LoggingRiskData
                        if (Prana.BusinessObjects.UserSettingConstants.IsRiskLoggingEnabled)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append(Environment.NewLine);
                            sb.Append("Method Name: RiskAPI_CheckSymbols ");
                            sb.Append(Environment.NewLine);
                            sb.Append("Parameters:- ");
                            sb.Append(Environment.NewLine);
                            sb.Append("Invalid Symbols Count: " + _inValidSymbols.Count);
                            sb.Append(Environment.NewLine);
                            sb.Append("Invalid Symbols:  " + GeneralUtilities.GetStringFromList(_inValidSymbols, ','));
                            sb.Append(Environment.NewLine);
                            sb.Append("TimeIn: " + TimeIn);
                            sb.Append(Environment.NewLine);
                            sb.Append("TimeOut: " + TimeOut);
                            Logger.LoggerWrite(sb, LoggingConstants.RISK_LOGGING, 1, 1, TraceEventType.Information);
                        }
                        #endregion
                    }

                    foreach (string validSymbol in lstRequestSymbols)
                    {
                        _validSymbols.Add(validSymbol);
                    }

                    foreach (KeyValuePair<string, PranaRiskResult> riskResult in riskObjcollection.IndividualSymbolList)
                    {
                        if (_validSymbols.Contains(riskResult.Value.PSSymbol))
                        {
                            validRiskObjCollection.IndividualSymbolList.Add(riskResult.Key, riskResult.Value);
                        }
                    }

                    foreach (KeyValuePair<string, PranaRiskResult> riskResult in riskObjcollection.GroupSymbolList)
                    {
                        if (_validSymbols.Contains(riskResult.Value.PSSymbol))
                        {
                            validRiskObjCollection.GroupSymbolList.Add(riskResult.Key, riskResult.Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
                return validRiskObjCollection;
            }
        }

        private void LoggingPSRiskData(string nirvanaMethodName, string psMethodName, DateTime inTime, DateTime outTime, PortfolioSciencesInputParameters inputParams, object psResponse, Exception exception)
        {
            try
            {
                TimeSpan span = outTime.Subtract(inTime);
                if (exception != null || Prana.BusinessObjects.UserSettingConstants.IsRiskLoggingEnabled || span.TotalSeconds > UserSettingConstants.RISKAPIEachCalDurationThreasholdToLog)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Environment.NewLine);
                    sb.Append("Nirvana Method Name: " + nirvanaMethodName);
                    sb.Append(Environment.NewLine);
                    sb.Append("PS Method Name: " + psMethodName);
                    sb.Append(Environment.NewLine);
                    sb.Append("Parameters:- ");

                    int symbolCount = 0;
                    foreach (PropertyInfo propertyInfo in inputParams.GetType().GetProperties())
                    {
                        sb.Append(Environment.NewLine);
                        sb.Append(propertyInfo.Name + ": " + propertyInfo.GetValue(inputParams).ToString());

                        if (propertyInfo.Name == "SymbolCount")
                            symbolCount = Convert.ToInt32(propertyInfo.GetValue(inputParams).ToString());
                    }
                    sb.Append(Environment.NewLine);

                    if (psResponse == null)
                    {
                        sb.Append("Response is NULL");
                    }
                    else if (psResponse is double[,])
                    {
                        double[,] response = psResponse as double[,];
                        sb.Append("Response Length: " + response.Length);
                        sb.Append(Environment.NewLine);
                        sb.Append("Response: " + GeneralUtilities.GetStringFromList((((response.Cast<double>().ToList()).Select(s => s.ToString()).ToList())), ','));
                    }
                    else if (psResponse is double[])
                    {
                        double[] response = psResponse as double[];
                        sb.Append("Response Length: " + response.Length);
                        sb.Append(Environment.NewLine);
                        sb.Append("Response: " + GeneralUtilities.GetStringFromList((((response.Cast<double>().ToList()).Select(s => s.ToString()).ToList())), ','));
                    }
                    else
                    {
                        sb.Append("Response Length: 1");
                        sb.Append(Environment.NewLine);
                        sb.Append("Response: " + psResponse.ToString());
                    }
                    sb.Append(Environment.NewLine);
                    sb.Append("Time In: " + inTime);
                    sb.Append(Environment.NewLine);
                    sb.Append("Time Out: " + outTime);
                    if (exception != null)
                    {
                        sb.Append(Environment.NewLine);
                        sb.Append("RiskAPI Exception: " + exception.Message.ToString());
                    }

                    if (span.TotalSeconds > UserSettingConstants.RISKAPIEachCalDurationThreasholdToLog)
                    {
                        sb.Append(Environment.NewLine);
                        sb.Append("RiskAPI took : " + span.TotalSeconds + " Seconds.");
                        InformationReporter.GetInstance.Write("RiskAPI took " + span.TotalSeconds.ToString() + " Seconds for Symbol Count: " + symbolCount + ", PS Method Name: " + psMethodName + ", Time In: " + inTime + ", Time Out: " + outTime);
                    }

                    Logger.LoggerWrite(sb, LoggingConstants.RISK_LOGGING, 1, 1, TraceEventType.Information);
                }
            }
            catch (Exception except)
            {
                Logger.HandleException(except, LoggingConstants.POLICY_LOGANDSHOW);

            }
        }


        private double CalculateRiskForGroup(PortfolioSciencesInputParameters inputParams)
        {
            double portfolioRisk = 0;
            try
            {
                DateTime inTime = DateTime.Now;
                Exception RiskAPI_Exception = null;
                try
                {
                    portfolioRisk = riskAPI.RiskAPI_PortfolioRisk(inputParams.PSSymbol, inputParams.Quantity, inputParams.Method, inputParams.ConfidenceLevel, inputParams.VolType, inputParams.Lambda, inputParams.StartDate, inputParams.EndDate, false);
                }
                catch (Exception except)
                {
                    RiskAPI_Exception = except;
                    Logger.HandleException(except, LoggingConstants.POLICY_LOGANDSHOW);

                }
                DateTime outTime = DateTime.Now;

                LoggingPSRiskData(MethodBase.GetCurrentMethod().Name, "RiskAPI_PortfolioRisk", inTime, outTime, inputParams, portfolioRisk, RiskAPI_Exception);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return Math.Round(portfolioRisk);
        }

        private double CalculateCorrelationForGroup(PortfolioSciencesInputParameters inputParams)
        {
            double correlation = 0;
            try
            {
                DateTime inTime = DateTime.Now;
                Exception RiskAPI_Exception = null;
                try
                {
                    RiskAPI_Exception = null;
                    correlation = riskAPI.RiskAPI_Correlation(inputParams.BenchMark, "1", inputParams.PSSymbol, inputParams.Quantity, inputParams.StartDate, inputParams.EndDate);
                }
                catch (Exception except)
                {
                    RiskAPI_Exception = except;
                    Logger.HandleException(except, LoggingConstants.POLICY_LOGANDSHOW);

                }
                DateTime outTime = DateTime.Now;

                LoggingPSRiskData(MethodBase.GetCurrentMethod().Name, "RiskAPI_Correlation", inTime, outTime, inputParams, correlation, RiskAPI_Exception);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return correlation;
        }

        private double CalculateStandardDeviationForGroup(PortfolioSciencesInputParameters inputParams)
        {
            double[] standardDevObj = null;
            try
            {
                DateTime inTime = DateTime.Now;
                Exception RiskAPI_Exception = null;
                try
                {
                    RiskAPI_Exception = null;
                    standardDevObj = riskAPI.RiskAPI_StandardDev(inputParams.PSSymbol, inputParams.Quantity, true, inputParams.StartDate, inputParams.EndDate);
                }
                catch (Exception except)
                {
                    RiskAPI_Exception = except;
                    Logger.HandleException(except, LoggingConstants.POLICY_LOGANDSHOW);

                }
                DateTime outTime = DateTime.Now;

                LoggingPSRiskData(MethodBase.GetCurrentMethod().Name, "RiskAPI_StandardDev", inTime, outTime, inputParams, (standardDevObj != null) ? Math.Round(standardDevObj[0]) : 0.0, RiskAPI_Exception);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return (standardDevObj != null) ? Math.Round(standardDevObj[0]) : 0.0;
        }

        private double CalculateBetaForGroup(PortfolioSciencesInputParameters inputParams)
        {
            double beta = 0;
            try
            {
                DateTime inTime = DateTime.Now;
                Exception RiskAPI_Exception = null;
                try
                {
                    RiskAPI_Exception = null;
                    beta = riskAPI.RiskAPI_Beta(inputParams.BenchMark, "1", inputParams.PSSymbol, inputParams.Quantity, inputParams.StartDate, inputParams.EndDate);
                }
                catch (Exception except)
                {
                    RiskAPI_Exception = except;
                    Logger.HandleException(except, LoggingConstants.POLICY_LOGANDSHOW);

                }
                DateTime outTime = DateTime.Now;

                LoggingPSRiskData(MethodBase.GetCurrentMethod().Name, "RiskAPI_Beta", inTime, outTime, inputParams, beta, RiskAPI_Exception);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return beta;
        }

        private double CalculateBenchmarkValueForGroup(PortfolioSciencesInputParameters inputParams)
        {
            double[] benchMarkValueObj = null;
            try
            {
                DateTime inTime = DateTime.Now;
                Exception RiskAPI_Exception = null;
                try
                {
                    RiskAPI_Exception = null;
                    benchMarkValueObj = riskAPI.RiskAPI_Value(inputParams.BenchMark, "1", true);
                }
                catch (Exception except)
                {
                    RiskAPI_Exception = except;
                    Logger.HandleException(except, LoggingConstants.POLICY_LOGANDSHOW);

                }
                DateTime outTime = DateTime.Now;

                LoggingPSRiskData(MethodBase.GetCurrentMethod().Name, "RiskAPI_Value", inTime, outTime, inputParams, (benchMarkValueObj != null) ? Convert.ToDouble(benchMarkValueObj[0]) : 0.0, RiskAPI_Exception);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return (benchMarkValueObj != null) ? Convert.ToDouble(benchMarkValueObj[0]) : 0.0;
        }

        private double CalculateStressImpactForGroup(PortfolioSciencesInputParameters inputParams)
        {
            double[,] pnlImpact = null;
            try
            {
                DateTime inTime = DateTime.Now;
                Exception RiskAPI_Exception = null;
                try
                {
                    RiskAPI_Exception = null;
                    pnlImpact = riskAPI.RiskAPI_StressImpact(inputParams.PSSymbol, inputParams.Quantity, 5, 0, 0, inputParams.IndexStress, "", inputParams.UnderlyingPrice, inputParams.PxSelectedFeed, inputParams.BenchMark, inputParams.BenchMarkQuantity, true, inputParams.StartDate, inputParams.EndDate);
                }
                catch (Exception except)
                {
                    RiskAPI_Exception = except;
                    Logger.HandleException(except, LoggingConstants.POLICY_LOGANDSHOW);

                }
                DateTime outTime = DateTime.Now;

                LoggingPSRiskData(MethodBase.GetCurrentMethod().Name, "RiskAPI_StressImpact", inTime, outTime, inputParams, (pnlImpact != null) ? (pnlImpact[0, 0]) : 0.0, RiskAPI_Exception);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return (pnlImpact != null) ? (pnlImpact[0, 0]) : 0.0;
        }

        private double[] RiskAPIIndividualRisk(PortfolioSciencesInputParameters inputParams)
        {
            double[] individualRisk = null;
            try
            {
                DateTime inTime = DateTime.Now;
                Exception RiskAPI_Exception = null;
                try
                {
                    RiskAPI_Exception = null;
                    individualRisk = riskAPI.RiskAPI_IndividualRisk(inputParams.PSSymbol, inputParams.Quantity, inputParams.Method, inputParams.ConfidenceLevel, inputParams.VolType, inputParams.Lambda, inputParams.StartDate, inputParams.EndDate, false);
                }
                catch (Exception except)
                {
                    RiskAPI_Exception = except;
                    Logger.HandleException(except, LoggingConstants.POLICY_LOGANDSHOW);

                }
                DateTime outTime = DateTime.Now;

                LoggingPSRiskData(MethodBase.GetCurrentMethod().Name, "RiskAPI_IndividualRisk", inTime, outTime, inputParams, individualRisk, RiskAPI_Exception);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return individualRisk;
        }

        private double[] RiskAPIComponentRisk(PortfolioSciencesInputParameters inputParams)
        {
            double[] componentRisk = null;
            try
            {
                DateTime inTime = DateTime.Now;
                Exception RiskAPI_Exception = null;
                try
                {
                    RiskAPI_Exception = null;
                    componentRisk = riskAPI.RiskAPI_ComponentRisk(inputParams.PSSymbol, inputParams.Quantity, inputParams.Method, inputParams.ConfidenceLevel, inputParams.VolType, inputParams.Lambda, inputParams.StartDate, inputParams.EndDate);
                }
                catch (Exception except)
                {
                    RiskAPI_Exception = except;
                    Logger.HandleException(except, LoggingConstants.POLICY_LOGANDSHOW);

                }
                DateTime outTime = DateTime.Now;

                LoggingPSRiskData(MethodBase.GetCurrentMethod().Name, "RiskAPI_ComponentRisk", inTime, outTime, inputParams, componentRisk, RiskAPI_Exception);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return componentRisk;
        }

        private double[] RiskAPIMarginalRisk(PortfolioSciencesInputParameters inputParams)
        {
            double[] marginalRisk = null;
            try
            {
                DateTime inTime = DateTime.Now;
                Exception RiskAPI_Exception = null;
                try
                {
                    RiskAPI_Exception = null;
                    marginalRisk = riskAPI.RiskAPI_MarginalRisk(inputParams.PSSymbol, inputParams.Quantity, inputParams.Method, inputParams.VolType, inputParams.ConfidenceLevel, RiskConstants.CONST_INCREMENT, inputParams.StartDate, inputParams.EndDate, inputParams.Lambda);
                }
                catch (Exception except)
                {
                    RiskAPI_Exception = except;
                    Logger.HandleException(except, LoggingConstants.POLICY_LOGANDSHOW);

                }
                DateTime outTime = DateTime.Now;

                LoggingPSRiskData(MethodBase.GetCurrentMethod().Name, "RiskAPI_MarginalRisk", inTime, outTime, inputParams, marginalRisk, RiskAPI_Exception);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return marginalRisk;
        }

        private double[] RiskAPIStandardDev(PortfolioSciencesInputParameters inputParams)
        {
            double[] individualStandardDev = null;
            try
            {
                DateTime inTime = DateTime.Now;
                Exception RiskAPI_Exception = null;
                try
                {
                    RiskAPI_Exception = null;
                    individualStandardDev = riskAPI.RiskAPI_StandardDev(inputParams.PSSymbol, inputParams.Quantity, false, inputParams.StartDate, inputParams.EndDate);
                }
                catch (Exception except)
                {
                    RiskAPI_Exception = except;
                    Logger.HandleException(except, LoggingConstants.POLICY_LOGANDSHOW);

                }
                DateTime outTime = DateTime.Now;

                LoggingPSRiskData(MethodBase.GetCurrentMethod().Name, "RiskAPI_StandardDev", inTime, outTime, inputParams, individualStandardDev, RiskAPI_Exception);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return individualStandardDev;
        }

        private double[] RiskAPIVectorCorrelation(PortfolioSciencesInputParameters inputParams)
        {
            double[] correlation = null;
            try
            {
                DateTime inTime = DateTime.Now;
                Exception RiskAPI_Exception = null;
                try
                {
                    RiskAPI_Exception = null;
                    correlation = riskAPI.RiskAPI_VectorCorrelation(inputParams.PSSymbol, inputParams.Quantity, inputParams.BenchMark, inputParams.BenchMarkQuantity, inputParams.StartDate, inputParams.EndDate, false);
                }
                catch (Exception except)
                {
                    RiskAPI_Exception = except;
                    Logger.HandleException(except, LoggingConstants.POLICY_LOGANDSHOW);

                }
                DateTime outTime = DateTime.Now;

                LoggingPSRiskData(MethodBase.GetCurrentMethod().Name, "RiskAPI_VectorCorrelation", inTime, outTime, inputParams, correlation, RiskAPI_Exception);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return correlation;
        }

        private double[] RiskAPIVolatility(PortfolioSciencesInputParameters inputParams)
        {
            double[] historicalVol = null;
            try
            {
                DateTime inTime = DateTime.Now;
                Exception RiskAPI_Exception = null;
                try
                {
                    RiskAPI_Exception = null;
                    historicalVol = riskAPI.RiskAPI_Volatility(inputParams.PSSymbol, inputParams.Quantity, inputParams.VolType, inputParams.StartDate, inputParams.EndDate, inputParams.Lambda, false);
                }
                catch (Exception except)
                {
                    RiskAPI_Exception = except;
                    Logger.HandleException(except, LoggingConstants.POLICY_LOGANDSHOW);

                }
                DateTime outTime = DateTime.Now;

                LoggingPSRiskData(MethodBase.GetCurrentMethod().Name, "RiskAPI_Volatility", inTime, outTime, inputParams, historicalVol, RiskAPI_Exception);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return historicalVol;
        }

        private double[] RiskAPIVectorBeta(PortfolioSciencesInputParameters inputParams)
        {
            double[] individualBeta = null;
            try
            {
                DateTime inTime = DateTime.Now;
                Exception RiskAPI_Exception = null;
                try
                {
                    RiskAPI_Exception = null;
                    individualBeta = riskAPI.RiskAPI_VectorBeta(inputParams.PSSymbol, inputParams.Quantity, inputParams.BenchMark, inputParams.BenchMarkQuantity, inputParams.StartDate, inputParams.EndDate, true);
                }
                catch (Exception except)
                {
                    RiskAPI_Exception = except;
                    Logger.HandleException(except, LoggingConstants.POLICY_LOGANDSHOW);

                }
                DateTime outTime = DateTime.Now;

                LoggingPSRiskData(MethodBase.GetCurrentMethod().Name, "RiskAPI_VectorBeta", inTime, outTime, inputParams, individualBeta, RiskAPI_Exception);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return individualBeta;
        }

        private double[,] RiskAPIStressImpact(PortfolioSciencesInputParameters inputParams)
        {
            double[,] pnlImpact = null;
            try
            {
                DateTime inTime = DateTime.Now;
                Exception RiskAPI_Exception = null;
                try
                {
                    RiskAPI_Exception = null;
                    pnlImpact = riskAPI.RiskAPI_StressImpact(inputParams.PSSymbol, inputParams.Quantity, 5, 0, 0, inputParams.IndexStress, "", inputParams.UnderlyingPrice, inputParams.PxSelectedFeed, inputParams.BenchMark, inputParams.BenchMarkQuantity, false, inputParams.StartDate, inputParams.EndDate);
                }
                catch (Exception except)
                {
                    RiskAPI_Exception = except;
                    Logger.HandleException(except, LoggingConstants.POLICY_LOGANDSHOW);

                }
                DateTime outTime = DateTime.Now;

                LoggingPSRiskData(MethodBase.GetCurrentMethod().Name, "RiskAPI_StressImpact", inTime, outTime, inputParams, pnlImpact, RiskAPI_Exception);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return pnlImpact;
        }
        #endregion

        #region IRiskServices Members
        public void UpdateRiskPreferences(RiskPrefernece Preference)
        {
            try
            {
                RiskPrefernece newRiskPref = Preference;
                RiskPrefernece oldRiskPref = RiskPreferenceManager.RiskPrefernece;
                oldRiskPref.ConfidenceLevelPercent = newRiskPref.ConfidenceLevelPercent;
                oldRiskPref.InterestRateTable = newRiskPref.InterestRateTable;
                oldRiskPref.Method = newRiskPref.Method;
                oldRiskPref.Lambda = newRiskPref.Lambda;
                RiskPreferenceManager.RefreshInterestRate();
                oldRiskPref.UnderlyingPriceType = newRiskPref.UnderlyingPriceType;
                oldRiskPref.VolatilityType = newRiskPref.VolatilityType;
                oldRiskPref.RiskCalculationBasedOn = newRiskPref.RiskCalculationBasedOn;
                oldRiskPref.RiskCalculationCurrency = newRiskPref.RiskCalculationCurrency;
                oldRiskPref.DaysBwDataPoints = newRiskPref.DaysBwDataPoints;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
        }

        public async Task<PranaRequestCarrier> CalculateRiskRelatedData(PranaRequestCarrier pranaRequestCarrier, bool isOnlyRiskCorrelationStdDevRequired)
        {
            PranaRequestCarrier pranaRequestCarrierOutput = CheckSymbolValidity(pranaRequestCarrier);
            try
            {
                SetCommonPreferences();

                bool isMonteCarlo = false;
                if (RiskPreferenceManager.RiskPrefernece.Method == RiskConstants.Method.MonteCarlo)
                {
                    isMonteCarlo = true;
                }

                PortfolioSciencesInputParameters inputParamsIndividual = new PortfolioSciencesInputParameters(pranaRequestCarrierOutput, false);

                if (string.IsNullOrEmpty(inputParamsIndividual.PSSymbol.Trim()))
                {
                    return null;
                }

                Task<double[]> individualRiskTask;
                if (pranaRequestCarrier.RiskParam.IsRiskRequired)
                {
                    individualRiskTask = Task<double[]>.Factory.StartNew(() => RiskAPIIndividualRisk(inputParamsIndividual));
                }
                else
                {
                    individualRiskTask = Task<double[]>.Factory.StartNew(() => { return (new double[] { }); });
                }

                Task<double[]> componentRiskTask;
                if (pranaRequestCarrier.RiskParam.IsComponentRiskRequired)
                {
                    componentRiskTask = Task<double[]>.Factory.StartNew(() => RiskAPIComponentRisk(inputParamsIndividual));
                }
                else
                {
                    componentRiskTask = Task<double[]>.Factory.StartNew(() => { return (new double[] { }); });
                }

                Task<double[]> individualStandardDevTask;
                if (pranaRequestCarrier.RiskParam.IsstddevRequired)
                {
                    individualStandardDevTask = Task<double[]>.Factory.StartNew(() => RiskAPIStandardDev(inputParamsIndividual));
                }
                else
                {
                    individualStandardDevTask = Task<double[]>.Factory.StartNew(() => { return (new double[] { }); });
                }

                Task<double[]> correlationTask;
                if (pranaRequestCarrier.RiskParam.IsCorrelationRequired)
                {
                    correlationTask = Task<double[]>.Factory.StartNew(() => RiskAPIVectorCorrelation(inputParamsIndividual));
                }
                else
                {
                    correlationTask = Task<double[]>.Factory.StartNew(() => { return (new double[] { }); });
                }

                Task<double[]> marginalRiskTask;
                if (!isMonteCarlo && pranaRequestCarrier.RiskParam.IsMarginalRiskRequired)
                {
                    marginalRiskTask = Task<double[]>.Factory.StartNew(() => RiskAPIMarginalRisk(inputParamsIndividual));
                }
                else
                {
                    marginalRiskTask = Task<double[]>.Factory.StartNew(() => { return (new double[] { }); });
                }

                Task<double[]> historicalVolTask;
                if (pranaRequestCarrier.RiskParam.IsVolatilityRequired)
                {
                    historicalVolTask = Task<double[]>.Factory.StartNew(() => RiskAPIVolatility(inputParamsIndividual));
                }
                else
                {
                    historicalVolTask = Task<double[]>.Factory.StartNew(() => { return (new double[] { }); });
                }

                PortfolioSciencesInputParameters inputParamsGroup = new PortfolioSciencesInputParameters(pranaRequestCarrierOutput, true);
                Task<double> groupRiskTask;
                if (pranaRequestCarrier.RiskParam.IsRiskRequired)
                {
                    groupRiskTask = Task<double>.Factory.StartNew(() => CalculateRiskForGroup(inputParamsGroup));
                }
                else
                {
                    groupRiskTask = Task<double>.Factory.StartNew(() => { return (new double { }); });
                }
                Task<double> groupCorrelationTask;
                if (pranaRequestCarrier.RiskParam.IsCorrelationRequired)
                {
                    groupCorrelationTask = Task<double>.Factory.StartNew(() => CalculateCorrelationForGroup(inputParamsGroup));
                }
                else
                {
                    groupCorrelationTask = Task<double>.Factory.StartNew(() => { return (new double { }); });
                }
                Task<double> groupStandardDeviationTask;
                if (pranaRequestCarrier.RiskParam.IsstddevRequired)
                {
                    groupStandardDeviationTask = Task<double>.Factory.StartNew(() => CalculateStandardDeviationForGroup(inputParamsGroup));
                }
                else
                {
                    groupStandardDeviationTask = Task<double>.Factory.StartNew(() => { return (new double { }); });
                }

                int requiredRequestCount = GetRequiredRequestCount(pranaRequestCarrier.RiskParam);
                int requestedSymbolCount = pranaRequestCarrierOutput.IndividualSymbolList.Count;
                int correctResponseCount = 0;
                double[] individualRisk = null;
                if (pranaRequestCarrier.RiskParam.IsRiskRequired)
                {
                    await individualRiskTask;
                    individualRisk = individualRiskTask.Result;
                    if (individualRisk != null && (individualRisk.Length == requestedSymbolCount))
                    {
                        correctResponseCount = correctResponseCount + 1;
                    }
                }

                double[] componentRisk = null;
                if (pranaRequestCarrier.RiskParam.IsComponentRiskRequired)
                {
                    await componentRiskTask;
                    componentRisk = componentRiskTask.Result;
                    if (componentRisk != null && (componentRisk.Length == requestedSymbolCount))
                    {
                        correctResponseCount = correctResponseCount + 1;
                    }
                }

                double[] individualStandardDev = null;
                if (pranaRequestCarrier.RiskParam.IsstddevRequired)
                {
                    await individualStandardDevTask;
                    individualStandardDev = individualStandardDevTask.Result;
                    if (individualStandardDev != null && (individualStandardDev.Length == requestedSymbolCount))
                    {
                        correctResponseCount = correctResponseCount + 1;
                    }
                }

                double[] correlation = null;
                if (pranaRequestCarrier.RiskParam.IsCorrelationRequired)
                {
                    await correlationTask;
                    correlation = correlationTask.Result;
                    if (correlation != null && (correlation.Length == requestedSymbolCount))
                    {
                        correctResponseCount = correctResponseCount + 1;
                    }
                }

                double[] marginalRisk = null;
                if (!isMonteCarlo && pranaRequestCarrier.RiskParam.IsMarginalRiskRequired)
                {
                    await marginalRiskTask;
                    marginalRisk = marginalRiskTask.Result;
                    if (marginalRisk != null && (marginalRisk.Length == requestedSymbolCount))
                    {
                        correctResponseCount = correctResponseCount + 1;
                    }
                }

                double[] historicalVol = null;
                if (pranaRequestCarrier.RiskParam.IsVolatilityRequired)
                {
                    await historicalVolTask;
                    historicalVol = historicalVolTask.Result;
                    if (historicalVol != null && (historicalVol.Length == requestedSymbolCount))
                    {
                        correctResponseCount = correctResponseCount + 1;
                    }
                }

                int counter = 0;
                if (correctResponseCount == requiredRequestCount)
                {
                    foreach (KeyValuePair<string, PranaRiskResult> riskResult in pranaRequestCarrierOutput.IndividualSymbolList)
                    {
                        if (individualRisk != null)
                            riskResult.Value.Risk = Math.Round(individualRisk[counter]);
                        if (individualStandardDev != null)
                            riskResult.Value.StandardDeviation = Math.Round(individualStandardDev[counter]);
                        if (correlation != null)
                            riskResult.Value.Correlation = correlation[counter];
                        if (componentRisk != null)
                            riskResult.Value.ComponentRisk = Math.Round(componentRisk[counter]);
                        if (historicalVol != null)
                            riskResult.Value.PSVolatility = historicalVol[counter];
                        if (!isMonteCarlo && marginalRisk != null)
                            riskResult.Value.MarginalRisk = Math.Round(marginalRisk[counter]);

                        counter++;
                    }
                }
                else
                {
                    #region LoggingRiskData
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Risk not showing proper results due to Portfolio science not giving output for all symbols", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);

                    if (Prana.BusinessObjects.UserSettingConstants.IsRiskLoggingEnabled)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(Environment.NewLine);
                        sb.Append("Risk: Input and Output rows count are not same.");
                        sb.Append(Environment.NewLine);
                        sb.Append("Rows Sent: " + requestedSymbolCount);
                        if (individualRisk == null)
                        {
                            sb.Append(Environment.NewLine);
                            sb.Append("Received Individual Risk is Null");
                        }
                        else if (individualRisk.Length != requestedSymbolCount)
                        {
                            sb.Append(Environment.NewLine);
                            sb.Append("Individual Risk Received for rows: " + individualRisk.Length);
                        }

                        if (individualStandardDev == null)
                        {
                            sb.Append(Environment.NewLine);
                            sb.Append("Received Individual Std Deviation is Null");
                        }
                        else if (individualStandardDev.Length != requestedSymbolCount)
                        {
                            sb.Append(Environment.NewLine);
                            sb.Append("Individual Std Deviation Received for rows: " + individualStandardDev.Length);
                        }

                        if (correlation == null)
                        {
                            sb.Append(Environment.NewLine);
                            sb.Append("Received Correlation is Null");
                        }
                        else if (correlation.Length != requestedSymbolCount)
                        {
                            sb.Append(Environment.NewLine);
                            sb.Append("Correlation Received for rows: " + correlation.Length);
                        }

                        if (componentRisk == null)
                        {
                            sb.Append(Environment.NewLine);
                            sb.Append("Received Component Risk is Null");
                        }
                        else if (componentRisk.Length != requestedSymbolCount)
                        {
                            sb.Append(Environment.NewLine);
                            sb.Append("Component Risk Received for rows: " + componentRisk.Length);
                        }

                        if (!isOnlyRiskCorrelationStdDevRequired)
                        {
                            if (historicalVol == null)
                            {
                                sb.Append(Environment.NewLine);
                                sb.Append("Received Historical Vol is Null");
                            }
                            else if (historicalVol.Length != requestedSymbolCount)
                            {
                                sb.Append(Environment.NewLine);
                                sb.Append("Historical Vol Received for rows: " + historicalVol.Length);
                            }

                            if (!isMonteCarlo)
                            {
                                if (marginalRisk == null)
                                {
                                    sb.Append(Environment.NewLine);
                                    sb.Append("Received Marginal Risk is Null");
                                }
                                else if (marginalRisk.Length != requestedSymbolCount)
                                {
                                    sb.Append(Environment.NewLine);
                                    sb.Append("Marginal Risk Received for rows: " + marginalRisk.Length);
                                }
                            }
                        }
                        Logger.LoggerWrite(sb, LoggingConstants.RISK_LOGGING, 1, 1, TraceEventType.Information);
                    }
                    #endregion
                }

                await groupRiskTask;
                pranaRequestCarrierOutput.PortfolioRisk = groupRiskTask.Result;

                await groupCorrelationTask;
                pranaRequestCarrierOutput.Correlation = groupCorrelationTask.Result;

                await groupStandardDeviationTask;
                pranaRequestCarrierOutput.StandardDeviation = groupStandardDeviationTask.Result;

                return pranaRequestCarrierOutput;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return null;
        }

        private int GetRequiredRequestCount(RiskParamameter riskParamameter)
        {
            int correctCountToCheck = 0;
            try
            {
                bool isMonteCarlo = false;
                if (RiskPreferenceManager.RiskPrefernece.Method == RiskConstants.Method.MonteCarlo)
                {
                    isMonteCarlo = true;
                }
                if (riskParamameter.IsVolatilityRequired)
                {
                    correctCountToCheck = correctCountToCheck + 1;
                }
                if (!isMonteCarlo && riskParamameter.IsMarginalRiskRequired)
                {
                    correctCountToCheck = correctCountToCheck + 1;
                }

                if (riskParamameter.IsCorrelationRequired)
                {
                    correctCountToCheck = correctCountToCheck + 1;
                }
                if (riskParamameter.IsstddevRequired)
                {
                    correctCountToCheck = correctCountToCheck + 1;
                }

                if (riskParamameter.IsComponentRiskRequired)
                {
                    correctCountToCheck = correctCountToCheck + 1;
                }

                if (riskParamameter.IsRiskRequired)
                {
                    correctCountToCheck = correctCountToCheck + 1;
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return correctCountToCheck;
        }

        private int GetRequiredRequestCountForBeta(RiskParamameter riskParamameter)
        {
            int correctCountToCheck = 0;

            try
            {
                if (riskParamameter.IsCorrelationRequired)
                {
                    correctCountToCheck = correctCountToCheck + 1;
                }
                if (riskParamameter.IsImpactRequired)
                {
                    correctCountToCheck = correctCountToCheck + 1;
                }
                if (riskParamameter.IsBetaRequired)
                {
                    correctCountToCheck = correctCountToCheck + 1;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return correctCountToCheck;
        }

        public async Task<PranaRequestCarrier> CalculateStressTestData(PranaRequestCarrier pranaRequestCarrier, PranaRequestCarrier pranaRequestCarrierForBeta)
        {
            PranaRequestCarrier pranaRequestCarrierOutput = CheckSymbolValidity(pranaRequestCarrier);
            PranaRequestCarrier pranaRequestCarrierForBetaOutput = CheckSymbolValidity(pranaRequestCarrierForBeta);
            try
            {
                SetCommonPreferences();

                PortfolioSciencesInputParameters inputParamsIndividual = new PortfolioSciencesInputParameters(pranaRequestCarrierOutput, false);

                if (string.IsNullOrEmpty(inputParamsIndividual.PSSymbol.Trim()))
                {
                    return null;
                }

                Task<double[]> vectorCorrelationTask;
                if (pranaRequestCarrier.RiskParam.IsCorrelationRequired)
                {
                    vectorCorrelationTask = Task<double[]>.Factory.StartNew(() => RiskAPIVectorCorrelation(inputParamsIndividual));
                }
                else
                {
                    vectorCorrelationTask = Task<double[]>.Factory.StartNew(() => { return (new double[] { }); });
                }

                Task<double[,]> stressImpactTask;
                if (pranaRequestCarrier.RiskParam.IsImpactRequired)
                {
                    stressImpactTask = Task<double[,]>.Factory.StartNew(() => RiskAPIStressImpact(inputParamsIndividual));
                }
                else
                {
                    stressImpactTask = Task<double[,]>.Factory.StartNew(() => { return (new double[,] { }); });
                }

                PortfolioSciencesInputParameters inputParamsGroup = new PortfolioSciencesInputParameters(pranaRequestCarrierOutput, true);
                Task<double> groupBenchmarkValueTask = Task<double>.Factory.StartNew(() => CalculateBenchmarkValueForGroup(inputParamsGroup));
                Task<double> groupCorrelationTask;
                if (pranaRequestCarrier.RiskParam.IsCorrelationRequired)
                {
                    groupCorrelationTask = Task<double>.Factory.StartNew(() => CalculateCorrelationForGroup(inputParamsGroup));
                }
                else
                {
                    groupCorrelationTask = Task<double>.Factory.StartNew(() => { return (new double { }); });
                }
                Task<double> groupStressImpactTask;
                if (pranaRequestCarrier.RiskParam.IsImpactRequired)
                {
                    groupStressImpactTask = Task<double>.Factory.StartNew(() => CalculateStressImpactForGroup(inputParamsGroup));
                }
                else
                {
                    groupStressImpactTask = Task<double>.Factory.StartNew(() => { return (new double { }); });
                }

                PortfolioSciencesInputParameters inputParamsForIndividualBeta = new PortfolioSciencesInputParameters(pranaRequestCarrierForBetaOutput, false);

                Task<double[]> vectorBetaTask;
                if (pranaRequestCarrier.RiskParam.IsBetaRequired)
                {
                    vectorBetaTask = Task<double[]>.Factory.StartNew(() => RiskAPIVectorBeta(inputParamsForIndividualBeta));
                }
                else
                {
                    vectorBetaTask = Task<double[]>.Factory.StartNew(() => { return (new double[] { }); });
                }
                PortfolioSciencesInputParameters inputParamsForGroupBeta = new PortfolioSciencesInputParameters(pranaRequestCarrierForBetaOutput, true);

                Task<double> groupBetaTask;
                if (pranaRequestCarrier.RiskParam.IsBetaRequired)
                {
                    groupBetaTask = Task<double>.Factory.StartNew(() => CalculateBetaForGroup(inputParamsForGroupBeta));
                }
                else
                {
                    groupBetaTask = Task<double>.Factory.StartNew(() => { return (new double { }); });
                }


                int correctResponseCount = 0;
                int requiredRequestCount = GetRequiredRequestCountForBeta(pranaRequestCarrier.RiskParam);
                int requestedSymbolCountForBeta = pranaRequestCarrierForBetaOutput.IndividualSymbolList.Count;
                int requestedSymbolCount = pranaRequestCarrierOutput.IndividualSymbolList.Count;
                double[] correlation = null;
                if (pranaRequestCarrier.RiskParam.IsCorrelationRequired)
                {
                    await vectorCorrelationTask;
                    correlation = vectorCorrelationTask.Result;
                    if (correlation != null && correlation.Length == requestedSymbolCount)
                    {
                        correctResponseCount = correctResponseCount + 1;
                    }
                }

                double[,] pnlImpact = null;
                if (pranaRequestCarrier.RiskParam.IsImpactRequired)
                {
                    await stressImpactTask;
                    pnlImpact = stressImpactTask.Result;
                    if (pnlImpact != null && pnlImpact.Length == 2 * requestedSymbolCount)
                    {
                        correctResponseCount = correctResponseCount + 1;
                    }

                }

                double[] individualBeta = null;
                if (pranaRequestCarrier.RiskParam.IsBetaRequired)
                {
                    await vectorBetaTask;
                    individualBeta = vectorBetaTask.Result;
                    if (individualBeta != null && individualBeta.Length == requestedSymbolCountForBeta)
                    {
                        correctResponseCount = correctResponseCount + 1;
                    }
                }


                int counter = 0;
                Dictionary<string, double> underlyingSymbolBeta = new Dictionary<string, double>();

                if (individualBeta != null && individualBeta.Length == requestedSymbolCountForBeta)
                {
                    foreach (KeyValuePair<string, PranaRiskResult> kvp in pranaRequestCarrierForBetaOutput.IndividualSymbolList)
                    {
                        underlyingSymbolBeta.Add(kvp.Value.Symbol, individualBeta[counter]);
                        counter++;
                    }
                }

                counter = 0;

                //  if (correlation != null && correlation.Length == requestedSymbolCount && individualBeta != null && individualBeta.Length == requestedSymbolCountForBeta && pnlImpact != null && pnlImpact.Length == 2 * requestedSymbolCount)
                if (correctResponseCount == requiredRequestCount)
                {
                    foreach (KeyValuePair<string, PranaRiskResult> riskResult in pranaRequestCarrierOutput.IndividualSymbolList)
                    {
                        if (underlyingSymbolBeta != null && underlyingSymbolBeta.ContainsKey(riskResult.Value.UnderlyingSymbol))
                            riskResult.Value.Beta = underlyingSymbolBeta[riskResult.Value.UnderlyingSymbol];
                        if (correlation != null)
                            riskResult.Value.Correlation = correlation[counter];
                        if (pnlImpact != null && !double.IsNaN(pnlImpact[0, counter]))
                        {
                            riskResult.Value.PNLImpact = pnlImpact[0, counter];
                        }
                        counter++;
                    }
                }
                else
                {
                    #region LoggingRiskData
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Risk not showing proper results due to Portfolio science not giving output for all symbols", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);

                    if (Prana.BusinessObjects.UserSettingConstants.IsRiskLoggingEnabled)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(Environment.NewLine);
                        sb.Append("Risk: Input and Output rows count are not same.");
                        sb.Append(Environment.NewLine);
                        sb.Append("Rows Sent for Correlation: " + requestedSymbolCount + ", and For Beta: " + requestedSymbolCountForBeta);
                        if (correlation == null)
                        {
                            sb.Append(Environment.NewLine);
                            sb.Append("Received Correlation is Null");
                        }
                        else if (correlation.Length != requestedSymbolCount)
                        {
                            sb.Append(Environment.NewLine);
                            sb.Append("Correlation Received for rows: " + correlation.Length);
                        }

                        if (pnlImpact == null)
                        {
                            sb.Append(Environment.NewLine);
                            sb.Append("Received PNLImpact is Null");
                        }
                        else if (pnlImpact.Length != 2 * requestedSymbolCount)
                        {
                            sb.Append(Environment.NewLine);
                            sb.Append("PNLImpact Received for rows: " + pnlImpact.Length);
                        }

                        if (individualBeta == null)
                        {
                            sb.Append(Environment.NewLine);
                            sb.Append("Received Beta is Null");
                        }
                        else if (individualBeta.Length != requestedSymbolCountForBeta)
                        {
                            sb.Append(Environment.NewLine);
                            sb.Append("Beta Received for rows: " + individualBeta.Length);
                        }
                        Logger.LoggerWrite(sb, LoggingConstants.RISK_LOGGING, 1, 1, TraceEventType.Information);
                    }
                    #endregion
                }

                await groupBenchmarkValueTask;
                pranaRequestCarrierOutput.BenchMarkValue = groupBenchmarkValueTask.Result;

                await groupCorrelationTask;
                pranaRequestCarrierOutput.Correlation = groupCorrelationTask.Result;

                await groupStressImpactTask;
                pranaRequestCarrierOutput.PNLImpact = groupStressImpactTask.Result;

                await groupBetaTask;
                pranaRequestCarrierOutput.Beta = groupBetaTask.Result;

                return pranaRequestCarrierOutput;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return null;
        }

        public PranaRequestCarrier CalculateHistoricalVol(PranaRequestCarrier pranaRequestCarrier)
        {
            PranaRequestCarrier pranaRequestCarrierOutput = CheckSymbolValidity(pranaRequestCarrier);
            try
            {
                IsConnected();
                SetCommonPreferences();

                List<string> listSymbols = new List<string>();
                foreach (KeyValuePair<string, PranaRiskResult> item in pranaRequestCarrierOutput.IndividualSymbolList)
                {
                    listSymbols.Add(item.Key);
                }
                string symbols = GeneralUtilities.GetStringFromList(listSymbols, ',');

                List<string> listQuantities = new List<string>();
                foreach (string item in pranaRequestCarrierOutput.IndividualSymbolList.Keys)
                {
                    listQuantities.Add("1");
                }
                string quantitys = GeneralUtilities.GetStringFromList(listQuantities, ',');

                int volType = Convert.ToInt32(pranaRequestCarrierOutput.VolatilityType);
                double lambda = RiskPreferenceManager.RiskPrefernece.Lambda;
                string startDate = pranaRequestCarrierOutput.StartDate.ToString("MM/dd/yy");
                string endDate = pranaRequestCarrierOutput.EndDate.ToString("MM/dd/yy");

                PortfolioSciencesInputParameters inputParams = new PortfolioSciencesInputParameters();
                inputParams.PSSymbol = symbols;
                inputParams.Quantity = quantitys;
                inputParams.VolType = volType;
                inputParams.Lambda = lambda;
                inputParams.StartDate = startDate;
                inputParams.EndDate = endDate;
                inputParams.SymbolCount = listSymbols.Count;

                double[] historicalVol = RiskAPIVolatility(inputParams);

                int counter = 0;
                int requestedSymbolCount = pranaRequestCarrierOutput.IndividualSymbolList.Count;
                if (historicalVol != null && historicalVol.Length == requestedSymbolCount)
                {
                    foreach (KeyValuePair<string, PranaRiskResult> riskResult in pranaRequestCarrierOutput.IndividualSymbolList)
                    {
                        riskResult.Value.PSVolatility = historicalVol[counter];
                        counter++;
                    }
                }
                else
                {
                    #region LoggingRiskData
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Risk not showing proper results due to Portfolio science not giving output for all symbols", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);

                    if (Prana.BusinessObjects.UserSettingConstants.IsRiskLoggingEnabled)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(Environment.NewLine);
                        sb.Append("Risk: Input and Output rows count are not same.");
                        sb.Append(Environment.NewLine);
                        sb.Append("Rows Sent: " + requestedSymbolCount);
                        if (historicalVol == null)
                        {
                            sb.Append(Environment.NewLine);
                            sb.Append("Received Historical Vol is Null");
                        }
                        else if (historicalVol.Length != requestedSymbolCount)
                        {
                            sb.Append(Environment.NewLine);
                            sb.Append("Historical Vol Received for rows: " + historicalVol.Length);
                        }
                        Logger.LoggerWrite(sb, LoggingConstants.RISK_LOGGING, 1, 1, TraceEventType.Information);
                    }
                    #endregion
                }
                return pranaRequestCarrierOutput;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return null;
        }
        #endregion

        #region IServiceOnDemandStatus Members
        public async System.Threading.Tasks.Task<bool> HealthCheck()
        {
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;

            return true;
        }
        #endregion

        public bool CheckRiskServiceConnected()
        {
            try
            {
                IsConnected();
                return _isConnected;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return false;
        }
    }
}