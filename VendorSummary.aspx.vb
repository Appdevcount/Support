Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing
Imports OfficeOpenXml

Partial Class VendorSummary
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Sql As String
        cn = New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations") Then
            Response.Redirect("~/login.aspx?ReturnURL=VendorSummary.aspx")
        End If

        If Not IsPostBack Then
            Sql = "Select ServiceCode from PayitServices where [type] like 'PIN' and (ServiceCode NOT LIKE 'iTunesB-O%') order by ServiceCode"
            da = New SqlDataAdapter(Sql, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")
            ddlService.DataSource = ds.Tables("deta")
            ddlService.DataTextField = "ServiceCode"
            ddlService.DataValueField = "ServiceCode"
            ddlService.DataBind()
            ddlService.Items.Insert(0, New ListItem("All", "All"))

            ddlVendor.Enabled = False
        End If
        If Page.IsPostBack = False Then
            Label14.Visible = False
            Label15.Visible = False
            FromDateTextBox.Text = Format(Date.Today.Date, "dd/MM/yyyy")
            ToDateTextBox.Text = Format(Date.Today.Date, "dd/MM/yyyy")
            Label17.Text = ""
        End If
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        counter1()
        Me.BindData()
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        Me.BindData()
        'Dim s As String
        's = Session("sqlLocalTrans")
        'sqldatabind(s)
    End Sub

    Private Sub BindData()
        Dim strQuery As String
        Dim fromdate, todate As String
        Dim grandTotSql As String = ""
        If Trim(FromDateTextBox.Text) = "" Or Trim(ToDateTextBox.Text) = "" Then
            Label14.Visible = True
            Label15.Visible = True
            Exit Sub
        Else
            Label14.Visible = False
            Label15.Visible = False
            fromdate = Trim(FromDateTextBox.Text) & " 00:00:00"
            todate = Trim(ToDateTextBox.Text) & " 23:59:59"

        End If
        Dim service As String = ddlService.SelectedItem.Text
        Dim vendor As String
        If (ddlVendor.Items.Count > 0) Then
            vendor = ddlVendor.SelectedItem.Text
        Else
            vendor = ""
        End If

        If vendor.ToLower().Equals("all") Then
            vendor = " (Vendor like '%')"
        Else
            vendor = " Vendor like '" & vendor & "'"
        End If

        If (chkSummary.Checked) Then
            If service <> "All" Then
                strQuery = "SELECT Service, case when CHARINDEX('-',PIN)>0 " & _
                                "then SUBSTRING(PIN,1,CHARINDEX('-',PIN)-1) else isnull(PIN,'Other Vendor') end 'Vendor' ,Amount2 Denomination,Amount SellingPrice,count(*) 'Quantity Sold', (Amount * count(*)) as Total " & _
                                "from payit.dbo.PINS where service like '" & service & "' and status=1 and (convert(datetime,OrderDate, 103) " & _
                                "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103)) and " & vendor & " group by Service,Amount,Amount2,case when CHARINDEX('-',PIN)>0 " & _
                                "then SUBSTRING(PIN,1,CHARINDEX('-',PIN)-1) else isnull(PIN,'Other Vendor') end order by Vendor,service,Amount,Amount2"
            Else
                strQuery = " SELECT Service, case when CHARINDEX('-',PIN)>0 " & _
                                "then SUBSTRING(PIN,1,CHARINDEX('-',PIN)-1) else isnull(PIN,'Other Vendor') end 'Vendor' ,Amount2 Denomination,Amount SellingPrice,count(*) 'Quantity Sold', (Amount * count(*)) as Total " & _
                                "from payit.dbo.PINS where status=1 and (convert(datetime,OrderDate, 103) " & _
                                "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103)) group by Service,Amount,Amount2,case when CHARINDEX('-',PIN)>0 " & _
                                "then SUBSTRING(PIN,1,CHARINDEX('-',PIN)-1) else isnull(PIN,'Other Vendor') end order by Vendor,service,Amount,Amount2"
            End If
        Else
            If (service <> "All") Then
                strQuery = "SELECT [Service],[Vendor],d.Amount2 AS Denomination,[SellingPrice],[OfferPrice],Quantity,count(*)/Quantity AS 'Sold', " & _
                             " cast(round(([OfferPrice] * count(*)),3) AS numeric(36,3)) AS Total " & _
                             " FROM [payit].[dbo].[PinTransactionsDetails] AS ptd  " & _
                             " JOIN [payit].[dbo].[PinTransactions] AS pt ON ptd.PinTransactionId = pt.ID " & _
                             " JOIN [payit].[dbo].[Services] AS s ON pt.Service = s.ServiceName " & _
                             " JOIN [payit].[dbo].[Denominations] AS d ON s.ServiceID = d.ServiceID and d.Amount = pt.SellingPrice " & _
                             " WHERE ptd.ServiceResult like 'SUCCESS' AND (convert(datetime,TransactionDate, 103) between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103)) " & _
                             " and service like '" & service & "' and " & vendor & " " & _
                             " GROUP BY [Service], [SellingPrice], d.Amount2, Quantity, OfferPrice, Vendor ORDER BY [Service], [SellingPrice], d.Amount2, Quantity, OfferPrice, Vendor"
            Else
                strQuery = "SELECT [Service],[Vendor],d.Amount2 AS Denomination,[SellingPrice],[OfferPrice],Quantity,count(*)/Quantity AS 'Sold', " & _
                            " cast(round(([OfferPrice] * count(*)),3) AS numeric(36,3)) AS Total " & _
                            " FROM [payit].[dbo].[PinTransactionsDetails] AS ptd  " & _
                            " JOIN [payit].[dbo].[PinTransactions] AS pt ON ptd.PinTransactionId = pt.ID " & _
                            " JOIN [payit].[dbo].[Services] AS s ON pt.Service = s.ServiceName " & _
                            " JOIN [payit].[dbo].[Denominations] AS d ON s.ServiceID = d.ServiceID and d.Amount = pt.SellingPrice " & _
                            " WHERE ptd.ServiceResult like 'SUCCESS' AND (convert(datetime,TransactionDate, 103) between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103)) " & _
                            " GROUP BY [Service], [SellingPrice], d.Amount2, Quantity, OfferPrice, Vendor ORDER BY [Service], [SellingPrice], d.Amount2, Quantity, OfferPrice, Vendor"
            End If
        End If

        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
        Dim totalRows As Integer
        Dim grandTotal, TotalTrans As Double
        totalRows = GridView1.Rows.Count
        grandTotal = 0
        Dim ggValue As Integer = 7
        Dim ttValue As Integer = 6
        If (chkSummary.Checked) Then
            ggValue = 5
            ttValue = 4
        End If
        For i = 0 To totalRows - 1

            grandTotal = grandTotal + Val(GridView1.Rows(i).Cells(ggValue).Text)
            TotalTrans = TotalTrans + Val(GridView1.Rows(i).Cells(ttValue).Text)
        Next
        Label17.Text = "Total Amount: " & grandTotal
        Label18.Text = "Total Transactions:  " & TotalTrans
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

    Public Sub sqldatabind(ByVal s As String)
        da = New SqlDataAdapter(s, cn)
        ds = New DataSet()
        da.Fill(ds, "det")
        GridView1.DataSource = ds.Tables("det")
        GridView1.DataBind()
    End Sub

    Protected Sub ddlService_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlVendor.Enabled = False
        ddlVendor.Items.Clear()
        Dim ProjectId As String = ddlService.SelectedItem.Text
        If ProjectId <> Nothing Then
            Dim query As String = String.Format("select DISTINCT case when CHARINDEX('-',PIN)>0 " & _
                                                  "then SUBSTRING(PIN,1,CHARINDEX('-',PIN)-1) else isnull(PIN,'Other Vendor') end 'Vendor' " & _
                                                  "from PINS where [Service] like '{0}%' order by Vendor", ProjectId)
            BindDropDownList(ddlVendor, query, "Vendor", "Vendor", "All")
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
        ddl.Items.Insert(0, New ListItem(defaultText, "0"))
    End Sub

    Public Sub counter1()
        Button2.Attributes.Add("onclick", "javascript:abct()")
        GridView1.Attributes.Add("databinding", "javascript:abct()")
    End Sub

    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
                                              "alert('" & s & "')" & _
                                              "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("VendorSummary.xls", Me.GridView1)
    End Sub

End Class
