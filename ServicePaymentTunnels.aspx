<%@ Page Title="OgMoneyKW:Service Payment Tunnels" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="ServicePaymentTunnels.aspx.vb" Inherits="ServicePaymentTunnels" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="Stylesheet/mainstyle.css" rel="Stylesheet" />
    <link rel="stylesheet" href="Stylesheet/pure.css">
    <link href="Stylesheet/forms.css" rel="Stylesheet" />
 
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
    <style>
        <style scoped > .button-success,
        .button-error,
        .button-warning,
        .button-secondary
        {
            color: white;
            border-radius: 4px;
            text-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
        }

        .button-success
        {
            background: rgb(28, 184, 65); /* this is a green */
        }

        .button-error
        {
            background: rgb(202, 60, 60); /* this is a maroon */
        }

        .button-warning
        {
            background: rgb(223, 117, 20); /* this is an orange */
        }

        .button-secondary
        {
            background: rgb(66, 184, 221); /* this is a light blue */
        }

        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .modalPopup
        {
            background-color: #272b30;
            color: #f5f5f5;
            position: relative;
            overflow-y: auto;
            padding: 15px;
            border-bottom: 1px solid #1c1e22;
            max-width: 600px;
        }

        .hiddencol
        {
            display: none;
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
    </style>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="wrapper">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <!-- Content Wrapper. Contains page content -->
                <div class="content-wrapper">
                    <!-- Content Header (Page header) -->
                    <div class="content-header">
                        <h4>Service PaymentTunnel Configuration
                        </h4>
                        <ol class="pull-right">
                            <asp:LinkButton ID="LinkButton1" runat="server">Export Report To Excel</asp:LinkButton>
                        </ol>
                    </div>
                    <div class="row" id="contact">
                        <div class="col-md-3"></div>
                        <!-- ./col -->
                        <div class="col-md-6">
                            <div class="">
                                <!-- /.box-header -->
                                <div class="form-horizontal">
                                    <fieldset>
                                        <p>Add Configuration</p>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label2" runat="server" Text="Service"></asp:Label>
                                            </div>
                                            <asp:DropDownList ID="ddlSevicePayment" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSevicePayment_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label5" runat="server" Text="Tunnel"></asp:Label>
                                            </div>
                                            <asp:DropDownList ID="ddlTunnel" runat="server" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label6" runat="server" Text="Amount Threshold"></asp:Label>
                                            </div>
                                            <asp:DropDownList ID="ddlThresholdType" runat="server" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label10" runat="server" Text="Threshold Value"></asp:Label>
                                            </div>
                                            <asp:TextBox runat="server" ID="txtThresholdValue"></asp:TextBox>
                                        </label>
                                       
                                        <div class="label-group input-below">
                                            <label style="visibility: hidden">Push Notificationss</label>
                                            <label>
                                                <asp:CheckBox runat="server" ID="chkStatus" AutoPostBack="false" />
                                                <span class="text-default" style="color: #fff;">Status</span>
                                            </label>
                                        </div>
                                        <div class="submit-wrapper pull-right">
                                            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-default btn-sm" Text="Add Configuration" />
                                            <asp:Button ID="btnDelete" CssClass="button-primary pure-button pull-right" runat="server" Text="Cancel" OnClick="Clear" />
                                            <asp:Button ID="btnSave" CssClass="button-success pure-button" runat="server" Text="Save" OnClick="Save" />
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                            <div class="col-md-3"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="box box-success">
                                <h4 class="box-title">Configuration Details</h4>
                                <div class="box-body">
                                    <asp:GridView ID="GridView1" runat="server" OnRowDataBound="OnRowDataBound"
                                        EmptyDataText="No Data" CssClass="table table-bordered table-responsive" AutoGenerateColumns="false" BackColor="#2b2028" HeaderStyle-ForeColor="White" HorizontalAlign="Center"
                                        PageSize="20" AllowPaging="true" OnPageIndexChanging="OnPaging" ForeColor="White">
                                        <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                        <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                        <HeaderStyle BackColor="Gray" Font-Bold="True" HorizontalAlign="Left" />
                                        <RowStyle HorizontalAlign="Left" />
                                        <EmptyDataTemplate>
                                            <div class="spinner">
                                                <div class="double-bounce1"></div>
                                                <div class="double-bounce2"></div>
                                            </div>
                                            <div style="text-align: center">
                                                No Configuration Found
                                            </div>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:BoundField DataField="ID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="ID" SortExpression="ID" />
                                            <asp:BoundField DataField="ServiceCode" HeaderText="Service" SortExpression="ServiceCode" />
                                            <asp:BoundField DataField="PaymentName" HeaderText="Payment" SortExpression="PaymentName" />
                                            <asp:BoundField DataField="TunnelAlias" HeaderText="Tunnel" SortExpression="TunnelCode" />
                                            <asp:BoundField DataField="ThresholdType" HeaderText="AmountThreshold" SortExpression="ThresholdType" />
                                            <asp:BoundField DataField="ThresholdValue" HeaderText="Value" SortExpression="ThresholdValue" />
                                            <asp:BoundField DataField="UpdatedBy" HeaderText="By" SortExpression="UpdatedBy" />
                                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                <ItemTemplate><%#IIf(Boolean.Parse(Eval("Status").ToString()), "Active", "Inactive")%></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" ForeColor="SteelBlue" runat="server" Text="Edit" OnClick="Edit"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>

                    <label class="hiddencol">
                        <div>
                            <asp:Label ID="Label7" runat="server" Text="ID"></asp:Label>
                        </div>
                        <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
                    </label>

                    <%--<asp:Panel ID="pnlAddEdit" runat="server" CssClass="panel panel-danger modalPopup">
                        <div class="form-horizontal">
                            <p>Edit Details</p>
                            <fieldset>
                                <label class="hiddencol">
                                    <div>
                                        <asp:Label ID="Label7" runat="server" Text="ID"></asp:Label>
                                    </div>
                                    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label17" runat="server" Text="Rule Type"></asp:Label>
                                    </div>
                                    <asp:DropDownList runat="server" ID="ddlRuleTypeEdit"></asp:DropDownList>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label3" runat="server" Text="Configuration"></asp:Label>
                                    </div>
                                    <asp:DropDownList runat="server" ID="ddlConfigEdit"></asp:DropDownList>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label4" runat="server" Text="User Type"></asp:Label>
                                    </div>
                                    <asp:DropDownList runat="server" ID="dllUserTypeEdit"></asp:DropDownList>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label11" runat="server" Text="Limit Type"></asp:Label>
                                    </div>
                                    <asp:DropDownList runat="server" ID="ddlLimitTypeEdit"></asp:DropDownList>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label8" runat="server" Text="Limit Value"></asp:Label>
                                    </div>
                                    <asp:TextBox runat="server" ID="txtLimitValueEdit"></asp:TextBox>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label9" runat="server" Text="Duration"></asp:Label>
                                    </div>
                                    <asp:TextBox runat="server" ID="txtDurationEdit"></asp:TextBox>
                                </label>

                                <div class="label-group input-below">
                                    <label style="visibility: hidden">Push Notificationss</label>
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkStatEdit" AutoPostBack="false" />
                                        <span class="text-default" style="color: #fff;">Status</span>
                                    </label>
                                </div>

                                <div style="border: none;">
                                    <div class="submit-wrapper pull-right">
                                    </div>
                                </div>
                                <div style="border: none;">
                                    <div class="submit-wrapper pull-left">
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </asp:Panel>--%>
                    <%-- <center>
                        <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
                        <cc1:ModalPopupExtender ID="popup" runat="server" DropShadow="false"
                            PopupControlID="pnlAddEdit" TargetControlID="lnkFake"
                            BackgroundCssClass="modalBackground">
                        </cc1:ModalPopupExtender>
                    </center>--%>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="GridView1" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" />
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>


