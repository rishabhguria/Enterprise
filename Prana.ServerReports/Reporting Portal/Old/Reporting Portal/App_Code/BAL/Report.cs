using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;

/// <summary>
/// Summary description for Report
/// </summary>
public class Report
{
	public Report()
	{
		
	}

    public Report(string strReportFormat, string strReportServer, string strName, string strFullPath, string strParameters, HandlerType eReportProvider, int intClientID, ReportPeriod eReportPeriod)
	{
        sReportFormat = strReportFormat;
        sReportServer = strReportServer;
        sFullPath = strFullPath;
        sName = strName;
        enumReportProvider = eReportProvider;
        iClientID = intClientID;
        enumReportPeriod = eReportPeriod;
        if (!strParameters.Equals(string.Empty))
        {
            string[] arrParameters = strParameters.Split('&');
            for (int i = 0; i < arrParameters.Length; i++)
            {
                lstParameters.Add(new RepParameter(arrParameters[i]));
            }
        }
	}

    private string sFullPath = string.Empty;
    public string FullPath
    {
        get { return sFullPath; }
        set { sFullPath = value; }
    }

    private string sName=string.Empty;
    public string Name
    {
        get { return sName; }
        set { sName = value; }
    }

    private HandlerType enumReportProvider = HandlerType.NotSet;
    public HandlerType ReportProvider
    {
        get { return enumReportProvider; }
        set { enumReportProvider = value; }
    }
    private ReportPeriod enumReportPeriod = ReportPeriod.Day;
    public ReportPeriod DefaultReportPeriod
    {
        get { return enumReportPeriod; }
        set { enumReportPeriod = value; }
    }
    private int iClientID;

    public int ClientID
    {
        get { return iClientID; }
        set { iClientID = value; }
    }

    private string sReportServer=string.Empty;
    public string ReportServer
    {
        get { return sReportServer; }
        set { sReportServer = value; }
    }

    private string sReportFormat=string.Empty;
    public string ReportFormat
    {
        get { return sReportFormat; }
        set { sReportFormat = value; }
    }

    private List<RepParameter> lstParameters = new List<RepParameter>();
    public List<RepParameter> Parameters
    {
        get { return lstParameters; }
        set { lstParameters = value; }
    }
}


public class RepParameter
{
    public RepParameter(string strName, string strValue)
	{
        sName = strName;
        sValue = strValue;
	}

    public RepParameter(string strKeyValue)
	{
        Parse(strKeyValue);
	}

    private string sName;
    public string Name
    {
        get { return sName; }
        set { sName = value; }
    }

    private string sValue;
    public string Value
    {
        get { return sValue; }
        set { sValue = value; }
    }

    private void Parse(string strKeyValue)
    {
        string[] arrParam = strKeyValue.Split('=');

        sName = arrParam[0];
        sValue = Util.ApplyMacro(arrParam[1].ToString());
    }
}