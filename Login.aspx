<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" %>
<!DOCTYPE html>
<html class="" lang="en">
    <head>
        <meta charset="utf-8">
        <title>Og Money KW:Support Portal</title>
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <meta content="" name="description" />
        <meta content="Og Money KW Support" name="author" />
       <link rel="shortcut icon" href="assets/images/favicon.png" type="image/png">
        <link href="assets/css/style.css" rel="stylesheet">
        <link href="assets/css/ui.css" rel="stylesheet">
        <link href="assets/plugins/bootstrap-loading/lada.min.css" rel="stylesheet">
    </head>
    <body class="account2" data-page="login">
        <form id ="form1" runat="server">
            <!-- BEGIN LOGIN BOX -->
            <div class="container" id="login-block">
                <i class="user-img icons-faces-users-03"></i>
                <div class="account-info">
                   <a href="login.aspx" class="logo"></a>
			       <h2 style="color: #103663;font-weight: 400;font-size: medium;">Og Money KW Support Portal</h2>
             
                </div>
                <div class="account-form">
                    
                        <h3>Sign-In</h3>
                        <div class="append-icon">
                            <input type="text" name="name" runat="server" id="name" class="form-control form-white username" placeholder="Username" onclick="ImageButton1_Click" required>
                            <i class="icon-user"></i>
                        </div>
                        <div class="append-icon m-b-20">
                         
                            <input type="password" name="password" runat="server" id="txtpassword" class="form-control form-white password" placeholder="Password" required>
                            <i class="icon-lock"></i>
                        </div>
                        <button type="submit" id="btnSubmit" runat="server" class="btn btn-lg btn-dark btn-rounded ladda-button" data-style="expand-left">Sign In</button>
                       <%-- <span class="forgot-password"><a id="forgot" href="account-forgot-password.html">Forgot password?</a></span>--%>
                   
                  <%--  <form class="form-password" role="form">
                        <h3><strong>Reset</strong> your password</h3>
                        <div class="append-icon m-b-20">
                            <input type="password" name="password" class="form-control form-white password" placeholder="Password" required>
                            <i class="icon-lock"></i>
                        </div>
                        <button type="submit" id="submit-password" class="btn btn-lg btn-danger btn-block ladda-button" data-style="expand-left">Send Password Reset Link</button>
                        <div class="clearfix m-t-60">
                            <p class="pull-left m-t-20 m-b-0"><a id="login" href="#">Have an account? Sign In</a></p>
                        </div>
                    </form>--%>
                </div>
			    <br/>
                 <div class="copyright" style="text-align: center;font-size: 13px; color:#fff;">Copyright© <%=DateTime.Now.Year %>, <a href="https://oneglobal.co">Oneglobal.co</a>, All rights reserved.</div>
            </div>
		</form>
        <!-- END LOCKSCREEN BOX -->
        <script src="assets/plugins/jquery/jquery-1.11.1.min.js"></script>
        <script src="assets/plugins/jquery/jquery-migrate-1.2.1.min.js"></script>
        <script src="assets/plugins/gsap/main-gsap.min.js"></script>
        <script src="assets/plugins/bootstrap/js/bootstrap.min.js"></script>
        <script src="assets/plugins/backstretch/backstretch.min.js"></script>
        <script src="assets/plugins/bootstrap-loading/lada.min.js"></script>
        <script src="assets/js/pages/login-v2.js"></script>
    </body>
</html>
