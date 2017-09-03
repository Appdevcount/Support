<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage4.master" AutoEventWireup="false" CodeFile="zakatSubprojects.aspx.vb" Inherits="zakatSubprojects" %>

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
                <h4>Zakat Sub Projects Details
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
                                    Add Zakat Sub Projects Details
                                </p>
                                 <label>
                                    <div>
                                        <asp:Label ID="Label4" runat="server" Text="ZakatProjectName"></asp:Label>
                                    </div>
                                     <asp:DropDownList runat="server" ID="ddlProjectName" AutoPostBack="false"></asp:DropDownList>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label1" runat="server" Text="SubProjectName"></asp:Label>
                                    </div>
                                    <asp:TextBox ID="zakSubName" runat="server"></asp:TextBox>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label3" runat="server" Text="Description"></asp:Label>
                                    </div>
                                    <asp:TextBox ID="zakSubDesc" runat="server"></asp:TextBox>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label2" runat="server" Text="Priority"></asp:Label>
                                    </div>
                                    <asp:TextBox ID="zakSubPriority" runat="server"></asp:TextBox>
                                </label>
                                <div class="label-group input-below">
                                    <label style="visibility: hidden">Statusdddddddsds</label>
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkStatus" AutoPostBack="false" />
                                        Status
                                    </label>
                                </div>
                                <div class="submit-wrapper pull-right">
                                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-default btn-sm" Text="Add Zakat Sub Project" />
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
                        <h4 class="box-title">Zakat Sub Projects Details</h4>
                        <div class="box-body">

                            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" PageSize="25" 
                                AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="SqlDataSource1" EnableModelValidation="True">
                                <RowStyle HorizontalAlign="Center" Font-Names="Times New Roman" />
                                <FooterStyle BorderWidth="5px" />
                                <PagerStyle HorizontalAlign="Center" CssClass = "GridPager" />
                                <HeaderStyle BorderStyle="Dotted" ForeColor="#00C0C0" />
                                <AlternatingRowStyle BackColor="#666666" BorderColor="#666666" Font-Bold="False"
                                    ForeColor="White" BorderWidth="1px" />
                              <Columns>
                                   <%-- <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
                                    <asp:BoundField DataField="ProjectType" HeaderText="ProjectType" SortExpression="ProjectType" />--%>
                                    <asp:BoundField DataField="ProjectID" HeaderText="ProjectID" SortExpression="ProjectID" />
                                    <asp:BoundField DataField="SubprojectName" HeaderText="SubprojectName" SortExpression="SubprojectName" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                                    <asp:CheckBoxField DataField="Status" HeaderText="Status" SortExpression="Status" />
                                    <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority" />
                                    <asp:CommandField ShowEditButton="True" />
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:payitConnectionString2 %>" DeleteCommand="DELETE FROM [CharitySubProjects] WHERE [ID] = @original_ID AND (([ProjectType] = @original_ProjectType) OR ([ProjectType] IS NULL AND @original_ProjectType IS NULL)) AND (([ProjectID] = @original_ProjectID) OR ([ProjectID] IS NULL AND @original_ProjectID IS NULL)) AND (([SubprojectName] = @original_SubprojectName) OR ([SubprojectName] IS NULL AND @original_SubprojectName IS NULL)) AND (([Description] = @original_Description) OR ([Description] IS NULL AND @original_Description IS NULL)) AND (([Status] = @original_Status) OR ([Status] IS NULL AND @original_Status IS NULL)) AND (([Priority] = @original_Priority) OR ([Priority] IS NULL AND @original_Priority IS NULL))" InsertCommand="INSERT INTO [CharitySubProjects] ([ProjectType], [ProjectID], [SubprojectName], [Description], [Status], [Priority]) VALUES (@ProjectType, @ProjectID, @SubprojectName, @Description, @Status, @Priority)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [CharitySubProjects]" UpdateCommand="UPDATE [CharitySubProjects] SET [ProjectType] = @ProjectType, [ProjectID] = @ProjectID, [SubprojectName] = @SubprojectName, [Description] = @Description, [Status] = @Status, [Priority] = @Priority WHERE [ID] = @original_ID">
                                <DeleteParameters>
                                    <asp:Parameter Name="original_ID" Type="Int32" />
                                    <asp:Parameter Name="original_ProjectType" Type="Int32" />
                                    <asp:Parameter Name="original_ProjectID" Type="Int32" />
                                    <asp:Parameter Name="original_SubprojectName" Type="String" />
                                    <asp:Parameter Name="original_Description" Type="String" />
                                    <asp:Parameter Name="original_Status" Type="Boolean" />
                                    <asp:Parameter Name="original_Priority" Type="Int32" />
                                </DeleteParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="ProjectType" Type="Int32" />
                                    <asp:Parameter Name="ProjectID" Type="Int32" />
                                    <asp:Parameter Name="SubprojectName" Type="String" />
                                    <asp:Parameter Name="Description" Type="String" />
                                    <asp:Parameter Name="Status" Type="Boolean" />
                                    <asp:Parameter Name="Priority" Type="Int32" />
                                </InsertParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="ProjectType" Type="Int32" />
                                    <asp:Parameter Name="ProjectID" Type="Int32" />
                                    <asp:Parameter Name="SubprojectName" Type="String" />
                                    <asp:Parameter Name="Description" Type="String" />
                                    <asp:Parameter Name="Status" Type="Boolean" />
                                    <asp:Parameter Name="Priority" Type="Int32" />
                                    <asp:Parameter Name="original_ID" Type="Int32" />
                                    <asp:Parameter Name="original_ProjectType" Type="Int32" />
                                    <asp:Parameter Name="original_ProjectID" Type="Int32" />
                                    <asp:Parameter Name="original_SubprojectName" Type="String" />
                                    <asp:Parameter Name="original_Description" Type="String" />
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


