<%@ Page Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="EnetSupport.aspx.vb" Inherits="EnetSupport" title="Pay-it Support-LocalTransactionDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <link href="Stylesheet/forms.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
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
    </style>
    <div style="text-align: right">
        <asp:LinkButton ID="LinkButton1" runat="server" Font-Names="Centaur"
            ForeColor="#00C3C6">Export Report To Excel</asp:LinkButton>
    </div>
    <asp:Label ID="Label8" runat="server" Font-Bold="False" Font-Size="Larger"
        Font-Underline="False" ForeColor="White" Style="text-align: center"
        Text="Particular Local Transaction Details "
        Font-Names="Times New Roman"></asp:Label>
  
    <br />

    <br />
    <table style="border-color: Aqua; border-style: none">
        <tr align="center">
            <td style="width: 47px; text-align: left">
                <asp:Label ID="Label3" runat="server" Font-Bold="False" Font-Names="Centaur"
                    ForeColor="#00C0C0" Height="17px" Text="Data Source:" Width="112px"></asp:Label>
            </td>
            <td style="text-align: left;">
                <asp:DropDownList ID="DropDownList3" runat="server" OnSelectedIndexChanged="DropDownList3_SelectedIndexChanged" AutoPostBack="True"
                    Width="160px">
                    <asp:ListItem>ThirdParty_knet_trans</asp:ListItem>
                    <asp:ListItem>ThirdParty_knet_trans_20160420</asp:ListItem>
                    <asp:ListItem>ThirdParty_knet_trans_20160324</asp:ListItem>
                    <asp:ListItem>ThirdParty_knet_trans_WithPartitions</asp:ListItem>
                    <asp:ListItem>ThirdParty_knet_trans_20151231</asp:ListItem>
                    <asp:ListItem>ThirdParty_knet_trans_20150729_20150920</asp:ListItem>
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
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 47px; text-align: left">
                <asp:Label ID="Label4" runat="server" Font-Bold="False" Font-Names="Centaur"
                    ForeColor="#00C0C0" Height="17px" Text="Mobile Number :" Width="112px"></asp:Label>
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="TextBox1" runat="server" Width="150px"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label11" runat="server" Font-Names="Times New Roman"
                    Font-Size="X-Small" ForeColor="#009933" Text="Optional"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: left">
                <asp:Label ID="Label7" runat="server" Font-Bold="False" Font-Names="Centaur"
                    ForeColor="#00C0C0" Height="17px" Text="Track ID :" Width="112px"></asp:Label>
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label12" runat="server" Font-Names="Times New Roman"
                    Font-Size="X-Small" ForeColor="#009933" Text="Optional"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: left">
                <asp:Label ID="Label1" runat="server" Font-Bold="False" Font-Names="Centaur"
                    ForeColor="#00C0C0" Height="17px" Text="Payment ID :" Width="112px"></asp:Label>
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" Font-Names="Times New Roman"
                    Font-Size="X-Small" ForeColor="#009933" Text="Optional"></asp:Label>
            </td>
        </tr>
        <tr>
            <div id="trPush" runat="server">
                <td style="text-align: left">
                    <asp:Label ID="lblPush" runat="server" Font-Bold="False" Font-Names="Centaur"
                        ForeColor="#00C0C0" Height="17px" Text="Push to Table:" Width="112px"></asp:Label>
                </td>
                <td style="width: 133px; text-align: left;">
                    <asp:CheckBox ID="chkPush" onchange="valueChanged()" runat="server" Checked="false" />
                </td>
                <td style="width: 133px; text-align: left;">
                    
                </td>

            </div>
        </tr>
   
        <tr>
            <td style="text-align: right">&nbsp;</td>
            <td style="text-align: center;">
                <asp:Button ID="Button2" runat="server" Text="Display" CssClass="btn btn-default" Style="text-align: left" />
            </td>
        </tr>

    </table>
   <%-- <table>
        <tr>
            <td>
                <span runat="server" ID="warnPush" class="badge text-danger" style="font-size:small;background-color:#fff;border-radius:2px;margin-right:-18px;"> This will move to ThirdParty_knet_trans</span>
            </td>
        </tr>
    </table>--%>
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
    <asp:GridView ID="GridView1" runat="server"
        BorderColor="DimGray" OnRowDataBound="GridView1_RowDataBound" CssClass="table table-bordered table-responsive" HeaderStyle-ForeColor="White"
        CellPadding="0" HorizontalAlign="Center">
         <PagerStyle Font-Size="Larger" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
        <HeaderStyle BackColor="Gray" Font-Bold="True" HorizontalAlign="Left" />
        <RowStyle HorizontalAlign="Left" />
    </asp:GridView>
    <asp:GridView ID="GridView2" runat="server"
        CssClass="table table-bordered table-responsive" HeaderStyle-ForeColor="White"
        CellPadding="0" AutoGenerateColumns="true">
        <PagerStyle Font-Size="Larger" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
        <HeaderStyle BackColor="Gray" Font-Bold="True" HorizontalAlign="Left" />
        <RowStyle HorizontalAlign="Left" />
        <Columns>
           <%-- <asp:TemplateField ItemStyle-Width="30px" HeaderText="Action">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkEdit" ForeColor="#009900" runat="server" Text="Edit" OnClick="Edit"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>--%>
          <%--  <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" /> --%>
           <%-- <asp:BoundField DataField="PIN" HeaderText="PIN" SortExpression="PIN" />
            <asp:BoundField DataField="Serial" HeaderText="Serial" SortExpression="Serialout" />
            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
            <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />--%>
             <%--<asp:TemplateField HeaderText="Status" SortExpression="Status">
                <ItemTemplate><%#IIf(Boolean.Parse(Eval("Status").ToString()), "Active", "Inactive")%></ItemTemplate>
            </asp:TemplateField>--%>
            <%--<asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
            <asp:BoundField DataField="Service" HeaderText="Service" SortExpression="Service" />
            <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />--%>
            <%--<asp:BoundField DataField="Status" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="Status" SortExpression="Status" />
           --%>
        </Columns>
        </asp:GridView>
         <%--   <asp:Panel ID="pnlAddEdit" runat="server" CssClass="modalPopup">
                <div class="panel-body">
                <div class="form-horizontal">
                    <fieldset>
                        <p>Edit Status</p>
                        <label class="hiddencol">
                            <div>
                                <asp:Label ID="Label5" runat="server" Text="ID"></asp:Label>
                            </div>
                            <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
                        </label>
                        <label>
                            <div>
                                <asp:Label ID="Label17" runat="server" Text="Status"></asp:Label>
                            </div>
                            <asp:DropDownList runat="server" ID="ddlStatusEdit">
                            </asp:DropDownList>
                        </label>
                    </fieldset>
                    <div class="panel-footer" style="border: none;background-color:#1C1E22;">
                        <div class="submit-wrapper">
                            <asp:Button ID="btnSave" CssClass="btn btn-success btn-sm pull-right" runat="server" Text="Save" OnClick="Save" />
                            <asp:Button ID="btnCancel" CssClass="btn btn-default btn-sm pull-left" runat="server" Text="Cancel" OnClientClick="return Hidepopup()" />
                        </div>
                    </div>
                </div>
                </div>
            </asp:Panel>
            <center>
                <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
                <cc1:ModalPopupExtender ID="popup" runat="server" DropShadow="false"
                    PopupControlID="pnlAddEdit" TargetControlID="lnkFake"
                    BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
            </center>--%>
        </ContentTemplate>
           <Triggers>
                <asp:AsyncPostBackTrigger ControlID="GridView1" />
                <asp:AsyncPostBackTrigger ControlID="GridView2" />
            </Triggers>
    </asp:UpdatePanel>
</asp:Content>

