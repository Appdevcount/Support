Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
'Imports System.Web.Mail

Imports System
Partial Class UpdateComplaint
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Then
            Label9.Text = "Invalid TrackID/MobileNumber"
            Exit Sub
        Else
            If DropDownList1.SelectedItem.Text = "TrackID" Then
                GridView1.Visible = True
                GridView2.Visible = False
            Else
                GridView2.Visible = True
                GridView1.Visible = False
            End If

        End If
        
        

    End Sub
End Class
