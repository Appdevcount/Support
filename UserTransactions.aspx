<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="UserTransactions.aspx.vb" Inherits="UserTransactions" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <link rel="stylesheet" href="Stylesheet/pure.css">
    <script src="Scripts/alertify.js"></script>
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
    <link href="Stylesheet/forms.css" rel="stylesheet" />
    <style scoped>
        .button-success,
        .button-error,
        .button-warning,
        .button-secondary {
            color: white;
            border-radius: 4px;
            text-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
        }

        .button-success {
            background: rgb(28, 184, 65); /* this is a green */
        }

        .button-error {
            background: rgb(202, 60, 60); /* this is a maroon */
        }

        .button-warning {
            background: rgb(223, 117, 20); /* this is an orange */
        }

        .button-secondary {
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
            background-color:  #272b30;
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
        <!-- Content Wrapper. Contains page content -->
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <div class="content-header">
                <h4>KYC
                </h4>
                <ol class="pull-right">
                    <asp:LinkButton ID="LinkButton1" runat="server">Export Report To Excel</asp:LinkButton>
                </ol>
            </div>
            <div class="alert-danger">
                <asp:Label ID="dberrorlabel" Font-Size="Larger" Font-Bold="true" runat="server" Text=""></asp:Label>
            </div>
            <div class="row" id="contact">
                <div class="col-md-3"></div>
                <div class="col-md-6">
                    <div class="">
                        <!-- /.box-header -->
                        <div class="form-horizontal">
                            <fieldset>
                                <p>Search User Transactions</p>
                                <label>
                                    <div>
                                        <asp:Label ID="Label15" runat="server" Text="Service"></asp:Label>
                                    </div>
                                     <asp:DropDownList runat="server" ID="ddlService"></asp:DropDownList>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label14" runat="server" Text="UserIdentifier"></asp:Label>
                                    </div>
                                   <asp:TextBox runat="server" ID="txtUID"></asp:TextBox>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label1" runat="server" Text="TrackId"></asp:Label>
                                    </div>
                                     <asp:TextBox runat="server" ID="txtTrackId"></asp:TextBox>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label6" runat="server" Text="Mobile"></asp:Label>
                                    </div>
                                     <asp:TextBox runat="server" ID="txtMobile"></asp:TextBox>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label4" runat="server" Text="TransactionType"></asp:Label>
                                    </div>
                                     <asp:TextBox runat="server" ID="txtTransType"></asp:TextBox>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label5" runat="server" Text="TransactionStatus"></asp:Label>
                                    </div>
                                     <asp:TextBox runat="server" ID="txtTransStatus"></asp:TextBox>
                                </label>
                                <div class="label-group input-below">
                                    <label style="visibility: hidden">Push Notificationss</label>
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkStatus" AutoPostBack="false" />
                                        <span class="text-default" style="color: #fff;">Status</span>
                                    </label>
                                </div>
                                <div class="submit-wrapper">
                                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-default btn-sm" Text="Search" />
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>
                <div class="col-md-3"></div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <div class="row">
                        <div class="col-lg-12">
                            <div class="box box-success">
                                <h4 class="box-title">User Transaction Details</h4>
                                <div class="box-body">
                                    <div class="table-responsive">
                                    <asp:GridView ID="GridView1" CssClass="table table-bordered" runat="server" BorderColor="DimGray" BorderStyle="Outset"
                                        BorderWidth="2px"  Font-Bold="True" Font-Size="Smaller" ForeColor="White"
                                        HorizontalAlign="Center" AllowPaging="true" OnPageIndexChanging="OnPaging" PageSize="25" AutoGenerateColumns="false">
                                        <RowStyle HorizontalAlign="Center" Font-Names="Times New Roman" />
                                        <FooterStyle BorderWidth="5px"  />
                                        <PagerStyle HorizontalAlign="Center"  CssClass = "GridPager" />
                                        <HeaderStyle BorderStyle="Dotted" ForeColor="#00C0C0" />
                                        <AlternatingRowStyle BackColor="#666666" BorderColor="#666666" Font-Bold="False"
                                            ForeColor="White" BorderWidth="1px" />
                                         <Columns>
                                        <asp:BoundField DataField="ID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="ID" SortExpression="ID" />
                                        <asp:BoundField DataField="UserIdentifier" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="ID" SortExpression="ID" />
                                           <asp:TemplateField ItemStyle-Width="30px" HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="UserId" ForeColor="SteelBlue" runat="server" Text='<%#Eval("UserIdentifier")%>' OnClick="UserInfo"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="TrackID" HeaderText="TrackID" SortExpression="TrackID" />
                                            <asp:BoundField DataField="ServiceCode" HeaderText="ServiceCode" SortExpression="ServiceCode" />
                                            <asp:BoundField DataField="PaymentChannel" HeaderText="PaymentChannel" SortExpression="PaymentChannel" />
                                            <asp:BoundField DataField="UserMobileNumber" HeaderText="MobileNumber" SortExpression="MobileNumber" />
                                            <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                                            <asp:BoundField DataField="TransactionType" HeaderText="TransactionType" SortExpression="TransactionType" />
                                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                <ItemTemplate><%#IIf(Boolean.Parse(Eval("Status").ToString()), "SUCCESS", "FAIL")%></ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                        </div>
                                </div>
                            </div>
                        </div>
                    </div>

                       <asp:Panel ID="pnlAddEdit" runat="server" CssClass="panel panel-danger modalPopup">
                            <div class="form-horizontal">
                                <p>User details</p>
                                <fieldset>
                                    <label class="hiddencol">
                                        <div>
                                            <asp:Label ID="Label7" runat="server" Text="ID"></asp:Label>
                                        </div>
                                            <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
                                    </label>
                                    <label>
                                        <div>
                                            <asp:Label ID="Label17" runat="server" Text="UserIdentifier"></asp:Label>
                                        </div>
                                        <asp:TextBox runat="server" ID="txtUIDEdit"></asp:TextBox>
                                    </label>
                                    <label>
                                        <div>
                                            <asp:Label ID="Label3" runat="server" Text="Service"></asp:Label>
                                        </div>
                                        <asp:DropDownList runat="server" ID="ddlServiceEdit"></asp:DropDownList>
                                    </label>
                                    <label>
                                        <div>
                                            <asp:Label ID="Label2" runat="server" Text="Rating"></asp:Label>
                                        </div>
                                        <asp:TextBox runat="server" ID="txtRatingEdit"></asp:TextBox>
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
                                            <asp:Button ID="btnDelete" CssClass="button-error pure-button pull-left" runat="server" Text="Delete" OnClick="Delete" />
                                            <div class="pull-right">
                                                <asp:Button ID="btnCancel" CssClass="button-warning pure-button" runat="server" Text="Cancel" OnClientClick="return Hidepopup()" />
                                                <asp:Button ID="btnSave" CssClass="button-success pure-button" runat="server" Text="Save" OnClick="Save" />
                                            </div>
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
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GridView1" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>



