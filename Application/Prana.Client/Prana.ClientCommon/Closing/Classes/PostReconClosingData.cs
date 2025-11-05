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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.ClientCommon
{
    public static class PostReconClosingData
    {
        public static string Symbol = string.Empty;
        public static string Side = string.Empty;
        public static string GroupId = string.Empty;
        public static int AccountId = 0;
        static double _markPrice = double.MinValue;
        //public static ClosingData _unSavedClosedData = new ClosingData();

        public static Dictionary<string, TaxLot> _dictTaxlots = new Dictionary<string, TaxLot>();

        //http://jira.nirvanasolutions.com:8080/browse/CHMW-2122
        //Fields Sequence in Tuple: 
        //PositionalTaxlot ,
        //ClosingTaxlot,
        //Quantity Closed,
        //PositionalTaxlot OpenTotalCommissionandFees,
        //ClosingTaxlot OpenTotalCommissionandFees
        public static Dictionary<string, Tuple<TaxLot, TaxLot, double, double, double>> _dictUnsavedPositionsWithTaxlots = new Dictionary<string, Tuple<TaxLot, TaxLot, double, double, double>>();


        public static Dictionary<string, int> _dictClosingAlgoWithPositions = new Dictionary<string, int>();
        public static Dictionary<string, int> DictClosingAlgoWithPositions
        {
            get { return _dictClosingAlgoWithPositions; }
            set {; }
        }

        public static Dictionary<string, double> dictTaxlotsToClose = new Dictionary<string, double>();
        public static Dictionary<string, double> DictTaxlotsToClose
        {
            get { return dictTaxlotsToClose; }
            set {; }
        }

        public static bool _isUnsavedChanges = false;
        public static bool IsUnsavedChanges
        {
            get { return _isUnsavedChanges; }
            set { _isUnsavedChanges = value; }
        }

        #region closed data information
        public static StringBuilder _taxlotClosingID = new StringBuilder();

        public static StringBuilder TaxlotClosingID
        {
            get { return _taxlotClosingID; }
            set {; }
        }

        public static StringBuilder _taxlotIDList = new StringBuilder();

        public static StringBuilder TaxlotIDList
        {
            get { return _taxlotIDList; }
            set {; }
        }

        public static StringBuilder _taxlotClosingIDWithClosingDate = new StringBuilder();

        public static StringBuilder TaxlotClosingIDWithClosingDate
        {
            get { return _taxlotClosingIDWithClosingDate; }
            set {; }
        }
        #endregion

        public static List<string> PositionalSidesForSell
        {
            get
            {
                List<string> sides = new List<string>();
                //TaxlotGridColumns = new List<string>();

                sides.Add(FIXConstants.SIDE_Buy);
                sides.Add(FIXConstants.SIDE_Buy_Open);
                sides.Add(FIXConstants.SIDE_Buy_Closed);
                sides.Add(FIXConstants.SIDE_Buy_Cover);
                sides.Add(FIXConstants.SIDE_BuyMinus);

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


        private static GenericBindingList<TaxLot> _openTaxlots = new GenericBindingList<TaxLot>();

        public static GenericBindingList<TaxLot> OpenTaxlots
        {
            get { return _openTaxlots; }
            set {; }
        }

        //private static GenericBindingList<TaxLot> _openTaxlotsToPopulate = new GenericBindingList<TaxLot>();

        //public static GenericBindingList<TaxLot> OpenTaxlotsToPopulate
        //{
        //    get { return _openTaxlotsToPopulate; }
        //    set { ; }
        //}


        private static GenericBindingList<Position> _netpositions = new GenericBindingList<Position>();

        public static GenericBindingList<Position> Netpositions
        {
            get { return _netpositions; }
            set {; }
        }

        //private static Dictionary<string, TaxLot> _unSavedTaxlots = new Dictionary<string, TaxLot>();

        //public static Dictionary<string, TaxLot> UnSavedTaxlots
        //{
        //    get { return _unSavedTaxlots; }
        //    set { ; }
        //}

        //private static Dictionary<string, Position> _unSavedNetpositions = new Dictionary<string, Position>();

        //public static Dictionary<string, Position> UnSavedNetpositions
        //{
        //    get { return _unSavedNetpositions; }
        //    set { ; }
        //}

        public static List<TaxLot> GetTaxlots(List<AllocationGroup> allocationGroupList, List<TaxLot> Taxlots)
        {
            List<TaxLot> closeTrades = new List<TaxLot>();

            try
            {
                for (int i = 0; i < Taxlots.Count; i++)
                {
                    if (i < allocationGroupList.Count)
                    {
                        TaxLot closeTrade = (TaxLot)Taxlots[i].Clone();
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
                _openTaxlots.Clear();

                UpdateRepository(closingData);

                //_unSavedClosedData = new ClosingData();
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
                _netpositions.Clear();
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

                if (data != null && data.Taxlots.Count > 0)
                {
                    _markPrice = data.Taxlots[0].MarkPrice;
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

                CreateOpenTaxlots(closingdata);

                CreateNetPositions(closingdata);

                //_unSavedClosedData = new ClosingData();
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

        //public static double GetSymbolAccountPositionForGivenDate(string symbol, int accountID, DateTime date)
        //{

        //    double currentPosition = 0;
        //    try
        //    {

        //        List<TaxLot> listAccountTaxlots = new List<TaxLot>(_openTaxlots);
        //        listAccountTaxlots.RemoveAll(delegate(TaxLot obj)
        //        {
        //            if (obj.Symbol.Equals(symbol) && obj.Level1ID.Equals(accountID) && obj.ProcessDate.Date <= date.Date)
        //            {
        //                return false;
        //            }
        //            return true;
        //        });


        //        foreach (TaxLot taxlot in listAccountTaxlots)
        //        {
        //            currentPosition += (taxlot.TaxLotQty * taxlot.SideMultiplier);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //    return currentPosition;
        //}

        public static void UpdateRepository(ClosingData closedData)
        {
            try
            {
                UpdateOpenTaxlots(closedData.Taxlots);

                if (closedData.ClosedPositions.Count > 0)
                {
                    UpdatePositions(closedData.ClosedPositions);
                }
                if (closedData.PositionsToUnwind.Length > 0)
                {
                    UnwindPositions(closedData.PositionsToUnwind);
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
                    taxlot.MarkPrice = _markPrice;
                    //UpdatePublishedTaxlots(taxlot);
                    if (_openTaxlots.GetItem(taxlot.TaxLotID) == null)
                    {
                        //if (taxlot.TaxLotQty >= 0)
                        //    _openTaxlots.Add(taxlot);
                        if (taxlot.ClosingStatus != ClosingStatus.Closed)
                        {
                            _openTaxlots.Add(taxlot);
                            UpdateDictTaxlots(taxlot);
                        }
                    }
                    else
                    {
                        if (taxlot.ClosingStatus == ClosingStatus.Closed || taxlot.TaxLotQty == 0)
                        {
                            _openTaxlots.Remove(taxlot);
                            UpdateDictTaxlots(taxlot);
                        }
                        else if (taxlot.ClosingStatus != ClosingStatus.Closed)
                        {
                            _openTaxlots.UpdateItem(taxlot);
                            UpdateDictTaxlots(taxlot);
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

        //public static bool UpdateTaxLotToPopulateFromAllocation(List<TaxLot> taxlotsToPopulate)
        //{
        //    bool isTaxlotsUpdated = false;
        //    try
        //    {
        //        foreach (TaxLot taxLot in taxlotsToPopulate)
        //        {
        //            //update data for symbol and side on close order UI
        //            if (GetSidesForClosingTaxlots().Contains(taxLot.OrderSideTagValue) && Symbol.Equals(taxLot.Symbol))
        //            {
        //                if (taxLot.TaxLotState == ApplicationConstants.TaxLotState.New && taxLot.Level1ID != 0)
        //                {
        //                    PostReconClosingData.OpenTaxlotsToPopulate.Add(taxLot);
        //                    isTaxlotsUpdated = true;
        //                }
        //                if (taxLot.TaxLotState == ApplicationConstants.TaxLotState.Deleted)
        //                {
        //                    PostReconClosingData.OpenTaxlotsToPopulate.Remove(taxLot);
        //                    isTaxlotsUpdated = true;
        //                }
        //                if (taxLot.TaxLotState == ApplicationConstants.TaxLotState.Updated)
        //                {
        //                    PostReconClosingData.OpenTaxlotsToPopulate.Update(taxLot);
        //                    isTaxlotsUpdated = true;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.

        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return isTaxlotsUpdated;
        //}

        //private static void UpdateTaxlotToPopulate(List<TaxLot> openTaxlots)
        //{
        //    try
        //    {
        //        //Narendra Kumar Jangir 2013 June 4
        //        //RaiseListChangedEvents property Gets or sets a value indicating whether adding or removing items within the list raises ListChanged events.
        //        //set the RaiseListChangedEvents so that ListChanged events does not raise and update binded source in the finally block 
        //        //http://stackoverflow.com/questions/1236983/what-causes-a-listchangedtype-itemmoved-listchange-event-in-a-bindinglistt
        //        _openTaxlotsToPopulate.RaiseListChangedEvents = false;
        //        foreach (TaxLot taxlot in openTaxlots)
        //        {
        //            if (_openTaxlotsToPopulate.GetItem(taxlot.TaxLotID) == null)
        //            {
        //                if (taxlot.ClosingStatus != ClosingStatus.Closed && GetSidesForClosingTaxlots().Contains(taxlot.OrderSideTagValue) && Symbol.Equals(taxlot.Symbol))
        //                {
        //                    _openTaxlotsToPopulate.Add(taxlot);
        //                }
        //            }
        //            else
        //            {
        //                if (taxlot.ClosingStatus != ClosingStatus.Closed)
        //                {
        //                    _openTaxlotsToPopulate.UpdateItem(taxlot);

        //                }
        //                else if (taxlot.ClosingStatus == ClosingStatus.Closed)
        //                {
        //                    _openTaxlotsToPopulate.Remove(taxlot);

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.

        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    finally
        //    {
        //        //Narendra Kumar Jangir 2012 Nov 20
        //        //RaiseListChangedEvents property Gets or sets a value indicating whether adding or removing items within the list raises ListChanged events.
        //        _openTaxlotsToPopulate.RaiseListChangedEvents = true;
        //        //When the BindingList is bound to Windows Forms controls, the ResetBindings method causes a refresh of all controls bound to the list.
        //        _openTaxlotsToPopulate.ResetBindings();
        //    }
        //}


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


        public static void ClearRepository()
        {
            try
            {
                _openTaxlots.Clear();
                _netpositions.Clear();
                //_unSavedClosedData = new ClosingData();
                _dictTaxlots.Clear();
                _dictUnsavedPositionsWithTaxlots.Clear();
                _taxlotClosingID.Clear();
                _taxlotIDList.Clear();
                _taxlotClosingIDWithClosingDate.Clear();
                _dictUnsavedPositionsWithTaxlots.Clear();
                dictTaxlotsToClose.Clear();
                _dictClosingAlgoWithPositions.Clear();
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static bool IsDataAvailabletoClose()
        {
            if (_openTaxlots.Count == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Remove closed positions data from cache
        /// </summary>
        /// <param name="list"></param>
        public static void UnwindUnSavedPositions(string list)
        {
            try
            {
                string[] idsToundwind = list.Split(',');
                Dictionary<string, TaxLot> _taxlots = new Dictionary<string, TaxLot>();
                for (int i = 0; i < idsToundwind.Length; i++)
                {
                    string ClosingID = idsToundwind[i].ToString();
                    if (_dictUnsavedPositionsWithTaxlots.ContainsKey(ClosingID))
                    {
                        TaxLot positionalTaxlot = null;
                        if (!_taxlots.ContainsKey(_dictUnsavedPositionsWithTaxlots[ClosingID].Item1.TaxLotID))
                        {
                            if (_openTaxlots.GetItem(_dictUnsavedPositionsWithTaxlots[ClosingID].Item1.TaxLotID) != null)
                            {
                                positionalTaxlot = _openTaxlots.GetItem(_dictUnsavedPositionsWithTaxlots[ClosingID].Item1.TaxLotID);

                                if (positionalTaxlot.TaxLotQty != positionalTaxlot.ExecutedQty)
                                {
                                    positionalTaxlot.OpenTotalCommissionandFees += GetNewCommissionByProportion(positionalTaxlot.OpenTotalCommissionandFees, positionalTaxlot.TaxLotQty, _dictUnsavedPositionsWithTaxlots[ClosingID].Item3);
                                    positionalTaxlot.TaxLotQty += _dictUnsavedPositionsWithTaxlots[ClosingID].Item3;
                                }
                            }
                            else
                            {
                                positionalTaxlot = _dictUnsavedPositionsWithTaxlots[ClosingID].Item1;
                                //if taxlot is not available in open taxlots(Top grid) but available in closed taxlots dictionary(lower grid).
                                //SO we add taxlot qty and commission two times
                                //to overcome from this issue we are setting taxlotqty and OpenTotalCommissionandFees to zero to start from initial values.
                                //Correct taxlot qty and commission info will be retrieved from _dictUnsavedPositionsWithTaxlots cache
                                positionalTaxlot.TaxLotQty = 0;
                                positionalTaxlot.OpenTotalCommissionandFees = 0;
                                //CHMW-2122	[Post Recon Amendments] Opening Fees & Commission is not updating when closing is unwound without saving
                                positionalTaxlot.OpenTotalCommissionandFees += _dictUnsavedPositionsWithTaxlots[ClosingID].Item4;// GetNewCommissionByProportion(positionalTaxlot.OpenTotalCommissionandFees, positionalTaxlot.TaxLotQty, _dictUnsavedPositionsWithTaxlots[ClosingID].Item3);
                                positionalTaxlot.TaxLotQty += _dictUnsavedPositionsWithTaxlots[ClosingID].Item3;
                            }
                            _taxlots.Add(positionalTaxlot.TaxLotID, (TaxLot)positionalTaxlot.Clone());
                        }
                        else
                        {
                            positionalTaxlot = _taxlots[_dictUnsavedPositionsWithTaxlots[ClosingID].Item1.TaxLotID];
                            _taxlots[positionalTaxlot.TaxLotID].OpenTotalCommissionandFees += GetNewCommissionByProportion(positionalTaxlot.OpenTotalCommissionandFees, positionalTaxlot.TaxLotQty, _dictUnsavedPositionsWithTaxlots[ClosingID].Item3);
                            _taxlots[positionalTaxlot.TaxLotID].TaxLotQty += _dictUnsavedPositionsWithTaxlots[ClosingID].Item3;
                        }

                        TaxLot closingTaxot = null;
                        if (!_taxlots.ContainsKey(_dictUnsavedPositionsWithTaxlots[ClosingID].Item2.TaxLotID))
                        {
                            if (_openTaxlots.GetItem(_dictUnsavedPositionsWithTaxlots[ClosingID].Item2.TaxLotID) != null)
                            {
                                closingTaxot = _openTaxlots.GetItem(_dictUnsavedPositionsWithTaxlots[ClosingID].Item2.TaxLotID);

                                if (closingTaxot.TaxLotQty != closingTaxot.ExecutedQty)
                                {
                                    closingTaxot.OpenTotalCommissionandFees += GetNewCommissionByProportion(closingTaxot.OpenTotalCommissionandFees, closingTaxot.TaxLotQty, _dictUnsavedPositionsWithTaxlots[ClosingID].Item3);
                                    closingTaxot.TaxLotQty += _dictUnsavedPositionsWithTaxlots[ClosingID].Item3;
                                }
                            }
                            else
                            {
                                closingTaxot = _dictUnsavedPositionsWithTaxlots[ClosingID].Item2;
                                //if taxlot is not available in open taxlots(Top grid) but available in closed taxlots dictionary(lower grid).
                                //SO we add taxlot qty and commission two times
                                //to overcome from this issue we are setting taxlotqty and OpenTotalCommissionandFees to zero to start from initial values.
                                //Correct taxlot qty and commission info will be retrieved from _dictUnsavedPositionsWithTaxlots cache
                                closingTaxot.TaxLotQty = 0;
                                closingTaxot.OpenTotalCommissionandFees = 0;
                                //CHMW-2122	[Post Recon Amendments] Opening Fees & Commission is not updating when closing is unwound without saving
                                closingTaxot.OpenTotalCommissionandFees += _dictUnsavedPositionsWithTaxlots[ClosingID].Item5;// GetNewCommissionByProportion(closingTaxot.OpenTotalCommissionandFees, closingTaxot.TaxLotQty, _dictUnsavedPositionsWithTaxlots[ClosingID].Item3);
                                closingTaxot.TaxLotQty += _dictUnsavedPositionsWithTaxlots[ClosingID].Item3;
                            }
                            _taxlots.Add(closingTaxot.TaxLotID, (TaxLot)closingTaxot.Clone());
                        }
                        else
                        {
                            closingTaxot = _taxlots[_dictUnsavedPositionsWithTaxlots[ClosingID].Item2.TaxLotID];
                            _taxlots[closingTaxot.TaxLotID].OpenTotalCommissionandFees += GetNewCommissionByProportion(closingTaxot.OpenTotalCommissionandFees, closingTaxot.TaxLotQty, _dictUnsavedPositionsWithTaxlots[ClosingID].Item3);
                            _taxlots[closingTaxot.TaxLotID].TaxLotQty += _dictUnsavedPositionsWithTaxlots[ClosingID].Item3;
                        }

                        _dictUnsavedPositionsWithTaxlots.Remove(ClosingID);

                    }
                }
                UpdateOpenTaxlots(_taxlots.Values.ToList());
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

        private static double GetNewCommissionByProportion(double prevCommission, double prevQuantity, double newQuantity)
        {
            double newCommission = 0;
            try
            {
                if (prevQuantity != 0)
                    newCommission = (prevCommission / prevQuantity) * newQuantity;
                if (double.IsNaN(newCommission))
                {
                    newCommission = 0;
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
            return newCommission;
        }

        /// <summary>
        /// Update unsaved taxlots in cache
        /// This cache will be used to save the unsaved closing data
        /// </summary>
        /// <param name="unSavedTaxlots"></param>
        //private static void UpdateUnsavedTaxlots(List<TaxLot> unSavedTaxlots)
        //{
        //    try
        //    {
        //        //_unSavedClosedData.UnSavedTaxlots.AddRange(unSavedTaxlots);
        //        //_unSavedClosedData.Taxlots.AddRange(unSavedTaxlots);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// Update Unsaved Positions collection
        /// </summary>
        /// <param name="unSavedPositions"></param>
        private static void UpdateUnSavedPositions(List<Position> unSavedPositions)
        {
            try
            {
                //_unSavedClosedData.ClosedPositions.AddRange(unSavedPositions);

                foreach (Position pos in unSavedPositions)
                {
                    TaxLot positionalTaxlot = (TaxLot)_dictTaxlots[pos.ID].Clone();
                    TaxLot closingTaxlot = (TaxLot)_dictTaxlots[pos.ClosingID].Clone();
                    //positionalTaxlot.TaxLotQty -= pos.ClosedQty;
                    //closingTaxlot.TaxLotQty -= pos.ClosedQty;
                    //CHMW-2122	[Post Recon Amendments] Opening Fees & Commission is not updating when closing is unwound without saving
                    Tuple<TaxLot, TaxLot, double, double, double> tuple1 = new Tuple<TaxLot, TaxLot, double, double, double>(positionalTaxlot, closingTaxlot, pos.ClosedQty, positionalTaxlot.ClosedTotalCommissionandFees, closingTaxlot.ClosedTotalCommissionandFees);
                    if (!_dictUnsavedPositionsWithTaxlots.Keys.Contains(pos.TaxLotClosingId, StringComparer.InvariantCultureIgnoreCase))
                    {
                        _dictUnsavedPositionsWithTaxlots.Add(pos.TaxLotClosingId, tuple1);
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

        /// <summary>
        /// Get closed data having unsaved taxlots from the generic binding list
        /// Closed data is used to save the data in database using unsaved taxlots.
        /// </summary>
        /// <returns></returns>
        //public static ClosingData GetUnSavedClosingData()
        //{
        //    return _unSavedClosedData;
        //}

        /// <summary>
        /// Clear unsaved taxlots and unsaved net positions after saving the data in database
        /// </summary>
        public static void ClearUnSavedData()
        {
            try
            {
                //_unSavedClosedData = new ClosingData();
                _taxlotClosingID.Clear();
                _taxlotIDList.Clear();
                _taxlotClosingIDWithClosingDate.Clear();
                dictTaxlotsToClose.Clear();
                _dictClosingAlgoWithPositions.Clear();
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
        /// Update unsaved closing data
        /// </summary>
        /// <param name="closedData"></param>
        public static void UpdateUnsavedClosingData(ClosingData closedData)
        {
            try
            {
                //UpdateUnsavedTaxlots(closedData.UnSavedTaxlots);

                if (closedData.ClosedPositions.Count > 0)
                {
                    UpdateUnSavedPositions(closedData.ClosedPositions);
                }
                if (closedData.PositionsToUnwind.Length > 0)
                {
                    UnwindUnSavedPositions(closedData.PositionsToUnwind);
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

        /// <summary>
        /// Update data to unwind in cache
        /// </summary>
        /// <param name="TaxlotClosingID"></param>
        /// <param name="taxlotIDList"></param>
        /// <param name="TaxlotClosingIDWithClosingDate"></param>
        public static void UpdateDataToUnwind(Position position)
        {
            try
            {
                if (position.IsClosingSaved)
                {
                    _taxlotClosingID.Append(position.TaxLotClosingId.ToString());
                    _taxlotClosingID.Append(",");

                    _taxlotClosingIDWithClosingDate.Append(position.TaxLotClosingId.ToString());
                    _taxlotClosingIDWithClosingDate.Append('_');
                    _taxlotClosingIDWithClosingDate.Append(position.ClosingTradeDate.ToString());
                    _taxlotClosingIDWithClosingDate.Append(",");

                    _taxlotIDList.Append(position.ID.ToString());
                    _taxlotIDList.Append(",");

                    _taxlotIDList.Append(position.ClosingID.ToString());
                    _taxlotIDList.Append(",");
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
        }

        /// <summary>
        /// Update taxlot in cache
        /// This cache will be used in virtual unwinding and closing
        /// </summary>
        /// <param name="taxlot"></param>
        public static void UpdateDictTaxlots(TaxLot taxlot)
        {
            if (_dictTaxlots.ContainsKey(taxlot.TaxLotID))
            {
                _dictTaxlots[taxlot.TaxLotID] = (TaxLot)taxlot.Clone();
            }
            else if (!_dictTaxlots.ContainsKey(taxlot.TaxLotID))
            {
                _dictTaxlots.Add(taxlot.TaxLotID, (TaxLot)taxlot.Clone());
            }
        }


        /// <summary>
        /// Update TaxLot With Amendments
        /// </summary>
        /// <param name="dictApprovedChanges"></param>
        public static void UpdateTaxLotWithAmendments(Dictionary<string, List<ApprovedChanges>> dictApprovedChanges)
        {
            try
            {
                foreach (KeyValuePair<string, List<ApprovedChanges>> kvp in dictApprovedChanges)
                {
                    if (_openTaxlots.GetItem(kvp.Key) != null)
                    {
                        TaxLot taxlot = _openTaxlots.GetItem(kvp.Key);
                        foreach (ApprovedChanges approvedChanges in dictApprovedChanges[kvp.Key])
                        {
                            double value = 0;
                            if (approvedChanges.ColumnName == "Quantity")
                            {
                                if (double.TryParse(approvedChanges.NewValue, out value))
                                {
                                    taxlot.TaxLotQty = value;
                                    taxlot.ExecutedQty = value;
                                    taxlot.Quantity = value;
                                    taxlot.CumQty = value;
                                }
                            }
                            else if (approvedChanges.ColumnName == "AvgPX")
                            {
                                if (double.TryParse(approvedChanges.NewValue, out value))
                                {
                                    taxlot.AvgPrice = value;
                                }
                            }
                        }
                        UpdateDictTaxlots(taxlot);
                        //_unSavedClosedData.UnSavedTaxlots.Add(taxlot);
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

        /// <summary>
        /// Return taxlot
        /// </summary>
        /// <param name="taxlotID"></param>
        /// <returns></returns>
        public static TaxLot GetTaxlotByTaxLotID(string taxlotID)
        {
            TaxLot taxlot = null;
            try
            {
                if (_dictTaxlots.ContainsKey(taxlotID))
                {
                    taxlot = (TaxLot)(_dictTaxlots[taxlotID].Clone());
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
            return taxlot;
        }

        /// <summary>
        /// Update open taxlots when taxlots updated from allocation, also update taxlot cache
        /// </summary>
        /// <param name="taxlot"></param>
        public static void UpdatePublishedTaxlots(TaxLot taxlot)
        {
            try
            {
                if (taxlot.TaxLotState == ApplicationConstants.TaxLotState.New && taxlot.Level1ID != 0)
                {
                    taxlot.ExecutedQty = taxlot.TaxLotQty;
                    PostReconClosingData.OpenTaxlots.Add(taxlot);
                    UpdateDictTaxlots(taxlot);
                }
                if (taxlot.TaxLotState == ApplicationConstants.TaxLotState.Deleted)
                {
                    PostReconClosingData.OpenTaxlots.Remove(taxlot);
                    //if (_dictTaxlots.ContainsKey(taxlot.TaxLotID))
                    //{
                    //    _dictTaxlots.Remove(taxlot.TaxLotID);
                    //}
                }
                if (taxlot.TaxLotState == ApplicationConstants.TaxLotState.Updated)
                {
                    PostReconClosingData.OpenTaxlots.Update(taxlot);
                    PostReconClosingData.UpdateCorrespondingPositions(taxlot);
                    UpdateDictTaxlots(taxlot);
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
