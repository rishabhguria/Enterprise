using Prana.Admin.RoutingLogic.MisclFunctions;
using System;
using System.Collections;
using System.Windows.Forms;


namespace Prana.Admin.RoutingLogic.Controls
{

    /// <summary>
    /// Summary description for AUEC.
    /// </summary>
    public class AUEC : System.Windows.Forms.UserControl
    {
        private Infragistics.Win.Misc.UltraLabel labelAsset;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbedtAsset;
        private Infragistics.Win.Misc.UltraLabel labelUnderLying;
        private Infragistics.Win.Misc.UltraLabel labelExchange;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbedtUnderLying;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbedtExchange;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Data.DataSet dsData; private BLL.DataRoutingLogicObjects dataRL;
        private string strMemoryID;
        private string strTabID;
        //		private string strLastCalledBy="";

        public AUEC()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            this.cmbedtAsset.Leave += new System.EventHandler(Functions.object_LostFocus);
            this.cmbedtAsset.Enter += new System.EventHandler(Functions.object_GotFocus);
            this.cmbedtUnderLying.Leave += new System.EventHandler(Functions.object_LostFocus);
            this.cmbedtUnderLying.Enter += new System.EventHandler(Functions.object_GotFocus);
            this.cmbedtExchange.Leave += new System.EventHandler(Functions.object_LostFocus);
            this.cmbedtExchange.Enter += new System.EventHandler(Functions.object_GotFocus);

        }

