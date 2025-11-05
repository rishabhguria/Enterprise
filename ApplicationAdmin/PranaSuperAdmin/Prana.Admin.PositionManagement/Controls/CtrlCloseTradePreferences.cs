using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Nirvana.Admin.PositionManagement.BusinessObjects;
using Nirvana.Admin.PositionManagement.Classes;
using Infragistics.Win.UltraWinGrid;


namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlCloseTradePreferences : UserControl
    {
        BindingSource _formBindingSource = new BindingSource();
        private CloseTradePreferences _closeTradePreferences = new CloseTradePreferences();

        public CtrlCloseTradePreferences()
        {
            InitializeComponent();
        }

        #region Initialize the control
        private bool _isInitialized = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [combo default methodology enabled].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [combo default methodology enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool ComboDefaultMethodologyEnabled
        {
            get { return cmbDefaultMethodology.Enabled; }
            set { cmbDefaultMethodology.Enabled = value; }
        }

        /// <summary>
        /// Gets the default methodology.
        /// </summary>
        /// <value>The default methodology.</value>
        public CloseTradeMethodology DefaultMethodology
        {
            get { return (CloseTradeMethodology) cmbDefaultMethodology.Value; }
            //set { cmbDefaultMethodology.Value = value; }
        }

        /// <summary>
        /// Gets the alogrithm.
        /// </summary>
        /// <value>The alogrithm.</value>
        public CloseTradeAlogrithm Alogrithm
        {
            get { return (CloseTradeAlogrithm) cmbAlgorithm.Value; }
            //set { _alogrithm = value; }
        }



        /// <summary>
        /// Gets the trade date.
        /// </summary>
        /// <value>The trade date.</value>
        public DateTime TradeDate
        {
            get { return (DateTime) cmbTradeDate.Value; }

        }
	
	
        /// <summary>
        /// Initialize the control.
        /// </summary>
        public void InitControl()
        {
            if (!_isInitialized)
            {
                ctrlSourceName1.IsSelectItemRequired = true;
                ctrlSourceName1.IsAllDataSourceAvailable = false;
                ctrlSourceName1.InitControl();
                SetupBinding();
                _isInitialized = true;
            }
        }

        public void InitControl(CloseTradePreferences closeTradePref)
        {
            if (!_isInitialized)
            {
                ctrlSourceName1.IsSelectItemRequired = true;
                ctrlSourceName1.IsAllDataSourceAvailable = false;
                ctrlSourceName1.InitControl();

                _closeTradePreferences = closeTradePref;

                SetupBinding();
                _isInitialized = true;
            }
           
        }
        #endregion

        /// <summary>
        /// Setups the binding.
        /// </summary>
        private void SetupBinding()
        {
            
             
            //_formBindingSource.DataSource = RetrieveCloseTradePreferences(dataSourceNameID);
            _formBindingSource.DataSource = _closeTradePreferences;

            BindComboAndListBoxes();

            Binding ctrlSourceNameBinding = new System.Windows.Forms.Binding("Value", _formBindingSource, "DataSourceNameID.ID", true);
            ctrlSourceName1.DataBindings.Clear();
            ctrlSourceName1.AddDataBindingForCombo(ctrlSourceNameBinding);

            

            //create a binding object
            Binding assetBinding = new System.Windows.Forms.Binding("SelectedValue", _formBindingSource, "Asset.ID");
            //add new binding
            lstAsset.DataBindings.Clear();
            lstAsset.DataBindings.Add(assetBinding);
            
            //cmbAsset.DataBindings.Clear();
            //cmbAsset.DataBindings.Add(assetBinding);

            //ListBox.SelectedObjectCollection selected = new ListBox.SelectedObjectCollection(lstFunds);

            //ListBox.ObjectCollection all = new ListBox.ObjectCollection(lstFunds);

            //foreach (Fund fund in _closeTradePreferences.Funds)
            //{
            //    if (!selected.Contains(fund))
            //    {
            //        selected.Add(fund);
            //    }
                
            //}

            for (int counter = 0; counter < lstFunds.Items.Count; counter++)
            {
                Fund currentfund = (Fund)lstFunds.Items[counter];
                foreach (Fund fund in _closeTradePreferences.Funds)
                {
                    if (currentfund.ID.Equals(fund.ID))
                    {
                        lstFunds.SelectedValue = currentfund.ID;
                    }
                }
            }


            for (int counter = 0; counter < lstUnderlying.Items.Count; counter++)
            {
                Underlying currentUnderlying = (Underlying)lstUnderlying.Items[counter];
                foreach (Underlying underlying in _closeTradePreferences.Underlyings)
                {
                    if (currentUnderlying.ID.Equals(underlying.ID))
                    {
                        lstUnderlying.SelectedValue = currentUnderlying.ID;
                    }
                }
            }


            //for (int counter = 0; counter < lstExchange.Items.Count; counter++)
            //{
            //    Exchange currentExchange = (Exchange)lstExchange.Items[counter];
            //    foreach (Exchange exchange in _closeTradePreferences.Exchanges)
            //    {
            //        if (currentExchange.ID.Equals(exchange.ID))
            //        {
            //            lstExchange.SelectedValue = currentExchange.ID;
            //        }
            //    }
            //}




            //Binding underlyingBinding = new System.Windows.Forms.Binding("SelectedItems", _closeTradePreferences, "Underlyings");
            //lstUnderlying.DataBindings.Add(underlyingBinding);

            Binding exchangeBinding = new System.Windows.Forms.Binding("SelectedValue", _closeTradePreferences, "Exchanges.ID");
            lstExchange.DataBindings.Add(exchangeBinding);

            Binding algorithmBinding = new System.Windows.Forms.Binding("Value", _formBindingSource, "Algorithm");
            cmbAlgorithm.DataBindings.Clear();
            cmbAlgorithm.DataBindings.Add(algorithmBinding);

            //Binding fundBinding = new System.Windows.Forms.Binding("SelectedItem", _closeTradePreferences, "Funds");
            //lstFunds.DataBindings.Add(fundBinding);

            Binding defaultMethodologyBinding = new System.Windows.Forms.Binding("Value", _formBindingSource, "DefaultMethodology");
            cmbDefaultMethodology.DataBindings.Clear();
            cmbDefaultMethodology.DataBindings.Add(defaultMethodologyBinding);
        }

       

        private void BindComboAndListBoxes()
        {
            lstAsset.DisplayMember = "Name";
            lstAsset.ValueMember = "ID";
            //lstAsset.DataSource = GetAssets();

            lstUnderlying.DisplayMember = "Name";
            lstUnderlying.ValueMember = "ID";
            //lstUnderlying.DataSource = _closeTradePreferences.Underlyings;


            lstExchange.DisplayMember = "Name";
            lstExchange.ValueMember = "ID";
            //lstExchange.DataSource = _closeTradePreferences.Exchanges;


            lstFunds.DisplayMember = "Name";
            lstFunds.ValueMember = "ID";
            //lstFunds.DataSource = _closeTradePreferences.Funds;

            cmbAlgorithm.DisplayMember = "DisplayText";
            cmbAlgorithm.ValueMember = "Value";
            cmbAlgorithm.DataSource = EnumHelper.ConvertEnumForBinding(typeof(CloseTradeAlogrithm));
            Utils.UltraComboFilter(cmbAlgorithm, "DisplayText");

            cmbDefaultMethodology.DisplayMember = "DisplayText";
            cmbDefaultMethodology.ValueMember = "Value";
            cmbDefaultMethodology.DataSource = EnumHelper.ConvertEnumForBinding(typeof(CloseTradeMethodology));
            Utils.UltraComboFilter(cmbDefaultMethodology, "DisplayText");
        }



        private CloseTradePreferences RetrieveCloseTradePreferences(DataSourceNameID dataSourceNameID)
        {
            _closeTradePreferences.DataSourceNameID = dataSourceNameID;

            if (dataSourceNameID.ID == 1)
            {
                lstAsset.DataSource = GetAssets();
                lstUnderlying.DataSource = GetUnderlyings();
                lstExchange.DataSource = GetExchanges();
                lstFunds.DataSource = GetFunds();

                _closeTradePreferences.Asset.ID = 1;
                _closeTradePreferences.Asset.Name = "Equity";
                _closeTradePreferences.Underlyings = GetUnderlyings();
                _closeTradePreferences.Exchanges = GetExchanges();
                _closeTradePreferences.Funds = GetFunds();
                _closeTradePreferences.DefaultMethodology = CloseTradeMethodology.Manual;
                _closeTradePreferences.Algorithm = CloseTradeAlogrithm.None;
            }
            if (dataSourceNameID.ID == 32)
            {
                lstAsset.DataSource = GetCurrentAssets();
                lstUnderlying.DataSource = GetCurrentUnderlyings();
                lstExchange.DataSource = GetCurrentExchanges();
                lstFunds.DataSource = GetCurrentFunds();

                _closeTradePreferences.Asset.ID = 2;
                _closeTradePreferences.Asset.Name = "Options";
                _closeTradePreferences.Underlyings = GetCurrentUnderlyings();
                _closeTradePreferences.Exchanges = GetCurrentExchanges();
                _closeTradePreferences.Funds = GetCurrentFunds();
                _closeTradePreferences.DefaultMethodology = CloseTradeMethodology.Automatic;
                _closeTradePreferences.Algorithm = CloseTradeAlogrithm.FIFO;
            }
            

            return _closeTradePreferences;
        }

        private SortableSearchableList<Exchange> GetCurrentExchanges()
        {
            SortableSearchableList<Exchange> list = new SortableSearchableList<Exchange>();

            list.Add(new Exchange(1, "NYSE"));
            list.Add(new Exchange(2, "LSE"));

            return list;
        }

        private SortableSearchableList<Underlying> GetCurrentUnderlyings()
        {

            SortableSearchableList<Underlying> list = new SortableSearchableList<Underlying>();

            list.Add(new Underlying(2, "US Options"));
            list.Add(new Underlying(3, "US Futures"));

            return list;
        }

        private SortableSearchableList<Fund> GetCurrentFunds()
        {

            SortableSearchableList<Fund> list = new SortableSearchableList<Fund>();

            list.Add(new Fund(2, "Fund Y"));
            list.Add(new Fund(3, "Fund Z"));

            return list;
        }

        private SortableSearchableList<Asset> GetAssets()
        {
            SortableSearchableList<Asset> list = new SortableSearchableList<Asset>();

            list.Add(new Asset(1, "Equity"));
            list.Add(new Asset(2, "Options"));
            list.Add(new Asset(3, "Futures"));

            return list;
        }

        private SortableSearchableList<Asset> GetCurrentAssets()
        {
            SortableSearchableList<Asset> list = new SortableSearchableList<Asset>();

            list.Add(new Asset(1, "Equity"));
            list.Add(new Asset(3, "Futures"));

            return list;
        }
        private SortableSearchableList<Underlying> GetUnderlyings()
        {
            SortableSearchableList<Underlying> list = new SortableSearchableList<Underlying>();

            list.Add(new Underlying(1, "US Equity"));
            list.Add(new Underlying(2, "US Options"));
            list.Add(new Underlying(3, "US Futures"));

            return list;
        }

        private SortableSearchableList<Exchange> GetExchanges()
        {
            SortableSearchableList<Exchange> list = new SortableSearchableList<Exchange>();

            list.Add(new Exchange(1, "NYSE"));
            list.Add(new Exchange(2, "LSE"));
            list.Add(new Exchange(3, "TSE"));

            return list;
        }

        private SortableSearchableList<Fund> GetFunds()
        {
            SortableSearchableList<Fund> list = new SortableSearchableList<Fund>();
         
            list.Add(new Fund(1, "Fund X"));
            list.Add(new Fund(2, "Fund Y"));
            list.Add(new Fund(3, "Fund Z"));

            return list;
        }

        private void ctrlSourceName1_SelectionChanged(object sender, EventArgs e)
        {
            DataSourceNameID changedDataSourceNameID = ((DataSourceEventArgs)e).DataSourceNameID;
            if (_formBindingSource.List.Count > 0)
            {
                _closeTradePreferences = _formBindingSource.List[0] as CloseTradePreferences;
            }

            _closeTradePreferences = RetrieveCloseTradePreferences(changedDataSourceNameID);
            

            //Reset datasource bindings
            _formBindingSource.ResetBindings(false);
        }

    }
}
