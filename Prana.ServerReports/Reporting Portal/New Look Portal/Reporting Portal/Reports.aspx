<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true"
    EnableViewState="true" CodeFile="Reports.aspx.cs" Inherits="Reports" %>

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
                <div style="background-image: url('images/TSBanner.jpg'); font-family: Calibri; color: #5A595A; size: 18px; text-align: right; height: 110px;">
                    <br />
                    <br />
                    <br />
                    <br />
                    <span style="font-size: 18px; padding-right: 150px;">Reporting Web-Portal</span>
                </div>

                <div style="clear: both; float: right; text-align: left; width: 100%; vertical-align: text-top; background-size: cover; vertical-align: top; background-image: url(images/Webportal_second_BackGround.jpg); margin-top: 2px;">
                    <div style="border: 3px solid grey; width: 95%; height: 30px; background-color: White; margin-left: 26px">
                    </div>
                    <div style="width: 89%; margin-left: 40px; text-align: right; padding-right: 60px; background-color: #10253f; padding-bottom: 2px; padding-top: 7px; position: relative; bottom: 26px; left: 3px;">
                        <asp:DropDownList CssClass="ComboBoxs" ID="ddSubClients" runat="server" AutoPostBack="True"
                            Visible="False" OnSelectedIndexChanged="ddSubClients_SelectedIndexChanged" Width="162px" />
                        <asp:LinkButton ID="lnLogOut" runat="server" ForeColor="#10253f" OnClick="lnLogOut_Click"
                            Style="-webkit-border-radius: 5; -moz-border-radius: 5; border-radius: 5px; font-family: 'Gill Sans MT'; color: #10253f; font-size: 14px; background: #ffffff; padding-bottom: 15px; padding-top: 2px; padding-right: 4px; text-decoration: none; font-weight: normal;"
                            Height="5px"
                            Width="54px">Log Out</asp:LinkButton>
                        <%--<asp:LinkButton ID="LinkButton1" runat="server" ForeColor="Black" OnClick="lnLogOut_Click"
                Style="font-weight: normal; font-size: 14px; color: #10253f; font-family: 'Gill Sans MT';
                background-color: #ffffff;padding-bottom: 1.5px; padding-top: 5px; text-align: justify;" Height="5px" Width="54px">Log Out</asp:LinkButton>--%>
                    </div>
                    <table border="0" cellpadding="3" cellspacing="0" width="100%" style="margin-left: 15%">
                        <tr>
                            <td class="ReportMenuHeader" colspan="3" align="left" style="height: 40px; padding-top: 50px;">
                                <span style="font-weight: normal; font-size: 28px; color: #10253f; font-family: Century Gothic;">Report Menu
                        <br />
                                </span>
                            </td>
                        </tr>
                        <%--<tr>
                <td align="center" class="ReportMenuFiller" colspan="3" style="height: 10px">
                </td>
            </tr>--%>
                        <tr>
                            <td align="left">
                                <asp:TextBox ID="txtTheDate" runat="server" Width="116px" CssClass="txtdate" AutoPostBack="true"
                                    Font-Size="Smaller"></asp:TextBox>
                                <div id="datepicker">
                                </div>
                            </td>
                            <td colspan="2" align="center" style="width: 392px">
                                <asp:DropDownList CssClass="ComboBoxs" ID="ddReportCategory" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddReportCategory_SelectedIndexChanged" Style="background-image: url(images/Webportal_second_BackGround.jpg);">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="ReportMenuFiller" colspan="3" style="height: 10px"></td>
                        </tr>
                        <tr>
                            <td align="left" class="SelectAll" colspan="3" style="height: 10px; font-family: Century Gothic; font-size: 14px; color: #102544; font-weight: normal; width: 450px;">
                                <% if (isAdminUser)
                                   {
                                       Response.Write("<input id=\"chkAll\" name=\"chkAll\" type=\"checkbox\" onclick=\"checkAll();\"/> Select / Un-Select All");
                                   }
                                %>
                            </td>
                        </tr>
                    </table>

                    <div style="clear: both; background-image: url(images\Webportal_second_BackGround.jpg); float: right; text-align: left; width: 100%; vertical-align: text-top; vertical-align: top">
                        <div style="margin-left: 15%">
                            <asp:Panel runat="server" ID="panel1" Height="350px" ScrollBars="Auto">
                                <asp:PlaceHolder ID="reportsPlaceHolder" runat="server"></asp:PlaceHolder>
                            </asp:Panel>
                            <asp:ImageButton ID="btnGenerateReport" OnClick="GenerateReports1" AlternateText="Approve Reports"
                                runat="server" ImageUrl="~/images/Approve.jpg" Style="padding-left: 10%" Width="92px" />
                            &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:ImageButton ID="btnSendEmail" runat="server" AlternateText="Send Email" OnClick="SendEmail"
                                ImageUrl="~/images/Sendreports.jpg" Style="padding-left: 5%" Width="92px" />
                            &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:ImageButton ID="btnUnApprove" runat="server" AlternateText="Un-Approve Reports"
                                ImageUrl="~/images/Unapprove.jpg" OnClick="RemoveReports" Style="padding-left: 5%" Width="92px" />
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