        //		public AUEC(System.Data.DataSet _dsData)
        //		{
        //			dsData=_dsData;
        //			// This call is required by the Windows.Forms Form Designer.
        //			InitializeComponent();
        //
        //			// TODO: Add any initialization after the InitializeComponent call
        //
        //
        //		}

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (labelAsset != null)
                {
                    labelAsset.Dispose();
                }
                if (cmbedtAsset != null)
                {
                    cmbedtAsset.Dispose();
                }
                if (labelUnderLying != null)
                {
                    labelUnderLying.Dispose();
                }
                if (labelExchange != null)
                {
                    labelExchange.Dispose();
                }
                if (cmbedtUnderLying != null)
                {
                    cmbedtUnderLying.Dispose();
                }
                if (cmbedtExchange != null)
                {
                    cmbedtExchange.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.labelAsset = new Infragistics.Win.Misc.UltraLabel();
            this.cmbedtAsset = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.labelUnderLying = new Infragistics.Win.Misc.UltraLabel();
            this.labelExchange = new Infragistics.Win.Misc.UltraLabel();
            this.cmbedtUnderLying = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbedtExchange = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtAsset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtUnderLying)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtExchange)).BeginInit();
            this.SuspendLayout();
            // 
            // labelAsset
            // 
            appearance1.ForeColorDisabled = System.Drawing.Color.Black;
            this.labelAsset.Appearance = appearance1;
            this.labelAsset.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelAsset.Location = new System.Drawing.Point(2, 2);
            this.labelAsset.Name = "labelAsset";
            this.labelAsset.Size = new System.Drawing.Size(32, 16);
            this.labelAsset.TabIndex = 1;
            this.labelAsset.Text = "Asset";
            // 
            // cmbedtAsset
            // 
            appearance2.ForeColorDisabled = System.Drawing.Color.Black;
            this.cmbedtAsset.Appearance = appearance2;
            this.cmbedtAsset.DropDownListWidth = 150;
            this.cmbedtAsset.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbedtAsset.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbedtAsset.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.cmbedtAsset.Location = new System.Drawing.Point(37, 1);
            this.cmbedtAsset.Name = "cmbedtAsset";
            this.cmbedtAsset.Size = new System.Drawing.Size(104, 20);
            this.cmbedtAsset.TabIndex = 1;
            this.cmbedtAsset.Tag = "cmbedtAsset";
            this.cmbedtAsset.SelectionChangeCommitted += new System.EventHandler(this.SelectedAsset);
            // 
            // labelUnderLying
            // 
            appearance3.ForeColorDisabled = System.Drawing.Color.Black;
            this.labelUnderLying.Appearance = appearance3;
            this.labelUnderLying.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.labelUnderLying.Location = new System.Drawing.Point(147, 2);
            this.labelUnderLying.Name = "labelUnderLying";
            this.labelUnderLying.Size = new System.Drawing.Size(61, 16);
            this.labelUnderLying.TabIndex = 2;
            this.labelUnderLying.Text = "UnderLying";
            // 
            // labelExchange
            // 
            appearance4.ForeColorDisabled = System.Drawing.Color.Black;
            this.labelExchange.Appearance = appearance4;
            this.labelExchange.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.labelExchange.Location = new System.Drawing.Point(322, 2);
            this.labelExchange.Name = "labelExchange";
            this.labelExchange.Size = new System.Drawing.Size(52, 16);
            this.labelExchange.TabIndex = 3;
            this.labelExchange.Text = "Exchange";
            // 
            // cmbedtUnderLying
            // 
            appearance5.ForeColorDisabled = System.Drawing.Color.Black;
            this.cmbedtUnderLying.Appearance = appearance5;
            this.cmbedtUnderLying.DropDownListWidth = 150;
            this.cmbedtUnderLying.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbedtUnderLying.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbedtUnderLying.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.cmbedtUnderLying.Location = new System.Drawing.Point(212, 1);
            this.cmbedtUnderLying.Name = "cmbedtUnderLying";
            this.cmbedtUnderLying.Size = new System.Drawing.Size(104, 20);
            this.cmbedtUnderLying.TabIndex = 2;
            this.cmbedtUnderLying.Tag = "cmbedtUnderLying";
            this.cmbedtUnderLying.SelectionChangeCommitted += new System.EventHandler(this.SelectedUnderLying);
            // 
            // cmbedtExchange
            // 
            appearance6.ForeColorDisabled = System.Drawing.Color.Black;
            this.cmbedtExchange.Appearance = appearance6;
            this.cmbedtExchange.DropDownListWidth = 150;
            this.cmbedtExchange.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbedtExchange.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbedtExchange.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.cmbedtExchange.Location = new System.Drawing.Point(377, 1);
            this.cmbedtExchange.Name = "cmbedtExchange";
            this.cmbedtExchange.Size = new System.Drawing.Size(104, 20);
            this.cmbedtExchange.TabIndex = 3;
            this.cmbedtExchange.Tag = "cmbedtExchange";
            this.cmbedtExchange.SelectionChangeCommitted += new System.EventHandler(this.SelectedExchange);
            // 
            // AUEC
            // 
            this.Controls.Add(this.cmbedtExchange);
            this.Controls.Add(this.cmbedtUnderLying);
            this.Controls.Add(this.labelExchange);
            this.Controls.Add(this.labelUnderLying);
            this.Controls.Add(this.cmbedtAsset);
            this.Controls.Add(this.labelAsset);
            this.Name = "AUEC";
            this.Size = new System.Drawing.Size(484, 24);
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtAsset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtUnderLying)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtExchange)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Load Data
        public void LoadData(ref System.Data.DataSet _dsData, ref BLL.DataRoutingLogicObjects _dataRL, string _strMemoryID, string _strTabID)
        {
            this.dsData = _dsData;
            this.dataRL = _dataRL;
            this.strMemoryID = _strMemoryID;
            this.strTabID = _strTabID;
            //this.txtedtName.Text = this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["RLName"].ToString();
            SelectedAsset(this.cmbedtUnderLying, null);




        }
        #endregion

        #region event for Asset-selection 
        private void SelectedAsset(object sender, System.EventArgs e)
        {
            //			bool _bSelectedEqualsMemory = this.cmbedtAsset.SelectedItem.DataValue.Equals(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AssetID"].ToString());
            //			bool _bSelectedNull=this.cmbedtAsset.SelectedItem.Equals(null);
            //			
            //			if(_bSelectedEqualsMemory)
            //			{
            //				return;
            //			}
            //			if(_bSelectedNull)
            //			{
            //				
            //
            //
            //			}
            //bool _bCalledByConstructor= ((((Infragistics.Win.UltraWinEditors.UltraComboEditor) sender).Tag==null));
            //			bool _bCalledItself= false;//this.strLastCalledBy.Equals("Asset");
            //			if(_bCalledItself )//|| _bCalledByConstructor)
            //			{
            //				return;
            //			}
            //			this.strLastCalledBy="Asset";


            bool _bCalledByUser = ((Infragistics.Win.UltraWinEditors.UltraComboEditor)sender).Tag.ToString().Equals("cmbedtAsset");
            if (_bCalledByUser)
            {
                //				this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AssetID"]=int.Parse(this.cmbedtAsset.SelectedItem.DataValue.ToString());
                //				this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AssetName"]=this.cmbedtAsset.SelectedItem.DisplayText;

                this.dataRL.AssetID(strTabID, int.Parse(this.cmbedtAsset.SelectedItem.DataValue.ToString()));

            }
            else
            {
                this.cmbedtAsset.SelectionChangeCommitted -= new System.EventHandler(this.SelectedAsset);
                this.cmbedtAsset.Items.Clear();
                System.Collections.ArrayList _alAssetID = new ArrayList();
                //=new DataRowCollection();
                //				_drcFoundRows.Add(new System.Data.DataRow);
                bool _bRowIsThere;
                bool _bListContainsMemory = false;
                string _strAssetID = "";
                foreach (System.Data.DataRow _drRowSelect in this.dsData.Tables["dtAUEC"].Select())
                {
                    _bRowIsThere = false;
                    _strAssetID = _drRowSelect["AssetID"].ToString();

                    _bRowIsThere = _alAssetID.Contains(_strAssetID);

                    if (!_bRowIsThere)
                    {
                        //						if(_strAssetID.Equals(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AssetID"].ToString()))
                        //						{
                        //							_bListContainsMemory=true;
                        //						}

                        if (Convert.ToInt32(_strAssetID) == this.dataRL.AssetID(strTabID))//_strAssetID.Equals(this.dataRL.AssetID(strTabID)))
                        {
                            _bListContainsMemory = true;
                        }


                        _alAssetID.Add(_strAssetID);
                        this.cmbedtAsset.Items.Add(int.Parse(_strAssetID), _drRowSelect["AssetName"].ToString());
                    }
                }

                if (this.cmbedtAsset.Items.Count == 0)
                {
                    MessageBox.Show(" no asset added to list, no data downloaded. ");
                    return;
                }



                if (_bListContainsMemory)
                {
                    //					this.cmbedtAsset.Value=int.Parse(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AssetID"].ToString());

                    this.cmbedtAsset.Value = this.dataRL.AssetID(strTabID);
                }
                else
                {
                    //					MessageBox.Show("selection not in new list of asset, selecting 1st value");
                    this.cmbedtAsset.SelectedItem = (Infragistics.Win.ValueListItem)this.cmbedtAsset.Items.GetItem(0);

                    //					this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AssetID"]=int.Parse(this.cmbedtAsset.SelectedItem.DataValue.ToString());
                    //					this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AssetName"]=this.cmbedtAsset.SelectedItem.DisplayText;

                    this.dataRL.AssetID(strTabID, int.Parse(this.cmbedtAsset.SelectedItem.DataValue.ToString()));

                    //								((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID,iRoutingIndex, true);
                }

                this.cmbedtAsset.SelectionChangeCommitted += new System.EventHandler(this.SelectedAsset);

                //GetClients method fetches the existing currencies from the database.
                //Prana.Admin.BLL.CompanyClient clients = CompanyManager.GetCompanyClient(_companyID);
                //Inserting the - Select - option in the Combo Box at the top.
                // clients.Insert(0, new CompanyClients(Functions.MinValue,C_COMBO_SELECT));
                //				this.cmbedtAsset.DataBindings.Add("Value",_drcFoundRows,"AssetName");
                //this.cmbedtAsset.d = _drcFoundRows;
                //this.cmbedtAsset.DisplayMember = "AssetName";
                //this.cmbedtAsset.ValueMember = "AssetID";
                //				this.cmbedtAsset.Value=int.Parse(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AssetID"].ToString());

            }

            if (this.cmbedtAsset.SelectedItem.DataValue == null || int.Parse(this.cmbedtAsset.SelectedItem.DataValue.ToString()) < 0)
            {
                return;
            }

            this.SelectedUnderLying(this.cmbedtAsset, null);


            //DataCascade();		
            //this.strLastCalledBy="";

        }
        #endregion


        #region event for UnderLying-selection 
        private void SelectedUnderLying(object sender, System.EventArgs e)
        {
            bool _bCalledByUser = ((Infragistics.Win.UltraWinEditors.UltraComboEditor)sender).Tag.ToString().Equals("cmbedtUnderLying");
            if (_bCalledByUser)
            {
                //				this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["UnderLyingID"]=int.Parse(this.cmbedtUnderLying.SelectedItem.DataValue.ToString());
                //				this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["UnderLyingName"]=this.cmbedtUnderLying.SelectedItem.DisplayText;

                this.dataRL.UnderLyingID(strTabID, int.Parse(this.cmbedtUnderLying.SelectedItem.DataValue.ToString()));
            }
            else
            {
                this.cmbedtUnderLying.SelectionChangeCommitted -= new System.EventHandler(this.SelectedUnderLying);
                this.cmbedtUnderLying.Items.Clear();
                System.Collections.ArrayList _alUnderLyingID = new ArrayList();

                bool _bRowIsThere;
                bool _bListContainsMemory = false;
                string _strUnderLyingID = "";
                foreach (System.Data.DataRow _drRowSelect in this.dsData.Tables["dtAUEC"].Select("AssetID = " + this.cmbedtAsset.SelectedItem.DataValue.ToString()))
                {
                    _bRowIsThere = false;
                    _strUnderLyingID = _drRowSelect["UnderLyingID"].ToString();

                    _bRowIsThere = _alUnderLyingID.Contains(_strUnderLyingID);

                    if (!_bRowIsThere)
                    {
                        //						if(_strUnderLyingID.Equals(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["UnderLyingID"].ToString()))
                        //						{
                        //							_bListContainsMemory=true;
                        //						}
                        if (Convert.ToInt32(_strUnderLyingID) == this.dataRL.UnderLyingID(strTabID))//_strUnderLyingID.Equals(this.dataRL.UnderLyingID(strTabID)))
                        {
                            _bListContainsMemory = true;
                        }


                        _alUnderLyingID.Add(_strUnderLyingID);
                        this.cmbedtUnderLying.Items.Add(int.Parse(_strUnderLyingID), _drRowSelect["UnderLyingName"].ToString());
                    }
                }


                if (_bListContainsMemory)
                {
                    //					this.cmbedtUnderLying.Value=int.Parse(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["UnderLyingID"].ToString());

                    this.cmbedtUnderLying.Value = this.dataRL.UnderLyingID(strTabID);
                }
                else
                {
                    //					MessageBox.Show("selection not in new list of underlying, selecting 1st value");
                    this.cmbedtUnderLying.SelectedItem = (Infragistics.Win.ValueListItem)this.cmbedtUnderLying.Items.GetItem(0);

                    //					this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["UnderLyingID"]=int.Parse(this.cmbedtUnderLying.SelectedItem.DataValue.ToString());
                    //					this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["UnderLyingName"]=this.cmbedtUnderLying.SelectedItem.DisplayText;

                    this.dataRL.UnderLyingID(strTabID, int.Parse(this.cmbedtUnderLying.SelectedItem.DataValue.ToString()));

                }

                this.cmbedtUnderLying.SelectionChangeCommitted += new System.EventHandler(this.SelectedUnderLying);
            }
            if (this.cmbedtUnderLying.SelectedItem.DataValue == null || int.Parse(this.cmbedtUnderLying.SelectedItem.DataValue.ToString()) < 0)
            {
                return;
            }

            this.SelectedExchange(this.cmbedtUnderLying, null);

        }
        #endregion

        #region event for Exchange-selection 

        private void SelectedExchange(object sender, System.EventArgs e)
        {
            bool _bCalledByUser = ((Infragistics.Win.UltraWinEditors.UltraComboEditor)sender).Tag.ToString().Equals("cmbedtExchange");
            if (_bCalledByUser)
            {
                //				this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AUECExchangeID"]=int.Parse(this.cmbedtExchange.SelectedItem.DataValue.ToString());
                //				this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ExchangeName"]=this.cmbedtExchange.SelectedItem.DisplayText;

                this.dataRL.ExchangeID(strTabID, int.Parse(this.cmbedtExchange.SelectedItem.DataValue.ToString()));
            }
            else
            {
                this.cmbedtExchange.SelectionChangeCommitted -= new System.EventHandler(this.SelectedExchange);
                this.cmbedtExchange.Items.Clear();
                System.Collections.ArrayList _alAUECExchangeID = new ArrayList();

                bool _bRowIsThere;
                bool _bListContainsMemory = false;
                string _strAUECExchangeID = "";
                foreach (System.Data.DataRow _drRowSelect in this.dsData.Tables["dtAUEC"].Select("AssetID = " + this.cmbedtAsset.SelectedItem.DataValue.ToString() + " AND UnderLyingID = " + this.cmbedtUnderLying.SelectedItem.DataValue.ToString()))
                {
                    _bRowIsThere = false;
                    _strAUECExchangeID = _drRowSelect["AUECExchangeID"].ToString();

                    _bRowIsThere = _alAUECExchangeID.Contains(_strAUECExchangeID);

                    if (!_bRowIsThere)
                    {
                        //						if(_strAUECExchangeID.Equals(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AUECExchangeID"].ToString()))
                        //						{
                        //							_bListContainsMemory=true;
                        //						}
                        if (Convert.ToInt32(_strAUECExchangeID) == this.dataRL.ExchangeID(strTabID))//_strAUECExchangeID.Equals(this.dataRL.ExchangeID(strTabID)))
                        {
                            _bListContainsMemory = true;
                        }

                        _alAUECExchangeID.Add(_strAUECExchangeID);
                        this.cmbedtExchange.Items.Add(int.Parse(_strAUECExchangeID), _drRowSelect["ExchangeName"].ToString());
                    }
                }


                if (_bListContainsMemory)
                {
                    //					this.cmbedtExchange.Value=int.Parse(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AUECExchangeID"].ToString());

                    this.cmbedtExchange.Value = this.dataRL.ExchangeID(strTabID);
                }
                else
                {
                    //					MessageBox.Show("selection not in new list of exchange, selecting 1st value");
                    this.cmbedtExchange.SelectedItem = (Infragistics.Win.ValueListItem)this.cmbedtExchange.Items.GetItem(0);

                    //					this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AUECExchangeID"]=int.Parse(this.cmbedtExchange.SelectedItem.DataValue.ToString());
                    //					this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ExchangeName"]=this.cmbedtExchange.SelectedItem.DisplayText;

                    this.dataRL.ExchangeID(strTabID, int.Parse(this.cmbedtExchange.SelectedItem.DataValue.ToString()));
                }


                this.cmbedtExchange.SelectionChangeCommitted += new System.EventHandler(this.SelectedExchange);
            }

            string _strSelect = "AssetID = " + this.cmbedtAsset.SelectedItem.DataValue.ToString() + " AND UnderLyingID = " + this.cmbedtUnderLying.SelectedItem.DataValue.ToString() + " AND AUECExchangeID = " + this.cmbedtExchange.SelectedItem.DataValue.ToString();
            System.Data.DataRow[] _drRow = this.dsData.Tables["dtAUEC"].Select(_strSelect);

            //				this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AUECID"]=_drRow[0]["AUECID"].ToString();

            this.dataRL.AUECID(strTabID, int.Parse(_drRow[0]["AUECID"].ToString()));

            //				this.UpdateParameterExchangeAUECID()



            if (this.cmbedtExchange.SelectedItem.DataValue == null || int.Parse(this.cmbedtExchange.SelectedItem.DataValue.ToString()) < 0)
            {
                return;
            }


            if (this.Parent.Parent.GetType().ToString().Trim().Equals("Prana.Admin.RoutingLogic.Controls.RLogic"))
            {
                ((Prana.Admin.RoutingLogic.Controls.RLogic)(this.Parent.Parent)).DelegateLoadData();
            }
            else
            {
                ((Prana.Admin.RoutingLogic.Controls.Client)(this.Parent)).DelegateLoadData();
            }

            //				MessageBox.Show(this.Parent.GetType().ToString());
            //				Type type = this.Parent.GetType().UnderlyingSystemType ;
            //				((type)this.Parent).DelegateLoadData(ref dsData, ref dataRL);
            //				((Controls.RLogic)(this.Parent)).DelegateLoadData(ref dsData, ref dataRL);
            //				object obj = this.Parent;
            //				System.Collections.IEnumerator enu = ((System.Windows.Forms.UserControl)obj).Controls.GetEnumerator();
            //				while(true)
            //				{
            //					MessageBox.Show(enu.GetType().Name + "   - "+enu.Current.ToString());
            //					if(!enu.MoveNext())
            //						break;
            //
            //
            //				}

            //	this.SelectedExchange(this.cmbedtExchange,null);		

        }
        #endregion


        //		#region cascase of data events
        //		private void DataCascade()
        //		{			
        //		//	this.ucAuec.LoadData(ref dsData, ref dataRL);
        //			//this.ucIfThenCondition[0].LoadData(ref dsData, ref dataRL);
        //
        //			
        //
        //		
        //		}
        //		#endregion

        //		#region UpdateParameterExchangeAUECID()
        //
        //		private void UpdateParameterExchangeAUECID()
        //		{
        //			foreach( System.Data.DataRow _row   in this.dsData.Tables["dtParameter"].Select("ParamName = Exchange"))
        //			{
        //				this.dsData.Tables["dtParameter"].Columns[""].Table.
        //					this.dsData.Tables[""].LoadDataRow(
        //
        //
        //			}
        //
        //
        //		}
        //
        //		#endregion

    }




}
