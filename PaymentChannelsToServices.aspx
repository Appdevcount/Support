<%@ Page Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="PaymentChannelsToServices.aspx.vb" Inherits="PaymentChannelsToServices" title="Untitled Page" %>

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
                            Height="17px" Text="ServiceType :" Width="112px"></asp:Label>
                    </td>
                    <td style="text-align: left; width: 133px;">
                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True"></asp:DropDownList>
                    </td>
                    <tr>
                        <td style="width: 47px; text-align: left">
                            <asp:Label ID="Label3" runat="server" Font-Bold="False" Font-Names="Centaur"
                                ForeColor="#00C0C0" Text="Knet :" Width="112px"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 133px;">
                            <asp:CheckBox ID="KnetIsActive" runat="server" Font-Names="Centaur"
                                ForeColor="#00C0C0" Text="isActive" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 47px;">
                            <asp:Label ID="Label4" runat="server" Font-Bold="False" Font-Names="Centaur"
                                ForeColor="#00C0C0" Text="CreditCard :" Width="112px"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 133px;">
                            <asp:CheckBox ID="CreditCardIsActive" runat="server" Font-Names="Centaur"
                                ForeColor="#00C0C0" Text="isActive" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 47px;">
                            <asp:Label ID="Label1" runat="server" Font-Bold="False" Font-Names="Centaur"
                                ForeColor="#00C0C0" Text="AMEX :" Width="112px"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 133px;">
                            <asp:CheckBox ID="AMEXIsActive" runat="server" Font-Names="Centaur"
                                ForeColor="#00C0C0" Text="isActive" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 47px; text-align: left">
                            <asp:Label ID="Label5" runat="server" Font-Bold="False" Font-Names="Centaur"
                                ForeColor="#00C0C0" Text="CashU :" Width="112px"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 133px;">
                            <asp:CheckBox ID="CashUIsActive" runat="server" Font-Names="Centaur"
                                ForeColor="#00C0C0" Text="isActive" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 47px; text-align: left">
                            <asp:Label ID="Label7" runat="server" Font-Bold="False" Font-Names="Centaur"
                                ForeColor="#00C0C0" Text="PayitCC :" Width="112px"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 133px;">
                            <asp:CheckBox ID="PayitIsActive" runat="server" Font-Names="Centaur"
                                ForeColor="#00C0C0" Text="isActive" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 47px; text-align: left">
                            <asp:Label ID="Label8" runat="server" Font-Bold="False" Font-Names="Centaur"
                                ForeColor="#00C0C0" Text="Wallet :" Width="112px"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 133px;">
                            <asp:CheckBox ID="WalletIsActive" runat="server" Font-Names="Centaur"
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
                                Style="text-align: left; height: 26px; margin-bottom: 0px;" Text="Update" />
                        </td>
                    </tr>
                </tr>
                </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


