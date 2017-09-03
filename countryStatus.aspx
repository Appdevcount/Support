<%@ Page Title="TopUp Countries" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="countryStatus.aspx.vb" Inherits="countryStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <link rel="stylesheet" href="Stylesheet/pure.css">
    <link href="Stylesheet/forms.css" rel="stylesheet" />
    <div class="wrapper">
        <!-- Content Wrapper. Contains page content -->
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <div class="content-header">
                <h4>TopUp Country Details
                </h4>
                <ol class="pull-right">
                    <asp:LinkButton ID="LinkButton1" runat="server">Export Report To Excel</asp:LinkButton>
                </ol>
            </div>
            <div class="alert-danger">
                <asp:Label ID="dberrorlabel" Font-Size="Larger" Font-Bold="true" runat="server" Text=""></asp:Label>
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <div class="box box-success">
                       
                        <div class="box-body">

                            <asp:GridView ID="GridView1" runat="server" CssClass="table table-condensed" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="SqlTopUpCountries"
                                BorderColor="DimGray" BorderStyle="Outset"
                                    BorderWidth="2px" CellPadding="0" Font-Bold="True" Font-Size="Smaller" ForeColor="White"
                                    HorizontalAlign="Center" PageSize="25" EnableModelValidation="True">
                                  <RowStyle HorizontalAlign="Center" Font-Names="Times New Roman" />
                                    <FooterStyle BorderWidth="5px" />
                                    <PagerStyle Font-Size="Larger" HorizontalAlign="Center" />
                                    <HeaderStyle BorderStyle="Dotted" ForeColor="#00C0C0" />
                                    <AlternatingRowStyle BackColor="#666666" BorderColor="#666666" Font-Bold="False"
                                        ForeColor="White" BorderWidth="1px" />
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" Visible="False" />
                                    <asp:BoundField DataField="Country_En" ReadOnly="true" HeaderText="Country_En" SortExpression="Country_En" />
                                    <asp:BoundField DataField="Country_Ar" ReadOnly="true" HeaderText="Country_Ar" SortExpression="Country_Ar" />
                                    <asp:BoundField DataField="Country_Code" ReadOnly="true" HeaderText="Country_Code" SortExpression="Country_Code" />
                                    <asp:CheckBoxField DataField="Status" HeaderText="Status" SortExpression="Status" />
                                   <asp:CommandField HeaderText="Action" ShowEditButton="True" ControlStyle-ForeColor="SteelBlue" />
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlTopUpCountries" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:payitConnectionActive %>" DeleteCommand="DELETE FROM [PayItCountries] WHERE [ID] = @original_ID AND (([Country_En] = @original_Country_En) OR ([Country_En] IS NULL AND @original_Country_En IS NULL)) AND (([Country_Ar] = @original_Country_Ar) OR ([Country_Ar] IS NULL AND @original_Country_Ar IS NULL)) AND (([Country_Code] = @original_Country_Code) OR ([Country_Code] IS NULL AND @original_Country_Code IS NULL)) AND (([Status] = @original_Status) OR ([Status] IS NULL AND @original_Status IS NULL))" InsertCommand="INSERT INTO [PayItCountries] ([Country_En], [Country_Ar], [Country_Code], [Status]) VALUES (@Country_En, @Country_Ar, @Country_Code, @Status)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [Country_En], [Country_Ar], [Country_Code], [Status], [ID] FROM [PayItCountries]" UpdateCommand="UPDATE [PayItCountries] SET [Status] = @Status WHERE [ID] = @original_ID AND (([Status] = @original_Status) OR ([Status] IS NULL AND @original_Status IS NULL))">
                                <DeleteParameters>
                                    <asp:Parameter Name="original_ID" Type="Int32" />
                                    <asp:Parameter Name="original_Country_En" Type="String" />
                                    <asp:Parameter Name="original_Country_Ar" Type="String" />
                                    <asp:Parameter Name="original_Country_Code" Type="Int32" />
                                    <asp:Parameter Name="original_Status" Type="Boolean" />
                                </DeleteParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="Country_En" Type="String" />
                                    <asp:Parameter Name="Country_Ar" Type="String" />
                                    <asp:Parameter Name="Country_Code" Type="Int32" />
                                    <asp:Parameter Name="Status" Type="Boolean" />
                                </InsertParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="Status" Type="Boolean" />
                                    <asp:Parameter Name="original_ID" Type="Int32" />
                                    <asp:Parameter Name="original_Status" Type="Boolean" />
                                </UpdateParameters>
                            </asp:SqlDataSource>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
