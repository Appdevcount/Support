Partial Class countryStatus
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "cs" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "qa") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=countryStatus.aspx")
        End If
    End Sub
End Class
