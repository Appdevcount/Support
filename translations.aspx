<%@ Page Title="Translations" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="translations.aspx.vb" Inherits="translations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="Stylesheet/pure.css">
    <link href="Stylesheet/mainstyle.css" rel="Stylesheet" />
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
  <div class="wrapper">
        <!-- Content Wrapper. Contains page content -->
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <div class="content-header">
                <h3>Translations
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
                                    <legend>Add Translation</legend>
                                    <label>
                                        <div>
                                            <asp:Label ID="Label1" runat="server" Text="LanguageCode"></asp:Label>
                                        </div>
                                        <asp:DropDownList ID="ddlLang" runat="server" AutoPostBack="false">
                                    </asp:DropDownList>
                                    </label>
                                    <label>
                                        <div>
                                            <asp:Label ID="Label3" runat="server" Text="Source Text"></asp:Label>
                                        </div>
                                        <asp:TextBox ID="txtSource" runat="server"></asp:TextBox>
                                    </label>
                                    <label>
                                        <div>
                                            <asp:Label ID="Label6" runat="server" Text="Translated Text"></asp:Label>
                                        </div>
                                        <asp:TextBox ID="txtTranslate" runat="server"></asp:TextBox>
                                    </label>
                                    
                                    <div class="submit-wrapper pull-right">
                                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-default btn-sm" Text="Add Translation" />
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
                            <h4 class="box-title">Translation Details</h4>
                            <div class="box-body">

                                <asp:GridView ID="GridView1" runat="server" CssClass="table"  BorderColor="DimGray" BorderStyle="Outset"
                                    BorderWidth="2px" CellPadding="0" Font-Bold="True" Font-Size="Smaller" ForeColor="White" OnDataBound="OnDataBound"
                                    HorizontalAlign="Center" PageSize="25" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataSourceID="SqlTranslate" EnableModelValidation="True" DataKeyNames="ID">
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
                                                No Data Found.
                                            </div>
                                        </EmptyDataTemplate>
                                    <Columns>
                                        <asp:BoundField DataField="LanguageCode" HeaderText="LanguageCode" SortExpression="LanguageCode" />
                                        <asp:BoundField DataField="SourceText" HeaderText="SourceText" SortExpression="SourceText" />
                                        <asp:BoundField DataField="TranslatedText" HeaderText="TranslatedText" SortExpression="TranslatedText" />
                                        <asp:CheckBoxField DataField="Status" HeaderText="Status" SortExpression="Status" />
                                        <asp:BoundField DataField="ID" Visible="false" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
                                        <asp:CommandField HeaderText="Action" ShowEditButton="True" ControlStyle-ForeColor="SteelBlue" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlTranslate" runat="server" ConnectionString="<%$ ConnectionStrings:payitConnectionActive %>" SelectCommand="SELECT [LanguageCode], [SourceText], [TranslatedText], [Status], [ID] FROM [Translations]" ConflictDetection="CompareAllValues" DeleteCommand="DELETE FROM [Translations] WHERE [ID] = @original_ID AND (([LanguageCode] = @original_LanguageCode) OR ([LanguageCode] IS NULL AND @original_LanguageCode IS NULL)) AND (([SourceText] = @original_SourceText) OR ([SourceText] IS NULL AND @original_SourceText IS NULL)) AND (([TranslatedText] = @original_TranslatedText) OR ([TranslatedText] IS NULL AND @original_TranslatedText IS NULL)) AND (([Status] = @original_Status) OR ([Status] IS NULL AND @original_Status IS NULL))" InsertCommand="INSERT INTO [Translations] ([LanguageCode], [SourceText], [TranslatedText], [Status]) VALUES (@LanguageCode, @SourceText, @TranslatedText, @Status)" OldValuesParameterFormatString="original_{0}" UpdateCommand="UPDATE [Translations] SET [LanguageCode] = @LanguageCode, [SourceText] = @SourceText, [TranslatedText] = @TranslatedText, [Status] = @Status WHERE [ID] = @original_ID AND (([LanguageCode] = @original_LanguageCode) OR ([LanguageCode] IS NULL AND @original_LanguageCode IS NULL)) AND (([SourceText] = @original_SourceText) OR ([SourceText] IS NULL AND @original_SourceText IS NULL)) AND (([TranslatedText] = @original_TranslatedText) OR ([TranslatedText] IS NULL AND @original_TranslatedText IS NULL)) AND (([Status] = @original_Status) OR ([Status] IS NULL AND @original_Status IS NULL))">
                                    <DeleteParameters>
                                        <asp:Parameter Name="original_ID" Type="Int32" />
                                        <asp:Parameter Name="original_LanguageCode" Type="String" />
                                        <asp:Parameter Name="original_SourceText" Type="String" />
                                        <asp:Parameter Name="original_TranslatedText" Type="String" />
                                        <asp:Parameter Name="original_Status" Type="Boolean" />
                                    </DeleteParameters>
                                    <InsertParameters>
                                        <asp:Parameter Name="LanguageCode" Type="String" />
                                        <asp:Parameter Name="SourceText" Type="String" />
                                        <asp:Parameter Name="TranslatedText" Type="String" />
                                        <asp:Parameter Name="Status" Type="Boolean" />
                                    </InsertParameters>
                                    <UpdateParameters>
                                        <asp:Parameter Name="LanguageCode" Type="String" />
                                        <asp:Parameter Name="SourceText" Type="String" />
                                        <asp:Parameter Name="TranslatedText" Type="String" />
                                        <asp:Parameter Name="Status" Type="Boolean" />
                                        <asp:Parameter Name="original_ID" Type="Int32" />
                                        <asp:Parameter Name="original_LanguageCode" Type="String" />
                                        <asp:Parameter Name="original_SourceText" Type="String" />
                                        <asp:Parameter Name="original_TranslatedText" Type="String" />
                                        <asp:Parameter Name="original_Status" Type="Boolean" />
                                    </UpdateParameters>
                                </asp:SqlDataSource>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript" src="Scripts/quicksearch.js"></script>
<script type="text/javascript">
    $(function () {
        $('.search_textbox').each(function (i) {
            $(this).quicksearch("[id*=GridView1] tr:not(:has(th))", {
                'testQuery': function (query, txt, row) {
                    return $(row).children(":eq(" + i + ")").text().toLowerCase().indexOf(query[0].toLowerCase()) != -1;
                }
            });
        });
    });
</script>
</asp:Content>


