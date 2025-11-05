using Prana.Admin.RoutingLogic.MisclFunctions;
using System.Data;
using System.Windows.Forms;

namespace Prana.Admin.RoutingLogic.Controls
{
    /// <summary>
    /// Summary description for RLogic.
    /// </summary>
    public class RLogic : System.Windows.Forms.UserControl
    {
        private Infragistics.Win.Misc.UltraLabel labelName;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtedtName;
        private Prana.Admin.RoutingLogic.Controls.AUEC ucAuec;
        private Prana.Admin.RoutingLogic.Controls.IfThenCondition[] ucIfThenCondition;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Data.DataSet dsData; private Prana.Admin.RoutingLogic.BLL.DataRoutingLogicObjects dataRL;
        private string strMemoryID = "RL";
        private string strTabID = "RL";
        private Infragistics.Win.Misc.UltraGroupBox grpboxRL;
        private System.Windows.Forms.NodeTree nodeMain;



        //		private System.Collections.IList ilUCIfThenCondition;

        public RLogic()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

            // 
            // ucIfThenCondition[0]
            // 


            this.ucIfThenCondition = new Controls.IfThenCondition[] { new Controls.IfThenCondition() };


            this.grpboxRL.Controls.Add(this.ucIfThenCondition[0]);

            this.ucIfThenCondition[0].Location = new System.Drawing.Point(6, 40);
            this.ucIfThenCondition[0].Name = "ucIfThenCondition[0]";
            this.ucIfThenCondition[0].Size = new System.Drawing.Size(635, 114);
            this.ucIfThenCondition[0].TabIndex = 3;
            this.ucIfThenCondition[0].Tag = "ucIfThenCondition[0]";

            //			this.Controls.Add(this.ucIfThenCondition[0]);

            //			this.ilUCIfThenCondition.Add(ucIfThenCondition[0]);


            this.txtedtName.Leave += new System.EventHandler(Functions.object_LostFocus);
            this.txtedtName.Enter += new System.EventHandler(Functions.object_GotFocus);


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
                if (labelName != null)
                {
                    labelName.Dispose();
                }
                if (txtedtName != null)
                {
                    txtedtName.Dispose();
                }
                if (ucAuec != null)
                {
                    ucAuec.Dispose();
                }
                if (grpboxRL != null)
                {
                    grpboxRL.Dispose();
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
            this.labelName = new Infragistics.Win.Misc.UltraLabel();
            this.txtedtName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ucAuec = new Prana.Admin.RoutingLogic.Controls.AUEC();
            this.grpboxRL = new Infragistics.Win.Misc.UltraGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtedtName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpboxRL)).BeginInit();
            this.grpboxRL.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(10, 14);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(52, 16);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "RL Name";
            // 
            // txtedtName
            // 
            this.txtedtName.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtedtName.Location = new System.Drawing.Point(68, 14);
            this.txtedtName.Name = "txtedtName";
            this.txtedtName.Size = new System.Drawing.Size(86, 20);
            this.txtedtName.TabIndex = 1;
            this.txtedtName.Tag = "txtedName";
            this.txtedtName.MaxLength = 50;
            this.txtedtName.Leave += new System.EventHandler(this.UpdateMemoryCascade);
            // 
            // ucAuec
            // 
            this.ucAuec.Location = new System.Drawing.Point(168, 10);
            this.ucAuec.Name = "ucAuec";
            this.ucAuec.Size = new System.Drawing.Size(484, 24);
            this.ucAuec.TabIndex = 2;
            this.ucAuec.Tag = "ucAuec";
            // 
            // grpboxRL
            // 
            this.grpboxRL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BorderColor = System.Drawing.Color.Black;
            appearance1.BorderColor3DBase = System.Drawing.Color.Black;
            this.grpboxRL.Appearance = appearance1;
            this.grpboxRL.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.Rounded;
            this.grpboxRL.Controls.Add(this.txtedtName);
            this.grpboxRL.Controls.Add(this.labelName);
            this.grpboxRL.Controls.Add(this.ucAuec);
            this.grpboxRL.Location = new System.Drawing.Point(10, 126);
            this.grpboxRL.Name = "grpboxRL";
            this.grpboxRL.Size = new System.Drawing.Size(676, 188);
            this.grpboxRL.SupportThemes = false;
            this.grpboxRL.TabIndex = 3;
            // 
            // RLogic
            // 
            this.Controls.Add(this.grpboxRL);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.Name = "RLogic";
            this.Size = new System.Drawing.Size(701, 430);
            ((System.ComponentModel.ISupportInitialize)(this.txtedtName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpboxRL)).EndInit();
            this.grpboxRL.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion


