Imports System.Data.SqlClient
Imports System.Data

Partial Class DealerTransactions
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

            'Me.BindData()

            Try
                cn = New SqlConnection(strConnString)
                ddlServiceName.Items.Insert(0, New ListItem("All", "All"))
                ddlServiceName.Items.Insert(1, New ListItem("ZAIN PREPAID", "ZAIN-X"))
                ddlServiceName.Items.Insert(2, New ListItem("ZAIN VOUCHERS", "ZAIN-O"))
                ddlServiceName.SelectedIndex = 1
                ddlServiceName_SelectedIndexChanged(sender, e)

                Dim Sql1 = "SELECT a.[AgentName], a.Id FROM [PayitGlobalDealersDB].[dbo].[Dealers] d " & _
                            "JOIN [PayitGlobalDealersDB].[dbo].[Agents] a ON d.id = a.DealerId WHERE d.Status = 1 AND CountryID = 4"
                Dim da1 = New SqlDataAdapter(Sql1, cn)
                Dim ds1 = New DataSet()
                da1.Fill(ds1, "desta")
                ddlDealer.DataSource = ds1.Tables("desta")
                ddlDealer.DataTextField = "AgentName"
                ddlDealer.DataValueField = "Id"
                ddlDealer.DataBind()
                ddlDealer.Items.Insert(0, New ListItem("All", "All"))
                ddlDealer.Items.Insert(1, New ListItem("og_customersupport", "cunit"))
                ddlDealer.Items.Insert(2, New ListItem("og_saad", "transferto"))

            Catch ex As Exception
            End Try
        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        fromdate = Trim(FromDateTextBox.Text) & " 00:00:00"
        todate = Trim(ToDateTextBox.Text) & " 23:59:59"
        cn = New SqlConnection(strConnString)
        Dim searchService As String = String.Empty
        Dim denom As String = String.Empty
        Dim service As String = String.Empty
        Dim dealer As String = String.Empty
        Dim dealer1 As String = String.Empty

        If (ddlDealer.SelectedItem.Value = "All") Then
        Else
            dealer = " AND UserName LIKE '" & ddlDealer.SelectedItem.Value & "'"
            dealer1 = " AND AgentId = " & ddlDealer.SelectedItem.Value & " "
        End If

        If (ddlDenomination.SelectedItem.Value = "All") Then
            denom = ""
        Else
            denom = " AND Amount = " & ddlDenomination.SelectedValue & ""
        End If

        If (ddlServiceName.SelectedItem.Value = "All") Then
            service = " AND (ServiceCode LIKE 'ZAIN-X'  OR SERVICECODE LIKE 'ZAIN-O')"
        Else
            service = " AND (ServiceCode LIKE '" & ddlServiceName.SelectedValue & "')"
        End If
        sqlLocalTrans = "SELECT [Username] AS 'Dealer Name',[ServiceCode] AS Service,[Amount] AS Denomination,Count(*) AS 'No. Of Transactions',SUM(Amount) AS 'Total Amount' " & _
                                 "FROM [payit].[dbo].[DPayitTPTransactions] WHERE ([TranDate] BETWEEN '" & fromdate & "' AND '" & todate & "') " & dealer & "  " & service & " " & denom & " AND Status = 'SUCCESS' GROUP BY [Username],ServiceCode,Amount UNION " & _
                                 "SELECT [AgentName] As 'Dealer Name',[ServiceCode] AS Service,Amount AS Denomination,Count(*) As 'No. Of Transactions',SUM(Amount) AS 'Total Amount' " & _
                                 "FROM [PayitGlobalDealersDB].[dbo].[DealerTransactions] WHERE ([TransactionDate] BETWEEN '" & fromdate & "' AND '" & todate & "') " & dealer1 & " " & service & "  " & denom & " AND Details = 'SUCCESS' GROUP BY [AgentName],Amount,[ServiceCode] "
        'Dim tempSQL As String = ""
        'If ddlServiceName.SelectedItem.Text <> "" Then
        '    If ddlServiceName.SelectedItem.Text = "All" Then
        '    Else
        '        searchService = "ServiceCode like '" & Trim(ddlServiceName.SelectedItem.Value) & "' "
        '    End If

        '    If tempSQL = "" Then
        '        tempSQL = searchService
        '    Else
        '        tempSQL = tempSQL & " and " & searchService
        '    End If
        'End If
       
        'If Trim(txtMobile.Text) <> "" And Trim(txtMobile.Text) <> "NULL" Then
        '    If tempSQL = "" Then
        '        tempSQL = "MobileNumber like '" & Trim(txtMobile.Text) & "' "
        '    Else
        '        tempSQL = tempSQL & " and MobileNumber like '" & Trim(txtMobile.Text) & "' "
        '    End If
        'End If

        'If Trim(txtAmount.Text) <> "" And Trim(txtAmount.Text) <> "NULL" Then
        '    If tempSQL = "" Then
        '        tempSQL = "Amount= '" & Trim(txtAmount.Text) & "' "
        '    Else
        '        tempSQL = tempSQL & " and Amount= '" & Trim(txtAmount.Text) & "' "
        '    End If
        'End If

        'If tempSQL <> "" Then
        '    sqlLocalTrans = sqlLocalTrans & " and " & tempSQL & " order by id desc"
        'Else
        '    sqlLocalTrans = sqlLocalTrans & " order by id desc"
        'End If
        Session("sql") = sqlLocalTrans
        BindGrid()
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
                ddlDenomination.Items.Insert(0, New ListItem("All", "All"))
            End Using
        End Using
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
        fromdate = Trim(FromDateTextBox.Text)
        todate = Trim(ToDateTextBox.Text)
        Dim strQuery As String = "SELECT [Username] AS DealerName,[ServiceCode] AS Service,[Amount] AS Denomination,Count(*) AS 'No. Of Transactions', SUM(Amount) AS 'Total Amount' " & _
                                 "FROM [payit].[dbo].[DPayitTPTransactions] WHERE (ServiceCode LIKE 'ZAIN-X'  OR SERVICECODE LIKE 'ZAIN-O') AND (CAST([TranDate] AS DATE) BETWEEN '" & fromdate & "' AND '" & todate & "') AND Status = 'SUCCESS' GROUP BY [Username],ServiceCode,Amount UNION " & _
                                 "SELECT DealerName As 'Dealer Name',[ServiceCode] AS Service,Amount AS Denomination,Count(*) As 'No. Of Transactions', SUM(Amount) AS 'Total Amount' " & _
                                 "FROM [PayitGlobalDealersDB].[dbo].[DealerTransactions] WHERE (ServiceCode LIKE 'ZAIN-X'  OR SERVICECODE LIKE 'ZAIN-O') AND (CAST([TransactionDate] AS DATE) BETWEEN '" & fromdate & "' AND '" & todate & "') AND Details = 'SUCCESS' GROUP BY DealerName,Amount,[ServiceCode] " & _
                                 "ORDER BY DealerName"
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()

        Session("sql") = strQuery
    End Sub

    Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim user As TableCell = e.Row.Cells(0)
            Dim service As TableCell = e.Row.Cells(1)
            If user.Text = "transferto" Then
                user.Text = "og_saad"
            ElseIf user.Text = "cunit" Then
                user.Text = "og_customersupport"
            ElseIf user.Text = "cupayment" Then
                user.Text = "og_customersupport"
            End If
            If service.Text.ToUpper().EndsWith("-X") Then
                service.Text = "ZAIN PREPAID"
            ElseIf service.Text.ToUpper().EndsWith("-P") Then
                service.Text = "ZAIN POSTPAID"
            ElseIf service.Text.ToUpper().EndsWith("-O") Then
                service.Text = "ZAIN VOUCHER"
            End If

        End If
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        GridView1.PageIndex = e.NewPageIndex
        Me.BindGrid()
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
        ExportToExcel.Export("DealerTransactions.xls", Me.GridView1)
    End Sub

    Protected Sub TryAgain(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect(Request.RawUrl)
    End Sub


End Class
