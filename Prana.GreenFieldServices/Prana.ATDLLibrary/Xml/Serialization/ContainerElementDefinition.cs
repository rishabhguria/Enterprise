using System.Xml.Linq;

namespace Prana.ATDLLibrary.Xml.Serialization
{
    public class ContainerElementDefinition : ElementDefinition
    {
        public ElementDefinition ChildDefinition { get; private set; }

        public ContainerElementDefinition(XName elementName, ElementDefinition childDefinition)
            : base(elementName, null, null, null, null)
        {
            ChildDefinition = childDefinition;
        }
    }
}
