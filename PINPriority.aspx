<%@ Page Title="PIN Priority" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="PINPriority.aspx.vb" Inherits="PINPriority" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
    <div class="wrapper">
        <!-- Content Wrapper. Contains page content -->
        <div class="content-wrapper">

            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <!-- Content Header (Page header) -->
                    <div class="content-header">
                        <h4>PIN Priority Details
                        </h4>
                        <ol class="pull-right">
                            <asp:LinkButton ID="LinkButton1" runat="server">Export Report To Excel</asp:LinkButton>
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
                                            Add PIN Priority
                                        </p>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label1" runat="server" Text="Service"></asp:Label>
                                            </div>
                                            <asp:DropDownList ID="ddlService" runat="server" OnSelectedIndexChanged="ddlService_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label3" runat="server" Text="Denomination"></asp:Label>
                                            </div>
                                            <asp:DropDownList ID="ddlDenomination" runat="server" AutoPostBack="false"></asp:DropDownList>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label2" runat="server" Text="Priority"></asp:Label>
                                            </div>
                                            <asp:DropDownList ID="ddlPriority" runat="server" AutoPostBack="false"></asp:DropDownList>
                                        </label>
                                        <div class="label-group input-below">
                                            <label style="visibility: hidden">Statusdddddddsds</label>
                                            <label>
                                                <asp:CheckBox runat="server" ID="chkStatus" AutoPostBack="false" />
                                                Status
                                            </label>
                                        </div>
                                        <div class="submit-wrapper pull-right">
                                            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-default btn-sm" Text="Add Priority" />
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3"></div>
                    </div>

                    <div class="row">
                        <div class="col-lg-12">
                            <div class="box box-success">
                                <h4 class="box-title">Priority Details</h4>
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
                                            <asp:BoundField DataField="Denomination" ReadOnly="true" HeaderText="Denomination" SortExpression="Denomination" />
                                            <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority" />
                                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                <ItemTemplate><%#IIf(Boolean.Parse(Eval("Status").ToString()), "Active", "Inactive")%></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Status" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="Status" SortExpression="Status" />
                                            <asp:BoundField DataField="CreatedDate" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="CreatedDate" Visible="false" SortExpression="CreatedDate" />
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" ForeColor="SteelBlue" runat="server" Text="Edit" OnClick="Edit"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Panel ID="pnlAddEdit" runat="server" CssClass="panel panel-danger modalPopup">
                                        <div class="form-horizontal">
                                            <p>Edit details</p>
                                            <fieldset>
                                                <label class="hiddencol">
                                                    <div>
                                                        <asp:Label ID="Label7" runat="server" Text="ID"></asp:Label>
                                                    </div>
                                                    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
                                                </label>
                                                <label>
                                                    <asp:Label ID="ServiceVal" runat="server"></asp:Label>
                                                </label>
                                                <label>
                                                    <asp:Label ID="DenominationVal" runat="server"></asp:Label>
                                                </label>
                                                <label>
                                                    <div>
                                                        <asp:Label ID="Label8" runat="server" Text="Priority"></asp:Label>
                                                    </div>
                                                    <asp:DropDownList ID="ddlPriorityEdit" runat="server" AutoPostBack="false">
                                                    </asp:DropDownList>
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
                                                            <asp:Button ID="btnSave" CssClass="button-success pure-button" runat="server" Text="Edit Priority" OnClick="Save" />
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
                        </div>
                    </div>
                </div>
            </div>
     </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="GridView1" />
            <asp:AsyncPostBackTrigger ControlID="ddlDenomination" />
        </Triggers>
    </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>



