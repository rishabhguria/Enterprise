using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class SubscriberViewInputs
    {
        #region private variables

        private Dictionary<string, OptionSimulationInputs> _dictOptSimInputs = new Dictionary<string, OptionSimulationInputs>();
        private Dictionary<string, List<string>> _dictSymbolOptions = new Dictionary<string, List<string>>();
        private Dictionary<string, List<VolSkewObject>> _dictVolSkewReq = new Dictionary<string, List<VolSkewObject>>();
        private int _hashCode;
        private string _iD;
        private bool _isStressTestRequest = false;
        private bool _isVolSkewRequest = false;
        private List<string> _listNonOptions = new List<string>();
        private List<StepAnalysisResponse> _listStepAnalysisInputs = new List<StepAnalysisResponse>();
        private Dictionary<string, Dictionary<string, StepAnalysisResponse>> _stepAnalResDict = new Dictionary<string, Dictionary<string, StepAnalysisResponse>>();
        private bool _stepAnalysisUsingStressData = false;
        private double _toleranceValue;

        #endregion private variables

        #region properties

        public Dictionary<string, OptionSimulationInputs> DictOptSimInputs
        {
            get { return _dictOptSimInputs; }
            set { _dictOptSimInputs = value; }
        }

        public Dictionary<string, List<string>> DictSymbolOptions
        {
            get { return _dictSymbolOptions; }
            set { _dictSymbolOptions = value; }
        }

        public Dictionary<string, List<VolSkewObject>> DictVolSkewReq
        {
            get { return _dictVolSkewReq; }
            set { _dictVolSkewReq = value; }
        }

        public int HashCode
        {
            get { return _hashCode; }
            set { _hashCode = value; }
        }

        public string ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        public bool IsStressTestRequest
        {
            get { return _isStressTestRequest; }
            set { _isStressTestRequest = value; }
        }

        public bool IsVolSkewRequest
        {
            get { return _isVolSkewRequest; }
            set { _isVolSkewRequest = value; }
        }

        public List<string> ListNonOptions
        {
            get { return _listNonOptions; }
            set { _listNonOptions = value; }
        }

        public List<StepAnalysisResponse> ListStepAnalysisInputs
        {
            get { return _listStepAnalysisInputs; }
            set { _listStepAnalysisInputs = value; }
        }

        public Dictionary<string, Dictionary<string, StepAnalysisResponse>> StepAnalResDict
        {
            get { return _stepAnalResDict; }
            set { _stepAnalResDict = value; }
        }

        public bool StepAnalysisUsingStressData
        {
            get { return _stepAnalysisUsingStressData; }
            set { _stepAnalysisUsingStressData = value; }
        }

        public double ToleranceValue
        {
            get { return _toleranceValue; }
            set { _toleranceValue = value; }
        }

        #endregion properties
    }
}