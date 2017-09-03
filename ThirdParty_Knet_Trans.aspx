<%@ Page Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="ThirdParty_Knet_Trans.aspx.vb" Inherits="ThirdParty_Knet_Trans" title="Pay-it Support-Transactions" %>



<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script language ="javascript">
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
 div1.innerHTML="<img src=loading.gif> <font Color=#00C0C0>Loading..</font>"//gallery-loading
 div1.style .width ="50"
 //img12.style.display="block" 
}
</script>
<table width="100%"  border="0">
  <tr>
    <td style="text-align: center; height: 29px; width: 852px;">
        <asp:Label ID="Label2" runat="server" Font-Names="Centaur" 
            Font-Size="XX-Large" Text="Third Party Transactions"
            Width="338px" ForeColor="#00C0C0" Height="33px"></asp:Label><br />
        <hr style="width: 299px" />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
        </td>
  </tr>
    <tr>
        <td style="text-align: right; ">
            <asp:LinkButton ID="LinkButton1" runat="server" ForeColor="#00CCFF">Download 
            Report To Excel Sheet</asp:LinkButton>
            </td>
    </tr>
    <tr>
        <td style="text-align: center; height: 29px; width: 852px;">
            <asp:Label ID="Label9" runat="server" ForeColor="#CC0000"></asp:Label>
            <br />
                <table 100%"="" __designer:mapid="a39" height:="">
                    <tr __designer:mapid="a3a">
                        <td __designer:mapid="a3b" style="width: 47px; text-align: left">
                            <asp:Label ID="Label4" runat="server" Font-Bold="False" Font-Names="Centaur" 
                                ForeColor="#00C0C0" Height="17px" Text="TransID :" Width="112px"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 101px;">
                            <asp:TextBox ID="TextBox1" runat="server" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left" >
                            <asp:Label ID="Label7" runat="server" Font-Bold="False" Font-Names="Centaur" 
                                ForeColor="#00C0C0" Height="17px" Text="From Date :" Width="112px"></asp:Label>
                        </td>
                        <td __designer:mapid="a42" style="width: 101px; text-align: left;">
                            <asp:TextBox ID="TextBox2" runat="server" Width="100px"></asp:TextBox>
                            <cc1:CalendarExtender ID="TextBox2_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="TextBox2" Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr __designer:mapid="a44">
                        <td __designer:mapid="a45" style="width: 47px; text-align: left">
                            <asp:Label ID="Label8" runat="server" Font-Bold="False" Font-Names="Centaur" 
                                ForeColor="#00C0C0" Height="17px" Text=" To Date :" Width="112px"></asp:Label>
                        </td>
                        <td __designer:mapid="a47" style="width: 101px; text-align: left;">
                            <asp:TextBox ID="TextBox3" runat="server" Width="100px"></asp:TextBox>
                            <cc1:CalendarExtender ID="TextBox3_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="TextBox3" Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr __designer:mapid="a49">
                        <td __designer:mapid="a4a" style="text-align: right" colspan="2">
                            <asp:Button ID="Button2" runat="server" Text="Display" />
                        </td>
                    </tr>
                </table>
            <br />
            <asp:GridView ID="GridView1" runat="server" 
                CellPadding="0" Font-Size="Smaller" AllowPaging="True" Width="44px" 
                BackColor="White" BackImageUrl="~/background_page.gif" BorderColor="#CCCCCC" 
                BorderStyle="None" BorderWidth="1px" 
                EnableSortingAndPagingCallbacks="True" Height="36px">
                <RowStyle ForeColor="#33CCCC" BackColor="Black" />
                 <FooterStyle BackColor="Black" ForeColor="White" />
                    <PagerStyle BackColor="White" HorizontalAlign="Left" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#009999" Font-Bold="True" ForeColor="White" />
            </asp:GridView>
            <br />
        </td>
    </tr>
</table>
</asp:Content>


