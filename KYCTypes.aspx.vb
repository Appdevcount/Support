Imports System.Data.SqlClient
Imports System.Data
Partial Class KYCTypes
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sql, sql2, strSQL As String
    Dim dispAmnt, info1 As String
    Dim sqlLocalTrans As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "cs" Or Session("role") = "operations" Or Session("role") = "qa") Then
            alert("Unauthorized Access")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=KYCTypes.aspx")
        End If

        If Not IsPostBack Then
            Me.BindData()
            GridView1.DataBind()
        End If
    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If Not (Session("role") = "superadmin" Or Session("role") = "operations" Or Session("role") = "accounts" Or Session("role") = "qa") Then
            alert("Unauthorized Access")
            Exit Sub
        End If

        If Page.IsValid Then
            Dim cn As SqlConnection = New SqlConnection(strConnString)
            If (txtKYCName.Text = "" Or txtDesc.Text = "") Then
                alert("Please fill all fields!")
            Else
                Dim catStat As Integer
                If chkStatus.Checked = True Then
                    catStat = 1
                Else
                    catStat = 0
                End If
              
                Dim sql1 As String = "SELECT * FROM [payit].[dbo].[KYCTypes] WHERE Name=@name and Description=@desc"
                Dim cmd1 As SqlCommand = New SqlCommand(sql1, cn)
                cmd1.Connection = cn
                cn.Open()
                cmd1.Parameters.AddWithValue("@name", txtKYCName.Text)
                cmd1.Parameters.AddWithValue("@desc", txtDesc.Text)
              
                Using reader As SqlDataReader = cmd1.ExecuteReader()
                    If reader.HasRows Then
                        ' Record already exists
                        alertme("Type Already exists for:<br><b> " & txtKYCName.Text & "</b>")
                    Else
                        ' Record does not exist, add them
                        Dim cmd As SqlCommand = New SqlCommand("INSERT INTO [payit].[dbo].[KYCTypes] ([Name],[Description],[CreatedDate],[Status]) VALUES ('" & txtKYCName.Text & "','" & txtDesc.Text & "',getdate()," & catStat & ")")
                        cmd.Connection = cn
                        Dim insert As Integer = cmd.ExecuteNonQuery()
                        If (insert = 1) Then
                            alert("KYC Type successfully added for:" & txtKYCName.Text & "!")
                        Else
                            alertme("Error! Try Again")
                        End If
                    End If
                End Using
                cn.Close()
                Clear()
                Me.BindData()
            End If
        End If
    End Sub
    Protected Sub Clear()
        txtKYCName.Text = String.Empty
        txtDesc.Text = String.Empty
        chkStatus.Checked = False
    End Sub
    Private Sub BindData()
        Dim strQuery As String = ("select TOP 1000 ID, Name, Description, Status, CreatedDate from [KYCTypes] order by ID desc")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        Me.BindData()
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataBind()
    End Sub
    Public Sub sqldatabind(ByVal s As String, ByVal e As GridViewRowEventArgs, Optional ByVal types1 As String = "", Optional ByVal type As String = "")
        da = New SqlDataAdapter()
        ds = New DataSet()
        da.Fill(ds, "det")
        GridView1.DataSource = ds.Tables("det")
        GridView1.DataBind()
    End Sub
    Private Function GetData(ByVal cmd As SqlCommand) As DataTable
        Dim dt As DataTable = New DataTable
        Dim con As SqlConnection = New SqlConnection(strConnString)
        Dim sda As SqlDataAdapter = New SqlDataAdapter
        cmd.Connection = con
        con.Open()
        sda.SelectCommand = cmd
        sda.Fill(dt)
        Return dt
    End Function
    Protected Sub Edit(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        txtID.Text = row.Cells(0).Text

        If row.Cells(1).Text = "None" Or row.Cells(1).Text = "Select" Or row.Cells(1).Text = "&nbsp;" Or row.Cells(1).Text = "" Then
            txtNameEdit.Text = ""
        Else
            txtNameEdit.Text = row.Cells(1).Text
        End If

        If row.Cells(2).Text = "None" Or row.Cells(2).Text = "Select" Or row.Cells(2).Text = "&nbsp;" Or row.Cells(2).Text = "" Then
            txtDescEdit.Text = ""
        Else
            txtDescEdit.Text = row.Cells(2).Text
        End If

        If row.Cells(3).Text = "True" Then
            chkStatEdit.Checked = True
        Else
            chkStatEdit.Checked = False
        End If
        popup.Show()
        alert("You can now edit")
    End Sub
    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        Dim catStatEdit As Integer
        If (txtNameEdit.Text = "" Or txtDescEdit.Text = "") Then
            alert("Please fill all fields!")
            'dberrorlabel.Text = "Please Fill all fields!"
        Else
            If chkStatEdit.Checked = True Then
                catStatEdit = 1
            Else
                catStatEdit = 0
            End If

            Using con As New SqlConnection(strConnString)
                Using cmd As New SqlCommand("UPDATE [payit].[dbo].[KYCTypes] SET [Name] = '" & txtNameEdit.Text & "', [Description] = '" & txtDescEdit.Text & "', Status = " & catStatEdit & " WHERE ID = " & txtID.Text.Trim())
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridView1.DataBind()
                        End Using
                    End Using
                End Using
                Me.BindData()
                alert("Updated Successfully")
            End Using
        End If
    End Sub
    Protected Sub Delete(ByVal sender As Object, ByVal e As EventArgs)
        Using con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand("DELETE FROM [payit].[dbo].[KYCTypes] WHERE ID = " & txtID.Text.Trim())
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GridView1.DataBind()
                    End Using
                End Using
            End Using
            Me.BindData()
        End Using
        alert("Deleted KYCType")
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("KYCTypes.xls", Me.GridView1)
    End Sub
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Public Sub alertme(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
                      "alertify.alert('" & s & "')" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
End Class
