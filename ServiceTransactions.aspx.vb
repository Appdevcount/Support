Imports System.Data.SqlClient
Imports System.Data

Partial Class ServiceTransactions
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
                cn = New SqlConnection(strConnString)
                Sql = "SELECT DISTINCT [ServiceCode] from [payit].[dbo].[PayitServices] order by ServiceCode"

                da = New SqlDataAdapter(Sql, cn)
                ds = New DataSet()
                da.Fill(ds, "deta")
                ddlServiceName.DataSource = ds.Tables("deta")
                ddlServiceName.DataTextField = "ServiceCode"
                ddlServiceName.DataValueField = "ServiceCode"
                ddlServiceName.DataBind()
                ddlServiceName.Items.Insert(0, New ListItem("All", ""))
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
        'sqlLocalTrans = "SELECT TOP 1000 [TrackId],[Service] AS ServiceCode,[MobileNo],[Amount],[Quantity] as Qnty,[AmntWithCommission] as Total,isnull([ServiceResult],'Unprocessed') as [ServiceResult],[PaymentResult],[TransactionDate]" & _
        '               " FROM [payit].[dbo].[ThirdpartyServiceTransactions] where [PaymentResult] like 'CAPTURED' AND [TransactionDate] between '" & fromdate & "' AND '" & todate & "'"
        sqlLocalTrans = "SELECT TOP 1000 [TrackId],[Service] AS ServiceCode,[MobileNo],[AmntWithCommission] as Amount,isnull([ServiceResult],'Unprocessed') as [ServiceResult],[PaymentResult],[TransactionDate]" & _
                        " FROM [payit].[dbo].[ThirdpartyServiceTransactions] where Service NOT LIKE 'QuickPay' AND ([PaymentResult] LIKE 'CAPTURED' OR [PaymentResult] LIKE 'Approved%') AND [TransactionDate] between '" & fromdate & "' AND '" & todate & "'"
        Dim tempSQL As String = ""
        If ddlServiceName.SelectedItem.Text <> "NULL" Then
            If ddlServiceName.SelectedItem.Text = "All" Then

            Else
                searchService = "Service like '" & Trim(ddlServiceName.SelectedItem.Text) & "' "
            End If

            If tempSQL = "" Then
                tempSQL = searchService
            Else
                tempSQL = tempSQL & " and " & searchService
            End If
        End If

        If (chkSuccess.Checked) Then
            If tempSQL = "" Then
                tempSQL = " ServiceResult  like '%' "
            Else
                tempSQL = tempSQL & " and ServiceResult like '%' "
            End If
        Else
            If tempSQL = "" Then
                tempSQL = " (ServiceResult not like 'success%' or ServiceResult is null) "
            Else
                tempSQL = tempSQL & " and (ServiceResult not like 'success%' or ServiceResult is null) "
            End If
        End If
        If Trim(txtMobile.Text) <> "" And Trim(txtMobile.Text) <> "NULL" Then
            If tempSQL = "" Then
                tempSQL = "MobileNo like '" & Trim(txtMobile.Text) & "' "
            Else
                tempSQL = tempSQL & " and MobileNo like '" & Trim(txtMobile.Text) & "' "
            End If
        End If
        If Trim(txtTrackid.Text) <> "" And Trim(txtTrackid.Text) <> "NULL" Then
            If tempSQL = "" Then
                tempSQL = "TrackId= " & Trim(txtTrackid.Text) & " "
            Else
                tempSQL = tempSQL & " and TrackId= '" & Trim(txtTrackid.Text) & "' "
            End If
        End If
        If Trim(txtAmount.Text) <> "" And Trim(txtAmount.Text) <> "NULL" Then
            If tempSQL = "" Then
                tempSQL = "Amount= '" & Trim(txtAmount.Text) & "' "
            Else
                tempSQL = tempSQL & " and Amount= '" & Trim(txtAmount.Text) & "' "
            End If
        End If

        If tempSQL <> "" Then
            sqlLocalTrans = sqlLocalTrans & " and " & tempSQL & " order by id desc"
        Else
            sqlLocalTrans = sqlLocalTrans & " order by id desc"
        End If
        Session("sql") = sqlLocalTrans
        BindGrid()


        Dim SqlBind As String = "Select COUNT(ID) as col, Sum(isnull(cast(AmntWithCommission as float),0)) as Amt from [payit].[dbo].[ThirdpartyServiceTransactions] where  Service NOT LIKE 'QuickPay' AND [PaymentResult] like 'CAPTURED' AND [TransactionDate] between '" & fromdate & "' and '" & todate & "'"
        Dim searchSQL As String = ""

        If ddlServiceName.SelectedItem.Text <> "Select" And ddlServiceName.SelectedItem.Text <> "NULL" Then
            If searchSQL = "" Then
                searchSQL = searchService
            Else
                searchSQL = searchSQL & " and " & searchService
            End If
        End If
        If Trim(txtMobile.Text) <> "" And Trim(txtMobile.Text) <> "NULL" Then
            If searchSQL = "" Then
                searchSQL = "MobileNo like '" & Trim(txtMobile.Text) & "' "
            Else
                searchSQL = searchSQL & " and MobileNo like '" & Trim(txtMobile.Text) & "' "
            End If
        End If

        If (chkSuccess.Checked) Then
            If searchSQL = "" Then
                searchSQL = " ServiceResult  like '%' "
            Else
                searchSQL = searchSQL & " and ServiceResult like '%' "
            End If
        Else
            If searchSQL = "" Then
                searchSQL = " (ServiceResult not like 'success%' or ServiceResult is null) "
            Else
                searchSQL = searchSQL & " and (ServiceResult not like 'success%' or ServiceResult is null) "
            End If
        End If

        If Trim(txtTrackid.Text) <> "" And Trim(txtTrackid.Text) <> "NULL" Then
            If searchSQL = "" Then
                searchSQL = "TrackId= " & Trim(txtTrackid.Text) & " "
            Else
                searchSQL = searchSQL & " and TrackId= " & Trim(txtTrackid.Text) & " "
            End If
        End If

        If Trim(txtAmount.Text) <> "" And Trim(txtAmount.Text) <> "NULL" Then
            If searchSQL = "" Then
                searchSQL = "Amount= '" & Trim(txtAmount.Text) & "' "
            Else
                searchSQL = searchSQL & " and Amount= '" & Trim(txtAmount.Text) & "' "
            End If
        End If


        If searchSQL <> "" Then
            SqlBind = SqlBind & " and " & searchSQL
        Else
            SqlBind = SqlBind
        End If

        Label19.Text = String.Empty
        Label20.Text = String.Empty
        da = New SqlDataAdapter(SqlBind, cn)
        ds = New DataSet()
        da.Fill(ds, "deta")
        If Me.IsPostBack Then
            If ds.Tables.Count > 0 Then
                'Dim s As String = ds.Rows(1)("mycolumn1").ToString()
                For Each myRow As DataRow In ds.Tables(0).Rows
                    If Not IsDBNull(myRow("Amt")) AndAlso myRow("Amt") <> Nothing Then
                        Label19.Text = "Grand Total: " & myRow("Amt").ToString()
                    End If
                    If Not IsDBNull(myRow("col")) AndAlso myRow("col") <> Nothing Then
                        Label20.Text = "Total Transactions: " & myRow("col").ToString()
                    End If
                Next
            End If
        End If
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

        For i = 0 To totalRows - 1
            total = total + Val(GridView1.Rows(i).Cells(3).Text)
        Next

        Label17.Text = "Total: " & total.ToString()
        Label18.Text = "Transactions: " & totalRows.ToString()

    End Sub

    Private Sub BindData()
        Dim strQuery As String = ("SELECT  TOP 1000 [TrackId],[Service] AS ServiceCode,[MobileNo],[Amount],[Quantity] as Qnty,[AmntWithCommission] as Total,[ServiceResult],[PaymentResult],[TransactionDate]" & _
                        " FROM [payit].[dbo].[ThirdpartyServiceTransactions] order by id desc")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
    End Sub

    Protected Sub ReprocessLinkButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        Dim lb As LinkButton = TryCast(sender, LinkButton)
        Dim trackIDC = lb.CommandArgument()
        alertNotify("Coming Soon!")
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
        ExportToExcel.Export("ServiceTransactions.xls", Me.GridView1)
    End Sub

    Protected Sub TryAgain(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect(Request.RawUrl)
    End Sub
End Class
