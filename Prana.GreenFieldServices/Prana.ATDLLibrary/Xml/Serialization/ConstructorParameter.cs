using System;

namespace Prana.ATDLLibrary.Xml.Serialization
{
    public enum SourceType
    {
        ElementAttribute,
        ParentObject,
        NamedPredecessor
    }

    public class ConstructorParameter
    {
        public Type Type { get; private set; }
        public SourceType SourceType { get; private set; }
        public string Source { get; private set; }

        public ConstructorParameter(Type type, SourceType sourceType, string source)
        {
            Type = type;
            SourceType = sourceType;
            Source = source;
        }
    }
}
