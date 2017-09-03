<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SummaryReport.aspx.vb" Inherits="SummaryReport" MasterPageFile="~/MasterPage2.master" Title="Pay-it Support-Summary" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     
    <script language ="javascript">
function abcf()
{
// alert("false");
 //img12.style.display ="none"   
   //img12.style.backgroundColor ="red" 
}
function abct()
{
//alert("True");
 //div1.style.backgroundColor ="white" 
 //div1.innerHTML="Loading Data.."
 div1.innerHTML="<img src=loading_black.gif> <font Color=#00C0C0>Loading..</font>"//gallery-loading
 div1.style .width ="50"
 //img12.style.display="block" 
}
</script>
<div style="text-align: right">
            <asp:LinkButton ID="LinkButton1" runat="server" Font-Names="Centaur" 
                ForeColor="#00C3C6">Export Report To Excel</asp:LinkButton>
        </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
     <asp:Label ID="Label8" runat="server" Font-Bold="False" Font-Size="Larger" 
                    Font-Underline="False" ForeColor="White" style="text-align: center" 
                    Text="Summary Report" Font-Names="Times New Roman"></asp:Label>
    
        <br />
    
        <asp:SqlDataSource ID="ProcessTypeSqlDataSource" runat="server" 
            ConnectionString="<%$ ConnectionStrings:payitConnectionActive %>" 
            SelectCommand="SELECT distinct([knetprocess]) FROM [ThirdParty_knet_trans] where knetprocess is not null ORDER BY [knetprocess]">
        </asp:SqlDataSource>
    
    <table  >
        <tr>
            <td style="width: 47px; text-align: left">
                <asp:Label ID="Label19" runat="server" Font-Bold="False" Font-Names="Centaur" 
                    ForeColor="#00C0C0" Height="17px" Text="Data Source :" Visible="False" 
                    Width="112px"></asp:Label>
            </td>
            <td style="text-align: left; width: 133px;">
                <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="True" 
                    Visible="False" Width="150px">
                    <asp:ListItem>ThirdParty_knet_trans</asp:ListItem>
                    <asp:ListItem>ThirdParty_knet_trans_20160420</asp:ListItem>
                    <asp:ListItem>ThirdParty_knet_trans_20160324</asp:ListItem>
                    <asp:ListItem>ThirdParty_knet_trans_WithPartitions</asp:ListItem>
                    <asp:ListItem>ThirdParty_knet_trans_20151231</asp:ListItem>
                    <asp:ListItem>ThirdParty_knet_trans_20150728_20150922</asp:ListItem>
                    <asp:ListItem>ThirdParty_knet_trans20141116</asp:ListItem>
                    <asp:ListItem>ThirdParty_knet_trans_20140501_20140531</asp:ListItem>
                    <asp:ListItem>ThirdParty_knet_trans_20140201_20140430</asp:ListItem>                    
                    <asp:ListItem>ThirdParty_knet_trans_20131021_20140131</asp:ListItem>
                    <asp:ListItem>ThirdParty_knet_trans_20130620_20131020</asp:ListItem>
                    <asp:ListItem>ThirdParty_knet_trans_20130101_20130620</asp:ListItem>
                    <asp:ListItem>ThirdParty_knet_trans_20120827_20121231</asp:ListItem>
                    <asp:ListItem>ThirdParty_knet_trans_20100628_20120827</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 47px; text-align: left">
                <asp:Label ID="Label16" runat="server" Font-Bold="False" Font-Names="Centaur" 
                    ForeColor="#00C0C0" Height="17px" Text="Service Provider :" Visible="False" 
                    Width="112px"></asp:Label>
            </td>
            <td style="text-align: left; width: 133px;">
                <asp:DropDownList ID="ddlServiceProvider" runat="server" AutoPostBack="True" 
                    Visible="False" Width="100px">
                    <asp:ListItem>All</asp:ListItem>
                    <asp:ListItem>iSYS</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 47px; text-align: left">
                <asp:Label ID="FromDateLabel" runat="server" Font-Bold="False" Font-Names="Centaur" 
                    ForeColor="#00C0C0" Height="17px" Text="FromDate :" Width="112px"></asp:Label>
            </td>
            <td style="text-align: left; width: 133px;">
                <asp:TextBox ID="FromDateTextBox" runat="server" Width="100px"></asp:TextBox>
                <cc1:CalendarExtender ID="FromDateTextBox_CalendarExtender" runat="server" 
                    Enabled="True" Format="dd/MM/yyyy" TargetControlID="FromDateTextBox" 
                    onclientshowing="CurrentDateShowing" CssClass="cal_KimTheme">
                </cc1:CalendarExtender>
                <asp:Label ID="Label14" runat="server" Font-Names="Times New Roman" 
                    ForeColor="#FF3300" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: left">
                <asp:Label ID="ToDateLabel" runat="server" Font-Bold="False" Font-Names="Centaur" 
                    ForeColor="#00C0C0" Height="17px" Text="ToDate :" Width="112px"></asp:Label>
            </td>
            <td  style="width: 133px; text-align: left;">
                <asp:TextBox ID="ToDateTextBox" runat="server" Width="100px"></asp:TextBox>
                <cc1:CalendarExtender ID="ToDateTextBox_CalendarExtender" runat="server" 
                    Enabled="True" Format="dd/MM/yyyy" TargetControlID="ToDateTextBox" 
                    onclientshowing="CurrentDateShowing" CssClass="cal_KimTheme">
                </cc1:CalendarExtender>
                <asp:Label ID="Label15" runat="server" Font-Names="Times New Roman" 
                    ForeColor="#FF3300" Text="*"></asp:Label>
            </td>
        </tr>
        <tr >
            <td style="text-align: left">
                <asp:CheckBox ID="CheckBox1" runat="server" Text="Device Info" 
                    ForeColor="#00C0C0" Height="17pt" AutoPostBack="true" />
            </td>
            <td style="text-align: center; width: 133px;">
                <asp:Button ID="Button2" runat="server" Text="Display" style="text-align: left; height: 26px;" 
                    Width="71px" />
            </td>
        </tr>
    </table>
    <div ID="div1">
                        </div>
                        
        <asp:Label ID="Label17" runat="server" Font-Bold="True" ForeColor="#33CCCC" 
            Font-Size="Smaller"></asp:Label>
        <br />
        <asp:Label ID="Label18" runat="server" Font-Bold="True" Font-Size="Smaller" 
            ForeColor="#33CCCC"></asp:Label>
        <br />
    <asp:GridView ID="GridView1" runat="server" 
                BorderColor="DimGray" 
                BorderStyle="Outset" BorderWidth="2px" CellPadding="0" 
                Font-Bold="True" 
                Font-Size="Smaller" ForeColor="White" HorizontalAlign="Center" 
            PageSize="50" Font-Names="Calibri">
                    <RowStyle HorizontalAlign="Center" Font-Names="Calibri"  />
                    <FooterStyle BorderWidth="5px" />
                    <HeaderStyle BorderStyle="Dotted" ForeColor="#00C0C0" />
                    <AlternatingRowStyle BackColor="#666666" BorderColor="#666666" 
                    Font-Bold="False" ForeColor="White" 
                        BorderWidth="1px" Font-Size="Small" />
                </asp:GridView>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

