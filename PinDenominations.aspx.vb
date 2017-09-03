Imports System.Data.SqlClient
Imports System.Data
Imports Data
Imports Data.payitEntities

Partial Class PinDenominations
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sqlLocalTrans, Sql As String
    Dim priceTotal As Decimal = 0
    Private grdTotal As Decimal
    Dim fromdate, todate As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Dim typeS As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=PinDenominations.aspx")
        End If

        If Not IsPostBack Then
            cn = New SqlConnection(strConnString)
            Sql = "Select [ServiceName],[ServiceID] from [Services] WHERE ServiceName like '%-o' order by ServiceName"
            da = New SqlDataAdapter(Sql, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")
            ddlService.DataSource = ds.Tables("deta")
            ddlService.DataTextField = "ServiceName"
            ddlService.DataValueField = "ServiceID"
            ddlService.DataBind()
            ddlService.Items.Insert(0, New ListItem("Select", "None"))

        End If
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        If Page.IsValid Then
            If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations") Then
                alert("You are not authorized to perform this action.")
                Exit Sub
            End If
            Try
                sqlLocalTrans = "select TOP 1000 d.[ID], s.ServiceName, d.[Amount], d.[Amount2], d.[Status] FROM [payit].[dbo].[Denominations] d LEFT JOIN [payit].[dbo].[Services] s " & _
                                "on d.ServiceID = s.ServiceID "
                Dim tempSQL As String = ""
                If ddlService.SelectedItem.Text <> "All" Or ddlService.SelectedItem.Text <> "NULL" Then
                    If tempSQL = "" Then
                        tempSQL = "where d.[ServiceID] = " & Trim(ddlService.SelectedItem.Value) & " "
                    End If
                End If
              
                If tempSQL <> "" Then
                    sqlLocalTrans = sqlLocalTrans & tempSQL & " order by cast(d.Amount as float)"
                Else
                    sqlLocalTrans = sqlLocalTrans & " order by cast(d.Amount as float)"
                End If
                Session("sql") = sqlLocalTrans
                BindGrid()
            Catch ex As Exception

            End Try
        End If
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

    Private Sub BindGrid()
        If (chkPaging.Checked) Then
            GridView1.AllowPaging = False
        Else
            GridView1.AllowPaging = True
        End If
        Dim query As String = Session("sql")
        Using con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand(query)
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GridView1.DataSource = dt
                        GridView1.DataBind()
                        GridView1.EmptyDataText = "No Transactions Found"
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private Sub BindData()
        Dim strQuery As String = ("select TOP 1000 d.[ID], s.ServiceName, d.[Amount], d.[Amount2], d.[Status] from FROM [payit].[dbo].[Denominations] d " & _
  "LEFT JOIN [payit].[dbo].[Services] s " & _
  "on d.ServiceID = s.ServiceID order by d.ID desc")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        GridView1.PageIndex = e.NewPageIndex
        Me.BindGrid()
    End Sub

    Public Sub sqldatabind(ByVal s As String, ByVal e As GridViewRowEventArgs, Optional ByVal types1 As String = "", Optional ByVal type As String = "")
        da = New SqlDataAdapter()
        ds = New DataSet()
        da.Fill(ds, "det")
        GridView1.DataSource = ds.Tables("det")
        GridView1.DataBind()
    End Sub
    
    Protected Sub Edit(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        txtID.Text = row.Cells(0).Text
        Dim dc As New payitEntities

        Dim deno = (From d In dc.Denominations Where d.ID = txtID.Text.Trim()).FirstOrDefault()
        If (deno IsNot Nothing) Then
            ServiceVal.Text = "Service: " & deno.Service.ServiceName
            DenominationVal.Text = "Denomination: " & deno.Amount2
            AmountVal.Text = "Amount: " & deno.Amount
            If (deno.Status = True) Then
                chkStatEdit.Checked = True
            Else
                chkStatEdit.Checked = False
            End If
        End If

        popup.Show()
        alert("You can now edit")
    End Sub

    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations") Then
            Response.Redirect("~/login.aspx?ReturnURL=PinDenominations.aspx")
        End If

        If (txtReason.Text = "" Or txtReason.Text Is Nothing) Then
            txtProcessedByError.Visible = True
            txtProcessedByError.Text = "Please Add a Reason"
            popup.Show()
            Exit Sub
        End If

        Dim catStatEdit As Integer
        If chkStatEdit.Checked = True Then
            catStatEdit = 1
        Else
            catStatEdit = 0
        End If
        Dim de As New payitEntities
        Using con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand("UPDATE [payit].[dbo].[Denominations] SET  Status = " & catStatEdit & "  WHERE ID = " & txtID.Text.Trim() & _
                                        ";INSERT INTO [payit].[dbo].[LogTrace] ([Page],[Service],[PaymentChannel],[status],[ChangedBy],[info1],[info2],[CreatedOn]) VALUES " & _
                                        "('Denominations','" & ddlService.SelectedItem.Text.Trim() & "', '" & AmountVal.Text.Trim() & "', " & catStatEdit & ", '" & Session("user") & "','" & Request.UserHostAddress & "', '" & txtReason.Text.Trim() & "', getdate())")
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GridView1.DataBind()
                    End Using
                End Using
            End Using
            Me.BindGrid()
            alert("Denomination Status Updated Successfully")
        End Using
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("PINDenominations.xls", Me.GridView1)
    End Sub

    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub

    Protected Sub ddlService_SelectedIndexChanged(sender As Object, e As EventArgs)
        If IsPostBack Then
            If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations") Then
                alert("You are not authorized to perform this action.")
                Exit Sub
            End If
            Try
                PageHeader.InnerText = "PIN Denomination Details"
                If (ddlService.SelectedIndex <> 0) Then
                    Dim db As New Data.payitEntities
                    Dim serviceStatus = (From c In db.PayitServices Where c.ServiceCode.Equals(ddlService.SelectedItem.Text) Select c.StatusNew).FirstOrDefault()
                    If (serviceStatus.HasValue) Then
                        Dim servStat As String = "Disabled"
                        If (serviceStatus = True) Then
                            servStat = "Enabled"
                        End If
                        PageSub.InnerText = ddlService.SelectedItem.Text.ToUpper() & " Service Status: " & servStat
                    End If
                End If
                sqlLocalTrans = "select TOP 1000 d.[ID], s.ServiceName, d.[Amount], d.[Amount2], d.[Status] FROM [payit].[dbo].[Denominations] d LEFT JOIN [payit].[dbo].[Services] s " & _
                                "on d.ServiceID = s.ServiceID "
                Dim tempSQL As String = ""
                If ddlService.SelectedItem.Text <> "All" Or ddlService.SelectedItem.Text <> "NULL" Then
                    If tempSQL = "" Then
                        tempSQL = "where d.[ServiceID] = " & Trim(ddlService.SelectedItem.Value) & " "
                    End If
                End If

                If tempSQL <> "" Then
                    sqlLocalTrans = sqlLocalTrans & tempSQL & " order by cast(d.Amount as float)"
                Else
                    sqlLocalTrans = sqlLocalTrans & " order by cast(d.Amount as float)"
                End If
                Session("sql") = sqlLocalTrans
                BindGrid()
            Catch ex As Exception

            End Try
        End If
    End Sub
End Class
