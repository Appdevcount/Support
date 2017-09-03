﻿<%@ Page Title="Zakat Summary" Language="VB" MasterPageFile="~/MasterPage4.master" AutoEventWireup="false" CodeFile="zakatSummary.aspx.vb" Inherits="zakatSummary" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <link rel="stylesheet" href="Stylesheet/pure.css">
    <link href="Stylesheet/forms.css" rel="stylesheet" />
    <div class="wrapper">
         <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <!-- Content Wrapper. Contains page content -->
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <div class="content-header">
                <h4>Zakat Summary Details
                </h4>
                <ol class="pull-right">
                    <asp:LinkButton ID="LinkButton1" runat="server">Export Report To Excel</asp:LinkButton>
                     <div class="label-group input-below">
                        <label style="visibility: hidden">Statusdddddddsd</label>
                        <label>
                            <%--<asp:CheckBox runat="server" ID="chkStatus" OnCheckedChanged="chkStatus_CheckedChanged" Checked="true" AutoPostBack="true" />
                            <asp:Label runat="server" ID="lblPaging" Text="Disable Paging"></asp:Label>--%>
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
                                    Search Zakat Projects
                                </p>
                                <label>
                                    <div>
                                        <asp:Label ID="Label1" runat="server" Text="Zakat Project"></asp:Label>
                                    </div>
                                    <asp:DropDownList ID="ddlServiceName" runat="server"></asp:DropDownList>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="FromDateLabel" runat="server" Text="From Date:"></asp:Label>
                                    </div>
                                    <asp:Label ID="Label2" runat="server" CssClass="label label-primary pull-right"></asp:Label>
                                    <asp:TextBox ID="FromDateTextBox" runat="server"></asp:TextBox>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="ToDateLabel" runat="server" Text="To Date:"></asp:Label>
                                    </div>
                                    <asp:Label ID="Label3" runat="server" CssClass="label label-primary pull-right"></asp:Label>
                                    <asp:TextBox ID="ToDateTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                                </label>
                                <div class="submit-wrapper pull-right">
                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-default btn-sm" Text="Search" />
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
                        <h4 class="box-title">Zakat Details</h4>
                        <div class="box-body">
                            <asp:Label ID="Label17" runat="server" CssClass="label label-success" Font-Bold="True" Font-Size="Small"></asp:Label><br />
                            <asp:Label ID="Label4" runat="server" CssClass="label label-success" Font-Bold="True" Font-Size="Small"></asp:Label>
                                        <asp:Label ID="Label5" runat="server" CssClass="label label-warning" Font-Bold="True" Font-Size="Small"></asp:Label>
                                        <asp:Label ID="Label6" runat="server" CssClass="label label-warning" Font-Bold="True" Font-Size="Small"></asp:Label>
                                        <asp:Label ID="Label19" runat="server" CssClass="label label-warning" Font-Bold="True" Font-Size="Small"></asp:Label>
                                        <asp:Label ID="Label20" runat="server" CssClass="label label-warning" Font-Bold="True" Font-Size="Small"></asp:Label>
                            <asp:GridView  ID="GridView1" runat="server" CssClass="table table-bordered table-responsive" HeaderStyle-ForeColor="White"
                                         BorderStyle="None" CellPadding="0" HorizontalAlign="Center" AllowPaging="True" OnPageIndexChanging="OnPaging" OnRowDataBound="GridView1_RowDataBound" PageSize="25">
                                        <PagerStyle HorizontalAlign="Center" CssClass = "GridPager" />
                                        <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                        <HeaderStyle BackColor="Gray" Font-Bold="True" HorizontalAlign="Left" />
                                        <RowStyle HorizontalAlign="Left" />
                                        <Columns>
                                            <%--<asp:BoundField DataField="Serial" HeaderText="Serial" SortExpression="Serial" />
                                            <asp:BoundField DataField="DecryptedPIN" HeaderText="DecryptedPIN" SortExpression="DecryptedPIN" />
                                            <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                                            <asp:BoundField DataField="Service" HeaderText="Service" SortExpression="Service" />
                                            <asp:BoundField DataField="TranDate" HeaderText="TransactionDate" SortExpression="TranDate" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                                           <asp:TemplateField HeaderText="Amount">
                                             <ItemTemplate>
                                                 <asp:Label ID="lblPriority" runat="server" Text='<%# Eval("Amount").ToString()%>'>
                                                 </asp:Label>
                                             </ItemTemplate>
                                             <FooterTemplate>
                                                 <asp:Label ID="lblTotal" runat="server"></asp:Label>
                                                 </FooterTemplate>
                                         </asp:TemplateField>--%>
                                            <%--<asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />--%>
                                            
                                           <%-- <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                <ItemTemplate><%#IIf(Boolean.Parse(Eval("Status").ToString()), "Active", "Inactive")%></ItemTemplate>
                                            </asp:TemplateField>--%>
                                        </Columns>
                                    </asp:GridView>
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
    <script src="Scripts/bootstrap-datepicker.js"></script>
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




