Imports System.Data.SqlClient
Imports System.Data
Partial Class PaymentChannelTunnels
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
            Response.Redirect("~/login.aspx?ReturnURL=PaymentChannelTunnels.aspx")
        End If
        If Not IsPostBack Then
            btnDelete.Visible = False
            btnSave.Visible = False
            Me.BindData()
            GridView1.DataBind()

            Dim db As New Data.payitEntities
            Dim row1 = (From c In db.PaymentChannels Order By c.PaymentName Select New With {Key .PaymentName = c.PaymentName, Key .ChannelId = c.ID}).ToList()
            ddlPaymentChannel.DataSource = row1
            ddlPaymentChannel.DataTextField = "PaymentName"
            ddlPaymentChannel.DataValueField = "ChannelId"
            ddlPaymentChannel.DataBind()
            ddlPaymentChannel.Items.Insert(0, New ListItem("Select", ""))
           
        End If
    End Sub
  
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If Not (Session("role") = "superadmin" Or Session("role") = "operations" Or Session("role") = "accounts") Then
            alertme("Unauthorized Access")
            Exit Sub
        End If

        If Page.IsValid Then
            If (ddlPaymentChannel.SelectedIndex = 0) Then
                alert("Please select a Payment Channel!")
            Else
                Dim catStat As String
                If chkStatus.Checked = True Then
                    catStat = 1
                Else
                    catStat = 0
                End If

                Using context As New Data.payitEntities
                    Dim tunnel As New Data.PaymentChannelTunnel
                    tunnel.PaymentChannel = ddlPaymentChannel.SelectedItem.Text.Trim()
                    tunnel.TunnelAlias = txtAlias.Text.Trim()
                    tunnel.TunnelCode = txtTunnel.Text.Trim()
                    tunnel.Status = catStat
                    tunnel.UpdatedBy = Session("user")
                    tunnel.CreatedBy = Session("user")
                    tunnel.Configuration = tunnel.PaymentChannel & "|" & tunnel.TunnelAlias & "|" & tunnel.TunnelCode
                    tunnel.CreatedOn = DateTime.Now
                    tunnel.Info2 = ddlPaymentChannel.SelectedValue
                    context.PaymentChannelTunnels.Add(tunnel)
                    context.SaveChanges()
                End Using
                Clear()
                Me.BindData()
            End If
        End If
    End Sub
    Protected Sub Clear()
        txtTunnel.Text = String.Empty
        txtAlias.Text = String.Empty
        ddlPaymentChannel.SelectedIndex = 0
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
        Dim strQuery As String = ("SELECT TOP 1000  pct.ID, pct.TunnelCode, pct.PaymentChannel, pct.Status, pct.TunnelAlias, pct.UpdatedBy FROM [payit].[dbo].[PaymentChannelTunnels] pct " & _
                                  "ORDER BY pct.TunnelCode ")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
    End Sub
    Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs)
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim ruleType As TableCell = e.Row.Cells(1)
        '    Dim configId As TableCell = e.Row.Cells(2)
        '    Dim userType As TableCell = e.Row.Cells(3)
        '    Dim limitType As TableCell = e.Row.Cells(4)
        '    If ruleType.Text = "1" Then
        '        ruleType.Text = "Service"
        '    ElseIf ruleType.Text = "2" Then
        '        ruleType.Text = "Payment"
        '    ElseIf ruleType.Text = "3" Then
        '        ruleType.Text = "ServicePayment"
        '    End If
        '    If userType.Text = "1" Then
        '        userType.Text = "UserId"
        '    ElseIf userType.Text = "2" Then
        '        userType.Text = "Mobile Number"
        '    ElseIf userType.Text = "5" Then
        '        userType.Text = "DeviceId"
        '    End If

        '    If limitType.Text = "1" Then
        '        limitType.Text = "Transaction Count"
        '    ElseIf limitType.Text = "2" Then
        '        limitType.Text = "Total Amount"
        '    End If
        'End If
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
        Dim rows = db.PaymentChannelTunnels.FirstOrDefault(Function(x) x.ID = txtID.Text.Trim())
        If (rows IsNot Nothing) Then
            ddlPaymentChannel.SelectedValue = rows.Info2
            txtTunnel.Text = rows.TunnelCode
            txtAlias.Text = rows.TunnelAlias
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
        If (ddlPaymentChannel.SelectedIndex = 0) Then
            alert("Please select a payment channel!")
        Else
            If chkStatus.Checked = True Then
                catStatEdit = 1
            Else
                catStatEdit = 0
            End If

            Using con As New SqlConnection(strConnString)
                Using cmd As New SqlCommand("UPDATE [payit].[dbo].[PaymentChannelTunnels] SET [PaymentChannel] = '" & ddlPaymentChannel.SelectedItem.Text & "', [Configuration] = '" & ddlPaymentChannel.SelectedItem.Text.Trim() & "|" & txtAlias.Text.Trim() & "|" & txtTunnel.Text.Trim() & "', [TunnelCode] = '" & txtTunnel.Text.Trim() & "', " & _
                                            "Status = " & catStatEdit & ",[UpdatedOn] = GETDATE(), [TunnelAlias]='" & txtAlias.Text.Trim() & "', [Info2]='" & ddlPaymentChannel.SelectedValue & "', [UpdatedBy]='" & Session("user") & "' WHERE ID = " & txtID.Text.Trim())
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
            Using cmd As New SqlCommand("DELETE FROM [payit].[dbo].[PaymentChannelTunnels] WHERE ID = " & txtID.Text.Trim())
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
        ExportToExcel.Export("PaymentChannelTunnels.xls", Me.GridView1)
    End Sub
End Class