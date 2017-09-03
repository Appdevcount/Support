<%@ Page Title="International Countries" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="IntlCountries.aspx.vb" Inherits="IntlCountries" %>
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
                <ol class="pull-right">
                    <asp:LinkButton ID="LinkButton1" runat="server">Export Report To Excel</asp:LinkButton>
                </ol>
            </div>
            <div class="alert-danger">
                <asp:Label ID="dberrorlabel" Font-Size="Larger" Font-Bold="true" runat="server" Text=""></asp:Label>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="box box-success">
                                <h4 class="box-title">Countries Details</h4>
                                <div class="box-body">

                                   <asp:GridView ID="GridView1" runat="server"
                                    EmptyDataText="No Data" CssClass="table table-bordered table-responsive" BackColor="#2b2028" HeaderStyle-ForeColor="White" HorizontalAlign="Center" 
                                    ForeColor="White" AllowPaging="true" DataKeyNames="ID" DataSourceID="SqlIntlCountry" PageSize="25" AutoGenerateColumns="false" >
                                    <PagerStyle HorizontalAlign="Center" CssClass = "GridPager" />
                                    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                    <HeaderStyle BackColor="Gray" Font-Bold="True" HorizontalAlign="Left" />
                                    <RowStyle HorizontalAlign="Left" />
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
                                            <asp:BoundField DataField="Country_En" HeaderText="Country_En" ReadOnly="True" SortExpression="Country_En" />
                                            <asp:BoundField DataField="Country_Code" HeaderText="Country_Code" ReadOnly="True" SortExpression="Country_Code" />
                                            <asp:CheckBoxField DataField="Status" HeaderText="Status" SortExpression="Status" />
                                            <asp:CommandField HeaderText="Action" ControlStyle-ForeColor="SteelBlue" ShowEditButton="True" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlIntlCountry" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:payitConnectionActive %>" DeleteCommand="DELETE FROM [PayItCountries] WHERE [ID] = @original_ID AND (([Country_En] = @original_Country_En) OR ([Country_En] IS NULL AND @original_Country_En IS NULL)) AND (([Country_Code] = @original_Country_Code) OR ([Country_Code] IS NULL AND @original_Country_Code IS NULL)) AND (([Status] = @original_Status) OR ([Status] IS NULL AND @original_Status IS NULL))" InsertCommand="INSERT INTO [PayItCountries] ([Country_En], [Country_Code], [Status]) VALUES (@Country_En, @Country_Code, @Status)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [ID], [Country_En], [Country_Code], [Status] FROM [PayItCountries]" UpdateCommand="UPDATE [PayItCountries] SET  [Status] = @Status WHERE [ID] = @original_ID">
                                        <DeleteParameters>
                                            <asp:Parameter Name="original_ID" Type="Int32" />
                                            <asp:Parameter Name="original_Country_En" Type="String" />
                                            <asp:Parameter Name="original_Country_Code" Type="Int32" />
                                            <asp:Parameter Name="original_Status" Type="Boolean" />
                                        </DeleteParameters>
                                        <InsertParameters>
                                            <asp:Parameter Name="Country_En" Type="String" />
                                            <asp:Parameter Name="Country_Code" Type="Int32" />
                                            <asp:Parameter Name="Status" Type="Boolean" />
                                        </InsertParameters>
                                        <UpdateParameters>
                                            <asp:Parameter Name="Country_En" Type="String" />
                                            <asp:Parameter Name="Country_Code" Type="Int32" />
                                            <asp:Parameter Name="Status" Type="Boolean" />
                                            <asp:Parameter Name="original_ID" Type="Int32" />
                                            <asp:Parameter Name="original_Country_En" Type="String" />
                                            <asp:Parameter Name="original_Country_Code" Type="Int32" />
                                            <asp:Parameter Name="original_Status" Type="Boolean" />
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



