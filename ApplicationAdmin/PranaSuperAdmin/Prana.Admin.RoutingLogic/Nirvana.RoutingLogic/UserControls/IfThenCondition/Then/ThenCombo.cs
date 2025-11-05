using Prana.Admin.RoutingLogic.MisclFunctions;
using System;
using System.Collections;
using System.Windows.Forms;

namespace Prana.Admin.RoutingLogic.Controls
{
    /// <summary>
    /// Summary description for ThenCombo.
    /// </summary>
    public class ThenCombo : System.Windows.Forms.UserControl
    {
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbedtTradeAcc;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbedtCounterParty;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbedtVenue;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Data.DataSet dsData; private BLL.DataRoutingLogicObjects dataRL;
        private string strMemoryID;
        private string strIndex;
        private string strTabID;
        private int iRoutingLogic;


        public ThenCombo()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();



            // TODO: Add any initialization after the InitializeComponent call
            this.cmbedtTradeAcc.Leave += new System.EventHandler(Functions.object_LostFocus);
            this.cmbedtTradeAcc.Enter += new System.EventHandler(Functions.object_GotFocus);
            this.cmbedtCounterParty.Leave += new System.EventHandler(Functions.object_LostFocus);
            this.cmbedtCounterParty.Enter += new System.EventHandler(Functions.object_GotFocus);
            this.cmbedtVenue.Leave += new System.EventHandler(Functions.object_LostFocus);
            this.cmbedtVenue.Enter += new System.EventHandler(Functions.object_GotFocus);

        }

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
                if (cmbedtVenue != null)
                {
                    cmbedtVenue.Dispose();
                }
                if (cmbedtTradeAcc != null)
                {
                    cmbedtTradeAcc.Dispose();
                }
                if (cmbedtCounterParty != null)
                {
                    cmbedtCounterParty.Dispose();
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
            this.cmbedtTradeAcc = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbedtCounterParty = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbedtVenue = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtTradeAcc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtCounterParty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtVenue)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbedtTradeAcc
            // 
            this.cmbedtTradeAcc.DropDownListWidth = 150;
            this.cmbedtTradeAcc.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbedtTradeAcc.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbedtTradeAcc.Location = new System.Drawing.Point(2, 1);
            this.cmbedtTradeAcc.Name = "cmbedtTradeAcc";
            this.cmbedtTradeAcc.Size = new System.Drawing.Size(88, 20);
            this.cmbedtTradeAcc.TabIndex = 0;
            this.cmbedtTradeAcc.ValueChanged += new System.EventHandler(this.SelectTrading);
            // 
            // cmbedtCounterParty
            // 
            this.cmbedtCounterParty.DropDownListWidth = 150;
            this.cmbedtCounterParty.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbedtCounterParty.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbedtCounterParty.Location = new System.Drawing.Point(118, 1);
            this.cmbedtCounterParty.Name = "cmbedtCounterParty";
            this.cmbedtCounterParty.Size = new System.Drawing.Size(88, 20);
            this.cmbedtCounterParty.TabIndex = 1;
            this.cmbedtCounterParty.SelectionChangeCommitted += new System.EventHandler(this.SelectCounterParty);
            // 
            // cmbedtVenue
            // 
            this.cmbedtVenue.DropDownListWidth = 150;
            this.cmbedtVenue.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbedtVenue.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbedtVenue.Location = new System.Drawing.Point(206, 1);
            this.cmbedtVenue.Name = "cmbedtVenue";
            this.cmbedtVenue.Size = new System.Drawing.Size(88, 20);
            this.cmbedtVenue.TabIndex = 2;
            this.cmbedtVenue.SelectionChangeCommitted += new System.EventHandler(this.SelectVenue);
            // 
            // ThenCombo
            // 
            this.Controls.Add(this.cmbedtVenue);
            this.Controls.Add(this.cmbedtCounterParty);
            this.Controls.Add(this.cmbedtTradeAcc);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.Name = "ThenCombo";
            this.Size = new System.Drawing.Size(294, 20);
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtTradeAcc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtCounterParty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtVenue)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion


        #region LoadData

        public void LoadData(ref System.Data.DataSet _dsData, ref BLL.DataRoutingLogicObjects _dataRL, string _strMemoryID, string _strTabID, int _iRoutingLogic)
        {
            this.dsData = _dsData; this.dataRL = _dataRL; this.dataRL = _dataRL;
            this.strMemoryID = _strMemoryID;
            this.strTabID = _strTabID;
            this.strIndex = this.Tag.ToString().Trim().Remove(0, "thenCombo".Length);
            this.iRoutingLogic = _iRoutingLogic;

            LoadDataTradingAccount();

            //			object _objMemoryValue = this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["TradingAccountID"+this.strIndex];
            //			
            //			this.cmbedtTradeAcc.Value = (IsNull(_objMemoryValue))? Functions.MinValue: int.Parse(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["TradingAccountID"+ this.strIndex].ToString());
            //			
            if (int.Parse(strIndex) < this.dataRL.TradingAccountCount(strTabID, iRoutingLogic))
            {
                if (this.dataRL.TradingAccountID(strTabID, iRoutingLogic, int.Parse(strIndex)) >= 0)
                {
                    ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = false;
                    this.cmbedtTradeAcc.Value = this.dataRL.TradingAccountID(strTabID, iRoutingLogic, int.Parse(strIndex));
                    ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = true;

                    this.cmbedtCounterParty.Value = "";
                    this.cmbedtVenue.Value = "";
                }
                else
                {
                    ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = false;
                    this.cmbedtTradeAcc.Value = Functions.MinValue;
                    this.cmbedtCounterParty.Value = this.dataRL.CounterPartyID(strTabID, iRoutingLogic, int.Parse(strIndex));
                    this.cmbedtVenue.Value = this.dataRL.VenueID(strTabID, iRoutingLogic, int.Parse(strIndex));
                    ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = true;

                }
            }
            else
            {
                this.NullTACPV();

                //				this.cmbedtTradeAcc.Value = Functions.MinValue;
                //				this.cmbedtCounterParty.Value = "";
                //				this.cmbedtVenue.Value = "";
            }


        }

        #endregion

        #region nullify all
        public void NullTACPV()
        {
            this.cmbedtTradeAcc.Value = Functions.MinValue;
            this.cmbedtCounterParty.Value = "";
            this.cmbedtCounterParty.SelectedItem = null;
            this.cmbedtVenue.Value = "";
            this.cmbedtVenue.SelectedItem = null;
            if (!Functions.IsNull(strTabID))
            {
                this.dataRL.CounterPartyID(strTabID, iRoutingLogic, int.Parse(strIndex), Functions.MinValue);
                this.dataRL.VenueID(strTabID, iRoutingLogic, int.Parse(strIndex), Functions.MinValue);
                this.dataRL.CounterPartyVenueID(strTabID, iRoutingLogic, int.Parse(strIndex), Functions.MinValue);
            }
        }

        #endregion


        #region trading account 
        #region LoadDataTradingAccountDefault

        private void LoadDataTradingAccount()
        {
            //			((Forms.CompanyMaster)(this.ParentForm)).UserTriggered=false;
            this.cmbedtTradeAcc.Items.Clear();
            this.cmbedtTradeAcc.Items.Add(Functions.MinValue, "None");

            foreach (System.Data.DataRow _row in this.dsData.Tables["dtTradingAccount"].Select())
            {
                this.cmbedtTradeAcc.Items.Add(int.Parse(_row["CompanyTradingAccountsID"].ToString()), _row["TradingAccountName"].ToString());
            }
        }

        #endregion

        #region SaveMemTrade


        private void SelectTrading(object sender, System.EventArgs e)
        {
            if (Functions.IsNull(this.cmbedtTradeAcc.SelectedItem))
            {
                return;
            }
            if (this.dataRL.RoutingPathCount(strTabID) <= iRoutingLogic)
            {
                this.dataRL.RoutingPathCount(strTabID, iRoutingLogic + 1);
            }


            int _iTradingAccountID = int.Parse(this.cmbedtTradeAcc.SelectedItem.DataValue.ToString());

            if (int.Parse(strIndex) >= this.dataRL.TradingAccountCount(strTabID, iRoutingLogic))
            {
                this.dataRL.TradingAccountCount(strTabID, iRoutingLogic, int.Parse(strIndex) + 1);
            }


            if (_iTradingAccountID == Functions.MinValue || _iTradingAccountID < 0)
            {

                //				this.cmbedtCounterParty.Show();
                //				this.cmbedtVenue.Show();
                this.cmbedtVenue.Enabled = true;
                this.cmbedtCounterParty.Enabled = true;

                LoadCounterPartyVenue();
                //				((Controls.IfThen)(this.Parent)).NextTradingAccountHide(int.Parse(strIndex));
            }
            else
            {
                int _iTradingAccountIDMemory = Functions.MinValue, i = -1;
                bool _bNextExist = false;

                //				string _strTradingAccountIDMemory,_colName = "TradingAccountID"+i.ToString();
                //				object _objMemoryValue;
                _iTradingAccountIDMemory = this.dataRL.TradingAccountIDDefault(strTabID, iRoutingLogic);


                do
                {
                    //					_strTradingAccountIDMemory=this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)[_colName].ToString();
                    //					
                    //					if((i==int.Parse(this.strIndex))  ||  (_strTradingAccountIDMemory.Equals("")))
                    //					{	
                    //						i++;
                    //						_colName = "TradingAccountID"+i.ToString();					
                    //						continue;
                    //					}
                    //										
                    //					_iTradingAccountIDMemory = Convert.ToInt32(_strTradingAccountIDMemory);

                    _bNextExist = false;

                    if (i == Convert.ToInt32(strIndex))
                    {
                        if (++i < this.dataRL.TradingAccountCount(strTabID, iRoutingLogic))
                        {
                            _bNextExist = true;
                            _iTradingAccountIDMemory = this.dataRL.TradingAccountID(strTabID, iRoutingLogic, i);
                        }
                        continue;
                    }



                    if (_iTradingAccountID == _iTradingAccountIDMemory)
                    {
                        MessageBox.Show(" You Can't Select Same Trading Account Twice or More ");
                        this.cmbedtTradeAcc.ValueChanged -= new System.EventHandler(this.SelectTrading);
                        //						_objMemoryValue = this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["TradingAccountID"+this.strIndex];
                        //						_iTradingAccountIDMemory = int.Parse(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["TradingAccountID"+this.strIndex].ToString()) ;
                        ////						cmbedtTradeAcc.SelectedIndex = cmbedtTradeAcc.Items.IndexOf(
                        //						this.cmbedtTradeAcc.Value = (IsNull(_objMemoryValue)? Functions.MinValue : _iTradingAccountIDMemory ) ;

                        this.cmbedtTradeAcc.Value = this.dataRL.TradingAccountID(strTabID, iRoutingLogic, int.Parse(strIndex));
                        this.cmbedtTradeAcc.ValueChanged += new System.EventHandler(this.SelectTrading);
                        return;
                    }
                    if (++i < this.dataRL.TradingAccountCount(strTabID, iRoutingLogic))
                    {
                        _bNextExist = true;
                        _iTradingAccountIDMemory = this.dataRL.TradingAccountID(strTabID, iRoutingLogic, i);
                    }

                    //					_colName = "TradingAccountID"+i.ToString();
                } while (_bNextExist);//this.dsData.Tables["dtMemoryRL"].Columns.Contains(_colName));

                this.cmbedtCounterParty.Value = "";
                this.cmbedtCounterParty.SelectedItem = null;
                this.cmbedtVenue.Value = "";
                this.cmbedtVenue.SelectedItem = null;
                this.dataRL.CounterPartyID(strTabID, iRoutingLogic, int.Parse(strIndex), Functions.MinValue);
                this.dataRL.VenueID(strTabID, iRoutingLogic, int.Parse(strIndex), Functions.MinValue);
                this.dataRL.CounterPartyVenueID(strTabID, iRoutingLogic, int.Parse(strIndex), Functions.MinValue);

                //				this.cmbedtCounterParty.Hide();
                //				this.cmbedtVenue.Hide();
                this.cmbedtVenue.Enabled = false;
                this.cmbedtCounterParty.Enabled = false;

                //				((Controls.IfThen)(this.Parent)).NextTradingAccountShow(int.Parse(strIndex));

            }


            int _iValue = _iTradingAccountID;
            int _iValueOld = this.dataRL.TradingAccountID(strTabID, iRoutingLogic, int.Parse(strIndex));

            if (_iValue != _iValueOld)
            {
                ((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID, iRoutingLogic, true);

            }

            //			this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["TradingAccountID"+this.strIndex] = _iTradingAccountID;
            //			this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["TradingAccountName"+this.strIndex] = this.cmbedtTradeAcc.SelectedItem.DisplayText.ToString();

            this.dataRL.TradingAccountID(strTabID, iRoutingLogic, int.Parse(strIndex), _iTradingAccountID);

            //					((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID,iRoutingIndex, true);


            ((Controls.IfThen)(this.Parent)).NextTradingAccountEvents(int.Parse(strIndex));


        }



        #endregion




        #endregion

        #region  counterparty & venue

        private void LoadCounterPartyVenue()
        {
            //			if(IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AUECID"]))
            //			{
            //				return ;
            //			}

            if (this.dataRL.TradingAccountCount(strTabID, iRoutingLogic) <= int.Parse(strIndex))
            {
                return;
            }

            LoadCounterParty();
            ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = false;
            this.cmbedtCounterParty.Value = this.dataRL.CounterPartyID(strTabID, iRoutingLogic, int.Parse(strIndex));
            ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = true;

            //			object _objMemoryValue = this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["CounterPartyID"+this.strIndex];
            //			
            //			this.cmbedtCounterParty.Value = (IsNull(_objMemoryValue))? Functions.MinValue: int.Parse(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["CounterPartyID"+ this.strIndex].ToString());

            LoadVenue();
            ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = false;
            this.cmbedtVenue.Value = this.dataRL.VenueID(strTabID, iRoutingLogic, int.Parse(strIndex));
            ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = true;

            //			_objMemoryValue = this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["VenueID"+this.strIndex];
            //		
            //			this.cmbedtVenue.Value = (IsNull(_objMemoryValue))? Functions.MinValue: int.Parse(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["VenueID"+ this.strIndex].ToString());


        }

        #region  Load counterparty

        private void LoadCounterParty()
        {
            this.cmbedtCounterParty.Items.Clear();

            //			this.cmbedtTradeAcc.Items.Add(Functions.MinValue, "None");



            System.Collections.ArrayList _alCP = new ArrayList();

            string _strSelect;
            if (Functions.IsNull(this.cmbedtVenue.SelectedItem))
            {
                _strSelect = " AUECID = " + this.dataRL.AUECID(strTabID).ToString();

            }
            else
            {
                _strSelect = " AUECID = " + this.dataRL.AUECID(strTabID).ToString() + "  AND VenueID = " + this.cmbedtVenue.SelectedItem.DataValue.ToString();
            }

            //			if(IsNull(this.cmbedtVenue.SelectedItem))
            //			{
            //				_strSelect = " AUECID = "+ this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AUECID"].ToString() ;
            //				
            //			}
            //			else
            //			{
            //				_strSelect = " AUECID = "+ this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AUECID"].ToString() + "  AND VenueID = " + this.cmbedtVenue.SelectedItem.DataValue.ToString() ;
            //			}

            foreach (System.Data.DataRow _row in this.dsData.Tables["dtCounterPartyVenue"].Select(_strSelect))
            {
                //				new Infragistics.Win.ValueList  lst[] = {new ValueList()};
                //				new Infragistics.Win.ValueListItem itemt[] = {new ValueListItem()};
                //				itemt.DisplayText = "dsfsdf";
                //				lst.ValueListItems.Add(itemt);

                //				if(this.cmbedtCounterParty.Items.Contains(
                if (!(_alCP.Contains(_row["CounterPartyID"].ToString())))
                {
                    _alCP.Add(_row["CounterPartyID"].ToString());
                    this.cmbedtCounterParty.Items.Add(int.Parse(_row["CounterPartyID"].ToString()), _row["CounterPartyName"].ToString());
                }
            }

            if (this.dataRL.TradingAccountCount(strTabID, iRoutingLogic) > int.Parse(this.strIndex))
            {
                ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = false;
                this.cmbedtCounterParty.Value = this.dataRL.CounterPartyID(strTabID, iRoutingLogic, Convert.ToInt32(strIndex));
                ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = true;
                //				this.cmbedtCounterParty.Value = Convert.ToInt32(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["CounterPartyID"+this.strIndex].ToString());

            }

        }
        #endregion

        #region  Load venue
        private void LoadVenue()
        {
            this.cmbedtVenue.Items.Clear();
            //			this.cmbedtTradeAcc.Items.Add(Functions.MinValue, "None");
            System.Collections.ArrayList _alV = new ArrayList();

            string _strSelect;
            if (Functions.IsNull(this.cmbedtCounterParty.SelectedItem))
            {
                _strSelect = " AUECID = " + this.dataRL.AUECID(strTabID).ToString();

            }
            else
            {
                _strSelect = " AUECID = " + this.dataRL.AUECID(strTabID).ToString() + "  AND CounterPartyID = " + this.cmbedtCounterParty.SelectedItem.DataValue.ToString();
            }
            //			if(IsNull(this.cmbedtCounterParty.SelectedItem))
            //			{
            //				_strSelect = " AUECID = "+ this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AUECID"].ToString() ;
            //				
            //			}
            //			else
            //			{
            //				_strSelect = " AUECID = "+ this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AUECID"].ToString() + "  AND CounterPartyID = " + this.cmbedtCounterParty.SelectedItem.DataValue.ToString() ;
            //				
            //			}

            foreach (System.Data.DataRow _row in this.dsData.Tables["dtCounterPartyVenue"].Select(_strSelect))
            {
                if (!(_alV.Contains(_row["CounterPartyID"].ToString())))
                {
                    _alV.Add(_row["CounterPartyID"].ToString());
                    this.cmbedtVenue.Items.Add(int.Parse(_row["VenueID"].ToString()), _row["VenueName"].ToString());
                }
            }

            if (this.dataRL.TradingAccountCount(strTabID, iRoutingLogic) > int.Parse(this.strIndex))
            {
                ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = false;
                this.cmbedtVenue.Value = this.dataRL.VenueID(strTabID, iRoutingLogic, Convert.ToInt32(strIndex));
                ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = true;
                //				this.cmbedtCounterParty.Value = Convert.ToInt32(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["CounterPartyID"+this.strIndex].ToString());

            }

            //			if(!IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["VenueID"+this.strIndex]))
            //			{
            //				this.cmbedtVenue.Value = Convert.ToInt32(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["VenueID"+this.strIndex].ToString());
            //
            //			}

        }
        #endregion

        #region eventhandlers  counterparty venue


        private void SelectVenue(object sender, System.EventArgs e)
        {
            if (Functions.IsNull(this.cmbedtVenue.SelectedItem))
            {
                return;
            }


            int _iVenueID = int.Parse(this.cmbedtVenue.SelectedItem.DataValue.ToString());
            LoadCounterParty();
            if (this.dataRL.RoutingPathCount(strTabID) <= iRoutingLogic)
            {
                this.dataRL.RoutingPathCount(strTabID, iRoutingLogic + 1);
            }

            int _iCVID = -1;

            if ((this.dataRL.TradingAccountCount(strTabID, iRoutingLogic) > int.Parse(this.strIndex)) /** !IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["CounterPartyID"+this.strIndex]) **/ && !Functions.IsNull(this.cmbedtCounterParty.SelectedItem))
            {
                string _str = " VenueID = " + _iVenueID.ToString() + "   AND CounterPartyID = " + this.cmbedtCounterParty.SelectedItem.DataValue.ToString();
                string _strCVID = this.dsData.Tables["dtCounterPartyVenue"].Select(_str)[0]["CounterPartyVenueID"].ToString().Trim();
                //			string _strVenueIDMemory , _strCounterPartyIDMemory, _strCVIDMemory ;
                int i = 0, _iCVIDMemory = -1;
                _iCVID = int.Parse(_strCVID);


                //				string _colNameV = "VenueID"+i.ToString();
                //				string _colNameCP = "CounterPartyID"+i.ToString();
                //				object _objMemoryValue;

                do
                {
                    if ((i == int.Parse(this.strIndex)))//|| ( this.dataRL.TradingAccountCount(strTabID,iRoutingLogic) > i ))// IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["VenueID"+i.ToString()])  || IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["CounterPartyID"+i.ToString()])   )
                    {
                        i++;
                        //						_colNameV = "VenueID"+i.ToString();
                        //						_colNameCP = "CounterPartyID"+i.ToString();					
                        continue;
                    }

                    //					_strVenueIDMemory=this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)[_colNameV].ToString();
                    //					_strCounterPartyIDMemory=this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)[_colNameCP].ToString();
                    //					_strCVIDMemory = this.dsData.Tables["dtCounterPartyVenue"].Select(" VenueID = " + _strVenueIDMemory +  "   AND CounterPartyID = " + _strCounterPartyIDMemory)[0]["CounterPartyVenueID"].ToString().Trim();
                    //				
                    _iCVIDMemory = this.dataRL.CounterPartyVenueID(strTabID, iRoutingLogic, i);

                    if (_iCVID == _iCVIDMemory)//_strCVIDMemory.Equals(_strCVID))
                    {
                        MessageBox.Show(" You Can't Select Same Venue-CounterParty Twice or More ");
                        this.cmbedtVenue.SelectionChangeCommitted -= new System.EventHandler(this.SelectVenue);
                        //						_objMemoryValue = this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["VenueID"+this.strIndex];
                        //						_strVenueIDMemory = this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["VenueID"+this.strIndex].ToString();
                        //						//						cmbedtVenue.SelectedIndex = cmbedtVenue.Items.IndexOf(
                        //						this.cmbedtVenue.Value = (IsNull(_objMemoryValue)? Functions.MinValue : int.Parse(_strVenueIDMemory) ) ;
                        this.cmbedtVenue.Value = this.dataRL.VenueID(strTabID, iRoutingLogic, int.Parse(strIndex));
                        if (Functions.IsNull(this.cmbedtVenue.Value) || this.cmbedtVenue.Value.ToString().Equals("") || (int.Parse(this.cmbedtVenue.Value.ToString())) < 0)
                        {
                            this.cmbedtVenue.SelectedItem = null;

                            this.dataRL.VenueID(strTabID, iRoutingLogic, int.Parse(strIndex), Functions.MinValue);
                            this.dataRL.CounterPartyVenueID(strTabID, iRoutingLogic, int.Parse(strIndex), Functions.MinValue);
                        }
                        this.cmbedtVenue.SelectionChangeCommitted += new System.EventHandler(this.SelectVenue);
                        return;
                    }

                    i++;
                    //					_colNameV = "VenueID"+i.ToString();
                    //					_colNameCP = "CounterPartyID"+i.ToString();	
                } while ((this.dataRL.TradingAccountCount(strTabID, iRoutingLogic) > i));//this.dsData.Tables["dtMemoryRL"].Columns.Contains(_colNameV));



            }

            int _iValue = _iCVID;
            int _iValueOld = this.dataRL.CounterPartyVenueID(strTabID, iRoutingLogic, int.Parse(strIndex));

            if (_iValue != _iValueOld)
            {
                ((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID, iRoutingLogic, true);

            }

            //			this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["VenueID"+this.strIndex] = _iVenueID;
            //			this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["VenueName"+this.strIndex] = this.cmbedtVenue.SelectedItem.DisplayText.ToString();

            this.dataRL.VenueID(strTabID, iRoutingLogic, int.Parse(strIndex), _iVenueID);
            this.dataRL.CounterPartyVenueID(strTabID, iRoutingLogic, int.Parse(strIndex), _iCVID);
            ((Controls.IfThen)(this.Parent)).NextTradingAccountEvents(int.Parse(strIndex));


            //				((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID,iRoutingIndex, true);


        }

        private void SelectCounterParty(object sender, System.EventArgs e)
        {

            if (Functions.IsNull(this.cmbedtCounterParty.SelectedItem))
            {
                return;
            }

            int _iCounterPartyID = int.Parse(this.cmbedtCounterParty.SelectedItem.DataValue.ToString());

            LoadVenue();
            if (this.dataRL.RoutingPathCount(strTabID) <= iRoutingLogic)
            {
                this.dataRL.RoutingPathCount(strTabID, iRoutingLogic + 1);
            }

            int _iCVID = -1;

            if ((this.dataRL.TradingAccountCount(strTabID, iRoutingLogic) > int.Parse(this.strIndex)) && !Functions.IsNull(this.cmbedtVenue.SelectedItem)) // !IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["VenueID"+this.strIndex]) && !IsNull(this.cmbedtVenue.SelectedItem))
            {
                string _str = " CounterPartyID = " + _iCounterPartyID.ToString() + "   AND VenueID = " + this.cmbedtVenue.SelectedItem.DataValue.ToString();
                string _strCVID = this.dsData.Tables["dtCounterPartyVenue"].Select(_str)[0]["CounterPartyVenueID"].ToString().Trim();
                //			string _strCounterPartyIDMemory , _strVenueIDMemory, _strCVIDMemory ;
                int i = 0, _iCVIDMemory = -1;
                _iCVID = int.Parse(_strCVID);



                //				string _colNameCP = "CounterPartyID"+i.ToString();
                //				string _colNameV = "VenueID"+i.ToString();
                //				object _objMemoryValue;
                //				
                do
                {
                    if ((i == int.Parse(this.strIndex)))//|| ( this.dataRL.TradingAccountCount(strTabID,iRoutingLogic) > i ) )//(i==int.Parse(this.strIndex))  ||  IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["CounterPartyID"+i.ToString()])  || IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["VenueID"+i.ToString()])   )
                    {
                        i++;
                        //						_colNameCP = "CounterPartyID"+i.ToString();
                        //						_colNameV = "VenueID"+i.ToString();					
                        continue;
                    }

                    _iCVIDMemory = this.dataRL.CounterPartyVenueID(strTabID, iRoutingLogic, i);

                    //					_strCounterPartyIDMemory=this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)[_colNameCP].ToString();
                    //					_strVenueIDMemory=this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)[_colNameV].ToString();
                    //					_strCVIDMemory = this.dsData.Tables["dtCounterPartyVenue"].Select(" CounterPartyID = " + _strCounterPartyIDMemory +  "   AND VenueID = " + _strVenueIDMemory)[0]["CounterPartyVenueID"].ToString().Trim();
                    //				
                    if (_iCVID == _iCVIDMemory)//_strCVIDMemory.Equals(_strCVID))
                    {
                        MessageBox.Show(" You Can't Select Same CounterParty-Venue Twice or More ");
                        this.cmbedtCounterParty.SelectionChangeCommitted -= new System.EventHandler(this.SelectCounterParty);
                        //						_objMemoryValue = this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["CounterPartyID"+this.strIndex];
                        //						_strCounterPartyIDMemory = this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["CounterPartyID"+this.strIndex].ToString();
                        //						//						cmbedtCounterParty.SelectedIndex = cmbedtCounterParty.Items.IndexOf(
                        //						this.cmbedtCounterParty.Value = (IsNull(_objMemoryValue)? Functions.MinValue : int.Parse(_strCounterPartyIDMemory) ) ;
                        this.cmbedtCounterParty.Value = this.dataRL.CounterPartyID(strTabID, iRoutingLogic, int.Parse(strIndex));
                        if (Functions.IsNull(this.cmbedtCounterParty.Value) || this.cmbedtCounterParty.Value.ToString().Equals("") || (int.Parse(this.cmbedtCounterParty.Value.ToString())) < 0)
                        {
                            this.cmbedtCounterParty.SelectedItem = null;
                            this.dataRL.CounterPartyID(strTabID, iRoutingLogic, int.Parse(strIndex), Functions.MinValue);
                            this.dataRL.CounterPartyVenueID(strTabID, iRoutingLogic, int.Parse(strIndex), Functions.MinValue);
                        }
                        this.cmbedtCounterParty.SelectionChangeCommitted += new System.EventHandler(this.SelectCounterParty);
                        return;
                    }

                    i++;
                    //					_colNameCP = "CounterPartyID"+i.ToString();
                    //					_colNameV = "VenueID"+i.ToString();	
                } while ((this.dataRL.TradingAccountCount(strTabID, iRoutingLogic) > i));//this.dsData.Tables["dtMemoryRL"].Columns.Contains(_colNameCP));



            }

            int _iValue = _iCVID;
            int _iValueOld = this.dataRL.CounterPartyVenueID(strTabID, iRoutingLogic, int.Parse(strIndex));

            if (_iValue != _iValueOld)
            {
                ((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID, iRoutingLogic, true);

            }

            //			this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["CounterPartyID"+this.strIndex] = _iCounterPartyID;
            //			this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["CounterPartyName"+this.strIndex] = this.cmbedtCounterParty.SelectedItem.DisplayText.ToString();

            this.dataRL.CounterPartyID(strTabID, iRoutingLogic, int.Parse(strIndex), _iCounterPartyID);
            this.dataRL.CounterPartyVenueID(strTabID, iRoutingLogic, int.Parse(strIndex), _iCVID);
            ((Controls.IfThen)(this.Parent)).NextTradingAccountEvents(int.Parse(strIndex));

            //						((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID,iRoutingIndex, true);

        }
        #endregion

        #endregion

        //		#region is null
        //		private bool IsNull(Object _obj)
        //		{
        //			try
        //			{
        //				if(_obj==null ||  _obj.Equals(null) || _obj.Equals(System.DBNull.Value))
        //				{
        //					return true;
        //				}
        //
        //				return false;
        //			}
        //			catch
        //			{
        //				return true;
        //			}
        //		}
        //		#endregion


    }
}
