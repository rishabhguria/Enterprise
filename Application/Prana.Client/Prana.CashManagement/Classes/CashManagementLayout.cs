using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Prana.CashManagement.Classes
{
    /// <summary>
    /// This class is the root class to store the column related preferences based on 
    /// different custom views
    /// </summary>
    [XmlRoot("CashManagementLayoutDetails")]
    [Serializable]
    public class CashManagementLayout : IDisposable
    {
        #region Members

        /// <summary>
        /// The tab name
        /// </summary>
        [XmlElement("TabName", typeof(string))]
        public string TabName;

        /// <summary>
        /// The selected columns collection
        /// </summary>
        [XmlArray("SelectedColumnsCollection"), XmlArrayItem("Column", typeof(CashGridColumn))]
        public List<CashGridColumn> SelectedColumnsCollection;

        /// <summary>
        /// The group by columns collection
        /// </summary>
        [XmlArray("GroupByColumnsCollection"), XmlArrayItem("GroupByColumn", typeof(string))]
        public List<string> GroupByColumnsCollection;

        /// <summary>
        /// The is saved
        /// </summary>
        private bool _isSaved = false;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is saved.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is saved; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool IsSaved
        {
            get { return _isSaved; }
            set { _isSaved = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CashManagementLayout"/> class.
        /// </summary>
        public CashManagementLayout()
        {
            TabName = string.Empty;
            SelectedColumnsCollection = new List<CashGridColumn>();
            GroupByColumnsCollection = new List<string>();
        }

        #endregion Constructors

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    TabName = null;
                    if (SelectedColumnsCollection != null)
                    {
                        foreach (CashGridColumn obj in SelectedColumnsCollection)
                        {
                            obj.Dispose();
                        }
                        SelectedColumnsCollection.Clear();
                        SelectedColumnsCollection = null;
                    }
                    if (GroupByColumnsCollection != null)
                        GroupByColumnsCollection.Clear();
                    GroupByColumnsCollection = null;
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

        #endregion 
    }
}
