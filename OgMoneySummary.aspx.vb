Imports System.Data.SqlClient
Imports System.Data

Partial Class OgMoneySummary
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sqlLocalTrans, Sql As String
    Private grdTotal As Decimal
    Dim priceTotal As Decimal = 0
    Dim fromdate, todate As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "qa" Or Session("user") = "zain") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=DealerTransactions.aspx")
        End If

        If Not Page.IsPostBack Then
            FromDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")
            ToDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")
            Try
                ddlServiceName.Items.Insert(0, New ListItem("ZAIN PREPAID", "ZAIN-X"))
                ddlServiceName.Items.Insert(1, New ListItem("ZAIN VOUCHERS", "ZAIN-O"))
                ddlServiceName.SelectedIndex = 0
                ddlServiceName_SelectedIndexChanged(sender, e)
            Catch ex As Exception
            End Try
        End If
    End Sub

    Protected Sub ddlServiceName_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlDenomination.Enabled = False
        ddlDenomination.Items.Clear()
        Dim ProjectId As String = ddlServiceName.SelectedItem.Value
        Dim query As String = String.Empty

        If (Not (String.IsNullOrEmpty(ProjectId))) Then
            query = String.Format("SELECT d.[ID],s.[ServiceName],[Amount],[Amount2],[Status] FROM [payit].[dbo].[Denominations] d " & _
                                  "JOIN [payit].[dbo].[Services] s on d.ServiceID = s.ServiceID WHERE s.ServiceName = '" & ProjectId & "' ORDER BY CAST([Amount] AS float)")
            BindDropDownList(ddlDenomination, query, "Amount2", "Amount", "All")
            ddlDenomination.Enabled = True
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
                ddl.Items.Insert(0, New ListItem(defaultText, "All"))
            End Using
        End Using
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        fromdate = Trim(FromDateTextBox.Text) & " 00:00:00"
        todate = Trim(ToDateTextBox.Text) & " 23:59:59"
        cn = New SqlConnection(strConnString)
        Dim searchService As String = String.Empty
        'Dim filter As String = String.Empty
        'If (ddlServiceName.SelectedItem.Value.ToLower.EndsWith("-x")) Then
        '    filter = "ServiceResult like 'SUCCESS%'"
        'Else
        '    filter = "ServiceResult like 'SUCCESS%'"
        'End If
        'sqlLocalTrans = "SELECT [Service], Amount AS Denomination,Count(*) AS 'No. of Transactions', SUM(Amount) AS 'Total Amount' " & _
        '                      "FROM [payit].[dbo].[PayitiSYS] where (ProcessTranDate BETWEEN '" & fromdate & "' AND '" & todate & "') " & _
        '                      "AND " & filter & " AND [Service] like '" & ddlServiceName.SelectedItem.Value.Trim() & "' "


        'SELECT [Service], Amount AS Denomination, Count(*) AS 'No. of Transactions', SUM(CAST(Amount AS float)) AS 'Total Amount' 
        'FROM [payit].[dbo].[ThirdpartyServiceTransactions] WHERE (transactiondate BETWEEN '2017-05-20 00:00:00' AND '2017-05-29 23:59:59') AND 
        'Amount IS NOT NULL AND ServiceResult like 'SUCCESS%' AND [Service] like 'ZAIN-O'  GROUP BY [Service], Amount ORDER BY CAST(Amount as float)


        sqlLocalTrans = "SELECT [Service], Amount AS Denomination, Count(*) AS 'No. of Transactions', SUM(CAST(Amount AS float)) AS 'Total Amount' " & _
                            "FROM [payit].[dbo].[ThirdpartyServiceTransactions] WHERE (TransactionDate BETWEEN '" & fromdate & "' AND '" & todate & "') " & _
                            "AND Amount IS NOT NULL AND ServiceResult like 'SUCCESS%' AND [Service] like '" & ddlServiceName.SelectedItem.Value.Trim() & "' "

        Dim tempSQL As String = ""
        If ddlDenomination.SelectedItem.Text <> "" Then
            If ddlDenomination.SelectedItem.Text = "All" Then
            Else
                searchService = "Amount='" & Trim(ddlDenomination.SelectedItem.Value) & "' "
            End If

            If tempSQL = "" Then
                tempSQL = searchService
            Else
                tempSQL = tempSQL & " and " & searchService
            End If
        End If

        If tempSQL <> "" Then
            sqlLocalTrans = sqlLocalTrans & " AND " & tempSQL & " GROUP BY [Service], Amount ORDER BY CAST(Amount as float)"
        Else
            sqlLocalTrans = sqlLocalTrans & " GROUP BY [Service], Amount ORDER BY CAST(Amount as float)"
        End If
        Session("sql") = sqlLocalTrans
        BindGrid()
    End Sub

    Private Sub BindGrid()
        Try
            If (chkStatus.Checked = True) Then
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
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BindData()
        fromdate = Trim(FromDateTextBox.Text)
        todate = Trim(ToDateTextBox.Text)
      
        Dim strQuery As String = "SELECT CASE WHEN [Service] = 'ZAIN-O' THEN 'ZAIN VOURCHERS' ELSE 'ZAIN PREPAID' END AS [Service], " & _
                                "Amount as Denomination, SUM(Amount) as 'Total Amount',Count(*) 'Transactions Count', " & _
                                "CASE WHEN PaymentAPI = 'iSYS-POS' THEN 'DEALER' ELSE 'OG Money KW' END AS [Source] " & _
                                "FROM [payit].[dbo].[PayitiSYS] where " & _
                                "(ProcessTranDescription like 'SUCCESS%' OR ProcessTranDescription like 'OK%') and ([Service] = 'Zain-O' or [Service] = 'Zain-X') " & _
                                "GROUP BY [Service], Amount, PAymentAPI order by [Service], Denomination"
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()

        Session("sql") = strQuery
    End Sub

    Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim service As TableCell = e.Row.Cells(0)
           
            If service.Text.ToUpper().EndsWith("-X") Then
                service.Text = "ZAIN PREPAID"
            ElseIf service.Text.ToUpper().EndsWith("-P") Then
                service.Text = "ZAIN POSTPAID"
            ElseIf service.Text.ToUpper().EndsWith("-O") Then
                service.Text = "ZAIN VOUCHER"
            End If

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

    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
                      "alertify.alert('" & s & "')" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Public Sub alertNotify(s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 2, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("OgMoneyTransactionSummary.xls", Me.GridView1)
    End Sub

    Protected Sub TryAgain(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect(Request.RawUrl)
    End Sub

End Class