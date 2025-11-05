using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;

/// <summary>
/// Summary description for ReportsHandler
/// </summary>
public interface IReportHandler
{
    void GenerateReport(Report r, string sGeneratedReportPath);
    void RemoveReport(string sGeneratedReportPath);
    HandlerType HandlerType { get;}
}
