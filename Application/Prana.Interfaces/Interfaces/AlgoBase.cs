//using System;
//using System.Collections.Generic;
//using System.Text;
//using Prana.BusinessObjects.PositionManagement;
//using Csla;
//using System.ComponentModel;

//namespace Prana.Interfaces
//{
//  public class  AlgoBase
//    {
//      abstract void SortBuyList(List<AllocatedTrade> taxlots);
//      abstract void SortSellList(List<AllocatedTrade> taxlots);
//      public virtual List<AllocatedTrade> GetTaxLotsByDate(DateTime date)
//      {
//          List<AllocatedTrade> taxlotList = new List<AllocatedTrade>();

//          foreach (AllocatedTrade sortedItem in listOftaxlots)
//          {
//              if (sortedItem.TradeDate.Date <= date)
//              {
//                  taxlotList.Add(sortedItem);
//              }
//              else
//              {
//                  break;
//              }
//          }
//          return taxlotList;
//      }

//    }
//}
