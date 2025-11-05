using System;

namespace Prana.BusinessObjects.LiveFeed
{
    public class ContainerEventArgs : EventArgs
    {
        Prana.BusinessObjects.LiveFeed.Container container;

        public ContainerEventArgs(Prana.BusinessObjects.LiveFeed.Container cont)
        {
            container = cont;
        }

        public Prana.BusinessObjects.LiveFeed.Container Container
        {
            get { return container; }
        }
    }

    //public delegate void ContainerEventHandler(object sender, ContainerEventArgs e);
}
