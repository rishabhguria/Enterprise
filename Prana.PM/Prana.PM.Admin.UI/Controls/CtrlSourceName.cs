using Infragistics.Win.UltraWinGrid;
//using Prana.PM.Common;
using Prana.BusinessObjects.PositionManagement;
using Prana.PM.BLL;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;
namespace Prana.PM.Admin.UI.Controls
{
    public partial class CtrlSourceName : UserControl
    {

        //private DataSourceNameIDList _dataSourceNameIDList;
        //private Prana.PM.Common.SortableSearchableList<DataSourceNameID> _dataSourceNameIDListNew = new SortableSearchableList<DataSourceNameID>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CtrlSourceName"/> class.
        /// </summary>
        public CtrlSourceName()
        {
            InitializeComponent();
            //InitControl();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control can respond to user interaction.
        /// </summary>
        /// <value></value>
        /// <returns>true if the control can respond to user interaction; otherwise, false. The default is true.</returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public new bool Enabled
        {
            get { return cmbSourceName.Enabled; }
            set { cmbSourceName.Enabled = value; }
        }

        private bool _isAllDataSourceAvailable = false;

        /// <summary>
        /// Gets or sets a value indicating whether All data source item to be added in 
        /// combo or not.
        /// </summary>
        /// <value><c>true</c> if [add data source all]; otherwise, <c>false</c>.</value>
        public bool IsAllDataSourceAvailable
        {
            private get
            {
                return _isAllDataSourceAvailable;
            }
            set
            {
                _isAllDataSourceAvailable = value;
            }
        }

        private bool _isSelectItemRequired = true;

        /// <summary>
        /// Sets a value indicating whether this instance is select item required.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is select item required; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelectItemRequired
        {
            private get
            {
                return _isSelectItemRequired;
            }
            set
            {
                _isSelectItemRequired = value;
            }
        }


        public RowsCollection Items
        {
            get
            {
                return cmbSourceName.Rows;
            }
        }



        MapAccounts _mapAccounts = new MapAccounts();
        /// <summary>
        /// Clears the previous dataBinding, and then adds the new databinding to the value 
        /// propperty of the combo
        /// </summary>
        /// <param name="binding">The binding.</param>
        public void AddDataBindingForCombo(System.Windows.Forms.Binding binding)
        {
            //errProviderSourceName.DataSource = binding;
            //errProviderSourceName.DataMember = "DataSourceNameID.ID";
            _mapAccounts = (MapAccounts)binding.DataSource;
            bindingSourceMapAccounts.DataSource = _mapAccounts;

            cmbSourceName.DataBindings.Clear();
            cmbSourceName.DataBindings.Add(binding);
            //cmbSourceName.DataMember = "DataSourceNameID.ID";
            //cmbSourceName.DataSource = bindingSourceMapAccounts;
        }


        //private DataSourceNameID _dataSourceNameID = null;

        //public DataSourceNameID DataSourceNameID
        //{
        //    get 
        //    {
        //        if (_dataSourceNameID == null)
        //        {
        //            _dataSourceNameID.FullName = cmbSourceName.Text;
        //            _dataSourceNameID.ID = Convert.ToInt32(cmbSourceName.Value.ToString());
        //        }
        //        return _dataSourceNameID; 
        //    }
        //    set { _dataSourceNameID = value; }
        //}


        //private string _thirdPartyID;

        //public string ThirdPartyID
        //{
        //    get { return _thirdPartyID; }
        //    private set { _thirdPartyID = value; }
        //}

        //private string _dataSourceFullName;

        //public string DataSourceFullName
        //{
        //    get { return _dataSourceFullName; }
        //    private set { _dataSourceFullName = value; }
        //}

        /// <summary>
        /// Handles the Load event of the CtrlSourceName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CtrlSourceName_Load(object sender, EventArgs e)
        {
            //InitControl();
        }

        /// <summary>
        /// Inits the control.
        /// </summary>
        public void InitControl()
        {
            errProviderSourceName.DataSource = bindingSourceMapAccounts;
            errProviderSourceName.DataMember = "DataSourceNameID.ID";

            //_dataSourceNameIDList = DataSourceNameIDList.GetInstance().Retrieve;
            //_dataSourceNameIDListNew = DataSourceNameIDList.GetInstance().Retrieve;

            bindingSourceDataSourceNameIDList.DataSource = Prana.PM.DAL.DataSourceManager.RetrieveDataSourceNames(_isSelectItemRequired, _isAllDataSourceAvailable);

            //Sugandh - IF --Select-- Item is their in the list, select it (value(-1)), else is ALL Item is their select that(value(0))
            if (bool.Equals(this.IsSelectItemRequired, true))
            {
                cmbSourceName.Value = -1;
            }
            else if (bool.Equals(this.IsAllDataSourceAvailable, true))
            {
                cmbSourceName.Value = 0;
            }

            //cmbSourceName.Value = 0;
            Utils.UltraComboFilter(cmbSourceName, "FullName");
        }


        public event EventHandler SelectionChanged;

        /// <summary>
        /// Handles the ValueChanged event of the cmbSourceName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cmbSourceName_ValueChanged(object sender, EventArgs e)
        {
            if (cmbSourceName.SelectedRow == null || cmbSourceName.SelectedRow.ListObject == null)
                return;

            ThirdPartyNameID selectedDataSourceNameID = cmbSourceName.SelectedRow.ListObject as ThirdPartyNameID;
            if (selectedDataSourceNameID != null)
            {
                _mapAccounts.DataSourceNameID = selectedDataSourceNameID;
                bindingSourceMapAccounts.DataSource = _mapAccounts;

                DataSourceEventArgs eventArgs = new DataSourceEventArgs();
                eventArgs.DataSourceNameID = selectedDataSourceNameID;

                if (SelectionChanged != null)
                {
                    SelectionChanged(this, eventArgs);
                }

            }
        }

        /// <summary>
        /// Handles the CurrentChanged event of the bindingSourceDataSourceName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void bindingSourceDataSourceName_CurrentChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the DataSourceChanged event of the bindingSourceDataSourceName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void bindingSourceDataSourceName_DataSourceChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the CurrentItemChanged event of the bindingSourceDataSourceName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void bindingSourceDataSourceName_CurrentItemChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the BindingComplete event of the bindingSourceDataSourceName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.BindingCompleteEventArgs"/> instance containing the event data.</param>
        private void bindingSourceDataSourceName_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            int value = Convert.ToInt32(cmbSourceName.Value);
        }

        /// <summary>
        /// Handles the CurrentChanged event of the bindingSourceDataSourceNameIDList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing
        /// the event data.</param>
        private void bindingSourceDataSourceNameIDList_CurrentChanged(object sender, EventArgs e)
        {
            string v = string.Empty;
        }

        /// <summary>
        /// Handles the BindingComplete event of the bindingSourceDataSourceNameIDList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.BindingCompleteEventArgs"/> instance containing the event data.</param>
        private void bindingSourceDataSourceNameIDList_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            string v = string.Empty;
        }



    }
}
