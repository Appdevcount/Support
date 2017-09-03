<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage2.master" EnableEventValidation="false" AutoEventWireup="false" CodeFile="Complaints.aspx.vb" Inherits="Complaints" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">
    $(function () {
        $("[id*=lnkView]").click(function () {
            var rowIndex = $(this).closest("tr")[0].rowIndex;
            popUpObj = window.open("Popup.aspx?rowIndex=" + rowIndex, "Popup", "width=600,height=100,toolbar=no,statusbar=no,left=490,top=300");
        }); 
        popUpObj.focus();
    });
</script>
    <style type="text/css">
        body
        {
            font-size: 10pt;
        }
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;

        }
        .modalPopup
        {
            background-color: #333;
            width: auto;
            padding: 0;
        }
        .modalPopup .header
        {
            background-color: #1c1e22;
            height: 30px;
            color: White;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
          border-bottom: 1px solid #1c1e22;
          min-height: 16.42857143px;
        }
        .modalPopup .body
        {
            min-height: 50px;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }
    </style>
    
    <script type="text/javascript">
        function ShowRowDetail() {
            $find("mpe").show();
        }
</script>
    <script type="text/javascript">
        function abcf()
        {
        // alert("false");
        //img12.style.display ="none"   
        //img12.style.backgroundColor ="red" 
        }
        function abct()
        {
        //alert("True");
        //div1.style.backgroundColor ="white" 
        //div1.innerHTML="Loading Data.."
         div1.innerHTML="<img src=loading_black.gif> <font Color=#00C0C0>Loading..</font>" //gallery-loading
         div1.style .width ="50"
        //img12.style.display="block" 
        }
    </script>
   
