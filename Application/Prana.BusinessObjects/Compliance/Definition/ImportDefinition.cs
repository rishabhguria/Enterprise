using System;

namespace Prana.BusinessObjects.Compliance.Definition
{
    /// <summary>
    /// Definition for rule to be imported.
    /// </summary>
    public class ImportDefinition : RuleBase
    {
        public String DirectoryPath { get; set; }
        public String OldRuleName { get; set; }
        public String ClientName { get; set; }

        public override RuleBase DeepClone()
        {
            throw new NotImplementedException();
        }
        public ImportDefinition()
            : base()
        { }
    }
}
