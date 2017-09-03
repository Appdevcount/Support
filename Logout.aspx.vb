
Partial Class Logout
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Abandon()
        Session("user") = String.Empty
        Session("role") = String.Empty
        Response.Redirect("login.aspx")
    End Sub
    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)
        Response.Redirect("login.aspx")
    End Sub
End Class
