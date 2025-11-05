using System;
using System.Collections.Generic;
using System.Text;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
namespace Prana.BasketTrading
{
    class GridRowCacheManager
    {
        private  Dictionary<string, UltraGridRow> _dictGrdRowCollection = new Dictionary<string, UltraGridRow>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientOrderID"></param>
        /// <param name="row"></param>
        public  void AddRow(string clientOrderID, UltraGridRow row)
        {
            if (!_dictGrdRowCollection.ContainsKey(clientOrderID))
            {
                _dictGrdRowCollection.Add(clientOrderID, row);
            }
            else
            {
               // _dictGrdRowCollection[clientOrderID] = row;
            }
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        public void AddGridRows(UltraGrid grid, BasketTradingConstants.GridType gridType)
        {
            try
            {
                foreach (UltraGridRow row in grid.Rows)
                {
                    Order order = ((Order)row.ListObject);
                    AddRow(row.Cells["ClientOrderID"].Value.ToString(), row);
                    if (order.SubOrders.Count > 0) // parent order
                    {
                        LockRow(row);
                    }
                    //if ((order.Parent  != null)  )
                    //{
                    //    row.Cells[OrderFields.CAPTION_SYMBOL].Activation = Activation.ActivateOnly;

                    //    row.Cells[OrderFields.PROPERTY_QUANTITY].Activation = Activation.ActivateOnly;
                    //    row.Cells[OrderFields.PROPERTY_ORDER_SIDETAGVALUE].Activation = Activation.ActivateOnly;

                    //    //row.Cells[OrderFields.CAPTION_STRATEGYID].Activation = Activation.ActivateOnly;
                    //    //row.Cells[OrderFields.CAPTION_AccountID].Activation = Activation.ActivateOnly;
                    //}
                    
                        // any order that is traded
                    else if (row.Cells[OrderFields.PROPERTY_ORDER_STATUSTAGVALUE].Value.ToString().Trim() != string.Empty)
                    {
                        LockRow(row);
                    }

                    if (gridType == BasketTradingConstants.GridType.GroupTktReplace)
                    {
                        row.Cells[OrderFields.PROPERTY_QUANTITY].Activation = Activation.AllowEdit;
                        row.Cells[OrderFields.PROPERTY_ORDER_TYPETAGVALUE].Activation = Activation.AllowEdit;
                        row.Cells[OrderFields.PROPERTY_PRICE].Activation = Activation.AllowEdit;
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientOrderID"></param>
        public  void LockRow(string clientOrderID)
        {
            try
            {
                if (_dictGrdRowCollection.ContainsKey(clientOrderID))
                {
                    UltraGridRow row = _dictGrdRowCollection[clientOrderID];
                    LockRow(row);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private List<string> GetLockedColumns()
        {
            List<string> lockedColumns = new List<string>();
            lockedColumns.Add(OrderFields.PROPERTY_COUNTERPARTY_ID);
            lockedColumns.Add(OrderFields.PROPERTY_COUNTERPARTY_NAME);
            lockedColumns.Add(OrderFields.PROPERTY_VENUE_ID);
            lockedColumns.Add(OrderFields.PROPERTY_VENUE);
            lockedColumns.Add(OrderFields.CAPTION_SYMBOL);
            lockedColumns.Add(OrderFields.PROPERTY_ORDER_SIDETAGVALUE);
            lockedColumns.Add(OrderFields.PROPERTY_ORDER_SIDE);
            lockedColumns.Add(OrderFields.PROPERTY_QUANTITY);
            lockedColumns.Add(OrderFields.PROPERTY_ORDER_TYPETAGVALUE);
            lockedColumns.Add(OrderFields.PROPERTY_ORDER_TYPE);
            lockedColumns.Add(OrderFields.PROPERTY_PRICE);
            lockedColumns.Add(OrderFields.PROPERTY_HANDLING_INST);
            lockedColumns.Add(OrderFields.PROPERTY_EXECUTION_INST);
            lockedColumns.Add(OrderFields.PROPERTY_TIF);
            lockedColumns.Add(OrderFields.PROPERTY_LEVEL2ID);
            lockedColumns.Add(OrderFields.PROPERTY_LEVEL1ID);
            
            return lockedColumns;
        }
        public  void LockRow(UltraGridRow row)
        {
            try
            {
                List<string> lockedColumns = GetLockedColumns(); 
                foreach (string lockedColumn in lockedColumns)
                {
                    //if (!(row.Cells[lockedColumn].Value.ToString() == string.Empty || row.Cells[lockedColumn].Value.ToString() == int.MinValue.ToString() || row.Cells[lockedColumn].Value.ToString() == Common.C_COMBO_SELECT))
                   // {
                        row.Cells[lockedColumn].Activation = Activation.ActivateOnly;
                   // }
                }
            }           

            catch (Exception)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                //bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                //if (rethrow)
                //{
                //    throw;
                //}
            }
        }

        public  UltraGridRow GetRow(string clientOrderID)
        {
            if (_dictGrdRowCollection.ContainsKey(clientOrderID))
                return _dictGrdRowCollection[clientOrderID];
            else
                return null;
        }
    }
}
