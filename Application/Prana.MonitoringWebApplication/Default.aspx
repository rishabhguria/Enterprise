<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="Style/StyleSheet.css" rel="Stylesheet" />
</head>


<body>
<div id="container">
<div id="header"><h1>Nirvana Solutions</h1></div>
<div class="divider">&nbsp;</div>
<div id="navigation" >
<p><a href="#">EmailConfiguration</a></p>
<p><a href="#">Admin</a></p>
<p><a href="#">Monitoring Services</a></p>

</div>

<div class="verticledivider"></div>


<div id="wrapper">
<div id="content">
 <form id="form1" runat="server">
    <div>
        
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div id ="AllConnections" style="overflow:auto">
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="Button" />
        <asp:Button ID="btnAddNew" runat="server" OnClick="btnAddNew_Click" Text="AddNew" CssClass="Button"/>
        <asp:UpdatePanel ID="updatePanelConnections" runat="server">
            <ContentTemplate>
        <asp:GridView ID="grdAllConnections" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="None" OnRowCancelingEdit="GridView1_RowCancelingEdit"
            OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating"  OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand">
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <Columns>
                <asp:CommandField HeaderText="Edit-Update" ShowEditButton="True" />
                <asp:BoundField DataField="IpAddress" HeaderText="IpAddress" />
                <asp:BoundField DataField="Name" HeaderText="Name"  />
                <asp:BoundField DataField="Ports" HeaderText="Ports" />         
                <asp:BoundField DataField="ServiceNames" HeaderText="ServiceNames" />
                <asp:ButtonField HeaderText="TradeServer" Text="Connect" CommandName="ConnectToTradeServer" />
                <asp:ButtonField HeaderText="ExpnlServer" Text="Connect" CommandName="ConnectToExpnlServer" />
                <asp:ButtonField HeaderText="PricingServer" Text="Connect" CommandName="ConnectToPricingServer" />
                 <asp:ButtonField HeaderText="ShowDetails" Text="ShowDetails" CommandName="ShowDetails" />
	            <asp:CommandField HeaderText="Delete" ShowDeleteButton="True"/>
            </Columns>
            <RowStyle BackColor="#E3EAEB" />
            <EditRowStyle BackColor="#7C6F57" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#42537A" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
                <asp:Timer ID="Timer1" runat="server" Interval="2000" OnTick="Timer1_Tick">
                </asp:Timer>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
        
        <div id="StatusWrapper"> 
       
        <div class="ServerStatus">
       <p class="Heading">ExpnlServerStatus</p>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdExpnlUsers" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="IdentifierName" HeaderText="Name" >
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ConnStatus" HeaderText="Status" >
                            <ItemStyle Width="40px" />
                        </asp:BoundField>
                    </Columns>
                    
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#7C6F57" Font-Bold="True" ForeColor="White" />
            
                </asp:GridView>
                <br />
                <asp:GridView ID="grdExpnlMiscConn" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="IdentifierName" HeaderText="Name" >
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ConnStatus" HeaderText="Status" >
                            <ItemStyle Width="40px" />
                        </asp:BoundField>
                    </Columns>
                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#7C6F57" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
            <asp:GridView ID="grdExpnlServerErrors" runat="server">
            </asp:GridView>
        </div>
      
       <div class="ServerStatus">
       <p class="Heading">PricingServerStatus</p>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdPricingServer" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="IdentifierName" HeaderText="Name" >
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ConnStatus" HeaderText="Status" >
                            <ItemStyle Width="40px" />
                        </asp:BoundField>
                    </Columns>
                    
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#7C6F57" Font-Bold="True" ForeColor="White" />
           
                </asp:GridView><asp:GridView ID="grdPricingMiscConn" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="IdentifierName" HeaderText="Name" >
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ConnStatus" HeaderText="Status" >
                            <ItemStyle Width="40px" />
                        </asp:BoundField>
                    </Columns>
                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#7C6F57" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
           <asp:GridView ID="grdPricingServerErrors" runat="server">
           </asp:GridView>
        </div>
        <div class="ServerStatus">
        <p class="Heading">TradeServerStatus</p>
        <asp:UpdatePanel ID="updatePanelTradeServerStatus" runat="server" >
            <ContentTemplate>
                <asp:GridView ID="GridViewUsers" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="IdentifierName" HeaderText="Name" >
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ConnStatus" HeaderText="Status" >
                            <ItemStyle Width="40px" />
                        </asp:BoundField>
                    </Columns>
                     
            <HeaderStyle BackColor="#7C6F57" Font-Bold="True" ForeColor="White" />
            
                </asp:GridView>
                <asp:GridView ID="GridViewFixConn" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="IdentifierName" HeaderText="Name" >
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ConnStatus" HeaderText="Status" >
                            <ItemStyle Width="40px" />
                        </asp:BoundField>
                    </Columns>
                     
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#7C6F57" Font-Bold="True" ForeColor="White" />
            
                </asp:GridView>
                <asp:GridView ID="grdTradeServerErrors" runat="server">
                
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#7C6F57" Font-Bold="True" ForeColor="White" />
           
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdAllConnections" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
            </Triggers>
        </asp:UpdatePanel>
       </div>
        </div>
    </div>
    </form>
</div>
</div>



<div id="footer"><p>Nirvana Solutions footer</p></div>

</div>


   
</body>
</html>
