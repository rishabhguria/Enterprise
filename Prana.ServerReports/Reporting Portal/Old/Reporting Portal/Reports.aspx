<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" EnableViewState="true" CodeFile="Reports.aspx.cs" Inherits="Reports" %>

<asp:Content ID="CnReports" ContentPlaceHolderID="Main" runat="Server">

    <script type="text/javascript">
        $(document).ready(function () {
            $(".datepick").datepicker({ maxDate: new Date() });
        });

        function checkAll() {
            for (var i = 0; i < document.forms[0].elements.length; i++) {
                var e = document.forms[0].elements[i];
                if ((e.name != 'chkAll') && (e.type == 'checkbox')) {
                    e.checked = document.forms[0].chkAll.checked;
                }
            }
        }
    </script>

    <table style="width: 100%">
        <tr>
            <td>
                <div style="float: left; width: 90%; text-align: right">
                    <asp:DropDownList CssClass="ComboBoxs" ID="ddSubClients" runat="server" AutoPostBack="True" Visible="False" OnSelectedIndexChanged="ddSubClients_SelectedIndexChanged" Width="162px" />
                </div>
                <div style="text-align: Right">
                    <asp:LinkButton ID="lnLogOut" runat="server" ForeColor="Black" OnClick="lnLogOut_Click">Log Out</asp:LinkButton>
                </div>
                <div style="clear: both; float: right; text-align: left; width: 75%; vertical-align: text-top; vertical-align: top; background-color: white;">
                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                        <tr>
                            <td class="ReportMenuHeader" colspan="3" align="left">Report Menu
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="ReportMenuFiller" colspan="3"></td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:TextBox ID="txtTheDate" runat="server" BorderStyle="Solid" Width="95px" BorderWidth="1px" AutoPostBack="true" Font-Size="Smaller"></asp:TextBox>
                                <div id="datepicker"></div>
                            </td>
                            <td colspan="2" align="center">
                                <asp:DropDownList CssClass="ComboBoxs" ID="ddReportCategory" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddReportCategory_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="ReportMenuFiller" colspan="3"></td>
                        </tr>
                        <tr>
                            <td align="left" class="SelectAll" colspan="3" style="height: 10px">
                                <% if (isAdminUser)
                                   {
                                       Response.Write("<input id=\"chkAll\" name=\"chkAll\" type=\"checkbox\" onclick=\"checkAll();\"/> Select/Un-Select All");
                                   }
                                %>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div style="clear: both; float: right; text-align: left; width: 75%; vertical-align: text-top; vertical-align: top">
                    <asp:Panel runat="server" ID="panel1" Height="350px" ScrollBars="Auto" BorderStyle="Solid" BorderColor="#183462" BorderWidth="1pt">
                        <asp:PlaceHolder ID="reportsPlaceHolder" runat="server"></asp:PlaceHolder>
                    </asp:Panel>
                    <asp:ImageButton ID="btnGenerateReport" OnClick="GenerateReports1" AlternateText="Approve Reports" runat="server" ImageUrl="~/images/Approve.jpg" />
                    <asp:ImageButton ID="btnUnApprove" runat="server" AlternateText="Un-Approve Reports" ImageUrl="~/images/Unapprove.jpg" OnClick="RemoveReports" />
                    <asp:ImageButton ID="btnSendEmail" runat="server" AlternateText="Send Email" OnClick="SendEmail" ImageUrl="~/images/Sendreports.jpg" />
                </div>
            </td>
        </tr>
    </table>



</asp:Content>
