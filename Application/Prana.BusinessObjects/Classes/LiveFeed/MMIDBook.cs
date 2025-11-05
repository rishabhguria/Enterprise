using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections;
namespace Prana.BusinessObjects.LiveFeed
{
    public class MMIDBook
    {
        private ContainerCollection bookRoot;
        private IComparer mmidPriority;
        public event EventHandler<EventArgs<ContainerEventArgs>> AddedNewContainer;
        public MMIDBook()
        {
            bookRoot = new ContainerCollection();
        }
        public ContainerCollection Containers
        {
            get { return bookRoot; }
        }

        public IComparer MMIDPriority
        {
            get { return mmidPriority; }
            set { mmidPriority = value; }

        }

        public void AddContainer(string name, MarketMakerCollection mmidCollectionBid, MarketMakerCollection mmidCollectionAsk)
        {
            if (!bookRoot.Exists(name))
            {
                //Add a new container if does not exist
                bookRoot[name] = new Container(name);

            }
            Container currentContainer = bookRoot[name];

            //Create new instance of a Bid type LeafContainer
            LeafContainer bidContainer = new LeafContainer("BID", mmidCollectionBid);
            //Create new instance of a Ask type LeafContainer
            LeafContainer askContainer = new LeafContainer("ASK", mmidCollectionAsk);
            //Add them as child containers.
            currentContainer.ChildContainers["BID"] = bidContainer;
            currentContainer.ChildContainers["ASK"] = askContainer;
        }

        private Container ProcessContainers(ContainerCollection containerCollection, string name, MarketMaker mmid)
        {
            if (!containerCollection.Exists(name))
            {
                throw new Exception("Parent Container for this symbol does not exist " + name);
                //containerCollection[name] = new Container(name);
            }

            Container currentContainer = containerCollection[name];
            //This will work only when the Container is of type LeafContainer
            currentContainer.ProcessMMID(mmid);

            return currentContainer;
        }


        public void Process(MarketMaker mmid)
        {
            try
            {
                Container container = ProcessContainers(bookRoot, mmid.Symbol, mmid);

                if (container.ChildContainers.Exists(mmid.BidAsk) == false)
                {
                    LeafContainer bidContainer = new LeafContainer("BID");
                    LeafContainer askContainer = new LeafContainer("ASK");
                    container.ChildContainers["BID"] = bidContainer;
                    container.ChildContainers["ASK"] = askContainer;
                    ContainerEventArgs e = new ContainerEventArgs(container);
                    if (AddedNewContainer != null)
                    {
                        AddedNewContainer(null, new EventArgs<ContainerEventArgs>(e));
                    }
                }

                LeafContainer leafContainer = container.ChildContainers[mmid.BidAsk.ToString()] as LeafContainer;

                leafContainer.ProcessMMID(mmid);
            }

            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}


