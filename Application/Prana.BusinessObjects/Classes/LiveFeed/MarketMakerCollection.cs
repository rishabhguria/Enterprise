using System.ComponentModel;

namespace Prana.BusinessObjects.LiveFeed
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public class MarketMakerCollection : AsyncBindingList<MarketMaker>
    {
        public void CustomListChanged()
        {
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
        }

    }
    //public class MarketMakerCollection: BindingList<MarketMaker>
    //{

    //    //PriceSizePriority sortByPriceSize = new PriceSizePriority ();
    //    public void CustomListChanged()
    //    {
    //        OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
    //    }

    //    protected override bool SupportsSearchingCore
    //    {
    //        get
    //        {
    //            return true;// base.SupportsSearchingCore;
    //        }
    //    }
    //    protected override void OnListChanged(ListChangedEventArgs e)
    //    {
    //        base.OnListChanged(e);
    //    }

    //    protected override bool  SupportsSortingCore
    //    {
    //        get
    //        {
    //            return true;
    //        }
    //    }

    //    public void Sort(IComparer<MarketMaker> sortCriteria)
    //    {
    //        List<MarketMaker> list = this.Items as List<MarketMaker>;
    //        list.Sort(sortCriteria);
    //    }

    //    public int Exists(MarketMaker mmid, IComparer<MarketMaker> searchCriteria)
    //    {
    //        List<MarketMaker> list = this.Items as List<MarketMaker>;
    //        return list.BinarySearch(mmid, searchCriteria);

    //    }

    //    protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
    //    {
    //        //List<MarketMaker> list = this.Items as List<MarketMaker>;
    //        //list.Sort(sortByPriceSize);

    //        base.ApplySortCore(prop, direction);
    //    }


    //    protected override int FindCore(PropertyDescriptor prop, object key)
    //    {
    //        //for (int i = 0; i < Count; i++)
    //        //{
    //        //    MarketMaker c = this[i];
    //        //    if (prop.GetValue (c) == key)
    //        //    {
    //        //        return i;
    //        //    }
    //        //}
    //        //return -1;
    //       return base.FindCore(prop, key);
    //    }
    //}
}
