using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Net;

/// <summary>
/// Summary description for FileSystemReportHandler
/// </summary>
public class FileSystemReportHandler:IReportHandler
{
	public FileSystemReportHandler()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #region IReportHandler Members

    public void GenerateReport(Report r, string sGeneratedReportPath)
    {
        WebClient fileReader = new WebClient();
        fileReader.DownloadFile(r.ReportServer, sGeneratedReportPath);
    }

    HandlerType _handlerType = HandlerType.NotSet;
    public HandlerType HandlerType
    {
        get { return _handlerType; }
    }

    public void RemoveReport(string sGeneratedReportPath)
    {
        Util.DeleteFile(sGeneratedReportPath);
    }

    #endregion
}
