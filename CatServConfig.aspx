<%@ Page Title="Category and Services" Language="VB" MasterPageFile="~/MasterPage2.master" EnableEventValidation="false" AutoEventWireup="false" CodeFile="CatServConfig.aspx.vb" Inherits="CatServConfig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.5.js" type="text/javascript"></script>
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <script src="Scripts/alertify.js"></script>
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
    <link rel="stylesheet" href="Stylesheet/pure.css">
    <link href="Stylesheet/forms.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.24/themes/smoothness/jquery-ui.css" />
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.24/jquery-ui.min.js"></script>
    <style>
       .hiddencol
        {
            display: none;
        }

        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .modalPopup
        {
            background-color: #3e444c;
            color: #f5f5f5;
            position: relative;
            overflow-y: auto;
            padding: 15px;
            border-bottom: 1px solid #1c1e22;
            max-width: 600px;
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
    <!--<script type="text/javascript">
    $(document).ready(function() {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(load_lazyload);
        load_lazyload();
    });
    function load_lazyload() {
        $("#<%= GridView1.ClientID %>").tableDnD({
	    onDragClass: "GbiHighlight",
	    onDrop: function(table, row) {
                var rows = table.tBodies[0].rows;
                var debugStr = "Row dropped was "+row.id+". New order: ";
                for (var i=0; i<rows.length; i++) {
                    debugStr += rows[i].id+" ";
                }
	            $("#debugArea").html(debugStr);
	        },
		    onDragStart: function(table, row) {
			    $("#debugArea").html("Started dragging row "+row.id);
		    }
	    });
	}

</script>-->
<script type="text/javascript">
    function showDiv() {
        document.getElementById('contact').style.display = "block";
    }
</script>
<script type="text/javascript">
    $(document).ready(function () {
        setTimeout(function () {
            $('#<%= dberrorlabel.ClientId%>').hide();
    }, 5000); //
});
</script>
<!--<script type="text/javascript">
$(function () {
    $("[id*=GridView1]").sortable({
        items: 'tr:not(tr:first-child)',
        cursor: 'pointer',
        axis: 'y',
        dropOnEmpty: false,
        start: function (e, ui) {
            ui.item.addClass("selected");
        },
        stop: function (e, ui) {
            ui.item.removeClass("selected");
        },
        receive: function (e, ui) {
            $(this).find("tbody").append(ui.item);
        }
    });
});
</script>-->
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="wrapper">
        <!-- Content Wrapper. Contains page content -->
        <div class="content-wrapper">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <!-- Content Header (Page header) -->
                    <div class="content-header">
                        <h3>Category and Services
                        </h3>
                        <ol class="pull-right">
                            <asp:LinkButton ID="LinkButton1" runat="server">Export Report To Excel</asp:LinkButton>
                        </ol>
                    </div>
                    <div class="alert-danger">
                        <asp:Label ID="dberrorlabel" Font-Size="Larger" Font-Bold="true" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="row">
                        <div class="col-md-2"></div>
                        <!-- ./col -->
                        <div class="col-md-6 col-md-offset-3">
                            <div>
                                <div class="form-horizontal">
                                    <fieldset>
                                        <p>Add Config Details</p>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label1" runat="server" Text="PayitCategory"></asp:Label>
                                            </div>
                                            <asp:DropDownList ID="ddlCat" runat="server" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label4" runat="server" Text="PayitServices"></asp:Label>
                                            </div>
                                            <asp:DropDownList ID="ddlService" runat="server" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label2" runat="server" Text="Status"></asp:Label>
                                            </div>
                                            <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </label>
                                        <div class="submit-wrapper">
                                            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-default btn-sm" Text="Add Configuration" />
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-1"></div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="box box-success">
                                <div style="margin-bottom: 20px;"></div>
                                <!--<a href="#" id="aa" onclick="showDiv()" class="one btn btn-success btn-sm pull-right">Add Configuration</a>-->
                                <h4 class="box-title">Configuration Details</h4>
                                <!--<input type="button" id="addCat" value="Add Category"  onclick="showDiv()" class="btn btn-success btn-sm pull-right" />-->
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-6 col-md-offset-3">
                                            <div class="form-horizontal">
                                                <label>
                                                    <div>
                                                        <asp:Label ID="ddlFilterLabel" runat="server" Font-Size="12px" Text="Filter Categories"></asp:Label>
                                                    </div>
                                                    <asp:DropDownList ID="ddlFilter" runat="server" AutoPostBack="true" DataSourceID="DropDownDataSource"
                                                        DataTextField="CategoryName" DataValueField="ID" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="All Categories" Value="" />
                                                    </asp:DropDownList>
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:SqlDataSource ID="DropDownDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:payitconnectionActive %>"
                                        SelectCommand="Select DISTINCT ID, CategoryName from PayitCategories"></asp:SqlDataSource>
                                    <%-- <asp:DropDownList ID="ddlFilter" runat="server" CssClass="input-field form-control" AutoPostBack="True">
                                    </asp:DropDownList>--%>
                                    <div class="row">
                                        <div class="col-md-12">
                                             <div class="col-md-12">
                                              <asp:GridView ID="GridView1" runat="server"   AllowSorting="true"
                                        EmptyDataText="No Records Found" CssClass="table table-bordered table-responsive" AutoGenerateColumns="false" BackColor="#2b2028" HeaderStyle-ForeColor="White" HorizontalAlign="Center"
                                        PageSize="20" AllowPaging="true"  ForeColor="White"  DataSourceID="SqlCatServConfig">
                                        <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                        <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                        <HeaderStyle BackColor="Gray" Font-Bold="True" HorizontalAlign="Left" />
                                        <RowStyle HorizontalAlign="Left" />

                                       <%--     <asp:GridView ID="GridView1" runat="server" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" AllowSorting="true"
                                                AllowPaging="true" PageSize="25" BorderColor="DimGray" BorderStyle="Outset"
                                                BorderWidth="2px" CellPadding="0" Font-Bold="True" Font-Size="Smaller" ForeColor="White"
                                                HorizontalAlign="Center" DataSourceID="SqlCatServConfig" AutoGenerateColumns="False">
                                                <RowStyle HorizontalAlign="Center" Font-Names="Times New Roman" />
                                                <FooterStyle BorderWidth="5px" />
                                                <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                                <HeaderStyle BorderStyle="Dotted" ForeColor="#00C0C0" />
                                                <AlternatingRowStyle BackColor="#666666" BorderColor="#666666" Font-Bold="False"
                                                    ForeColor="White" BorderWidth="1px" />--%>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Id" ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" ItemStyle-Width="30">
                                                        <ItemTemplate>
                                                            <%# Eval("ID") %>
                                                            <input type="hidden" name="Id" value='<%# Eval("ID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="PayitCategoryID" HeaderText="PayitCategoryID" SortExpression="CategoryName" />

                                                    <asp:BoundField DataField="CategoryName" HeaderText="Category Name" SortExpression="CategoryName" />

                                                    <asp:BoundField DataField="PayitServicesID" HeaderText="PayitCategoryID" SortExpression="CategoryName" />
                                                    <asp:BoundField DataField="ServiceName" HeaderText="Services Name" SortExpression="ServiceName" />
                                                    <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                        <ItemTemplate><%#IIf(Boolean.Parse(Eval("Status").ToString()), "Active", "Inactive")%></ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Status" DataField="Status" ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" SortExpression="Status" />
                                                    <asp:BoundField DataField="PayitServicePriority" HeaderText="Priority" SortExpression="PayitServicePriority" />
                                                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                                                    <asp:TemplateField ItemStyle-Width="30px" HeaderText="Action">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" ForeColor="#0000EE" runat="server" Text="Edit" OnClick="Edit"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>

                                        <asp:SqlDataSource ID="SqlCatServConfig" runat="server" ConnectionString="<%$ ConnectionStrings:payitConnectionActive %>"
                                            SelectCommand="SELECT psc.ID, pc.CategoryName, ps.ServiceName, psc.Status, psc.PayitServicePriority, psc.PayitCategoryID, psc.PayitServicesID FROM payit.dbo.PayitServicesCategories psc LEFT JOIN payit.dbo.PayitCategories pc ON psc.PayitCategoryID = pc.ID LEFT JOIN payit.dbo.PayitServices ps ON psc.PayitServicesID = ps.ID ORDER BY Status DESC, PayitServicePriority" FilterExpression="PayitCategoryID = {0}"
                                            UpdateCommand="UPDATE PayitServicesCategories SET PayitCategoryID=@PayitCategoryID, PayitServicesID=@PayitServicesID, Status=@Status, PayitServicePriority=@PayitServicePriority WHERE ID=@original_ID">
                                            <FilterParameters>
                                                <asp:ControlParameter Name="PayitCategoryID" ControlID="ddlFilter" PropertyName="SelectedValue" />
                                            </FilterParameters>
                                            <UpdateParameters>
                                                <asp:FormParameter Name="PayitCategoryID" FormField="ddlCatEdit" Type="Int32" />
                                                <asp:FormParameter Name="Status" FormField="ddlStatusEdit" Type="Boolean" />
                                                <asp:FormParameter Name="PayitServicesID" FormField="ddlServiceEdit" Type="Int32" />
                                                <asp:FormParameter Name="ID" FormField="txtID" Type="Int32" />
                                                <asp:FormParameter Name="PayitServicePriority" FormField="txtPriority" Type="Int32" />
                                                <asp:FormParameter Name="original_PayitCategoryID" Type="Int32" />
                                                <asp:FormParameter Name="original_PayitServicesID" Type="Int32" />
                                                <asp:FormParameter Name="original_Status" Type="Boolean" />
                                                <asp:FormParameter Name="original_ID" Type="Int32" />
                                                <asp:FormParameter Name="original_PayitServicePriority" Type="Int32" />
                                            </UpdateParameters>
                                        </asp:SqlDataSource>
                                        <asp:Panel ID="pnlAddEdit" runat="server" CssClass="modalPopup">
                                            <div class="form-horizontal">
                                                <p>Edit details</p>
                                                <fieldset>

                                                    <label class="hiddencol">
                                                        <div>
                                                            <asp:Label ID="Label7" runat="server" Text="ID"></asp:Label>
                                                        </div>
                                                        <asp:TextBox ID="txtID" runat="server"></asp:TextBox>

                                                    </label>
                                                    <label>
                                                        <div>
                                                            <asp:Label ID="Label10" runat="server" Text="Category"></asp:Label>
                                                        </div>
                                                        <asp:DropDownList runat="server" ID="ddlCatEdit"></asp:DropDownList>
                                                    </label>
                                                    <label>
                                                        <div>
                                                            <asp:Label ID="Label17" runat="server" Text="Service"></asp:Label>
                                                        </div>
                                                        <asp:DropDownList runat="server" ID="ddlServiceEdit"></asp:DropDownList>
                                                    </label>

                                                    <label>
                                                        <div>
                                                            <asp:Label ID="Label8" runat="server" Text="Status"></asp:Label>
                                                        </div>
                                                        <asp:DropDownList runat="server" ID="ddlStatusEdit"></asp:DropDownList>
                                                    </label>
                                                    <label>
                                                        <div>
                                                            <asp:Label ID="Label18" runat="server" Text="Priority"></asp:Label>
                                                        </div>
                                                        <asp:TextBox runat="server" ID="txtPriority"></asp:TextBox>
                                                    </label>

                                                    <div class="panel-footer" style="border: none;">
                                                        <div class="submit-wrapper pull-right">
                                                            <asp:Button ID="btnSave" CssClass="pure-button pure-button-primary" runat="server" Text="Save" OnClick="Save" />
                                                            <asp:Button ID="btnCancel" CssClass="pure-button" runat="server" Text="Cancel" OnClientClick="return Hidepopup()" />
                                                        </div>
                                                    </div>
                                                </fieldset>
                                            </div>
                                        </asp:Panel>
                                        <center>
                                            <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
                                            <cc1:ModalPopupExtender ID="popup" runat="server" DropShadow="false"
                                                PopupControlID="pnlAddEdit" TargetControlID="lnkFake"
                                                BackgroundCssClass="modalBackground">
                                            </cc1:ModalPopupExtender>
                                        </center>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    
</asp:Content>

               