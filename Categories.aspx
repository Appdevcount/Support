<%@ Page Title="Categories" Language="VB" MasterPageFile="~/MasterPage2.master" EnableEventValidation="false" AutoEventWireup="false" CodeFile="Categories.aspx.vb" Inherits="Categories" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="Stylesheet/pure.css">
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <script src="Scripts/alertify.js"></script>
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
    <link href="Stylesheet/forms.css" rel="stylesheet" />
    <style>
        .hide{
            display:none;
            }

            .show{
            display:block;
            }
    </style>
    <script type="text/javascript">
        function showDiv() {
            document.getElementById('contact').style.display = "block";
        }
        function block_none() {
            document.getElementById('contact').classList.add('show');
            document.getElementById('aa').classList.add('hide');
        }

    </script>
    <script type="text/javascript">
    $(document).ready(function() {
        setTimeout(function() {
        $('#<%= dberrorlabel.ClientId%>').hide();
        }, 2000); //
    });
    </script>

    <div class="wrapper">
        <!-- Content Wrapper. Contains page content -->
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <div class="content-header">
                <h3>Category and Services
                </h3>
                <ol class="pull-right">
                    <asp:LinkButton ID="LinkButton1" runat="server">Export Report To Excel</asp:LinkButton>
                </ol>
            </div>
            <div class="">
                <div class="alert-danger">
                    <asp:Label ID="dberrorlabel" Font-Size="Larger" Font-Bold="true" runat="server" Text=""></asp:Label>
                </div>
                <div class="row">
                    <div class="col-md-3"></div>
                    <!-- ./col -->
                    <div class="col-md-6" style="float: right;">
                        <div>
                            <div class="form-horizontal">
                                <fieldset>
                                    <legend>Add Category Details</legend>
                                    <label>
                                        <div>
                                            <asp:Label ID="Label1" runat="server" Text="Category Name"></asp:Label>
                                        </div>
                                        <asp:TextBox ID="catName" runat="server"></asp:TextBox>
                                    </label>
                                    <label>
                                        <div>
                                            <asp:Label ID="Label3" runat="server" Text="Category info"></asp:Label>
                                        </div>
                                        <asp:TextBox ID="catInfo" runat="server"></asp:TextBox>
                                    </label>
                                  <%--  <label>
                                        <div>
                                            <asp:Label ID="Label6" runat="server" Text="Priority"></asp:Label>
                                        </div>
                                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                                    </label>--%>
                                    <div class="label-group input-below">
                                        <label style="visibility: hidden">Category Statussss</label>
                                        <label>
                                            <asp:CheckBox runat="server" ID="chkStatus" AutoPostBack="false" />
                                            <span class="text-default" style="color: #fff">Status</span>
                                        </label>
                                    </div>
                                    <div class="submit-wrapper pull-right">
                                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-default btn-sm" Text="Add Category" />
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
                            <div style="margin-bottom:20px;"></div>
                            <!--<a href="#" id="aa"  onclick="block_none()" class="pure-button pure-button-primary pull-right">Add Category</a>-->
                            <h4 class="box-title">Category Details</h4>
                            <div class="box-body">

                                <asp:GridView ID="GridView1" runat="server" OnRowDataBound="GridView1_RowDataBound" BorderColor="DimGray" BorderStyle="Outset"
                                    BorderWidth="2px" CellPadding="0" Font-Bold="True" Font-Size="Smaller" ForeColor="White"
                                    HorizontalAlign="Center" PageSize="25" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="SqlCatServies" EnableModelValidation="True">
                                    <RowStyle HorizontalAlign="Center" Font-Names="Times New Roman" />
                                    <FooterStyle BorderWidth="5px" />
                                    <HeaderStyle BorderStyle="Dotted" ForeColor="#00C0C0" />
                                    <AlternatingRowStyle BackColor="#666666" BorderColor="#666666" Font-Bold="False"
                                        ForeColor="White" BorderWidth="1px" />
                                    <Columns>
                                        <asp:BoundField DataField="CategoryName" HeaderText="CategoryName" SortExpression="CategoryName" />

                                        <asp:BoundField DataField="CategoryInfo" HeaderText="CategoryInfo" SortExpression="CategoryInfo" />
                                        <%--<asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                    <ItemTemplate><%#IIf(Boolean.Parse(Eval("Status").ToString()), "Active", "Inactive")%></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="textStatus" runat="server" Text='<%# Bind("Status") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="150px" />
                                                </asp:TemplateField>--%>
                                        <asp:CheckBoxField HeaderText="Status"
                                            DataField="Status" />

                                        <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority" />
                                        <asp:CommandField HeaderText="Action" ShowEditButton="True" ControlStyle-ForeColor="SteelBlue" />
                                        <asp:BoundField DataField="ID" Visible="false" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlCatServies" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:payitConnectionActive %>" DeleteCommand="DELETE FROM [PayitCategories] WHERE [ID] = @original_ID AND (([CategoryName] = @original_CategoryName) OR ([CategoryName] IS NULL AND @original_CategoryName IS NULL)) AND (([Status] = @original_Status) OR ([Status] IS NULL AND @original_Status IS NULL)) AND (([CategoryInfo] = @original_CategoryInfo) OR ([CategoryInfo] IS NULL AND @original_CategoryInfo IS NULL)) AND (([Priority] = @original_Priority) OR ([Priority] IS NULL AND @original_Priority IS NULL))" InsertCommand="INSERT INTO [PayitCategories] ([CategoryName], [Status], [CategoryInfo], [Priority]) VALUES (@CategoryName, @Status, @CategoryInfo, @Priority)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [CategoryName], [Status], [CategoryInfo], [Priority], [ID] FROM [PayitCategories] ORDER BY [ID]" UpdateCommand="UPDATE [PayitCategories] SET [CategoryName] = @CategoryName, [Status] = @Status, [CategoryInfo] = @CategoryInfo, [Priority] = @Priority WHERE [ID] = @original_ID AND (([CategoryName] = @original_CategoryName) OR ([CategoryName] IS NULL AND @original_CategoryName IS NULL)) AND (([Status] = @original_Status) OR ([Status] IS NULL AND @original_Status IS NULL)) AND (([CategoryInfo] = @original_CategoryInfo) OR ([CategoryInfo] IS NULL AND @original_CategoryInfo IS NULL)) AND (([Priority] = @original_Priority) OR ([Priority] IS NULL AND @original_Priority IS NULL))">
                                    <DeleteParameters>
                                        <asp:Parameter Name="original_ID" Type="Int32" />
                                        <asp:Parameter Name="original_CategoryName" Type="String" />
                                        <asp:Parameter Name="original_Status" Type="Boolean" />
                                        <asp:Parameter Name="original_CategoryInfo" Type="String" />
                                        <asp:Parameter Name="original_Priority" Type="Int32" />
                                    </DeleteParameters>
                                    <InsertParameters>
                                        <asp:Parameter Name="CategoryName" Type="String" />
                                        <asp:Parameter Name="Status" Type="Boolean" />
                                        <asp:Parameter Name="CategoryInfo" Type="String" />
                                        <asp:Parameter Name="Priority" Type="Int32" />
                                    </InsertParameters>
                                    <UpdateParameters>
                                        <asp:Parameter Name="CategoryName" Type="String" />
                                        <asp:Parameter Name="Status" Type="Boolean" />
                                        <asp:Parameter Name="CategoryInfo" Type="String" />
                                        <asp:Parameter Name="Priority" Type="Int32" />
                                        <asp:Parameter Name="original_ID" Type="Int32" />
                                        <asp:Parameter Name="original_CategoryName" Type="String" />
                                        <asp:Parameter Name="original_Status" Type="Boolean" />
                                        <asp:Parameter Name="original_CategoryInfo" Type="String" />
                                        <asp:Parameter Name="original_Priority" Type="Int32" />
                                    </UpdateParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

