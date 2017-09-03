<%@ Page Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="IntlTopUpSupport.aspx.vb" Inherits="IntlTopUpSupport" title="Pay-it Support-IntlTransaction" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <br />
    
    
    
    <table  >
        <tr >
            <td style="text-align: center" colspan="2">
                <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Size="Larger" 
                    ForeColor="White" style="text-align: center" 
                    Text="Intl. Transaction Details " Font-Underline="True"></asp:Label>
                <br />
                <br />
                <br />
            </td>
        </tr>
        <tr >
            <td style="width: 47px; text-align: left">
                <asp:Label ID="Label4" runat="server" Font-Bold="False" Font-Names="Centaur" 
                    ForeColor="#00C0C0" Height="17px" Text="Mobile Number :" Width="112px"></asp:Label>
            </td>
            <td style="text-align: left; width: 133px;">
                <asp:TextBox ID="TextBox1" runat="server" Width="100px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: left">
                <asp:Label ID="Label7" runat="server" Font-Bold="False" Font-Names="Centaur" 
                    ForeColor="#00C0C0" Height="17px" Text="Track ID :" Width="112px"></asp:Label>
            </td>
            <td  style="width: 133px; text-align: left;">
                <asp:TextBox ID="TextBox2" runat="server" Width="100px"></asp:TextBox>
            </td>
        </tr>
        <tr >
            <td style="text-align: right">
                &nbsp;</td>
            <td style="text-align: center; width: 133px;">
                <asp:Button ID="Button2" runat="server" Text="Display" style="text-align: left" 
                    Width="71px" />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
    <br />
   <asp:GridView ID="GridView1" runat="server" 
                    BorderColor="DimGray" 
                    BorderStyle="Outset" BorderWidth="2px" CellPadding="0" 
                    Font-Bold="True" Font-Names="Calibri" 
                    Font-Size="Smaller" ForeColor="White" HorizontalAlign="Center" 
                    Width="212px" AllowPaging="true">
                    <RowStyle HorizontalAlign="Center" />
                    <FooterStyle BorderWidth="5px" />
                    <HeaderStyle BorderStyle="Dotted" ForeColor="#666666" />
                    <AlternatingRowStyle BackColor="#666666" BorderColor="#666666" 
                    Font-Bold="False" Font-Names="Calibri" Font-Size="Small" ForeColor="White" 
                        BorderWidth="1px" />
                </asp:GridView>
</asp:Content>

