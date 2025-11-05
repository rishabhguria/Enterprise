using Infragistics.Win.UltraWinEditors;
using Prana.Admin.RoutingLogic.MisclFunctions;
using System;
using System.Collections;
using System.Windows.Forms;

namespace Prana.Admin.RoutingLogic.Controls
{
    /// <summary>
    /// Summary description for IfCondition.
    /// </summary>
    public class IfCondition : System.Windows.Forms.UserControl
    {
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbedt1;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbedt2;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbedt3;
        private Controls.AndOr andOr0;
        private Controls.AndOr andOr1;
        private System.Windows.Forms.Panel panel0;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Collections.ArrayList alUCCmbEdt = new ArrayList();
        private System.Collections.ArrayList alUCAndOr = new ArrayList();
        private System.Collections.ArrayList alUCPanel = new ArrayList();


        private System.Data.DataSet dsData; private BLL.DataRoutingLogicObjects dataRL;
        private string strMemoryID;
        private string strTabID;
        private int iRoutingIndex;

        public IfCondition()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            this.alUCCmbEdt.Add(this.cmbedt1);
            this.alUCCmbEdt.Add(this.cmbedt2);
            this.alUCCmbEdt.Add(this.cmbedt3);
            this.alUCAndOr.Add(this.andOr0);
            this.alUCAndOr.Add(this.andOr1);
            this.alUCPanel.Add(this.panel0);
            this.alUCPanel.Add(this.panel1);
            this.alUCPanel.Add(this.panel2);

