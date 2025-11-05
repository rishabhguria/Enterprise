using Prana.Admin.RoutingLogic.MisclFunctions;
using System;
using System.Text;

namespace Prana.Admin.RoutingLogic.Controls
{
    /// <summary>
    /// Summary description for Quantity.
    /// </summary>
    public class Quantity : System.Windows.Forms.UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtedtQuantity;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbedtLogicOperator;

        private System.Data.DataSet dsData; private BLL.DataRoutingLogicObjects dataRL;
        private System.String strMemoryID;
        private string strTabID;
        private int iRoutingIndex;

        public Quantity()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

            this.txtedtQuantity.Leave += new System.EventHandler(Functions.object_LostFocus);
            this.txtedtQuantity.Enter += new System.EventHandler(Functions.object_GotFocus);
            this.cmbedtLogicOperator.Leave += new System.EventHandler(Functions.object_LostFocus);
            this.cmbedtLogicOperator.Enter += new System.EventHandler(Functions.object_GotFocus);

        }
        public Quantity(ref System.Data.DataSet _dsData, ref BLL.DataRoutingLogicObjects _dataRL, string _strIndex, string _strMemoryID, string _strTabID, int _iRoutingIndex)
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
                if (txtedtQuantity != null)
                {
                    txtedtQuantity.Dispose();
                }
                if (cmbedtLogicOperator != null)
                {
                    cmbedtLogicOperator.Dispose();
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
            this.cmbedtLogicOperator = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.txtedtQuantity = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtLogicOperator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtedtQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbedtLogicOperator
            // 
            this.cmbedtLogicOperator.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbedtLogicOperator.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbedtLogicOperator.Location = new System.Drawing.Point(0, 0);
            this.cmbedtLogicOperator.Name = "cmbedtLogicOperator";
            this.cmbedtLogicOperator.Size = new System.Drawing.Size(40, 20);
            this.cmbedtLogicOperator.TabIndex = 0;
            this.cmbedtLogicOperator.Tag = "cmbedtLogicOperator";
            this.cmbedtLogicOperator.SelectionChangeCommitted += new System.EventHandler(this.SaveMemOperator);
            // 
            // txtedtQuantity
            // 
            appearance1.TextHAlign = Infragistics.Win.HAlign.Right;
            this.txtedtQuantity.Appearance = appearance1;
            this.txtedtQuantity.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtedtQuantity.Location = new System.Drawing.Point(40, 0);
            this.txtedtQuantity.Name = "txtedtQuantity";
            this.txtedtQuantity.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtedtQuantity.Size = new System.Drawing.Size(60, 20);
            this.txtedtQuantity.TabIndex = 1;
            this.txtedtQuantity.Tag = "txtedtQuantity";
            this.txtedtQuantity.Text = "1";
            this.txtedtQuantity.WordWrap = false;
            this.txtedtQuantity.Leave += new System.EventHandler(this.SaveMemText);
            // 
            // Quantity
            // 
            this.Controls.Add(this.txtedtQuantity);
            this.Controls.Add(this.cmbedtLogicOperator);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.Name = "Quantity";
            this.Size = new System.Drawing.Size(100, 20);
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtLogicOperator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtedtQuantity)).EndInit();
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
            //				this.txtedtQuantity.Text = (IsNull(_objMemoryValue))?"":(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ParameterValue"+_strIndex]).ToString();
            //
            //			_objMemoryValue = this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["OperatorID"+_strIndex];
            //			this.cmbedtLogicOperator.Value = Convert.ToInt32((IsNull(_objMemoryValue))? Functions.MinValue.ToString():this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["OperatorID"+_strIndex].ToString());
            //
            if (!(Functions.IsNull(this.ParentForm)))
            {
                ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = false;
            }
            this.txtedtQuantity.Text = this.dataRL.ParameterValue(strTabID, iRoutingIndex, int.Parse(_strIndex));
            if (!(Functions.IsNull(this.ParentForm)))
            {
                ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = true;
            }
            if (!(Functions.IsNull(this.ParentForm)))
            {
                ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = false;
            }
            this.cmbedtLogicOperator.Value = this.dataRL.OperatorID(strTabID, iRoutingIndex, int.Parse(_strIndex));
            if (!(Functions.IsNull(this.ParentForm)))
            {
                ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = true;
            }

        }

        #endregion


        #region loaing oprtrs
        private void LoadOperator()
        {
            string _strIndex = this.Tag.ToString().Trim();

            foreach (System.Data.DataRow _drRowSelect in this.dsData.Tables["dtParameters"].Select("ParamID = " + this.dataRL.ParameterID(strTabID, iRoutingIndex, Convert.ToInt32(_strIndex)).ToString()))//this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ParameterID"+_strIndex]))
            {
                this.cmbedtLogicOperator.Items.Add(int.Parse(_drRowSelect["OperatorID"].ToString()), _drRowSelect["OperatorName"].ToString());
            }
        }

        #endregion

        private void SaveMemText(object sender, System.EventArgs e)
        {
            //			string _strParameterName = "ParameterValue" + this.Tag.ToString().Trim() ;

            string _strValue = this.txtedtQuantity.Text;
            string _strValueOld = this.dataRL.ParameterValue(strTabID, iRoutingIndex, int.Parse(this.Tag.ToString().Trim())).Trim();

            char[] cCharEntered = _strValue.ToCharArray(0, ((int)_strValue.Length));
            Encoding eAscii = Encoding.ASCII;

            foreach (char cChar in cCharEntered)
            {
                string sChar = cChar.ToString();
                Byte[] bCharAscii = eAscii.GetBytes(sChar);

                // Ascii code for '0' is 48 while that of '9' is 57
                bool bNumeric = (((int)(bCharAscii[0]) > 47) && ((int)(bCharAscii[0]) < 58));

                if (!bNumeric)
                {
                    //					MessageBox.Show(" ' "+ sChar + " ' Is not allowed. "+   " Only positive integer numbers " );
                    _strValue = _strValue.Replace(sChar, "");
                }
            }

            _strValue = _strValue.Trim();
            if (_strValue.Equals(""))
            {
                _strValue = "0";
            }

            this.txtedtQuantity.Text = _strValue;



            if (!_strValue.Equals(_strValueOld))
            {
                ((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID, iRoutingIndex, true);

            }

            //			this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)[_strParameterName] = this.txtedtQuantity.Text;

            this.dataRL.ParameterValue(strTabID, iRoutingIndex, int.Parse(this.Tag.ToString().Trim()), _strValue);
            //
            //						((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID,iRoutingIndex, true);
        }

        private void SaveMemOperator(object sender, System.EventArgs e)
        {
            string _strColOperatorID = "OperatorID" + this.Tag.ToString().Trim();

            int _iValue = int.Parse(this.cmbedtLogicOperator.SelectedItem.DataValue.ToString());
            int _iValueOld = this.dataRL.OperatorID(strTabID, iRoutingIndex, int.Parse(this.Tag.ToString().Trim()));

            if (_iValue != _iValueOld)
            {
                ((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID, iRoutingIndex, true);

            }


            //			this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)[_strColOperatorID] = int.Parse(this.cmbedtLogicOperator.SelectedItem.DataValue.ToString());

            //			int _iIndex = int.Parse(this.Tag.ToString().Trim()) ;
            //			if(this.dataRL.RoutingPathCount(strTabID)<= _iIndex )
            //			{
            //				this.dataRL.RoutingPathCount(strTabID,_iIndex);
            //			}
            this.dataRL.OperatorID(strTabID, iRoutingIndex, int.Parse(this.Tag.ToString().Trim()), _iValue);

            //						((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID,iRoutingIndex, true);
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
        //		#endregion
    }
}
