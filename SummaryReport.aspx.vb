Imports System.Data
Imports System.Data.SqlClient
Partial Class SummaryReport
    Inherits System.Web.UI.Page

    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "cs" Or Session("user") = "zain") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=SummaryReport.aspx")
        End If

        cn = New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")
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
        'Summary of only Smart phone transactions (excluding POS tranmsactions)


        'If ddlServiceProvider.SelectedItem.Text = "All" Then
        '    'sqlLocalTrans = "select [Service] ServiceCode,sum(Amount) Amount,count(*) TransCount " & _
        '    '         "from PayitiSYS " & _
        '    '         "where(Convert(DateTime, ProcessTranDate, 103)) " & _
        '    '         "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and id>=26 " & _
        '    '         "and ProcessTranDescription like '%SUCCESS%' and isysid not in (select isysid from DPayitTPTransactions where (Convert(DateTime, TranDate, 103)) " & _
        '    '         " between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and isysid is not null ) " & _
        '    '         "group by [Service]".

        '    '" WHEN TUser = 'Android' or TUser = 'Android2' or TUser = 'Android4'  THEN 'Android'" & _

        '    '    If CheckBox1.Checked Then
        '    '        sqlLocalTrans = "SELECT udf2 as ServiceCode, sum(convert(FLOAT,amt)) as Amount, count(*) AS TransCount," & _
        '    '                        " CASE" & _
        '    '                        " WHEN TUser = 'XSmart' THEN 'iPhone'" & _
        '    '                        " WHEN TUser = 'iphonev3' THEN 'iPhone'" & _
        '    '                        " WHEN TUser = 'Blackberry' THEN 'Blackberry'" & _
        '    '                        " WHEN TUser = 'Android' or TUser = 'Android2' or TUser = 'Android4' or TUser = 'Android5' or TUser = 'Android6' THEN 'Android'" & _
        '    '                        " End" & _
        '    '                        " AS Handset" & _
        '    '                        " FROM ThirdParty_knet_trans" & _
        '    '                        " WHERE UDF1 LIKE '%SUCCESS%' " & _
        '    '                        " AND (Convert(DateTime, TDATETIME, 103)) " & _
        '    '                        " between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) " & _
        '    '                        " GROUP BY TUser, udf2" & _
        '    '                        " ORDER BY udf2"
        '    '    Else
        '    '        'sqlLocalTrans = "select [Service] ServiceCode,sum(Amount) Amount,count(*) TransCount " & _
        '    '        '         "from PayitiSYS " & _
        '    '        '         "where(Convert(DateTime, ProcessTranDate, 103)) " & _
        '    '        '         "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and id>=26 " & _
        '    '        '         "and (ProcessTranDescription like '%SUCCESS%' and PaymentAPI NOT LIKE '%POS') or (ProcessTranDescription like 'OK%' and (Service like 'CashU' or Service like 'VIVA-O' or Service like 'iTunes-O' or Service like 'OneCard-O' or Service like 'ZAIN-O' or Service like 'WATANIYA-O')) " & _
        '    '        '         "group by [Service]"

        '    '        sqlLocalTrans = "select [Service] ServiceCode,sum(Amount) Amount,count(*) TransCount " & _
        '    ' "from PayitiSYS " & _
        '    ' "where(Convert(DateTime, ProcessTranDate, 103)) " & _
        '    ' "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and id>=26 " & _
        '    ' "and (ProcessTranDescription like 'OK%' or ProcessTranDescription like '%SUCCESS%') and PaymentAPI NOT LIKE '%POS' " & _
        '    ' "group by [Service]"
        '    '    End If
        '    'Else
        '    '    'sqlLocalTrans = "select [Service] ServiceCode,sum(Amount) Amount,count(*) TransCount " & _
        '    '    '         "from PayitiSYS " & _
        '    '    '         "where(Convert(DateTime, ProcessTranDate, 103)) " & _
        '    '    '         "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and id>=26 " & _
        '    '    '         "and ProcessTranDescription like '%SUCCESS%'  and PaymentAPI like '" & ddlServiceProvider.SelectedItem.Text & "'  " & _
        '    '    '         " and isysid not in (select isysid from DPayitTPTransactions where (Convert(DateTime, TranDate, 103)) " & _
        '    '    '         " between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and isysid is not null  ) group by [Service]"

        '    '    If CheckBox1.Checked Then
        '    '        sqlLocalTrans = "SELECT udf2 as ServiceCode, sum(convert(FLOAT,amt)) as Amount, count(*) AS TransCount," & _
        '    '        " CASE" & _
        '    '        " WHEN TUser = 'XSmart' THEN 'iPhone'" & _
        '    '        " WHEN TUser = 'iphonev3' THEN 'iPhone'" & _
        '    '        " WHEN TUser = 'Android' or TUser = 'Android2' or TUser = 'Android4' or TUser = 'Android5' or TUser = 'Android6' THEN 'Android'" & _
        '    '        " WHEN TUser = 'Blackberry' THEN 'Blackberry'" & _
        '    '        " End" & _
        '    '        " AS Handset" & _
        '    '        " FROM ThirdParty_knet_trans" & _
        '    '        " WHERE UDF1 LIKE '%SUCCESS%' " & _
        '    '        " AND (Convert(DateTime, TDATETIME, 103)) " & _
        '    '        " between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) " & _
        '    '        " GROUP BY TUser, udf2" & _
        '    '        " ORDER BY udf2"
        '    '    Else

        '    '        'sqlLocalTrans = "select [Service] ServiceCode,sum(Amount) Amount,count(*) TransCount " & _
        '    '        '         "from PayitiSYS " & _
        '    '        '         "where(Convert(DateTime, ProcessTranDate, 103)) " & _
        '    '        '         "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and id>=26 " & _
        '    '        '        " group by [Service]"

        '    '        sqlLocalTrans = "select [Service] ServiceCode,sum(Amount) Amount,count(*) TransCount " & _
        '    '                 "from PayitiSYS " & _
        '    '                 "where(Convert(DateTime, ProcessTranDate, 103)) " & _
        '    '                 "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and id>=26 " & _
        '    '                 "and (ProcessTranDescription like 'OK%' or ProcessTranDescription like '%SUCCESS%') and PaymentAPI NOT LIKE '%POS'" & _
        '    '                " group by [Service]"
        '    '    End If
        '    'End If





        'If ddlServiceProvider.SelectedItem.Text = "All" Then
        '    sqlLocalTrans = "select [Service] ServiceCode,sum(Amount) Amount,count(*) TransCount " & _
        '             "from PayitiSYS " & _
        '             "where(Convert(DateTime, TranDate, 103)) " & _
        '             "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and id>=26 " & _
        '             "and ProcessTranDescription like '%SUCCESS%' and isysid not in (select isysid from DPayitTPTransactions  ) " & _
        '             "group by [Service]"

        'Else
        '    sqlLocalTrans = "select [Service] ServiceCode,sum(Amount) Amount,count(*) TransCount " & _
        '             "from PayitiSYS " & _
        '             "where(Convert(DateTime, TranDate, 103)) " & _
        '             "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and id>=26 " & _
        '             "and ProcessTranDescription like '%SUCCESS%'  and PaymentAPI like '" & ddlServiceProvider.SelectedItem.Text & "'  " & _
        '             " and isysid not in (select isysid from DPayitTPTransactions  ) group by [Service]"

        'End If


        'For all pay-it transactions-Both iphone and pos
        'If ddlServiceProvider.SelectedItem.Text = "All" Then
        '    sqlLocalTrans = "select [Service] ServiceCode,sum(Amount) Amount,count(*) TransCount " & _
        '             "from PayitiSYS " & _
        '             "where(Convert(DateTime, TranDate, 103)) " & _
        '             "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and id>=26 " & _
        '             "and ProcessTranDescription like '%SUCCESS%'  " & _
        '             "group by [Service]"

        'Else
        '    sqlLocalTrans = "select [Service] ServiceCode,sum(Amount) Amount,count(*) TransCount " & _
        '             "from PayitiSYS " & _
        '             "where(Convert(DateTime, TranDate, 103)) " & _
        '             "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and id>=26 " & _
        '             "and ProcessTranDescription like '%SUCCESS%'  and PaymentAPI like '" & ddlServiceProvider.SelectedItem.Text & "'  " & _
        '             "group by [Service]"

        'End If


        'Disabled below query due to performance issues

        'sqlLocalTrans = "select ServiceCode,sum(convert(float,Amount)) Amount,count(*) TransCount  from " & _
        '                "( select tk.TrackID,isnull(et.serviceCode,'UnProcessed') ServiceCode,et.Account Mob, tk.amt Amount," & _
        '            " tk.knetProcess,tk.udf1 KnetResult,et.trans_result Result,tk.tdatetime,pay.PaymentAPI" & _
        '            " from  thirdParty_knet_trans tk left outer join  enetservices_trans et " & _
        '            " on tk.trackid=et.trackid" & _
        '            " join PayitiSYS pay on tk.TrackId=pay.isysID " & _
        '            " where convert(datetime,tk.tdatetime, 103) " & _
        '            " between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103)" & _
        '            " and pay.id>=26 " & _
        '            ") as temp group by ServiceCode order by serviceCode desc"

        If CheckBox1.Checked Then
            sqlLocalTrans = "SELECT udf2 as ServiceCode, sum(convert(FLOAT,amt)) as Amount, count(*) AS TransCount," & _
            " CASE" & _
            " WHEN TUser = 'XSmart' THEN 'iPhone'" & _
            " WHEN TUser = 'iphonev3' or TUser = 'iphonev4'  or TUser = 'iphonev5' THEN 'iPhone'" & _
            " WHEN TUser = 'Android' or TUser = 'Android2' or TUser = 'Android4' or TUser = 'Android5' or TUser = 'Android6' or TUser = 'Android7' or TUser = 'Android8' or TUser = 'Android9' or TUser = 'Android10' THEN 'Android'" & _
            " WHEN TUser = 'Blackberry' THEN 'Blackberry'" & _
            " WHEN TUser = 'Windowsph1' or TUser = 'Windowsdt1' or TUser = 'WindowsTest' THEN 'Windows'" & _
            " End" & _
            " AS Handset" & _
            " FROM " & DropDownList3.SelectedItem.Text & _
            " WHERE UDF1 LIKE '%SUCCESS%' " & _
            " AND (Convert(DateTime, TDATETIME, 103)) " & _
            " between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) " & _
            " GROUP BY TUser, udf2" & _
            " ORDER BY udf2"
        Else
            sqlLocalTrans = "select [Service] ServiceCode,sum(Amount) Amount,count(*) TransCount " & _
         "from PayitiSYS " & _
         "where(Convert(DateTime, TranDate, 103)) " & _
         "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and id>=26 " & _
         " and PaymentAPI NOT LIKE '%POS' and ((ProcessTranDescription like '%SUCCESS%' " & _
         " or (ProcessTranDescription like '%ePayments: Your transaction has taken longer than expected%' and [service] like 'VIVA%')) " & _
         " or (ProcessTranDescription like 'OK%' and Service like '%-O')" & _
          " or ((ProcessTranDescription  like 'SUCCESS%' or ProcessTranDescription like 'P%') and Service like 'wataniya-x'))" & _
         " group by [Service]"

            'sqlLocalTrans = "select [Service] ServiceCode,sum(Amount) Amount,count(*) TransCount " & _
            '" from PayitiSYS " & _
            '" where(Convert(DateTime, ProcessTranDate, 103)) " & _
            '" between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and id>=26 " & _
            '" and PaymentAPI NOT LIKE '%POS' and ((ProcessTranDescription like '%SUCCESS%' " & _
            '" or (ProcessTranDescription like '%ePayments: Your transaction has taken longer than expected%' and [service] like 'VIVA%')) " & _
            '" or (ProcessTranDescription like 'OK%' and Service like '%-O')) " & _
            '" union all " & _
            '" select [Service] ServiceCode,sum(Amount) Amount,count(*) TransCount " & _
            '" from PayitiSYS20140403 " & _
            '" where(Convert(DateTime, ProcessTranDate, 103)) " & _
            '" between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and id>=26 " & _
            '" and PaymentAPI NOT LIKE '%POS' and ((ProcessTranDescription like '%SUCCESS%' " & _
            '" or (ProcessTranDescription like '%ePayments: Your transaction has taken longer than expected%' and [service] like 'VIVA%')) " & _
            '" or (ProcessTranDescription like 'OK%' and Service like '%-O'))" & _
            '" group by [Service]"

        End If

        Session("sqlLocalTrans") = sqlLocalTrans
        sqldatabind(sqlLocalTrans)

    End Sub
   
    Public Sub sqldatabind(ByVal s As String)
        Dim cmd1 As New SqlCommand(s, cn)
        cmd1.CommandTimeout = 3600

        da = New SqlDataAdapter(cmd1)
        ds = New DataSet()
        da.Fill(ds, "det")
        GridView1.DataSource = ds.Tables("det")
        GridView1.DataBind()
        Dim totalRows As Integer
        Dim grandTotal, Totaltrans As Double
        totalRows = GridView1.Rows.Count
        grandTotal = 0
        Totaltrans = 0
        For i = 0 To totalRows - 1
            grandTotal = grandTotal + Val(GridView1.Rows(i).Cells(1).Text)
            Totaltrans = Totaltrans + Val(GridView1.Rows(i).Cells(2).Text)
        Next
        Label17.Text = "Total Amount: " & grandTotal
        Label18.Text = "Total Transactions :" & Totaltrans

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
        ExportToExcel.Export("Summary.xls", Me.GridView1)
    End Sub

    Protected Sub ddlServiceProvider_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlServiceProvider.SelectedIndexChanged

    End Sub

    Protected Sub CheckBox1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            Label19.Visible = True
            DropDownList3.Visible = True
        Else
            Label19.Visible = False
            DropDownList3.Visible = False
        End If
    End Sub
End Class
