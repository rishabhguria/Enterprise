//
using Csla;
using Csla.Validation;

namespace Prana.PM.BLL
{
    public class TableType : BusinessBase<TableType>
    {
        #region Constants
        const string CONST_TableTypeID = "TableTypeID";
        #endregion

        public TableType()
        {
            MarkAsChild();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableType"/> class.
        /// </summary>
        /// <param name="tableTypeID">The table type ID.</param>
        /// <param name="tableTypeName">Name of the table type.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TableType(int tableTypeID, string tableTypeName)
        {
            this.TableTypeID = tableTypeID;
            this.TableTypeName = tableTypeName;
        }

        private int _tableTypeID;

        /// <summary>
        /// Gets or sets the table type ID.
        /// </summary>
        /// <value>The data table type ID.</value>
        public int TableTypeID
        {
            get
            {
                return _tableTypeID;
            }
            set
            {
                _tableTypeID = value;
                PropertyHasChanged(CONST_TableTypeID);
            }
        }


        private string _tableTypeName;

        /// <summary>
        /// Gets or sets the name of the table type name.
        /// </summary>
        /// <value>The name of the table type name.</value>
        public string TableTypeName
        {
            get
            {
                return _tableTypeName;

            }
            set
            {
                _tableTypeName = value;
                PropertyHasChanged();
            }
        }

        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        //  private int _id;
        /// <summary>
        /// TODO : Can't afford to have this ID property in multiuer environment
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return 0;
        }

        /// <summary>
        /// Adds the business rules.
        /// </summary>
        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(CommonRules.MinValue<int>, new CommonRules.MinValueRuleArgs<int>(CONST_TableTypeID, 1));
        }
    }
}
