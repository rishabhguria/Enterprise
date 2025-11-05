using System;
using System.Xml.Linq;

namespace Prana.ATDLLibrary.Xml.Serialization
{
    public class ElementDefinition
    {
        public XName ElementName { get; set; }
        public Type TargetType { get; private set; }
        public ElementAttribute[] Attributes { get; private set; }
        public ConstructorParameter[] ConstructorParameters { get; private set; }
        public ChildElementDefinition[] ChildElements { get; private set; }

        public ElementDefinition(XName elementName, Type targetType, ElementAttribute[] attributes)
            : this(elementName, targetType, null, attributes, new ChildElementDefinition[] { })
        {
        }

        public ElementDefinition(XName elementName, Type targetType, ElementAttribute[] attributes, ChildElementDefinition child)
            : this(elementName, targetType, null, attributes, new ChildElementDefinition[] { child })
        {
        }

        public ElementDefinition(XName elementName, Type targetType, ElementAttribute[] attributes, ChildElementDefinition[] children)
            : this(elementName, targetType, null, attributes, children)
        {
        }

        public ElementDefinition(XName elementName, Type targetType, ConstructorParameter[] constructorParameters,
            ElementAttribute[] attributes, ChildElementDefinition[] children)
        {
            ElementName = elementName;
            TargetType = targetType;
            ConstructorParameters = constructorParameters;
            Attributes = attributes;
            ChildElements = children;
        }
    }
}