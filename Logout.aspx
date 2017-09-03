<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Logout.aspx.vb" Inherits="Logout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <style>
        body {
              min-height: 100px;
              padding-top: 70px;
              margin-bottom: 60px;
            }
            html {
                position: relative;
                min-height: 100%;
            }
        
            .footer {
                position: absolute;
                bottom: 0;
                width: 100% auto;

                /* Set the fixed height of the footer here */
                height: 60px;
          
            }
        .jumbotron
        {
            margin-top: 100px;
        }
    </style>
</head>
<body>
    <div class="container">
    <form id="form1" runat="server">
    

      <!-- The justified navigation menu is meant for single line per list item.
           Multiple lines will require custom code not provided by Bootstrap. -->
      <div class="masthead">
        <h3 class="text-muted">Pay-it Support</h3>
      </div>

      <!-- Jumbotron -->
      <div class="jumbotron">
        <h1>Thank you for using Pay-it!</h1>
          <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-lg btn-primary" Text="Login Again" OnClick="btnLogin_Click" />
      </div>

      <!-- Site footer -->
        <footer class="footer">
          <div class="container">
            <p class="text-muted text-center">&copy <%=DateTime.Now.Year %> <a href="http://isys.mobi">iSYS</a>. All Rights Reserved.</p>
          </div>
    </footer>

        </form>
    </div> <!-- /container -->


    <!-- IE10 viewport hack for Surface/desktop Windows 8 bug -->
    <script src="../../assets/js/ie10-viewport-bug-workaround.js"></script>
    <script src="Scripts/jQuery-1.11.3.js"></script>

    <script src="Scripts/main.js"></script>
</body>
</html>
