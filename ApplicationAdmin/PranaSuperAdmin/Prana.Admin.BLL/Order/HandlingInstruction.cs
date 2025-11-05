namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for HandlingInstruction.
    /// </summary>
    public class HandlingInstruction
    {
        private int _handlingInstructionID = int.MinValue;
        private string _name = string.Empty;
        private string _tagValue = string.Empty;

        private int _companyID = int.MinValue;

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

        public int CompanyID
        {
            get
            {
                return _companyID;
            }

            set
            {
                _companyID = value;
            }
        }
    }
}