            for (int i = 0; i < this.alUCCmbEdt.Count; i++)
            {
                ((UltraComboEditor)this.alUCCmbEdt[i]).Leave += new System.EventHandler(Functions.object_LostFocus);
                ((UltraComboEditor)this.alUCCmbEdt[i]).Enter += new System.EventHandler(Functions.object_GotFocus);
            }

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
                if (cmbedt1 != null)
                {
                    cmbedt1.Dispose();
                }
                if (cmbedt2 != null)
                {
                    cmbedt2.Dispose();
                }
                if (cmbedt3 != null)
                {
                    cmbedt3.Dispose();
                }
                if (andOr0 != null)
                {
                    andOr0.Dispose();
                }
                if (andOr1 != null)
                {
                    andOr1.Dispose();
                }
                if (panel0 != null)
                {
                    panel0.Dispose();
                }
                if (panel1 != null)
                {
                    panel1.Dispose();
                }
                if (panel2 != null)
                {
                    panel2.Dispose();
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
            this.cmbedt1 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbedt2 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbedt3 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.panel0 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.andOr0 = new Prana.Admin.RoutingLogic.Controls.AndOr();
            this.andOr1 = new Prana.Admin.RoutingLogic.Controls.AndOr();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedt1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedt2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedt3)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbedt1
            // 
            this.cmbedt1.DropDownListWidth = 150;
            this.cmbedt1.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbedt1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbedt1.Location = new System.Drawing.Point(1, 1);
            this.cmbedt1.Name = "cmbedt1";
            this.cmbedt1.Size = new System.Drawing.Size(56, 20);
            this.cmbedt1.TabIndex = 0;
            this.cmbedt1.Tag = "cmbedt1";
            this.cmbedt1.ValueChanged += new System.EventHandler(this.SelectedParameter);
            // 
            // cmbedt2
            // 
            this.cmbedt2.DropDownListWidth = 150;
            this.cmbedt2.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbedt2.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbedt2.Location = new System.Drawing.Point(236, 1);
            this.cmbedt2.Name = "cmbedt2";
            this.cmbedt2.Size = new System.Drawing.Size(56, 20);
            this.cmbedt2.TabIndex = 1;
            this.cmbedt2.Tag = "cmbedt2";
            this.cmbedt2.ValueChanged += new System.EventHandler(this.SelectedParameter);
            // 
            // cmbedt3
            // 
            this.cmbedt3.DropDownListWidth = 150;
            this.cmbedt3.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbedt3.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbedt3.Location = new System.Drawing.Point(474, 1);
            this.cmbedt3.Name = "cmbedt3";
            this.cmbedt3.Size = new System.Drawing.Size(56, 20);
            this.cmbedt3.TabIndex = 2;
            this.cmbedt3.Tag = "cmbedt3";
            this.cmbedt3.ValueChanged += new System.EventHandler(this.SelectedParameter);
            // 
            // panel0
            // 
            this.panel0.Location = new System.Drawing.Point(59, 1);
            this.panel0.Name = "panel0";
            this.panel0.Size = new System.Drawing.Size(100, 20);
            this.panel0.TabIndex = 3;
            this.panel0.Tag = "panel0";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(295, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(100, 20);
            this.panel1.TabIndex = 4;
            this.panel1.Tag = "panel1";
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(533, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(100, 20);
            this.panel2.TabIndex = 5;
            this.panel2.Tag = "panel2";
            // 
            // andOr0
            // 
            this.andOr0.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.andOr0.Location = new System.Drawing.Point(164, 4);
            this.andOr0.Name = "andOr0";
            this.andOr0.Size = new System.Drawing.Size(70, 16);
            this.andOr0.TabIndex = 1;
            this.andOr0.Tag = "andOr0";
            // 
            // andOr1
            // 
            this.andOr1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.andOr1.Location = new System.Drawing.Point(401, 4);
            this.andOr1.Name = "andOr1";
            this.andOr1.Size = new System.Drawing.Size(68, 14);
            this.andOr1.TabIndex = 0;
            this.andOr1.Tag = "andOr1";
            // 
            // IfCondition
            // 
            this.Controls.Add(this.andOr1);
            this.Controls.Add(this.andOr0);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel0);
            this.Controls.Add(this.cmbedt3);
            this.Controls.Add(this.cmbedt2);
            this.Controls.Add(this.cmbedt1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.Name = "IfCondition";
            this.Size = new System.Drawing.Size(635, 21);
            this.Load += new System.EventHandler(this.IfCondition_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbedt1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedt2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedt3)).EndInit();
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

            foreach (Controls.AndOr _objAndOr in this.alUCAndOr)
            {
                _objAndOr.LoadData(ref dsData, ref dataRL, this.strMemoryID, this.strTabID, this.iRoutingIndex);
            }


            foreach (UltraComboEditor _objCmbLoop in this.alUCCmbEdt)
            {
                //				((Forms.CompanyMaster)(this.ParentForm)).UserTriggered=false;
                _objCmbLoop.Items.Clear();
                //				_objCmbLoop.Value="";
            }

            foreach (System.Windows.Forms.Panel _pParameterPanel in this.alUCPanel)
            {
                _pParameterPanel.Controls.Clear();
            }


            UltraComboEditor _objCmbEdt = (UltraComboEditor)this.alUCCmbEdt[0];

            LoadParamToCombo(ref _objCmbEdt);
            string _strIndex = (this.alUCCmbEdt.IndexOf(_objCmbEdt)).ToString();
            //			object _objMemValueObject = this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ParameterID"+_strIndex];
            //			int _iValue = int.Parse(IsNull(_objMemValueObject)?Functions.MinValue.ToString():(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ParameterID"+_strIndex].ToString()));
            //			
            if (this.dataRL.ConditionsCount(strTabID, iRoutingIndex) < (Convert.ToInt32(_strIndex) + 1))
            {
                this.dataRL.ConditionsCount(strTabID, iRoutingIndex, (Convert.ToInt32(_strIndex) + 1));

            }
            int _iValue = this.dataRL.ParameterID(strTabID, this.iRoutingIndex, 0);

            if (!((Convert.ToInt32(_strIndex) == 0) && (_iValue < 0)))
            {
                ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = false;
                _objCmbEdt.Value = _iValue;
                ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = true;
            }

            //			this.SelectedParameter(this.alUCCmbEdt[0],null);
            ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = true;
            //			((Forms.CompanyMaster)(this.ParentForm)).Refresh();



        }

        #endregion


