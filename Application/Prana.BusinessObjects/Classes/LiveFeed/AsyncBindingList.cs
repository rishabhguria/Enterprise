using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Prana.BusinessObjects.LiveFeed
{
    public class AsyncBindingList<T> : BindingList<T>, IDisposable
    {

        public AsyncBindingList()
            : base()
        {
            Initialize();
        }

        // Create AsyncBindingList on the UI thread.
        public AsyncBindingList(IList<T> list)
            : base(list)
        {
            Initialize();
        }

        private void Initialize()
        {
            this.syncContext = new System.Windows.Forms.WindowsFormsSynchronizationContext();
            this.updateCallback = new SendOrPostCallback(this.UpdateBindingList);
        }

        #region Private members for posting messages.
        private bool executeSynchronously = false;
        #endregion

        #region Overrides from the base class. Executed on the Worker thread or on the UI thread.

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            if (this.executeSynchronously)
            {
                base.OnListChanged(e);

            }
            else
            {
                this.syncContext.Post(this.updateCallback, new DataTransaction(DataOperationKind.ListChanged, e, -1));
            }
        }

        protected override bool SupportsSortingCore
        {
            get
            {
                return true;
            }
        }

        protected override bool SupportsSearchingCore
        {
            get
            {
                return true;
            }
        }

        public void Sort(IComparer<T> sortCriteria)
        {
            if (this.executeSynchronously)
            {
                List<T> list = this.Items as List<T>;
                list.Sort(sortCriteria);
            }
            else
            {
                this.syncContext.Post(this.updateCallback, new DataTransaction(DataOperationKind.ApplySort, null, -1, sortCriteria));
            }
        }

        public int Exists(T item, IComparer<T> searchCriteria)
        {
            if (this.executeSynchronously)
            {
                List<T> list = this.Items as List<T>;
                return list.BinarySearch(item, searchCriteria);
            }
            else
            {
                DataTransaction transaction = new DataTransaction(DataOperationKind.Find, item, -1, searchCriteria);
                this.syncContext.Send(this.updateCallback, transaction);
                return (int)transaction.TransactionResult;
            }
        }

        public new int Count
        {
            get
            {
                if (this.executeSynchronously)
                {
                    return base.Count;
                }
                else
                {
                    DataTransaction tr = new DataTransaction(DataOperationKind.GetCount, null, -1);
                    this.syncContext.Send(this.updateCallback, tr);
                    return (int)tr.TransactionResult;
                }
            }
        }

        public new void Add(T item)
        {
            if (this.executeSynchronously)
            {
                base.Add(item);
            }
            else
            {
                this.syncContext.Post(this.updateCallback, new DataTransaction(DataOperationKind.Add, item, -1));
            }
        }

        protected override object AddNewCore()
        {
            if (this.executeSynchronously)
            {
                return base.AddNewCore();
            }
            else
            {
                // we need the result for AddNewCore...
                // So we block until AddNewCore finishes on the UI thread.
                DataTransaction transaction = new DataTransaction(DataOperationKind.AddNewItem, null, -1);
                this.syncContext.Send(this.updateCallback, transaction);

                return transaction.TransactionResult;
            }
        }

        protected override void ClearItems()
        {
            if (this.executeSynchronously)
            {
                base.ClearItems();
            }
            else
            {
                this.syncContext.Post(this.updateCallback, new DataTransaction(DataOperationKind.Clear, null, -1));
            }
        }

        protected override void InsertItem(int index, T item)
        {
            if (this.executeSynchronously)
            {
                base.InsertItem(index, item);
            }
            else
            {
                DataTransaction tr = new DataTransaction(DataOperationKind.InsertItem, item, index);
                this.syncContext.Post(this.updateCallback, tr);
            }
        }

        protected override void RemoveItem(int index)
        {
            if (this.executeSynchronously)
            {
                base.RemoveItem(index);
            }
            else
            {
                this.syncContext.Post(this.updateCallback, new DataTransaction(DataOperationKind.RemoveItem, null, index));
            }
        }

        protected override void SetItem(int index, T item)
        {
            if (this.executeSynchronously)
            {
                base.SetItem(index, item);
            }
            else
            {
                this.syncContext.Post(this.updateCallback, new DataTransaction(DataOperationKind.SetItem, item, index));
            }
        }

        public override void CancelNew(int itemIndex)
        {
            if (this.executeSynchronously)
            {
                base.CancelNew(itemIndex);
            }
            else
            {
                this.syncContext.Post(this.updateCallback, new DataTransaction(DataOperationKind.CancelNew, null, itemIndex));
            }
        }

        public override void EndNew(int itemIndex)
        {
            if (this.executeSynchronously)
            {
                base.EndNew(itemIndex);
            }
            else
            {
                this.syncContext.Post(this.updateCallback, new DataTransaction(DataOperationKind.EndNew, null, itemIndex));
            }
        }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            // not implemented.
        }

        protected override void RemoveSortCore()
        {
            // not implemented.
        }

        protected override int FindCore(PropertyDescriptor prop, object key)
        {
            // not implemented.
            return -1;
        }
        #endregion

        #region Private members for processing Posted messages. Used on the UI thread.
        private SynchronizationContext syncContext = null;
        private SendOrPostCallback updateCallback = null;
        #endregion

        #region Process Posted messages. Executed on the UI thread.
        private void UpdateBindingList(object arg)
        {
            DataTransaction transaction = arg as DataTransaction;
            if (transaction == null)
            {
                return;
            }

            ExecuteTransaction(transaction);
        }

        private void ExecuteTransaction(DataTransaction transaction)
        {
            switch (transaction.DataOperationKind)
            {
                case DataOperationKind.Clear:
                    base.ClearItems();
                    break;
                case DataOperationKind.InsertItem:
                    base.InsertItem(transaction.Index, (T)transaction.DataObject);
                    break;
                case DataOperationKind.RemoveItem:
                    base.RemoveItem(transaction.Index);
                    break;
                case DataOperationKind.SetItem:
                    base.SetItem(transaction.Index, (T)transaction.DataObject);
                    break;
                case DataOperationKind.AddNewItem:
                    this.executeSynchronously = true;
                    try
                    {
                        object res = base.AddNewCore();
                        transaction.TransactionResult = res;
                    }
                    finally
                    {
                        this.executeSynchronously = false;
                    }
                    break;
                case DataOperationKind.Add:
                    this.executeSynchronously = true;
                    try
                    {
                        base.Add((T)transaction.DataObject);
                    }
                    finally
                    {
                        this.executeSynchronously = false;
                    }
                    break;
                case DataOperationKind.EndNew:
                    base.EndNew(transaction.Index);
                    break;
                case DataOperationKind.CancelNew:
                    this.executeSynchronously = true;
                    try
                    {
                        base.CancelNew(transaction.Index);
                    }
                    finally
                    {
                        this.executeSynchronously = false;
                    }
                    break;
                case DataOperationKind.GetCount:
                    transaction.TransactionResult = base.Count;
                    break;
                case DataOperationKind.ApplySort:
                    this.executeSynchronously = true;
                    try
                    {

                        this.Sort(transaction.IComparerSortCriteria);
                    }
                    finally
                    {
                        this.executeSynchronously = false;
                    }
                    break;
                case DataOperationKind.Find:
                    this.executeSynchronously = true;
                    try
                    {
                        object res = this.Exists((T)transaction.DataObject, transaction.IComparerSortCriteria);
                        transaction.TransactionResult = res;
                    }
                    finally
                    {
                        this.executeSynchronously = false;
                    }
                    break;
                case DataOperationKind.ListChanged:
                    this.executeSynchronously = true;
                    try
                    {
                        base.OnListChanged((ListChangedEventArgs)transaction.DataObject);
                    }
                    finally
                    {
                        this.executeSynchronously = false;
                    }
                    break;
            }
        }
        #endregion

        #region Enums and classes for Posting.
        private enum DataOperationKind
        {
            Clear,
            InsertItem,
            RemoveItem,
            SetItem,
            AddNewItem,
            Add,
            EndNew,
            CancelNew,
            GetCount,
            ApplySort,          // use this once Sorting is implemented
            RemoveSort,         // use this once Sorting is implemented
            Find,                // use this once Searching is implemented
            ListChanged
        }

        private class DataTransaction
        {
            DataOperationKind kind;
            object obj;
            object result;
            int index;
            IComparer<T> comparer;

            public DataTransaction(DataOperationKind kind, object obj, int index)
            {
                this.kind = kind;
                this.obj = obj;
                this.index = index;
            }
            public DataTransaction(DataOperationKind kind, object obj, int index, IComparer<T> comparer)
            {
                this.kind = kind;
                this.obj = obj;
                this.index = index;
                this.comparer = comparer;
            }

            public DataOperationKind DataOperationKind
            {
                get
                {
                    return this.kind;
                }
            }

            public Object DataObject
            {
                get
                {
                    return this.obj;
                }
            }

            public int Index
            {
                get
                {
                    return this.index;
                }
            }

            public IComparer<T> IComparerSortCriteria
            {
                get
                {
                    return this.comparer;
                }
            }

            public object TransactionResult
            {
                get
                {
                    return this.result;
                }
                set
                {
                    this.result = value;
                }
            }
        }
        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (syncContext != null)
                        (syncContext as System.Windows.Forms.WindowsFormsSynchronizationContext).Dispose();
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
    }
}
