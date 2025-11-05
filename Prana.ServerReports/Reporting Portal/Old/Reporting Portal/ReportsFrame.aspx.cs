using System;
using System.Web;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;
using System.Data;

public partial class ReportsFrame : System.Web.UI.Page
{
    private List<ReportParameter> lstParameters = new List<ReportParameter>();
    Dictionary<string, Report> chkReportsDic = new Dictionary<string, Report>();
    internal static string sModule = "Reports";
    internal Dictionary<string, Dictionary<string, string>> Settings = BaseSetting.BaseSettings;
    protected bool isAdminUser = false;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["User"] == null)
            Server.Transfer("Index.aspx?msg=Your Session Expired !");
        if (Session["TermAccepted"] != null && Session["TermAccepted"].ToString() == "0")
            Server.Transfer("terms.aspx");
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!Page.IsPostBack)
            {
                DataTable dtUser = (DataTable)Session["User"];
                isAdminUser = (1 == Convert.ToInt32(dtUser.Rows[0]["Role"]));

                Report rReport = null;

                if (isAdminUser)
                {
                   rReport = ((Dictionary<string, Report>)Session["chkReportsDic"])["chk" + HttpUtility.UrlDecode(Request.QueryString["report"]).Replace(" ", "")];
                }
                else
                {
                   rReport = ((Dictionary<string, Report>)Session["chkReportsDic"])[HttpUtility.UrlDecode(Request.QueryString["report"]).Replace(" ", "")];
                }
                List<RepParameter> lstReportParams = rReport.Parameters;
                string sPathParams = string.Empty;

                foreach (RepParameter rp in lstReportParams)
                {
                    if (!rp.Name.Equals("ghostparam"))
                    {
                        ReportParameter rep = lstParameters.Find(delegate(ReportParameter r) { return r.Name.Equals(rp.Name); });
                        if (null != rep)
                        {
                            rep.Values.Add(rp.Value);
                        }
                        else
                        {
                            lstParameters.Add(new ReportParameter(rp.Name, rp.Value));
                        }
                    }

                }
                reportVwr.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                reportVwr.EnableViewState = true;
                reportVwr.AsyncRendering = false;
                reportVwr.SizeToReportContent = true;
                reportVwr.ServerReport.ReportServerUrl = new Uri(rReport.ReportServer); //actionURL + "&" + inputTag;
                reportVwr.ServerReport.ReportPath = Util.GetPathInReportServer(rReport.FullPath);
                reportVwr.ServerReport.SetParameters(lstParameters);
            }

            ReportParameterInfoCollection rpic = reportVwr.ServerReport.GetParameters();

            foreach (ReportParameterInfo rpi in rpic)
            {
                string tempEvalVal = string.Empty;
                if (!rpi.Name.Equals("ghostparam"))
                {
                    if (rpi.Name.Equals("paramMasterFundID") && rpi.Values.Count != 1)
                    {
                        throw new System.Exception("Report not configured properly. Please contact Nirvana.");
                    }
                    //Response.Write("Name: " + rpi.Name + " Count: " + rpi.Values.Count + " Value:" + rpi.Values[0] + "<br>");
                }
            }
        }
        catch (Exception ex)
        {
            throw new System.Exception("Report not loaded correctly. Please contact system admin.",ex);
        }
    }
}
    