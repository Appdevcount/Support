<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="UploadPINS.aspx.vb" Inherits="UploadPINS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <%-- <div style="height: 254px">--%>
    <script src="Scripts/alertify.js"></script>
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <br />
        <br />
    
        <br />
    
        <br />
        <table style="width: 85%; height: 53px;">
            <tr>
                <td style="width: 107px; text-align: left;">
                    <asp:Label ID="Label3" runat="server" ForeColor="#00C0C0" Text="Service"></asp:Label>
                </td>
                <td style="width: 240px; text-align: left;">
    
        <asp:DropDownList ID="DropDownList1" runat="server"       
            DataTextField="ServiceName" DataValueField="ServiceID" AutoPostBack="true" 
                        Height="20px" Width="200px">
        </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 107px; text-align: left;">
                    <asp:Label ID="Label4" runat="server" ForeColor="#00C0C0" Text="Denomination"></asp:Label>
                </td>
                <td style="width: 240px; text-align: left;">
    
        <asp:DropDownList ID="DropDownList2" runat="server" DataTextField="Amount2" 
                        DataValueField="Amount" Height="20px" Width="200px">
        </asp:DropDownList>
                </td>
            </tr>
             <tr>
                <td style="width: 107px; text-align: left;">
                    <asp:Label ID="Label1" runat="server" ForeColor="#00C0C0" Text="Vendor"></asp:Label>
                </td>
                <td style="width: 240px; text-align: left;">
    
        <asp:DropDownList ID="ddlVendor" runat="server" Height="20px" Width="200px">
        </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 107px; text-align: left;">
                    <asp:Label ID="Label2" runat="server" ForeColor="#00C0C0" Text="Description"></asp:Label>
                </td>
                <td style="width: 240px; text-align: left;">
    
                    <asp:TextBox ID="txtInvoice" runat="server" Height="13px" Width="193px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 107px; text-align: left; height: 27px;">
                    <asp:Label ID="Label5" runat="server" ForeColor="#00C0C0" Text="PIN File"></asp:Label>
                </td>
                <td style="width: 240px; text-align: left; height: 27px;">
    
                    <asp:FileUpload ID="FileUpload1" runat="server" />
    
                </td>
            </tr>
            <tr>
                <td style="width: 107px">
                    &nbsp;</td>
                <td style="width: 240px">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 107px">
                    &nbsp;</td>
                <td style="width: 240px; text-align: left;">
        <asp:Button ID="Button1" runat="server" Text="Upload" Height="28px" Width="200px" />
    
                </td>
            </tr>
            <tr>
                <td colspan="2">
        <asp:Label ID="MsgLbl" runat="server" ForeColor="#00C0C0"></asp:Label>
    
                </td>
            </tr>
            </table>
    <%--
    </div>--%>
</asp:Content>

