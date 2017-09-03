<%@ Page Title="OgMoneyKW:Local Topup" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false"
    CodeFile="LocalTopup.aspx.vb" Inherits="LocalTopup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <!-- JavaScript -->
    <script src="Scripts/alertify.js"></script>
    <!-- CSS -->
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
    <!-- Default theme -->
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
    <script type="text/javascript">
        $(document).ready(function () {
            $(function buttonChk() { $('#ctl00_ContentPlaceHolder1_btnOnay, #ctl00_ContentPlaceHolder1_btnRed').hide(); });
        });
     

        function abcf()
        {
        }
        function abct()
        {
         div1.style.width = "50"
         $("#ctl00_ContentPlaceHolder1_Button2").prop('value', 'Searching');
         $("#ctl00_ContentPlaceHolder1_Button2").removeClass("btn-info").addClass("btn-warning");
         //img12.style.display="block" 
        }
    </script>
<div style="text-align: right">
            <asp:LinkButton ID="LinkButton1" runat="server" Font-Names="Centaur" 
                ForeColor="#00C3C6">Export Report To Excel</asp:LinkButton>
    <br />
            <asp:Label ID="lblComm" runat="server" Font-Bold="False" Font-Names="Centaur"
                ForeColor="#00C0C0" Height="17px" Text="With Commission" Width="112px"></asp:Label>
        
            <asp:CheckBox ID="chkCommission" runat="server" AutoPostBack="true" OnCheckedChanged="chkCommission_CheckedChanged" Checked="false" />
                            
        </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="GridView1" />
            <asp:AsyncPostBackTrigger ControlID="Button2" />
        </Triggers>
        <ContentTemplate>
            <%-- <asp:Label ID="Label8" Text="<%$Resources:Resource, LocalTopUp %>" runat="server"
         Font-Bold="False" Font-Size="Larger" Font-Underline="False" ForeColor="White" Style="text-align: center"/>--%>
            <asp:Label ID="Label8" runat="server" Font-Bold="False" Font-Size="Larger" Font-Underline="False"
                ForeColor="White" Style="text-align: center" Text="Local Transactions Details "
                Font-Names="Times New Roman"></asp:Label>

            <asp:SqlDataSource ID="ProcessTypeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:payitConnectionActive %>"
                SelectCommand="SELECT distinct([knetprocess]) FROM [ThirdParty_knet_trans] where knetprocess is not null ORDER BY [knetprocess]"></asp:SqlDataSource>
            <table>
                <tr>
                    <td style="width: 47px; text-align: left">
                        <asp:Label ID="Label21" runat="server" Font-Bold="False" Font-Names="Centaur" ForeColor="#00C0C0"
                            Height="17px" Text="DataSource :" Width="112px"></asp:Label>
                    </td>
                    <td style="text-align: left; width: 133px;">
                        <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="True"
                            Width="155px">
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
                    </td>
                    <td style="text-align: left; width: 133px;">&nbsp;</td>
                </tr>
                <asp:Button ID="btnOnay" runat="server" EnableViewState="False" CssClass="hiddencol" Text="Reprocess" OnClick="btnOnay_Click" Visible="true" />
                <asp:Button ID="btnRed" runat="server" EnableViewState="False" CssClass="hiddencol" OnClick="btnRed_Click" Visible="true" />
                <tr>
                    <td style="width: 47px; text-align: left">
                        <asp:Label ID="Label9" runat="server" Font-Bold="False" Font-Names="Centaur"
                            ForeColor="#00C0C0" Height="17px" Text="ServiceType :" Width="112px"></asp:Label>
                    </td>
                    <td style="text-align: left; width: 133px;">
                        <asp:DropDownList ID="DropDownList2" runat="server" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged" AutoPostBack="True"
                            Width="155px">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: left; width: 133px;">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 47px; text-align: left">
                        <asp:Label ID="Label16" runat="server" Font-Bold="False" Font-Names="Centaur" ForeColor="#00C0C0"
                            Height="17px" Text="Service Provider :" Width="112px"></asp:Label>
                    </td>
                    <td style="text-align: left; width: 133px;">
                        <asp:DropDownList ID="ddlServiceProvider" runat="server" AutoPostBack="True" Width="155px">
                            <asp:ListItem>All</asp:ListItem>
                            <asp:ListItem>iSYS</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: left; width: 133px;">&nbsp;
                    </td>
                </tr>

                <tr>
                    <td style="width: 47px; text-align: left">
                        <asp:Label ID="Label10" runat="server" Font-Bold="False" Font-Names="Centaur" ForeColor="#00C0C0"
                            Height="17px" Text="Mobile NO :" Width="112px"></asp:Label>
                    </td>
                    <td style="text-align: left; width: 133px;">
                        <asp:TextBox ID="MobileNOTextBox" runat="server" Width="145px"></asp:TextBox>
                    </td>
                    <td style="text-align: left; width: 133px;">
                        <asp:Label ID="Label11" runat="server" Font-Names="Times New Roman" Font-Size="X-Small"
                            ForeColor="#009933" Text="Optional"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 47px; text-align: left">
                        <asp:Label ID="AmtLabel" runat="server" Font-Bold="False" Font-Names="Centaur" ForeColor="#00C0C0"
                            Height="17px" Text="Amount :" Width="112px"></asp:Label>
                    </td>
                    <td style="text-align: left; width: 133px;">
                        <asp:TextBox ID="AmountTextBox" runat="server" Width="145px"></asp:TextBox>
                    </td>
                    <td style="text-align: left; width: 133px;">
                        <asp:Label ID="Label12" runat="server" Font-Names="Times New Roman" Font-Size="X-Small"
                            ForeColor="#009933" Text="Optional"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left">
                        <asp:Label ID="TrackIDLabel" runat="server" Font-Bold="False" Font-Names="Centaur"
                            ForeColor="#00C0C0" Height="17px" Text="TrackID :" Width="112px"></asp:Label>
                    </td>
                    <td style="width: 133px; text-align: left;">
                        <asp:TextBox ID="TrackIDTextBox" runat="server" Width="145px"></asp:TextBox>
                    </td>
                    <td style="width: 133px; text-align: left;">
                        <asp:Label ID="Label13" runat="server" Font-Names="Times New Roman" Font-Size="X-Small"
                            ForeColor="#009933" Text="Optional"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td style="width: 47px; text-align: left">
                        <asp:Label ID="FromDateLabel" runat="server" Font-Bold="False" Font-Names="Centaur"
                            ForeColor="#00C0C0" Height="17px" Text="FromDate :" Width="112px"></asp:Label>
                    </td>
                    <td style="text-align: left; width: 133px;">
                        <asp:TextBox ID="FromDateTextBox" runat="server" Width="145px"></asp:TextBox>
                        <%-- <cc1:CalendarExtender ID="FromDateTextBox_CalendarExtender" runat="server" 
                                    Enabled="True" Format="dd/MM/yyyy" TargetControlID="FromDateTextBox" 
                                    onclientshowing="CurrentDateShowing" CssClass="cal_KimTheme">
                                </cc1:CalendarExtender>--%>
                        <asp:Label ID="Label14" runat="server" Font-Names="Times New Roman" ForeColor="#FF3300"
                            Text="*"></asp:Label>
                    </td>
                    <td style="text-align: left; width: 133px;">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left">
                        <asp:Label ID="ToDateLabel" runat="server" Font-Bold="False" Font-Names="Centaur"
                            ForeColor="#00C0C0" Height="17px" Text="ToDate :" Width="112px"></asp:Label>
                    </td>
                    <td style="width: 133px; text-align: left;">
                        <asp:TextBox ID="ToDateTextBox" runat="server" Width="145px"></asp:TextBox>
                        <%-- <cc1:CalendarExtender ID="ToDateTextBox_CalendarExtender" runat="server" 
                                    Enabled="True" Format="dd/MM/yyyy" TargetControlID="ToDateTextBox" 
                                    onclientshowing="CurrentDateShowing" CssClass="cal_KimTheme">
                                </cc1:CalendarExtender>--%>
                        <asp:Label ID="Label15" runat="server" Font-Names="Times New Roman" ForeColor="#FF3300"
                            Text="*"></asp:Label>
                    </td>


                    <td style="width: 133px; text-align: left;">&nbsp;
                    </td>
                </tr>

                <tr>
                    <td style="text-align: left">
                        <asp:Label ID="AllowPagingLable" runat="server" Font-Bold="False" Font-Names="Centaur"
                            ForeColor="#00C0C0" Height="17px" Text="Allow Paging :" Width="112px"></asp:Label>
                    </td>
                    <td style="width: 133px; text-align: left;">
                        <asp:CheckBox ID="CheckBox1" runat="server" Checked="True" />
                    </td>
                    <td style="width: 133px; text-align: left;">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left">
                        <asp:Label ID="Label1" runat="server" Font-Bold="False" Font-Names="Centaur"
                            ForeColor="#00C0C0" Height="17px" Text="All Trans:" Width="112px"></asp:Label>
                    </td>
                    <td style="width: 133px; text-align: left;">
                        <asp:CheckBox ID="CheckBox2" runat="server" name="quux[1]" AutoPostBack="true" OnCheckedChanged="CheckBox2_CheckedChanged" Checked="True" />
                    </td>
                    <td style="width: 133px; text-align: left;">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left">
                        <asp:Label ID="Label2" runat="server" Font-Bold="False" Font-Names="Centaur"
                            ForeColor="#00C0C0" Height="17px" Text="Reprocessed:" Width="112px"></asp:Label>
                    </td>
                    <td style="width: 133px; text-align: left;">
                        <asp:CheckBox ID="chkReprocess" runat="server" AutoPostBack="true" OnCheckedChanged="chkReprocess_CheckedChanged" Checked="false" />
                    </td>
                    <td style="width: 133px; text-align: left;">&nbsp;
                    </td>
                </tr>

                <tr>
                    <td style="text-align: right">&nbsp;
                    </td>
                    <td style="text-align: center; width: 133px;">
                        <asp:Button ID="Button2" runat="server" CssClass="btn btn-info" Style="text-align: left; height: 36px;" Text="Search" />
                    </td>
                    <td style="text-align: center; width: 133px;">&nbsp;
                    </td>
                </tr>

            </table>
            <div id="div1">
            </div>
            <asp:Label ID="Label17" runat="server" Font-Size="Small" CssClass="label label-success"></asp:Label>
            <asp:Label ID="Label19" runat="server" Font-Size="Small" CssClass="label label-success"></asp:Label>
            <asp:Label ID="Label20" runat="server" Font-Size="Small" CssClass="label label-success"></asp:Label>
            <asp:GridView ID="GridView1" runat="server" EmptyDataText="No Data" BorderColor="DimGray"
                BorderStyle="Outset" BorderWidth="2px" CellPadding="0"
                Font-Bold="True"
                Font-Size="Small" ForeColor="White" HorizontalAlign="Center"
                PageSize="50" Font-Names="Calibri" OnRowDataBound="GridView1_RowDataBound">
                <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                <RowStyle HorizontalAlign="Center" Font-Names="Calibri" />
                <FooterStyle BorderWidth="5px" />
                <HeaderStyle BorderStyle="Dotted" ForeColor="#00C0C0" />
                <AlternatingRowStyle BackColor="#666666" BorderColor="#666666"
                    Font-Bold="False" ForeColor="White"
                    BorderWidth="1px" Font-Size="Small" />
                <Columns>
                    <asp:TemplateField ControlStyle-ForeColor="#0066ff">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" CommandName="ContentType" CommandArgument='<%# Eval("TrackID") %>'
                                ID="LinkButton2" Text='Process' OnClick="ReprocessLinkButton_Click" />
                        </ItemTemplate>
                        <ItemStyle ForeColor="#009999" />
                    </asp:TemplateField>
                    <asp:TemplateField ControlStyle-ForeColor="#009999">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" CommandName="ContentType" CommandArgument='<%# Eval("TrackID") %>'
                                ID="LinkButtonChk" Text='Check' OnClick="CheckLinkButton_Click" />
                        </ItemTemplate>
                        <ItemStyle ForeColor="#009999" />
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
            <asp:Panel ID="pnlAddEdit" runat="server" CssClass="panel panel-danger modalPopup">
                <div class="form-horizontal">
                    <fieldset>
                        <div class="hiddencol">
                            <asp:TextBox runat="server" ID="TxtHidden"></asp:TextBox>
                        </div>
                        <div>
                            <p>There is an issue with this transaction. Do you want to reprocess?</p>
                        </div>

                        <div style="border: none;">
                            <div class="submit-wrapper pull-right">
                                <asp:Button ID="btnDelete" CssClass="btn btn-danger btn-md pull-left" runat="server" Text="Yes Reprocess" OnClick="ReprocessLinkButton_Click" />
                                <div class="pull-right">
                                    <asp:Button ID="btnCancel" CssClass="btn btn-warning btn-md" runat="server" Text="No" OnClientClick="return Hidepopup()" />
                                </div>
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
        </ContentTemplate>
    </asp:UpdatePanel>

    <script src="Scripts/bootstrap-datepicker.js"></script>
    <script type="text/javascript">
        function pageLoad() {
            $("#ctl00_ContentPlaceHolder1_FromDateTextBox").unbind();
            $('#ctl00_ContentPlaceHolder1_FromDateTextBox').datepicker({
                format: "dd/mm/yyyy",
                todayBtn: "linked",
                todayHighlight: true,
                autoclose: true,
                multidate: false
            });
            $("#ctl00_ContentPlaceHolder1_ToDateTextBox").unbind();
            $('#ctl00_ContentPlaceHolder1_ToDateTextBox').datepicker({
                format: "dd/mm/yyyy",
                todayBtn: "linked",
                autoclose: true,
                todayHighlight: true
            });
        }
    </script>
</asp:Content>
