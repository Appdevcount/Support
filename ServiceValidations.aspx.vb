Imports System.Data.SqlClient
Imports System.Data
Imports Data

Partial Class ServiceValidations
    Inherits System.Web.UI.Page
    Dim db As New payitEntities()
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
            Response.Redirect("~/login.aspx?ReturnURL=ServiceValidations.aspx")
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
            If (ddlService.SelectedIndex = 0) Then
                alert("Please Select Service!")
            Else
                Dim catStat, catValid, catValidEvery As Integer
                If chkStatus.Checked = True Then
                    catStat = 1
                Else
                    catStat = 0
                End If

                If chkValid.Checked Then
                    catValid = 1
                Else
                    catValid = 0
                End If

                If chkValidEvery.Checked Then
                    catValidEvery = 1
                Else
                    catValidEvery = 0
                End If

                Dim sql1 As String = "SELECT * FROM [payit].[dbo].[PayitServiceValidations] WHERE ServiceID=@service"
                Dim cmd1 As SqlCommand = New SqlCommand(sql1, cn)
                cmd1.Connection = cn
                cn.Open()
                cmd1.Parameters.AddWithValue("@service", ddlService.SelectedValue)

                Using reader As SqlDataReader = cmd1.ExecuteReader()
                    If reader.HasRows Then
                        ' Record already exists
                        alertme("Validation Already exists!")
                    Else
                        ' Record does not exist, add them
                        Dim cmd As SqlCommand = New SqlCommand("INSERT INTO [payit].[dbo].[PayitServiceValidations] ([ServiceID],[Info1],[IsValidationRequired],[IsValidationRequiredForEveryNumber],[CreatedDate],[Status]) VALUES " & _
                                                               "(@service,@label,@validation,@validationEvery,getdate(),@status)")
                        cmd.Connection = cn
                        cmd.Parameters.AddWithValue("@service", ddlService.SelectedValue)
                        cmd.Parameters.AddWithValue("@label", txtLabel.Text)
                        cmd.Parameters.AddWithValue("@validation", catValid)
                        cmd.Parameters.AddWithValue("@validationEvery", catValidEvery)
                        cmd.Parameters.AddWithValue("@status", catStat)
                        Dim insert As Integer = cmd.ExecuteNonQuery()
                        If (insert = 1) Then
                            alert("Validation successfully added!")
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
        ddlService.SelectedIndex = 0
        txtLabel.Text = ""
        chkStatus.Checked = False
        chkValid.Checked = False
        chkValidEvery.Checked = False
    End Sub
    Private Sub BindData()
        Dim strQuery As String = ("SELECT TOP 1000 psv.[ID],psv.[ServiceID],ps.[ServiceCode] Service,psv.[IsValidationRequired] " & _
                                    ",psv.[IsValidationRequiredForEveryNumber],psv.[Info1],psv.[Status],psv.[CreatedDate] " & _
                                    "FROM [payit].[dbo].[PayitServiceValidations] psv " & _
                                    "LEFT JOIN payit.dbo.PayitServices as ps ON psv.ServiceID = ps.ID " & _
                                    "ORDER BY ps.[ServiceCode]")
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
    Protected Sub OnDataBound(sender As Object, e As EventArgs)
        Dim row As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal)
        For i As Integer = 0 To GridView1.Columns.Count - 1
            Dim cell As New TableHeaderCell()
            Dim txtSearch As New TextBox()
            txtSearch.Attributes("placeholder") = GridView1.Columns(i).HeaderText
            txtSearch.CssClass = "search_textbox"
            cell.Controls.Add(txtSearch)
            row.Controls.Add(cell)
        Next
        GridView1.HeaderRow.Parent.Controls.AddAt(1, row)
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
        Dim rows = (From c In db.PayitServiceValidations Where c.ID = txtID.Text.Trim() Select c).FirstOrDefault()

        ddlServiceEdit.SelectedValue = rows.ServiceID
        chkValidEdit.Checked = rows.IsValidationRequired
        chkValidEveryEdit.Checked = rows.IsValidationRequiredForEveryNumber
        chkStatEdit.Checked = rows.Status
        txtLabelEdit.Text = rows.Info1
        popup.Show()
        alert("You can now edit")
    End Sub
    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        Dim catStatEdit, catValidEdit, catValidEveryEdit As Integer
        If (ddlServiceEdit.SelectedIndex = 0) Then
            alert("Please fill all fields!")

        Else
            If chkStatEdit.Checked = True Then
                catStatEdit = 1
            Else
                catStatEdit = 0
            End If

            If chkValidEdit.Checked Then
                catValidEdit = 1
            Else
                catValidEdit = 0
            End If

            If chkValidEveryEdit.Checked Then
                catValidEveryEdit = 1
            Else
                catValidEveryEdit = 0
            End If

            Using con As New SqlConnection(strConnString)
                Using cmd As New SqlCommand("UPDATE [payit].[dbo].[PayitServiceValidations] SET [ServiceID] = '" & ddlServiceEdit.SelectedValue & "', [IsValidationRequired] = " & catValidEdit & ", IsValidationRequiredForEveryNumber = " & catValidEveryEdit & ", Status = " & catStatEdit & ", [Info1]='" & txtLabelEdit.Text.Trim() & "' WHERE ID = " & txtID.Text.Trim())
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
            Using cmd As New SqlCommand("DELETE FROM [payit].[dbo].[PayitServiceValidations] WHERE ID = " & txtID.Text.Trim())
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
        alert("Deleted Validation")
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("ServiceValidations.xls", Me.GridView1)
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
