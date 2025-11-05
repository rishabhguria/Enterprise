using Prana.Admin.RoutingLogic.MisclFunctions;

namespace Prana.Admin.RoutingLogic.Controls
{
    /// <summary>
    /// Summary description for AndOr.
    /// </summary>
    public class AndOr : System.Windows.Forms.UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Data.DataSet dsData; private BLL.DataRoutingLogicObjects dataRL;
        private string strMemoryID;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet optionset;
        private string strTabID;
        private int iRoutingIndex;

        public AndOr()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

            //			this.optionset.Leave += new System.EventHandler(Functions.object_LostFocus);
            //			this.optionset.Enter += new System.EventHandler(Functions.object_GotFocus);

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
                if (optionset != null)
                {
                    optionset.Dispose();
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
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance("AND");
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance("OR");
            this.optionset = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            ((System.ComponentModel.ISupportInitialize)(this.optionset)).BeginInit();
            this.SuspendLayout();
            // 
            // optionset
            // 
            this.optionset.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.optionset.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.optionset.ItemAppearance = appearance1;
            appearance2.FontData.Name = "Tahoma";
            appearance2.FontData.SizeInPoints = 7.25F;
            appearance2.Tag = 0;
            valueListItem1.Appearance = appearance2;
            valueListItem1.DataValue = 0;
            valueListItem1.DisplayText = "AND";
            valueListItem1.Tag = 0;
            appearance3.FontData.Name = "Tahoma";
            appearance3.FontData.SizeInPoints = 7.25F;
            appearance3.Tag = 1;
            valueListItem2.Appearance = appearance3;
            valueListItem2.DataValue = 1;
            valueListItem2.DisplayText = "OR";
            valueListItem2.Tag = 1;
            this.optionset.Items.Add(valueListItem1);
            this.optionset.Items.Add(valueListItem2);
            this.optionset.Location = new System.Drawing.Point(0, 0);
            this.optionset.Name = "optionset";
            this.optionset.Size = new System.Drawing.Size(72, 14);
            this.optionset.TabIndex = 2;
            this.optionset.ValueChanged += new System.EventHandler(this.CommitMemory);
            // 
            // AndOr
            // 
            this.Controls.Add(this.optionset);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.Name = "AndOr";
            this.Size = new System.Drawing.Size(72, 14);
            ((System.ComponentModel.ISupportInitialize)(this.optionset)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        //		#region return selected condition
        //
        //		/// <summary>
        //		/// returns the selection if any in AND or OR.
        //		/// </summary>
        //		/// <returns>|| for or   & && for and  : default  &&</returns>
        //		public string selectedCondition()
        //		{
        //			bool bOr = this.rbOr.Checked;
        //			
        //			if (bOr)
        //			{
        //				return "||";
        //			}
        //			else
        //			{
        //				return "&&";
        //			}
        //		}
        //
        //		#endregion

        #region LoadData

        public void LoadData(ref System.Data.DataSet _dsData, ref BLL.DataRoutingLogicObjects _dataRL, string _strMemoryID, string _strTabID, int _iRoutingIndex)
        {
            this.dsData = _dsData; this.dataRL = _dataRL;
            this.strMemoryID = _strMemoryID;
            this.strTabID = _strTabID;
            this.iRoutingIndex = _iRoutingIndex;
            // 0-> and     1-> Or

            string _strIndex = this.Tag.ToString().Trim().Remove(0, "AndOr".Length);

            if (int.Parse(_strIndex) >= this.dataRL.ConditionsCount(strTabID, this.iRoutingIndex))
            {
                this.Hide();
                return;
            }
            else
            {
                this.Show();
            }

            //			int _iJoining = (Functions.IsNull(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["JoinCondition"+_strIndex]))? 0 : int.Parse(((this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["JoinCondition"+_strIndex])).ToString());
            int _iJoining = this.dataRL.JoinConditonID(strTabID, this.iRoutingIndex, int.Parse(_strIndex));
            ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = false;
            if (_iJoining == 0)
            {
                //this.rbAnd.Checked = true;
                this.optionset.Value = 0;
            }
            else
            {
                //this.rbOr.Checked = true;
                this.optionset.Value = 1;
            }
            ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = true;



        }

        #endregion

        private void CommitMemory(object sender, System.EventArgs e)
        {
            //			if(Functions.IsNull(this.dsData.Tables["dtMemoryRL"]))
            //			{
            //				return;
            //			}
            int _iIndex = int.Parse(this.Tag.ToString().Trim().Remove(0, "AndOr".Length));
            //			string _strColJoinCondition = "JoinCondition" + _strIndex ;
            if (Functions.IsNull(this.dataRL) || iRoutingIndex >= this.dataRL.RoutingPathCount(strTabID) || _iIndex >= this.dataRL.ConditionsCount(strTabID, iRoutingIndex))
            {
                return;
            }


            int _iValue = int.Parse(this.optionset.Value.ToString());
            int _iValueOld = this.dataRL.JoinConditonID(strTabID, iRoutingIndex, _iIndex);

            if (_iValue != _iValueOld)
            {
                ((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID, iRoutingIndex, true);

            }

            if (_iValue == 1)
            {

                //				this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)[_strColJoinCondition] = 1;
                this.dataRL.JoinConditonID(strTabID, iRoutingIndex, _iIndex, 1);
            }
            else
            {
                //				this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)[_strColJoinCondition] = 0;
                this.dataRL.JoinConditonID(strTabID, iRoutingIndex, _iIndex, 0);
            }





        }


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
        ////		#endregion

    }
}
