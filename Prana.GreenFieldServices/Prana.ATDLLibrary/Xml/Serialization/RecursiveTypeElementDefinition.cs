using System;
using System.Xml.Linq;

namespace Prana.ATDLLibrary.Xml.Serialization
{
    public class RecursiveTypeElementDefinition : ElementDefinition
    {
        public RecursiveTypeElementDefinition()
            : base(null, null, null)
        {
        }

        public new XName ElementName
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public new Type TargetType
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public new ElementAttribute[] Attributes
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public new ConstructorParameter[] ConstructorParameters
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public new ChildElementDefinition[] ChildElements
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }
    }
}

