
Partial Class index
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("user") = "" Then
            Response.Redirect("Login.aspx")
        End If
    End Sub
End Class
