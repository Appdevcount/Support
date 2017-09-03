<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="refundTransactions.aspx.vb" Inherits="refundTransactions" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!DOCTYPE html>
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <link href="Stylesheet/forms.css" rel="stylesheet" />
    <style>
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }
         .modalPopup
        {
            background-color:  #1c1e22;
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
            .GridPager a, .GridPager span
        {
            display: block;
            height: 18px;
            width: 18px;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }
        .GridPager a
        {
            color: #fff;
            border: 1px solid #969696;
        }
        .GridPager span
        {
            background-color: #A1DCF2;
            color: #000;
            border: 1px solid #3AC0F2;
        }
    </style>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h5>Pay-It Refunds</h5>
        </div>
        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="pull-right" Font-Names="Centaur" 
                ForeColor="#00C3C6">Export Report To Excel</asp:LinkButton>
        <div class="panel-body">
            <div class="row">
                <div class="col-lg-3"></div>
                <div class="col-lg-6">
                    <div class="form-horizontal">
                        <fieldset>
                            <label>
                                <div>
                                <asp:Label ID="Label2" runat="server" Text="Data Source"></asp:Label></div>
                                <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="false" >
                                    <asp:ListItem>ThirdParty_knet_trans</asp:ListItem>
                                    <asp:ListItem>ThirdParty_knet_trans_20160420</asp:ListItem>
                                    <asp:ListItem>ThirdParty_knet_trans_20160324</asp:ListItem>
                                    <asp:ListItem>ThirdParty_knet_trans_WithPartitions</asp:ListItem>
                                    <asp:ListItem>ThirdParty_knet_trans_20151231</asp:ListItem>
                                    <asp:ListItem>ThirdParty_knet_trans_20150728_20150922</asp:ListItem>
                                    <asp:ListItem>ThirdParty_knet_trans_20150728</asp:ListItem>
                                    <asp:ListItem>ThirdParty_knet_trans_20150518</asp:ListItem>
                                    <asp:ListItem>ThirdParty_knet_trans20150422</asp:ListItem>
                                    <asp:ListItem>ThirdParty_knet_trans_20150319</asp:ListItem>
                                    <asp:ListItem>ThirdParty_knet_trans_20150219</asp:ListItem>
                                    <asp:ListItem>ThirdParty_knet_trans_20141224_20150120</asp:ListItem>
                                    <asp:ListItem>ThirdParty_knet_trans_20141223</asp:ListItem>
                                    <asp:ListItem>ThirdParty_knet_trans20141116</asp:ListItem> 
                                    <asp:ListItem>ThirdParty_knet_trans_20140910_20141023</asp:ListItem>                           
                                    <asp:ListItem>ThirdParty_knet_trans20140801_20140910</asp:ListItem>                                                        
                                    <asp:ListItem>ThirdParty_knet_trans_20140501_20140731</asp:ListItem>                            
                                    <asp:ListItem>ThirdParty_knet_trans_20140201_20140430</asp:ListItem>                            
                                    <asp:ListItem>ThirdParty_knet_trans_20131021_20140131</asp:ListItem>
                                    <asp:ListItem>ThirdParty_knet_trans_20130620_20131020</asp:ListItem>
                                    <asp:ListItem>ThirdParty_knet_trans_20130101_20130620</asp:ListItem>
                                    <asp:ListItem>ThirdParty_knet_trans_20120827_20121231</asp:ListItem>
                                    <asp:ListItem>ThirdParty_knet_trans_20100628_20120827</asp:ListItem>
                                </asp:DropDownList>
                            </label>
                            <label>
                                <div>
                                <asp:Label ID="Label1" runat="server" Text="Service Type"></asp:Label></div>
                                <asp:DropDownList ID="ddlService" runat="server" AutoPostBack="false">
                                </asp:DropDownList>
                            </label>
                             <label>
                                <div>
                                    <asp:Label ID="Label4" runat="server" Text="Refund Type"></asp:Label>
                                </div>
                                <asp:DropDownList ID="ddlRefundType" runat="server" AutoPostBack="false"></asp:DropDownList>
                            </label>
                            <label>
                                <div>
                                <asp:Label ID="LabelMsg" runat="server" Text="Mobile"></asp:Label></div>
                                <asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>
                            </label>
                            <label>
                                <div>
                                <asp:Label ID="Label5" runat="server" Text="Track ID"></asp:Label></div>
                                <asp:TextBox ID="txtTrackID" runat="server"></asp:TextBox>
                            </label>
                            <label>
                                <div>
                                <asp:Label ID="FromDateLabel" runat="server" Text="From Date"></asp:Label></div>
                                <asp:Label ID="Label14" runat="server" CssClass="label label-primary pull-right"></asp:Label>
                                <asp:TextBox ID="FromDateTextBox" runat="server"></asp:TextBox>
                            </label>
                            <label>
                                <div>
                                <asp:Label ID="Label3" runat="server" Text="To Date"></asp:Label></div>
                                <asp:Label ID="Label15" runat="server" CssClass="label label-primary pull-right"></asp:Label>
                                <asp:TextBox ID="ToDateTextBox" runat="server"></asp:TextBox>
                            </label>
                            <div class="label-group input-below">
                                <label style="visibility: hidden">Statusdddddddsds</label>
                                <label>
                                    <asp:CheckBox runat="server" ID="chkStatus" Checked="false" AutoPostBack="false" />
                                    <asp:Label runat="server" ID="lblPaging" Text="Disable Paging"></asp:Label>
                                </label>
                            </div>
                            <div style="margin-bottom:10px;"></div>
                            <div class="submit-wrapper pull-right">
                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-default btn-sm" Text="Search" />
                            </div>
                        </fieldset>
                    </div>
                </div>
                <!-- /.col-lg-6 -->
                <div class="col-lg-3"></div>
            </div>
            <!-- /.row -->
        </div>
    </div>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:Label ID="Label17" runat="server" Font-Size="Small" CssClass="label label-success"></asp:Label>
    <asp:Label ID="Label19" runat="server" Font-Size="Small" CssClass="label label-success"></asp:Label>
    <asp:Label ID="Label20" runat="server" Font-Size="Small" CssClass="label label-success"></asp:Label>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-responsive" HeaderStyle-ForeColor="White"
                AutoGenerateColumns="True" BorderStyle="None" CellPadding="0" HorizontalAlign="Center" AllowPaging="True"
                OnPageIndexChanging="OnPaging"
                PageSize="25" EnableModelValidation="True">
                <PagerStyle HorizontalAlign="Center" CssClass = "GridPager"  />
                <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                <HeaderStyle BackColor="Gray" Font-Bold="True" HorizontalAlign="Left" />
                <RowStyle HorizontalAlign="Left" />
            </asp:GridView>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="GridView1" />
        </Triggers>
    </asp:UpdatePanel>
     <script src="Scripts/bootstrap-datepicker.js"></script>
    <script type="text/javascript">
        function pageLoad() {
            $("#ctl00_ContentPlaceHolder1_FromDateTextBox").unbind();
            $('#ctl00_ContentPlaceHolder1_FromDateTextBox').datepicker({
                format: "yyyy-mm-dd",
                todayBtn: "linked",
                todayHighlight: true,
                autoclose: true,
                orientation: "top auto",
                multidate: false
            });
            $("#ctl00_ContentPlaceHolder1_ToDateTextBox").unbind();
            $('#ctl00_ContentPlaceHolder1_ToDateTextBox').datepicker({
                format: "yyyy-mm-dd",
                todayBtn: "linked",
                autoclose: true,
                todayHighlight: true
            });
        }
    </script>
</asp:Content>

