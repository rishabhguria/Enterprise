using System.Collections;

namespace Prana.BusinessObjects.LiveFeed
{
    public class Container : IEnumerable
    {
        protected string _contName;

        public string Name
        {
            get { return _contName; }
        }

        public Container(string name)
        {
            _contName = name;

        }
        //public Container(string name, MarketMakerCollection _mmidCollection)
        //{
        //    _contName = name;
        //}
        public virtual void ProcessMMID(MarketMaker mmid)
        {

        }

        protected ContainerCollection leafItems = new ContainerCollection();
        public ContainerCollection ChildContainers
        {
            get { return leafItems; }
        }

        #region IEnumerable Members

        public virtual IEnumerator GetEnumerator()
        {
            return null;
        }

        public virtual void ClearContainer()
        {

        }
        #endregion
    }
}
