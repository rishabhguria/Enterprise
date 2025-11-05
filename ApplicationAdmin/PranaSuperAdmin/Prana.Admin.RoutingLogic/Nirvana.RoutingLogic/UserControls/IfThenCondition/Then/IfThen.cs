using Infragistics.Win.Misc;
using Prana.Admin.RoutingLogic.MisclFunctions;
using Prana.Global;
using System.Collections;
using System.Windows.Forms;

namespace Prana.Admin.RoutingLogic.Controls
{
    /// <summary>
    /// Summary description for IfThen.
    /// </summary>
    public class IfThen : System.Windows.Forms.UserControl
    {
        private Infragistics.Win.Misc.UltraLabel labelTradingAccount;
        private Infragistics.Win.Misc.UltraLabel labelCounterParty;
        private Infragistics.Win.Misc.UltraLabel labelVenue;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Data.DataSet dsData; private BLL.DataRoutingLogicObjects dataRL;
        private Controls.ThenCombo thenCombo0;
        private Controls.ThenCombo thenCombo1;
        private Controls.ThenCombo thenCombo2;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbedtTradeDefault;

        private System.Collections.ArrayList alThenCombo = new ArrayList();
        private System.Collections.ArrayList allabelRank = new ArrayList();
        private string strMemoryID;
        private string strTabID;
        private Infragistics.Win.Misc.UltraLabel labelRank1;
        private Infragistics.Win.Misc.UltraLabel labelRank2;
        private Infragistics.Win.Misc.UltraLabel labelRank3;
        private Infragistics.Win.Misc.UltraLabel labelRankDefault;
        private int iRoutingIndex;

        public IfThen()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

            this.alThenCombo.Add(this.thenCombo0);
            this.alThenCombo.Add(this.thenCombo1);
            this.alThenCombo.Add(this.thenCombo2);

            this.allabelRank.Add(this.labelRank1);
            this.allabelRank.Add(this.labelRank2);
            this.allabelRank.Add(this.labelRank3);
            this.allabelRank.Add(this.labelRankDefault);


