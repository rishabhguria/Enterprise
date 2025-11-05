using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class MappingItem
    {

        public MappingItem()
        {

        }

        public MappingItem(string sourceItemName, string applicationItemName)
        {
            this.SourceItemName = sourceItemName;
            this.ApplicationItemName = applicationItemName;
        }

        public MappingItem(string sourceItemName, string applicationItemName, string applicationItemFullName)
        {
            this.SourceItemName = sourceItemName;
            this.ApplicationItemName = applicationItemName;
            this.ApplicationItemFullName = applicationItemFullName;
        }

        public MappingItem(string sourceItemName, string applicationItemName, string applicationItemFullName, bool Lock)
        {
            this.SourceItemName = sourceItemName;
            this.ApplicationItemName = applicationItemName;
            this.ApplicationItemFullName = applicationItemFullName;
            this.Lock = Lock;
        }

        private string _sourceItemName;

        /// <summary>
        /// Gets or sets the name of the source item.
        /// </summary>
        /// <value>The name of the source item.</value>
        public string SourceItemName
        {
            get { return _sourceItemName; }
            set { _sourceItemName = value; }
        }

        private string _sourceItemFullName;

        /// <summary>
        /// Gets or sets the full name of the source item.
        /// </summary>
        /// <value>The full name of the source item.</value>
        public string SourceItemFullName
        {
            get { return _sourceItemFullName; }
            set { _sourceItemFullName = value; }
        }


        private string _applicationItemName;

        /// <summary>
        /// Gets or sets the name of the application item.
        /// </summary>
        /// <value>The name of the application item.</value>
        public string ApplicationItemName
        {
            get { return _applicationItemName; }
            set { _applicationItemName = value; }
        }

        //To Do: if required add SourceItemFullName, as of now it's not required so ignoring it!
        private string _applicationItemFullName;

        /// <summary>
        /// Gets or sets the full name of the application item.
        /// </summary>
        /// <value>The full name of the application item.</value>
        public string ApplicationItemFullName
        {
            get { return _applicationItemFullName; }
            set { _applicationItemFullName = value; }
        }

        private bool _lock;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MappingItem"/> is lock.
        /// </summary>
        /// <value><c>true</c> if lock; otherwise, <c>false</c>.</value>
        public bool Lock
        {
            get { return _lock; }
            set { _lock = value; }
        }
    }
}