        public int NumberOfRoutingPath
        {
            get
            {
                return this.ucIfThenCondition.Length;
            }
        }

        #region Load Data
        public void LoadData(ref System.Data.DataSet _dsData, ref BLL.DataRoutingLogicObjects _dataRL, ref System.Windows.Forms.NodeTree _nodeMain)
        {
            this.dsData = _dsData;
            this.dataRL = _dataRL;
            this.nodeMain = _nodeMain;

            this.dataRL.Name(strTabID, this.dataRL.RoutingPathName(strTabID, 0));
            this.dataRL.ID(strTabID, this.dataRL.RoutingPathID(strTabID, 0));

            //			this.txtedtName.Text = this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["RLName"].ToString();
            this.txtedtName.Text = this.dataRL.Name(strTabID);
            //			this.UpdateMemoryCascade(this.txtedtName,new System.EventArgs());
            this.DataCascade();
        }
        #endregion

        #region event for txt editior- name of RL
        private void UpdateMemoryCascade(object sender, System.EventArgs e)
        {
            if (((Infragistics.Win.UltraWinEditors.UltraTextEditor)sender).ContainsFocus)
            {
                return;
            }
            if (Functions.IsNull(this.dataRL))
            {
                return;
            }
            this.dataRL.Name(strTabID, this.txtedtName.Text);
            this.dataRL.RoutingPathName(strTabID, 0, this.dataRL.Name(strTabID));


            //			if(Functions.IsNull(this.dsData.Tables["dtMemoryRL"]))
            //			{
            //				return;
            //			}
            //			this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["RLName"] = this.txtedtName.Text ;
            //			DataCascade();		





        }
        #endregion

        #region cascase of data events
        private void DataCascade()
        {
            this.ucAuec.LoadData(ref dsData, ref dataRL, strMemoryID, strTabID);
            //			this.ucIfThenCondition[0].LoadData(ref dsData, ref dataRL);
        }
        #endregion


        #region delegations of event  auec- rl logic
        public void DelegateLoadData()
        {

            //			foreach (Controls.IfThenCondition _uc in ilUCIfThenCondition)
            //			{
            //				_uc.LoadData(ref _dsData);
            //			}



            for (int _iRoutingIndex = 0; _iRoutingIndex < this.ucIfThenCondition.Length; _iRoutingIndex++)
            {
                //			 this.dataRL.ConditionsCount(strTabID,_iRoutingIndex,3);
                this.ucIfThenCondition[_iRoutingIndex].LoadData(ref dsData, ref dataRL, strMemoryID, strTabID, _iRoutingIndex);
            }



            //			ConsoleApplication1.Class1 c2=new Class1();
            //			MyDelegate d1 = new MyDelegate(c2.delegateMethod1);
            //			MyDelegate d2 = new MyDelegate(c2.delegateMethod2);
            //			MyDelegate d3 = d1 + d2;
            //			return d3;
        }
        #endregion


        #region save

        public string SaveData()