<div style="text-align: right">
            <asp:LinkButton ID="LinkButton1" runat="server" ForeColor="skyblue">Export Report To Excel</asp:LinkButton>
        </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1"  runat="server">
     <Triggers>
        <asp:PostBackTrigger ControlID="GridView1" />
     </Triggers>
        <ContentTemplate>
        <h1>Pay-It Complaints</h1>
            <%--<asp:Label ID="Label8" runat="server" Font-Bold="False" Font-Size="Larger" Font-Underline="False"
                Style="text-align: center" Text="Payit Complaints"
                Font-Names="Helvetica"></asp:Label>--%>

            <asp:SqlDataSource ID="ProcessTypeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:payitConnectionActive %>"
                SelectCommand="SELECT distinct([knetprocess]) FROM [ThirdParty_knet_trans] where knetprocess is not null ORDER BY [knetprocess]">
            </asp:SqlDataSource>

           <%--   <table class="table">
              <tr>
                    <td style="width: 47px; text-align: left">
                        <asp:Label ID="Label21" runat="server" Font-Bold="False" Font-Names="Centaur" ForeColor="#00C0C0"
                            Height="17px" Text="DataSource :" Width="112px"></asp:Label>
                    </td>
                    <td style="text-align: left; width: 133px;">
                        <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="True" 
                            Width="150px">
                            <asp:ListItem>ThirdParty_knet_trans</asp:ListItem>
                            <asp:ListItem>ThirdParty_knet_trans_20131021_20140131</asp:ListItem>
                            <asp:ListItem>ThirdParty_knet_trans_20130620_20131020</asp:ListItem>
                            <asp:ListItem>ThirdParty_knet_trans_20130101_20130620</asp:ListItem>
                            <asp:ListItem>ThirdParty_knet_trans_20120827_20121231</asp:ListItem>
                            <asp:ListItem>ThirdParty_knet_trans_20100628_20120827</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: left; width: 133px;">
                        &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 47px; text-align: left">
                            <asp:Label ID="Label9" runat="server" Font-Bold="False" Font-Names="Centaur" 
                                ForeColor="#00C0C0" Height="17px" Text="ServiceType :" Width="112px"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 133px;">
                            <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" 
                                Width="150px">
                                <asp:ListItem>All</asp:ListItem>
                                <asp:ListItem>ZAIN-X</asp:ListItem>
                                <asp:ListItem>ZAIN-P</asp:ListItem>
                                <asp:ListItem>VIVA-X</asp:ListItem>
                                <asp:ListItem>VIVA-P</asp:ListItem>
                                <asp:ListItem>WATANIYA-X</asp:ListItem>
                                <asp:ListItem>WATANIYA-P</asp:ListItem>
                                <asp:ListItem>WATANIYA-O</asp:ListItem>
                                <asp:ListItem>CashU-O</asp:ListItem>
                                <asp:ListItem>OneCard-O</asp:ListItem>
                                <asp:ListItem>iTunes-O</asp:ListItem>
                                <asp:ListItem>iTunes-UK-O</asp:ListItem>
                                <asp:ListItem>ZAIN-O</asp:ListItem>
                                <asp:ListItem>VIVA-O</asp:ListItem>
                                <asp:ListItem>TravelMate</asp:ListItem>
                                <asp:ListItem>Koutbo6-O</asp:ListItem>
                                <asp:ListItem>UKash-O</asp:ListItem>
                                <asp:ListItem>PlayStation-O</asp:ListItem>
                                <asp:ListItem>GooglePlay-O</asp:ListItem>
                                <asp:ListItem>Amazon-O</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: left; width: 133px;">
                            &nbsp;
                        </td>
                </tr>
                    <tr>
                        <td style="width: 47px; text-align: left">
                            <asp:Label ID="Label16" runat="server" Font-Bold="False" Font-Names="Centaur" ForeColor="#00C0C0"
                                Height="17px" Text="Service Provider :" Width="112px"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 133px;">
                            <asp:DropDownList ID="ddlServiceProvider" runat="server" AutoPostBack="True" Width="150px">
                                <asp:ListItem>All</asp:ListItem>
                                <asp:ListItem>iSYS</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: left; width: 133px;">
                            &nbsp;
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
                            <td style="text-align: left">
                                <asp:Label ID="AllowPagingLable" runat="server" Font-Bold="False" Font-Names="Centaur"
                                    ForeColor="#00C0C0" Height="17px" Text="Allow Paging :" Width="112px"></asp:Label>
                            </td>
                            <td style="width: 133px; text-align: left;">
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked="True" />
                            </td>
                            <td style="width: 133px; text-align: left;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 47px; text-align: left">
                                <asp:Label ID="FromDateLabel" runat="server" Font-Bold="False" Font-Names="Centaur"
                                    ForeColor="#00C0C0" Height="17px" Text="FromDate :" Width="112px"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 133px;">
                                <asp:TextBox ID="FromDateTextBox" runat="server" Width="100px"></asp:TextBox>
                                <cc1:CalendarExtender ID="FromDateTextBox_CalendarExtender" runat="server" CssClass="cal_KimTheme"
                                    Enabled="True" Format="dd/MM/yyyy" OnClientShowing="CurrentDateShowing" TargetControlID="FromDateTextBox">
                                </cc1:CalendarExtender>
                                <asp:Label ID="Label14" runat="server" Font-Names="Times New Roman" ForeColor="#FF3300"
                                    Text="*"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 133px;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <asp:Label ID="ToDateLabel" runat="server" Font-Bold="False" Font-Names="Centaur"
                                    ForeColor="#00C0C0" Height="17px" Text="ToDate :" Width="112px"></asp:Label>
                            </td>
                            <td style="width: 133px; text-align: left;">
                                <asp:TextBox ID="ToDateTextBox" runat="server" Width="100px"></asp:TextBox>
                                <cc1:CalendarExtender ID="ToDateTextBox_CalendarExtender" runat="server" CssClass="cal_KimTheme"
                                    Enabled="True" Format="dd/MM/yyyy" OnClientShowing="CurrentDateShowing" TargetControlID="ToDateTextBox">
                                </cc1:CalendarExtender>
                                <asp:Label ID="Label15" runat="server" Font-Names="Times New Roman" ForeColor="#FF3300"
                                    Text="*"></asp:Label>
                            </td>
                            <td style="width: 133px; text-align: left;">
                                &nbsp;
                            </td>
                        </tr>
                       <tr>
                            
                            <td class="text-center">
                                <asp:Button ID="Button2" runat="server" CssClass="btn btn-default btn-sm" Text="Refresh" />
                            </td>
                            
                        </tr>
            </table>
               --%> 

            <div class="row">
                <div class="col-lg-10">
                <div class="input-group">
                    <span class="input-group-btn">
                    <asp:Button ID="Button1" Text="Search" CssClass="btn btn-default" runat="server" OnClick="Search" />
                    </span>
                         <asp:TextBox ID="txtSearch" CssClass="form-control" runat="server" />
                </div><!-- /input-group -->
                </div><!-- /.col-lg-6 -->
                <div class="col-lg-2">
                <div class="input-group">
                    <asp:Button ID="Button2" runat="server" CssClass="btn btn-default" Text="Refresh" />
                </div><!-- /input-group -->
                </div><!-- /.col-lg-6 -->
            </div><!-- /.row -->



