Imports System.Data.SqlClient
Imports System.Data

Partial Class QuickPayTransaction
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
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "cs" Or Session("role") = "qa") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=ServiceTransactions.aspx")
        End If
        Dim username As String = String.Empty

        If Not Page.IsPostBack Then
            FromDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")
            ToDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")
            Try
            Catch ex As Exception
                dberrorlabel.Text = ex.ToString
            End Try
        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        fromdate = Trim(FromDateTextBox.Text) & " 00:00:00"
        todate = Trim(ToDateTextBox.Text) & " 23:59:59"
        cn = New SqlConnection(strConnString)
        Dim searchService As String = String.Empty
       
        sqlLocalTrans = "SELECT TOP 1000 [ID],[TrackID],[MobileNo],[Ptype],[Status],[StatusDesc],[Amount],[Commission]," & _
                        "[TotalCount],[SuccessCount],[FailedCount],[CreatedOn],[ProcessedOn],[UserEmail],[UserName] FROM [payit].[dbo].[QucikPayMaster] WHERE Status > 0 AND [CreatedOn] between '" & fromdate & "' AND '" & todate & "'"
        Dim tempSQL As String = ""
        'If ddlServiceName.SelectedItem.Text <> "NULL" Then
        '    If ddlServiceName.SelectedItem.Text = "All" Then

        '    Else
        '        searchService = "Service like '" & Trim(ddlServiceName.SelectedItem.Text) & "' "
        '    End If

        '    If tempSQL = "" Then
        '        tempSQL = searchService
        '    Else
        '        tempSQL = tempSQL & " and " & searchService
        '    End If
        'End If

        'If (chkSuccess.Checked) Then
        '    If tempSQL = "" Then
        '        tempSQL = " ServiceResult  like '%' "
        '    Else
        '        tempSQL = tempSQL & " and ServiceResult like '%' "
        '    End If
        'Else
        '    If tempSQL = "" Then
        '        tempSQL = " (ServiceResult not like 'success%' or ServiceResult is null) "
        '    Else
        '        tempSQL = tempSQL & " and (ServiceResult not like 'success%' or ServiceResult is null) "
        '    End If
        'End If

        If Trim(txtMobile.Text) <> "" And Trim(txtMobile.Text) <> "NULL" Then
            If tempSQL = "" Then
                tempSQL = "[MobileNo] like '" & Trim(txtMobile.Text) & "' "
            Else
                tempSQL = tempSQL & " and [MobileNo] like '" & Trim(txtMobile.Text) & "' "
            End If
        End If
        If Trim(txtTrackid.Text) <> "" And Trim(txtTrackid.Text) <> "NULL" Then
            If tempSQL = "" Then
                tempSQL = "[TrackID]= " & Trim(txtTrackid.Text) & " "
            Else
                tempSQL = tempSQL & " and [TrackID]= '" & Trim(txtTrackid.Text) & "' "
            End If
        End If
        If Trim(txtAmount.Text) <> "" And Trim(txtAmount.Text) <> "NULL" Then
            If tempSQL = "" Then
                tempSQL = "Amount= '" & Trim(txtAmount.Text) & "' "
            Else
                tempSQL = tempSQL & " and Amount= '" & Trim(txtTrackid.Text) & "' "
            End If
        End If

        If tempSQL <> "" Then
            sqlLocalTrans = sqlLocalTrans & " and " & tempSQL & " order by id desc"
        Else
            sqlLocalTrans = sqlLocalTrans & " order by id desc"
        End If
        Session("sql") = sqlLocalTrans
        BindGrid()


        'Dim SqlBind As String = "Select COUNT(ID) as col, Sum(isnull(cast(AmntWithCommission as float),0)) as Amt from [payit].[dbo].[ThirdpartyServiceTransactions] where  Service NOT LIKE 'QuickPay' AND [PaymentResult] like 'CAPTURED' AND [TransactionDate] between '" & fromdate & "' and '" & todate & "'"
        'Dim searchSQL As String = ""

        'If ddlServiceName.SelectedItem.Text <> "Select" And ddlServiceName.SelectedItem.Text <> "NULL" Then
        '    If searchSQL = "" Then
        '        searchSQL = searchService
        '    Else
        '        searchSQL = searchSQL & " and " & searchService
        '    End If
        'End If
        'If Trim(txtMobile.Text) <> "" And Trim(txtMobile.Text) <> "NULL" Then
        '    If searchSQL = "" Then
        '        searchSQL = "MobileNo like '" & Trim(txtMobile.Text) & "' "
        '    Else
        '        searchSQL = searchSQL & " and MobileNo like '" & Trim(txtMobile.Text) & "' "
        '    End If
        'End If

        'If (chkSuccess.Checked) Then
        '    If searchSQL = "" Then
        '        searchSQL = " ServiceResult  like '%' "
        '    Else
        '        searchSQL = searchSQL & " and ServiceResult like '%' "
        '    End If
        'Else
        '    If searchSQL = "" Then
        '        searchSQL = " (ServiceResult not like 'success%' or ServiceResult is null) "
        '    Else
        '        searchSQL = searchSQL & " and (ServiceResult not like 'success%' or ServiceResult is null) "
        '    End If
        'End If

        'If Trim(txtTrackid.Text) <> "" And Trim(txtTrackid.Text) <> "NULL" Then
        '    If searchSQL = "" Then
        '        searchSQL = "TrackId= " & Trim(txtTrackid.Text) & " "
        '    Else
        '        searchSQL = searchSQL & " and TrackId= " & Trim(txtTrackid.Text) & " "
        '    End If
        'End If

        'If Trim(txtMobile.Text) <> "" And Trim(txtMobile.Text) <> "NULL" Then
        '    If searchSQL = "" Then
        '        searchSQL = " MobileNo like '" & Trim(txtMobile.Text) & "' "
        '    Else
        '        searchSQL = searchSQL & " and MobileNo like '" & Trim(txtMobile.Text) & "' "
        '    End If
        'End If

        'If Trim(txtAmount.Text) <> "" And Trim(txtAmount.Text) <> "NULL" Then
        '    If searchSQL = "" Then
        '        searchSQL = "Amount= '" & Trim(txtAmount.Text) & "' "
        '    Else
        '        searchSQL = searchSQL & " and Amount= '" & Trim(txtAmount.Text) & "' "
        '    End If
        'End If


        'If searchSQL <> "" Then
        '    SqlBind = SqlBind & " and " & searchSQL
        'Else
        '    SqlBind = SqlBind
        'End If

        'Label19.Text = String.Empty
        'Label20.Text = String.Empty
        'da = New SqlDataAdapter(SqlBind, cn)
        'ds = New DataSet()
        'da.Fill(ds, "deta")
        'If Me.IsPostBack Then
        '    If ds.Tables.Count > 0 Then
        '        'Dim s As String = ds.Rows(1)("mycolumn1").ToString()
        '        For Each myRow As DataRow In ds.Tables(0).Rows
        '            If Not IsDBNull(myRow("Amt")) AndAlso myRow("Amt") <> Nothing Then
        '                Label19.Text = "Grand Total: " & myRow("Amt").ToString()
        '            End If
        '            If Not IsDBNull(myRow("col")) AndAlso myRow("col") <> Nothing Then
        '                Label20.Text = "Total Transactions: " & myRow("col").ToString()
        '            End If
        '        Next
        '    End If
        'End If
    End Sub

    Private Sub BindGrid()
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
        Dim totalRows As Integer
        Dim total As Double
        totalRows = GridView1.Rows.Count

        'For i = 0 To totalRows - 1
        '    total = total + Val(GridView1.Rows(i).Cells(3).Text)
        'Next

        'Label17.Text = "Total: " & total.ToString()
        Label18.Text = "Transactions: " & totalRows.ToString()

    End Sub

    Private Sub BindData()
        Dim strQuery As String = ("SELECT TOP 1000 [ID],[TrackID],[MobileNo],[Ptype],[Status],[StatusDesc],[Amount],[Commission]," & _
                                  "[TotalCount],[SuccessCount],[FailedCount],[CreatedOn],[ProcessedOn],[UserEmail],[UserName] FROM [payit].[dbo].[QucikPayMaster] WHERE Status > 0  order by id desc")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
    End Sub

    Private Sub BindTransactionData(ByVal isSuccess As Boolean, ByVal trackId As String, ByVal allTransactions As Boolean)
        Dim strQuery As String

        If (isSuccess) Then
            strQuery = ("SELECT TOP 1000 [TrackID],[QuickPayTrackID],[ServiceCode] AS Service, [MobileNo], [PaymentAmount] AS Amount, [Info1] AS Serial, [ServiceStatusDesc] AS Status " & _
                        ",[CreatedDate],[ProcessedDate] FROM [payit].[dbo].[QuickPayTransactions] WHERE ServiceStatus = 1 and ServiceStatusDesc = 'SUCCESS' and TrackID = " & trackId & " order by id desc")
        Else
            strQuery = ("SELECT TOP 1000 [TrackID],[QuickPayTrackID],[ServiceCode] AS Service, [MobileNo], [PaymentAmount] AS Amount, [Info1] AS Serial, ISNULL([ServiceStatusDesc],'FAIL') AS Status " & _
                        ",[CreatedDate],[ProcessedDate] FROM [payit].[dbo].[QuickPayTransactions] WHERE ServiceStatus = 0 and TrackID = " & trackId & " order by id desc")
        End If

        If (allTransactions) Then
            strQuery = ("SELECT TOP 1000 [TrackID],[QuickPayTrackID],[ServiceCode] AS Service, [MobileNo], [PaymentAmount] AS Amount, [Info1] AS Serial, ISNULL([ServiceStatusDesc],'FAIL') AS Status " & _
                      ",[CreatedDate],[ProcessedDate] FROM [payit].[dbo].[QuickPayTransactions] WHERE TrackID = " & trackId & " order by id desc")
        End If

        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView2.DataSource = GetData(cmd)
        GridView2.DataBind()
    End Sub

    Protected Sub ReprocessLinkButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        Dim lb As LinkButton = TryCast(sender, LinkButton)
        Dim trackIDC = lb.CommandArgument()

        Dim reprocess As String = ""
        Dim ServiceResult = ""
        Dim dd As New Data.payitEntities
        Dim transaction = dd.QuickPayTransactions.FirstOrDefault(Function(x) x.QuickPayTrackID = trackIDC AndAlso x.ServiceStatus = 0)
        If (Not transaction Is Nothing) Then
            Dim proxy As New VWZForAlghanim.ServiceSoapClient
            reprocess = proxy.ProcessTransactionSupportQP(transaction.ServiceCode, -1, transaction.ServiceAmount, transaction.MobileNo, transaction.QuickPayTrackID, "XSMART", "XSMART")
            If Not reprocess.StartsWith("ERROR") Then
                'updateThirdpartyStatus(trackIDC, "SUCCESSRI")
                GridView2.DataBind()
                ServiceResult = "Your transction has been successfully processed"
            Else
                ServiceResult = "An error Occured while processing your request. Please try agan later"
            End If
        Else
            ServiceResult = "Invalid TrackID or transaction is already processed"
        End If
        alert(ServiceResult)
    End Sub

    Protected Sub SuccessLinkButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim Lnk As LinkButton = DirectCast(sender, LinkButton)
        BindTransactionData(True, Lnk.CommandArgument(), False)
        panelHeading.InnerText = "Success Transaction Details for TrackId: " & Lnk.CommandArgument()
        LinkButton1_ModalPopupExtender.Show()
       
    End Sub

    Protected Sub FailLinkButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim Lnk As LinkButton = DirectCast(sender, LinkButton)
        BindTransactionData(False, Lnk.CommandArgument(), False)
        panelHeading.InnerText = "Failed Transaction Details for TrackId: " & Lnk.CommandArgument()
        LinkButton1_ModalPopupExtender.Show()
    End Sub

    Protected Sub TotalLinkButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim Lnk As LinkButton = DirectCast(sender, LinkButton)
        BindTransactionData(True, Lnk.CommandArgument(), True)
        panelHeading.InnerText = "Transaction Details for TrackId: " & Lnk.CommandArgument()
        LinkButton1_ModalPopupExtender.Show()
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        GridView1.PageIndex = e.NewPageIndex
        Me.BindGrid()
    End Sub

    Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim configId As TableCell = e.Row.Cells(2)
            Dim userType As TableCell = e.Row.Cells(3)
            Dim limitType As TableCell = e.Row.Cells(4)
            Dim status As TableCell = e.Row.Cells(6)
            Dim total As LinkButton = e.Row.FindControl("lnkTotal")
            Dim success As LinkButton = e.Row.FindControl("lnkSuccess")
            Dim fail As LinkButton = e.Row.FindControl("lnkFail")

            Dim totalTrans As Integer = total.Text
            Dim successTrans As Integer = success.Text

            'If (totalTrans = successTrans) Then
            '    status.Text = "SUCCESS"
            'ElseIf (successTrans < totalTrans) Then
            '    status.Text = "FAIL"
            'End If


            If status.Text = "1" Then
                status.Text = "PENDING"
            ElseIf status.Text = "2" Then
                status.Text = "IN-PROGRESS"
            ElseIf status.Text = "3" Then
                status.Text = "COMPLETED"
            ElseIf status.Text = "4" Then
                status.Text = "COMPLETED"
            ElseIf status.Text = "5" Then
                status.Text = "COMPLETED"
            End If
            'If userType.Text = "1" Then
            '    userType.Text = "UserId"
            'ElseIf userType.Text = "2" Then
            '    userType.Text = "Mobile Number"
            'ElseIf userType.Text = "5" Then
            '    userType.Text = "DeviceId"
            'End If

            'If limitType.Text = "1" Then
            '    limitType.Text = "Transaction Count"
            'ElseIf limitType.Text = "2" Then
            '    limitType.Text = "Total Amount"
            'End If
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
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.alert('" & s & "')", True)
    End Sub
    Public Sub alertNotify(s As String)
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertNotify", "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 2, function(){  console.log('dismissed'); });", True)
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("QuickPay.xls", Me.GridView1)
    End Sub

    Protected Sub TryAgain(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect(Request.RawUrl)
    End Sub
End Class
