using System;
namespace Nirvana.RuleEngine.Core
{
    public interface IRule
    {
        Actions Actions { get; set; }
        string Condition { get; set; }
        string Description { get; set; }
        string Name { get; set; }
    }
}
