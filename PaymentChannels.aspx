<%@ Page Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="PaymentChannels.aspx.vb" Inherits="PaymentChannels" title="Untitled Page" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- JavaScript -->
    <script src="Scripts/alertify.js"></script>
    <!-- CSS -->
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="Button1" />
        </Triggers>
        <ContentTemplate>
            <br />
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:payitConnectionActive %>"
                SelectCommand="SELECT distinct([knetprocess]) FROM [ThirdParty_knet_trans] where knetprocess is not null ORDER BY [knetprocess]"></asp:SqlDataSource>
            <table>
                <tr>
                    <td style="width: 47px; text-align: left">
                        <asp:Label ID="Label2" runat="server" Font-Bold="False" Font-Names="Centaur" ForeColor="#00C0C0"
                            Height="17px" Text="Pay through :" Width="112px"></asp:Label>
                    </td>
                    <td style="text-align: left; width: 133px;">
                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True">
                            <asp:ListItem>Knet</asp:ListItem>
                            <asp:ListItem>CreditCard</asp:ListItem>
                            <asp:ListItem>CashU</asp:ListItem>
                            <asp:ListItem>AMEX</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <tr>
                        <td style="width: 47px; text-align: left">
                            <asp:Label ID="Label3" runat="server" Font-Bold="False" Font-Names="Centaur"
                                ForeColor="#00C0C0" Text="Status :" Width="112px"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 133px;">
                            <asp:CheckBox ID="isActive" runat="server" Font-Names="Centaur"
                                ForeColor="#00C0C0" Text="isActive" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 47px; text-align: left">
                            <asp:Label ID="Label6" runat="server" Font-Bold="False" Font-Names="Centaur"
                                ForeColor="#00C0C0" Text="Reason :" Width="112px"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 133px;">
                            <asp:TextBox ID="txtReason" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">&nbsp;
                        </td>
                        <td style="text-align: left;">
                            <asp:Button ID="Button1" runat="server"
                                Style="text-align: left; height: 26px; margin-bottom: 0px;"
                                Text="Update" />
                        </td>
                    </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