            this.cmbedtTradeDefault.Leave += new System.EventHandler(Functions.object_LostFocus);
            this.cmbedtTradeDefault.Enter += new System.EventHandler(Functions.object_GotFocus);

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
                if (labelTradingAccount != null)
                {
                    labelTradingAccount.Dispose();
                }
                if (labelCounterParty != null)
                {
                    labelCounterParty.Dispose();
                }
                if (labelVenue != null)
                {
                    labelVenue.Dispose();
                }
                if (thenCombo0 != null)
                {
                    thenCombo0.Dispose();
                }
                if (thenCombo1 != null)
                {
                    thenCombo1.Dispose();
                }
                if (thenCombo2 != null)
                {
                    thenCombo2.Dispose();
                }
                if (cmbedtTradeDefault != null)
                {
                    cmbedtTradeDefault.Dispose();
                }
                if (labelRank1 != null)
                {
                    labelRank1.Dispose();
                }
                if (labelRank2 != null)
                {
                    labelRank2.Dispose();
                }
                if (labelRank3 != null)
                {
                    labelRank3.Dispose();
                }
                if (labelRankDefault != null)
                {
                    labelRankDefault.Dispose();
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
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            this.labelTradingAccount = new Infragistics.Win.Misc.UltraLabel();
            this.labelCounterParty = new Infragistics.Win.Misc.UltraLabel();
            this.labelVenue = new Infragistics.Win.Misc.UltraLabel();
            this.labelRank1 = new Infragistics.Win.Misc.UltraLabel();
            this.labelRank2 = new Infragistics.Win.Misc.UltraLabel();
            this.labelRank3 = new Infragistics.Win.Misc.UltraLabel();
            this.labelRankDefault = new Infragistics.Win.Misc.UltraLabel();
            this.cmbedtTradeDefault = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.thenCombo0 = new Prana.Admin.RoutingLogic.Controls.ThenCombo();
            this.thenCombo1 = new Prana.Admin.RoutingLogic.Controls.ThenCombo();
            this.thenCombo2 = new Prana.Admin.RoutingLogic.Controls.ThenCombo();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtTradeDefault)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTradingAccount
            // 
            appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance1.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.labelTradingAccount.Appearance = appearance1;
            this.labelTradingAccount.Location = new System.Drawing.Point(13, 0);
            this.labelTradingAccount.Name = "labelTradingAccount";
            this.labelTradingAccount.Size = new System.Drawing.Size(86, 16);
            this.labelTradingAccount.TabIndex = 0;
            this.labelTradingAccount.Text = "Trading Account";
            // 
            // labelCounterParty
            // 
            appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance2.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.labelCounterParty.Appearance = appearance2;
            this.labelCounterParty.Location = new System.Drawing.Point(132, 1);
            this.labelCounterParty.Name = "labelCounterParty";
            this.labelCounterParty.Size = new System.Drawing.Size(78, 14);
            this.labelCounterParty.TabIndex = 1;
            this.labelCounterParty.Text = ApplicationConstants.CONST_BROKER;
            // 
            // labelVenue
            // 
            appearance3.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance3.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.labelVenue.Appearance = appearance3;
            this.labelVenue.Location = new System.Drawing.Point(238, 0);
            this.labelVenue.Name = "labelVenue";
            this.labelVenue.Size = new System.Drawing.Size(36, 16);
            this.labelVenue.TabIndex = 2;
            this.labelVenue.Text = "Venue";
            // 
            // labelRank1
            // 
            appearance4.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance4.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.labelRank1.Appearance = appearance4;
            this.labelRank1.Location = new System.Drawing.Point(2, 24);
            this.labelRank1.Name = "labelRank1";
            this.labelRank1.Size = new System.Drawing.Size(8, 14);
            this.labelRank1.TabIndex = 3;
            this.labelRank1.Text = "1";
            // 
            // labelRank2
            // 
            appearance5.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance5.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.labelRank2.Appearance = appearance5;
            this.labelRank2.Location = new System.Drawing.Point(2, 48);
            this.labelRank2.Name = "labelRank2";
            this.labelRank2.Size = new System.Drawing.Size(8, 12);
            this.labelRank2.TabIndex = 4;
            this.labelRank2.Text = "2";
            // 
            // labelRank3
            // 
            appearance6.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance6.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.labelRank3.Appearance = appearance6;
            this.labelRank3.Location = new System.Drawing.Point(2, 70);
            this.labelRank3.Name = "labelRank3";
            this.labelRank3.Size = new System.Drawing.Size(6, 12);
            this.labelRank3.TabIndex = 5;
            this.labelRank3.Text = "3";
            // 
            // labelRankDefault
            // 
            appearance7.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance7.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.labelRankDefault.Appearance = appearance7;
            this.labelRankDefault.Location = new System.Drawing.Point(344, 4);
            this.labelRankDefault.Name = "labelRankDefault";
            this.labelRankDefault.Size = new System.Drawing.Size(102, 12);
            this.labelRankDefault.TabIndex = 6;
            this.labelRankDefault.Text = "Default Destination";
            // 
            // cmbedtTradeDefault
            // 
            this.cmbedtTradeDefault.DropDownListWidth = 150;
            this.cmbedtTradeDefault.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbedtTradeDefault.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbedtTradeDefault.Location = new System.Drawing.Point(350, 22);
            this.cmbedtTradeDefault.Name = "cmbedtTradeDefault";
            this.cmbedtTradeDefault.Size = new System.Drawing.Size(88, 20);
            this.cmbedtTradeDefault.TabIndex = 7;
            this.cmbedtTradeDefault.Tag = "cmbedtTradeDefault";
            this.cmbedtTradeDefault.SelectionChanged += new System.EventHandler(this.SaveMemDefaultTrade);
            // 
            // thenCombo0
            // 
            this.thenCombo0.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.thenCombo0.Location = new System.Drawing.Point(14, 18);
            this.thenCombo0.Name = "thenCombo0";
            this.thenCombo0.Size = new System.Drawing.Size(296, 24);
            this.thenCombo0.TabIndex = 8;
            this.thenCombo0.Tag = "thenCombo0";
            // 
            // thenCombo1
            // 
            this.thenCombo1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.thenCombo1.Location = new System.Drawing.Point(14, 42);
            this.thenCombo1.Name = "thenCombo1";
            this.thenCombo1.Size = new System.Drawing.Size(294, 24);
            this.thenCombo1.TabIndex = 9;
            this.thenCombo1.Tag = "thenCombo1";
            // 
            // thenCombo2
            // 
            this.thenCombo2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.thenCombo2.Location = new System.Drawing.Point(14, 66);
            this.thenCombo2.Name = "thenCombo2";
            this.thenCombo2.Size = new System.Drawing.Size(294, 22);
            this.thenCombo2.TabIndex = 0;
            this.thenCombo2.Tag = "thenCombo2";
            // 
            // IfThen
            // 
            this.Controls.Add(this.thenCombo2);
            this.Controls.Add(this.thenCombo1);
            this.Controls.Add(this.thenCombo0);
            this.Controls.Add(this.cmbedtTradeDefault);
            this.Controls.Add(this.labelRankDefault);
            this.Controls.Add(this.labelRank3);
            this.Controls.Add(this.labelRank2);
            this.Controls.Add(this.labelRank1);
            this.Controls.Add(this.labelVenue);
            this.Controls.Add(this.labelCounterParty);
            this.Controls.Add(this.labelTradingAccount);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.Name = "IfThen";
            this.Size = new System.Drawing.Size(448, 90);
            this.Load += new System.EventHandler(this.IfThen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtTradeDefault)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion


