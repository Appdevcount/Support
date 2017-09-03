<%@ Page Language="VB" AutoEventWireup="false" CodeFile="uploadPINAdmin.aspx.vb" Inherits="SummaryReport" MasterPageFile="~/MasterPage2.master" Title="PayIt:Approve PINS" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <script src="Scripts/alertify.js"></script>
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
    <link rel="stylesheet" href="Stylesheet/pure.css">
    <link href="Stylesheet/forms.css" rel="stylesheet" />
    <script language="javascript">
        function abcf() {
            // alert("false");
            //img12.style.display ="none"   
            //img12.style.backgroundColor ="red" 
        }
        function abct() {
            //alert("True");
            //div1.style.backgroundColor ="white" 
            //div1.innerHTML="Loading Data.."
            div1.innerHTML = "<img src=loading_black.gif> <font Color=#00C0C0>Loading..</font>"//gallery-loading
            div1.style.width = "50"
            //img12.style.display="block" 
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="wrapper">
        <!-- Content Wrapper. Contains page content -->
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <div class="content-header">
                <h4>Approve PINS
                </h4>
                <ol class="pull-right">
                    <asp:LinkButton ID="LinkButton2" runat="server">Export Report To Excel</asp:LinkButton>
                </ol>
            </div>

            <div class="row" id="contact">
                <div class="col-md-3">
                </div>
                <!-- ./col -->
                <div class="col-md-6" style="float: inherit">
                    <!-- /.box-header -->
                    <div class="form-horizontal">
                        <fieldset>
                            <p>PINS Uploaded for Approval</p>
                            <label>
                                <div>
                                    <asp:Label ID="Label1" runat="server" Text="Service"></asp:Label>
                                </div>
                                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True"
                                    DataTextField="Service" DataValueField="Service">
                                </asp:DropDownList>
                            </label>
                            <label>
                                <div>
                                    <asp:Label ID="Label3" runat="server" Text="Denomination"></asp:Label></div>
                                <asp:DropDownList ID="DropDownList2" runat="server"
                                    DataTextField="Amount2" DataValueField="Amount" AutoPostBack="false">
                                </asp:DropDownList>
                            </label>
                            <label>
                                <div>
                                    <asp:Label ID="Label2" runat="server" Text="Description"></asp:Label></div>
                                <asp:Label ID="lblDescription" runat="server"></asp:Label>
                            </label>
                            <div class="submit-wrapper pull-right">

                                <asp:Button ID="Button1" runat="server" CssClass="btn btn-default btn-sm" Text="Display" />
                                <asp:Button ID="Button2" runat="server" CssClass="btn btn-success btn-sm" Text="Approve PINS" />
                                <div class="pull-right">
                                    <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" CssClass="btn btn-danger btn-sm" Text="Delete PINS" />
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div class="col-md-3"></div>
            </div>

            <asp:Label ID="Label17" runat="server" CssClass="alert-info" Font-Size="Large"></asp:Label>
            <asp:Label ID="Label18" runat="server" Font-Bold="True" Font-Size="Smaller" ForeColor="#00C0C0"></asp:Label>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="box box-success">
                                <h4 class="box-title">PIN Details</h4>
                                <div class="box-body">
                                    <asp:GridView ID="GridView1" runat="server" PageSize="50"
                                        CssClass="table table-bordered table-responsive" EmptyDataText="No PINS Found"
                                        HeaderStyle-ForeColor="White" BackColor="#2b2028" ForeColor="White" HorizontalAlign="Center">
                                        <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                        <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                        <HeaderStyle BackColor="Gray" Font-Bold="True" HorizontalAlign="Left" />
                                        <RowStyle HorizontalAlign="Left" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

