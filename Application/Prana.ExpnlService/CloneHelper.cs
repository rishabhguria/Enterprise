using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;

namespace Prana.ExpnlService
{
    public static class CloneHelper
    {
        /// <summary>
        /// Creates the ex PNL order collection clone.
        /// </summary>
        /// <param name="orderCollection">The order collection.</param>
        /// <param name="cloneOrderCollection">The clone order collection.</param>
        public static ExposureAndPnlOrderCollection CreateEPnlOrderCollectionClone(ExposureAndPnlOrderCollection orderCollection)
        {
            ExposureAndPnlOrderCollection cloneOrderCollection = new ExposureAndPnlOrderCollection();
            try
            {
                if (orderCollection != null)
                {
                    for (int i = 0; i < orderCollection.Count; i++)
                    {
                        EPnlOrder cloneOrder;
                        EPnlOrder order = orderCollection[i];

                        //TODO: Get Clone from Factory
                        cloneOrder = CreateEPnlOrderClone(order);
                        if (cloneOrder != null)
                        {
                            cloneOrderCollection.Add(cloneOrder);
                        }
                    }
                }
                cloneOrderCollection.IsUpdated = true;
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
            return cloneOrderCollection;
        }

        public static EPnlOrder CreateEPnlOrderClone(EPnlOrder order)
        {
            try
            {
                //binary formattor not used as noticibly slow while cloning individual objects
                EPnlOrder cloneOrder;
                switch (order.ClassID)
                {
                    case EPnLClassID.EPnlOrder:
                        cloneOrder = ((EPnlOrder)order).Clone();
                        break;

                    case EPnLClassID.EPnLOrderEquity:
                        cloneOrder = ((EPnLOrderEquity)order).Clone();
                        break;
                    case EPnLClassID.EPnLOrderOption:
                        cloneOrder = ((EPnLOrderOption)order).Clone();
                        break;
                    case EPnLClassID.EPnLOrderFuture:
                        cloneOrder = ((EPnLOrderFuture)order).Clone();
                        break;
                    case EPnLClassID.EPnLOrderFX:
                        cloneOrder = ((EPnLOrderFX)order).Clone();
                        break;
                    case EPnLClassID.EPnLOrderFXForward:
                        cloneOrder = ((EPnLOrderFXForward)order).Clone();
                        break;
                    case EPnLClassID.EPnLOrderEquitySwap:
                        cloneOrder = ((EPnLOrderEquitySwap)order).Clone();
                        cloneOrder.IsSwapped = order.IsSwapped;
                        break;
                    case EPnLClassID.EPnLOrderFixedIncome:
                        cloneOrder = ((EPnLOrderFixedIncome)order).Clone();
                        break;
                    default:
                        cloneOrder = ((EPnlOrder)order).Clone();
                        break;
                }
                return cloneOrder;
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

                return null;
            }
        }
    }
}