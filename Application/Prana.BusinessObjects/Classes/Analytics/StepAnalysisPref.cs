using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class StepAnalysisPref
    {
        private string _stepAnalViewName;
        public string StepAnalViewName
        {
            get { return _stepAnalViewName; }
            set { _stepAnalViewName = value; }
        }

        private string _stepAnalViewID;
        public string StepAnalViewID
        {
            get { return _stepAnalViewID; }
            set { _stepAnalViewID = value; }
        }

        private string _stepAnalysisColumns = String.Empty;
        public string StepAnalysisColumns
        {
            get { return _stepAnalysisColumns; }
            set { _stepAnalysisColumns = value; }
        }

        private decimal _changeVolatility = 0;
        public decimal ChangeVolatility
        {
            get { return _changeVolatility; }
            set { _changeVolatility = value; }
        }

        private decimal _changeUnderlyingPrice = 0;
        public decimal ChangeUnderlyingPrice
        {
            get { return _changeUnderlyingPrice; }
            set { _changeUnderlyingPrice = value; }
        }

        private decimal _changeInterestRate = 0;
        public decimal ChangeInterestRate
        {
            get { return _changeInterestRate; }
            set { _changeInterestRate = value; }
        }

        private decimal _changeDaysToExpiration = 0;
        public decimal ChangeDaysToExpiration
        {
            get { return _changeDaysToExpiration; }
            set { _changeDaysToExpiration = value; }
        }

        private bool _isVolShock = false;
        public bool IsVolShock
        {
            get { return _isVolShock; }
            set { _isVolShock = value; }
        }

        private bool _isUnderlyingShock = false;
        public bool IsUnderlyingShock
        {
            get { return _isUnderlyingShock; }
            set { _isUnderlyingShock = value; }
        }

        private bool _isIntRateShock = false;
        public bool IsIntRateShock
        {
            get { return _isIntRateShock; }
            set { _isIntRateShock = value; }
        }

        private bool _isDaysToExpShock = false;
        public bool IsDaysToExpShock
        {
            get { return _isDaysToExpShock; }
            set { _isDaysToExpShock = value; }
        }

        private bool _useVolSkew;
        public bool UseVolSkew
        {
            get { return _useVolSkew; }
            set { _useVolSkew = value; }
        }

        private bool _useNonParallelShifts;
        public bool UseNonParallelShifts
        {
            get { return _useNonParallelShifts; }
            set { _useNonParallelShifts = value; }
        }

        private bool _useAbsoluteValuesForUnderlyingPrice;
        public bool UseAbsoluteValuesForUnderlyingPrice
        {
            get { return _useAbsoluteValuesForUnderlyingPrice; }
            set { _useAbsoluteValuesForUnderlyingPrice = value; }
        }

        private bool _useStressTestDataInStepAnalysis;
        public bool UseStressTestDataInStepAnalysis
        {
            get { return _useStressTestDataInStepAnalysis; }
            set { _useStressTestDataInStepAnalysis = value; }
        }

        private bool _useVolShockAdjustment;
        public bool UseVolShockAdjustment
        {
            get { return _useVolShockAdjustment; }
            set { _useVolShockAdjustment = value; }
        }

        private DataTable _dtGroupShocks = null;
        //[XmlIgnore]
        public DataTable DtGroupShocks
        {
            get
            {
                //if (_dtGroupShocks == null)
                //{  
                //    _dtGroupShocks = GetDefaultGroupShocksDataTable();
                //}
                return _dtGroupShocks;
            }
            set
            {
                _dtGroupShocks = value;

                UpdateGroupWiseSimulationInputsDictionary();
            }
        }

        private DataTable _dtVolShockFactor = null;
        //[XmlIgnore]
        public DataTable DtVolShockFactor
        {
            get { return _dtVolShockFactor; }
            set { _dtVolShockFactor = value; }
        }

        private SerializableDictionary<int, string> _allowedAssetCategory = null;
        public SerializableDictionary<int, string> AllowedAssetCategory
        {
            get { return _allowedAssetCategory; }
            set { _allowedAssetCategory = value; }
        }

        private string _groupShockFilter = null;
        public string GroupShockFilter
        {
            get { return _groupShockFilter; }
            set { _groupShockFilter = value; }
        }

        private Dictionary<string, OptionSimulationInputs> _dictGroupWiseShocks = new Dictionary<string, OptionSimulationInputs>();

        public StepAnalysisPref()
        {
        }

        public StepAnalysisPref(string viewName)
        {
            _stepAnalViewName = viewName;
            UpdateGroupWiseSimulationInputsDictionary();
        }
        private bool _useBetaAdjPrice;
        public bool UseBetaAdjPrice
        {
            get { return _useBetaAdjPrice; }
            set { _useBetaAdjPrice = value; }
        }

        public void SetValues(decimal changeVolatility, decimal changeUnderlyingPrice, decimal changeInterestRate, decimal changeDaysToExpiration, bool isVolShocked, bool isUnderlyingShocked, bool isIntRateShocked, bool isDaysToExpShocked, bool useVolSkew, bool useAbsoluteValuesForUnderlyingPrice, bool useStressTestDataInStepAnalysis, Dictionary<int, string> allowedAssetCategory, bool useBetaAdjPrice)
        {
            _changeVolatility = changeVolatility;
            _changeUnderlyingPrice = changeUnderlyingPrice;
            _changeInterestRate = changeInterestRate;
            _changeDaysToExpiration = changeDaysToExpiration;
            _isVolShock = isVolShocked;
            _isUnderlyingShock = isUnderlyingShocked;
            _isDaysToExpShock = isDaysToExpShocked;
            _isIntRateShock = isIntRateShocked;
            _useVolSkew = useVolSkew;
            _useAbsoluteValuesForUnderlyingPrice = useAbsoluteValuesForUnderlyingPrice;
            _useStressTestDataInStepAnalysis = useStressTestDataInStepAnalysis;
            _allowedAssetCategory = new SerializableDictionary<int, string>();
            foreach (KeyValuePair<int, string> kvp in allowedAssetCategory)
            {
                _allowedAssetCategory.Add(kvp.Key, kvp.Value);
            }
            _useBetaAdjPrice = useBetaAdjPrice;
        }

        private void UpdateGroupWiseSimulationInputsDictionary()
        {
            OptionSimulationInputs inputs = null;
            if (_dtGroupShocks != null && _dtGroupShocks.Rows.Count > 0)
            {
                foreach (DataRow row in _dtGroupShocks.Rows)
                {
                    if (row != null && row.Table.Columns.Contains("Group") && row.Table.Columns.Contains("UnderlyingPriceShock")
                        && row.Table.Columns.Contains("VolatilityShock") && row.Table.Columns.Contains("DaysToExpShock") && row.Table.Columns.Contains("InteresRateShock"))
                    {
                        inputs = new OptionSimulationInputs();

                        string positionType = row["Group"] != null ? row["Group"].ToString() : string.Empty;
                        string groupingParam = row["PositionType"] != null ? row["PositionType"].ToString() : string.Empty;
                        inputs.ChangeVolatility = row["VolatilityShock"] != null ? 1 + (Convert.ToDouble(row["VolatilityShock"].ToString()) / 100) : inputs.ChangeVolatility;
                        //Bharat Kumar Jangir (11 December 2013)
                        //Check for Underlying Price used as absolute value or percentage value
                        if (UseAbsoluteValuesForUnderlyingPrice)
                        {
                            inputs.ChangeUnderlyingPrice = row["UnderlyingPriceShock"] != null ? Convert.ToDouble(row["UnderlyingPriceShock"].ToString()) : inputs.ChangeUnderlyingPrice;
                            inputs.UnderlyingPriceAsAbosluteValue = true;
                        }
                        else
                        {
                            inputs.ChangeUnderlyingPrice = row["UnderlyingPriceShock"] != null ? 1 + (Convert.ToDouble(row["UnderlyingPriceShock"].ToString()) / 100) : inputs.ChangeUnderlyingPrice;
                        }
                        inputs.ChangeInterestRate = row["InteresRateShock"] != null ? 1 + (Convert.ToDouble(row["InteresRateShock"].ToString()) / 100) : inputs.ChangeInterestRate;
                        inputs.ChangeDaysToExpiration = row["DaysToExpShock"] != null ? Convert.ToInt32(row["DaysToExpShock"].ToString()) : inputs.ChangeDaysToExpiration;

                        //Bharat Kumar Jangir (05 December 2013)
                        //Adding all shocks to dictionary on the opening of Risk UI
                        //Currently we are only adding PositionSideExpPortfolio shocks
                        if (groupingParam != string.Empty && positionType != string.Empty) // && positionType == GroupShockFilter)
                        {
                            if (_dictGroupWiseShocks.ContainsKey(groupingParam + "-" + positionType))
                            {
                                _dictGroupWiseShocks[groupingParam + "-" + positionType] = inputs;
                            }
                            else
                            {
                                _dictGroupWiseShocks.Add(groupingParam + "-" + positionType, inputs);
                            }
                        }
                    }
                    else
                    {
                        //this else case for handling the old preferences
                        _dtGroupShocks = null;
                        break;
                    }
                }
            }

        }

        public OptionSimulationInputs GetGroupSimulationInputs(string groupBasis, bool isDefault)
        {
            OptionSimulationInputs SimInputs = null;

            if (!isDefault)
            {
                if (_dictGroupWiseShocks.ContainsKey(groupBasis))
                {
                    SimInputs = new OptionSimulationInputs();
                    OptionSimulationInputs inputs = _dictGroupWiseShocks[groupBasis];
                    SimInputs.ChangeDaysToExpiration = inputs.ChangeDaysToExpiration;
                    SimInputs.ChangeInterestRate = inputs.ChangeInterestRate;
                    SimInputs.ChangeVolatility = inputs.ChangeVolatility;
                    SimInputs.ChangeUnderlyingPrice = inputs.ChangeUnderlyingPrice;
                    SimInputs.UnderlyingPriceAsAbosluteValue = inputs.UnderlyingPriceAsAbosluteValue;
                }
                else
                {
                    SimInputs = new OptionSimulationInputs();
                }
            }
            else
            {
                SimInputs = new OptionSimulationInputs();
            }
            return SimInputs;
        }

        public double GetVolShockAdjFactor(int daysToExp)
        {
            double volAdjFactor = 1;
            try
            {
                if (_dtVolShockFactor != null)
                {
                    DataRow[] foundRows = _dtVolShockFactor.Select(string.Format("FromDaysToExp <= '{0}' AND ToDaysToExp >='{0}'", daysToExp));
                    if (foundRows.Length > 0)
                    {
                        foreach (DataRow dr in foundRows)
                        {
                            object[] items = dr.ItemArray;
                            if (items.Length > 2)
                            {
                                if (items[2] != null)
                                {
                                    volAdjFactor = double.Parse(items[2].ToString());
                                }
                                break;
                            }
                        }
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
            return volAdjFactor;
        }

        public bool CheckIFVolSkewReqValid()
        {
            foreach (OptionSimulationInputs inputs in _dictGroupWiseShocks.Values)
            {
                if (inputs.ChangeUnderlyingPrice != 1 || inputs.ChangeDaysToExpiration != 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
