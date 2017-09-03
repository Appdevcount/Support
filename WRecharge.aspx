<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="WRecharge.aspx.vb" Inherits="WRecharge" %>

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
                    Text="WRecharge MO/MT Report" Font-Names="Times New Roman"></asp:Label>
    
        <br />
<%--    
        <asp:SqlDataSource ID="ProcessTypeSqlDataSource" runat="server" 
            ConnectionString="<%$ ConnectionStrings:payitConnectionActive %>" 
            SelectCommand="SELECT distinct([knetprocess]) FROM [ThirdParty_knet_trans] where knetprocess is not null ORDER BY [knetprocess]">
        </asp:SqlDataSource>--%>
    
    <table  >
        <%--<tr>
            <td style="width: 47px; text-align: left">
                <asp:Label ID="Label16" runat="server" Font-Bold="False" Font-Names="Centaur" 
                    ForeColor="#00C0C0" Height="17px" Text="Service  :" Width="112px"></asp:Label>
            </td>
            <td style="text-align: left; width: 133px;">
                <asp:DropDownList ID="ddlService" runat="server" AutoPostBack="True" 
                    Width="100px">
                    <asp:ListItem>All</asp:ListItem>
                    <asp:ListItem>iTunes-O</asp:ListItem>
                    <asp:ListItem>iTunes-UK-O</asp:ListItem>                    
                    <asp:ListItem>CashU-O</asp:ListItem>
                    <asp:ListItem>OneCard-O</asp:ListItem>
                    <asp:ListItem>VIVA-O</asp:ListItem>
                    <asp:ListItem>Zain-O</asp:ListItem>
                    <asp:ListItem>Wataniya-O</asp:ListItem>
                    <asp:ListItem>Zain-X</asp:ListItem>
                    <asp:ListItem>Koutbo6-O</asp:ListItem>
                    <asp:ListItem>UKash-O</asp:ListItem>
                    <asp:ListItem>PlayStation-O</asp:ListItem>
                    <asp:ListItem>GooglePlay-O</asp:ListItem>
                    <asp:ListItem>Amazon-O</asp:ListItem>                    
                </asp:DropDownList>
            </td>
        </tr>--%>
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
        <tr>
            <td style="text-align: left">
                <asp:Label ID="Label3" runat="server" Font-Bold="False" Font-Names="Centaur" 
                    ForeColor="#00C0C0" Height="17px" Text="Mobile No:" Width="112px"></asp:Label>
            </td>
            <td  style="width: 133px; text-align: left;">
                <asp:TextBox ID="MobileNoFilterTextBox" runat="server" Width="100px"></asp:TextBox>
            </td>
        </tr>        
        <tr >
            <td style="text-align: right">
                &nbsp;</td>
            <td style="text-align: center; width: 133px;">
                <asp:Button ID="Button2" runat="server" Text="Display" style="text-align: left; height: 26px;" 
                    Width="71px" />
            </td>
        </tr>
    </table>
    
                    <br /><br /><br /><br />
                    
    <div ID="div1">
                        </div>
                        
        <asp:Label ID="Label17" runat="server" Font-Bold="True" ForeColor="#33CCCC" 
            Font-Size="Smaller" Visible="False"></asp:Label>
        <br />
        <asp:Label ID="Label18" runat="server" Font-Bold="True" Font-Size="Smaller" 
            ForeColor="#33CCCC"></asp:Label>
        <br />
        <asp:Label ID="Label19" runat="server" Font-Bold="True" Font-Size="Smaller" 
            ForeColor="#33CCCC" Text="MO Report"></asp:Label>
        <br />
    <asp:GridView ID="GridView1" runat="server" 
                BorderColor="DimGray" 
                BorderStyle="Outset" BorderWidth="2px" CellPadding="0" 
                Font-Bold="True" 
                Font-Size="Smaller" ForeColor="White" HorizontalAlign="Center" 
            PageSize="25" Font-Names="Calibri" AllowPaging="True">
                    <RowStyle HorizontalAlign="Center" Font-Names="Calibri"  />
                    <FooterStyle BorderWidth="5px" />
                    <PagerStyle HorizontalAlign="Center" CssClass = "GridPager" />
                    <HeaderStyle BorderStyle="Dotted" ForeColor="#00C0C0" />
                    <AlternatingRowStyle BackColor="#666666" BorderColor="#666666" 
                    Font-Bold="False" ForeColor="White" 
                        BorderWidth="1px" Font-Size="Small" />
                </asp:GridView>
                
                <br /><br /><br /><br />
                
                    <div ID="div2">
                        </div>
                        
        <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#33CCCC" 
            Font-Size="Smaller" Visible="False"></asp:Label>
        <br />
        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Smaller" 
            ForeColor="#33CCCC"></asp:Label>
        <br />
        <asp:Label ID="Label20" runat="server" Font-Bold="True" Font-Size="Smaller" 
            ForeColor="#33CCCC" Text="MT Report"></asp:Label>
        <br />
    <asp:GridView ID="GridView2" runat="server" 
                BorderColor="DimGray" 
                BorderStyle="Outset" BorderWidth="2px" CellPadding="0" 
                Font-Bold="True" 
                Font-Size="Smaller" ForeColor="White" HorizontalAlign="Center" 
            PageSize="25" Font-Names="Calibri" AllowPaging="True">
                    <RowStyle HorizontalAlign="Center" Font-Names="Calibri"  />
                    <FooterStyle BorderWidth="5px" />
                    <PagerStyle HorizontalAlign="Center" CssClass = "GridPager" />
                    <HeaderStyle BorderStyle="Dotted" ForeColor="#00C0C0" />
                    <AlternatingRowStyle BackColor="#666666" BorderColor="#666666" 
                    Font-Bold="False" ForeColor="White" 
                        BorderWidth="1px" Font-Size="Small" />
                </asp:GridView>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

