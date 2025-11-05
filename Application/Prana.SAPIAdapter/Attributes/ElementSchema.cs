using System;

namespace Bloomberg.Library
{
    class ElementSchema : Attribute
    {
        protected string type;
        protected string name;
        public ElementSchema(string name, string type)
        {
            this.type = type;
            this.name = name;
        }
        public object Value
        {
            get
            {
                return name;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
    }
}
