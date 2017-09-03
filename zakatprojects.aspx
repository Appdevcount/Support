<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage4.master" AutoEventWireup="false" CodeFile="zakatprojects.aspx.vb" Inherits="zakatprojects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <script src="Scripts/alertify.js"></script>
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />

    <link rel="stylesheet" href="Stylesheet/pure.css">
    <link href="Stylesheet/forms.css" rel="stylesheet" />
    <div class="wrapper">
        <!-- Content Wrapper. Contains page content -->
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <div class="content-header">
                <h4>Zakat Details
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
                                    Add Zakat Details
                                </p>
                                <label>
                                    <div>
                                        <asp:Label ID="Label1" runat="server" Text="ZakatProjectName"></asp:Label>
                                    </div>
                                    <asp:TextBox ID="zakName" runat="server"></asp:TextBox>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label3" runat="server" Text="Description"></asp:Label>
                                    </div>
                                    <asp:TextBox ID="zakDesc" runat="server"></asp:TextBox>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label2" runat="server" Text="Priority"></asp:Label>
                                    </div>
                                    <asp:TextBox ID="zakPriority" runat="server"></asp:TextBox>
                                </label>
                                <div class="label-group input-below">
                                    <label style="visibility: hidden">Statusdddddddsds</label>
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkStatus" AutoPostBack="false" />
                                        Status
                                    </label>
                                </div>
                                <div class="submit-wrapper pull-right">
                                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-default btn-sm" Text="Add Zakat Project" />
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
                        <h4 class="box-title">Zakat Details</h4>
                        <div class="box-body">

                            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" PageSize="25" 
                                AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="SqlZakat" EnableModelValidation="True">
                                <RowStyle HorizontalAlign="Center" Font-Names="Times New Roman" />
                                <FooterStyle BorderWidth="5px" />
                                <PagerStyle HorizontalAlign="Center" CssClass = "GridPager" />
                                <HeaderStyle BorderStyle="Dotted" ForeColor="#00C0C0" />
                                <AlternatingRowStyle BackColor="#666666" BorderColor="#666666" Font-Bold="False"
                                    ForeColor="White" BorderWidth="1px" />
                                <Columns>
                                    <asp:BoundField Visible="false" DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
                                    <asp:BoundField DataField="ZakatProjectName" HeaderText="ZakatProjectName" SortExpression="ZakatProjectName" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                                    <asp:BoundField DataField="CreatedDate" HeaderText="Date" DataFormatString="{0:d}" SortExpression="CreatedDate" />
                                    <asp:CheckBoxField DataField="Status" HeaderText="Status" SortExpression="Status" />
                                    <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority" />
                                    <asp:CommandField HeaderText="Action" ShowEditButton="True" ControlStyle-ForeColor="SteelBlue" />
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlZakat" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:payitConnectionActive %>" DeleteCommand="DELETE FROM [ZakatProjects] WHERE [ID] = @original_ID AND (([Description] = @original_Description) OR ([Description] IS NULL AND @original_Description IS NULL)) AND (([ZakatProjectName] = @original_ZakatProjectName) OR ([ZakatProjectName] IS NULL AND @original_ZakatProjectName IS NULL)) AND (([CreatedDate] = @original_CreatedDate) OR ([CreatedDate] IS NULL AND @original_CreatedDate IS NULL)) AND (([Status] = @original_Status) OR ([Status] IS NULL AND @original_Status IS NULL)) AND (([Priority] = @original_Priority) OR ([Priority] IS NULL AND @original_Priority IS NULL))" InsertCommand="INSERT INTO [ZakatProjects] ([Description], [ZakatProjectName], [CreatedDate], [Status], [Priority]) VALUES (@Description, @ZakatProjectName, @CreatedDate, @Status, @Priority)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [ID], [Description], [ZakatProjectName], [CreatedDate], [Status], [Priority] FROM [ZakatProjects]" UpdateCommand="UPDATE [ZakatProjects] SET [Description] = @Description, [ZakatProjectName] = @ZakatProjectName, [CreatedDate] = @CreatedDate, [Status] = @Status, [Priority] = @Priority WHERE [ID] = @original_ID AND (([Description] = @original_Description) OR ([Description] IS NULL AND @original_Description IS NULL)) AND (([ZakatProjectName] = @original_ZakatProjectName) OR ([ZakatProjectName] IS NULL AND @original_ZakatProjectName IS NULL)) AND (([CreatedDate] = @original_CreatedDate) OR ([CreatedDate] IS NULL AND @original_CreatedDate IS NULL)) AND (([Status] = @original_Status) OR ([Status] IS NULL AND @original_Status IS NULL)) AND (([Priority] = @original_Priority) OR ([Priority] IS NULL AND @original_Priority IS NULL))">
                                <DeleteParameters>
                                    <asp:Parameter Name="original_ID" Type="Int32" />
                                    <asp:Parameter Name="original_Description" Type="String" />
                                    <asp:Parameter Name="original_ZakatProjectName" Type="String" />
                                    <asp:Parameter Name="original_CreatedDate" Type="DateTime" />
                                    <asp:Parameter Name="original_Status" Type="Boolean" />
                                    <asp:Parameter Name="original_Priority" Type="Int32" />
                                </DeleteParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="Description" Type="String" />
                                    <asp:Parameter Name="ZakatProjectName" Type="String" />
                                    <asp:Parameter Name="CreatedDate" Type="DateTime" />
                                    <asp:Parameter Name="Status" Type="Boolean" />
                                    <asp:Parameter Name="Priority" Type="Int32" />
                                </InsertParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="Description" Type="String" />
                                    <asp:Parameter Name="ZakatProjectName" Type="String" />
                                    <asp:Parameter Name="CreatedDate" Type="DateTime" />
                                    <asp:Parameter Name="Status" Type="Boolean" />
                                    <asp:Parameter Name="Priority" Type="Int32" />
                                    <asp:Parameter Name="original_ID" Type="Int32" />
                                    <asp:Parameter Name="original_Description" Type="String" />
                                    <asp:Parameter Name="original_ZakatProjectName" Type="String" />
                                    <asp:Parameter Name="original_CreatedDate" Type="DateTime" />
                                    <asp:Parameter Name="original_Status" Type="Boolean" />
                                    <asp:Parameter Name="original_Priority" Type="Int32" />
                                </UpdateParameters>
                            </asp:SqlDataSource>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>


