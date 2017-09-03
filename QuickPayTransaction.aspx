<%@ Page Title="OgMoneyKW:QuickPay" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="QuickPayTransaction.aspx.vb" Inherits="QuickPayTransaction" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
   
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
    <link rel="stylesheet" href="Stylesheet/pure.css">
    <link href="Stylesheet/forms.css" rel="stylesheet" />
    <!-- Content Wrapper. Contains page content -->
    <style>
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

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!-- Content Header (Page header) -->
            <div class="wrapper">
                <!-- Content Wrapper. Contains page content -->
                <div class="content-wrapper">
                    <!-- Content Header (Page header) -->
                    <div class="content-header">
                        <h4>QuickPay Transactions
                        </h4>
                        <ol class="pull-right">
                            <asp:LinkButton ID="LinkButton1" runat="server">Export Report To Excel</asp:LinkButton>
                        </ol>
                    </div>
                    <div class="alert-danger">
                        <asp:Label ID="dberrorlabel" Font-Size="Larger" Font-Bold="true" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="row" id="contact">

                        <!-- ./col -->
                        <div class="col-md-6" style="float: inherit">
                            <div class="">
                                <!-- /.box-header -->
                                <div class="form-horizontal">
                                    <fieldset>
                                        <p>
                                            Search Transactions
                                        </p>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label4" runat="server" Text="Track ID"></asp:Label>
                                            </div>
                                            <asp:TextBox ID="txtTrackid" CssClass="form-control" runat="server" />
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="LabelMob" runat="server" Text="Mobile No."></asp:Label>
                                            </div>
                                            <asp:TextBox ID="txtMobile" CssClass="form-control" runat="server" />
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="LabelAmount" runat="server" Text="Amount"></asp:Label>
                                            </div>
                                            <asp:TextBox ID="txtAmount" CssClass="form-control" runat="server" />
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="FromDateLabel" runat="server" Text="From Date:"></asp:Label>
                                            </div>
                                            <asp:Label ID="Label14" runat="server" CssClass="label label-primary pull-right"></asp:Label>
                                            <asp:TextBox ID="FromDateTextBox" runat="server"></asp:TextBox>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="ToDateLabel" runat="server" Text="To Date:"></asp:Label>
                                            </div>
                                            <asp:Label ID="Label15" runat="server" CssClass="label label-primary pull-right"></asp:Label>
                                            <asp:TextBox ID="ToDateTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                                        </label>
                                        <div class="label-group input-below">
                                            <label style="visibility: hidden">Statusdddddddsds</label>
                                            <label>
                                                <asp:CheckBox runat="server" ID="chkStatus" Checked="false" AutoPostBack="false" />
                                                <asp:Label runat="server" ID="lblPaging" Text="Disable Paging"></asp:Label>
                                            </label>
                                        </div>
                                        <div class="label-group input-below">
                                            <label style="visibility: hidden">Statusdddddddsds</label>
                                            <label>
                                                <asp:CheckBox runat="server" ID="chkSuccess" Checked="false" AutoPostBack="false" />
                                                <asp:Label runat="server" ID="Label1" Text="All Transactions"></asp:Label>
                                            </label>
                                        </div>
                                        <div class="submit-wrapper pull-right">
                                            <asp:Button ID="btnSearch" CssClass="btn btn-default btn-sm" runat="server" Text="Search" />

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
                                <h4 class="box-title" runat="server" id="heading">Transaction Details</h4>
                                <div class="box-body">
                                    <asp:Label ID="Label17" runat="server" CssClass="label label-success" Font-Bold="True" Font-Size="Small"></asp:Label>
                                    <asp:Label ID="Label18" runat="server" CssClass="label label-warning" Font-Bold="True" Font-Size="Small"></asp:Label>
                                    <asp:Label ID="Label19" runat="server" CssClass="label label-success" Font-Bold="True" Font-Size="Small"></asp:Label>
                                    <asp:Label ID="Label20" runat="server" CssClass="label label-warning" Font-Bold="True" Font-Size="Small"></asp:Label>
                                    <asp:GridView ID="GridView1" runat="server"  OnRowDataBound="OnRowDataBound"
                                        EmptyDataText="No Transactions Found" CssClass="table table-bordered table-responsive" AutoGenerateColumns="false" BackColor="#2b2028" HeaderStyle-ForeColor="White" HorizontalAlign="Center"
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
                                                No Transactions Found.<asp:LinkButton ID="lnkTry" ForeColor="SteelBlue" runat="server" Text="Try Again" OnClick="TryAgain"></asp:LinkButton>
                                            </div>
                                        </EmptyDataTemplate>

                                        <Columns>
                                            <asp:BoundField DataField="ID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="ID" SortExpression="ID" />
                                            <asp:BoundField DataField="TrackID" HeaderText="TrackID" SortExpression="TrackID" />
                                            <asp:BoundField DataField="MobileNo" HeaderText="MobileNo" SortExpression="MobileNo" />
                                            <asp:BoundField DataField="Ptype" HeaderText="Payment" SortExpression="Ptype" />
                                            <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                                            <asp:BoundField DataField="Commission" HeaderText="Commission" SortExpression="Commission" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="Total" ControlStyle-ForeColor="#7297ff">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" CommandName="ContentType" Text='<%#Eval("TotalCount")%>' value='<%#Eval("TotalCount")%>' CommandArgument='<%# Eval("TrackID") %>'
                                                        ID="lnkTotal" OnClick="TotalLinkButton_Click" />
                                                </ItemTemplate>
                                                <ItemStyle ForeColor="#74ff72" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="Success" ControlStyle-ForeColor="#74ff72">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" CommandName="ContentType" Text='<%#Eval("SuccessCount")%>' value='<%#Eval("SuccessCount")%>' CommandArgument='<%# Eval("TrackID") %>'
                                                        ID="lnkSuccess" OnClick="SuccessLinkButton_Click" />
                                                </ItemTemplate>
                                                <ItemStyle ForeColor="#74ff72" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="Fail" ControlStyle-ForeColor="#e3ff72">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" CommandName="ContentType" Text='<%#Eval("FailedCount")%>' value='<%#Eval("FailedCount")%>' CommandArgument='<%# Eval("TrackID") %>'
                                                        ID="lnkFail" OnClick="FailLinkButton_Click" />
                                                </ItemTemplate>
                                                <ItemStyle ForeColor="#e3ff72" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CreatedOn" HeaderText="TransactionDate" SortExpression="CreatedOn" />
                                        </Columns>
                                    </asp:GridView>
                                   <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
                                    <cc1:ModalPopupExtender ID="LinkButton1_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" Enabled="True" TargetControlID="lnkDummy" PopupControlID="Panel1"></cc1:ModalPopupExtender>
                                    <asp:Panel ID="Panel1" CssClass="table table-condensed table-responsive" Width="75%" runat="server">
                                        <br />
                                        <br />
                                        <center>
                                            <div class="well">
                                                <h4 id="panelHeading" runat="server">Transaction Details</h4>
                                                <asp:HiddenField ID="Label6" runat="server"></asp:HiddenField>
                                                <asp:GridView ID="GridView2" EmptyDataText="No Transactions Found" CssClass="table table-bordered" BorderStyle="None"
                                                     CellPadding="0" runat="server" AutoGenerateColumns="true">
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-Width="30px" HeaderText="Action" ControlStyle-ForeColor="#74ff72">
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" CommandName="ContentType" Text="Reprocess" CommandArgument='<%# Eval("TrackID") %>'
                                                                    ID="lmkReprocess" OnClick="ReprocessLinkButton_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle ForeColor="#74ff72" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:Button ID="Button3" runat="server" CssClass="btn btn-default btn-md" Text="Close" />
                                            </div>
                                        </center>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="GridView1" />
        </Triggers>
    </asp:UpdatePanel>
    <script src="Scripts/bootstrap-datepicker.js" type="text/javascript"></script>
    <script src="Scripts/alertify.js" type="text/javascript"></script>
    <script type="text/javascript">
        function pageLoad() {
            $("#ctl00_ContentPlaceHolder1_FromDateTextBox").unbind();
            $('#ctl00_ContentPlaceHolder1_FromDateTextBox').datepicker({
                format: "yyyy-mm-dd",
                todayBtn: "linked",
                todayHighlight: true,
                autoclose: true,
                multidate: false
            });
            $("#ctl00_ContentPlaceHolder1_ToDateTextBox").unbind();
            $('#ctl00_ContentPlaceHolder1_ToDateTextBox').datepicker({
                format: "yyyy-mm-dd",
                todayBtn: "linked",
                autoclose: true,
                todayHighlight: true
            });
        }
    </script>
</asp:Content>



