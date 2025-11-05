namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for ExecutionInstruction.
    /// </summary>
    public class ExecutionInstruction
    {
        private int _executionInstructionsID = int.MinValue;
        private string _executionInstructions = string.Empty;
        private string _tagValue = string.Empty;

        private int _companyID = int.MinValue;

        public ExecutionInstruction()
        {
        }

        public ExecutionInstruction(int executionInstructionsID, string executionInstructions, string tagValue)
        {
            _executionInstructionsID = executionInstructionsID;
            _executionInstructions = executionInstructions;
            _tagValue = tagValue;
        }

        public int ExecutionInstructionsID
        {
            get
            {
                return _executionInstructionsID;
            }

            set
            {
                _executionInstructionsID = value;
            }
        }

        public string ExecutionInstructions
        {
            get
            {
                return _executionInstructions;
            }

            set
            {
                _executionInstructions = value;
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