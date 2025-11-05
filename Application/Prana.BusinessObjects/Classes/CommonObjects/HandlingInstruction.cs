namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for HandlingInstruction.
    /// </summary>
    public class HandlingInstruction
    {
        int _handlingInstructionID = int.MinValue;
        string _name = string.Empty;
        string _tagValue = string.Empty;

        int _cvAuecID = int.MinValue;

        public HandlingInstruction()
        {
        }

        public HandlingInstruction(int handlingInstructionID, string name, string tagValue)
        {
            _handlingInstructionID = handlingInstructionID;
            _name = name;
            _tagValue = tagValue;
        }

        public int HandlingInstructionID
        {
            get
            {
                return _handlingInstructionID;
            }

            set
            {
                _handlingInstructionID = value;
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

        public string TagValue
        {
            get
            {
                return _tagValue;
            }

            set
            {
                _tagValue = value;
            }
        }

        public int CVAUECID
        {
            get
            {
                return _cvAuecID;
            }

            set
            {
                _cvAuecID = value;
            }
        }

    }
}
