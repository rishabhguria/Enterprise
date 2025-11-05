namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for ExecutionInstruction.
    /// </summary>
    public class ExecutionInstruction
    {
        int _executionInstructionsID = int.MinValue;
        string _executionInstructions = string.Empty;
        string _tagValue = string.Empty;

        int _cvAuecID = int.MinValue;

        public ExecutionInstruction()
        {
        }

        public ExecutionInstruction(string executionInstructions, string tagValue)
        {
            _executionInstructions = executionInstructions;
            _tagValue = tagValue;
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
