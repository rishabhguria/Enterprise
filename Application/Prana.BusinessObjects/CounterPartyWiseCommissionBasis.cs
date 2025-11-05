namespace Prana.BusinessObjects
{
    public class CounterPartyWiseCommissionBasis
    {

        private SerializableDictionary<int, int> _dictCounterPartyWiseCommissionBasis = new SerializableDictionary<int, int>();
        public SerializableDictionary<int, int> DictCounterPartyWiseCommissionBasis
        {
            get { return _dictCounterPartyWiseCommissionBasis; }
            set { _dictCounterPartyWiseCommissionBasis = value; }
        }

        private SerializableDictionary<int, int> _dictCounterPartyWiseSoftCommissionBasis = new SerializableDictionary<int, int>();
        public SerializableDictionary<int, int> DictCounterPartyWiseSoftCommissionBasis
        {
            get { return _dictCounterPartyWiseSoftCommissionBasis; }
            set { _dictCounterPartyWiseSoftCommissionBasis = value; }
        }

        private SerializableDictionary<int, int> _dictCounterPartyWiseExecutionInstructions = new SerializableDictionary<int, int>();
        public SerializableDictionary<int, int> DictCounterPartyWiseExecutionInstructions
        {
            get { return _dictCounterPartyWiseExecutionInstructions; }
            set { _dictCounterPartyWiseExecutionInstructions = value; }
        }

        private SerializableDictionary<int, int> _dictCounterPartyWiseVenue = new SerializableDictionary<int, int>();
        public SerializableDictionary<int, int> DictCounterPartyWiseExecutionVenue
        {
            get { return _dictCounterPartyWiseVenue; }
            set { _dictCounterPartyWiseVenue = value; }
        }

        private SerializableDictionary<int, int> _dictCounterPartyWiseAlgoType = new SerializableDictionary<int, int>();
        public SerializableDictionary<int, int> DictCounterPartyWiseExecutionAlgoType
        {
            get { return _dictCounterPartyWiseAlgoType; }
            set { _dictCounterPartyWiseAlgoType = value; }
        }

        private int _UserID = int.MinValue;
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
    }
}
