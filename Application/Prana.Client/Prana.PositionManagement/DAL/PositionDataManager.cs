using Prana.BusinessObjects;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;

namespace Prana.PositionManagement
{
    class PositionDataManager
    {
        #region Proxy Section

        static ProxyBase<IPranaPositionServices> _positionManagementServices = null;
        public static ProxyBase<IPranaPositionServices> PositionManagementServices
        {
            set
            {
                _positionManagementServices = value;

            }
            get { return _positionManagementServices; }
        }

        public static void CreatePositionManagementProxy()
        {
            PositionManagementServices = new ProxyBase<IPranaPositionServices>("TradePositionServiceEndpointAddress");
        }


        #endregion        

        private static PositionDataManager _sinleton = null;
        private static object _loker = new object();
        public static PositionDataManager GetInstance()
        {
            if (_sinleton == null)
                lock (_loker)
                    if (_sinleton == null)
                    {
                        _sinleton = new PositionDataManager();
                        if (_positionManagementServices == null)
                            CreatePositionManagementProxy();
                    }
            return _sinleton;
        }

        public DateTime GetSnapShotDate()
        {
            DateTime snapShotDate = new DateTime();
            try
            {
                snapShotDate = _positionManagementServices.InnerChannel.GetSnapShotDate();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return snapShotDate;
        }
        public bool SaveSnapShotDate(DateTime givenDate)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = _positionManagementServices.InnerChannel.SaveSnapShotDate(givenDate);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isSuccess;
        }

        private GenericBindingList<TaxLot> _openPositions;
        public GenericBindingList<TaxLot> OpenPositions
        {
            get { return _openPositions; }
            set { _openPositions = value; }
        }

        public void GetOpenPositionsDAL(DateTime date)
        {
            try
            {
                List<TaxLot> _lsTaxLot = _positionManagementServices.InnerChannel.GetOpenPositions(date);
                OpenPositions = new GenericBindingList<TaxLot>();
                OpenPositions.Clear();
                foreach (TaxLot t in _lsTaxLot)
                    OpenPositions.Add(t);
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

        private GenericBindingList<TaxLot> _openTransactions;
        public GenericBindingList<TaxLot> OpenTransactions
        {
            get { return _openTransactions; }
            set { _openTransactions = value; }
        }

        public void GetTransactionsDAL(DateTime date)
        {
            try
            {
                List<TaxLot> _lsTaxLot = _positionManagementServices.InnerChannel.GetTransactions(date);
                OpenTransactions = new GenericBindingList<TaxLot>();
                OpenTransactions.Clear();
                foreach (TaxLot t in _lsTaxLot)
                    OpenTransactions.Add(t);
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

        //private GenericBindingList<TaxLot> _openTaxlots;
        //public GenericBindingList<TaxLot> OpenTaxlots
        //{
        //    get { return _openTaxlots; }
        //    set { _openTaxlots = value; }
        //}


    }
}
