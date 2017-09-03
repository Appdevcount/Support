Imports System.Data.SqlClient
Imports System.Data
Partial Class ServicePaymentTunnels
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
            alertme("Unauthorized Access")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=ServicePaymentTunnels.aspx")
        End If
        If Not IsPostBack Then
            btnDelete.Visible = False
            btnSave.Visible = False
            ddlTunnel.Enabled = False

            Me.BindData()
            GridView1.DataBind()
            Try
                cn = New SqlConnection(strConnString)
                sql = "SELECT ID, ([ServiceCode] + ' | ' + [PaymentName]) AS 'ServicePayment' FROM [payit].[dbo].[ServicesAndPayments] ORDER BY ServiceCode"

                da = New SqlDataAdapter(sql, cn)
                ds = New DataSet()
                da.Fill(ds, "deta")
                ddlSevicePayment.DataSource = ds.Tables("deta")
                ddlSevicePayment.DataTextField = "ServicePayment"
                ddlSevicePayment.DataValueField = "ID"
                ddlSevicePayment.DataBind()
                ddlSevicePayment.Items.Insert(0, New ListItem("SELECT", ""))

                ddlThresholdType.Items.Insert(0, New ListItem("LESS THAN OR EQUAL", "1"))
            Catch ex As Exception
            End Try
        End If
    End Sub

    Protected Sub ddlSevicePayment_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlTunnel.Enabled = False
        ddlTunnel.Items.Clear()
        Dim ProjectId As String = ddlSevicePayment.SelectedItem.Text
        Dim delimiters As Char() = New Char() {"|"c}
        Dim payment As String() = ProjectId.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
        Dim query As String = String.Empty

        If (payment.Length > 0) Then
            query = String.Format("SELECT ID, [TunnelAlias] AS Tunnel FROM [payit].[dbo].[PaymentChannelTunnels] WHERE PaymentChannel = '" & payment(1).Trim() & "'")
        End If
        If ProjectId <> Nothing Then
            BindDropDownList(ddlTunnel, query, "Tunnel", "ID", "SELECT")
            ddlTunnel.Enabled = True
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
        ddl.Items.Insert(0, New ListItem(defaultText, ""))
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If Not (Session("role") = "superadmin" Or Session("role") = "operations" Or Session("role") = "accounts" Or Session("role") = "qa") Then
            alertme("Unauthorized Access")
            Exit Sub
        End If

        If Page.IsValid Then
            If (ddlSevicePayment.SelectedIndex = 0 Or ddlTunnel.SelectedIndex = 0) Then
                alertme("Please fill all fields!")
            Else
                Dim catStat As String
                If chkStatus.Checked = True Then
                    catStat = 1
                Else
                    catStat = 0
                End If

                Using context As New Data.payitEntities
                    Dim rows = (From c In context.ServicePaymentTunnels Where c.ServicePaymentID = ddlSevicePayment.SelectedValue And c.PaymentChannelTunnelID = ddlTunnel.SelectedValue And
                              c.ThresholdType = ddlThresholdType.SelectedValue And c.ThresholdValue = txtThresholdValue.Text.Trim() Select c).FirstOrDefault()
                    If (rows Is Nothing) Then
                        Dim spt As New Data.ServicePaymentTunnel
                        spt.ServicePaymentID = ddlSevicePayment.SelectedValue
                        spt.PaymentChannelTunnelID = ddlTunnel.SelectedValue
                        spt.ThresholdType = ddlThresholdType.SelectedValue
                        spt.ThresholdValue = txtThresholdValue.Text.Trim()
                        spt.Configuration = ddlSevicePayment.SelectedItem.Text.Trim() & "|" & ddlTunnel.SelectedItem.Text.Trim()
                        spt.Status = catStat
                        spt.CreatedBy = Session("user")
                        spt.UpdatedBy = Session("user")
                        spt.CreatedOn = DateTime.Now
                        context.ServicePaymentTunnels.Add(spt)
                        context.SaveChanges()
                    Else
                        alertme("Configuration already exists for: " & ddlSevicePayment.SelectedItem.Text & " - " & ddlTunnel.SelectedItem.Text)
                    End If
                
                End Using
                Clear()
                Me.BindData()
            End If
        End If
    End Sub
    Protected Sub Clear()
        txtThresholdValue.Text = String.Empty
        ddlSevicePayment.SelectedIndex = 0
        ddlTunnel.SelectedIndex = 0
        ddlThresholdType.SelectedIndex = 0
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
        Dim strQuery As String = ("SELECT TOP 1000 spt.[ID],sap.[ServiceCode],sap.PaymentName,pct.TunnelCode,pct.TunnelAlias,[ThresholdType],[ThresholdValue],spt.[Status],spt.[UpdatedBy],spt.[Configuration] " & _
                                    "FROM [payit].[dbo].[ServicePaymentTunnels] spt LEFT JOIN [payit].[dbo].[ServicesAndPayments] sap ON spt.ServicePaymentID = sap.ID " & _
                                    "LEFT JOIN [payit].[dbo].[PaymentChannelTunnels] pct ON spt.PaymentChannelTunnelID = pct.ID ORDER BY spt.ID DESC")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
    End Sub
    Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim thresholdType As TableCell = e.Row.Cells(4)
            If thresholdType.Text = "1" Then
                thresholdType.Text = "LESS THAN OR EQUAL"
            ElseIf thresholdType.Text = "2" Then
                thresholdType.Text = "GREATER THAN OR EQUAL"
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
        Dim rows = db.ServicePaymentTunnels.FirstOrDefault(Function(x) x.ID = txtID.Text.Trim())
        If (rows IsNot Nothing) Then
            ddlSevicePayment.SelectedValue = rows.ServicePaymentID
            ddlSevicePayment_SelectedIndexChanged(sender, e)
            ddlTunnel.SelectedValue = rows.PaymentChannelTunnelID
            ddlThresholdType.SelectedValue = rows.ThresholdType
            txtThresholdValue.Text = rows.ThresholdValue
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
    End Sub
    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        Dim catStatEdit As Integer
        If (ddlSevicePayment.SelectedIndex = 0 Or ddlTunnel.SelectedIndex = 0) Then
            alertme("Please fill all fields!")
        Else
            If chkStatus.Checked = True Then
                catStatEdit = 1
            Else
                catStatEdit = 0
            End If

            Using con As New SqlConnection(strConnString)
                Using cmd As New SqlCommand("UPDATE [payit].[dbo].[ServicePaymentTunnels] SET [ServicePaymentID] = " & ddlSevicePayment.SelectedValue & ", [PaymentChannelTunnelID] = " & ddlTunnel.SelectedValue & ", [ThresholdType] = " & ddlThresholdType.SelectedValue & ", " & _
                                            "[ThresholdValue] = '" & txtThresholdValue.Text.Trim() & "', Status = " & catStatEdit & ",[Configuration] = '" & ddlSevicePayment.SelectedItem.Text.Trim() & "|" & ddlTunnel.SelectedItem.Text.Trim() & "',[UpdatedOn] = GETDATE(), [UpdatedBy]='" & Session("user") & "'  WHERE ID = " & txtID.Text.Trim())
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
                alertme("Updated Successfully")
            End Using
            Clear()
            Response.Redirect(Request.RawUrl)
        End If
    End Sub
    Protected Sub Delete(ByVal sender As Object, ByVal e As EventArgs)
        Using con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand("DELETE FROM [payit].[dbo].[ServicePaymentTunnels] WHERE ID = " & txtID.Text.Trim())
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
        alertme("Rule deleted")
        Clear()
        Response.Redirect(Request.RawUrl)
    End Sub
    Public Sub alertme(ByVal s As String)
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.alert('" & s & "')", True)
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("ServicePaymentTunnels.xls", Me.GridView1)
    End Sub

   
End Class