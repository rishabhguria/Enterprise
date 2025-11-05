#region Using

using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

#endregion

namespace Nirvana.Admin
{
	/// <summary>
	/// Summary description for GlobalPolicyExceptionHandler.
	/// </summary>
	public class AppMessageExceptionHandler : ExceptionHandler
	{
		public AppMessageExceptionHandler()
		{
		}

		public override void Initialize(ConfigurationView configurationView)
		{
		}

		public override Exception HandleException(Exception exception, string policyName, Guid correlationID) 
		{
			DialogResult result = this.ShowThreadExceptionDialog(exception);
			
			//TODO: Mail the exception.
			Nirvana.Client.BLL.MailManager.SendExceptionMail(); 

			// Exits the program when the user clicks Abort.
			if (result == DialogResult.Abort) 
				Application.Exit();

			return exception;
		}

		// Creates the error message and displays it.
		private DialogResult ShowThreadExceptionDialog(Exception e) 
		{
			string errorMsg = e.Message + Environment.NewLine + Environment.NewLine;

			return MessageBox.Show(errorMsg, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
		}
	}
}
