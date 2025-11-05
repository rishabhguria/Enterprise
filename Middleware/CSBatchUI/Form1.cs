using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace CSBatchUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dtFrom.Value = DateTime.Now.AddDays(-2);
            dtTo.Value = DateTime.Now.AddDays(-1);

        }

        private void btnRunBatch_Click(object sender, EventArgs e)
        {
            try
            {
                using (Process batch = new Process())
                {
                    batch.StartInfo.FileName = ".\\CSBatch.exe";
                    batch.StartInfo.Arguments = string.Format("@FromDate={0},@ToDate={1}, @Cores={2},@Step={3},@AdjustFrom=0,@AdjustTo=0", 
                        dtFrom.Value.ToShortDateString(), dtTo.Value.ToShortDateString(), nCores.Value, nStep.Value);
                    
                    batch.Start();
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            

        }

        private void btnCancelBatch_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBatch.Text.Trim().Length == 0) return;

                string cmd = string.Format("CSBatch.exe @FromDate={0},@ToDate={1}, @Cores={2},@Step={3}",
                            dtFrom.Value.ToShortDateString(), dtTo.Value.ToShortDateString(), nCores.Value, nStep.Value);


                if (txtBatch.Text.ToLower().EndsWith("cmd") == false)
                    txtBatch.Text += ".cmd";

                File.WriteAllText(txtBatch.Text, cmd);
                MessageBox.Show("Done creating batch file");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

        }
    }
}
