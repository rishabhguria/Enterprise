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
public class SSRS2008ReportHandler:IReportHandler
{


    public SSRS2008ReportHandler()
	{
		//
		// TODO: Add constructor logic here
		//
	}


    #region IReportHandler Members

    public void GenerateReport(Report r, string sGeneratedReportPath)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public HandlerType HandlerType
    {
        get { throw new Exception("The method or operation is not implemented."); }
    }

    public void RemoveReport(string sGeneratedReportPath)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    #endregion
}
