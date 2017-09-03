<%@ Page Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="UpdateComplaint.aspx.vb" Inherits="UpdateComplaint" title="Pay-it Support-IntlComplaint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <center style="text-align: center" >
         &nbsp;</center>
    <center style="text-align: center">
             <asp:Label ID="Label1" runat="server" Font-Size="X-Large" ForeColor="White" Text="Update Complaint"
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
                     <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    CellPadding="4" Font-Size="Small" ForeColor="#39697B" 
            GridLines="None" DataKeyNames="ID" DataSourceID="SqlDataSource1">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="Smaller" />
                         <Columns>
                             <asp:CommandField ShowEditButton="True" />
                             <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" 
                                 ReadOnly="True" SortExpression="ID"   />
                             <asp:BoundField DataField="TopUpTransID" HeaderText="TopUpTransID" 
                                 SortExpression="TopUpTransID"  ReadOnly="true"/>
                             <asp:BoundField DataField="DestMobileNumber" HeaderText="DestMobile" 
                                 SortExpression="DestMobileNumber"  ReadOnly="true"/>
                             <asp:BoundField DataField="TrackID" HeaderText="TrackID" 
                                 SortExpression="TrackID"  ReadOnly="true"/>
                             <asp:BoundField DataField="TransactionDate" HeaderText="TranDate" 
                                 SortExpression="TransactionDate"  ReadOnly="true"/>
                             <asp:BoundField DataField="ComplaintDate" HeaderText="ComplaintDate" 
                                 SortExpression="ComplaintDate"  ReadOnly="true"/>
                             <asp:BoundField DataField="TransferToComplaintResponse" 
                                 HeaderText="ComplaintResponse" 
                                 SortExpression="TransferToComplaintResponse" />
                             <asp:BoundField DataField="Status" HeaderText="Status" 
                                 SortExpression="Status" />
                         </Columns>
                    <FooterStyle BackColor="Black" Font-Bold="True" ForeColor="White" 
                             Font-Size="Smaller" />
                    <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" 
                             Font-Size="Smaller" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" 
                             Font-Size="Smaller" />
                    <EditRowStyle BackColor="#39697B" Font-Size="X-Small" ForeColor="Black" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#39697B" />
                </asp:GridView>
        
         <br />
        <asp:GridView ID="GridView2" runat="server" AllowPaging="True" 
            AutoGenerateColumns="False" CellPadding="4" DataKeyNames="ID" 
            DataSourceID="SqlDataSource2" Font-Size="Small" ForeColor="#39697B" 
            GridLines="None">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="Smaller" />
            <Columns>
                <asp:CommandField ShowEditButton="True" />
                <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" 
                    ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="TopUpTransID" HeaderText="TopUpTransID" 
                    SortExpression="TopUpTransID" ReadOnly="true" />
                <asp:BoundField DataField="DestMobileNumber" HeaderText="DestMobile" 
                    SortExpression="DestMobileNumber"  ReadOnly="true"/>
                <asp:BoundField DataField="TrackID" HeaderText="TrackID" 
                    SortExpression="TrackID"  ReadOnly="true"/>
                <asp:BoundField DataField="TransactionDate" HeaderText="TranDate" 
                    SortExpression="TransactionDate" ReadOnly="true" />
                <asp:BoundField DataField="ComplaintDate" HeaderText="ComplaintDate" 
                    SortExpression="ComplaintDate"  ReadOnly="true"/>
                <asp:BoundField DataField="TransferToComplaintResponse" 
                    HeaderText="ComplaintResponse" 
                    SortExpression="TransferToComplaintResponse" />
                <asp:BoundField DataField="Status" HeaderText="Status" 
                    SortExpression="Status" />
            </Columns>
            <FooterStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="Black" ForeColor="White" 
                HorizontalAlign="Center" Font-Size="Smaller" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" 
                Font-Size="Smaller" />
                    <EditRowStyle BackColor="#39697B" Font-Size="X-Small" ForeColor="Black" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#39697B" />
        </asp:GridView>
        
         <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
            ConnectionString="<%$ ConnectionStrings:payitConnectionString2 %>" 
              SelectCommand="SELECT * FROM [TopUpComplaints] WHERE ([DestMobileNumber] = @DestMobileNumber) ORDER BY [ID]" 
            
            UpdateCommand="UPDATE [TopUpComplaints] SET [TransferToComplaintResponse] = @TransferToComplaintResponse, [Status] = @Status WHERE [ID] = @original_ID AND (([TransferToComplaintResponse] = @original_TransferToComplaintResponse) OR ([TransferToComplaintResponse] IS NULL AND @original_TransferToComplaintResponse IS NULL)) AND (([Status] = @original_Status) OR ([Status] IS NULL AND @original_Status IS NULL))" 
            ConflictDetection="CompareAllValues" 
            OldValuesParameterFormatString="original_{0}">
             <SelectParameters>
                 <asp:ControlParameter ControlID="TextBox1" Name="DestMobileNumber" PropertyName="Text" 
                     Type="Decimal" />
             </SelectParameters>
             <DeleteParameters>
                 <asp:Parameter Name="original_ID" Type="Int32" />
                 <asp:Parameter Name="original_TopUpTransID" Type="Int32" />
                 <asp:Parameter Name="original_DestMobileNumber" Type="Decimal" />
                 <asp:Parameter Name="original_TrackID" Type="Int64" />
                 <asp:Parameter DbType="Date" Name="original_TransactionDate" />
                 <asp:Parameter DbType="Date" Name="original_ComplaintDate" />
                 <asp:Parameter Name="original_TransferToResponse" Type="String" />
                 <asp:Parameter Name="original_TransferToComplaintResponse" Type="String" />
                 <asp:Parameter Name="original_Status" Type="String" />
             </DeleteParameters>
             <UpdateParameters>
                
               
                 <asp:Parameter Name="TransferToComplaintResponse" Type="String" />
                 <asp:Parameter Name="Status" Type="String" />
                 <asp:Parameter Name="original_ID" Type="Int32" />
                 <asp:Parameter Name="original_TransferToComplaintResponse" Type="String" />
                 <asp:Parameter Name="original_Status" Type="String" />
             </UpdateParameters>
             <InsertParameters>
                 <asp:Parameter Name="TopUpTransID" Type="Int32" />
                 <asp:Parameter Name="DestMobileNumber" Type="Decimal" />
                 <asp:Parameter Name="TrackID" Type="Int64" />
                 <asp:Parameter Name="TransactionDate" DbType="Date" />
                 <asp:Parameter Name="ComplaintDate" DbType="Date" />
                 <asp:Parameter Name="TransferToResponse" Type="String" />
                 <asp:Parameter Name="TransferToComplaintResponse" Type="String" />
                 <asp:Parameter Name="Status" Type="String" />
             </InsertParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:payitConnectionString2 %>" 
               OldValuesParameterFormatString="original_{0}" 
            SelectCommand="SELECT * FROM [TopUpComplaints] WHERE ([TrackID] = @TrackID) ORDER BY [ID]" 
            
            UpdateCommand="UPDATE [TopUpComplaints] SET  [TransferToComplaintResponse] = @TransferToComplaintResponse, [Status] = @Status WHERE [ID] = @original_ID">
            <SelectParameters>
                <asp:ControlParameter ControlID="TextBox1" Name="TrackID" PropertyName="Text" 
                    Type="Int64" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="original_ID" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="TransferToComplaintResponse" Type="String" />
                <asp:Parameter Name="Status" Type="String" />
                <asp:Parameter Name="original_ID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="TopUpTransID" Type="Int32" />
                <asp:Parameter Name="DestMobileNumber" Type="Decimal" />
                <asp:Parameter Name="TrackID" Type="Int64" />
                <asp:Parameter Name="TransactionDate" DbType="Date" />
                <asp:Parameter Name="ComplaintDate" DbType="Date" />
                <asp:Parameter Name="TransferToResponse" Type="String" />
                <asp:Parameter Name="TransferToComplaintResponse" Type="String" />
                <asp:Parameter Name="Status" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>
         &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;</center>
        </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

