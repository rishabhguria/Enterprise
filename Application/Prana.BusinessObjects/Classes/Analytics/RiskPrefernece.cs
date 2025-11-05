using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class RiskPrefernece : IDisposable
    {
        public RiskPrefernece()
        {
        }

        private RiskConstants.VolType _volType = RiskConstants.VolType.Normal;
        private RiskConstants.Method _method = RiskConstants.Method.VarianceCovariance;
        private PriceUsedType _underlyingPriceType = PriceUsedType.Last;
        private int _confidenceLevelPercent = 95;
        private double _lambda = 0.94;
        private DataSet _interesRateTable;
        private RiskConstants.RiskCalculationBasedOn _riskCalculationBasedOn = RiskConstants.RiskCalculationBasedOn.Quantity;
        private string _riskCalculationCurrency = "USD";
        private int _daysBwDataPoints = 1;
        private bool _useExportFileFormat = false;
        private string _riskExportFileName;
        private string _riskExportDateFormat = string.Empty;
        private bool _isAutoLoadDataOnStartup = true;
        public int _stressTestDateRange = 30;
        public int _riskReportDateRange = 30;
        public int _riskSimulationDateRange = 30;

        [NonSerialized]
        private DataSet _symbolMappingTable;

        [NonSerialized]
        [XmlElement("StepAnalysisViewPreferencesList")]
        private SerializableDictionary<string, StepAnalysisPref> _stepAnalPreferencesDict = new SerializableDictionary<string, StepAnalysisPref>();

        [NonSerialized]
        [XmlElement("ExportColumnList")]
        private SerializableDictionary<string, bool> _exportColumnList = new SerializableDictionary<string, bool>();

        private int _maxStressTestViewsWithVolSkew = 5;
        public int MaxStressTestViewsWithVolSkew
        {
            get { return _maxStressTestViewsWithVolSkew; }
            set { _maxStressTestViewsWithVolSkew = value; }
        }

        private int _maxStressTestViewsWithoutVolSkew = 5;
        public int MaxStressTestViewsWithoutVolSkew
        {
            get { return _maxStressTestViewsWithoutVolSkew; }
            set { _maxStressTestViewsWithoutVolSkew = value; }
        }

        private string _groupByColumnForExport = "None";
        public string GroupByColumnForExport
        {
            get { return _groupByColumnForExport; }
            set { _groupByColumnForExport = value; }
        }

        public void UpdateExportColumnList(SerializableDictionary<string, bool> exportColumnList)
        {
            _exportColumnList = exportColumnList;
        }

        public StepAnalysisPref GetStepAnalViewPreferences(string viewName)
        {
            StepAnalysisPref preference = null;
            try
            {
                if (_stepAnalPreferencesDict.ContainsKey(viewName))
                {
                    return _stepAnalPreferencesDict[viewName];
                }
                //return default value 
                preference = new StepAnalysisPref(viewName);
                //preference.StepAnalViewID = viewID;
                preference.DtVolShockFactor = DtVolShockFactorDefault;
                _stepAnalPreferencesDict.Add(viewName, preference);
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
            return preference;
        }

        public void UpdateStepAnalPrefDict(string key, StepAnalysisPref stepAnalpreferences)
        {
            try
            {
                if (_stepAnalPreferencesDict.ContainsKey(key))
                {
                    _stepAnalPreferencesDict[key] = stepAnalpreferences;
                }
                else
                {
                    _stepAnalPreferencesDict.Add(key, stepAnalpreferences);
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
        }

        public RiskConstants.VolType VolatilityType
        {
            get { return _volType; }
            set { _volType = value; }
        }

        public RiskConstants.Method Method
        {
            get { return _method; }
            set { _method = value; }
        }

        public PriceUsedType UnderlyingPriceType
        {
            get { return _underlyingPriceType; }
            set { _underlyingPriceType = value; }
        }

        public int ConfidenceLevelPercent
        {
            get { return _confidenceLevelPercent; }
            set { _confidenceLevelPercent = value; }
        }

        public double ConfidenceLevel
        {
            get { return _confidenceLevelPercent / 100; }
        }

        public double Lambda
        {
            get { return _lambda; }
            set { _lambda = value; }
        }

        public RiskConstants.RiskCalculationBasedOn RiskCalculationBasedOn
        {
            get { return _riskCalculationBasedOn; }
            set { _riskCalculationBasedOn = value; }
        }

        public string RiskCalculationCurrency
        {
            get { return _riskCalculationCurrency; }
            set { _riskCalculationCurrency = value; }
        }

        public int DaysBwDataPoints
        {
            get { return _daysBwDataPoints; }
            set { _daysBwDataPoints = value; }
        }

        public SerializableDictionary<string, StepAnalysisPref> StepAnalPreferencesDict
        {
            get { return _stepAnalPreferencesDict; }
            set { _stepAnalPreferencesDict = value; }
        }

        public SerializableDictionary<string, bool> ExportColumnList
        {
            get
            {
                return _exportColumnList;
            }
            set { _exportColumnList = value; }
        }
        public string RiskExportFileName
        {
            get { return _riskExportFileName; }
            set { _riskExportFileName = value; }
        }
        public bool UseExportFileFormat
        {
            get { return _useExportFileFormat; }
            set { _useExportFileFormat = value; }
        }
        public string RiskExportDateFormat
        {
            get { return _riskExportDateFormat; }
            set { _riskExportDateFormat = value; }
        }
        [XmlIgnore]
        public DataSet InterestRateTable
        {
            get { return _interesRateTable; }
            set { _interesRateTable = value; }
        }

        [XmlIgnore]
        public DataSet SymbolMappingTable
        {
            get { return _symbolMappingTable; }
            set { _symbolMappingTable = value; }
        }

        [NonSerialized]
        private DataTable _dtVolShockFactorDefault = new DataTable();
        [XmlIgnore]
        public DataTable DtVolShockFactorDefault
        {
            get { return _dtVolShockFactorDefault; }
            set { _dtVolShockFactorDefault = value; }
        }

        public bool IsAutoLoadDataOnStartup
        {
            get { return _isAutoLoadDataOnStartup; }
            set { _isAutoLoadDataOnStartup = value; }
        }

        [XmlArray("Grouping"), XmlArrayItem("Column", typeof(string))]
        public List<string> Grouping = new List<string>();
        public int BackColorLevel1 = -9868951; // Gray
        public int BackColorLevel2 = -8355712; // Dim Gray
        public int BackColorLevel3 = -5658199; // Light Gray.
        public int ForeColorLevel1 = -1; // White
        public int ForeColorLevel2 = -1; // White
        public int ForeColorLevel3 = -1; // White   
        public decimal FontSize = 11m;
        public int ColorSummaryText = -1; // White

        public bool WrapHeader = false;

        #region StressTestTab Preferences
        private string _stressTestColumns = String.Empty;
        public string StressTestColumns
        {
            get { return _stressTestColumns; }
            set { _stressTestColumns = value; }
        }

        private double _benchmarkPercentMove;
        public double BenchMarkPercentMove
        {
            get { return _benchmarkPercentMove; }
            set { _benchmarkPercentMove = value; }
        }

        private string _stressTestTabBenchMarkSymbolName;
        public string StressTestTabBenchMarkSymbolName
        {
            get { return _stressTestTabBenchMarkSymbolName; }
            set { _stressTestTabBenchMarkSymbolName = value; }
        }

        private bool _includeCash;
        public bool IncludeCash
        {
            get { return _includeCash; }
            set { _includeCash = value; }
        }
        #endregion

        #region RiskSimulationTab Preferences
        private string _riskSimulationColumns = String.Empty;
        public string RiskSimulationColumns
        {
            get { return _riskSimulationColumns; }
            set { _riskSimulationColumns = value; }
        }

        private string _correlationSymbolName;
        public string CorrelationSymbolName
        {
            get { return _correlationSymbolName; }
            set { _correlationSymbolName = value; }
        }
        #endregion

        #region RiskReportTab Preferences
        private string _riskReportColumns = String.Empty;
        public string RiskReportColumns
        {
            get { return _riskReportColumns; }
            set { _riskReportColumns = value; }
        }

        private string _riskReportsTabBenchMarkSymbolName;
        public string RiskReportsTabBenchMarkSymbolName
        {
            get { return _riskReportsTabBenchMarkSymbolName; }
            set { _riskReportsTabBenchMarkSymbolName = value; }
        }
        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_dtVolShockFactorDefault != null)
                        _dtVolShockFactorDefault.Dispose();
                    if (DtVolShockFactorDefault != null)
                        DtVolShockFactorDefault.Dispose();
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
