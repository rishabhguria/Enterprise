using System;
namespace Nirvana.RuleEngine.Core
{
   public interface IGroup
    {
        int CollectionClass { get; set; }
        Facts Facts { get; set; }
        string Name { get; set; }
        int ItemClass { get; set; }
    }
}
