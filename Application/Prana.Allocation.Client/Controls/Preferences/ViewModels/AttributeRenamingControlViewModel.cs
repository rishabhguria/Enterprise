using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.Allocation.Client.Controls.Preferences.ViewModels
{
    public class AttributeRenamingControlViewModel : ViewModelBase, IDisposable
    {
        #region Members

        /// <summary>
        /// The _attribute renaming collection
        /// </summary>
        private DataTable _attributeRenamingCollection;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the attribute renaming collection.
        /// </summary>
        /// <value>
        /// The attribute renaming collection.
        /// </value>
        public DataTable AttributeRenamingCollection
        {
            get { return _attributeRenamingCollection; }
            set
            {
                _attributeRenamingCollection = value;
                RaisePropertyChangedEvent("AttributeRenamingCollection");
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeRenamingControlViewModel"/> class.
        /// </summary>
        public AttributeRenamingControlViewModel()
        {
            try
            {
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Called when [load attribute renaming control].
        /// </summary>
        internal void OnLoadAttributeRenamingControl()
        {
            try
            {
                DataSet attibuteRenamingSet = CachedDataManager.GetInstance.GetAttributeNames();
                AttributeRenamingCollection = attibuteRenamingSet.Tables[0];

                // Attach the event handler
                AttributeRenamingCollection.ColumnChanging += HandleColumnChanging;
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void HandleColumnChanging(object sender, DataColumnChangeEventArgs e)
        {
            if (e.Column.ColumnName == "KeepRecord" && e.ProposedValue == DBNull.Value)
            {
                e.ProposedValue = false;
            }
        }

        public void Dispose()
        {
            if (AttributeRenamingCollection != null)
            {
                AttributeRenamingCollection.ColumnChanging -= HandleColumnChanging;
            }
        }

        #endregion Methods
    }
}