        #region selected parameter
        private void SelectedParameter(object sender, System.EventArgs e)
        {
            //			try
            //			{
            UltraComboEditor _objCmbEdt = (UltraComboEditor)sender;
            int _iIndex = this.alUCCmbEdt.IndexOf(_objCmbEdt);


            //			if(_objCmbEdt.SelectedItem.DataValue.ToString().Equals(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ParameterID"].ToString())
            //			{
            //				return;
            //			}

            //				LoadParamToCombo(ref _objCmbEdt);
            //				_objCmbEdt.SelectionChangeCommitted -= new System.EventHandler(this.SelectedParameter);
            //				_objCmbEdt.Value =	int.Parse((this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ParameterID"+_iIndex.ToString()].Equals(null)?Functions.MinValue:this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ParameterID"]).ToString());
            //				_objCmbEdt.SelectionChangeCommitted += new System.EventHandler(this.SelectedParameter);

            foreach (UltraComboEditor _objCmbLoop in this.alUCCmbEdt)
            {
                if (this.alUCCmbEdt.IndexOf(_objCmbLoop) > _iIndex)
                {
                    if (!Functions.IsNull(_objCmbLoop.SelectedItem))
                    {
                        if (_objCmbLoop.SelectedItem.DataValue.Equals(_objCmbEdt.SelectedItem.DataValue))
                        {

                            _objCmbLoop.Value = Functions.MinValue;
                        }

                    }
                }
                else if (this.alUCCmbEdt.IndexOf(_objCmbLoop) < _iIndex)
                {
                    if (!Functions.IsNull(_objCmbLoop.SelectedItem))
                    {
                        if (Convert.ToInt32(_objCmbEdt.SelectedItem.DataValue.ToString()) >= 0)
                        {
                            if (_objCmbLoop.SelectedItem.DataValue.Equals(_objCmbEdt.SelectedItem.DataValue))
                            {

                                MessageBox.Show(" You Can't Choose The Already Choosen One. ");
                                _objCmbEdt.ValueChanged -= new System.EventHandler(this.SelectedParameter);
                                _objCmbEdt.Value = this.dataRL.ParameterID(strTabID, iRoutingIndex, _iIndex);
                                //									_objCmbEdt.Value = this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ParameterID"+_iIndex.ToString()];
                                _objCmbEdt.ValueChanged += new System.EventHandler(this.SelectedParameter);

                            }
                        }

                    }

                }
            }


            if (!Functions.IsNull(_objCmbEdt.SelectedItem))
            {
                //					this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ParameterID"+_iIndex.ToString()]=_objCmbEdt.SelectedItem.DataValue;
                if ((int.Parse(_objCmbEdt.SelectedItem.DataValue.ToString()) >= 0))
                {
                    if (this.dataRL.RoutingPathCount(strTabID) <= iRoutingIndex)
                    {
                        this.dataRL.RoutingPathCount(strTabID, iRoutingIndex + 1);
                    }

                    if (this.dataRL.ConditionsCount(strTabID, iRoutingIndex) <= _iIndex)
                    {
                        this.dataRL.ConditionsCount(strTabID, iRoutingIndex, _iIndex + 1);
                    }
                    ((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID, iRoutingIndex, true);
                    this.dataRL.ParameterID(strTabID, iRoutingIndex, _iIndex, int.Parse(_objCmbEdt.SelectedItem.DataValue.ToString()));
                }
            }
            if (_iIndex != (this.alUCCmbEdt.Count - 1))
            {
                UltraComboEditor _objCmbEdtNext = ((UltraComboEditor)(this.alUCCmbEdt[_iIndex + 1]));
                LoadParamToCombo(ref _objCmbEdtNext); // _objCmbEdt1);
                                                      //					((UltraComboEditor)this.alUCCmbEdt[_iIndex+1]).Value;
                string _strIndex = (1 + _iIndex).ToString();

                //					object _objMemNextObject = this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ParameterID"+_strIndex];
                //					int _iValueNextCmbEdt = int.Parse(IsNull(_objMemNextObject)?Functions.MinValue.ToString():(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ParameterID"+_strIndex].ToString()));
                int _iValueNextCmbEdt;
                if (int.Parse(_strIndex) >= this.dataRL.ConditionsCount(strTabID, iRoutingIndex))
                {
                    _iValueNextCmbEdt = Functions.MinValue;
                }
                else
                {
                    _iValueNextCmbEdt = this.dataRL.ParameterID(strTabID, iRoutingIndex, int.Parse(_strIndex));
                }
                ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = false;
                _objCmbEdtNext.Value = _iValueNextCmbEdt;
                ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = true;

                if (Functions.IsNull(_objCmbEdt.SelectedItem) || (int.Parse(_objCmbEdt.SelectedItem.DataValue.ToString()) < 0))
                {

                    _objCmbEdtNext.Value = Functions.MinValue;
                    _objCmbEdtNext.Hide();
                    ((Controls.AndOr)(this.alUCAndOr[_iIndex])).Hide();
                    ((System.Windows.Forms.Panel)(this.alUCPanel[_iIndex + 1])).Controls.Clear();

                }
                else
                {
                    _objCmbEdtNext.Show();
                    ((Controls.AndOr)(this.alUCAndOr[_iIndex])).Show();
                }


            }



            foreach (UltraComboEditor _objCmbLoop in this.alUCCmbEdt)
            {
                if (Functions.IsNull(_objCmbLoop.SelectedItem))
                {
                    continue;
                }
                if (int.Parse(_objCmbLoop.SelectedItem.DataValue.ToString()) == Functions.MinValue)
                {
                    this.dataRL.ConditionsCount(strMemoryID, iRoutingIndex, this.alUCCmbEdt.IndexOf(_objCmbLoop));
                    break;
                }

            }


            LoadPanelParameter(ref _objCmbEdt);

            return;
            //			}
            //			catch (Exception ex)
            //			{
            //				MessageBox.Show(ex.Message+ex.StackTrace+ex.InnerException+ex.Source+ex.ToString());
            //			}

        }

        #endregion

        #region load the corrsponding panel
        private void LoadPanelParameter(ref UltraComboEditor _objCmbEdt)
        {
            int _iIndex = this.alUCCmbEdt.IndexOf(_objCmbEdt);

            string _strSelectedText = _objCmbEdt.SelectedItem.DisplayText.Trim().ToLower();
            System.Windows.Forms.UserControl _obj;

            switch (_strSelectedText)
            {
                case "none":
                    _obj = null;
                    break;
                case "symbol":
                    _obj = new Controls.Symbol(ref dsData, ref dataRL, _iIndex.ToString(), strMemoryID, strTabID, this.iRoutingIndex);
                    break;
                case "quantity":
                    _obj = new Controls.Quantity(ref dsData, ref dataRL, _iIndex.ToString(), strMemoryID, strTabID, this.iRoutingIndex);
                    break;
                default:
                    _obj = new Controls.Miscl(ref dsData, ref dataRL, _iIndex.ToString(), strMemoryID, strTabID, this.iRoutingIndex);
                    break;
            }


            ((System.Windows.Forms.Panel)(this.alUCPanel[_iIndex])).Controls.Clear();
            if (_obj != null)
            {
                ((System.Windows.Forms.Panel)(this.alUCPanel[_iIndex])).Controls.Add(_obj);
            }
            //			else
            //			{
            ////				this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["
            //			}
        }
        #endregion

        #region loading data to combo
        private void LoadParamToCombo(ref UltraComboEditor _objCmbEdt)
        {
            //			((Forms.CompanyMaster)(this.ParentForm)).UserTriggered=false;
            _objCmbEdt.Items.Clear();
            int _iIndex = this.alUCCmbEdt.IndexOf(_objCmbEdt);
            System.Collections.ArrayList _alParamID = new ArrayList();
            bool _bRowIsThere;

            bool _bAlreadySelected = false;
            string _strParamID = "";

            if (_iIndex != 0)
            {
                _objCmbEdt.Items.Add(Functions.MinValue, "None");
            }

            foreach (System.Data.DataRow _drRowSelect in this.dsData.Tables["dtParameters"].Select())
            {
                _bRowIsThere = false;
                _strParamID = _drRowSelect["ParamID"].ToString();

                _bRowIsThere = _alParamID.Contains(_strParamID);
                _bAlreadySelected = false;

                if (!_bRowIsThere)
                {
                    foreach (UltraComboEditor _objCmbEdtOther in this.alUCCmbEdt)
                    {
                        if (this.alUCCmbEdt.IndexOf(_objCmbEdt) != this.alUCCmbEdt.IndexOf(_objCmbEdtOther))
                        {
                            if ((_objCmbEdtOther.SelectedItem == null) ? false : (_objCmbEdtOther.SelectedItem.DataValue.ToString().Equals(_strParamID)))
                            {
                                _bAlreadySelected = true;
                                break;
                            }
                        }
                    }

                    _alParamID.Add(_strParamID);

                    if (!_bAlreadySelected)
                    {
                        _objCmbEdt.Items.Add(int.Parse(_strParamID), _drRowSelect["ParamName"].ToString());
                    }
                }
            }

            ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = true;
            return;
        }
        #endregion

        private void IfCondition_Load(object sender, System.EventArgs e)
        {

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
