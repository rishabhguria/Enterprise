using System;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for HandlingInstruction.
    /// </summary>
    [Serializable]
    public class Instruction
    {
        int _instructionID = int.MinValue;
        string _name = string.Empty;

        public int InstructionID
        {
            get { return _instructionID; }
            set { _instructionID = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
