<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true"
    CodeFile="Index.aspx.cs" Inherits="Index" %>

<%@ MasterType VirtualPath="MasterPage.master" %>
<asp:Content ID="CnIndex" ContentPlaceHolderID="Main" runat="Server">

    <script language="javascript" type="text/javascript">
        function validate() {
            if (document.getElementById("<%=txtUserName.ClientID%>").value == "") {
                alert("UserName Field can not be blank");
                document.getElementById("<%=txtUserName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtPassword.ClientID %>").value == "") {
                alert("Password Field can not be blank");
                document.getElementById("<%=txtPassword.ClientID %>").focus();
                return false;
            }
            return true;
        }
        function IMG1_onclick() {

        }

    </script>
    <div id="divBanner" class="tableBannerbg">
    </div>
    <%-- <div id="divTopBorder" class="tableTopBorder">
        </div>--%>
    <%--<div class="LoginWelcomeMessage">
            <%=Settings[sModule]["Report_LoginMessage"]%>
        </div>--%>
    <div class="login" style="background-image: url(images\\PortalBackground.jpg); height: 450px; background-size: cover">
        <table class="ReportMenuItem" border="0" cellspacing="5" style="padding-top: 100px;">
            <tr>
                <td colspan="2" align="left">
                    <asp:Label ID="lblResult" runat="server" ForeColor="Red" CssClass="Label"></asp:Label></td>
            </tr>
            <tr>
                <td colspan="2" style="color: #102544; font-family: 'Gill Sans MT'; size: 14px; text-align: center">
                    <span style="font-size: 14pt">Client Login </span>
                </td>
            </tr>
            <tr>
                <td align="right" style="font-size: 12pt; color: #10253f; font-style: italic; font-family: 'Gill Sans MT'; height: 32px; width: 167px;">Username:</td>
                <td style="height: 32px" align="left">
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="inputtext" Style="border-left-color: #102544; border-bottom-color: #102544; border-top-style: solid; border-top-color: #102544; border-right-style: solid; border-left-style: solid; border-right-color: #102544; border-bottom-style: solid"></asp:TextBox>
                    <%--<asp:TextBox ID="txtUserName" runat="server" CssClass="inputtext"
                            BorderColor="Transparent" Style="border-left-color: #102544; border-bottom-color: #102544;
                            border-right-style: solid; border-left-style: solid; border-right-color: #102544;
                            border-bottom-style: solid; border-top: solid;"></asp:TextBox>--%>
                </td>
            </tr>
            <tr>
                <td align="right" style="font-size: 12pt; color: #102544; font-style: italic; font-family: 'Gill Sans MT'; width: 167px; height: 32px;">Password:</td>
                <td align="left" style="height: 32px">
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="inputtext" TextMode="Password"
                        Style="border-left-color: #102544; border-bottom-color: #102544; border-top-style: solid; border-top-color: #102544; border-right-style: solid; border-left-style: solid; border-right-color: #102544; border-bottom-style: solid"></asp:TextBox>
                </td>
            </tr>
            <tr style="height: 2px;">
                <td colspan="2" style="height: 1px; text-align: center; padding-left: 70px">
                    <hr style="height: 1px; color: #10253f; width: 268px; background-color: #102544;" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center">&nbsp;<asp:ImageButton ID="btn_SecureLogin" runat="server" OnClick="btn_SecureLogin_Click"
                    OnClientClick="return validate()" AlternateText="Secure Login" ImageUrl="~/images/Login.jpg" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center">
                    <span style="font-family: Gill Sans MT"><em><span style="color: #10253f">Forgot Password
                : <span style="text-decoration: underline"><strong></strong></span></span></em></span>
                    <a href="mailto:<%=Settings[sModule]["Report_SupportEmailID"]%>?Subject=<%=Settings[sModule]["Report_PageTitle"]%> password reset"
                        class="ReportMenu"><u><span style="font-family: Gill Sans MT; color: #10253f;"><em><strong>Click Here</strong></em></span></u></a><br />
                </td>

            </tr>
            <tr>
                <td colspan="2" style="text-align: center">
                    <span style="font-family: Gill Sans MT"><em><span style="color: #10253f">
                        <br />
                        <br />
                        <br />
                        <br />
                        Powered by
                :<span style="text-decoration: underline"><strong></strong></span></span></em></span>

                    <span style="font-family: Gill Sans MT"><em><span style="color: #10253f"><span style="text-decoration: underline">Nirvana</span>
                        <span style="text-decoration: underline"></span></span></em></span>

                </td>
            </tr>
        </table>
        <%--  
            <br />
            <span style="font-family: Gill Sans MT"><span style="color: #102544"><em>
                <span style="text-decoration: underline"></span></em></span></span></div>--%>
        <%--  <div class="forgotPWDText">
            <p style="background-position: left center; background-image: url(images/HFM2013.jpg)">
                <a href="mailto:<%=Settings[sModule]["Report_SupportEmailID"]%>?Subject=<%=Settings[sModule]["Report_PageTitle"]%> password reset"
                    class="ReportMenu"></a>&nbsp;</p>
        </div>--%>
        &nbsp;
    </div>
    <hr id="HR1" style="padding-top: 1px; background-color: #102544; margin-top: 1px" />
    <div>
        <img src="images/HFM2013.jpg" alt="HFM Award 2013" style="position: static; width: 71px; height: 72px;" />
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp;
            <img src="images/HFM2014.jpg" alt="HFM Award 2014" style="position: static; height: 87px;" id="IMG1"
                onclick="return IMG1_onclick()" />
    </div>
    <div>
        &nbsp;
    </div>
</asp:Content>
