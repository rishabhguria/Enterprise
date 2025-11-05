using System.Collections.Generic;

namespace Prana.BusinessObjects.LiveFeed
{
    public class ContainerCollection
    {
        Dictionary<string, Container> _contCollection = new Dictionary<string, Container>();

        public bool Exists(string containerName)
        {
            return _contCollection.ContainsKey(containerName);
        }

        public Container this[string name]
        {
            get { return _contCollection[name] as Container; }
            set { _contCollection[name] = value; }
        }
    }
}
