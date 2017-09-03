<%@ Page Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="ZainPrepaidSummary.aspx.vb" Inherits="ZainPrepaidSummary" title="Pay-it Support-Zain-X Summary" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <link href="Stylesheet/forms.css" rel="Stylesheet" />
     <link href="Stylesheet/mainstyle.css" rel="Stylesheet" />
    <script src="Scripts/alertify.js"></script>
    <!-- CSS -->
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
    <style>
        #ctl00_ContentPlaceHolder1_ddlService
        {
            color: darkslategray;
            font-family: sans-serif;
            box-shadow: none;
        }
        input[type="text"]
        {
            color: darkslategray;
            font-family: sans-serif;
            box-shadow: none;
        }
        input[type="text"]:focus
        {
            outline: none;
        }
            .GridPager a, .GridPager span
        {
            display: block;
            height: 18px;
            width: 18px;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }
        .GridPager a
        {
            color: #fff;
            border: 1px solid #969696;
        }
        .GridPager span
        {
            background-color: #A1DCF2;
            color: #000;
            border: 1px solid #3AC0F2;
        }
    </style>
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
                    Text="Service Summary"></asp:Label>
    
        <br />
    
        <asp:SqlDataSource ID="ProcessTypeSqlDataSource" runat="server" 
            ConnectionString="<%$ ConnectionStrings:payitConnectionActive %>" 
            SelectCommand="SELECT distinct([knetprocess]) FROM [ThirdParty_knet_trans] where knetprocess is not null ORDER BY [knetprocess]">
        </asp:SqlDataSource>
    
    <table  >
        <tr>
            <td style="width: 47px; text-align: left">
                <asp:Label ID="Label16" runat="server" CssClass="label label-primary"  Text="Service: "></asp:Label>
            </td>
            <td style="text-align: left; width: 133px;">
                <asp:DropDownList ID="ddlService" runat="server" AutoPostBack="false" >
                </asp:DropDownList>
            </td>
        </tr>
         <tr>
            <td style="width: 47px; text-align: left">
                <asp:Label ID="Label1" runat="server" CssClass="label label-primary"  Text="Denomination: "></asp:Label>
            </td>
            <td style="text-align: left; width: 133px;">
                <asp:DropDownList ID="ddlDenomination" runat="server" AutoPostBack="false" >
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 47px; text-align: left">
                <asp:Label ID="FromDateLabel" runat="server" CssClass="label label-primary"  Text="FromDate:"></asp:Label>
            </td>
            <td style="text-align: left; width: 133px;">
                <asp:TextBox ID="FromDateTextBox" runat="server" ></asp:TextBox>
               <%-- <cc1:CalendarExtender ID="FromDateTextBox_CalendarExtender" runat="server" 
                    Enabled="True" Format="dd/MM/yyyy" TargetControlID="FromDateTextBox" 
                    onclientshowing="CurrentDateShowing" CssClass="cal_KimTheme">
                </cc1:CalendarExtender>--%>
                <asp:Label ID="Label14" runat="server" Font-Names="Times New Roman" 
                    ForeColor="#FF3300" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: left">
                <asp:Label ID="ToDateLabel" runat="server" CssClass="label label-primary" Text="To Date: " ></asp:Label>
            </td>
            <td  style="width: 133px; text-align: left;">
                <asp:TextBox ID="ToDateTextBox" runat="server" ></asp:TextBox>
             <%--   <cc1:CalendarExtender ID="ToDateTextBox_CalendarExtender" runat="server" 
                    Enabled="True" Format="dd/MM/yyyy" TargetControlID="ToDateTextBox" 
                    onclientshowing="CurrentDateShowing" CssClass="cal_KimTheme">
                </cc1:CalendarExtender>--%>
                <asp:Label ID="Label15" runat="server" Font-Names="Times New Roman" 
                    ForeColor="#FF3300" Text="*"></asp:Label>
            </td>
        </tr>
        <tr >
            <td style="text-align: right">
                &nbsp;</td>
            <td style="text-align: center; width: 133px;">
                <asp:Button ID="Button2" runat="server" CssClass="btn btn-default" Text="Search" />
            </td>
        </tr>
    </table>
    <div ID="div1">
                        </div>
     <%--   <br />
            <asp:GridView ID="GridView2" runat="server"
            EmptyDataText="No Data" CssClass="table table-bordered table-responsive" BackColor="#2b2028" HeaderStyle-ForeColor="White" HorizontalAlign="Center" 
            PageSize="50" ForeColor="White">
            <PagerStyle HorizontalAlign="Center" CssClass = "GridPager" />
            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
            <HeaderStyle BackColor="Gray" Font-Bold="True" HorizontalAlign="Left" />
            <RowStyle HorizontalAlign="Left" />
        </asp:GridView>   --%>
        <asp:Label ID="Label17" runat="server" Font-Size="Smaller" CssClass="label label-warning"></asp:Label>
            <asp:Label ID="Label18" runat="server" Font-Size="Smaller" CssClass="label label-success"></asp:Label>
       <%-- <asp:Label ID="Label17" runat="server" Font-Bold="True" ForeColor="#33CCCC" 
            Font-Size="Smaller"></asp:Label><span style="margin-right:15px"></span>
        <asp:Label ID="Label18" runat="server" Font-Bold="True" Font-Size="Smaller" 
            ForeColor="#33CCCC"></asp:Label>--%>
        <br /><br />
        
    <asp:GridView ID="GridView1" runat="server" 
                 CssClass="table table-bordered table-responsive" HeaderStyle-ForeColor="White" BackColor="#2b2028" ForeColor="White" HorizontalAlign="Center" PageSize="50" >
                    <PagerStyle HorizontalAlign="Center" CssClass = "GridPager" />
                    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                    <HeaderStyle BackColor="Gray" Font-Bold="True" HorizontalAlign="Left" />
                    <RowStyle HorizontalAlign="Left" />
                </asp:GridView>
    </ContentTemplate>
    </asp:UpdatePanel>
    <script src="Scripts/bootstrap-datepicker.js"></script>
    <script type="text/javascript">
        function pageLoad() {
            $("#ctl00_ContentPlaceHolder1_FromDateTextBox").unbind();
            $('#ctl00_ContentPlaceHolder1_FromDateTextBox').datepicker({
                format: "dd/mm/yyyy",
                todayBtn: "linked",
                todayHighlight: true,
                autoclose: true,
                multidate: false
            });
            $("#ctl00_ContentPlaceHolder1_ToDateTextBox").unbind();
            $('#ctl00_ContentPlaceHolder1_ToDateTextBox').datepicker({
                format: "dd/mm/yyyy",
                todayBtn: "linked",
                autoclose: true,
                todayHighlight: true
            });
        }
    </script>
</asp:Content>


