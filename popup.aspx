<%@ Page Language="VB" AutoEventWireup="false" CodeFile="popup.aspx.vb" Inherits="popup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
       
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }

    </style>
</head>
<body>
    <div class="container">
    <form id="form1" runat="server">
    

    <!--<table class="table table-hover">
      <thead>
        <tr>
          <th>Status</th>
          <th>Comments</th>
          <th>Processed By</th>
         
        </tr>
      </thead>
      <tbody>
        <tr>
          <th scope="row"><span id="status"></span></th>
          <td><span id="Comments"></span></td>
          <td><span id="Processed_By"></span></td>
 
        </tr>
      </tbody>
    </table>-->

            
     <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" EnableModelValidation="True">
            <Columns>
                
                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                <asp:BoundField DataField="Comments" HeaderText="Comments" SortExpression="Comments" />
                <asp:BoundField DataField="Processed_By" HeaderText="Processed_By" SortExpression="Processed_By" />
                
            </Columns>
        </asp:GridView>
       
        </form>
        </div>
</body>
</html>
