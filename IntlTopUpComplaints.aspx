<%@ Page Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="IntlTopUpComplaints.aspx.vb" Inherits="IntlTopUpComplaints" title="Pay-it Support-IntlTopUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <center style="text-align: center" >
         &nbsp;</center>
    <center style="text-align: center">
             <asp:Label ID="Label1" runat="server" Font-Size="X-Large" ForeColor="White" Text="Place Complaint"
             Width="329px"></asp:Label>&nbsp;&nbsp;</center>
    <center style="text-align: center">
        <br />
        <asp:Label ID="Label9" runat="server" ForeColor="#CC3300"></asp:Label>
        <br />
         <table>
             <tr>
                 <td style="width: 100px; text-align: center;">
   
                <hr style="width: 457px" />
                     <table>
                         <tr>
                             <td style="width: 100px">
                                 <asp:Label ID="Label8" runat="server" Text="Search On" Width="68px" ForeColor="#F7F6F3"></asp:Label></td>
                             <td style="width: 100px">
                                 <asp:DropDownList ID="DropDownList1" runat="server">
                                <asp:ListItem>TrackID</asp:ListItem>
                                <asp:ListItem>DestNumber</asp:ListItem>
                            </asp:DropDownList></td>
                             <td style="width: 100px">
                            <asp:TextBox ID="TextBox1" runat="server" Width="167px"></asp:TextBox></td>
                             <td style="width: 100px">
                                 <asp:Button ID="Button1" runat="server" Text="Get Details" /></td>
                         </tr>
                     </table>
                <hr style="width: 454px" />
                 </td>
             </tr>
         </table>
                     <asp:GridView ID="GridView1" runat="server"  BorderColor="DimGray" 
                             BorderStyle="Outset" BorderWidth="2px" CellPadding="0" 
                             Font-Bold="True" Font-Names="Calibri" 
                             Font-Size="Smaller" ForeColor="White" HorizontalAlign="Center" 
                             Width="212px" AllowPaging="True" AutoGenerateColumns="False"
                             GridLines="None">
                    <RowStyle HorizontalAlign="Center" />
                    <Columns>
                                 <asp:TemplateField HeaderText="SendMail">
                                                        <ItemTemplate>
                                        <asp:LinkButton runat="server"  CommandName="SendMail" CommandArgument='<%# Eval("myid") %>' ID="LinkButton2" Text='SendMail' />
                                        <%-- OnClick="lnkCustDetails_Click" />--%>

                                    </ItemTemplate>
                                     <ItemStyle BackColor="White" ForeColor="#009999" />
                                </asp:TemplateField>
                        <asp:BoundField DataField="myid" HeaderText="id" />
                        <asp:BoundField DataField="Company" HeaderText="Company" />
                        <asp:BoundField DataField="amt" HeaderText="Amount" />
                        <asp:BoundField DataField="ptype" HeaderText="Purchase" />
                        <asp:BoundField DataField="trackid" HeaderText="TrackId" />
                        <asp:BoundField DataField="knetprocess" HeaderText="Knet Result" >
                            <HeaderStyle Wrap="False" />
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="destmob" HeaderText="Dest Number" >
                            <HeaderStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="topupamt" HeaderText="TopUp Amt" >
                            <HeaderStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="topupresult" HeaderText="TopUp Result" >
                            <HeaderStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="tdatetime" HeaderText="DateTime">
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle BorderWidth="5px" BackColor="#fff" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BorderStyle="Dotted" ForeColor="#666666" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="#666666" BorderColor="#666666" 
                    Font-Bold="False" Font-Names="Calibri" Font-Size="Smaller" ForeColor="White" 
                        BorderWidth="1px"  />
                </asp:GridView>
         &nbsp; &nbsp; &nbsp; &nbsp; 
        &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;</center>
</asp:Content>

