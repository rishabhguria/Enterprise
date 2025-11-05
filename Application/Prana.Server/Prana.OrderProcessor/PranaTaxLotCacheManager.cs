using Prana.BusinessObjects;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;



namespace Prana.OrderProcessor
{
    public class PranaTaxLotCacheManager
    {
        static IQueueProcessor _queueCommMgrOut = null;
        IQueueProcessor _dbQueue = null;
        static PranaTaxLotCacheManager _pranaTaxLotCacheManager = null;
        static PranaBinaryFormatter binaryFormatter = new PranaBinaryFormatter();
        static PranaTaxLotCacheManager()
        {
            _pranaTaxLotCacheManager = new PranaTaxLotCacheManager();
        }
        public static PranaTaxLotCacheManager GetInstance
        {
            get
            {
                return _pranaTaxLotCacheManager;
            }
        }

        IAllocationServices _allocationServices = null;

        static int _hashCode = int.MinValue;
        public void Initlise(IQueueProcessor queueCommMgrOut, IQueueProcessor dbQueue, IAllocationServices allocationServices)
        {
            _allocationServices = allocationServices;
            _dbQueue = dbQueue;
            _queueCommMgrOut = queueCommMgrOut;
            _queueCommMgrOut.HandlerType = HandlerType.PostTradeHandler;
            _hashCode = this.GetHashCode();
            _queueCommMgrOut = queueCommMgrOut;
        }

        public void CreateGroupAndSendTaxLots(Order order)
        {
            try
            {
                AllocationGroup group = null;
                switch (order.MsgType)
                {
                    // SK 20100416, JIRA issues NEWLAND-260
                    // Group is also created from dispatcher by a call to CreateAndSendTaxlots method. Follow the dispatch method of order processor.
                    // However race conditions are happening and at times. Sometimes the time dispatcher takes to place allocation message
                    // in dbqueue for new order next fill is processcessed and group is not updated. It doesn't affect the preallocation or 
                    // auto allocation using fix rules as in those cases every execution is updated DB using pranataxlotcachemanager. Where as 
                    // in case of unallocated trades saveresponse sp updates group when subsequent fills arrive.
                    // Commented following lines to remove duplicate new order group creation.
                    case FIXConstants.MSGExecutionReport:
                    case FIXConstants.MSGOrderCancelReject:
                        group = _allocationServices.CreateAllocationGroup(order, true);
                        if (group == null)
                        {
                            break;
                        }
                        if ((order.Level1ID != int.MinValue))
                        {
                            if (group.TaxLots != null)
                            {
                                // create taxlot and send to clients
                                foreach (TaxLot taxLot in group.TaxLots)
                                {
                                    //taxLot.CopyBasicDetails(group);
                                    taxLot.Description = group.Description;
                                    taxLot.Quantity = group.Quantity;
                                    taxLot.CumQty = group.CumQty;
                                    taxLot.GroupID = group.GroupID;
                                    SendTaxLot(taxLot);
                                }
                            }
                        }
                        else // create unallocated taxlots and don't save them 
                        {
                            //TaxLot taxLot = PostTradeCacheManager.CreateUnAllocatedTaxLot(order,group.GroupID);
                            TaxLot taxLot = _allocationServices.CreateUnAllocatedTaxLot(order, group.GroupID);
                            if (taxLot != null)
                            {
                                taxLot.Quantity = group.Quantity;
                                taxLot.CumQty = group.CumQty;
                                taxLot.Description = group.Description;
                                taxLot.GroupID = group.GroupID;
                                SendTaxLot(taxLot);
                            }
                        }
                        break;
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

        private void SendTaxLot(TaxLot taxLot)
        {
            try
            {
                TaxLotDetails taxLotDetail = new TaxLotDetails();
                taxLotDetail.CopyBasicDetails(taxLot);

                _secMasterServices.SetSecuritymasterDetails(taxLotDetail);

                //Set UDA details to taxlot - OM
                _secMasterServices.SetSecurityUDADetails(taxLot);

                string request = binaryFormatter.Serialize(taxLotDetail);
                QueueMessage qMsg = new QueueMessage(FIXConstants.MSGAllocation, request);
                _queueCommMgrOut.SendMessage(qMsg);
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
        public void RefreshAllocationDefaults()
        {
            Prana.CommonDataCache.CachedDataManager.GetInstance.RefreshAllocationDefaults();
        }

        private ISecMasterServices _secMasterServices;
        public ISecMasterServices SecMasterServices
        {
            set { _secMasterServices = value; }

        }
    }
}
