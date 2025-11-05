using System;
using System.Data;
using System.Configuration;
using System.Net.Mail;
using System.Collections.Generic;
using System.IO;
using System.Web;

/// <summary>
/// Summary description for Util
/// </summary>
public static class Util
{
    static Dictionary<string, Dictionary<string, string>> Settings = BaseSetting.BaseSettings;

    public static string ApplyMacro(string sValue)
    {
        DateTime dtToday = DateTime.Today.Date;
        DateTime dtLastBusinessDay = DataManager.getLastBusinessDay().Date;


        return sValue.Replace("$today", dtToday.ToString(BaseSetting.DateFormat))
                     .Replace("$lastbusinessday", dtLastBusinessDay.ToString(BaseSetting.DateFormat))
                     .Replace("$T2", DataManager.AdjustBusinessDay(dtLastBusinessDay, -1).Date.ToString(BaseSetting.DateFormat));
    }

    public static void SendEmail(string sToAddresses, string sBCCReportRecipients, List<string> lstReports,string sModule)
    {
        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();

        try
        {
            smtpClient.Host = Settings[sModule]["Report_FromEmailIDHost"];
            smtpClient.Port = Convert.ToInt32(Settings[sModule]["Report_FromMailServerPort"]);
            smtpClient.Credentials = new System.Net.NetworkCredential(Settings[sModule]["Report_FromEmailID"], Settings[sModule]["Report_FromEmailIDPWD"]);
            smtpClient.EnableSsl = Convert.ToBoolean(Settings[sModule]["Report_MailUseSSL"]);
            message.From = new MailAddress(Settings[sModule]["Report_FromEmailID"], Settings[sModule]["Report_FromMailDisplayName"]);

            string[] sArrToAddresses = null;

            if (sToAddresses.Contains(","))
            {    
                sArrToAddresses = sToAddresses.Split(',');
            }
            else if (sToAddresses.Contains(";"))
            {
                sArrToAddresses = sToAddresses.Split(';');
            }
            else if (sToAddresses.Trim().Length > 0)
            {
                message.To.Add(sToAddresses);
            }

            if (null != sArrToAddresses)
            {
                for (int i = 0; i < sArrToAddresses.Length; i++)
                {
                    message.To.Add(sArrToAddresses[i].ToString());
                }
            }

            string[] sArrBCCAddresses = null;

            if (sBCCReportRecipients.Contains(","))
            {
                sArrBCCAddresses = sBCCReportRecipients.Split(',');
            }
            else if (sBCCReportRecipients.Contains(";"))
            {
                sArrBCCAddresses = sBCCReportRecipients.Split(';');
            }
            else if (sBCCReportRecipients.Trim().Length > 0)
            {
                message.Bcc.Add(sBCCReportRecipients);
            }

            if (null != sArrBCCAddresses)
            {
                for (int i = 0; i < sArrBCCAddresses.Length; i++)
                {
                    message.Bcc.Add(sArrBCCAddresses[i].ToString());
                }
            }
            
            message.Subject = Settings[sModule]["Report_MailSubject"];

            message.IsBodyHtml = true;

            message.Body = Settings[sModule]["Report_MailBody"];

            foreach (string sReport in lstReports)
            {
                message.Attachments.Add(new Attachment(sReport));
            }

            smtpClient.Send(message);
            message.Dispose();
        }
        catch (Exception ex)
        {
            throw new System.Web.HttpException(404, "Report not yet approved."+ "Inner:" + ex.Message + "  " + ex.InnerException + "  " + ex.StackTrace);
        }
    }

    public static string FileNameWithDate(string FileName, DateTime date, string fileformat)
    {
        return FileName.Replace("/", "_").Replace("%", "_").Replace("&", "And").Replace(" ", "_") + "_" + date.Year.ToString()
                                + date.Month.ToString().PadLeft(2, '0')
                                + date.Day.ToString().PadLeft(2, '0') + "." + fileformat;

    }

    public static string GetPathInReportServer(string sFullPath)
    {
        string[] arrURL = sFullPath.Split('/');
        string t = arrURL[arrURL.Length - 1];
        return t.Substring(t.IndexOf("?") + 1, t.IndexOf("&") - (t.IndexOf("?") + 1)).Replace("%2f", "/").Replace("+", " "); ;
    }

    public static string GetTargetPath(string sClientReportFolder, string sModule)
    {
        return Settings[sModule]["Report_FilePath"] + "\\" + sClientReportFolder + "\\";
    }

    public static bool DeleteFile(string path)
    {
        bool retVal = false;
        try
        {
            File.Delete(path);
            retVal = true;
        }
        catch (Exception)
        {
            throw;
        }
        return retVal;

    }
}

public enum HandlerType
{
    NotSet,
    SSRS2005ReportHandler,
    SSRS2008Reporthandler,
    FileSystemReportHandler
}

public enum ReportPeriod
{
    NotSet,
    Day,
    MTD,
    QTD,
    YTD
}
public enum ReportFormat
{
    pdf,
    csv
}