using Prana.Admin.RoutingLogic.MisclFunctions;
using System.Collections;


namespace Prana.Admin.RoutingLogic.Controls
{
    /// <summary>
    /// Summary description for Miscl.
    /// </summary>
    public class Miscl : System.Windows.Forms.UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbedt;

        private System.Data.DataSet dsData; private BLL.DataRoutingLogicObjects dataRL;
        private System.String strMemoryID;
        private string strTabID;
        private int iRoutingIndex;

        public Miscl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            this.cmbedt.Leave += new System.EventHandler(Functions.object_LostFocus);
            this.cmbedt.Enter += new System.EventHandler(Functions.object_GotFocus);

        }
        public Miscl(ref System.Data.DataSet _dsData, ref BLL.DataRoutingLogicObjects _dataRL, string _strIndex, string _strMemoryID, string _strTabID, int _iRoutingIndex)
        {
            this.dsData = _dsData; this.dataRL = _dataRL; this.dataRL = _dataRL;
            this.Tag = _strIndex;
            this.iRoutingIndex = _iRoutingIndex;
            this.strMemoryID = _strMemoryID;
            this.strTabID = _strTabID;
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            LoadData();

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
                if (cmbedt != null)
                {
                    cmbedt.Dispose();
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
            this.cmbedt = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedt)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbedt
            // 
            this.cmbedt.DropDownListWidth = 150;
            this.cmbedt.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbedt.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbedt.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.cmbedt.Location = new System.Drawing.Point(0, 0);
            this.cmbedt.Name = "cmbedt";
            this.cmbedt.Size = new System.Drawing.Size(100, 20);
            this.cmbedt.TabIndex = 0;
            this.cmbedt.Tag = "cmbedt";
            this.cmbedt.SelectionChangeCommitted += new System.EventHandler(this.SaveMem);
            this.cmbedt.ValueChanged += new System.EventHandler(this.cmbedt_ValueChanged);
            // 
            // Miscl
            // 
            this.Controls.Add(this.cmbedt);
            this.Name = "Miscl";
            this.Size = new System.Drawing.Size(100, 20);
            ((System.ComponentModel.ISupportInitialize)(this.cmbedt)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region LoadData

        private void LoadData()
        {
            string _strIndex = this.Tag.ToString().Trim();
            if (int.Parse(_strIndex) >= this.dataRL.ConditionsCount(strTabID, iRoutingIndex))
            {
                this.Hide();
                return;
            }
            else
            {
                this.Show();
            }


            LoadOperator();
            //			object _objMemoryValue = this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ParameterValue"+_strIndex];
            //
            //
            //			this.cmbedt.Value = (IsNull(_objMemoryValue))? 0:Convert.ToInt32(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ParameterValue"+_strIndex].ToString());
            if (!(Functions.IsNull(this.ParentForm)))
            {
                ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = false;
            }
            this.cmbedt.Value = this.dataRL.ParameterValue(strTabID, iRoutingIndex, int.Parse(_strIndex));
            //			this.cmbedt.Value = ((Infragistics.Win.ValueListItem)(((this.cmbedt.Items.GetItem(0))))).DataValue;

            if (!(Functions.IsNull(this.ParentForm)))
            {
                ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = true;
            }


        }

        #endregion

        #region loaing options
        private void LoadOperator()
        {
            string _strExchangeName = "exchange";
            string _strIndex = this.Tag.ToString().Trim();

            //			bool _bIsExchangeSelected = int.Parse(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ParameterID"+_strIndex].ToString()) == int.Parse(((this.dsData.Tables["dtParameters"].Select(" ParamName = '" + _strExchangeName+"'"))[0]["ParamID"]).ToString());
            bool _bIsExchangeSelected = this.dataRL.ParameterID(strTabID, iRoutingIndex, int.Parse(_strIndex)) == int.Parse(((this.dsData.Tables["dtParameters"].Select(" ParamName = '" + _strExchangeName + "'"))[0]["ParamID"]).ToString());


            System.Data.DataRow[] _rowarraySelectedRows;

            if (_bIsExchangeSelected)
            {
                _rowarraySelectedRows = this.dsData.Tables["dtParameters"].Select("ParamID = " + this.dataRL.ParameterID(strTabID, iRoutingIndex, int.Parse(_strIndex)).ToString());
                //				foreach(System.Data.DataRow _drRowSelect in this.dsData.Tables["dtParameters"].Select("ParamID = "+ this.dataRL.ParameterID(strTabID,iRoutingIndex,int.Parse(_strIndex)).ToString() ))// this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ParameterID"+_strIndex].ToString() ))
                //				{
                //					this.cmbedt.Items.Add(int.Parse(_drRowSelect["ID"].ToString()),_drRowSelect["Name"].ToString());
                //				}

            }
            else
            {
                _rowarraySelectedRows = this.dsData.Tables["dtParameters"].Select("ParamID = " + this.dataRL.ParameterID(strTabID, iRoutingIndex, int.Parse(_strIndex)).ToString() + " AND AUEC= " + this.dataRL.AUECID(strTabID).ToString());
                //				foreach(System.Data.DataRow _drRowSelect in this.dsData.Tables["dtParameters"].Select("ParamID = "+ this.dataRL.ParameterID(strTabID,iRoutingIndex,int.Parse(_strIndex)).ToString() + " AND AUEC= "+  this.dataRL.AUECID(strTabID).ToString() ))//this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ParameterID"+_strIndex].ToString() + " AND AUEC= "+ this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AUECID"].ToString()))
                //				{
                //					this.cmbedt.Items.Add(int.Parse(_drRowSelect["ID"].ToString()),_drRowSelect["Name"].ToString());
                //				}
            }

            System.Collections.ArrayList _alCheckID = new ArrayList();
            string _strName = "";
            int _iID = Functions.MinValue;

            foreach (System.Data.DataRow _drRowSelect in _rowarraySelectedRows)
            {
                _iID = int.Parse(_drRowSelect["ID"].ToString());
                _strName = _drRowSelect["Name"].ToString();

                if (!_alCheckID.Contains(_iID))
                {
                    _alCheckID.Add(_iID);

                    this.cmbedt.Items.Add(_iID, _strName);
                }
            }

        }

        #endregion

        #region saving to meme

        private void SaveMem(object sender, System.EventArgs e)
        {

            //			string _strParameterName = "ParameterValue" + this.Tag.ToString().Trim() ;

            int _iIndex = int.Parse(this.Tag.ToString().Trim());
            string _strValue = this.cmbedt.SelectedItem.DataValue.ToString();
            string _strValueOld = this.dataRL.ParameterValue(strTabID, iRoutingIndex, _iIndex);

            if (!_strValue.Equals(_strValueOld))
            {
                ((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID, iRoutingIndex, true);

            }

            //			this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)[_strParameterName] = int.Parse(this.cmbedt.SelectedItem.DataValue.ToString());


            //			if(this.dataRL.RoutingPathCount(strTabID)<= iRoutingIndex )
            //			{
            //				this.dataRL.RoutingPathCount(strTabID,iRoutingIndex+1);
            //			}
            this.dataRL.ParameterValue(strTabID, iRoutingIndex, _iIndex, _strValue);

            //						((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID,iRoutingIndex, true);
        }

        #endregion

        private void cmbedt_ValueChanged(object sender, System.EventArgs e)
        {

        }
        //
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
