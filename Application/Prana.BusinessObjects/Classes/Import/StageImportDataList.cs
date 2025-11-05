using Prana.LogManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Customized List of StageImportData
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IList{Prana.BusinessObjects.StageImportData}" />
    public class StageImportDataList : IList<StageImportData>
    {
        /// <summary>
        /// The underlying list containing StageImportData objects
        /// </summary>
        private readonly IList<StageImportData> _list = new List<StageImportData>();


        /// <summary>
        /// The synchronize root
        /// </summary>
        private static readonly object syncRoot = new Object();

        #region Implementation of IEnumerable

        public IEnumerator<StageImportData> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<T>

        /// <summary>
        /// Adds a new order to the StageImportDataList.
        /// </summary>
        /// <param name="Symbology">The symbology.</param>
        /// <param name="Symbol">The symbol.</param>
        /// <param name="order">The order.</param>
        public void Add(int Symbology, string Symbol, OrderSingle order, AccountCollection accountCollection)
        {
            //check if there is already an StageImportData object with same symbology and symbol. 
            //If yes, then add to that object, otherwise create and add new object
            try
            {
                lock (syncRoot)
                {
                    if (accountCollection != null && !accountCollection.Contains(order.Level1ID))
                    {
                        if (order.Level1ID != int.MinValue && order.Level1ID > 0)
                        {
                            order.OriginalAllocationPreferenceID = order.Level1ID;
                            StageImportData item = new StageImportData(Symbology, order.Symbol, order);
                            _list.Add(item);
                        }
                    }
                    else
                    {

                        if (this.Contains(Symbology, Symbol))
                        {
                            _list.First(d => d.Symbology.Equals(Symbology) && d.Symbol.Equals(Symbol)).AddToList(order, accountCollection);
                        }
                        else
                        {
                            StageImportData item = new StageImportData(Symbology, order.Symbol, order);
                            _list.Add(item);
                        }
                    }
                }
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

        /// <summary>
        /// Checks if an StageImportData with same symbol and symbology exists. If yes, then Add the orderSingles to that object, otherwise add as new StageImportData
        /// </summary>
        /// <param name="item"></param>
        public void Add(StageImportData item)
        {
            try
            {
                lock (syncRoot)
                {
                    _list.Add(item);
                }
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

        public void Clear()
        {
            _list.Clear();
        }

        /// <summary>
        /// Determines whether the StageImportDataList contains a specific object having same Symbology and Symbol.
        /// </summary>
        /// <param name="item">The object to locate.</param>
        /// <returns>
        /// true if an item is found with same Symbology and Symbol in the StageImportDataList ; otherwise, false.
        /// </returns>
        public bool Contains(StageImportData item)
        {
            try
            {
                lock (syncRoot)
                {
                    for (int index = 0; index < _list.Count; index++)
                    {
                        if (_list[index].Symbology.Equals(item.Symbology) && _list[index].Symbol.Equals(item.Symbol))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether the list contains an object with the specified symbology and symbol.
        /// </summary>
        /// <param name="Symbology">The symbology.</param>
        /// <param name="Symbol">The symbol.</param>
        /// <returns>true if an item is found with same Symbology and Symbol in the StageImportDataList ; otherwise, false.</returns>
        public bool Contains(int Symbology, string Symbol)
        {
            try
            {
                lock (syncRoot)
                {
                    for (int index = 0; index < _list.Count; index++)
                    {
                        if (_list[index].Symbology.Equals(Symbology) && _list[index].Symbol.Equals(Symbol))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        public void CopyTo(StageImportData[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(StageImportData item)
        {
            return _list.Remove(item);
        }

        /// <summary>
        /// Removes the first occurrence of an object with specified symbology and symbol from the StageImportDataList.
        /// </summary>
        /// <returns>
        /// true if an item was successfully removed from the list; otherwise, false. This method also returns false if no item is not found in the original List.
        /// </returns>
        public bool Remove(int Symbology, string Symbol)
        {
            try
            {
                lock (syncRoot)
                {
                    for (int index = 0; index < _list.Count; index++)
                    {
                        if (_list[index].Symbology.Equals(Symbology) && _list[index].Symbol.Equals(Symbol))
                        {
                            StageImportData item = _list[index];
                            return _list.Remove(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;

        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return _list.IsReadOnly; }
        }

        #endregion

        #region Implementation of IList<T>

        /// <summary>
        /// Determines the index of a specific item in the StageImportDataList.
        /// </summary>
        /// <param name="item">The object to locate in the StageImportDataList.</param>
        /// <returns>
        /// The index of <paramref name="item" /> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(StageImportData item)
        {
            return _list.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the StageImportDataList at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the StageImportDataList.</param>
        public void Insert(int index, StageImportData item)
        {
            _list.Insert(index, item);
        }

        /// <summary>
        /// Removes the StageImportDataList item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public StageImportData this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }

        #endregion

        #region New Added Stuff

        /// <summary>
        /// Gets the stage import data.
        /// </summary>
        /// <param name="Symbology">The symbology.</param>
        /// <param name="Symbol">The symbol.</param>
        /// <returns>The object of stageImportData with specified Symbology and Symbol</returns>
        public StageImportData getStageImportData(int Symbology, string Symbol)
        {
            StageImportData Importdata = null;
            try
            {
                lock (syncRoot)
                {
                    Importdata = this.FirstOrDefault(data => data.Symbology.Equals(Symbology) && data.Symbol.Equals(Symbol));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return Importdata;
        }

        /// <summary>
        /// Gets the Allocation scheme.
        /// </summary>
        /// <returns>Datatable containing the scheme</returns>
        public DataTable GetScheme()
        {
            try
            {
                DataTable allocationScheme = new DataTable();
                if (_list != null && _list.Count > 0)
                {
                    //Sets the layout and table name of datatable based on first entry in list
                    DataTable symbolAllocationScheme = _list[0].GetSymbolAllocationScheme();
                    if (symbolAllocationScheme != null)
                        allocationScheme = symbolAllocationScheme.Clone();

                    foreach (StageImportData data in _list)
                    {
                        DataTable temp = data.GetSymbolAllocationScheme();
                        if (temp != null)
                            allocationScheme.Merge(temp);
                    }
                }
                return allocationScheme;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        #endregion
    }
}
