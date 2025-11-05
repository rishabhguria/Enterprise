using System;
using System.Data;
using System.Configuration;
using rs2005;
using rsExecService;
using System.IO;
using System.Collections.Generic;
/// <summary>
/// Summary description for SSRS2005Handler
/// </summary>
public class SSRS2005ReportHandler: IReportHandler 
{

    private static IReportHandler _reportHandler = null;
	static SSRS2005ReportHandler()
	{
        _reportHandler = new SSRS2005ReportHandler();
	}
    public static IReportHandler GetInstance
    {
        get
        {
            return _reportHandler;
        }
    }

    #region IReportHandler Members
    private HandlerType _handlerType = HandlerType.SSRS2005ReportHandler;
    public HandlerType HandlerType
    {
        get
        {
            return _handlerType;
        }
    }
    private rs2005.ReportingService2005 rs;
    private rsExecService.ReportExecutionService rsExec;
    //[System.Web.Services.WebMethod]
    public void GenerateReport(Report r, string sGeneratedReportPath)
    {
        rs = new rs2005.ReportingService2005();
        rsExec = new rsExecService.ReportExecutionService();

        // Authenticate to the Web service using Windows credentials
        rs.Credentials = System.Net.CredentialCache.DefaultCredentials;
        rsExec.Credentials = System.Net.CredentialCache.DefaultCredentials;

        // Assign the URL of the Web service
        string sReportServer = r.ReportServer;
        string sSSRSReportPath = Util.GetPathInReportServer(r.FullPath);
        List<RepParameter> lstRP = r.Parameters;

        rs.Url = sReportServer + "ReportService2005.asmx";
        rsExec.Url = sReportServer + "ReportExecution2005.asmx";

        string historyID = null;
        string deviceInfo = null;
        string format = string.Empty;
        format = sGeneratedReportPath.Substring(sGeneratedReportPath.LastIndexOf(".") + 1).ToUpper();
        // ReportingService2005 doesn't support "csv" extension for rendering web page to HTML
        // TODO : Update reporting service for unsupported rendering extension
        if (format.Equals("CSV"))
            format = "EXCEL";
        Byte[] results;
        string encoding = String.Empty;
        string mimeType = String.Empty;
        string extension = String.Empty;
        rsExecService.Warning[] warnings = null;
        string[] streamIDs = null;

        try
        {

            // Load the selected report.
            rsExecService.ExecutionInfo ei =
                  rsExec.LoadReport(sSSRSReportPath, historyID);

            // Prepare report parameter.
            // Set the parameters for the report needed.
            rsExecService.ParameterValue[] parameters =
                   new rsExecService.ParameterValue[lstRP.Count];

            // Place to include the parameter.
            if (lstRP.Count >= 1)
            {
                int i = 0;
                foreach (RepParameter rp in lstRP)
                {

                    parameters[i] = new rsExecService.ParameterValue();
                    parameters[i].Name = rp.Name;
                    parameters[i].Value = rp.Value;
                    i++;
                }
            }
            rsExec.Timeout = 600000; //Lets wait for 10 minutes for a report to be generated
            rsExec.SetExecutionParameters(parameters, "en-us");
            results = rsExec.Render(format, deviceInfo,
                      out extension, out encoding,
                      out mimeType, out warnings, out streamIDs);

            // Create a file stream and write the report to it
            //Check for folder
            string fReportFolderPath = sGeneratedReportPath.Substring(0, sGeneratedReportPath.LastIndexOf("\\"));

            if (!Directory.Exists(fReportFolderPath))
            {
                Directory.CreateDirectory(fReportFolderPath);
            }

            using (FileStream stream = File.OpenWrite(sGeneratedReportPath))
            {
                stream.Write(results, 0, results.Length);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error in writing report file.", ex);
        }
    }
 
    public void RemoveReport(string sGeneratedReportPath)
    {
        Util.DeleteFile(sGeneratedReportPath);
    }
    #endregion
}
