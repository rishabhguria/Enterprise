<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>

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
    </script>
    <div align="center">       
        <div class="LoginWelcomeMessage"><%=Settings[sModule]["Report_LoginMessage"]%></div>
        <div class="login">
            <table class="ReportMenuItem" border="0">
                <tr>
                    <td></td>
                    <td align="left">
                        <asp:Label ID="lblResult" runat="server" ForeColor="Red" CssClass="Label"></asp:Label></td>
                </tr>
                <tr>
                    <td align="left">User Name:</td>
                    <td>
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="inputtext"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left">Password:</td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="inputtext" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left"></td>
                    <td align="left">
                        <asp:ImageButton ID="btn_SecureLogin" runat="server" OnClick="btn_SecureLogin_Click"
                            OnClientClick="return validate()" AlternateText="Secure Login" ImageUrl="~/images/Login.jpg" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center"></td>
                </tr>
            </table>
        </div>
        <div class="forgotPWDText">
            <p>If you&#8217;re an existing user and forgotten your password, please <a href="mailto:<%=Settings[sModule]["Report_SupportEmailID"]%>?Subject=<%=Settings[sModule]["Report_PageTitle"]%> password reset" class="ReportMenu"><u>click here</u></a></p>
        </div>
        <div style="margin-top:150px">
        </div>
    </div>
</asp:Content>

