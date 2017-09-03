Imports System.Data.SqlClient
Imports System.Data
Partial Class Validations
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sql, sql2, strSQL As String
    Dim dispAmnt, info1 As String
    Dim sqlLocalTrans As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations") Then
            alert("Unauthorized Access")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=Validations.aspx")
        End If
        If Not IsPostBack Then
            btnDelete.Visible = False
            btnSave.Visible = False

            Me.BindData()
            GridView1.DataBind()
            cn = New SqlConnection(strConnString)
            sql = "SELECT DISTINCT ServiceCode FROM [payit].[dbo].[ServicesAndPayments]"
            da = New SqlDataAdapter(sql, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")

            ddlValidation.Items.Insert(0, New ListItem("Select", "None"))
            ddlValidation.Items.Insert(1, New ListItem("Service", "1"))
            ddlValidation.Items.Insert(2, New ListItem("Payment", "2"))
            ddlValidation.Items.Insert(3, New ListItem("ServicePayment", "3"))

            ddlvalidationEdit.Items.Insert(0, New ListItem("Select", "None"))
            ddlvalidationEdit.Items.Insert(1, New ListItem("Service", "1"))
            ddlvalidationEdit.Items.Insert(2, New ListItem("Payment", "2"))
            ddlvalidationEdit.Items.Insert(3, New ListItem("ServicePayment", "3"))

        End If
    End Sub
    Protected Sub ddlValidation_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlConfiguration.Enabled = False
        ddlConfiguration.Items.Clear()
        Dim ProjectId As String = ddlValidation.SelectedItem.Text
        Dim query As String = String.Empty
        If (ProjectId.ToLower() = "service") Then
            query = String.Format("Select [ServiceCode] ID,[ServiceCode] Validation from [payit].[dbo].[PayitServices] order by ServiceName")
        ElseIf (ProjectId.ToLower() = "payment") Then
            query = String.Format("SELECT [PaymentName] ID,[PaymentName] Validation FROM [payit].[dbo].[PaymentChannels]")
        ElseIf (ProjectId.ToLower() = "servicepayment") Then
            query = String.Format("SELECT DISTINCT [ID],[ServiceCode] + ' - ' + [PaymentName] Validation FROM [payit].[dbo].[ServicesAndPayments]")
        End If
        If ProjectId <> Nothing Then
            BindDropDownList(ddlConfiguration, query, "Validation", "ID", "Select")
            ddlConfiguration.Enabled = True
        End If
    End Sub

    Private Sub BindDropDownList(ddl As DropDownList, query As String, text As String, value As String, defaultText As String)
        Dim cmd As New SqlCommand(query)
        Using con As New SqlConnection(strConnString)
            Using sda As New SqlDataAdapter()
                cmd.Connection = con
                con.Open()
                ddl.DataSource = cmd.ExecuteReader()
                ddl.DataTextField = text
                ddl.DataValueField = value
                ddl.DataBind()
                con.Close()
            End Using
        End Using
        ddl.Items.Insert(0, New ListItem(defaultText, "None"))
    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If Not (Session("role") = "superadmin" Or Session("role") = "operations" Or Session("role") = "accounts") Then
            alert("Unauthorized Access")
            Exit Sub
        End If

        If Page.IsValid Then
            Dim cn As SqlConnection = New SqlConnection(strConnString)
            If (ddlValidation.SelectedIndex = 0 Or ddlConfiguration.SelectedIndex = 0) Then
                alert("Please fill all fields!")
                'dberrorlabel.Text = "Please Fill all fields!"
            Else
                Dim catStat As String
                If chkStatus.Checked = True Then
                    catStat = 1
                Else
                    catStat = 0
                End If

                Dim config As String = String.Empty
                If ddlValidation.SelectedItem.Text = "ServicePayment" Then
                    config = ddlConfiguration.SelectedValue
                Else
                    config = ddlConfiguration.SelectedItem.Text
                End If

                Dim sql1 As String = "SELECT * FROM [payit].[dbo].[Validations] WHERE [ValidationType]=@validType and [ConfigID]=@configID"
                Dim cmd1 As SqlCommand = New SqlCommand(sql1, cn)
                cmd1.Connection = cn
                cn.Open()
                cmd1.Parameters.AddWithValue("@validType", ddlValidation.SelectedValue)
                cmd1.Parameters.AddWithValue("@configID", config)
                
                Using reader As SqlDataReader = cmd1.ExecuteReader()
                    If reader.HasRows Then
                        ' Record already exists
                        alertme("Validation Already exists for:<br><b> " & ddlValidation.SelectedItem.Text & " - " & ddlConfiguration.SelectedItem.Text & "</b><br>")
                    Else
                        ' Record does not exist, add them
                        Dim cmd As SqlCommand = New SqlCommand("INSERT INTO [payit].[dbo].[Validations]([ValidationType],[ConfigID],[MinAmt],[MaxAmt],[Status],[CreatedOn]) VALUES ('" & ddlValidation.SelectedValue & "','" & config & "','" & txtMinAmount.Text.Trim() & "'," & txtMaxAmount.Text.Trim() & "," & catStat & ",getdate())")
                        cmd.Connection = cn
                        Dim insert As Integer = cmd.ExecuteNonQuery()
                        If (insert = 1) Then
                            alert("Validation successfully added for:" & ddlValidation.SelectedItem.Text & " - " & ddlConfiguration.SelectedItem.Text & "!")
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
        txtMinAmount.Text = String.Empty
        txtMaxAmount.Text = String.Empty
        ddlValidation.SelectedIndex = 0
        ddlConfiguration.SelectedIndex = 0
        chkStatus.Checked = False
    End Sub
    Private Sub BindData()
        Dim strQuery As String = ("select TOP 1000 ID, [ValidationType], [ConfigID], [MinAmt], [MaxAmt], [Status], [CreatedOn], [Configuration] from [payit].[dbo].[Validations] order by ID desc")
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
            ddlValidation.SelectedIndex = 0
        ElseIf (row.Cells(1).Text.ToLower.Contains("&amp;")) Then
            Dim service = row.Cells(1).Text.Replace("&amp;", "&")
            ddlValidation.SelectedValue = service
        Else
            ddlValidation.SelectedValue = row.Cells(1).Text
        End If

        If (ddlValidation.SelectedIndex <> 0) Then
            ddlValidation_SelectedIndexChanged(sender, e)
        End If

        If row.Cells(2).Text = "None" Or row.Cells(2).Text = "Select" Or row.Cells(2).Text = "&nbsp;" Or row.Cells(2).Text = "" Then
            ddlConfiguration.SelectedIndex = 0
        Else
            ddlConfiguration.SelectedValue = row.Cells(2).Text
        End If


        If row.Cells(3).Text = "None" Or row.Cells(3).Text = "Select" Or row.Cells(3).Text = "&nbsp;" Or row.Cells(3).Text = "" Then
            txtMinAmount.Text = ""
        Else
            txtMinAmount.Text = row.Cells(3).Text
        End If

        If row.Cells(4).Text = "None" Or row.Cells(4).Text = "&nbsp;" Or row.Cells(4).Text = "" Then
            txtMaxAmount.Text = ""
        Else
            txtMaxAmount.Text = row.Cells(4).Text
        End If

        'If row.Cells(5).Text = "None" Or row.Cells(5).Text = "&nbsp;" Or row.Cells(5).Text = "" Then
        '    txtThresholdEdit.Text = ""
        'Else
        '    txtThresholdEdit.Text = row.Cells(5).Text
        'End If

        If row.Cells(6).Text = "True" Then
            chkStatus.Checked = True
        Else
            chkStatus.Checked = False
        End If

        btnSave.Visible = True
        btnDelete.Visible = True
        btnSubmit.Visible = False
        'popup.Show()
        alert("You can now edit")
    End Sub
    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        Dim catStatEdit As Integer
        If (ddlValidation.SelectedIndex = 0 Or ddlConfiguration.SelectedIndex = 0) Then
            alert("Please fill all fields!")
        Else
            If chkStatus.Checked = True Then
                catStatEdit = 1
            Else
                catStatEdit = 0
            End If

            Using con As New SqlConnection(strConnString)
                Using cmd As New SqlCommand("UPDATE [payit].[dbo].[Validations] SET [ValidationType] = '" & ddlValidation.SelectedValue & "', [ConfigID] = '" & ddlConfiguration.SelectedValue & "', [MinAmt] = '" & txtMinAmount.Text.Trim() & "', [MaxAmt] = " & txtMaxAmount.Text.Trim() & ", Status = " & catStatEdit & " WHERE ID = " & txtID.Text.Trim())
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
            Clear()
            Response.Redirect(Request.RawUrl)
        End If
    End Sub
    Protected Sub Delete(ByVal sender As Object, ByVal e As EventArgs)
        Using con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand("DELETE FROM [payit].[dbo].[Validations] WHERE ID = " & txtID.Text.Trim())
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
        Clear()
        Response.Redirect(Request.RawUrl)
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
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("Validation.xls", Me.GridView1)
    End Sub
End Class