using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class CounterParty
    {
        int _counterPartyID = int.MinValue;
        string _name = string.Empty;
        public CounterParty(int id, string name)
        {
            _counterPartyID = id;
            _name = name;
        }
        public int CounterPartyID
        {
            get
            {
                return _counterPartyID;
            }

            set
            {
                _counterPartyID = value;
            }
        }


        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public bool? IsAlgoBroker { get; set; }

        public bool IsOTDorEMS { get; set; }

    }
}
