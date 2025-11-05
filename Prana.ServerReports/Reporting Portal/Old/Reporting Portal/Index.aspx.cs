using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Collections.Generic;

public partial class Index : System.Web.UI.Page
{        
    internal static string sModule = "Reports";
    internal Dictionary<string, Dictionary<string, string>> Settings = BaseSetting.BaseSettings;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        lblResult.Text = "";
        if (Page.Request.QueryString["msg"] != null)
            lblResult.Text = Page.Request.QueryString["msg"].ToString();
        
        
    }
   
    private void Clear()
    {
        txtUserName.Text = "";
        txtPassword.Text = "";
        lblResult.Text = "";
        
    }

    protected void btn_SecureLogin_Click(object sender, EventArgs e)
    {
        System.Media.SystemSounds.Asterisk.Play();
        DataManager DAL = new DataManager();
        DataTable dtUser = DAL.getUsers(txtUserName.Text, txtPassword.Text);
       // Response.Write(dtUser.Rows.Count.ToString());
        if (dtUser.Rows.Count > 0)
        {
            FormsAuthentication.SetAuthCookie(txtUserName.Text, true);

            Session["TermAccepted"] = dtUser.Rows[0]["TermAccepted"].ToString();
            Session["User"] = dtUser;

            Server.Transfer("Reports.aspx");
            //UncommentIf This Require---------
            //if (Session["TermAccepted"].ToString() == "1")                
            //    Server.Transfer("Reports.aspx");
            //else
            //    Server.Transfer("terms.aspx");                
            
        }
        else
        {
            //Response.Write("<script language=\"javascript\">alert('Invalid UserName OR Password !')</script>");
            lblResult.Text = "Invalid UserName OR Password !";
        }     
    }
}
