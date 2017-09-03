<%@ Page Title="Payit:Pin Stock" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="PinStock.aspx.vb" Inherits="PinStock" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <link rel="stylesheet" href="Stylesheet/pure.css">
    <script src="Scripts/alertify.js"></script>
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
    <link href="Stylesheet/forms.css" rel="stylesheet" />
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }

        .links
        {
            font-weight: bold;
        }

        .hiddencol
        {
            display: none;
        }

        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .modalPopup
        {
            background-color: #1c1e22;
            color: #f5f5f5;
            position: relative;
            overflow-y: auto;
            padding: 15px;
            border-bottom: 1px solid #1c1e22;
            max-width: 600px;
            margin-bottom: 5px;
        }
    </style>
    <div class="wrapper">
        <!-- Content Wrapper. Contains page content -->
        <div class="content-wrapper">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <!-- Content Header (Page header) -->
                    <div class="content-header">
                        <h4>PIN StockReturn Details
                        </h4>
                        <ol class="pull-right">
                            <asp:LinkButton ID="LinkButton1" runat="server">Export Report To Excel</asp:LinkButton>
                            <div class="label-group input-below">
                                <label style="visibility: hidden">Statusdddddddsd</label>
                                <label>
                                    <asp:CheckBox runat="server" ID="chkPaging" Checked="true" AutoPostBack="true" />
                                    <asp:Label runat="server" ID="lblPaging" Text="Disable Paging"></asp:Label>
                                </label>
                            </div>
                        </ol>
                    </div>
                    <div class="alert-danger">
                        <asp:Label ID="dberrorlabel" Font-Size="Larger" Font-Bold="true" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="row" id="contact">
                        <div class="col-md-3">
                        </div>
                        <!-- ./col -->
                        <div class="col-md-6" style="float: inherit">
                            <div class="">
                                <!-- /.box-header -->
                                <div class="form-horizontal">
                                    <fieldset>
                                        <p>
                                            Search PIN 
                                        </p>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label1" runat="server" Text="Serial"></asp:Label>
                                            </div>
                                            <asp:TextBox ID="txtSerial" runat="server" AutoPostBack="false"></asp:TextBox>
                                        </label>
                                        <div class="submit-wrapper pull-right">
                                            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-default btn-sm" Text="Search" />
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3"></div>
                    </div>

                    <div class="hidden">
                        <asp:Label ID="ServiceLB" Visible="false" runat="server"></asp:Label>
                        <asp:Label ID="SerialLb" Visible="false" runat="server"></asp:Label>
                        <asp:Label ID="PinLB" Visible="false" runat="server"></asp:Label>
                        <asp:Label ID="VendorLB" Visible="false" runat="server"></asp:Label>
                        <asp:Label ID="AmountLB" Visible="false" runat="server"></asp:Label>
                        <asp:Label ID="OrderDateLb" Visible="false" runat="server"></asp:Label>
                        <asp:Label ID="Amount2LB" Visible="false" runat="server"></asp:Label>
                    </div>

                    <div class="row">
                        <div class="col-lg-12">
                            <div class="box box-success">
                                <h4 class="box-title" runat="server" id="PageHeader"></h4>
                                <div class="box-body">
                                    <asp:GridView ID="GridView1" runat="server"
                                        CssClass="table table-bordered table-responsive" EmptyDataText="No Priority Found"
                                        HeaderStyle-ForeColor="White" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="OnPaging"
                                        BackColor="#2b2028" ForeColor="White" HorizontalAlign="Center" PageSize="25">
                                        <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                        <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                        <HeaderStyle BackColor="Gray" Font-Bold="True" HorizontalAlign="Left" />
                                        <RowStyle HorizontalAlign="Left" />
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderText="ID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" SortExpression="ID" />
                                            <asp:BoundField DataField="Service" ReadOnly="true" HeaderText="Service" SortExpression="Service" />
                                            <asp:BoundField DataField="Vendor" ReadOnly="true" HeaderText="Vendor" SortExpression="Vendor" />
                                            <asp:BoundField DataField="Amount" ReadOnly="true" HeaderText="Amount" SortExpression="Amount" />
                                            <asp:BoundField DataField="Denomination" ReadOnly="true" HeaderText="Denomination" SortExpression="Amount2" />
                                            <asp:BoundField DataField="Serial" ReadOnly="true" HeaderText="Serial" SortExpression="Serial" />
                                            <asp:BoundField DataField="DecryptedPIN" ReadOnly="true" HeaderText="DecryptedPIN" SortExpression="DecryptedPIN" />
                                            <asp:BoundField DataField="OrderDate" ReadOnly="true" HeaderText="OrderDate" SortExpression="OrderDate" />
                                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                <ItemTemplate><%#IIf(Boolean.Parse(Eval("Status").ToString()), "Sold", "InStock")%></ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="CreatedDate" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="CreatedDate" Visible="false" SortExpression="CreatedDate" />
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" ForeColor="SteelBlue" runat="server" Text="Edit" OnClick="Edit"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <h4 runat="server" id="headinglb"></h4>
                                    <asp:GridView ID="GridView2" runat="server"
                                        CssClass="table table-bordered table-responsive" EmptyDataText="No History Found"
                                        HeaderStyle-ForeColor="White" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="OnPaging"
                                        BackColor="#2b2028" ForeColor="White" HorizontalAlign="Center" PageSize="25">
                                        <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                        <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                        <HeaderStyle BackColor="Gray" Font-Bold="True" HorizontalAlign="Left" />
                                        <RowStyle HorizontalAlign="Left" />
                                        <Columns>
                                            <asp:BoundField DataField="Service" HeaderText="Service" SortExpression="Service" />
                                            <asp:BoundField DataField="Vendor" HeaderText="Vendor" SortExpression="Vendor" />
                                            <asp:BoundField DataField="Amount" HeaderText="Denom" SortExpression="Amount" />
                                            <asp:BoundField DataField="Amount2" HeaderText="Amount" SortExpression="Amount2" />
                                            <asp:BoundField DataField="Serial" HeaderText="Serial" SortExpression="Serial" />
                                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                <ItemTemplate><%#IIf(Boolean.Parse(Eval("Status").ToString()), "Sold", "InStock")%></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Reason" HeaderText="Reason" SortExpression="Reason" />
                                            <asp:BoundField DataField="ReturnedBy" HeaderText="ReturnedBy" SortExpression="ReturnedBy" />
                                            <asp:BoundField DataField="OrderDate" HeaderText="OrderDate" SortExpression="OrderDate" />
                                            <asp:BoundField DataField="ReturnDate" HeaderText="ReturnDate" SortExpression="ReturnDate" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Panel ID="pnlAddEdit" runat="server" CssClass="panel panel-danger modalPopup">
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
                                                    <asp:Label ID="ServiceVal" CssClass="alert alert-success" runat="server"></asp:Label>
                                                </label>
                                                <label>
                                                    <asp:Label ID="AmountVal" CssClass="alert alert-warning" runat="server"></asp:Label>
                                                </label>
                                                <label>
                                                    <asp:Label ID="DenominationVal" CssClass="alert alert-info" runat="server"></asp:Label>
                                                </label>
                                                <label>
                                                    <asp:Label ID="SerialVal" CssClass="alert alert-danger" runat="server"></asp:Label>
                                                </label>
                                                <label>
                                                    <asp:Label ID="PinVal" CssClass="alert alert-success" runat="server"></asp:Label>
                                                </label>
                                                <div class="label-group input-below">
                                                    <label style="visibility: hidden">Push Notificationss</label>
                                                    <label>
                                                        <asp:CheckBox runat="server" ID="chkStatEdit" AutoPostBack="false" />
                                                        <span class="text-default" style="color: #fff;">Status</span>
                                                    </label>
                                                </div>
                                                <label>
                                                    <div>
                                                        <asp:Label ID="Label2" runat="server" Text="Reason"></asp:Label>
                                                    </div>
                                                    <asp:TextBox ID="txtReason" CssClass="form-control" runat="server"></asp:TextBox>
                                                    <asp:Label ID="txtProcessedByError" CssClass="alert alert-info" runat="server" Visible="false"></asp:Label>
                                                </label>
                                                <div style="border: none;">
                                                    <div class="submit-wrapper pull-right">
                                                        <asp:Button ID="btnCancel" CssClass="button-warning pure-button" runat="server" Text="Cancel" OnClientClick="return Hidepopup()" />
                                                        <asp:Button ID="btnSave" CssClass="button-success pure-button" runat="server" Text="Edit Status" OnClick="Save" />
                                                    </div>
                                                </div>
                                                <div style="border: none;">
                                                    <div class="submit-wrapper pull-left">
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </asp:Panel>
                                    <center>
                                        <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
                                        <cc1:ModalPopupExtender ID="popup" runat="server" DropShadow="false"
                                            PopupControlID="pnlAddEdit" TargetControlID="lnkFake"
                                            BackgroundCssClass="modalBackground">
                                        </cc1:ModalPopupExtender>
                                    </center>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GridView1" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>