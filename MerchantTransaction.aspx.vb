Imports System.Data.SqlClient
Imports System.Data

Partial Class MerchantTransaction
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sqlLocalTrans, Sql As String
    Private grdTotal As Decimal
    Dim priceTotal As Decimal = 0
    Dim fromdate, todate As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Dim typeS As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "checkoutuser" Or Session("role") = "cs" Or Session("role") = "qa") Then
            alert("Unauthorized Access")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=MerchantTransaction.aspx")
        End If
        Dim username As String = String.Empty

        If Not Page.IsPostBack Then
            FromDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")
            ToDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")
            Try
                cn = New SqlConnection(strConnString)
                Sql = "SELECT DISTINCT [ServiceCode] from [payit].[dbo].[PayitServices] where" & _
                      " ([ServiceCode] NOT LIKE '%-C' AND [ServiceCode] NOT LIKE '%-E'  AND [ServiceCode] NOT LIKE '%-O'" & _
                      "AND [ServiceCode] NOT LIKE '%-P' AND [ServiceCode] NOT LIKE '%-X' AND [ServiceCode] NOT LIKE 'ZakatProject' AND [ServiceCode] NOT LIKE '%-Store') order by [ServiceCode]"

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

        sqlLocalTrans = "SELECT TOP 1000 [ID],[IsysID],[MobileNo],[Service],[PaymentAPI],[Amount],[TranDate],[ProcessTranDate],[ProcessTranDescription]" & _
                        " FROM [payit].[dbo].[PayitiSYS] where [TranDate] between '" & fromdate & "' and '" & todate & "'"
        Dim tempSQL As String = ""
        If ddlServiceName.SelectedItem.Text <> "NULL" Then
            If ddlServiceName.SelectedItem.Text = "All" Then
                searchService = "(Service like '%-y' OR Service like '%-vs') "
            Else
                searchService = "Service like '" & Trim(ddlServiceName.SelectedItem.Text) & "' "
            End If
            If tempSQL = "" Then
                tempSQL = searchService
            Else
                tempSQL = tempSQL & " and " & searchService
            End If
        End If

        If Trim(txtRefNo.Text) <> "" And Trim(txtRefNo.Text) <> "NULL" Then
            If tempSQL = "" Then
                tempSQL = "MobileNo like '" & Trim(txtRefNo.Text) & "' "
            Else
                tempSQL = tempSQL & " and MobileNo like '" & Trim(txtRefNo.Text) & "' "
            End If
        End If
        If Trim(txtTrackid.Text) <> "" And Trim(txtTrackid.Text) <> "NULL" Then
            If tempSQL = "" Then
                tempSQL = "IsysID= '" & Trim(txtTrackid.Text) & "' "
            Else
                tempSQL = tempSQL & " and IsysID= '" & Trim(txtTrackid.Text) & "' "
            End If
        End If
        
        If tempSQL <> "" Then
            sqlLocalTrans = sqlLocalTrans & " and " & tempSQL & " order by id desc"
        Else
            sqlLocalTrans = sqlLocalTrans & " order by id desc"
        End If
        Session("sql") = sqlLocalTrans
        BindGrid()


        Dim SqlBind As String = "Select COUNT(ID) as col, Sum(isnull(cast(Amount as float),0)) as Amt from [payit].[dbo].[PayitiSYS] where [TranDate] between '" & fromdate & "' and '" & todate & "'"
        Dim searchSQL As String = ""

        If ddlServiceName.SelectedItem.Text <> "Select" And ddlServiceName.SelectedItem.Text <> "NULL" Then
            If searchSQL = "" Then
                searchSQL = searchService
            Else
                searchSQL = searchSQL & " and " & searchService
            End If
        End If
        If Trim(txtRefNo.Text) <> "" And Trim(txtRefNo.Text) <> "NULL" Then
            If searchSQL = "" Then
                searchSQL = "MobileNo like '" & Trim(txtRefNo.Text) & "' "
            Else
                searchSQL = searchSQL & " and MobileNo like '" & Trim(txtRefNo.Text) & "' "
            End If
        End If
        If Trim(txtTrackid.Text) <> "" And Trim(txtTrackid.Text) <> "NULL" Then
            If searchSQL = "" Then
                searchSQL = "IsysID= '" & Trim(txtTrackid.Text) & "' "
            Else
                searchSQL = searchSQL & " and IsysID= '" & Trim(txtTrackid.Text) & "' "
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
    End Sub

    Private Sub BindData()
        Dim strQuery As String = ("SELECT TOP 1000 [ID],[IsysID],[MobileNo],[Service],[PaymentAPI],[Amount],[TranDate],[ProcessTranDate],[ProcessTranDescription]" & _
                        "FROM [payit].[dbo].[PayitiSYS] order by id desc")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        alert("Refreshing")
        Dim sqlLocalTrans As String
        sqlLocalTrans = "SELECT TOP 1000 [ID],[IsysID],[TransactionID],[MobileNo],[Service],[PaymentAPI],[Amount],[TranDate],[ProcessTranDate],[ProcessTranDescription]" & _
                        "FROM [payit].[dbo].[PayitiSYS] order by id desc"
        Session("sqlLocalTrans") = sqlLocalTrans
        Dim cmd As SqlCommand = New SqlCommand(sqlLocalTrans)
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
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 2, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("MerchantTransactions.xls", Me.GridView1)
    End Sub

    Protected Sub TryAgain(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect(Request.RawUrl)
    End Sub
End Class
