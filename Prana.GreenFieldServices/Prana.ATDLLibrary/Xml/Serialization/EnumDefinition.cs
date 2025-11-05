using System;
using System.Collections.Generic;

namespace Prana.ATDLLibrary.Xml.Serialization
{
    public class EnumDefinition
    {
        public Type EnumType { get; private set; }
        public Dictionary<string, Enum> TextValues { get; private set; }

        public EnumDefinition(Type enumType, Dictionary<string, Enum> textValues)
        {
            EnumType = enumType;
            TextValues = textValues;
        }
    }
}