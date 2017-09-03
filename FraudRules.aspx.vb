Imports System.Data.SqlClient
Imports System.Data
Partial Class FraudRules
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sql, sql2, strSQL As String
    Dim dispAmnt, info1 As String
    Dim sqlLocalTrans As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "qa") Then
            alert("Unauthorized Access")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=FraudRules.aspx")
        End If
        If Not IsPostBack Then
            btnDelete.Visible = False
            btnSave.Visible = False

            Me.BindData()
            GridView1.DataBind()

            ddlRuleType.Items.Insert(0, New ListItem("Select", "0"))
            ddlRuleType.Items.Insert(1, New ListItem("Service", "1"))
            ddlRuleType.Items.Insert(2, New ListItem("Payment", "2"))
            ddlRuleType.Items.Insert(3, New ListItem("ServicePayment", "3"))

            ddlUserType.Items.Insert(0, New ListItem("Select", "0"))
            ddlUserType.Items.Insert(1, New ListItem("UserId", "1"))
            ddlUserType.Items.Insert(2, New ListItem("MobileNumber", "2"))
            ddlUserType.Items.Insert(3, New ListItem("DeviceId", "5"))

            ddlLimitType.Items.Insert(0, New ListItem("Select", "0"))
            ddlLimitType.Items.Insert(1, New ListItem("Transaction Count", "1"))
            ddlLimitType.Items.Insert(2, New ListItem("Total Amount", "2"))

        End If
    End Sub
    Protected Sub ddlRule_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlConfiguration.Enabled = False
        ddlConfiguration.Items.Clear()
        Dim ProjectId As String = ddlRuleType.SelectedItem.Text
        Dim query As String = String.Empty
        If (ProjectId.ToLower() = "service") Then
            query = String.Format("Select [ServiceID] ID,[ServiceName] Validation from [payit].[dbo].[Services] order by ServiceName")
        ElseIf (ProjectId.ToLower() = "payment") Then
            query = String.Format("SELECT ID,[PaymentName] Validation FROM [payit].[dbo].[PaymentChannels] ORDER BY PaymentName")
        ElseIf (ProjectId.ToLower() = "servicepayment") Then
            query = String.Format("SELECT [ID],[ServiceCode] + ' - ' + [PaymentName] Validation FROM [payit].[dbo].[ServicesAndPayments]  order by ServiceCode")
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
        If Not (Session("role") = "superadmin" Or Session("role") = "operations" Or Session("role") = "accounts" Or Session("role") = "qa") Then
            alertme("Unauthorized Access")
            Exit Sub
        End If

        If Page.IsValid Then
            If (ddlRuleType.SelectedIndex = 0 Or ddlConfiguration.SelectedIndex = 0) Then
                alert("Please fill all fields!")
            Else
                Dim catStat As String
                If chkStatus.Checked = True Then
                    catStat = 1
                Else
                    catStat = 0
                End If

                Using context As New Data.payitEntities
                    Dim rule As New Data.FraudRule
                    rule.RuleType = ddlRuleType.SelectedValue
                    rule.ConfigID = ddlConfiguration.SelectedValue
                    rule.UserType = ddlUserType.SelectedValue
                    rule.LimitType = ddlLimitType.SelectedValue
                    rule.LimitValue = txtLimitValue.Text.Trim()
                    rule.Duration = txtDuration.Text.Trim()
                    rule.Status = catStat
                    rule.UpdatedBy = Session("user")
                    rule.Info2 = ddlConfiguration.SelectedItem.Text
                    rule.CreatedOn = DateTime.Now
                    context.FraudRules.Add(rule)
                    context.SaveChanges()

                    'Dim cmd As SqlCommand = New SqlCommand("INSERT INTO [payit].[dbo].[FraudRules]([RuleType],[ConfigID],[UserType],[LimitType],[LimitValue],[Duration],[Status],[CreatedOn],[UpdateBy]) VALUES " & _
                    '                                       "('" & ddlRuleType.SelectedValue & "','" & config & "','" & ddlUserType.SelectedValue & "','" & ddlLimitType.SelectedValue & "','" & txtLimitValue.Text.Trim() & "'," & txtDuration.Text.Trim() & "," & catStat & ",getdate(),'" & Session("user") & "')")
                    'cmd.Connection = cn
                    'Dim insert As Integer = cmd.ExecuteNonQuery()
                    'If (insert = 1) Then
                    '    alert("Rule successfully added for:" & ddlRuleType.SelectedItem.Text & " - " & ddlConfiguration.SelectedItem.Text & "!")
                    'Else
                    '    alertme("Error! Try Again")
                    'End If
                End Using
                Clear()
                Me.BindData()
            End If
        End If
    End Sub
    Protected Sub Clear()
        txtDuration.Text = String.Empty
        txtLimitValue.Text = String.Empty
        ddlRuleType.SelectedIndex = 0
        ddlConfiguration.Items.Clear()
        ddlLimitType.SelectedIndex = 0
        ddlUserType.SelectedIndex = 0
        chkStatus.Checked = False

        If (btnDelete.Visible) Then
            btnDelete.Visible = False
        End If
        If (btnSave.Visible) Then
            btnSave.Visible = False
        End If
        If Not (btnSubmit.Visible) Then
            btnSubmit.Visible = True
        End If

    End Sub
    Private Sub BindData()
        Dim strQuery As String = ("select TOP 1000 ID,[RuleType],[ConfigID],[UserType],[LimitType],[LimitValue],[Duration],[Status],[UpdatedBy],[CreatedOn],[Info2] from [payit].[dbo].[FraudRules] order by ID desc")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
    End Sub
    Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ruleType As TableCell = e.Row.Cells(1)
            Dim configId As TableCell = e.Row.Cells(2)
            Dim userType As TableCell = e.Row.Cells(3)
            Dim limitType As TableCell = e.Row.Cells(4)
            If ruleType.Text = "1" Then
                ruleType.Text = "Service"
            ElseIf ruleType.Text = "2" Then
                ruleType.Text = "Payment"
            ElseIf ruleType.Text = "3" Then
                ruleType.Text = "ServicePayment"
            End If
            If userType.Text = "1" Then
                userType.Text = "UserId"
            ElseIf userType.Text = "2" Then
                userType.Text = "Mobile Number"
            ElseIf userType.Text = "5" Then
                userType.Text = "DeviceId"
            End If

            If limitType.Text = "1" Then
                limitType.Text = "Transaction Count"
            ElseIf limitType.Text = "2" Then
                limitType.Text = "Total Amount"
            End If
        End If
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

        Dim db As New Data.payitEntities
        Dim rows = db.FraudRules.FirstOrDefault(Function(x) x.ID = txtID.Text.Trim())
        If (rows IsNot Nothing) Then
            ddlRuleType.SelectedValue = rows.RuleType
            If (ddlRuleType.SelectedIndex <> 0) Then
                ddlRule_SelectedIndexChanged(sender, e)
            End If
            ddlConfiguration.SelectedValue = rows.ConfigID
            ddlUserType.SelectedValue = rows.UserType
            ddlLimitType.SelectedValue = rows.LimitType
            txtLimitValue.Text = rows.LimitValue
            txtDuration.Text = rows.Duration
            If rows.Status Then
                chkStatus.Checked = True
            Else
                chkStatus.Checked = False
            End If

        End If

        btnSave.Visible = True
        btnDelete.Visible = True
        btnSubmit.Visible = False
        'popup.Show()
        alert("You can now edit")
    End Sub
    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        Dim catStatEdit As Integer
        If (ddlRuleType.SelectedIndex = 0 Or ddlConfiguration.SelectedIndex = 0) Then
            alert("Please fill all fields!")
        Else
            If chkStatus.Checked = True Then
                catStatEdit = 1
            Else
                catStatEdit = 0
            End If

            Using con As New SqlConnection(strConnString)
                Using cmd As New SqlCommand("UPDATE [payit].[dbo].[FraudRules] SET [RuleType] = '" & ddlRuleType.SelectedValue & "', [ConfigID] = '" & ddlConfiguration.SelectedValue & "', [UserType] = '" & ddlUserType.SelectedValue & "', [LimitType] = " & ddlLimitType.SelectedValue & ", " & _
                                            "[LimitValue] = '" & txtLimitValue.Text.Trim() & "',[Duration] = '" & txtDuration.Text.Trim() & "',Status = " & catStatEdit & ",[UpdatedOn] = GETDATE(), [Info2]='" & ddlConfiguration.SelectedItem.Text & "', [UpdatedBy]='" & Session("user") & "'  WHERE ID = " & txtID.Text.Trim())
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
            Using cmd As New SqlCommand("DELETE FROM [payit].[dbo].[FraudRules] WHERE ID = " & txtID.Text.Trim())
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
        alert("Rule deleted")
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
        ExportToExcel.Export("FraudRules.xls", Me.GridView1)
    End Sub
End Class