        #region LoadData

        public void LoadData(ref System.Data.DataSet _dsData, ref BLL.DataRoutingLogicObjects _dataRL, string _strMemoryID, string _strTabID, int _iRoutingIndex)
        {
            this.dsData = _dsData; this.dataRL = _dataRL;
            this.strMemoryID = _strMemoryID;
            this.strTabID = _strTabID;
            this.iRoutingIndex = _iRoutingIndex;

            foreach (Controls.ThenCombo _ucThenCombo in this.alThenCombo)
            {
                _ucThenCombo.LoadData(ref dsData, ref dataRL, strMemoryID, strTabID, iRoutingIndex);
            }

            LoadDataTradingAccountDefault();
            ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = false;
            //			this.cmbedtTradeDefault.Value = int.Parse(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["TradingAccountDefaultID"].ToString());
            this.cmbedtTradeDefault.Value = this.dataRL.TradingAccountIDDefault(strTabID, iRoutingIndex);
            ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = true;


        }

        #endregion

        #region LoadDataTradingAccountDefault

        private void LoadDataTradingAccountDefault()
        {
            this.cmbedtTradeDefault.Items.Clear();

            System.Collections.ArrayList _alContainsCheckerTAID = new ArrayList();
            int _iTAID = Functions.MinValue;
            string _strTAName = "";

            foreach (System.Data.DataRow _row in this.dsData.Tables["dtTradingAccount"].Select())
            {


                _iTAID = int.Parse(_row["CompanyTradingAccountsID"].ToString());
                _strTAName = _row["TradingAccountName"].ToString();

                if (!_alContainsCheckerTAID.Contains(_iTAID))
                {
                    _alContainsCheckerTAID.Add(_iTAID);
                    this.cmbedtTradeDefault.Items.Add(_iTAID, _strTAName);
                }

            }


            //			foreach( Controls.ThenCombo _ucThenCombo in this.alThenCombo)
            //			{
            //				if(
            //
            //
            //			}



        }

        #endregion

