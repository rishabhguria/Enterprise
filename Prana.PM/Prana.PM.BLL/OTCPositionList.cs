using Csla;
using Prana.BusinessObjects;
using System;

namespace Prana.PM.BLL
{
    [Serializable()]
    public class OTCPositionList : BusinessListBase<OTCPositionList, OTCPosition>
    {


        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            //base.ClearItems();
            base.Clear();
        }


    }
}