        {
            //			//object[] parameter = new object[this.dsData.Tables["dtMemoryRL"].Columns.Count];
            //			System.Data.SqlClient.SqlParameter[] _sqlParam = new System.Data.SqlClient.SqlParameter[this.dsData.Tables["dtMemoryRL"].Columns.Count];
            //				
            //			System.Data.DataRow _row = this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID);
            //				
            //			string _strValue="" ;
            //			int _iValue=0;
            //
            //			for(int i=0;i<this.dsData.Tables["dtMemoryRL"].Columns.Count; i++)
            //			{
            //				if(_row[i].GetType().Equals( _strValue.GetType()))
            //				{
            //					_strValue = (IsNull(_row[i]))?"":_row[i].ToString();
            //					_sqlParam[i] = new System.Data.SqlClient.SqlParameter("@"+this.dsData.Tables["dtMemoryRL"].Columns[i].ColumnName.Trim(),_strValue);
            //				}
            //				else if(_row[i].GetType().Equals( _iValue.GetType()))
            //				{
            //					_iValue = (IsNull(_row[i]))?(Functions.MinValue):Convert.ToInt32(_row[i].ToString());
            //					_sqlParam[i] = new System.Data.SqlClient.SqlParameter("@"+this.dsData.Tables["dtMemoryRL"].Columns[i].ColumnName.Trim(),_iValue);
            //				}
            //				else   // null  system.dbnull
            //				{
            //					_sqlParam[i] = new System.Data.SqlClient.SqlParameter("@"+this.dsData.Tables["dtMemoryRL"].Columns[i].ColumnName.Trim(),Functions.MinValue);
            //				}
            //			}
            //
            //			//actula saving call
            //			System.Data.DataTable _dtTemp = BLL.DataHandelingManager.DataStoredProcedure("P_SaveRL",_sqlParam);
            //			
            System.Data.DataTable[] _dtTemp = new DataTable[this.dataRL.RoutingPathCount(strTabID)];

            for (int i = 0; i < this.dataRL.RoutingPathCount(strTabID); i++)
            {
                _dtTemp[i] = Prana.Admin.RoutingLogic.BLL.DataCallFunctionsManager.SaveRLogic(strMemoryID, strTabID, i);
            }

            //			if(Functions.IsNull(_dtTemp[0]))
            //			{
            //				MessageBox.Show(" Saving RL Failed ");
            //				return Functions.MinValue;
            //			}
            //			return int.Parse(_dtTemp[0].Rows[0][0].ToString());




            int _iIDRetunedFromSave;
            string _strKey = "";
            const string _cPrefixRL = "r";


            if (Functions.IsNull(_dtTemp[0]))
            {
                MessageBox.Show(" Saving RL Failed ");
                _iIDRetunedFromSave = Functions.MinValue;
            }
            else
            {
                _iIDRetunedFromSave = int.Parse(_dtTemp[0].Rows[0][0].ToString());
            }
            _strKey = _cPrefixRL + ":" + _iIDRetunedFromSave.ToString();
            //tree modification
            if (_iIDRetunedFromSave >= 0)
            {
                //				return ;
                //			}

                //				const string _cPrefixRL = "r";
                //				const string _cPrefixClient = "c";
                //				const string _cPrefixGroup = "g";
                const string _strKeyHeadingRL = "r:-1";
                //				const string _strKeyHeadingClient = "c:-2";
                //				const string _strKeyHeadingGroup = "g:-3";
                //			const string strTabIDRL="RL";
                //			const string strTabIDGroup="group";
                //			const string strTabIDClient="client";
                //			string _strTabID;

                System.Windows.Forms.NodeTree _nodeRL = (System.Windows.Forms.NodeTree)(this.nodeMain[_strKeyHeadingRL]);
                //				System.Windows.Forms.NodeTree _nodeClient=(System.Windows.Forms.NodeTree)(this.nodeMain.Nodes.GetItem(this.nodeMain.IndexOf(_strKeyHeadingClient)));
                //				System.Windows.Forms.NodeTree _nodeClientGrp=(System.Windows.Forms.NodeTree)(_nodeClient.Nodes.GetItem(_nodeClient.IndexOf(_strKeyHeadingGroup)));


                _strKey = _cPrefixRL + ":" + _iIDRetunedFromSave.ToString();
                string _strName = this.dataRL.Name(strTabID);//this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["RLName"].ToString();

                int _iIndex = _nodeRL.IndexOf(_strKey);
                if (_iIndex >= 0)
                {
                    //					_nodeRL.Nodes.Remove((System.Windows.Forms.NodeTree)_nodeRL.Nodes.GetItem(_iIndex));
                    //					_nodeRL.Nodes.Insert(_iIndex, _strKey, _strName);	
                    _nodeRL.ChangeAtIndex(_iIndex, _strKey, _strName);
                }
                else
                {
                    if (this.dataRL.ID(strTabID) >= 0)//! IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["RLID"]))
                    {
                        //remving older one

                        string _strKeyOld = _cPrefixRL + ":" + this.dataRL.ID(strTabID).ToString();//this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["RLID"].ToString();
                        int _iIndexOld = _nodeRL.IndexOf(_strKeyOld);
                        //						if(_iIndexOld >= 0)
                        //						{
                        //							_nodeRL.Nodes.Remove((System.Windows.Forms.NodeTree)_nodeRL.Nodes.GetItem(_iIndexOld));
                        //						}
                        _nodeRL.ChangeAtIndex(_iIndexOld, _strKey, _strName);
                    }
                    else
                    {

                        _nodeRL.Add(_strKey, _strName);
                    }


                    if (this.dsData.Tables.Contains("dtRLList"))
                    {
                        object[] _objNewRL = new object[] { this.dataRL.RoutingPathID(strTabID, 0), this.dataRL.RoutingPathName(strTabID, 0), this.dataRL.AUECID(strTabID) };
                        this.dsData.Tables["dtRLList"].Rows.Add(_objNewRL);
                    }
                    this.dataRL.ID(strTabID, _iIDRetunedFromSave);
                    //					this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["RLID"] = Convert.ToInt32(_strKey.Substring("r:".Length));
                }

                //				this.treeMain.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);
                //				_nodeRL[_strKey].Selected=true;
                //				this.treeMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);

            }

            return _strKey;

        }

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
