Imports System.Data.Entity
Imports Data

Partial Class mConnectStock
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("user") = "" Or Session("user") <> "Shareef" Or Session("user") <> "Shareef" Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=offers.aspx")
        End If
        If Not IsPostBack Then
            Dim db As New payitEntities
        End If
    End Sub
End Class
