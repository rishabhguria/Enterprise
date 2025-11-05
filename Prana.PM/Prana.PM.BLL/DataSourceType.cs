using Csla;
using Csla.Validation;

namespace Prana.PM.BLL
{
    public class DataSourceType : BusinessBase<DataSourceType>
    {
        #region Constants
        const string CONST_TypeID = "DataSourceTypeID";
        #endregion
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceType"/> class.
        /// </summary>
        public DataSourceType()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceType"/> class.
        /// </summary>
        /// <param name="dataSourceTypeID">The data source type ID.</param>
        /// <param name="dataSourceTypeName">Name of the data source type.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DataSourceType(int dataSourceTypeID, string dataSourceTypeName)
        {
            this.DataSourceTypeID = dataSourceTypeID;
            this.DataSourceTypeName = dataSourceTypeName;
        }

        private int _dataSourceTypeID;

        /// <summary>
        /// Gets or sets the data source type ID.
        /// </summary>
        /// <value>The data source type ID.</value>
        public int DataSourceTypeID
        {
            get
            {
                return _dataSourceTypeID;
            }
            set
            {
                _dataSourceTypeID = value;
                PropertyHasChanged(CONST_TypeID);
            }
        }


        private string _dataSourceTypeName;

        /// <summary>
        /// Gets or sets the name of the data source type.
        /// </summary>
        /// <value>The name of the data source type.</value>
        public string DataSourceTypeName
        {
            get
            {
                return _dataSourceTypeName;

            }
            set
            {
                _dataSourceTypeName = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value><c>true</c> if this instance is dirty; otherwise, <c>false</c>.</value>
        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }


        protected override object GetIdValue()
        {
            return _dataSourceTypeID;
        }

        /// <summary>
        /// Adds the business rules.
        /// </summary>
        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(CommonRules.MinValue<int>, new CommonRules.MinValueRuleArgs<int>(CONST_TypeID, 1));
        }

    }
}
