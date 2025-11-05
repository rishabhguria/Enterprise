namespace Prana.Admin.RoutingLogic.Controls
{
    /// <summary>
    /// Summary description for IfThenCondition.
    /// </summary>
    /// 
    //	public delegate void DelegateLoadData(ref System.Data.DataSet _dsData, ref BLL.DataRoutingLogicObjects _dataRL);

    public class IfThenCondition : System.Windows.Forms.UserControl
    {
        private Controls.IfThen ucIfThen;
        private Controls.IfCondition ucIfCondition;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        //		private System.Data.DataSet dsData;    private BLL.DataRoutingLogicObjects dataRL;

        public IfThenCondition()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

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
                if (ucIfThen != null)
                {
                    ucIfThen.Dispose();
                }
                if (ucIfCondition != null)
                {
                    ucIfCondition.Dispose();
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
            this.ucIfThen = new Prana.Admin.RoutingLogic.Controls.IfThen();
            this.ucIfCondition = new Prana.Admin.RoutingLogic.Controls.IfCondition();
            this.SuspendLayout();
            // 
            // ucIfThen
            // 
            this.ucIfThen.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.ucIfThen.Location = new System.Drawing.Point(88, 24);
            this.ucIfThen.Name = "ucIfThen";
            this.ucIfThen.Size = new System.Drawing.Size(448, 90);
            this.ucIfThen.TabIndex = 0;
            // 
            // ucIfCondition
            // 
            this.ucIfCondition.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.ucIfCondition.Location = new System.Drawing.Point(0, 0);
            this.ucIfCondition.Name = "ucIfCondition";
            this.ucIfCondition.Size = new System.Drawing.Size(635, 21);
            this.ucIfCondition.TabIndex = 0;
            // 
            // IfThenCondition
            // 
            this.Controls.Add(this.ucIfCondition);
            this.Controls.Add(this.ucIfThen);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.Name = "IfThenCondition";
            this.Size = new System.Drawing.Size(636, 114);
            this.ResumeLayout(false);

        }
        #endregion

        #region LoadData

        public void LoadData(ref System.Data.DataSet _dsData, ref BLL.DataRoutingLogicObjects _dataRL, string _strMemoryID, string _strTabID, int _iRoutingIndex)
        {
            //			this.dsData = _dsData;      this.dataRL = _dataRL;      this.dataRL = _dataRL;
            this.ucIfCondition.LoadData(ref _dsData, ref _dataRL, _strMemoryID, _strTabID, _iRoutingIndex);
            this.ucIfThen.LoadData(ref _dsData, ref _dataRL, _strMemoryID, _strTabID, _iRoutingIndex);
            //			this.Refresh();

        }

        #endregion
    }
}
