using Prana.BusinessObjects.PositionManagement;
using System;
using System.Collections.Generic;

namespace Prana.Interfaces
{
    /// <summary>
    /// This interface provides the bridge between the consumers of position management and the pm itself.
    //TODO : Eventually this interface needs to be used in PM functions also. NetPositionList class is derived 
    // from CSLA businesslistbase hence we need to take the reference from csla also, Need to remove this 
    /// </summary>
    public interface IPMInteraction
    {
        NetPositionList GetDailyPositions(DateTime _date, int companyID, int userID);

        OpenTaxLotList GetCurrentOpenAllocatedData(int companyID);
        List<string> GetSymbolListForOpenPositionsAndTrades(int companyID);
    }
}
