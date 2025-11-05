using Prana.BusinessObjects;
//using Prana.BusinessObjects.Classes.Analytics;
using Prana.LogManager;
using System;
using System.Data;
using System.Windows.Forms;

namespace Prana.Admin.Controls.RiskPrefs
{
    public partial class RiskPrefsCtrl : UserControl
    {
        public RiskPrefsCtrl()
        {
            InitializeComponent();
        }

        public void SetRiskUIFromPrefs(DataSet ds)
        {
            try
            {
                //modified by omshiv, if risk preferences on found
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int MaxStressTestViewsWithVolSkew = int.Parse(ds.Tables[0].Rows[0]["MaxStressTestViewsWithVolSkew"].ToString());
                    int MaxStressTestViewsWithoutVolSkew = int.Parse(ds.Tables[0].Rows[0]["MaxStressTestViewsWithoutVolSkew"].ToString());
                    if (MaxStressTestViewsWithVolSkew <= numericUpDown1.Maximum && MaxStressTestViewsWithVolSkew >= numericUpDown1.Minimum)
                    {
                        numericUpDown1.Value = MaxStressTestViewsWithVolSkew;
                    }
                    else
                    {
                        numericUpDown1.Value = numericUpDown1.Maximum;
                    }
                    if (MaxStressTestViewsWithoutVolSkew <= numericUpDown2.Maximum && MaxStressTestViewsWithoutVolSkew >= numericUpDown2.Minimum)
                    {
                        numericUpDown2.Value = MaxStressTestViewsWithoutVolSkew;
                    }
                    else
                    {
                        numericUpDown2.Value = numericUpDown2.Maximum;
                    }

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                throw;
            }
        }

        public RiskPrefernece GetRiskPrefsFromUI()
        {
            RiskPrefernece selectedPrefs = new RiskPrefernece();
            try
            {
                selectedPrefs.MaxStressTestViewsWithVolSkew = (int)numericUpDown1.Value;
                selectedPrefs.MaxStressTestViewsWithoutVolSkew = (int)numericUpDown2.Value;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                throw;
            }
            return selectedPrefs;

        }

    }
}
