Imports System.Data.SqlClient
Imports System.Data

Partial Class UploadPINHistory
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sqlLocalTrans, Sql As String
    Private grdTotal As Decimal
    Dim fromdate, todate As String
    Dim priceTotal As Decimal = 0
    Dim grandTotal As Decimal = 0
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=UploadPINHistory.aspx")
        End If
        
        If Not Page.IsPostBack Then
            FromDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")
            ToDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")
            Try
                cn = New SqlConnection(strConnString)
                Sql = "select ServiceName,ServiceID from services WHERE ServiceName like '%-O' order by ServiceName "
                da = New SqlDataAdapter(Sql, cn)
                ds = New DataSet()
                da.Fill(ds, "deta")
                ddlServiceName.DataSource = ds.Tables("deta")
                ddlServiceName.DataTextField = "ServiceName"
                ddlServiceName.DataValueField = "ServiceID"
                ddlServiceName.DataBind()
            Catch ex As Exception
                dberrorlabel.Text = ex.ToString
            End Try

        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        fromdate = Trim(FromDateTextBox.Text) & " 00:00:00"
        todate = Trim(ToDateTextBox.Text) & " 23:59:59"
        If (ddlServiceName.SelectedItem.Text = "All") Then

        End If
        Try
            sqlLocalTrans = "SELECT [Service],case when CHARINDEX('-',PIN)>0 " & _
                                                "then SUBSTRING(PIN,1,CHARINDEX('-',PIN)-1) else isnull(PIN,'Other Vendor') end 'Vendor',[Amount],Count([TranDate]) as PINS, [TranDate] as Date FROM [payit].[dbo].[PINS] where [TranDate] between '" & fromdate & "' and '" & todate & "'"
            Dim tempSQL As String = ""
            Dim DenomSQL As String = ""
            If ddlServiceName.SelectedItem.Text <> "All" Or ddlServiceName.SelectedItem.Text <> "NULL" Then
                If tempSQL = "" Then
                    tempSQL = "[Service] like '" & Trim(ddlServiceName.SelectedItem.Text) & "' "
                Else
                    tempSQL = tempSQL & " and [Service] like '" & Trim(ddlServiceName.SelectedItem.Text) & "' "
                End If
            End If
            If ddlVendor.SelectedItem.Text <> "All" Then
                If (ddlVendor.SelectedItem.Text = "HA") Then
                    ddlVendor.SelectedItem.Text = "HA-OG"
                End If

                If tempSQL = "" Then
                    tempSQL = "[PIN] like '" & Trim(ddlVendor.SelectedItem.Text) & "' "
                Else
                    tempSQL = tempSQL & " and [PIN] like '" & Trim(ddlVendor.SelectedItem.Text) & "' "
                End If
            End If
            If ddlDenom.SelectedItem.Text <> "All" Or ddlDenom.SelectedItem.Text <> "NULL" Then
                If DenomSQL = "" Then
                    DenomSQL = "[Amount] like '" & Trim(ddlDenom.SelectedValue) & "' "
                Else
                    DenomSQL = DenomSQL & " and [Amount] like '" & Trim(ddlDenom.SelectedValue) & "' "
                End If
            End If
            If tempSQL <> "" Then
                sqlLocalTrans = sqlLocalTrans & " and " & tempSQL & " and " & DenomSQL & " Group by PIN,TranDate,[Service],Amount order by cast(Amount as float),[Service]"
            Else
                sqlLocalTrans = sqlLocalTrans & " Group by TranDate,[Service],Amount order by Date,cast(Amount as float),[Service]"
            End If
            Session("sql") = sqlLocalTrans
            BindGrid()
        Catch ex As Exception

        End Try
      
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
   
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' add the UnitPrice and QuantityTotal to the running total variables
            priceTotal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, _
              "Amount"))
            grandTotal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PINS"))
            Label17.Text = "Total Amount:" & priceTotal.ToString("N2")
            Label19.Text = "Total PINS:" & grandTotal.ToString("N2")
        End If
    End Sub

    Private Sub BindGrid()
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
        Dim strQuery As String = ("SELECT [Service],case when CHARINDEX('-',PIN)>0 " & _
                                                  "then SUBSTRING(PIN,1,CHARINDEX('-',PIN)-1) else isnull(PIN,'Other Vendor') end 'Vendor',[Amount],Count([TranDate]) as PINS, [TranDate] as Date FROM [payit].[dbo].[PINS] Group by PIN,TranDate,[Service],Amount")
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

    Protected Sub chkStatus_CheckedChanged(sender As Object, e As EventArgs)
        'If IsPostBack Then
        '    Dim sqlLocalTrans As String
        '    sqlLocalTrans = "SELECT [Service],[Amount],Count([TranDate]) as TransactionDate FROM [payit].[dbo].[PINS] Group by TranDate,[Service],Amount"
        '    Session("sqlLocalTrans") = sqlLocalTrans
        '    Dim cmd As SqlCommand = New SqlCommand(sqlLocalTrans)
        '    If chkStatus.Checked = True Then
        '        GridView1.AllowPaging = True
        '        lblPaging.Text = "Disable Paging"
        '    Else
        '        GridView1.AllowPaging = False
        '        lblPaging.Text = "Enable Paging"
        '    End If
        '    GridView1.DataSource = GetData(cmd)
        '    GridView1.DataBind()
        'End If
    End Sub

    Protected Sub ddlService_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlDenom.Enabled = False
        ddlDenom.Items.Clear()

        Dim ProjectId As String = ddlServiceName.SelectedValue
        Dim ServiceName As String = ddlServiceName.SelectedItem.Text
        If ProjectId <> Nothing Then
            Dim query As String = String.Format("select Amount2, Amount " & _
                                                  "from [payit].[dbo].[Denominations] where [ServiceID] like '{0}%' order by cast(Amount as float)", ProjectId)
            BindDropDownList(ddlDenom, query, "Amount2", "Amount", "All")
            ddlDenom.Enabled = True
        End If

        ddlVendor.Enabled = False
        ddlVendor.Items.Clear()

        If (ServiceName <> Nothing AndAlso Not (ServiceName.ToLower.Equals("all"))) Then
            Dim sql As String = String.Format("select DISTINCT case when CHARINDEX('-',PIN)>0 " & _
                                                  "then SUBSTRING(PIN,1,CHARINDEX('-',PIN)-1) else isnull(PIN,'Other Vendor') end 'Vendor' " & _
                                                  "from PINS where [Service] like '{0}%' order by Vendor", ServiceName)
            BindDropDownList(ddlVendor, sql, "Vendor", "Vendor", "All")
            ddlVendor.Enabled = True
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
        ddl.Items.Insert(0, New ListItem(defaultText, "%"))
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
        ExportToExcel.Export("pinHistory.xls", Me.GridView1)
    End Sub
End Class
