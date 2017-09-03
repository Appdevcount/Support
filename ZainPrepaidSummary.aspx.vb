Imports System.Data
Imports System.Data.SqlClient
Partial Class ZainPrepaidSummary
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim username As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Sql As String
        cn = New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("user") = "zain") Then
            Response.Redirect("~/login.aspx?ReturnURL=ZainPrepaidSummary.aspx")
        End If
        username = Session("user")

        If Not IsPostBack Then
            If (username.Equals("zain")) Then
                Sql = "Select [ServiceName], ServiceCode from PayitServices where ServiceCode like 'zain-x' or ServiceCode like 'zain-o'"
            Else
                Sql = "Select [ServiceName], ServiceCode from PayitServices where (Type='PIN' or Type='ZakatProject' or ServiceCode like '%Zain-X%') And (ServiceCode NOT LIKE '%-Z%' and ServiceCode NOT LIKE '%iTunesB-O%') order by ServiceOrder desc"
            End If
            da = New SqlDataAdapter(Sql, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")
            ddlService.DataSource = ds.Tables("deta")
            ddlService.DataTextField = "ServiceName"
            ddlService.DataValueField = "ServiceCode"
            ddlService.DataBind()
            ddlService.Items.Insert(0, New ListItem("All", "All"))



        End If
        If Page.IsPostBack = False Then
            counter1()
            Label14.Visible = False
            Label15.Visible = False
            FromDateTextBox.Text = Format(Date.Today.Date, "dd/MM/yyyy")
            ToDateTextBox.Text = Format(Date.Today.Date, "dd/MM/yyyy")
            Label17.Text = ""

        End If
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        counter1()
        Dim sqlLocalTrans As String
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
        If ddlService.SelectedItem.Text = "All" Then
            If (username.Equals("zain")) Then
                sqlLocalTrans = "SELECT CASE WHEN [Service] = 'ZAIN-O' THEN 'ZAIN VOURCHERS' ELSE 'ZAIN PREPAID' END AS [Service], " & _
                                "Amount as Denomination, SUM(Amount) as 'Total Amount',Count(*) 'Transactions Count', " & _
                                "CASE WHEN PaymentAPI = 'iSYS-POS' THEN 'DEALER' ELSE 'OG Money KW' END AS [Source] " & _
                                "FROM [payit].[dbo].[PayitiSYS] where convert(datetime,ProcessTranDate, 103) " & _
                                "BETWEEN convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) AND " & _
                                "(ProcessTranDescription like 'SUCCESS%' OR ProcessTranDescription like 'OK%') and ([Service] = 'Zain-O' or [Service] = 'Zain-X') " & _
                                "GROUP BY [Service], Amount, PAymentAPI order by [Service], Denomination"

                grandTotSql = "Select Substring(udf4, 1,Charindex('|', udf4)-1) as 'Multiple of PINS', COUNT(Substring(udf4, 1,Charindex('|', udf4)-1)) as 'Total PINS Sold'" & _
                               "from ThirdParty_knet_trans where (udf4 like '%|%' and udf4 is not null) and (knetprocess like 'CAPTURED') and udf1 like 'success%' group by Substring(udf4, 1,Charindex('|', udf4)-1) order by Substring(udf4, 1,Charindex('|', udf4)-1)"
            Else
                sqlLocalTrans = "select p.[Service],cast(Substring(th.udf4,Charindex('|', th.udf4)+1,7) as float) as Denomination,p.PaymentAPI, sum(cast(Substring(th.udf4,Charindex('|', th.udf4)+1,7) as float)) as 'TotalAmount',Count(*) TotalTransactions " & _
                    "from  PayitiSYS p join ThirdParty_knet_trans th on th.trackid  = p.IsysID   where convert(datetime,ProcessTranDate, 103) " & _
                    "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and id>=26 " & _
                    "and (p.ProcessTranDescription like '%SUCCESS%' OR p.ProcessTranDescription like 'OK%') and th.udf4 is not null and (p.[Service] like 'iTunes-O' or p.[Service] like 'CashU-O' or p.[Service] like 'OneCard-O' or p.[Service] like 'VIVA-O' or p.[Service] like 'Zain-O' or p.[Service] like 'Wataniya-O' or p.[Service] like 'Zain-X')" & _
                    "group by p.[Service],Substring(th.udf4,Charindex('|', th.udf4)+1,7),p.PAymentAPI order by Denomination"
                grandTotSql = "Select Substring(udf4, 1,Charindex('|', udf4)-1) as 'Multiple of PINS', COUNT(Substring(udf4, 1,Charindex('|', udf4)-1)) as 'Total PINS Sold'" & _
                               "from ThirdParty_knet_trans where (udf4 like '%|%' and udf4 is not null) and (knetprocess like 'CAPTURED') and udf1 like 'success%' group by Substring(udf4, 1,Charindex('|', udf4)-1) order by Substring(udf4, 1,Charindex('|', udf4)-1)"
            End If
           
        Else
            If (username.Equals("zain")) Then
                sqlLocalTrans = "select CASE WHEN [Service] = 'ZAIN-O' THEN 'ZAIN VOURCHERS' ELSE 'ZAIN PREPAID' END AS [Service], " & _
                                "Amount as Denomination, SUM(Amount) as 'Total Amount',Count(*) 'Transactions Count', " & _
                                "CASE WHEN PaymentAPI = 'iSYS-POS' THEN 'DEALER' ELSE 'OG Money KW' END AS [Source] " & _
                                "from [payit].[dbo].[PayitiSYS] where convert(datetime,ProcessTranDate, 103) between convert(datetime,'" & fromdate & "', 103) and " & _
                                "convert(datetime,'" & todate & "', 103) and (ProcessTranDescription like 'SUCCESS%' OR ProcessTranDescription like 'OK%')  and [Service] like '" & ddlService.SelectedItem.Value & "' " & _
                                "group by [Service], Amount, PAymentAPI order by Denomination"
            Else
                sqlLocalTrans = "select p.[Service],cast(Substring(th.udf4,Charindex('|', th.udf4)+1,7) as float) as Denomination,p.PaymentAPI, sum(cast(Substring(th.udf4,Charindex('|', th.udf4)+1,7) as float)) as 'TotalAmount',Count(*) TotalTransactions " & _
                    "from  PayitiSYS p join ThirdParty_knet_trans th on th.trackid  = p.IsysID   where convert(datetime,p.ProcessTranDate, 103) " & _
                    "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and id>=26 " & _
                    "and (p.ProcessTranDescription like '%SUCCESS%' OR p.ProcessTranDescription like 'OK%')  and th.udf4 is not null and p.[Service] like '" & ddlService.SelectedItem.Value & "'  " & _
                    "group by p.[Service],Substring(th.udf4,Charindex('|', th.udf4)+1,7),p.PAymentAPI order by Denomination"
            End If
                grandTotSql = "Select udf2 Service, Substring(udf4, 1,Charindex('|', udf4)-1) as 'Multiple of PINS', COUNT(Substring(udf4, 1,Charindex('|', udf4)-1)) as 'Total PINS Sold'" & _
            "from ThirdParty_knet_trans where (udf4 like '%|%' and udf4 is not null) and (knetprocess like 'CAPTURED') and udf1 like 'success%' and [udf2] like '" & ddlService.SelectedItem.Value & "' group by udf2, Substring(udf4, 1,Charindex('|', udf4)-1) order by Substring(udf4, 1,Charindex('|', udf4)-1)"
            End If
        Session("sqlLocalTrans") = sqlLocalTrans
        sqldatabind(sqlLocalTrans)

        'grandTotSql = "Select Substring(udf4, 1,Charindex('|', udf4)-1) as 'Multiple of PINS', COUNT(Substring(udf4, 1,Charindex('|', udf4)-1)) as 'Total PINS Sold'" & _
        ' "from ThirdParty_knet_trans where (udf4 like '%|%' and udf4 is not null) and (knetprocess like 'CAPTURED') and udf1 like 'success%' group by Substring(udf4, 1,Charindex('|', udf4)-1) order by Substring(udf4, 1,Charindex('|', udf4)-1)"
        'Using cmd As New SqlCommand(grandTotSql)
        '    Using sda As New SqlDataAdapter()
        '        cmd.Connection = cn
        '        sda.SelectCommand = cmd
        '        Using dt As New DataTable()
        '            sda.Fill(dt)
        '            GridView2.DataSource = dt
        '            GridView2.DataBind()
        '        End Using
        '    End Using
        'End Using

    End Sub

    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
                                              "alert('" & s & "')" & _
                                              "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Public Sub sqldatabind(ByVal s As String)
        da = New SqlDataAdapter(s, cn)
        ds = New DataSet()
        da.Fill(ds, "zain")
        GridView1.DataSource = ds.Tables("zain")
        GridView1.DataBind()
        Dim totalRows As Integer
        Dim grandTotal, TotalTrans As Double
        totalRows = GridView1.Rows.Count
        grandTotal = 0
        Dim grand As Integer = 3
        Dim total As Integer = 4
        If (username.Equals("zain")) Then
            grand = 2
            total = 3
        End If
        For i = 0 To totalRows - 1
            grandTotal = grandTotal + Val(GridView1.Rows(i).Cells(grand).Text)
            TotalTrans = TotalTrans + Val(GridView1.Rows(i).Cells(total).Text)
        Next
        Label17.Text = "Total Amount: " & grandTotal
        Label18.Text = "Total No. Of transactions:  " & TotalTrans

    End Sub
    Public Sub counter1()
        Button2.Attributes.Add("onclick", "javascript:abct()")
        GridView1.Attributes.Add("databinding", "javascript:abct()")
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        Dim s As String
        s = Session("sqlLocalTrans")
        sqldatabind(s)
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("Service-wiseSummary.xls", Me.GridView1)
    End Sub
End Class
