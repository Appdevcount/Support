<%@ Page Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="Services.aspx.vb" Inherits="Services" title="Untitled Page" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


<div style="text-align: right">
            <asp:LinkButton ID="LinkButton1" runat="server" Font-Names="Centaur" 
                ForeColor="#00C3C6">Export Report To Excel</asp:LinkButton>
        </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1"  runat="server">
     <Triggers>
              <asp:PostBackTrigger ControlID="GridView1" />
              </Triggers>
        <ContentTemplate>
        
            <asp:Label ID="Label8" runat="server" Font-Bold="False" Font-Size="Larger" Font-Underline="False"
                ForeColor="White" Style="text-align: center" Text="Services Information"
                Font-Names="Times New Roman"></asp:Label>
            <br />
            <br />

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
                     <Columns>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


