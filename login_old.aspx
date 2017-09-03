<%@ Page Language="VB" AutoEventWireup="false" CodeFile="login_old.aspx.vb" Inherits="login_old" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Pay-it Support-Login</title>
    <script src="Scripts/jquery-1.4.1.min.js"></script>
    <script src="Scripts/alertify.js"></script>
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />

        <style type="text/css">
            body
            {
                font-family: sans-serif;
            }
            .style2
            {
                height: 85px;
            }
            .style3
            {
                height: 340px;
            }
        </style>
</head>

<body bgcolor="#000000" style="background-repeat:no-repeat; margin-top: 50px; margin-bottom: 40px;" >
    <form id="form1" runat="server">
<table width="795" height="508" border="0"   background="loginpage2.png" cellspacing="0" cellpadding="0" align="center" style=" vertical-align:middle; background-repeat:no-repeat">
   <tr>
    <td class="style2" align="center" colspan="2">&nbsp;</td>
  </tr>
  <tr >
    <td align="center" class="style3" colspan="2"> 
        <table style="width: 30%;">
            <tr>
                <td>
                   <asp:Label ID="Label3" runat="server" Font-Bold="True" 
            Font-Names="Times New Roman" ForeColor="White" Text="UserName:"></asp:Label>
                    
                </td>
                <td align="left">
                    <asp:TextBox ID="txtUser" runat="server" Width="100px"></asp:TextBox>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
                    ControlToValidate="txtUser"
                    Display="Dynamic" 
                    ErrorMessage="Username Cannot be empty." 
                    runat="server" />
                    <asp:RegularExpressionValidator ID="revUserID"
                    runat="server" ErrorMessage="AlphaNumeric Only"
                        ValidationExpression="^([\S\s]{0,16})$"  ControlToValidate="txtUser"
                        Display="Dynamic"></asp:RegularExpressionValidator>
                </td>
               
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" 
            Font-Names="Times New Roman" ForeColor="White" Text="Password :"></asp:Label>
                </td>
                <td align="left">
                     <asp:TextBox ID="txtPass" runat="server" TextMode="Password" Width="100px"></asp:TextBox>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" 
                    ControlToValidate="txtPass"
                    Display="Dynamic" 
                    ErrorMessage="Password be empty." 
                    runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td align="left">
                    <asp:ImageButton ID="ImageButton1" runat="server" Height="50px" 
                        ImageUrl="~/login_icon.png" Width="50px" />
                </td>
            </tr>
        </table>
    
</td>
  </tr>
  <tr>
<td>&nbsp;</td>
<td>&nbsp;</td>
  </tr>
 
</table>

    </form>

</body>
</html>
