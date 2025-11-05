using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Act40OrderGeneratorTool
{

    [Serializable]
    public class Preference
    {
        private const String PREF_FILE = "Rebalancer.prefrence";

        private String _startUpDestination;

        public String StartupDestination
        {
            get { return _startUpDestination; }
        }

        private String _startUpModel;

        public String StartUpModel
        {
            get { return _startUpModel; }
        }

        private Boolean _excludeNakedSecurities;

        public Boolean ExcludeNakedSecurities
        {
            get { return _excludeNakedSecurities; }
            set { _excludeNakedSecurities = value; }
        }

        private ModelPrefrence _modelPrefrence;

        public ModelPrefrence ModelPrefrence
        {
            get { return _modelPrefrence; }
            set { _modelPrefrence = value; }
        }

        private List<String> _excludedSymbolList;

        public List<String> ExcludedSymbols
        {
            get { return _excludedSymbolList; }
            set { _excludedSymbolList = value; }
        }

        private CalculationPreference _calculationPreference;

        public CalculationPreference CalculationPreference
        {
            get { return _calculationPreference; }
            set { _calculationPreference = value; }
        }




        #region SingiltonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static Preference _singiltonObject = null;

        /// <summary>
        /// private cunstructor
        /// </summary>
        private Preference()
        {
            _startUpModel = "";
            _startUpDestination = "";
            _excludeNakedSecurities = false;
            _modelPrefrence = ModelPrefrence.Account;
            _excludedSymbolList = new List<String>();
            _calculationPreference = CalculationPreference.Exposure;
        }

        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        internal static Preference GetInstance()
        {
            lock (_lock)
            {
                if (_singiltonObject == null)
                {
                    if (File.Exists(PREF_FILE))
                    {
                        try
                        {
                            Stream stream = File.Open(PREF_FILE, FileMode.Open);
                            BinaryFormatter bformatter = new BinaryFormatter();
                            _singiltonObject = (Preference)bformatter.Deserialize(stream);
                            stream.Close();
                        }
                        catch
                        {
                            _singiltonObject = new Preference();
                        }
                    }
                    else
                    {
                        _singiltonObject = new Preference();
                    }
                }
                return _singiltonObject;
            }
        }
        #endregion

        internal void Save(String destination, String model, Boolean excludenakedSec, ModelPrefrence modelPref, String excludedSymbols, CalculationPreference calcPref)
        {
            _startUpDestination = destination;
            _startUpModel = model;
            _excludeNakedSecurities = excludenakedSec;
            _modelPrefrence = modelPref;
            excludedSymbols = excludedSymbols.Trim(',');
            _calculationPreference = calcPref;
            if (String.IsNullOrWhiteSpace(excludedSymbols.Trim()))
                _excludedSymbolList = new List<String>();
            else
            {
                List<String> symbols = excludedSymbols.Split(',').ToList();
                _excludedSymbolList = symbols.Select(x => x.Trim()).ToList();
            }
            Stream stream = File.Open(PREF_FILE, FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();
            bformatter.Serialize(stream, this);
            stream.Close();
        }
    }
}
