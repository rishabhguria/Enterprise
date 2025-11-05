using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using System.Windows.Forms;
using System.Windows;
using System.Drawing;
using Infragistics.Win.Misc;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Prana.AllocationNew
{
    class AllocationUIHelper
    {
        public static int AddAccountsLabels(AccountCollection _accounts, UltraPanel userControl)
        {

            UltraLabel[] lblAccounts = new UltraLabel[_accounts.Count];

            int i = 0;
            int startLabel_X = 15;
            int start_Y = 40;
            int yIncrement = 20;
            int lblLength = 20;

            try
            {
                for (i = 0; i < _accounts.Count; i++)
                {
                    Account account = (Account)(_accounts[i]);

                    lblAccounts[i] = new UltraLabel();
                    //lblAccounts[i].AutoSize = true;
                    lblAccounts[i].Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);
                    userControl.ClientArea.Controls.Add(lblAccounts[i]);
                    lblAccounts[i].Location = new System.Drawing.Point(startLabel_X, start_Y + i * yIncrement);
                    //lblAccounts[i].Size = new System.Drawing.Size(lblLength, lblheight);
                    lblAccounts[i].Name = account.AccountID.ToString();
                    lblAccounts[i].Text = account.Name;
                    lblAccounts[i].AutoSize = true;
                    if (lblLength < lblAccounts[i].Width)
                        lblLength = lblAccounts[i].Width;
                }
                UltraLabel labelTotal = new UltraLabel();
                labelTotal.Location = new System.Drawing.Point(startLabel_X, start_Y + i * yIncrement);
                //labelTotal.Size = new System.Drawing.Size(lblLength, lblheight);
                labelTotal.AutoSize = true;
                labelTotal.Text = "Total";
                //if (lblLength < labelTotal.Width)
                //    lblLength = labelTotal.Width;
                userControl.ClientArea.Controls.Add(labelTotal);

                i++;
                UltraLabel labelRemaining = new UltraLabel();
                labelRemaining.Location = new System.Drawing.Point(startLabel_X, start_Y + i * yIncrement);
                //labelTotal.Size = new System.Drawing.Size(lblLength, lblheight);
                labelRemaining.AutoSize = true;
                labelRemaining.Text = "Remaining";
                userControl.ClientArea.Controls.Add(labelRemaining);

            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return lblLength + startLabel_X;
        }
    }
}
