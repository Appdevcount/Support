<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Loginnew.aspx.vb" Inherits="Loginnew" %>

<!DOCTYPE html>

<html lang="en-Us">
<head>
  <meta charset="utf-8">
  <title>Payit Support</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Stylesheet/forms.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.4.1.min.js"></script>
    <script src="Scripts/alertify.js"></script>
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
    <!--[if lt IE 9]>
		<script src="http://html5shiv.googlecode.com/svn/trunk/html5.js"></script>
	<![endif]-->
    <style type="text/css">
        @charset "utf-8";
        /* CSS Document */
        @import url(http://weloveiconfonts.com/api/?family=fontawesome);
        /* ---------- ERIC MEYER'S RESET CSS ---------- */
        /* ---------- http://meyerweb.com/eric/tools/css/reset/ ---------- */

        @import url(http://meyerweb.com/eric/tools/css/reset/reset.css);
        /* ---------- FONTAWESOME ---------- */

        [class*="fontawesome-"]:before {
            font-family: 'FontAwesome', sans-serif;
        }
        /* ---------- GENERAL ---------- */

        body {
            background: #f4f4f4;
            color: #605ca8;
            font: 100%/1.5em sans-serif;
            margin: 0;
        }

        a {
            text-decoration: none;
        }

        h1 {
            font-size: 1em;
        }

        h1,
        p {
            margin-bottom: 10px;
        }

        strong {
            font-weight: bold;
        }

        .uppercase {
            text-transform: uppercase;
        }
        /* ---------- LOGIN ---------- */

        #login {
            padding-top: 100px;
            margin: 50px auto;
            width: 300px;
        }

        form fieldset input[type="text"],
        input[type="password"] {
            background: #e5e5e5;
            border: none;
            border-radius: 3px;
            color: #5a5656;
            font-family: inherit;
            font-size: 14px;
            height: 50px;
            outline: none;
            padding: 0px 10px;
            width: 280px;
            -webkit-appearance: none;
        }

        form fieldset input[type="submit"] {
            background-color: #008dde;
            border: none;
            border-radius: 3px;
            color: #f4f4f4;
            cursor: pointer;
            font-family: inherit;
            height: 50px;
            text-transform: uppercase;
            width: 300px;
            -webkit-appearance: none;
        }

        form fieldset a {
            color: #5a5656;
            font-size: 10px;
        }

        form fieldset a:hover {
            text-decoration: underline;
        }

        .btn-round {
            background: #5a5656;
            border-radius: 50%;
            color: #f4f4f4;
            display: block;
            font-size: 12px;
            height: 50px;
            line-height: 50px;
            margin: 30px 125px;
            text-align: center;
            text-transform: uppercase;
            width: 50px;
        }
        .footer {
            position: absolute;
            bottom: 0;
            text-align: center; 
            width: 100%;
            /* Set the fixed height of the footer here */
            height: 60px;
            background-color: #6C7A89;
        }
        
        .profile-img-card {
            width: 70px;
            height: 70px;
            margin: 0 auto 10px;
            display: block;
        }
    </style>
</head>
<body>
  <div class="container" id="login">
       <img id="profile-img" class="profile-img-card" src="images/logoOneG.png" />
    <h1><strong>Welcome to Payit Support</strong></h1>
    <form id="form1" runat="server">
      <fieldset>
        <p>
          <input type="text" runat="server" id="txtUser" required  placeholder="Username">
        </p>
        <!-- JS because of IE support; better: placeholder="Username" -->

        <p>
          <input type="password" runat="server" id="txtPass" required placeholder="Password">
        </p>
        <!-- JS because of IE support; better: placeholder="Password" -->

        
          
        
        <p>
            <asp:Button ID="btnLogin" runat="server" Text="Login" />
         <%-- <input type="submit" id="btnLogin" runat="server" onclick="btnLogin_Click" value="Login">--%>
        </p>
          <p class="pull-right"><asp:LinkButton runat="server" CssClass="btn btn-link" ID="forgetLink" Text=""></asp:LinkButton></p>
      </fieldset>

    </form>
      
  </div>
  <!-- end login -->
     <footer class="footer text-center">
      <div class="container">
        <p class="text-center">Copyright &copy; <%=DateTime.Now.Year%> by <strong> <a href="http://isys.mobi">iSYS</a>.</strong> All rights reserved.</p>
      </div>
    </footer>
    <script src="Scripts/alertify.js"></script>
    <script src="Scripts/main.js"></script>
</body>

</html>


