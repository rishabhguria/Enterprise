using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prana.ClientCommon
{
    public static class ClosingClientSideMapper
    {

        private static ClosingData _closedData = new ClosingData();
        private static ProxyBase<IClosingServices> _closingServices = null;
        public static string Symbol = string.Empty;
        public static string Side = string.Empty;
        public static string GroupId = string.Empty;
        public static int AccountId = 0;

        public static AllocationGroup AllocationGroup = null;

        public static List<string> PositionalSidesForSell
        {
            get
            {
                List<string> sides = new List<string>();
                //TaxlotGridColumns = new List<string>();

                sides.Add(FIXConstants.SIDE_Buy);
                sides.Add(FIXConstants.SIDE_Buy_Open);
                sides.Add(FIXConstants.SIDE_Buy_Closed);
                //sides.Add(FIXConstants.SIDE_Buy);
                //sides.Add(FIXConstants.SIDE_Buy);

                return sides;
            }
        }

        public static List<string> PositionalSidesForBuyToClose
        {
            get
            {
                List<string> sides = new List<string>();
                //TaxlotGridColumns = new List<string>();

                sides.Add(FIXConstants.SIDE_SellShort);
                sides.Add(FIXConstants.SIDE_Sell_Open);
                sides.Add(FIXConstants.SIDE_Sell);
                sides.Add(FIXConstants.SIDE_Sell_Closed);
                //sides.Add(FIXConstants.SIDE_Buy_Open);
                //sides.Add(FIXConstants.SIDE_Buy_Closed);
                //sides.Add(FIXConstants.SIDE_Buy);
                //sides.Add(FIXConstants.SIDE_Buy);

                return sides;
            }
        }


        public static ClosingData ClosedData
        {
            get { return _closedData; }
            set { _closedData = value; }
        }


        private static GenericBindingList<TaxLot> _openTaxlots = new GenericBindingList<TaxLot>();

        public static GenericBindingList<TaxLot> OpenTaxlots
        {
            get { return _openTaxlots; }
            set {; }
        }

        private static GenericBindingList<TaxLot> _openTaxlotsToPopulate = new GenericBindingList<TaxLot>();

        public static GenericBindingList<TaxLot> OpenTaxlotsToPopulate
        {
            get { return _openTaxlotsToPopulate; }
            set {; }
        }


        private static GenericBindingList<Position> _netpositions = new GenericBindingList<Position>();

        public static GenericBindingList<Position> Netpositions
        {
            get { return _netpositions; }
            set {; }
        }
        public static List<TaxLot> GetTaxlots(List<AllocationGroup> allocationGroupList, List<TaxLot> Taxlots, bool isCopyTradeAttrbsPrefUsed)
        {
            List<TaxLot> closeTrades = new List<TaxLot>();

            try
            {
                for (int i = 0; i < Taxlots.Count; i++)
                {
                    if (i < allocationGroupList.Count)
                    {
                        TaxLot closeTrade = (TaxLot)Taxlots[i].Clone();
                        if ((Taxlots[i].AssetID == (int)AssetCategory.FX || Taxlots[i].AssetID == (int)AssetCategory.FXForward))
                            closeTrade.AvgPrice = closeTrade.AvgPrice = closeTrade.CashSettledPrice;
                        else
                            closeTrade.AvgPrice = Convert.ToDouble(allocationGroupList[i].AvgPrice);
                        closeTrade.OpenTotalCommissionandFees = allocationGroupList[i].TotalCommissionandFees;
                        closeTrade.ClosedTotalCommissionandFees = 0.0;
                        closeTrade.GroupID = allocationGroupList[i].GroupID;
                        closeTrade.TaxLotID = allocationGroupList[i].TaxLots[0].TaxLotID;
                        closeTrade.TaxLotQty = allocationGroupList[i].CumQty;
                        // closeTrade.TaxLotClosingId = allocationGroupList[0].TaxLotClosingId;
                        closeTrade.AUECLocalDate = allocationGroupList[i].AllocationDate;
                        closeTrade.OriginalPurchaseDate = allocationGroupList[i].AllocationDate;
                        closeTrade.ProcessDate = allocationGroupList[i].AllocationDate;
                        closeTrade.OrderSideTagValue = allocationGroupList[i].OrderSideTagValue;
                        closeTrade.OrderSide = allocationGroupList[i].OrderSide;
                        if (!isCopyTradeAttrbsPrefUsed)
                        {
                            closeTrade.TradeAttribute1 = string.Empty;
                            closeTrade.TradeAttribute2 = string.Empty;
                            closeTrade.TradeAttribute3 = string.Empty;
                            closeTrade.TradeAttribute4 = string.Empty;
                            closeTrade.TradeAttribute5 = string.Empty;
                            closeTrade.TradeAttribute6 = string.Empty;
                            closeTrade.ResetTradeAttributes();
                        }
                        closeTrades.Add(closeTrade);
                    }
                }
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
            return closeTrades;
        }

        public static List<string> GetSidesForClosingTaxlots()
        {
            List<string> list = new List<string>();
            try
            {
                if (Side.Equals(FIXConstants.SIDE_Sell) || Side.Equals(FIXConstants.SIDE_Sell_Closed))
                    return PositionalSidesForSell;
                else if (Side.Equals(FIXConstants.SIDE_Buy_Closed))
                    return PositionalSidesForBuyToClose;
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return list;
        }
        private static void CreateOpenTaxlots(ClosingData closingData)
        {
            try
            {
                // _openTaxlotsToPopulate collection is used to populate data on new closing UI
                _openTaxlotsToPopulate.Clear();
                _openTaxlots.Clear();
                UpdateRepository(closingData);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static void ClearNetPositions()
        {
            try
            {
                lock (_netpositions)
                {
                    _netpositions.Clear();
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static void CreateNetPositions(ClosingData closingData)
        {
            try
            {
                ClearNetPositions();
                UpdateRepository(closingData);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static async Task<ClosingData> GetAllClosingData(DateTime FromDate, DateTime Todate, bool IsCurrentDateClosing, ProxyBase<IClosingServices> _closingServices, string CommaSeparatedAccountIds, string commaSeparatedAssetIDs, string commaSeparatedSymbols, string CustomConditions)
        {
            ClosingData data = new ClosingData();
            try
            {
                data = await System.Threading.Tasks.Task.Run(() => _closingServices.InnerChannel.GetAllClosingData(FromDate, Todate, IsCurrentDateClosing, CommaSeparatedAccountIds, commaSeparatedAssetIDs, commaSeparatedSymbols, CustomConditions));


                //CreateOpenTaxlots(closingdata);

                //CreateNetPositions(closingdata);

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return data;
            //ClearCache();
        }

        public static ClosingData GetClosingDataForASymbol(string symbol, ProxyBase<IClosingServices> _closingServices, string CommaSeparatedAccountIds, string orderSideTagValue, string groupID)
        {
            ClosingData data = new ClosingData();
            try
            {
                data = _closingServices.InnerChannel.GetClosingDataForASymbol(symbol, CommaSeparatedAccountIds, orderSideTagValue, groupID);

                //CreateOpenTaxlots(closingdata);

                //CreateNetPositions(closingdata);

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return data;
            //ClearCache();
        }

        public static void CreateRepository(ClosingData closingdata)
        {
            try
            {
                _closedData = closingdata;

                CreateOpenTaxlots(closingdata);

                CreateNetPositions(closingdata);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static void UpdateCorrespondingPositions(TaxLot taxlot)
        {
            lock (_netpositions)
            {
                foreach (Position position in _netpositions)
                {

                    if (!position.ClosingAlgo.Equals((int)PostTradeEnums.CloseTradeAlogrithm.ACA))
                    {
                        if (position.ID.Equals(taxlot.TaxLotID) || position.ClosingID.Equals(taxlot.TaxLotID))
                        {
                            position.Update(taxlot);
                            Prana.BusinessLogic.Calculations.SetAveragePriceRealizedPNL(position);
                            position.PropertyHasChanged();
                        }
                    }
                }
            }
        }

        public static double GetSymbolAccountPositionForGivenDate(string symbol, int accountID, DateTime date)
        {

            double currentPosition = 0;
            try
            {

                List<TaxLot> listAccountTaxlots = new List<TaxLot>(_openTaxlots);
                listAccountTaxlots.RemoveAll(delegate (TaxLot obj)
                {
                    if (obj.Symbol.Equals(symbol) && obj.Level1ID.Equals(accountID) && obj.ProcessDate.Date <= date.Date)
                    {
                        return false;
                    }
                    return true;
                });


                foreach (TaxLot taxlot in listAccountTaxlots)
                {
                    currentPosition += (taxlot.TaxLotQty * taxlot.SideMultiplier);
                }


                //if (currentPosition >= 0)
                //{
                //    return PositionType.Long;
                //}

                //return PositionType.Short;



            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return currentPosition;
        }

        /// <summary>
        /// Gets the symbol account position for given date new.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="accountID">The account identifier.</param>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static Dictionary<PositionTag, double> GetSymbolAccountPositionForGivenDateNew(string symbol, int accountID, DateTime date)
        {
            Dictionary<PositionTag, double> dictPositionTagPos = new Dictionary<PositionTag, double>();
            try
            {
                List<TaxLot> listAccountTaxlots = new List<TaxLot>(_openTaxlots);
                listAccountTaxlots.RemoveAll(delegate (TaxLot obj)
                {
                    if (obj.Symbol.Equals(symbol) && obj.Level1ID.Equals(accountID) && obj.ProcessDate.Date <= date.Date)
                    {
                        return false;
                    }
                    return true;
                });


                foreach (TaxLot taxlot in listAccountTaxlots)
                {
                    if (taxlot.LongOrShort.Equals(PositionTag.Short))
                    {
                        if (dictPositionTagPos.ContainsKey(PositionTag.Short))
                        {
                            dictPositionTagPos[PositionTag.Short] += (taxlot.TaxLotQty * taxlot.SideMultiplier);
                        }
                        else
                        {
                            dictPositionTagPos.Add(PositionTag.Short, (taxlot.TaxLotQty * taxlot.SideMultiplier));
                        }
                    }
                    else
                    {
                        if (dictPositionTagPos.ContainsKey(PositionTag.Long))
                        {
                            dictPositionTagPos[PositionTag.Long] += (taxlot.TaxLotQty * taxlot.SideMultiplier);
                        }
                        else
                        {
                            dictPositionTagPos.Add(PositionTag.Long, (taxlot.TaxLotQty * taxlot.SideMultiplier));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return dictPositionTagPos;
        }


        public static void UpdateRepository(ClosingData closedData)
        {
            try
            {
                lock (_netpositions)
                {
                    if (closedData.UnSavedTaxlots.Count > 0)
                    {
                        UpdateOpenTaxlots(closedData.UnSavedTaxlots);
                        UpdateTaxlotToPopulate(closedData.UnSavedTaxlots);
                    }
                    else
                    {
                        UpdateOpenTaxlots(closedData.Taxlots);
                        UpdateTaxlotToPopulate(closedData.Taxlots);
                    }

                    if (closedData.ClosedPositions != null && closedData.ClosedPositions.Count > 0)
                    {
                        UpdatePositions(closedData.ClosedPositions);
                    }
                    if (closedData.PositionsToUnwind != null && closedData.PositionsToUnwind.Length > 0)
                    {
                        UnwindPositions(closedData.PositionsToUnwind);
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

        }




        public static void UpdateRepositoryWithSecmasterData(SecMasterbaseList secMasterUpdatedList)
        {
            try
            {

                //Narendra Kumar Jangir 2012 Nov 20
                //RaiseListChangedEvents property Gets or sets a value indicating whether adding or removing items within the list raises ListChanged events.
                //set the RaiseListChangedEvents so that ListChanged events does not raise and update binded source in the finally block 
                //http://stackoverflow.com/questions/1236983/what-causes-a-listchangedtype-itemmoved-listchange-event-in-a-bindinglistt
                _openTaxlots.RaiseListChangedEvents = false;
                List<string> listUpdatedSymbols = new List<string>();
                foreach (SecMasterBaseObj obj in secMasterUpdatedList)
                {
                    listUpdatedSymbols.Add(obj.TickerSymbol);
                }
                List<TaxLot> taxlotstoUpdate = _openTaxlots.GetList().FindAll(delegate (TaxLot obj)
                 {
                     if (listUpdatedSymbols.Contains(obj.Symbol))
                     {
                         return true;
                     }
                     return false;
                 });

                foreach (TaxLot taxlotToUpdate in taxlotstoUpdate)
                {
                    foreach (SecMasterBaseObj secMasterObject in secMasterUpdatedList)
                    {
                        if (secMasterObject.TickerSymbol == taxlotToUpdate.Symbol)
                        {
                            taxlotToUpdate.CopyBasicDetails(secMasterObject);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            finally
            {
                //Narendra Kumar Jangir 2012 Nov 20
                //RaiseListChangedEvents property Gets or sets a value indicating whether adding or removing items within the list raises ListChanged events.
                _openTaxlots.RaiseListChangedEvents = true;
                //When the BindingList is bound to Windows Forms controls, the ResetBindings method causes a refresh of all controls bound to the list.
                _openTaxlots.ResetBindings();


            }
        }

        private static void UnwindPositions(string list)
        {
            try
            {
                //Narendra Kumar Jangir 2012 Nov 20
                //RaiseListChangedEvents property Gets or sets a value indicating whether adding or removing items within the list raises ListChanged events.
                //set the RaiseListChangedEvents so that ListChanged events does not raise and update binded source in the finally block 
                //http://stackoverflow.com/questions/1236983/what-causes-a-listchangedtype-itemmoved-listchange-event-in-a-bindinglistt
                //_netpositions.RaiseListChangedEvents = false;

                string[] idsToundwind = list.Split(',');
                for (int i = 0; i < idsToundwind.Length; i++)
                {
                    string ClosingID = idsToundwind[i].ToString();
                    _netpositions.Remove(ClosingID);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            finally
            {
                //Narendra Kumar Jangir 2012 Nov 20
                //RaiseListChangedEvents property Gets or sets a value indicating whether adding or removing items within the list raises ListChanged events.
                //     _netpositions.RaiseListChangedEvents = true;
                //  _netpositions.ResetBindings();
            }
        }


        private static void UpdateOpenTaxlots(List<TaxLot> openTaxlots)
        {

            //Narendra Kumar Jangir 2012 Nov 20
            //RaiseListChangedEvents property Gets or sets a value indicating whether adding or removing items within the list raises ListChanged events.
            //set the RaiseListChangedEvents so that ListChanged events does not raise and update binded source in the finally block 
            //http://stackoverflow.com/questions/1236983/what-causes-a-listchangedtype-itemmoved-listchange-event-in-a-bindinglistt
            _openTaxlots.RaiseListChangedEvents = false;
            try
            {
                foreach (TaxLot taxlot in openTaxlots)
                {
                    //Taxlot Taxlot = GetUIObject(taxlot);

                    if (_openTaxlots.GetItem(taxlot.TaxLotID) == null)
                    {
                        //if (taxlot.TaxLotQty >= 0)
                        //    _openTaxlots.Add(taxlot);
                        if (taxlot.ClosingStatus != ClosingStatus.Closed)
                        {
                            _openTaxlots.Add(taxlot);

                        }
                    }
                    else
                    {
                        //if (taxlot.TaxLotQty != 0)
                        //{
                        //    _openTaxlots.UpdateItem(taxlot);

                        //}
                        if (taxlot.ClosingStatus != ClosingStatus.Closed)
                        {
                            _openTaxlots.UpdateItem(taxlot);

                        }
                        else if (taxlot.ClosingStatus == ClosingStatus.Closed)
                        {
                            _openTaxlots.Remove(taxlot);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                //Narendra Kumar Jangir 2012 Nov 20
                //RaiseListChangedEvents property Gets or sets a value indicating whether adding or removing items within the list raises ListChanged events.
                _openTaxlots.RaiseListChangedEvents = true;
                //When the BindingList is bound to Windows Forms controls, the ResetBindings method causes a refresh of all controls bound to the list.
                _openTaxlots.ResetBindings();
            }
        }

        public static bool UpdateTaxLotToPopulateFromAllocation(List<TaxLot> taxlotsToPopulate)
        {
            bool isTaxlotsUpdated = false;
            try
            {
                foreach (TaxLot taxLot in taxlotsToPopulate)
                {
                    //update data for symbol and side on close order UI
                    if (GetSidesForClosingTaxlots().Contains(taxLot.OrderSideTagValue) && Symbol.Equals(taxLot.Symbol))
                    {
                        if (taxLot.TaxLotState == ApplicationConstants.TaxLotState.New && taxLot.Level1ID != 0)
                        {
                            ClosingClientSideMapper.OpenTaxlotsToPopulate.Add(taxLot);
                            isTaxlotsUpdated = true;
                        }
                        if (taxLot.TaxLotState == ApplicationConstants.TaxLotState.Deleted)
                        {
                            ClosingClientSideMapper.OpenTaxlotsToPopulate.Remove(taxLot);
                            isTaxlotsUpdated = true;
                        }
                        if (taxLot.TaxLotState == ApplicationConstants.TaxLotState.Updated)
                        {
                            ClosingClientSideMapper.OpenTaxlotsToPopulate.Update(taxLot);
                            isTaxlotsUpdated = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return isTaxlotsUpdated;
        }

        private static void UpdateTaxlotToPopulate(List<TaxLot> openTaxlots)
        {
            try
            {
                //Narendra Kumar Jangir 2013 June 4
                //RaiseListChangedEvents property Gets or sets a value indicating whether adding or removing items within the list raises ListChanged events.
                //set the RaiseListChangedEvents so that ListChanged events does not raise and update binded source in the finally block 
                //http://stackoverflow.com/questions/1236983/what-causes-a-listchangedtype-itemmoved-listchange-event-in-a-bindinglistt
                _openTaxlotsToPopulate.RaiseListChangedEvents = false;
                foreach (TaxLot taxlot in openTaxlots)
                {
                    if (_openTaxlotsToPopulate.GetItem(taxlot.TaxLotID) == null)
                    {
                        if (taxlot.ClosingStatus != ClosingStatus.Closed && GetSidesForClosingTaxlots().Contains(taxlot.OrderSideTagValue) && Symbol.Equals(taxlot.Symbol))
                        {
                            _openTaxlotsToPopulate.Add(taxlot);
                        }
                    }
                    else
                    {
                        if (taxlot.ClosingStatus != ClosingStatus.Closed)
                        {
                            _openTaxlotsToPopulate.UpdateItem(taxlot);

                        }
                        else if (taxlot.ClosingStatus == ClosingStatus.Closed)
                        {
                            _openTaxlotsToPopulate.Remove(taxlot);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                //Narendra Kumar Jangir 2012 Nov 20
                //RaiseListChangedEvents property Gets or sets a value indicating whether adding or removing items within the list raises ListChanged events.
                _openTaxlotsToPopulate.RaiseListChangedEvents = true;
                //When the BindingList is bound to Windows Forms controls, the ResetBindings method causes a refresh of all controls bound to the list.
                _openTaxlotsToPopulate.ResetBindings();
            }
        }


        private static void UpdatePositions(List<Position> netpositions)
        {
            try
            {

                //Narendra Kumar Jangir 2012 Nov 20
                //RaiseListChangedEvents property Gets or sets a value indicating whether adding or removing items within the list raises ListChanged events.
                //set the RaiseListChangedEvents so that ListChanged events does not raise and update binded source in the finally block 
                //http://stackoverflow.com/questions/1236983/what-causes-a-listchangedtype-itemmoved-listchange-event-in-a-bindinglistt
                // _netpositions.RaiseListChangedEvents = false;

                foreach (Position position in netpositions)
                {

                    if (_netpositions.GetItem(position.TaxLotClosingId) == null)
                    {
                        _netpositions.Add(position);
                    }
                    else if (position.IsUnwind)
                    {
                        _netpositions.Remove(position);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            finally
            {
                //Narendra Kumar Jangir 2012 Nov 20
                //RaiseListChangedEvents property Gets or sets a value indicating whether adding or removing items within the list raises ListChanged events.
                //_netpositions.RaiseListChangedEvents = true;
                // _netpositions.ResetBindings();
            }
        }




        //public static void UpdateCache(ClosingData closedData)
        //{
        //    foreach (TaxLot TaxLot in closedData.Taxlots)
        //    {
        //        if (!_closedData.UpdatedTaxlots.ContainsKey(TaxLot.TaxLotID))
        //        {
        //            _closedData.UpdatedTaxlots.Add(TaxLot.TaxLotID, TaxLot);
        //            _closedData.Taxlots.Add(TaxLot);
        //        }
        //        else
        //        {
        //            TaxLot OldTaxLot = _closedData.UpdatedTaxlots[TaxLot.TaxLotID];
        //            _closedData.Taxlots.Remove(OldTaxLot);
        //            _closedData.UpdatedTaxlots[TaxLot.TaxLotID] = TaxLot;
        //            _closedData.Taxlots.Add(TaxLot);
        //        }
        //    }

        //    foreach (TaxLot taxlot in closedData.UnSavedTaxlots)
        //    {
        //        _closedData.UnSavedTaxlots.Add(taxlot);
        //    }

        //    foreach (Position position in closedData.ClosedPositions)
        //    {
        //        _closedData.ClosedPositions.Add(position);
        //    }
        //}

        public static void ClearRepository()
        {
            _closedData = new ClosingData();
            _openTaxlots.Clear();
            ClearNetPositions();
        }

        public static bool IsDataAvailabletoClose()
        {
            if (_openTaxlots.Count == 0)
                return false;
            else
                return true;
        }

        public static bool IsUnsavedData()
        {
            if (_closedData.UnSavedTaxlots.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// refresh closing preferences so that closing algorithms and secondary sort can be updated in server closing cache
        /// </summary>
        public static void RefreshClosingPreferences()
        {
            try
            {
                //create closing services proxy if it is null
                CreateClosingServicesProxy();
                _closingServices.InnerChannel.UpdatePreferencesFromDB();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// create closing services proxy if it is null
        /// </summary>
        private static void CreateClosingServicesProxy()
        {
            try
            {
                if (_closingServices == null)
                {
                    _closingServices = new ProxyBase<IClosingServices>("TradeClosingServiceEndpointAddress");
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

    }
}
