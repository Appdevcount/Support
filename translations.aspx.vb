Imports System.Data.SqlClient
Imports System.Data

Partial Class translations
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim da As SqlDataAdapter
    Dim ds, ds1 As DataSet
    Dim sql As String
    Dim s As String
    Dim constring As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "operations" Or Session("role") = "accounts") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=Services.aspx")
        End If
        If Not Me.IsPostBack Then
            ddlLang.Items.Insert(0, New ListItem("Select", "None"))
            ddlLang.Items.Insert(1, New ListItem("ar", "ar"))
            'ddlLang.Items.Insert(2, New ListItem("en", "en"))
            ddlLang.SelectedIndex = 1

        End If
    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If Page.IsValid Then
            If (txtSource.Text = "" Or txtTranslate.Text = "") Then
                alert("Source and Translated Values Cannot be Empty")
            Else
                Using con As New SqlConnection(constring)
                    Using cmd As New SqlCommand("INSERT INTO Translations (LanguageCode,SourceText,TranslatedText,CreatedDate,Status) VALUES (@langCode,@sourceTxt,@translateTxt,@date,@stat)")
                        cmd.Parameters.AddWithValue("@langcode", ddlLang.SelectedValue)
                        cmd.Parameters.AddWithValue("@sourceTxt", txtSource.Text)
                        cmd.Parameters.AddWithValue("@TranslateTxt", txtTranslate.Text)
                        cmd.Parameters.AddWithValue("@date", DateAndTime.Now)
                        cmd.Parameters.AddWithValue("@stat", 1)

                        Using sda As New SqlDataAdapter()
                            cmd.Connection = con
                            sda.SelectCommand = cmd
                            Using dt As New DataTable()
                                sda.Fill(dt)
                                GridView1.DataBind()
                            End Using
                        End Using
                    End Using
                End Using
                Clear()
                alert("Translation Added")
            End If
        End If
    End Sub
    Protected Sub GridView1_DataBound(sender As Object, e As EventArgs)
    End Sub
    Protected Sub OnDataBound(sender As Object, e As EventArgs)
        Dim row As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal)
        For i As Integer = 0 To GridView1.Columns.Count - 4
            Dim cell As New TableHeaderCell()
            Dim txtSearch As New TextBox()
            txtSearch.Attributes("placeholder") = GridView1.Columns(i).HeaderText
            txtSearch.CssClass = "search_textbox"
            cell.Controls.Add(txtSearch)
            row.Controls.Add(cell)
        Next
        GridView1.HeaderRow.Parent.Controls.AddAt(1, row)
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("Translation.xls", Me.GridView1)
    End Sub
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.success('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Protected Sub Clear()
        txtSource.Text = String.Empty
        txtTranslate.Text = String.Empty
    End Sub
End Class