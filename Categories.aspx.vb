Imports System.Data
Imports System.Data.SqlClient

Public Class Categories
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim da As SqlDataAdapter
    Dim ds, ds1 As DataSet
    Dim sql As String
    Dim s As String
    Dim constring As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "qa") Then
            alert("Unauthorized Access")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=Categories.aspx")
        End If
    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If Not (Session("role") = "superadmin" Or Session("role") = "operations") Then
            alert("Unauthorized Access")
            Exit Sub
        End If
        If Page.IsValid Then
            If (catName.Text = "") Then
                alert("Category Name Cannot be Empty")
            Else
                Dim catStat As String
                If chkStatus.Checked = True Then
                    catStat = 1
                Else
                    catStat = 0
                End If
                Using con As New SqlConnection(constring)
                    Using cmd As New SqlCommand("INSERT INTO PayitCategories (CategoryName,CreatedDate,Status,CategoryInfo,Priority) VALUES ('" & catName.Text & "',GETDATE()," & catStat & ",'" & catInfo.Text & "', (SELECT MAX(CONVERT(int, Priority))+1 from PayitCategories))")
                        Using sda As New SqlDataAdapter()
                            cmd.Connection = con
                            sda.SelectCommand = cmd
                            con.Open()
                            Dim insert As Integer = cmd.ExecuteNonQuery()
                            Using dt As New DataTable()
                                sda.Fill(dt)
                                GridView1.DataBind()
                            End Using
                            If (insert = 1) Then
                                alert("Category Added")
                            Else
                                alert("Error. Try Again")
                            End If
                            con.Close()
                        End Using
                    End Using
                End Using
            End If
        End If
    End Sub
    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Label2 As Label = DirectCast(e.Row.FindControl("Label4"), Label)
        End If
    End Sub
    Protected Sub GridView1_DataBound(sender As Object, e As EventArgs)
    End Sub
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("categories.xls", Me.GridView1)
    End Sub
End Class
