<%@ Page Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="Balance.aspx.vb" Inherits="Balance" title="Pay-it Support-Balance" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript">
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
 div1.innerHTML="<img src=loading_black.gif> <font Color=#00C0C0>Loading..</font>"//gallery-loading
 div1.style .width ="50"
 //img12.style.display="block" 
}
    </script>

            <asp:Label ID="Label8" runat="server" Font-Bold="False" 
    Font-Size="Larger" Font-Underline="False"
                ForeColor="White" Style="text-align: center" Text="Stock Balance"
                Font-Names="Times New Roman"></asp:Label>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1"  runat="server">
     <Triggers>
              <asp:PostBackTrigger ControlID="Button2" />
              </Triggers>
        <ContentTemplate>
            <br />
            <table>
                <tr>
                    <td style="width: 47px; text-align: left">
                        <asp:Label ID="Label9" runat="server" Font-Bold="False" Font-Names="Centaur" ForeColor="#00C0C0"
                            Height="17px" Text="ServiceType :" Width="112px"></asp:Label>
                    </td>
                    <td style="text-align: left; width: 133px;">
                        <asp:DropDownList ID="ddlService" runat="server" AutoPostBack="True" Width="150px">
                           
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: left; ">
                        &nbsp;
                    </td>
                    <tr>
                        <td style="text-align: right">
                            &nbsp;
                        </td>
                        <td style="text-align: left; ">
                            <asp:Button ID="Button2" runat="server" Style="text-align: left; height: 26px;" 
                                Text="GetBalance"  />
                        </td>
                        <td style="text-align: center; ">
                            &nbsp;
                        </td>
                        </table>
            <div id="div1">
            </div>
            <asp:Label ID="Label17" runat="server" Font-Bold="True" ForeColor="#33CCCC" Font-Size="Smaller"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label19" runat="server" Font-Bold="True" Font-Size="Smaller" ForeColor="#33CCCC"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label20" runat="server" Font-Bold="True" Font-Size="Smaller" ForeColor="#33CCCC"></asp:Label>
            <asp:GridView ID="GridView1" runat="server" BorderColor="DimGray" BorderStyle="Outset"
                BorderWidth="2px" CellPadding="0" Font-Bold="True" Font-Size="Smaller" ForeColor="White"
                HorizontalAlign="Center" PageSize="50">
                <RowStyle HorizontalAlign="Center" Font-Names="Times New Roman" />
               
                <FooterStyle BorderWidth="5px" />
                <HeaderStyle BorderStyle="Dotted" ForeColor="#00C0C0" />
                <AlternatingRowStyle BackColor="#666666" BorderColor="#666666" Font-Bold="False"
                    ForeColor="White" BorderWidth="1px" />
                     
            </asp:GridView>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
              <br />

             <asp:Label ID="MConnectBalance" runat="server"  Font-Bold="True" ForeColor="#33CCCC" Font-Size="Smaller"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            
             <br />

            <br />
           <%-- <asp:Label ID="Label21" runat="server" Font-Bold="True" Font-Size="Smaller" 
                ForeColor="#33CCCC">Available Pins :</asp:Label>--%>
            <asp:GridView ID="GridView2" runat="server" BorderColor="DimGray" 
                BorderStyle="Outset" BorderWidth="2px" CellPadding="0" Font-Bold="True" 
                Font-Size="Smaller" ForeColor="White" AutoGenerateColumns="true" HorizontalAlign="Center" PageSize="50">
                <RowStyle Font-Names="Times New Roman" HorizontalAlign="Center" />
                <FooterStyle BorderWidth="5px" />
                <HeaderStyle BorderStyle="Dotted" ForeColor="#00C0C0" />
                <AlternatingRowStyle BackColor="#666666" BorderColor="#666666" 
                    BorderWidth="1px" Font-Bold="False" ForeColor="White" />
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdatePanel ID="UpdatePanel2"  runat="server">
     <Triggers>
              <asp:PostBackTrigger ControlID="Button1" />
              </Triggers>
        <ContentTemplate>
            <br />
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:payitConnectionActive %>"
                SelectCommand="SELECT distinct([knetprocess]) FROM [ThirdParty_knet_trans] where knetprocess is not null ORDER BY [knetprocess]">
            </asp:SqlDataSource>
            <table>
                <tr>
                    <td style="width: 47px; text-align: left">
                        <asp:Label ID="Label2" runat="server" Font-Bold="False" Font-Names="Centaur" ForeColor="#00C0C0"
                            Height="17px" Text="Service:" Width="112px"></asp:Label>
                    </td>
                    <td style="text-align: left; width: 133px;">
                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" Width="150px">
                            <asp:ListItem>Zain</asp:ListItem>
                            <asp:ListItem>VIVA-X</asp:ListItem>
                            <asp:ListItem>VIVA-P</asp:ListItem>
                            <asp:ListItem>WATANIYA-X</asp:ListItem>
                            <asp:ListItem>WATANIYA-P</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <tr>
                        <td style="width: 47px; text-align: left">
                            <asp:Label ID="Label3" runat="server" Font-Bold="False" Font-Names="Centaur" ForeColor="#00C0C0"
                                Height="17px" Text="Amount:" Width="112px"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 133px;">
                            <asp:TextBox ID="TextBox1" runat="server" Width="145px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 47px; text-align: left">
                            <asp:Label ID="Label1" runat="server" Font-Bold="False" Font-Names="Centaur" ForeColor="#00C0C0"
                                Height="17px" Text="Reason:" Width="112px"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 133px;">
                            <asp:TextBox ID="txtReason" runat="server" Width="145px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">&nbsp;
                        </td>
                        <td style="text-align: left;">
                            <asp:Button ID="Button1" runat="server"
                                Style="text-align: left; height: 26px; margin-bottom: 0px;" Text="Add Balance"
                                Width="87px" />
                        </td>
                    </tr>
            </table>
         
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
