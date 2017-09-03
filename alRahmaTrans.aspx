<%@ Page Title="Charity Transactions" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="alRahmaTrans.aspx.vb" Inherits="alRahmaTrans" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <script src="Scripts/alertify.js"></script>
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
    <link rel="stylesheet" href="Stylesheet/pure.css">
    <link href="Stylesheet/forms.css" rel="stylesheet" />
        <!-- Content Wrapper. Contains page content -->
       <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
            <!-- Content Header (Page header) -->
    <div class="wrapper">
        <!-- Content Wrapper. Contains page content -->
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <div class="content-header">
                <h4>Charity Project Transactions
                </h4>
                <ol class="pull-right">
                    <asp:LinkButton ID="LinkButton1" runat="server">Export Report To Excel</asp:LinkButton>
                   
                </ol>
            </div>
            <div class="alert-danger">
                <asp:Label ID="dberrorlabel" Font-Size="Larger" Font-Bold="true" runat="server" Text=""></asp:Label>
            </div>
              <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
            <div class="row" id="contact">
                <%--<div class="col-md-3">
                    <asp:Button ID="Button2" runat="server" CssClass="btn btn-primary btn-xs" Text="Refresh" /></div>--%>
                <!-- ./col -->
                <div class="col-md-6" style="float:inherit">
                    <div class="">
                        <!-- /.box-header -->
                        <div class="form-horizontal">
                            <fieldset>
                                <p>
                                    Search Charity Projects
                                </p>
                                <label>
                                    <div>
                                        <asp:Label ID="Label3" runat="server" Text="Service Name"></asp:Label>
                                    </div>
                                    <asp:DropDownList ID="ddlServiceName" OnSelectedIndexChanged="ddlServiceName_SelectedIndexChanged" runat="server" AutoPostBack="True">
                                    </asp:DropDownList>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label1" runat="server" Text="Project Name"></asp:Label>
                                    </div>
                                    <asp:DropDownList ID="ddlAlRahma" runat="server"  OnSelectedIndexChanged ="ddlAlRahma_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:DropDownList>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label4" runat="server" Text="Sub Project Name"></asp:Label>
                                    </div>
                                    <asp:DropDownList ID="dddlSubProject"  runat="server" AutoPostBack="False">
                                    </asp:DropDownList>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label2" runat="server" Text="Track ID"></asp:Label>
                                    </div>
                                    <asp:TextBox ID="TextID" CssClass="form-control" runat="server" />
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="LabelMob" runat="server" Text="Mobile No"></asp:Label>
                                    </div>
                                    <asp:TextBox ID="TextMobile" CssClass="form-control" runat="server" />
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
                                <h4 class="box-title" runat="server" id="heading">Charity Project Details</h4>
                                <div class="box-body">
                                        <%--<asp:Label ID="Label17" runat="server" Font-Bold="True" ForeColor="#33CCCC" Font-Size="Smaller"></asp:Label>
                                        <asp:Label ID="Label19" runat="server" Font-Bold="True" Font-Size="Smaller" ForeColor="#33CCCC"></asp:Label>
                                        <asp:Label ID="Label20" runat="server" Font-Bold="True" Font-Size="Smaller" ForeColor="#33CCCC"></asp:Label>--%>
                                        <asp:Label ID="Label17" runat="server" CssClass="label label-success" Font-Bold="True" Font-Size="Small"></asp:Label>
                                        <asp:Label ID="Label19" runat="server" CssClass="label label-warning" Font-Bold="True" Font-Size="Small"></asp:Label>
                                        <asp:Label ID="Label20" runat="server" CssClass="label label-warning" Font-Bold="True" Font-Size="Small"></asp:Label>
                                    <asp:GridView ID="GridView1" runat="server"
                                        EmptyDataText="No Data" CssClass="table table-bordered table-responsive" AutoGenerateColumns="false" BackColor="#2b2028" HeaderStyle-ForeColor="White" HorizontalAlign="Center"
                                        PageSize="20" AllowPaging="true" OnPageIndexChanging="OnPaging" ForeColor="White" >
                                        <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                        <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                        <HeaderStyle BackColor="Gray" Font-Bold="True" HorizontalAlign="Left" />
                                        <RowStyle HorizontalAlign="Left" />
                                        <EmptyDataTemplate>
                                            <div class="spinner">
                                                <div class="double-bounce1"></div>
                                                <div class="double-bounce2"></div>
                                            </div>
                                            <div style="text-align: center">No Data Found.<asp:LinkButton ID="lnkTry" ForeColor="SteelBlue" runat="server" Text="Try Again" OnClick="TryAgain"></asp:LinkButton>
                                            </div>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:BoundField DataField="Service" HeaderText="ServiceName" SortExpression="Service" />
                                            <asp:BoundField DataField="ProjectName" HeaderText="ProjectName" SortExpression="ProjectID" />
                                            <asp:BoundField DataField="SubprojectName" HeaderText="SubprojectName" SortExpression="SubprojectName" />
                                            <asp:BoundField DataField="MobileNumber" HeaderText="MobileNumber" SortExpression="MobileNumber" />
                                            <asp:BoundField DataField="EmailID" HeaderText="EmailID" SortExpression="EmailID" />
                                            <%--<asp:TemplateField HeaderText="Amount">
                                             <ItemTemplate>
                                                 <asp:Label ID="lblPriority" runat="server" Text='<%# Eval("Amount").ToString()%>'>
                                                 </asp:Label>
                                             </ItemTemplate>
                                             <FooterTemplate>
                                                 <asp:Label ID="lblTotal" runat="server"></asp:Label>
                                                 </FooterTemplate>
                                         </asp:TemplateField>--%>
                                            <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                                            <asp:BoundField DataField="CreatedDate" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="CreatedDate" />
                                          <%--  <asp:BoundField DataField="TransactionStatus" HeaderText="TransactionStatus" SortExpression="TransactionStatus" />--%>
                                            <asp:BoundField DataField="TrackID" HeaderText="TrackID" SortExpression="TrackID" />
                                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                <ItemTemplate><%#IIf(Boolean.Parse(Eval("Status").ToString()), "Success", "Fail")%></ItemTemplate>
                                            </asp:TemplateField>
                                             
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




