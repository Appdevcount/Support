Imports System.Data
Imports System.Data.SqlClient

Partial Class WRecharge
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        counter1()
        Dim sqlLocalTrans As String
        Dim sqlLocalTrans2 As String

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

        'If ddlService.SelectedItem.Text = "All" Then
        '    'sqlLocalTrans = "select [Service],Amount Denomination,PAymentAPI,sum(Amount) TotalAmount,Count(*) TotalTransactions " & _
        '    '         "from  PayitiSYS  where convert(datetime,ProcessTranDate, 103) " & _
        '    '         "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and id>=26 " & _
        '    '         "and ProcessTranDescription like '%SUCCESS%' and [Service] like 'Zain-X'  " & _
        '    '         "group by [Service],Amount,PAymentAPI "

        '    sqlLocalTrans = "select [Service],Amount Denomination,PAymentAPI,sum(Amount) TotalAmount,Count(*) TotalTransactions " & _
        '             "from  PayitiSYS  where convert(datetime,ProcessTranDate, 103) " & _
        '             "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and id>=26 " & _
        '             "and (ProcessTranDescription like '%SUCCESS%' OR ProcessTranDescription like 'OK%') and ([Service] like 'iTunes-O' or [Service] like 'CashU-O' or [Service] like 'OneCard-O' or [Service] like 'VIVA-O' or [Service] like 'Zain-O' or [Service] like 'Wataniya-O' or [Service] like 'Zain-X')" & _
        '             "group by [Service],Amount,PAymentAPI "

        'Else

        '    sqlLocalTrans = "select [Service],Amount Denomination,PAymentAPI,sum(Amount) TotalAmount,Count(*) TotalTransactions " & _
        '           "from  PayitiSYS  where convert(datetime,ProcessTranDate, 103) " & _
        '           "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and id>=26 " & _
        '           "and (ProcessTranDescription like '%SUCCESS%' OR ProcessTranDescription like 'OK%') and [Service] like '" & ddlService.SelectedItem.Text & "'  " & _
        '           "group by [Service],Amount,PAymentAPI "


        'End If

        Dim mobileFilter As String = String.Empty
        Dim mobileFilter2 As String = String.Empty
        If MobileNoFilterTextBox.Text.Length > 1 Then mobileFilter = " and  momsghstsmsbody like '%" & MobileNoFilterTextBox.Text & "%' "
        If MobileNoFilterTextBox.Text.Length > 1 Then mobileFilter2 = " and  mtmsghstsmstext like '%" & MobileNoFilterTextBox.Text & "%' "

        sqlLocalTrans = "select momsghstid ID,momsghstmsisdn Mobile,momsghstshortcode ShortCode,momsghstlangId Lang,momsghstsmsbody Message,momsghstdate TranDate" & _
                  " from  momsghistory  where convert(datetime,momsghstdate, 103) " & _
                  "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103)  " & mobileFilter & "  order by momsghstid desc"

        sqlLocalTrans2 = "select  mtmsghstid ID,mtmsghstcorrelationId MO_ID,mtmsghstmessageid MESSAGE_ID,mtmsghststatus STATUS,mtmsghsterror Error_Desc,mtmsghstservicetype ServiceType,mtmsghstsmstext Message,mtmsghstinsertdate TranDate,mtmsghstdate ProcessTranDate, mtmsghstoperatorid OperatorID" & _
          " from  mtmsghistory  where convert(datetime,mtmsghstdate, 103) " & _
          "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) " & mobileFilter2 & " order by mtmsghstid desc"

        Session("sqlLocalTrans") = sqlLocalTrans
        Session("sqlLocalTrans2") = sqlLocalTrans2

        sqldatabind(sqlLocalTrans)
        sqldatabind2(sqlLocalTrans2)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("user") = "" Then
            Response.Redirect("~/login.aspx?ReturnURL=WRecharge.aspx")
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
        Dim count As Integer = 0
        For i = 0 To totalRows - 1
            grandTotal = grandTotal + Val(GridView1.Rows(i).Cells(3).Text)
            TotalTrans = TotalTrans + Val(GridView1.Rows(i).Cells(4).Text)
            count = count + 1
        Next
        Label17.Text = "Total Amount: " & grandTotal
        'Label18.Text = "Total No. Of transactions:  " & TotalTrans
        Label18.Text = "Total No. Of transactions:  " & count

    End Sub

    Public Sub sqldatabind2(ByVal s As String)
        da = New SqlDataAdapter(s, cn)
        ds = New DataSet()
        da.Fill(ds, "zain2")
        GridView2.DataSource = ds.Tables("zain2")
        GridView2.DataBind()
        Dim totalRows As Integer
        Dim grandTotal, TotalTrans As Double
        totalRows = GridView2.Rows.Count
        grandTotal = 0
        For i = 0 To totalRows - 1
            grandTotal = grandTotal + Val(GridView2.Rows(i).Cells(3).Text)
            TotalTrans = TotalTrans + Val(GridView2.Rows(i).Cells(4).Text)
        Next
        'Label1.Text = "Total Amount: " & grandTotal
        Label2.Text = "Total No. Of transactions:  " & grandTotal

    End Sub

    Public Sub counter1()
        Button2.Attributes.Add("onclick", "javascript:abct()")
        GridView1.Attributes.Add("databinding", "javascript:abct()")

        GridView2.Attributes.Add("databinding", "javascript:abct()")
    End Sub


    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        Dim s As String
        s = Session("sqlLocalTrans")
        sqldatabind(s)

    End Sub

    Protected Sub GridView2_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView2.PageIndexChanging
        GridView2.PageIndex = e.NewPageIndex
        Dim s As String
        s = Session("sqlLocalTrans2")
        sqldatabind2(s)
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("WRecharge.xls", Me.GridView1)
    End Sub

End Class
