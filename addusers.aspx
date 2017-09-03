<%@ Page Title="Payit Users" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="addusers.aspx.vb" Inherits="addusers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                <h4>User Details
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
                <div class="col-md-6" style="float:inherit">
                    <div class="">
                        <!-- /.box-header -->
                        <div class="form-horizontal">
                            <fieldset>
                                <p>
                                    Add User
                                </p>
                                <label>
                                    <div>
                                        <asp:Label ID="Label3" runat="server" Text="Company"></asp:Label>
                                    </div>
                                    <asp:DropDownList ID="ddlCompany" runat="server"></asp:DropDownList>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label1" runat="server" Text="Username"></asp:Label>
                                    </div>
                                    <asp:TextBox ID="TextUser" runat="server" CssClass="form-control"></asp:TextBox>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label2" runat="server" Text="Password"></asp:Label>
                                    </div>
                                    <asp:TextBox ID="TextPwd" CssClass="form-control" runat="server" />
                                </label>
                                <div class="submit-wrapper pull-right">
                                    <asp:Button ID="btnSearch" CssClass="btn btn-default btn-sm" runat="server" Text="Add" />

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
                                <h4 class="box-title">User Details</h4>
                                <div class="box-body">
                                        <%--<asp:Label ID="Label17" runat="server" Font-Bold="True" ForeColor="#33CCCC" Font-Size="Smaller"></asp:Label>
                                        <asp:Label ID="Label19" runat="server" Font-Bold="True" Font-Size="Smaller" ForeColor="#33CCCC"></asp:Label>
                                        <asp:Label ID="Label20" runat="server" Font-Bold="True" Font-Size="Smaller" ForeColor="#33CCCC"></asp:Label>--%>
                                       
                                        <asp:GridView  ID="GridView1" runat="server"  BorderColor="DimGray" BorderStyle="Outset"
                                        BorderWidth="2px" CellPadding="0" Font-Bold="True" Font-Size="Smaller" ForeColor="White"
                                        HorizontalAlign="Center" AllowPaging="True"  PageSize="25" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="myid" DataSourceID="PayitUser" EnableModelValidation="True">
                                        <RowStyle HorizontalAlign="Center" Font-Names="Times New Roman" />
                                            <PagerStyle HorizontalAlign="Center" CssClass = "GridPager" />
                                            <Columns>
                                                <asp:CommandField ShowEditButton="True" ControlStyle-ForeColor="#0066ff" />
                                                <asp:BoundField DataField="myid" HeaderText="myid" InsertVisible="False" ReadOnly="True" SortExpression="myid" />
                                                <asp:BoundField DataField="company" HeaderText="company" SortExpression="company" />
                                                <asp:BoundField DataField="auser" HeaderText="auser" SortExpression="auser" />
                                                <asp:BoundField DataField="apass" HeaderText="apass" SortExpression="apass" />
                                                <asp:BoundField DataField="mob" HeaderText="mob" SortExpression="mob" />
                                                <asp:BoundField DataField="acc_type" HeaderText="acc_type" SortExpression="acc_type" />
                                                <asp:BoundField DataField="acc_status" HeaderText="acc_status" SortExpression="acc_status" />
                                                <asp:BoundField DataField="dates" HeaderText="dates" SortExpression="dates" />
                                                <asp:BoundField DataField="times" HeaderText="times" SortExpression="times" />
                                                <asp:CommandField ShowDeleteButton="True"  ControlStyle-ForeColor="#0066ff" />
                                            </Columns>
                                        <FooterStyle BorderWidth="5px" />
                                        <PagerStyle Font-Size="Larger" HorizontalAlign="Center" />
                                        <HeaderStyle BorderStyle="Dotted" ForeColor="#00C0C0" />
                                        <AlternatingRowStyle BackColor="#666666" BorderColor="#666666" Font-Bold="False"
                                            ForeColor="White" BorderWidth="1px" />
                                    </asp:GridView>
                                        <asp:SqlDataSource ID="PayitUser" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:payitConnectionActive %>" DeleteCommand="DELETE FROM [users] WHERE [myid] = @original_myid AND (([company] = @original_company) OR ([company] IS NULL AND @original_company IS NULL)) AND (([auser] = @original_auser) OR ([auser] IS NULL AND @original_auser IS NULL)) AND (([apass] = @original_apass) OR ([apass] IS NULL AND @original_apass IS NULL)) AND (([mob] = @original_mob) OR ([mob] IS NULL AND @original_mob IS NULL)) AND (([acc_type] = @original_acc_type) OR ([acc_type] IS NULL AND @original_acc_type IS NULL)) AND (([acc_status] = @original_acc_status) OR ([acc_status] IS NULL AND @original_acc_status IS NULL)) AND (([dates] = @original_dates) OR ([dates] IS NULL AND @original_dates IS NULL)) AND (([times] = @original_times) OR ([times] IS NULL AND @original_times IS NULL))" InsertCommand="INSERT INTO [users] ([company], [auser], [apass], [mob], [acc_type], [acc_status], [dates], [times]) VALUES (@company, @auser, @apass, @mob, @acc_type, @acc_status, @dates, @times)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [users] ORDER BY [myid]" UpdateCommand="UPDATE [users] SET [company] = @company, [auser] = @auser, [apass] = @apass, [mob] = @mob, [acc_type] = @acc_type, [acc_status] = @acc_status, [dates] = @dates, [times] = @times WHERE [myid] = @original_myid AND (([company] = @original_company) OR ([company] IS NULL AND @original_company IS NULL)) AND (([auser] = @original_auser) OR ([auser] IS NULL AND @original_auser IS NULL)) AND (([apass] = @original_apass) OR ([apass] IS NULL AND @original_apass IS NULL)) AND (([mob] = @original_mob) OR ([mob] IS NULL AND @original_mob IS NULL)) AND (([acc_type] = @original_acc_type) OR ([acc_type] IS NULL AND @original_acc_type IS NULL)) AND (([acc_status] = @original_acc_status) OR ([acc_status] IS NULL AND @original_acc_status IS NULL)) AND (([dates] = @original_dates) OR ([dates] IS NULL AND @original_dates IS NULL)) AND (([times] = @original_times) OR ([times] IS NULL AND @original_times IS NULL))">
                                            <DeleteParameters>
                                                <asp:Parameter Name="original_myid" Type="Int32" />
                                                <asp:Parameter Name="original_company" Type="String" />
                                                <asp:Parameter Name="original_auser" Type="String" />
                                                <asp:Parameter Name="original_apass" Type="String" />
                                                <asp:Parameter Name="original_mob" Type="String" />
                                                <asp:Parameter Name="original_acc_type" Type="String" />
                                                <asp:Parameter Name="original_acc_status" Type="String" />
                                                <asp:Parameter Name="original_dates" Type="String" />
                                                <asp:Parameter Name="original_times" Type="String" />
                                            </DeleteParameters>
                                            <InsertParameters>
                                                <asp:Parameter Name="company" Type="String" />
                                                <asp:Parameter Name="auser" Type="String" />
                                                <asp:Parameter Name="apass" Type="String" />
                                                <asp:Parameter Name="mob" Type="String" />
                                                <asp:Parameter Name="acc_type" Type="String" />
                                                <asp:Parameter Name="acc_status" Type="String" />
                                                <asp:Parameter Name="dates" Type="String" />
                                                <asp:Parameter Name="times" Type="String" />
                                            </InsertParameters>
                                            <UpdateParameters>
                                                <asp:Parameter Name="company" Type="String" />
                                                <asp:Parameter Name="auser" Type="String" />
                                                <asp:Parameter Name="apass" Type="String" />
                                                <asp:Parameter Name="mob" Type="String" />
                                                <asp:Parameter Name="acc_type" Type="String" />
                                                <asp:Parameter Name="acc_status" Type="String" />
                                                <asp:Parameter Name="dates" Type="String" />
                                                <asp:Parameter Name="times" Type="String" />
                                                <asp:Parameter Name="original_myid" Type="Int32" />
                                                <asp:Parameter Name="original_company" Type="String" />
                                                <asp:Parameter Name="original_auser" Type="String" />
                                                <asp:Parameter Name="original_apass" Type="String" />
                                                <asp:Parameter Name="original_mob" Type="String" />
                                                <asp:Parameter Name="original_acc_type" Type="String" />
                                                <asp:Parameter Name="original_acc_status" Type="String" />
                                                <asp:Parameter Name="original_dates" Type="String" />
                                                <asp:Parameter Name="original_times" Type="String" />
                                            </UpdateParameters>
                                        </asp:SqlDataSource>
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


