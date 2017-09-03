<%@ Page Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="ServiceStatus.aspx.vb" Inherits="ServiceStatus" title="Pay-it Support-ServiceStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link rel="stylesheet" href="Stylesheet/pure.css">
    <link href="Stylesheet/mainstyle.css" rel="Stylesheet" />
    <script src="Scripts/alertify.js"></script>
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
    <link href="Stylesheet/forms.css" rel="stylesheet" />
     <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <div class="wrapper">
        <!-- Content Wrapper. Contains page content -->
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <div class="content-header">
                <h3>Service Status
                </h3>
                <ol class="pull-right">
                    <asp:LinkButton ID="LinkButton1" runat="server">Export Report To Excel</asp:LinkButton>
                </ol>
            </div>
            <div class="alert-danger">
                <asp:Label ID="dberrorlabel" Font-Size="Larger" Font-Bold="true" runat="server" Text=""></asp:Label>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-2"></div>
                        <!-- ./col -->
                        <div class="col-md-6">
                            <div>
                                <div class="form-horizontal">
                                    <fieldset>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label4" runat="server" Text="ServiceType"></asp:Label>
                                            </div>
                                            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label3" runat="server" Text="Reason"></asp:Label>
                                            </div>
                                            <asp:TextBox ID="txtReason" runat="server"></asp:TextBox>
                                        </label>

                                        <div class="label-group input-below">
                                            <label style="visibility: hidden">Push Notifid</label>
                                            <label style="color:#C8C8C8;">Status</label>
                                            <label>
                                                <asp:CheckBox runat="server" ID="isActive" />
                                                <%--<span class="text-default" style="color: #fff;">Status</span>--%>
                                                <asp:Label ID="lblStatus" ForeColor="White" runat="server"></asp:Label>
                                            </label>
                                        </div>

                                        <div class="label-group input-below">
                                            <label style="visibility: hidden">P</label>
                                            <label style="color:#C8C8C8;">Status New Build</label>
                                            <label>
                                                <asp:CheckBox runat="server" ID="isActiveNew" />
                                                <asp:Label ID="lblStatusNew" ForeColor="White" runat="server"></asp:Label>
                                            </label>
                                        </div>

                                        <div class="submit-wrapper">
                                            <asp:Button ID="Button1" runat="server" CssClass="btn btn-default btn-sm" Text="Update" />
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-1"></div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="DropDownList1" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

