Imports System.Data.SqlClient
Imports System.Data
Imports Data

Partial Class PayitServiceUserKYCs
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sql, sql2, strSQL As String
    Dim dispAmnt, info1 As String
    Dim sqlLocalTrans As String
    Dim db As New payitEntities()
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "cs" Or Session("role") = "operations" Or Session("role") = "qa") Then
            alert("Unauthorized Access")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=PayitServicesKYCTypes.aspx")
        End If
  

        If Not IsPostBack Then
            Me.BindData()
            GridView1.DataBind()

            Dim row1 = (From c In db.PayitServices Order By c.ServiceCode Select New With {Key .ServiceCode = c.ServiceCode, Key .ID = c.ID}).ToList()
            ddlService.DataSource = row1
            ddlService.DataTextField = "ServiceCode"
            ddlService.DataValueField = "ID"
            ddlService.DataBind()
            ddlService.Items.Insert(0, New ListItem("Select", "None"))

            ddlServiceEdit.DataSource = row1
            ddlServiceEdit.DataTextField = "ServiceCode"
            ddlServiceEdit.DataValueField = "ID"
            ddlServiceEdit.DataBind()
            ddlServiceEdit.Items.Insert(0, New ListItem("Select", "None"))
        End If
    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If Not (Session("role") = "superadmin" Or Session("role") = "operations" Or Session("role") = "accounts" Or Session("role") = "qa") Then
            alert("Unauthorized Access")
            Exit Sub
        End If

        If Page.IsValid Then
            Dim cn As SqlConnection = New SqlConnection(strConnString)
            If (txtUID.Text = "" Or ddlService.SelectedIndex = 0 Or txtRating.Text = "") Then
                alert("Please fill all fields!")
            Else
                Dim catStat As Integer
                If chkStatus.Checked = True Then
                    catStat = 1
                Else
                    catStat = 0
                End If

                Dim sql1 As String = "SELECT * FROM [payit].[dbo].[PayitServiceUserKYCs] WHERE [UserIdentifier]=@userID and [PayitServiceID]=@serviceID and [KYCRating]=@rating"
                Dim cmd1 As SqlCommand = New SqlCommand(sql1, cn)
                cmd1.Connection = cn
                cn.Open()
                cmd1.Parameters.AddWithValue("@userID", txtUID.Text.Trim())
                cmd1.Parameters.AddWithValue("@serviceID", ddlService.SelectedValue)
                cmd1.Parameters.AddWithValue("@rating", txtRating.Text)

                Using reader As SqlDataReader = cmd1.ExecuteReader()
                    If reader.HasRows Then
                        ' Record already exists
                        alertme("Rating Already exists for:<br><b> " & txtUID.Text & "</b>")
                    Else
                        ' Record does not exist, add them
                        Dim cmd As SqlCommand = New SqlCommand("INSERT INTO [payit].[dbo].[PayitServiceUserKYCs]([UserIdentifier],[PayitServiceID],[KYCRating],[Status],[CreatedDate]) VALUES (@userID,@serviceID,@rating,@status,getdate())")
                        cmd.Connection = cn
                        cmd.Parameters.AddWithValue("@userID", txtUID.Text.Trim())
                        cmd.Parameters.AddWithValue("@serviceID", ddlService.SelectedValue)
                        cmd.Parameters.AddWithValue("@rating", txtRating.Text)
                        cmd.Parameters.AddWithValue("@status", catStat)
                        Dim insert As Integer = cmd.ExecuteNonQuery()
                        If (insert = 1) Then
                            alert("KYC successfully added for:" & txtUID.Text & "!")
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
        txtUID.Text = ""
        ddlService.SelectedIndex = 0
        txtRating.Text = String.Empty
        chkStatus.Checked = False
    End Sub
    Private Sub BindData()
        Dim strQuery As String = ("SELECT TOP 1000 psk.[ID],psk.UserIdentifier,psk.[PayitServiceID], ps.ServiceCode ServiceName,psk.[KYCRating],psk.[Status],psk.CreatedDate [Date] from " & _
                                  " [payit].[dbo].[PayitServiceUserKYCs] psk LEFT JOIN [payit].dbo.PayitServices as ps on psk.PayitServiceID = ps.ID " & _
                                  " order by psk.ID desc")
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
            txtUIDEdit.Text = 0
        Else
            txtUIDEdit.Text = row.Cells(1).Text
        End If

        If row.Cells(2).Text = "None" Or row.Cells(2).Text = "Select" Or row.Cells(2).Text = "&nbsp;" Or row.Cells(2).Text = "" Then
            ddlServiceEdit.SelectedIndex = 0
        Else
            ddlServiceEdit.SelectedValue = row.Cells(2).Text
        End If

        If row.Cells(5).Text = "None" Or row.Cells(5).Text = "Select" Or row.Cells(5).Text = "&nbsp;" Or row.Cells(5).Text = "" Then
            txtRatingEdit.Text = ""
        Else
            txtRatingEdit.Text = row.Cells(5).Text
        End If

        If row.Cells(6).Text = "True" Then
            chkStatEdit.Checked = True
        Else
            chkStatEdit.Checked = False
        End If
        popup.Show()
        alert("You can now edit")
    End Sub
    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        Dim catStatEdit As Integer
        If (txtUIDEdit.Text = "" Or ddlServiceEdit.SelectedIndex = 0 Or txtRatingEdit.Text = "") Then
            alert("Please fill all fields!")
        Else
            If chkStatEdit.Checked = True Then
                catStatEdit = 1
            Else
                catStatEdit = 0
            End If

            Using con As New SqlConnection(strConnString)
                Using cmd As New SqlCommand("UPDATE [payit].[dbo].[PayitServiceUserKYCs] SET [UserIdentifier]= '" & txtUIDEdit.Text.Trim() & "', [PayitServiceID] = '" & ddlServiceEdit.SelectedValue & "', [KYCRating]='" & txtRatingEdit.Text.Trim() & "', Status = " & catStatEdit & " WHERE ID = " & txtID.Text.Trim())
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
            Using cmd As New SqlCommand("DELETE FROM [payit].[dbo].[PayitServiceUserKYCs] WHERE ID = " & txtID.Text.Trim())
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
        alert("Deleted Rating")
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("PayitServiceUserKYC.xls", Me.GridView1)
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
