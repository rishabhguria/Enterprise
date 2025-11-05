using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Reflection;
using System.IO;


public partial class Reports : System.Web.UI.Page
{
    List<Report> rptListGenerate = null;
    DateTime theDate = DateTime.Today.Date;
    string sReportRecipients = string.Empty;
    string sBCCReportRecipients = string.Empty;
    string sClientReportFolder = string.Empty;
    internal Dictionary<string, Dictionary<string, string>> Settings = BaseSetting.BaseSettings;
    Dictionary<HandlerType, IReportHandler> _reporthandlers = null;
    internal static string sModule = "Reports";
    protected bool isAdminUser = false;
    protected int iPranaUserID = 0;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["User"] == null)
            Server.Transfer("Index.aspx?msg=Your Session Expired !");
        if (Session["TermAccepted"] != null && Session["TermAccepted"].ToString() == "0")
            Server.Transfer("terms.aspx");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dtUser = (DataTable)Session["User"];
        string userID = dtUser.Rows[0]["UserID"].ToString();
        chkReportsDic = (Dictionary<string, Report>)Session["chkReportsDic"];
        isAdminUser = (1 == Convert.ToInt32(dtUser.Rows[0]["Role"]));
        iPranaUserID = Convert.ToInt32(dtUser.Rows[0]["PranaUserID"]);

        if (!IsPostBack)
        {
            fillClientDropDown(userID);
            //To Attach MasterFundID With ReportPath If Exist
            //string clientID = ddSubClients.Text;
            Session["ClientID"] = ddSubClients.Text;
            fillReportsDic(ddSubClients.Text);
            fillReportCategoryDropDown();
            if (ddReportCategory.Items.Count > 0)
                ShowReports(ddReportCategory.Text);

            txtTheDate.Attributes.Add("class", "datepick");
            txtTheDate.Attributes.Add("style", "text-align:center;font-family:Verdana, Arial, Helvetica, sans-serif;");
            txtTheDate.Text = DataManager.getLastBusinessDay().Date.ToString(BaseSetting.DateFormat);

            btnGenerateReport.Attributes.Add("onclick", "$.blockUI(); this.src= 'images/Processing.jpg';" + ClientScript.GetPostBackEventReference(btnGenerateReport, "").ToString() + "; return false;");
            btnUnApprove.Attributes.Add("onclick", "$.blockUI(); this.src= 'images/Processing.jpg';" + ClientScript.GetPostBackEventReference(btnUnApprove, "").ToString() + "; return false;");
            btnSendEmail.Attributes.Add("onclick", "$.blockUI(); this.src= 'images/Processing.jpg';" + ClientScript.GetPostBackEventReference(btnSendEmail, "").ToString() + "; return false;");
        }

        if (IsPostBack)
        {
            rptListGenerate = new List<Report>();

            for (int i = 0; i < Request.Form.AllKeys.Length; i++)
            {
                if (Request.Form.GetValues("__EVENTTARGET")[0].Contains("ddSubClients"))
                    break;


                string k = (String)Request.Form.AllKeys.GetValue(i);
                if (!k.Substring(0, 2).Equals("__"))
                {
                    string v = (String)Request.Form.Get(i);
                    if (v.Equals("on"))
                    {
                        string strCheckBoxID = k.Substring(k.LastIndexOf("$") + 1);
                        if (chkReportsDic.ContainsKey(strCheckBoxID))
                        {
                            rptListGenerate.Add(chkReportsDic[strCheckBoxID]);
                        }
                    }
                    //Response.Write("Key: " + k + " Value: " + v + "<Br>");
                }
            }
        }
        
        if (!DateTime.TryParse(txtTheDate.Text, out theDate))
        {
            theDate = DataManager.getLastBusinessDay().Date;
        }

        //if (theDate.Date.CompareTo(DateTime.Today.Date) == 0)
        //{
        //    theDate = DataManager.getLastBusinessDay().Date;
        //}

        Session["theDate"] = theDate;

        DataManager DAL = new DataManager();
        DataTable dt = DAL.getClientDetailsByID(ddSubClients.SelectedValue);
        sReportRecipients = dt.Rows[0]["EmailID"].ToString();
        sBCCReportRecipients = dt.Rows[0]["BCCEmailID"].ToString();
        sClientReportFolder = dt.Rows[0]["ReportFolder"].ToString();
        Session["ClientID"] = ddSubClients.Text;
        fillReportsDic(ddSubClients.Text);
        ShowReports(ddReportCategory.Text);

        //fill dictionary of available reporthandlers
        _reporthandlers = new Dictionary<HandlerType, IReportHandler>();
        Array sArr = Enum.GetValues(new HandlerType().GetType());

        IReportHandler rh = null;
        for (int i = 1; i < sArr.Length; i++)
        {
            rh = null;
            switch (sArr.GetValue(i).ToString())
            {
                case "SSRS2005ReportHandler":
                    rh = new SSRS2005ReportHandler();
                    break;
                case "SSRS2008Reporthandler":
                    rh = new SSRS2008ReportHandler();
                    break;
                case "FileSystemReportHandler":
                    rh = new FileSystemReportHandler();
                    break;
            }

            _reporthandlers.Add((HandlerType)Enum.Parse(new HandlerType().GetType(), sArr.GetValue(i).ToString()), rh);
        }

        btnGenerateReport.Visible = isAdminUser;
        btnSendEmail.Visible = isAdminUser;
        btnUnApprove.Visible = isAdminUser;
        lnLogOut.ForeColor = System.Drawing.Color.FromName(Settings[sModule]["Report_LogOutFontForeColor"]);
    }
    protected void lnLogOut_Click(object sender, EventArgs e)
    {
        Session.Clear();
        FormsAuthentication.SignOut();
        Server.Transfer("Index.aspx?msg=Log Out Successfully !");
    }

    #region SubClient DropDownList Section

    protected void ddSubClients_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DicRType_ReportWithPath has to be change in session
        Session["ClientID"] = ddSubClients.Text;
        fillReportsDic(ddSubClients.Text);
        fillReportCategoryDropDown();
        ShowReports(ddReportCategory.Text);
    }
    private void fillClientDropDown(string userID)
    {
        DataTable dtSubClients;
        DataManager DAL = new DataManager();
        dtSubClients = DAL.getSubClients(userID);
        if (dtSubClients.Columns.Count > 1)
        {
            if (dtSubClients.Rows.Count > 1)
                ddSubClients.Visible = true;
            foreach (DataRow dr in dtSubClients.Rows)
                ddSubClients.Items.Add(new ListItem(dr["ClientName"].ToString(), dr["ClientID"].ToString()));
        }
    }

    #endregion

    #region Report Section
    private Dictionary<string, Table> reportTypeTables;

    public Dictionary<string, Table> ReportTypeTables
    {
        get { return reportTypeTables; }
        set { reportTypeTables = value; }
    }

    protected void ShowReports(string ReportType)
    {
        string LabelText = string.Empty;
        reportsPlaceHolder.Controls.Clear();
        if (reportTypeTables != null && reportTypeTables.ContainsKey(ReportType))
        {
            foreach (string reportType in reportTypeTables.Keys)
            {
                if (reportType.Equals(ReportType))
                {
                    reportsPlaceHolder.Controls.Add(reportTypeTables[reportType]);
                    reportTypeTables[reportType].Visible = true;
                }
                else
                {
                    reportTypeTables[reportType].Visible = false;
                }
            }
        }
    }

    Dictionary<string, Report> chkReportsDic = new Dictionary<string, Report>();

    private void fillReportsDic(string ClientID)
    {
        DataManager DAL = new DataManager();
        DataTable dt = DAL.getReports(ClientID);
        string ClientName = ddSubClients.SelectedItem.Text;

        chkReportsDic = new Dictionary<string, Report>();

        ReportTypeTables = new Dictionary<string, Table>();
        Table t = new Table();
        string reportName = string.Empty;

        String sPreviousUIGroupName = string.Empty; 
        if (dt.Rows.Count > 0)
        {
            sPreviousUIGroupName = dt.Rows[0]["GroupName"].ToString();
        }
        String sCurrentGroupName = string.Empty;
        foreach (DataRow dr in dt.Rows)
        {
            HandlerType enumReportProvider = (HandlerType)Convert.ToInt32(dr["ReportProvider"]);
            ReportPeriod enumReportPeriod = (ReportPeriod)Convert.ToInt32(dr["DefaultPeriod"]);

            TableRow rGroup = new TableRow();
            TableCell cGroupName = new TableCell();
            Label lGroupName = new Label();
           
            if (!ReportTypeTables.ContainsKey(dr["ReportTypeName"].ToString()))
            {
                t = new Table();
                t.ID = "MainReportContainer";
                t.Visible = false;
                ReportTypeTables.Add(dr["ReportTypeName"].ToString(), t);
            }

            if (!sCurrentGroupName.Equals(string.Empty))
            {
                sCurrentGroupName = dr["GroupName"].ToString();
            }

            if (!sPreviousUIGroupName.Equals(sCurrentGroupName))
            {
                rGroup = new TableRow();
                cGroupName = new TableCell();
                lGroupName = new Label();
                lGroupName.Text = dr["GroupName"].ToString();
                cGroupName.Controls.Add(lGroupName);
                cGroupName.ColumnSpan = 3;
                cGroupName.CssClass = "UIGroupName";
                rGroup.Cells.Add(cGroupName);
                t.Rows.Add(rGroup);
                if (!sCurrentGroupName.Equals(string.Empty))
                {
                    sPreviousUIGroupName = sCurrentGroupName;
                }
            }
            sCurrentGroupName = dr["GroupName"].ToString();

            TableRow r = new TableRow();
            TableCell cCheckBox = new TableCell();
            TableCell cSpaces = new TableCell();
            TableCell cReport = new TableCell();
            TableCell cPdfIcon = new TableCell();
            HyperLink ReportAndLink = new HyperLink();

            string sReportParams = dr["Parameters"].ToString();
            reportName = dr["ReportName"].ToString();
            ReportAndLink.Text = reportName;
            ReportAndLink.CssClass = "ReportMenu";

            switch (enumReportProvider)
            {
                case HandlerType.SSRS2005ReportHandler:
                case HandlerType.SSRS2008Reporthandler:
            ReportAndLink.NavigateUrl = string.Format("ReportsFrame.aspx?Report={0}", HttpUtility.UrlEncode(reportName));
                    break;
                case HandlerType.FileSystemReportHandler:
                    ReportAndLink.NavigateUrl = dr["ReportPath"].ToString();
                    break;
            }
            //if (0 != iPranaUserID)
            //{
                sReportParams = sReportParams.Replace("$PranaUserID", iPranaUserID.ToString());
            //}

            string sEndDateParam = dr["EndDateParam"].ToString();
            string sStartDateParam = dr["StartDateParam"].ToString();
            string sDateParams = string.Empty;
            switch (enumReportPeriod)
            {
                case ReportPeriod.Day:
                    if (string.Empty != sStartDateParam)
                    {
                        sDateParams = sStartDateParam + "=" + theDate.ToString(BaseSetting.DateFormat) + "&";
                    }
                    
                    if (string.Empty != sEndDateParam)
                    {
                        sDateParams = sDateParams + sEndDateParam + "=" + theDate.ToString(BaseSetting.DateFormat);
                    }
                    break;
                case ReportPeriod.MTD:
                    if (string.Empty != sStartDateParam)
                    {
                        sDateParams = sStartDateParam + "=" + DataManager.getMonthStartForDate(theDate).ToString(BaseSetting.DateFormat) + "&";
                    }
                    
                    if (string.Empty != sEndDateParam)
                    {
                        sDateParams = sDateParams + sEndDateParam + "=" + theDate.ToString(BaseSetting.DateFormat);
                    }


                    break;
                case ReportPeriod.QTD:
                    if (string.Empty != sStartDateParam)
                    {
                        sDateParams = sStartDateParam + "=" + DataManager.getStartDate(Convert.ToDateTime(((theDate.Month - 1) / 3)*3 + 1 + "/1/" + theDate.Year)).ToString(BaseSetting.DateFormat) + "&";
                    }

                    if (string.Empty != sEndDateParam)
                    {
                        sDateParams = sDateParams + sEndDateParam + "=" + theDate.ToString(BaseSetting.DateFormat);
                    }
                    break;
               case ReportPeriod.YTD:
                    if (string.Empty != sStartDateParam)
                    {
                        sDateParams = sStartDateParam + "=" + DataManager.getStartDate(Convert.ToDateTime("1/1/" + theDate.Year)).ToString(BaseSetting.DateFormat) + "&";
                    }
                    
                    if (string.Empty != sEndDateParam)
                    {
                        sDateParams = sDateParams + sEndDateParam + "=" + theDate.ToString(BaseSetting.DateFormat);
                    }
                    break;
                case ReportPeriod.NotSet:
                    break;
            }

            ReportAndLink.Target = dr["NewWindowName"].ToString();

            if (isAdminUser)
            {
                CheckBox chkReport = new CheckBox();
                chkReport.Text = "";
                chkReport.ID = "chk" + ReportAndLink.Text.Replace(" ", "");
                cCheckBox.Controls.Add(chkReport);
                r.Cells.Add(cCheckBox);

                if (enumReportProvider != HandlerType.FileSystemReportHandler)
                {
                    if (enumReportPeriod != ReportPeriod.NotSet)
                    {
                        sReportParams = sDateParams + "&" + sReportParams;
                    }
                }
                chkReportsDic.Add(chkReport.ID, new Report(dr["ReportFormat"].ToString(), dr["ReportServer"].ToString(), ReportAndLink.Text, dr["ReportPath"].ToString(), sReportParams, enumReportProvider, Int32.Parse(dr["ClientID"].ToString()), enumReportPeriod));
            }
            else
            {
                chkReportsDic.Add(ReportAndLink.Text.Replace(" ", ""), new Report(dr["ReportFormat"].ToString(), dr["ReportServer"].ToString(), ReportAndLink.Text, dr["ReportPath"].ToString(), sReportParams, enumReportProvider, Int32.Parse(dr["ClientID"].ToString()), enumReportPeriod));
            }

            Label lblSpaces = new Label();
            lblSpaces.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            cSpaces.Controls.Add(lblSpaces);
            r.Cells.Add(cSpaces);
            cReport.Controls.Add(ReportAndLink);
            cReport.Attributes.Add("class", "ReportMenuItem");
            r.Cells.Add(cReport);

            HyperLink imgPdfIcon = new HyperLink();
            //imgPdfIcon.ImageUrl.AlternateText = "Pre-generated report no available";
            string strDisabledIcon = "images/pdficon_d.jpg";
            string strEnabledIcon = "images/pdficon_e.jpg";

            if (!dr["ReportFormat"].ToString().Equals("pdf"))
            {
                strDisabledIcon = "images/AdminFileIcon_d.jpg";
                strEnabledIcon = "images/AdminFileIcon_e.jpg";
            }

            imgPdfIcon.ImageUrl = strDisabledIcon;
            if (File.Exists(Util.GetTargetPath(sClientReportFolder, sModule) + Util.FileNameWithDate(ReportAndLink.Text, theDate, dr["ReportFormat"].ToString())))
            {
                //imgPdfIcon.AlternateText = "View pre generated report";
                imgPdfIcon.ImageUrl = strEnabledIcon;
                imgPdfIcon.NavigateUrl = sClientReportFolder + "/" + Util.FileNameWithDate(ReportAndLink.Text, theDate, dr["ReportFormat"].ToString());
                imgPdfIcon.Target = "hist";
            }

            if (isAdminUser)
            {
                if (null != rptListGenerate)
                {
                    foreach (Report rl in rptListGenerate)
                    {
                        if (rl.Name.Equals(ReportAndLink.Text))
                        {
                            if (Request.Form.GetValues("__EVENTTARGET")[0].Contains("btnGenerateReport"))
                            {
                                imgPdfIcon.ImageUrl = strEnabledIcon;
                                imgPdfIcon.NavigateUrl = sClientReportFolder + "/" + Util.FileNameWithDate(ReportAndLink.Text, theDate, dr["ReportFormat"].ToString());
                                imgPdfIcon.Target = "hist";
                            }
                            if (Request.Form.GetValues("__EVENTTARGET")[0].Contains("btnUnApprove"))
                            {
                                imgPdfIcon.ImageUrl = strDisabledIcon;
                                imgPdfIcon.NavigateUrl = string.Empty;
                                imgPdfIcon.Target = string.Empty;
                            }
                        }
                    }
                }
            }

            cPdfIcon.Controls.Add(imgPdfIcon);

            r.Cells.Add(cPdfIcon);

            t.Rows.Add(r);
        }

        Session["dicRType_ReportWithPath"] = ReportTypeTables;
        Session["chkReportsDic"] = chkReportsDic;
    }

    private void fillReportCategoryDropDown()
    {
        if (reportTypeTables == null)
            reportTypeTables = (Dictionary<string, Table>)Session["dicRType_ReportWithPath"];
        ddReportCategory.Items.Clear();
        if (reportTypeTables != null)
        {
            foreach (string reportType in reportTypeTables.Keys)
                ddReportCategory.Items.Add(reportType);
        }
        if (ddReportCategory.Items.Count > 1)
            ddReportCategory.Visible = true;
        else
            ddReportCategory.Visible = false;
    }
    protected void ddReportCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (reportTypeTables == null)
            reportTypeTables = (Dictionary<string, Table>)Session["dicRType_ReportWithPath"];
        ShowReports(ddReportCategory.SelectedItem.Text);
    }

    #endregion

    protected void GenerateReports1(object sender, EventArgs e)
    {
        foreach (Report r in rptListGenerate)
        {
            _reporthandlers[r.ReportProvider].GenerateReport(r, Util.GetTargetPath(sClientReportFolder, sModule) + Util.FileNameWithDate(r.Name, theDate, r.ReportFormat));
        }
        return;
    }

    protected void SendEmail(object sender, EventArgs e)
    {
        List<string> lstReports = new List<string>();
        string sPhysicalPath = string.Empty;

        foreach (Report r in rptListGenerate)
        {
            sPhysicalPath = Util.GetTargetPath(sClientReportFolder, sModule) + Util.FileNameWithDate(r.Name, theDate, r.ReportFormat);

            if (File.Exists(sPhysicalPath))
            {
                lstReports.Add(sPhysicalPath);
            }
        }

        if (lstReports.Count > 0)
        {
            Util.SendEmail(sReportRecipients, sBCCReportRecipients, lstReports, sModule);
        }
        return;
    }
    protected void RemoveReports(object sender, ImageClickEventArgs e)
    {
        foreach (Report r in rptListGenerate)
        {
            _reporthandlers[r.ReportProvider].RemoveReport(Util.GetTargetPath(sClientReportFolder, sModule) + Util.FileNameWithDate(r.Name, theDate, r.ReportFormat));
        }
        return;
    }
}