        #region SaveMemDefaultTrade
        private void SaveMemDefaultTrade(object sender, System.EventArgs e)
        {
            if (Functions.IsNull(this.cmbedtTradeDefault.SelectedItem))
            {
                return;
            }

            int _iTradingAccountDefaultID = int.Parse(this.cmbedtTradeDefault.SelectedItem.DataValue.ToString());
            //			bool  _bSelectedOld = ((int.Parse(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["TradingAccountDefaultID"].ToString()))==_iTradingAccountDefaultID)?true:false;

            if (this.dataRL.RoutingPathCount(strTabID) <= iRoutingIndex)
            {
                this.dataRL.RoutingPathCount(strTabID, iRoutingIndex + 1);
            }
            bool _bSelectedOld = (this.dataRL.TradingAccountIDDefault(strTabID, iRoutingIndex) == _iTradingAccountDefaultID) ? true : false;


            int _iTradingAccountIDMemory = Functions.MinValue;
            //			string _str;



            for (int i = 0; i < this.dataRL.TradingAccountCount(strTabID, iRoutingIndex); i++)
            {

                //				 _str   = (this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["TradingAccountID"+i.ToString()]).ToString();
                //				_str = this.dataRL
                //				if(_str.Equals(""))
                //				{
                //					continue;
                //				}
                //
                //				_iTradingAccountIDMemory = int.Parse(_str);

                _iTradingAccountIDMemory = this.dataRL.TradingAccountID(strTabID, iRoutingIndex, i);
                if (_iTradingAccountIDMemory < 0)
                {
                    continue;
                }

                if (_iTradingAccountDefaultID == _iTradingAccountIDMemory)
                {
                    MessageBox.Show(" You Can't Select Same Trading Account Twice or More :Default");
                    //this.cmbedtTradeDefault.SelectionChanged -= new System.EventHandler(this.SaveMemDefaultTrade);
                    //					this.cmbedtTradeDefault.Value = Convert.ToInt32(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["TradingAccountDefaultID"].ToString());
                    this.cmbedtTradeDefault.Value = this.dataRL.TradingAccountIDDefault(strTabID, iRoutingIndex);
                    //this.cmbedtTradeDefault.SelectionChanged += new System.EventHandler(this.SaveMemDefaultTrade);

                    return;
                }
            }



            //			this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["TradingAccountDefaultID"] = _iTradingAccountDefaultID ;
            //			this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["TradingAccountNameDefault"] = this.cmbedtTradeDefault.SelectedItem.DisplayText.ToString();

            this.dataRL.TradingAccountIDDefault(strTabID, iRoutingIndex, int.Parse(this.cmbedtTradeDefault.SelectedItem.DataValue.ToString()));

            if (!_bSelectedOld)
            {
                ((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID, iRoutingIndex, true);
            }

        }
        #endregion


        #region   hdidng/show teh next one

        public void NextTradingAccountEvents(int _iIndexCurrent)
        {
            if (_iIndexCurrent < this.alThenCombo.Count - 1)
            {
                if (_iIndexCurrent < this.dataRL.TradingAccountCount(strMemoryID, iRoutingIndex))
                {
                    if ((this.dataRL.TradingAccountID(strTabID, iRoutingIndex, _iIndexCurrent) < 0) && (this.dataRL.CounterPartyVenueID(strTabID, iRoutingIndex, _iIndexCurrent) < 0))
                    {
                        bool _bHide = false;
                        if (_iIndexCurrent + 1 < this.dataRL.TradingAccountCount(strMemoryID, iRoutingIndex))
                        {
                            _bHide = !(((Controls.ThenCombo)(this.alThenCombo[_iIndexCurrent])).Visible) || ((this.dataRL.TradingAccountID(strTabID, iRoutingIndex, _iIndexCurrent + 1) < 0) && (this.dataRL.CounterPartyVenueID(strTabID, iRoutingIndex, _iIndexCurrent + 1) < 0));
                        }
                        else
                        {
                            _bHide = !(((Controls.ThenCombo)(this.alThenCombo[_iIndexCurrent])).Visible);
                        }
                        if (_bHide)
                        {
                            if (this.dataRL.TradingAccountCount(strTabID, iRoutingIndex) > (_iIndexCurrent + 1))
                            {
                                this.dataRL.TradingAccountCount(strTabID, iRoutingIndex, _iIndexCurrent + 1);
                            }
                            ((Controls.ThenCombo)(this.alThenCombo[_iIndexCurrent + 1])).Hide();
                            ((UltraLabel)(this.allabelRank[_iIndexCurrent + 1])).Hide();
                            ((Controls.ThenCombo)(this.alThenCombo[_iIndexCurrent + 1])).NullTACPV();
                        }
                        else
                        {

                            ((Controls.ThenCombo)(this.alThenCombo[_iIndexCurrent + 1])).Show();
                            ((UltraLabel)(this.allabelRank[_iIndexCurrent + 1])).Show();

                        }
                    }
                    else
                    {
                        ((Controls.ThenCombo)(this.alThenCombo[_iIndexCurrent + 1])).Show();
                        ((UltraLabel)(this.allabelRank[_iIndexCurrent + 1])).Show();
                    }

                }
                else
                {
                    ((Controls.ThenCombo)(this.alThenCombo[_iIndexCurrent + 1])).Hide();
                    ((UltraLabel)(this.allabelRank[_iIndexCurrent + 1])).Hide();
                    ((Controls.ThenCombo)(this.alThenCombo[_iIndexCurrent + 1])).NullTACPV();
                }
            }


            //			for(int i=0;i<this.dataRL.TradingAccountCount(strTabID,iRoutingIndex);i++)
            //			{
            //				if(this.dataRL.CounterPartyVenueID(strTabID,iRoutingIndex,i)>=0 || this.dataRL.TradingAccountID(strTabID,iRoutingIndex,i)<0 )
            //				{
            //					this.labelCounterParty.Show();
            //					this.labelVenue.Show();
            //					break;
            //				}
            //
            //				this.labelCounterParty.Hide();
            //				this.labelVenue.Hide();
            //			}


        }

        private void IfThen_Load(object sender, System.EventArgs e)
        {

        }


        //		public void NextTradingAccountShow(int _iIndexCurrent)
        //		{
        //			if(_iIndexCurrent < this.dataRL.TradingAccountCount(strMemoryID,iRoutingIndex)-1)
        //			{
        //				((UltraLabel)(this.allabelRank[i]))
        //
        //			}
        //
        //
        //		}



        #endregion

        //		#region is null
        //		private bool IsNull(Object _obj)
        //		{
        //			if(_obj==null)
        //			{
        //				return true;
        //			}
        //			else if( _obj.Equals(null))
        //			{
        //				return true;
        //			}
        //			else if (_obj.Equals(System.DBNull.Value))
        //			{
        //				return true;
        //			}
        //
        //			return false;
        //		}
        //		#endregion
    }
}
