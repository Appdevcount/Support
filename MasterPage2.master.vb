Imports System.Net

Partial Class MasterPage2
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Label2.Text = Session("user").ToString.ToUpperInvariant()
    End Sub
End Class