<%--            <div id="div1">
            </div>
            <asp:Label ID="Label17" runat="server" Font-Bold="True" ForeColor="#33CCCC" Font-Size="Smaller"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label19" runat="server" Font-Bold="True" Font-Size="Smaller" ForeColor="#33CCCC"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label20" runat="server" Font-Bold="True" Font-Size="Smaller" ForeColor="#33CCCC"></asp:Label>--%>
            
            <asp:GridView ID="GridView1"  CssClass="table table-bordered table-condensed" HeaderStyle-ForeColor="White" runat="server"  AutoGenerateColumns="false" BorderStyle="None" CellPadding="0" HorizontalAlign="Center" PageSize="25" AllowPaging="True" EnableModelValidation="True">
                <RowStyle HorizontalAlign="Center" />
               
                <FooterStyle BorderWidth="3px" />
                
                <Columns>

                    <asp:BoundField ItemStyle-ForeColor="#ffffff" HeaderText="ID" 
                     DataField="ID" 
                     SortExpression="ID"></asp:BoundField>

                    <asp:BoundField HeaderText="Name" 
                     DataField="Name" 
                     SortExpression="Name"></asp:BoundField>

                    <asp:BoundField HeaderText="EmailID" 
                     DataField="EmailID" 
                     SortExpression="EmailID"></asp:BoundField>

                    <asp:BoundField HeaderText="Mobile" 
                     DataField="MobileNo" 
                     SortExpression="MobileNo"></asp:BoundField>

                    <asp:BoundField HeaderText="Message" 
                     DataField="Msg" 
                     SortExpression="Msg"></asp:BoundField>

                    <asp:BoundField HeaderText="Date" 
                     DataField="Datetime" 
                     DataFormatString = "{0:dd/MM/yyyy}"
                     SortExpression="Datetime"></asp:BoundField>

                    <%-- <asp:BoundField HeaderText="Status" 
                     DataField="Status" 
                     SortExpression="Status">
                    </asp:BoundField>--%>
                    
                   <%--  <asp:TemplateField>
                        <HeaderTemplate>
                            Status
                            <asp:DropDownList ID="ddlCountry" runat="server"
                             AutoPostBack = "true"
                            AppendDataBoundItems = "true">
                            <asp:ListItem Text = "All" Value = "all"></asp:ListItem>
                            <asp:ListItem Text = "Pending" Value = "pending"></asp:ListItem>
                            <asp:ListItem Text = "Follow Up" Value = "follow"></asp:ListItem>
                            <asp:ListItem Text = "Closed" Value = "closed"></asp:ListItem>
                            </asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkShow" NavigateUrl="#"  Text="Details" Font-Bold="true" runat="server" onClick="ShowRowDetail();" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>

                    <asp:TemplateField>
                        <HeaderTemplate>
                            Status
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkView" Text="View" NavigateUrl="javascript:;" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                   <%--  <asp:HyperLinkField DataNavigateUrlFields="ID" 
                        
                    DataNavigateUrlFormatString="/MemberPages/profile.aspx?ID={0}"
                    NavigateUrl="#"
                    DataTextField="Status" 
                    HeaderText="Status" 
                    SortExpression="Name" 
                    ItemStyle-Width="100px"
                    ItemStyle-Wrap="true" />--%>
                </Columns>
                </asp:GridView>

                <asp:LinkButton ID="lnkDummy"  Text="Status" runat="server" />
                <cc1:ModalPopupExtender ID="ModalPopupExtender1" BehaviorID="mpe" runat="server"
                PopupControlID="pnlPopup" TargetControlID="lnkDummy" CancelControlID="lnkHide"
                BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
                Status
                <div class="body">
                    <asp:GridView ID="GridView2" CssClass="table table-bordered" AutoGenerateColumns="False" runat="server" CellPadding="0" HorizontalAlign="Center" PageSize="25" AllowPaging="True" EnableModelValidation="True">
                <RowStyle HorizontalAlign="Center" />
               
                <FooterStyle BorderWidth="5px" />
                
                <Columns>

                    <asp:BoundField HeaderText="ID" 
                     DataField="ID" 
                     SortExpression="ID"></asp:BoundField>

                    <asp:BoundField HeaderText="C. ID" 
                     DataField="ComplaintID" 
                     SortExpression="ComplaintID"></asp:BoundField>

                     <asp:BoundField HeaderText="Status" 
                     DataField="Status" 
                     SortExpression="Status"></asp:BoundField>

                     <asp:BoundField HeaderText="Comments" 
                     DataField="Comments" 
                     SortExpression="Comments"></asp:BoundField>

                     <asp:BoundField HeaderText="Processed By" 
                     DataField="Processed_By" 
                     SortExpression="Processed_By"></asp:BoundField>

                  
                </Columns>
                </asp:GridView>
                    <br />
                    <asp:HyperLink ID="HyperLink1" CssClass="btn btn-default btn-xs" NavigateUrl="#" Text="Update" runat="server" />
                    <asp:HyperLink ID="lnkHide" CssClass="btn btn-warning btn-xs" NavigateUrl="#" Text="Close" runat="server" />
                    <br />
                </div>
                </asp:Panel>

        </ContentTemplate>

    </asp:UpdatePanel>
   
</asp:Content>