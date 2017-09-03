<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage2.master" EnableEventValidation="false" AutoEventWireup="false" CodeFile="Complaints_new.aspx.vb" Inherits="Complaints_new" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <!DOCTYPE html>
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <link href="Stylesheet/bootstrap-select.css" rel="stylesheet" />
    <script src="Scripts/alertify.js"></script>
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
    <script src="scripts/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="scripts/jquery.blockUI.js" type="text/javascript"></script>
    <script src="Scripts/bootstrap-switch.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("[id*=lnkView]").click(function () {
                var rowIndex = $(this).closest("tr")[0].rowIndex;
                window.open("Popup.aspx?rowIndex=" + rowIndex, "Popup", "width=350,height=100");
            });
        });
    </script>
    <script type="text/javascript">

        $(document).ready(function () {

            BlockUI("<%=pnlAddEdit.ClientID %>");
        $.blockUI.defaults.css = {};
    });
    function Hidepopup() {
        $find("popup").hide();
        return false;
    }
    </script>

    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }

        .links
        {
            font-weight: bold;
        }

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
            background-color: #1c1e22;
            color: #f5f5f5;
            position: relative;
            overflow-y: auto;
            padding: 15px;
            border-bottom: 1px solid #1c1e22;
            max-width: 600px;
            margin-bottom: 5px;
        }
    </style>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3>Pay-It Complaints</h3>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-lg-10">
                    <div class="input-group-sm">
                        <span class="col-sm-3" style="margin-top: 20px;">
                            <asp:Button ID="Button2" runat="server" CssClass="btn btn-default" Text="Refresh" OnClick="Button2_Click" />
                        </span>
                        <span class="col-sm-3">
                            <asp:Label ID="Label21" CssClass="label label-primary" Font-Bold="true" Font-Size="Small" runat="server" Height="18px" Text="Complaint ID:"></asp:Label>
                            <asp:TextBox ID="TextID" CssClass="form-control" runat="server" />
                        </span>
                        <span class="col-sm-3">
                            <asp:Label ID="Label7" CssClass="label label-primary" Font-Bold="true" Font-Size="Small" runat="server" Height="18px" Text="Mobile No:"></asp:Label>
                            <asp:TextBox ID="TextMobile" CssClass="form-control" runat="server" />
                        </span>
                        <span class="col-sm-3">
                            <asp:Label ID="Label8" CssClass="label label-primary" Font-Bold="true" Font-Size="Small" runat="server" Height="18px" Text="Status:"></asp:Label>
                            <asp:DropDownList ID="DropDownStatus" runat="server" CssClass="form-control" AutoPostBack="false">
                                <asp:ListItem>All</asp:ListItem>
                                <asp:ListItem>Pending</asp:ListItem>
                                <asp:ListItem>Follow Up</asp:ListItem>
                                <asp:ListItem>Closed</asp:ListItem>
                            </asp:DropDownList>
                        </span>
                    </div>
                    <!-- /input-group -->
                </div>
                <!-- /.col-lg-10 -->
                <div class="col-lg-2">
                    <div class="input-group" style="margin-top: 18px;">
                        <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" Text="Search" />
                    </div>
                    <!-- /input-group -->
                </div>
                <!-- /.col-lg-6 -->
            </div>
            <!-- /.row -->
        </div>
    </div>
    <div class="row">
        <div class="col-md-1"></div>
        <div class="col-md-10">
            <asp:RadioButtonList ID="applist" runat="server">
                <asp:ListItem Text="App" Value="app" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Facebook" Value="fb"></asp:ListItem>
                <asp:ListItem Text="Instagram" Value="in"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="col-md-1"></div>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-responsive" HeaderStyle-ForeColor="White"
                AutoGenerateColumns="false" BorderStyle="None" CellPadding="0" HorizontalAlign="Center" AllowPaging="True"
                OnPageIndexChanging="OnPaging"
                PageSize="25">
                <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                <HeaderStyle BackColor="Gray" Font-Bold="True" HorizontalAlign="Left" />
                <RowStyle HorizontalAlign="Left" />
                <Columns>

                    <asp:TemplateField ControlStyle-ForeColor="#009999">
                        <HeaderTemplate>ID</HeaderTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" Text='<%#Eval("ID")%>' value='<%#Eval("ID")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField HeaderText="Name"
                        DataField="Name"
                        SortExpression="Name"
                        HtmlEncode="true"></asp:BoundField>

                    <asp:BoundField HeaderText="EmailID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                        DataField="EmailID"
                        NullDisplayText="None"
                        SortExpression="EmailID"
                        HtmlEncode="true"></asp:BoundField>

                    <asp:BoundField HeaderText="Mobile"
                        DataField="MobileNo"
                        NullDisplayText="None"
                        SortExpression="MobileNo"
                        HtmlEncode="true"></asp:BoundField>

                    <asp:BoundField HeaderText="Message"
                        DataField="Msg"
                        NullDisplayText="None"
                        SortExpression="Msg"
                        HtmlEncode="true"></asp:BoundField>

                    <asp:BoundField HeaderText="Date"
                        DataField="Datetime"
                        DataFormatString="{0:dd/MM/yyyy hh:mm:ss}"
                        SortExpression="Datetime"
                        HtmlEncode="true"></asp:BoundField>


                    <asp:BoundField HeaderText="Status"
                        DataField="Status"
                        SortExpression="Status"
                        HtmlEncode="true"></asp:BoundField>

                    <asp:BoundField HeaderText="Comments"
                        DataField="Comments"
                        SortExpression="Comments" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                        HtmlEncode="true"></asp:BoundField>

                    <asp:BoundField HeaderText="Processed By"
                        DataField="Processed_By"
                        SortExpression="Processed_By" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                        HtmlEncode="true"></asp:BoundField>

                    <asp:BoundField HeaderText="ComplaintId"
                        DataField="ID"
                        SortExpression="ID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                        HtmlEncode="true"></asp:BoundField>

                    <asp:TemplateField ItemStyle-Width="30px" HeaderText="Action" ControlStyle-ForeColor="#0066ff">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" OnClick="Edit"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <%-- <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick = "Add" /> --%>
            <center>
                <asp:Panel ID="pnlAddEdit" runat="server" CssClass="modalPopup">
                    <asp:Label Font-Bold="true" ID="Label4" Font-Size="Medium" runat="server" Text="Edit Details"></asp:Label>
                    <br />
                    <table class="table" style="border: none; background-color: #1c1e22" align="center">
                        <tr class="hiddencol">
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="ID"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtID" CssClass="form-control" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="hiddencol">
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="UID"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUID" CssClass="form-control" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="border: none;">
                                <asp:Label ID="Label1" runat="server" Text="Status"></asp:Label>
                            </td>
                            <td style="border: none;">
                                <asp:DropDownList runat="server" ID="ddlStatus" CssClass="form-control"></asp:DropDownList>
                                <!-- <asp:TextBox ID="txtStatus1" CssClass="form-control" MaxLength = "25" runat="server"></asp:TextBox>-->
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Comments"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox TextMode="MultiLine" ID="txtComments" CssClass="form-control" runat="server"></asp:TextBox>
                            </td>
                        </tr>

                        <%-- <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Processed By"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlProcessed" CssClass="form-control"></asp:DropDownList>
                      
                            </td>
                        </tr>--%>

                        <tr>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text=""></asp:Label>
                            </td>
                            <td>

                                <asp:Label ID="txtProcessedByError" CssClass="alert alert-info" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="text-center">
                                <asp:Button ID="btnSave" CssClass="btn btn-success btn-md" runat="server" Text="Save" OnClick="Save" />
                            </td>
                            <td class="text-right">
                                <asp:Button ID="btnCancel" CssClass="btn btn-warning btn-md" runat="server" Text="Cancel" OnClientClick="return Hidepopup()" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </center>
            <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
            <cc1:ModalPopupExtender ID="popup" runat="server" DropShadow="false"
                PopupControlID="pnlAddEdit" TargetControlID="lnkFake"
                BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>

            <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
            <cc1:ModalPopupExtender ID="LinkButton1_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" Enabled="True" TargetControlID="lnkDummy" PopupControlID="Panel1"></cc1:ModalPopupExtender>
            <asp:Panel ID="Panel1" CssClass="table table-condensed table-responsive" Width="40%" runat="server">
                <br />
                <br />
                <center>
                    <div class="well">
                        <h4>Pay-It Complaints Track</h4>
                        <asp:HiddenField ID="Label6" runat="server"></asp:HiddenField>
                        <asp:GridView ID="GridView2" EmptyDataText="No Records Found" CssClass="table table-bordered" BorderStyle="None" CellPadding="0" runat="server" AutoGenerateColumns="false">

                            <Columns>
                                <asp:BoundField DataField="Status" ControlStyle-CssClass="tab-content" HeaderText="Status" SortExpression="Status" />
                                <asp:BoundField DataField="Comments" HeaderText="Comments" SortExpression="Comments" />
                                <asp:BoundField DataField="Processed_By" HeaderText="Processed By" SortExpression="Processed_By" />
                                <asp:BoundField DataField="Created" HeaderText="Processed On" SortExpression="Created" />
                            </Columns>
                        </asp:GridView>
                        <asp:Button ID="Button3" runat="server" CssClass="btn btn-default btn-md" Text="Close" />
                    </div>
                </center>
            </asp:Panel>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="GridView1" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
