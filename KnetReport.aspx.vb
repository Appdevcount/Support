Imports System.Data
Imports System.Data.SqlClient
Partial Class KnetReport
    Inherits System.Web.UI.Page


    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        counter1()
        Dim sqlPayment, sqlPaymentNew, sqlPay As String
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
        If CheckBox1.Checked Then
         
            sqlPayment = "select ptype as PaymentType,udf2 ServiceCode," & _
            " sum(convert(money,amt)) TotalAmount,count(*) Transactions from " & DropDownList3.SelectedItem.Text & _
            " where (Convert(DateTime, lastupdateon, 103)) " & _
            " between convert(datetime,'" & fromdate & "', 103) " & _
            " and convert(datetime,'" & todate & "', 103) and udf2 is not null and udf2 not like ''"

            sqlPaymentNew = "select ptype as PaymentType,udf2 ServiceCode," & _
            " sum(convert(money,amt)) TotalAmount,count(*) Transactions from thirdParty_knet_trans " & _
            " where (Convert(DateTime, lastupdateon, 103)) " & _
            " between convert(datetime,'" & fromdate & "', 103) " & _
            " and convert(datetime,'" & todate & "', 103) and udf2 is not null and udf2 not like ''"

        Else
         
            sqlPayment = "select ptype as  PaymentType,udf2 ServiceCode," & _
        " sum(convert(money,amt)) as  TotalAmount,count(*) as Transactions from " & DropDownList3.SelectedItem.Text & _
        " where (Convert(DateTime, tdatetime, 103)) " & _
        " between convert(datetime,'" & fromdate & "', 103) " & _
        " and convert(datetime,'" & todate & "', 103) and udf2 is not null and udf2 not like ''"

            sqlPaymentNew = "select ptype as PaymentType,udf2 ServiceCode," & _
            " sum(convert(money,amt)) as TotalAmount,count(*) as Transactions from thirdParty_knet_trans " & _
            " where (Convert(DateTime, tdatetime, 103)) " & _
            " between convert(datetime,'" & fromdate & "', 103) " & _
            " and convert(datetime,'" & todate & "', 103) and udf2 is not null and udf2 not like ''"
        End If

        

        If ddlPayment.SelectedItem.Text = "Knet" Then'sqlPayment += " and knetprocess like 'Captured%'"
            'sqlPaymentNew += " and knetprocess like 'Captured%'"

            'HARI 
            If ddlKnetTunnel.SelectedItem.Text.ToLower.Equals("all") Then
                sqlPayment += " and ptype like 'Knet' and knetprocess like 'Captured%'"
                sqlPaymentNew += " and ptype like 'Knet' and knetprocess like 'Captured%'"
            Else
                Dim tunnel As String = String.Empty
                
                If (ddlKnetTunnel.SelectedItem.Text.ToUpper().Equals("KFH")) Then
                    tunnel = "isys"
                ElseIf (ddlKnetTunnel.SelectedItem.Text.ToUpper().Equals("BUBYAN")) Then
                    tunnel = "payit"
                ElseIf (ddlKnetTunnel.SelectedItem.Text.ToUpper().Equals("BURGAN")) Then
                    tunnel = "ogmoney"
                End If
                sqlPayment += " and ptype like 'Knet' and knetprocess like 'Captured%'" & " and RTRIM(LTRIM(LOWER(UDF5))) LIKE RTRIM(LTRIM(LOWER('" & tunnel & "')))"
                sqlPaymentNew += " and ptype like 'Knet' and knetprocess like 'Captured%'" & " and RTRIM(LTRIM(LOWER(UDF5))) LIKE RTRIM(LTRIM(LOWER('" & tunnel & "')))"
            End If



        ElseIf ddlPayment.SelectedItem.Text = "Creditcard" Then
            sqlPayment += " and ptype like 'Creditcard' and knetprocess like 'Approved%'"
            sqlPaymentNew += " and ptype like 'Creditcard' and knetprocess like 'Approved%'"
        ElseIf ddlPayment.SelectedItem.Text = "KWKNETDC" Then
            sqlPayment += " and ptype like 'KWKNETDC' and knetprocess like 'Captured%'"
            sqlPaymentNew += " and ptype like 'KWKNETDC' and knetprocess like 'Captured%'"
        ElseIf ddlPayment.SelectedItem.Text = "WALLET" Then
            sqlPayment += " and ptype like 'Warba-W' and knetprocess like 'Captured%'"
            sqlPaymentNew += " and ptype like 'Warba-W' and knetprocess like 'Captured%'"
        ElseIf ddlPayment.SelectedItem.Text = "CashU" Then

            If CheckBox1.Checked Then
                sqlPayment = "select ptype PaymentType,udf2 ServiceCode," & _
            " sum(convert(money,amt)) TotalAmount,count(*) Transactions from " & DropDownList3.SelectedItem.Text & _
            " where (Convert(DateTime, lastupdateon, 103)) " & _
            " between convert(datetime,'" & fromdate & "', 103) " & _
            " and convert(datetime,'" & todate & "', 103) and udf2 is not null and udf2 not like ''"

                sqlPaymentNew = "select ptype PaymentType,udf2 ServiceCode," & _
                " sum(convert(money,amt)) TotalAmount,count(*) Transactions from thirdParty_knet_trans " & _
                " where (Convert(DateTime, lastupdateon, 103)) " & _
                " between convert(datetime,'" & fromdate & "', 103) " & _
                " and convert(datetime,'" & todate & "', 103) and udf2 is not null and udf2 not like ''"
            Else
                sqlPayment = "select ptype PaymentType,udf2 ServiceCode," & _
            " sum(convert(money,amt)) TotalAmount,count(*) Transactions from " & DropDownList3.SelectedItem.Text & _
            " where (Convert(DateTime, tdatetime, 103)) " & _
            " between convert(datetime,'" & fromdate & "', 103) " & _
            " and convert(datetime,'" & todate & "', 103) and udf2 is not null and udf2 not like ''"

                sqlPaymentNew = "select ptype PaymentType,udf2 ServiceCode," & _
                " sum(convert(money,amt)) TotalAmount,count(*) Transactions from thirdParty_knet_trans " & _
                " where (Convert(DateTime, tdatetime, 103)) " & _
                " between convert(datetime,'" & fromdate & "', 103) " & _
                " and convert(datetime,'" & todate & "', 103) and udf2 is not null and udf2 not like ''"
            End If


            sqlPayment += " and ptype like 'CashU' and knetprocess like 'CAPTURED%'"
            sqlPaymentNew += " and ptype like 'CashU' and knetprocess like 'CAPTURED%'"
        ElseIf ddlPayment.SelectedItem.Text = "AMEX" Then
            sqlPayment += " and ptype like 'AMEX' and knetprocess like 'Approved%'"
            sqlPaymentNew += " and ptype like 'AMEX' and knetprocess like 'Approved%'"
        Else 'All

            sqlPayment += " and (knetprocess like 'Approved%' or knetprocess like 'Captured%') "
            sqlPaymentNew += " and (knetprocess like 'Approved%' or knetprocess like 'Captured%') "


        End If


        If ddlServiceCode.SelectedItem.Text = "Intl" Then
            sqlPayment += " and udf2 like 'INTl%'"
            sqlPaymentNew += " and udf2 like 'INTl%'"

        ElseIf ddlServiceCode.SelectedItem.Text = "Local" Then
            sqlPayment += " and udf2 not like 'INTl%'"
            sqlPaymentNew += " and udf2 not like 'INTl%'"

        End If


        If ddlPayment.SelectedItem.Text = "CashU" Then
            sqlPayment += " group by ptype,udf2 "
            sqlPaymentNew += " group by ptype,udf2 "
        Else
           
            sqlPayment += " group by ptype,udf2 "
            sqlPaymentNew += " group by ptype,udf2 "
        End If

        'sqlPay = "select PaymentType,ServiceCode,sum(TotalAmount),sum(Transactions) from( " & sqlPayment & " union " & sqlPaymentNew & " )as temp  group by PaymentType,ServiceCode  order by PaymentType"
        sqlPay = "select PaymentType,ServiceCode,sum(TotalAmount) as TotalAmount,sum(Transactions) as Transactions from( " & sqlPayment & " )as temp  group by PaymentType,ServiceCode  order by PaymentType"
         
        Session("sqlPay") = Nothing
        Session("sqlPay") = sqlPay
        sqldatabind(sqlPay, True)

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "cs" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "qa") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=KnetReport.aspx")
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
    Public Sub sqldatabind(ByVal s As String, ByVal isSummary As Boolean)


        Try
            GridView1.DataSource = Nothing
            GridView1.DataBind()

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

            If isSummary = True Then
                For i = 0 To totalRows - 1
                    grandTotal = grandTotal + Val(GridView1.Rows(i).Cells(2).Text)
                    Totaltrans = Totaltrans + Val(GridView1.Rows(i).Cells(3).Text)
                Next
            Else
                For i = 0 To totalRows - 1

                    grandTotal = grandTotal + Val(GridView1.Rows(i).Cells(2).Text)
                    'Totaltrans = Totaltrans + Val(GridView1.Rows(i).Cells(3).Text)
                    Totaltrans = i + 1.ToString

                Next

            End If

            Label17.Text = "Total Amount: " & grandTotal
            Label18.Text = "Total Transactions :" & Totaltrans

        Catch ex As Exception
            alert("Error: " & ex.Message)
            Return
        End Try
    End Sub

    Public Sub alert(ByVal s As String)
        Dim popupscript As String

        popupscript = "<script language='javascript'>" & _
                                              "alert('" & s & "')" & _
                                              "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)


    End Sub

    Public Sub counter1()
        Button2.Attributes.Add("onclick", "javascript:abct()")
        Button3.Attributes.Add("onclick", "javascript:abct()")
        GridView1.Attributes.Add("databinding", "javascript:abct()")
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        Dim s As String
        s = Session("sqlPay")

        If s.EndsWith("order by myid desc") Then
            sqldatabind(s, False)
        Else
            sqldatabind(s, True)
        End If



    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("stats.xls", Me.GridView1)
    End Sub

  
    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click

        counter1()

        If CheckBox2.Checked Then
            GridView1.AllowPaging = True
            GridView1.PageSize = 50

        Else
            GridView1.AllowPaging = False
        End If


        Dim sqlPayment, sqlPaymentNew, sqlPay As String
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
        If CheckBox1.Checked Then
            sqlPayment = "SELECT myid,TrackID,Amt,Refid,replace(replace(KnetProcess,'Approved','CreditCard'),'Captured','KNET') PaymentType,udf2 as 'Service',udf3 as 'MobileNo',tdatetime,lastupdateon " & _
                            " FROM " & DropDownList3.SelectedItem.Text & " where (Convert(DateTime, lastupdateon, 103)) " & _
                            " between convert(datetime,'" & fromdate & "', 103) " & _
                            " and convert(datetime,'" & todate & "', 103)  and udf2 is not null and udf2 not like ''"

            sqlPaymentNew = "SELECT myid,TrackID,Amt,Refid,replace(replace(KnetProcess,'Approved','CreditCard'),'Captured','KNET') PaymentType,udf2 as 'Service',udf3 as 'MobileNo',tdatetime,lastupdateon " & _
                            " FROM thirdParty_knet_trans where (Convert(DateTime, lastupdateon, 103)) " & _
                            " between convert(datetime,'" & fromdate & "', 103) " & _
                            " and convert(datetime,'" & todate & "', 103)  and udf2 is not null and udf2 not like ''"


        Else
            sqlPayment = "SELECT myid,TrackID,Amt,Refid,ptype PaymentType,udf2 as 'Service',udf3 as 'MobileNo',tdatetime,lastupdateon " & _
                            " FROM " & DropDownList3.SelectedItem.Text & " where (Convert(DateTime, tdatetime, 103)) " & _
                            " between convert(datetime,'" & fromdate & "', 103) " & _
                            " and convert(datetime,'" & todate & "', 103)  and udf2 is not null and udf2 not like ''"

            sqlPaymentNew = "SELECT myid,TrackID,Amt,Refid,ptype PaymentType,udf2 as 'Service',udf3 as 'MobileNo',tdatetime,lastupdateon " & _
                            " FROM thirdParty_knet_trans where (Convert(DateTime, tdatetime, 103)) " & _
                            " between convert(datetime,'" & fromdate & "', 103) " & _
                            " and convert(datetime,'" & todate & "', 103)  and udf2 is not null and udf2 not like ''"
        End If



        If ddlPayment.SelectedItem.Text = "Knet" Then

            If ddlKnetTunnel.SelectedItem.Text.ToLower.Equals("all") Then
                sqlPayment += " and knetprocess like 'Captured%' and ptype like 'knet'"
                sqlPaymentNew += " and knetprocess like 'Captured%' and ptype like 'knet'"
            Else

                Dim tunnel As String = String.Empty

                If (ddlKnetTunnel.SelectedItem.Text.ToUpper().Equals("KFH")) Then
                    tunnel = "isys"
                ElseIf (ddlKnetTunnel.SelectedItem.Text.ToUpper().Equals("BUBYAN")) Then
                    tunnel = "payit"
                ElseIf (ddlKnetTunnel.SelectedItem.Text.ToUpper().Equals("BURGAN")) Then
                    tunnel = "ogmoney"
                End If
                sqlPayment += " and knetprocess like 'Captured%'" & " and RTRIM(LTRIM(LOWER(UDF5))) LIKE RTRIM(LTRIM(LOWER('" & tunnel & "')))"
                sqlPaymentNew += " and knetprocess like 'Captured%'" & " and RTRIM(LTRIM(LOWER(UDF5))) LIKE RTRIM(LTRIM(LOWER('" & tunnel & "')))"
            End If

        ElseIf ddlPayment.SelectedItem.Text = "Creditcard" Then
            sqlPayment += " and knetprocess like 'Approved%' and ptype like 'CreditCard'"
            sqlPaymentNew += " and knetprocess like 'Approved%' and ptype like 'CreditCard'"

        ElseIf ddlPayment.SelectedItem.Text = "KWKNETDC" Then
            sqlPayment += " and knetprocess like 'Captured%' and ptype like 'KWKNETDC'"
            sqlPaymentNew += " and knetprocess like 'Captured%' and ptype like 'KWKNETDC'"
        ElseIf ddlPayment.SelectedItem.Text = "WALLET" Then
            sqlPayment += " and ptype like 'Warba-W' and knetprocess like 'Captured%'"
            sqlPaymentNew += " and ptype like 'Warba-W' and knetprocess like 'Captured%'"
        ElseIf ddlPayment.SelectedItem.Text = "CashU" Then
            If CheckBox1.Checked Then
                sqlPayment = "SELECT myid,TrackID,Amt,Refid,ptype PaymentType,udf2 as 'Service',udf3 as 'MobileNo',tdatetime,lastupdateon " & _
                                " FROM " & DropDownList3.SelectedItem.Text & " where (Convert(DateTime, lastupdateon, 103)) " & _
                                " between convert(datetime,'" & fromdate & "', 103) " & _
                                " and convert(datetime,'" & todate & "', 103)  and udf2 is not null and udf2 not like ''"

                sqlPaymentNew = "SELECT myid,TrackID,Amt,Refid,ptype PaymentType,udf2 as 'Service',udf3 as 'MobileNo',tdatetime,lastupdateon " & _
                                " FROM thirdParty_knet_trans where (Convert(DateTime, lastupdateon, 103)) " & _
                                " between convert(datetime,'" & fromdate & "', 103) " & _
                                " and convert(datetime,'" & todate & "', 103)  and udf2 is not null and udf2 not like ''"


            Else
                sqlPayment = "SELECT myid,TrackID,Amt,Refid,ptype PaymentType,udf2 as 'Service',udf3 as 'MobileNo',tdatetime,lastupdateon " & _
                                " FROM " & DropDownList3.SelectedItem.Text & " where (Convert(DateTime, tdatetime, 103)) " & _
                                " between convert(datetime,'" & fromdate & "', 103) " & _
                                " and convert(datetime,'" & todate & "', 103)  and udf2 is not null and udf2 not like ''"

                sqlPaymentNew = "SELECT myid,TrackID,Amt,Refid,ptype PaymentType,udf2 as 'Service',udf3 as 'MobileNo',tdatetime,lastupdateon " & _
                                " FROM thirdParty_knet_trans where (Convert(DateTime, tdatetime, 103)) " & _
                                " between convert(datetime,'" & fromdate & "', 103) " & _
                                " and convert(datetime,'" & todate & "', 103)  and udf2 is not null and udf2 not like ''"
            End If

            sqlPayment += " and knetprocess like 'Captured%' and ptype like 'CashU'"
            sqlPaymentNew += " and knetprocess like 'Captured%' and ptype like 'CashU'"
        ElseIf ddlPayment.SelectedItem.Text = "AMEX" Then
            sqlPayment += " and ptype like 'AMEX' and knetprocess like 'Approved%'"
            sqlPaymentNew += " and ptype like 'AMEX' and knetprocess like 'Approved%'"
        Else 'All

            sqlPayment += " and (knetprocess like 'Approved%' or knetprocess like 'Captured%') "
            sqlPaymentNew += " and (knetprocess like 'Approved%' or knetprocess like 'Captured%') "

        End If


        If ddlServiceCode.SelectedItem.Text = "Intl" Then
            sqlPayment += " and udf2 like 'INTl%'"
            sqlPaymentNew += " and udf2 like 'INTl%'"

        ElseIf ddlServiceCode.SelectedItem.Text = "Local" Then
            sqlPayment += " and udf2 not like 'INTl%'"
            sqlPaymentNew += " and udf2 not like 'INTl%'"

        End If



        'sqlPayment += " group by KnetProcess,udf2 "
        'sqlPaymentNew += " group by KnetProcess,udf2 "
        'sqlPay = "select PaymentType,ServiceCode,sum(TotalAmount),sum(Transactions) from( " & sqlPayment & " union " & sqlPaymentNew & " )as temp  group by PaymentType,ServiceCode  order by PaymentType"

        'sqlPay = "select * from( " & sqlPayment & " union " & sqlPaymentNew & " )as temp    order by myid desc"
        sqlPay = "select * from( " & sqlPayment & " )as temp order by myid desc"

        Session("sqlPay") = sqlPay
        sqldatabind(sqlPay, False)

    End Sub

    Protected Sub CheckBox2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged



    End Sub

    Protected Sub ddlKnetTunnel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlKnetTunnel.SelectedIndexChanged

    End Sub

    Protected Sub ddlPayment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPayment.SelectedIndexChanged
        If ddlPayment.SelectedItem.Text.ToLower.Equals("knet") Then
            ddlKnetTunnel.Visible = True
            Label19.Visible = True
        Else
            ddlKnetTunnel.Visible = False
            Label19.Visible = False
        End If
    
    End Sub
End Class
