using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
    using System.Web    ;
using Nirvana.Global;
using System.Web.Mail;

namespace Nirvana.Utilities
{
	/// <summary>
	/// Summary description for MailManager.
	/// </summary>
	public class MailManager
	{
		private MailManager()
		{
		}
		
		/// <summary>
		/// Send Exception to the Nirvana Developement members.
		/// </summary>
		public static void SendExceptionMail()
		{
			try
			{
				MailSetting mailSetting = GetMailSettings();
				if(mailSetting != null)
				{
					MailMessage aMessage = new MailMessage();
			 
					aMessage.From = mailSetting.From;
					aMessage.To = mailSetting.To;				
					aMessage.Subject = mailSetting.Subject;
					aMessage.Body = mailSetting.Body;
					aMessage.BodyFormat = MailFormat.Text;
 
					// Attach file.					
					aMessage.Attachments.Add(new MailAttachment(Environment.CurrentDirectory + @"\Log\Exception" + DateTime.Now.Month + DateTime.Now.Day + ".log", MailEncoding.Base64));
					aMessage.Attachments.Add(new MailAttachment(Environment.CurrentDirectory + @"\Log\Trace" + DateTime.Now.Month + DateTime.Now.Day + ".log", MailEncoding.Base64));
					
					SmtpMail.SmtpServer = mailSetting.SMTPServer;//"SMTP.algorismtech.com"
					SmtpMail.Send(aMessage);
				}
			}
			#region Catch
			catch(Exception ex)
			{
				Console.WriteLine(ex.InnerException.ToString());				
			}				
			#endregion
		}

		/// <summary>
		/// Gets mail setting from Database.
		/// </summary>
		/// <returns>Returns mail setting information.</returns>
		private static MailSetting GetMailSettings()
		{
			MailSetting mailSetting = null;
			Database db = DatabaseFactory.CreateDatabase();

			try
			{	
				using(SqlDataReader reader = (SqlDataReader) db.ExecuteReader(CommandType.StoredProcedure, "P_GetMailSettings"))
				{
					while(reader.Read())
					{		
						mailSetting = new MailSetting();
						mailSetting.From = reader.GetString(1);
						mailSetting.To = reader.GetString(2);
						mailSetting.Subject = reader.GetString(5);
						mailSetting.Body = reader.GetString(6);
						mailSetting.SMTPServer = reader.GetString(7);			
					}
				}
			}
			#region Catch
			catch(Exception ex)
			{
				// Invoke our policy that is responsible for making sure no secure information
				// gets out of our layer.
				bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDTHROW);

				if (rethrow)
				{
					throw;			
				}
			}				
			#endregion
			return mailSetting;
		}
	}

	#region Mailseting Class

	public class MailSetting
	{
		#region Private Variables
		int _mailSettingID = int.MinValue;
		string _from = string.Empty;
		string _to = string.Empty;
		string _carbonCopy = string.Empty;
		string _blankCarbonCopy = string.Empty;
		string _subject = string.Empty;
		string _body = string.Empty;
		string _smtpServer = string.Empty;
		#endregion
		#region Constructors
		public MailSetting()
		{
		}

		public MailSetting(string from, string to, string subject, string body, string smtpServer)
		{
			_from = from;
			_to = to;
			_subject = subject;
			_body = body;
			_smtpServer = smtpServer;
		}

		public MailSetting(string from, string to, string carbonCopy, string subject, string body, string smtpServer)
		{
			_from = from;
			_to = to;
			_carbonCopy = carbonCopy;
			_subject = subject;
			_body = body;
			_smtpServer = smtpServer;
		}

		public MailSetting(string from, string to, string blankCarbonCopy, string carbonCopy, string subject, string body, string smtpServer)
		{
			_from = from;
			_to = to;
			_carbonCopy = carbonCopy;
			_blankCarbonCopy = blankCarbonCopy;
			_subject = subject;
			_body = body;
			_smtpServer = smtpServer;
		}
		#endregion
		#region Properties

		public string From
		{
			get{return _from;}
			set{_from = value;}
		}

		public string To
		{
			get{return _to;}
			set{_to = value;}
		}

		public string Subject
		{
			get{return _subject;}
			set{_subject = value;}
		}

		public string Body
		{
			get{return _body;}
			set{_body = value;}
		}

		public string SMTPServer
		{
			get{return _smtpServer;}
			set{_smtpServer = value;}
		}
		#endregion
	}

	#endregion
}
