using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.Tools.PL.SecMaster
{
    public partial class ctrlDynamicUDASymbol : UserControl
    {
        public ctrlDynamicUDASymbol()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Binds the dynamic UDAs panels and dropdown to Security Information Template
        /// </summary>
        /// <param name="_dynamicUDAcache"></param>
        public void BindDynamicUDAs(SerializableDictionary<string, DynamicUDA> _dynamicUDAcache)
        {
            try
            {
                if (_dynamicUDAcache != null && _dynamicUDAcache.Count > 0)
                {
                    int xlocation = 24, ylocation = 24, noOfUDAs = 0;

                    foreach (string uda in _dynamicUDAcache.Keys)
                    {
                        if (this.ultraPanel1.ClientArea.Controls.ContainsKey("lbl" + uda) || this.ultraPanel1.ClientArea.Controls.ContainsKey("ugpc" + uda))
                        {
                            this.ultraPanel1.ClientArea.Controls.RemoveByKey("lbl" + uda);
                            this.ultraPanel1.ClientArea.Controls.RemoveByKey("ugpc" + uda);
                        }
                    }

                    foreach (string uda in _dynamicUDAcache.Keys)
                    {
                        if (!this.ultraPanel1.ClientArea.Controls.ContainsKey("lbl" + uda) && !this.ultraPanel1.ClientArea.Controls.ContainsKey("ugpc" + uda))
                        {
                            Infragistics.Win.Misc.UltraLabel ultralabel = new Infragistics.Win.Misc.UltraLabel();
                            Infragistics.Win.Appearance appearance = new Infragistics.Win.Appearance();
                            appearance.BackColor = System.Drawing.Color.Transparent;
                            ultralabel.Appearance = appearance;
                            ultralabel.AutoSize = true;
                            ultralabel.Location = new System.Drawing.Point(xlocation, ylocation);
                            ultralabel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
                            ultralabel.Name = "lbl" + uda;
                            //ultralabel.Size = new System.Drawing.Size(95, 18);
                            ultralabel.TabIndex = 90;
                            ultralabel.Tag = "lbl" + uda;
                            ultralabel.Text = _dynamicUDAcache[uda].HeaderCaption + ":";
                            //Added to set maximum size of ultralabel control, PRANA-12359
                            ultralabel.MaximumSize = new System.Drawing.Size(150, 35);
                            this.ultraPanel1.ClientArea.Controls.Add(ultralabel);

                            Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcUDA = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
                            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
                            appearance2.ForeColor = System.Drawing.Color.Black;
                            ugpcUDA.ColumnKey = uda;
                            ugpcUDA.EditAppearance = appearance2;
                            ugpcUDA.Location = new System.Drawing.Point(xlocation + 150, ylocation);
                            ugpcUDA.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
                            ugpcUDA.Name = "ugpc" + uda;
                            ugpcUDA.Size = new System.Drawing.Size(150, 24);
                            ugpcUDA.TabIndex = 32;
                            ugpcUDA.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
                            ugpcUDA.Text = "Band or column does not exist.";
                            this.ultraPanel1.ClientArea.Controls.Add(ugpcUDA);

                            noOfUDAs++;
                            if ((noOfUDAs % 2) == 0)
                            {
                                xlocation -= 450;
                                ylocation += 35;
                            }
                            else
                            {
                                xlocation += 450;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
