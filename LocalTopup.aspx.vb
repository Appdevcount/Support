Imports System.Data
Imports System.Drawing
Imports System.Data.SqlClient
Imports SMS
Imports Data

Partial Class LocalTopup
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim typeS As String = ""
    Dim Sql, SqlOrder As String
    Dim trck, servCode As String
    Dim constring As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "cs" Or Session("role") = "operations" Or Session("role") = "accounts" Or Session("role") = "qa") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=LocalTopup.aspx")
        End If

        cn = New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096;Connection Timeout=120")

        If Not IsPostBack Then
            btnOnay.Attributes.Add("onclick", "javascript:buttonChk()")
            btnRed.Attributes.Add("onclick", "javascript:buttonChk()")
            Sql = "Select ServiceCode from PayitServices where (ServiceCode NOT in ('iTunesB-O%', 'Alghanim', 'AlghanimT-Y')) order by ServiceCode"
            da = New SqlDataAdapter(Sql, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")
            DropDownList2.DataSource = ds.Tables("deta")
            DropDownList2.DataTextField = "ServiceCode"
            DropDownList2.DataValueField = "ServiceCode"
            DropDownList2.DataBind()
            DropDownList2.Items.Insert(0, New ListItem("All", "All"))
            DropDownList2.Items.Insert(1, New ListItem("Warba-W", "Warba-W"))
        End If

        If Page.IsPostBack = False Then
            Label14.Visible = False
            Label15.Visible = False
            FromDateTextBox.Text = Format(Date.Today.Date, "dd/MM/yyyy")
            ToDateTextBox.Text = Format(Date.Today.Date, "dd/MM/yyyy")
            Label17.Text = ""
            Dim typeS As String = ""
            typeS = Request.QueryString("type")
            If typeS = "fail" Then
                Label8.Text = "Payment Transactions"
                ddlServiceProvider.Enabled = False

                DropDownList3.Visible = True
                Label21.Visible = True
                CheckBox2.Checked = False
                lblComm.Visible = False
                chkCommission.Visible = False
            Else
                ddlServiceProvider.Enabled = True
                Label8.Text = "Success Transactions"
                DropDownList3.Visible = False
                Label21.Visible = False
                chkReprocess.Checked = False
                chkReprocess.Visible = False
                Label2.Visible = False
            End If
            counter1()
        End If
    End Sub

    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        typeS = Request.QueryString("type")
        If chkReprocess.Checked Then
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim cell As TableCell = e.Row.Cells(11)
                Dim cell1 As TableCell = e.Row.Cells(3)
                Dim cell2 As TableCell = e.Row.Cells(4)
                Dim cell3 As TableCell = e.Row.Cells(2)
                Dim trackid As String = String.Format(cell3.Text)
                Dim status As String = String.Format(cell.Text)
                Dim service As String = String.Format(cell1.Text)
                Dim pins As String() = New String() {}
                Dim pin, word As String, denomination = 0, pin1 = 0, pin2 = 0, pin3 = 0

                Try
                    If service.ToLower().EndsWith("-o") And service <> "" Then
                        If status.ToLower().StartsWith("ok2") And status <> "" Then
                            Dim s As String = cell.Text
                            Dim parts As String() = s.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
                            word = Trim(parts(0))
                            pin = Trim(parts(1))
                            If pin.Contains(",") And pin <> "" Then
                                pins = pin.Split(New Char() {","c})
                            End If

                            If (pin.Length > 0) Then
                                Dim sqlPIN As String = String.Empty
                                If (pins.Length = 0) Then
                                    sqlPIN = "SELECT Serial from [payit].[dbo].[PINS] where ID=" & pin
                                Else
                                    If (pins.Length <> 0) Then
                                        sqlPIN = "SELECT Serial from [payit].[dbo].[PINS] where ID in (" & (pin) & ")"
                                    End If
                                End If
                                Dim displayPINS As String = String.Empty
                                Using con As New SqlConnection(constring)
                                    Using cmd As New SqlCommand(sqlPIN)
                                        Using dt As New DataTable()
                                            cmd.CommandText = sqlPIN
                                            da = New SqlDataAdapter(sqlPIN, con)
                                            ds = New DataSet()
                                            da.Fill(ds, "deta")
                                            If Me.IsPostBack Then
                                                If ds.Tables.Count > 0 Then
                                                    For Each myRow As DataRow In ds.Tables(0).Rows
                                                        If Not IsDBNull(myRow("Serial")) AndAlso myRow("Serial") <> Nothing Then
                                                            displayPINS = displayPINS & " " & myRow("Serial").ToString()
                                                        End If
                                                    Next
                                                    cell2.Text = displayPINS
                                                End If
                                            End If
                                        End Using
                                    End Using
                                End Using
                            End If
                        End If
                        'ElseIf service.ToLower().Equals("wataniya-p") Then
                        '    Dim sqlTrack As String = String.Empty
                        '    sqlTrack = "SELECT TransactionID from [payit].[dbo].[PayitiSYS] where [IsysID]=" & trackid & " and Trandate ="
                        '    Dim displayPINS As String = String.Empty
                        '    Using con As New SqlConnection(constring)
                        '        Using cmd As New SqlCommand(sqlTrack)
                        '            Using dt As New DataTable()
                        '                cmd.CommandText = sqlTrack
                        '                da = New SqlDataAdapter(sqlTrack, con)
                        '                ds = New DataSet()
                        '                da.Fill(ds, "deta")
                        '                If Me.IsPostBack Then
                        '                    If ds.Tables.Count > 0 Then
                        '                        For Each myRow As DataRow In ds.Tables(0).Rows
                        '                            If Not IsDBNull(myRow("TransactionID")) AndAlso myRow("TransactionID") <> Nothing Then
                        '                                displayPINS = displayPINS & " " & myRow("TransactionID").ToString()
                        '                            End If
                        '                        Next
                        '                        cell2.Text = displayPINS
                        '                    End If
                        '                End If
                        '            End Using
                        '        End Using
                        '    End Using
                    End If
                Catch ex As Exception
                    Console.WriteLine("Error")
                    Exit Sub
                End Try
            End If
            Exit Sub
        End If

        If (typeS = "fail") Then
            Exit Sub
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim cell As TableCell = e.Row.Cells(10)
            Dim cell1 As TableCell = e.Row.Cells(3)
            Dim cell2 As TableCell = e.Row.Cells(4)
            Dim cell3 As TableCell = e.Row.Cells(2)
            Dim trackid As String = String.Format(cell3.Text)
            Dim status As String = String.Format(cell.Text)
            Dim service As String = String.Format(cell1.Text)
            Dim pins As String() = New String() {}
            Dim i As Integer = 0
            Dim pin, word As String, denomination = 0, pin1 = 0, pin2 = 0, pin3 = 0

            Try
                If service.ToLower().EndsWith("-o") And service <> "" Then
                    If status.ToLower().StartsWith("ok2") And status <> "" Then
                        Dim s As String = cell.Text
                        Dim parts As String() = s.Split(New Char() {" "c})
                        word = Trim(parts(0))
                        pin = Trim(parts(1))
                        If pin.Contains(",") And pin <> "" Then
                            pins = pin.Split(New Char() {","c})
                        End If

                        If (pin.Length > 0) Then
                            Dim sqlPIN As String = String.Empty
                            If (pins.Length = 0) Then
                                sqlPIN = "SELECT Serial, Amount2 from [payit].[dbo].[PINS] where ID=" & pin
                            Else
                                If (pins.Length <> 0) Then
                                    sqlPIN = "SELECT Serial, Amount2 from [payit].[dbo].[PINS] where ID in (" & (pin) & ")"
                                End If
                            End If
                            Dim displayPINS As String = String.Empty
                            Using con As New SqlConnection(constring)
                                Using cmd As New SqlCommand(sqlPIN)
                                    Using dt As New DataTable()
                                        cmd.CommandText = sqlPIN
                                        da = New SqlDataAdapter(sqlPIN, con)
                                        ds = New DataSet()
                                        da.Fill(ds, "deta")
                                        If Me.IsPostBack Then
                                            If ds.Tables.Count > 0 Then
                                                For Each myRow As DataRow In ds.Tables(0).Rows
                                                    If Not IsDBNull(myRow("Serial")) AndAlso myRow("Serial") <> Nothing Then
                                                        displayPINS = displayPINS & " " & myRow("Serial").ToString()
                                                    End If
                                                    If Not IsDBNull(myRow("Amount2")) AndAlso myRow("Amount2") <> Nothing Then
                                                        displayPINS = displayPINS & " " & myRow("Amount2").ToString()
                                                    End If
                                                Next
                                                cell2.Text = displayPINS
                                            End If
                                        End If
                                    End Using
                                End Using
                            End Using
                        End If
                    End If
                    'ElseIf service.ToLower().Equals("wataniya-p") Then
                    '    Dim sqlTrack As String = String.Empty
                    '    sqlTrack = "SELECT TransactionID from [payit].[dbo].[PayitiSYS] where [IsysID]=" & trackid
                    '    Dim displayPINS As String = String.Empty
                    '    Using con As New SqlConnection(constring)
                    '        Using cmd As New SqlCommand(sqlTrack)
                    '            Using dt As New DataTable()
                    '                cmd.CommandText = sqlTrack
                    '                da = New SqlDataAdapter(sqlTrack, con)
                    '                ds = New DataSet()
                    '                da.Fill(ds, "deta")
                    '                If Me.IsPostBack Then
                    '                    If ds.Tables.Count > 0 Then
                    '                        For Each myRow As DataRow In ds.Tables(0).Rows
                    '                            If Not IsDBNull(myRow("TransactionID")) AndAlso myRow("TransactionID") <> Nothing Then
                    '                                displayPINS = displayPINS & " " & myRow("TransactionID").ToString()
                    '                            End If
                    '                        Next
                    '                        cell2.Text = displayPINS
                    '                    End If
                    '                End If
                    '            End Using
                    '        End Using
                    '    End Using
                End If
            Catch ex As Exception
                Console.WriteLine("Error")
                Exit Sub
            End Try
        End If
    End Sub

    Protected Sub DropDownList2_SelectedIndexChanged(sender As Object, e As EventArgs)
        'Dim ServiceVoucher As String = DropDownList2.SelectedItem.Text
        'Sql = "Select ServiceCode from PayitServices WHERE ServiceCode LIKE '%-O%' order by ServiceOrder"
        'da = New SqlDataAdapter(Sql, cn)
        'ds = New DataSet()
        'da.Fill(ds, "deta")
        'If ds.Tables.Count > 0 Then
        '    'Dim s As String = ds.Rows(1)("mycolumn1").ToString()
        '    For Each myRow As DataRow In ds.Tables(0).Rows
        '        'If Not IsDBNull(myRow("ServiceCode")) AndAlso myRow("ServiceCode") <> Nothing Then
        '        '    Label11.Text = myRow("ServiceCode").ToString()
        '        'End If
        '        'If (ServiceVoucher = myRow("ServiceCode")) Then
        '        'End If
        '    Next
        'End If
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click

        typeS = Request.QueryString("type")
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

        If typeS = "fail" Then

            If chkReprocess.Checked Then

                sqlLocalTrans = "select tk.TrackID,isnull(tk.udf2,'Unknown') serviceCode,isnull(t3.serial,'No Serial') Serial,tk.udf3 Mob, tk.amt Amount," & _
                                     " tk.knetProcess,tk.udf1 Result,tk.tdatetime TranDate,tk.LastUpdateOn ProcessDate,t2.ProcessTranDescription Description " & _
                                     " from  " & DropDownList3.SelectedItem.Text & " tk LEFT OUTER join PayitiSYS as t2 on tk.trackid = t2.IsysID LEFT OUTER join PINS as t3 on t2.OperatorOrderID = t3.ID " & _
                                   " where convert(datetime,tk.LastUpdateOn, 103) " & _
                     " between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103)" & _
                     " and (udf1 like 'SuccessRI%' and udf1 is not null) and ( KnetProcess like 'CAPTURED%' or KnetProcess like 'Approved%') "


                If DropDownList2.SelectedItem.Text <> "All" And DropDownList2.SelectedItem.Text <> "NULL" Then
                    sqlLocalTrans += " and tk.udf2 like '" & Trim(DropDownList2.SelectedItem.Text) & "%' "
                End If

                If Trim(MobileNOTextBox.Text) <> "" Then
                    sqlLocalTrans += "and (tk.udf3 like '" & Trim(MobileNOTextBox.Text) & "%') "
                End If
                If Trim(AmountTextBox.Text) <> "" Then
                    sqlLocalTrans += " and tk.amt like '" & Trim(AmountTextBox.Text) & "' "
                End If
                If Trim(TrackIDTextBox.Text) <> "" Then
                    sqlLocalTrans += " and tk.trackid like '" & Trim(TrackIDTextBox.Text) & "%'"
                End If
                'sqlLocalTrans += " order by tk.LastUpdateOn desc"
                Session("sqlLocalTrans") = sqlLocalTrans
                sqldatabind(sqlLocalTrans, "successRI", "first")
                Exit Sub
            End If

            If CheckBox2.Checked Then

                sqlLocalTrans = "select tk.TrackID,isnull(tk.udf2,'Unknown') serviceCode,'No Serial' Serial,tk.udf3 Mob, tk.amt Amount," & _
                                      " tk.knetProcess,tk.tdatetime TranDate,tk.LastUpdateOn ProcessDate  " & _
                                      " from  " & DropDownList3.SelectedItem.Text & " tk FULL OUTER JOIN PayitiSYS as t2 ON tk.trackid = t2.IsysID " & _
                                      " where convert(datetime,tk.LastUpdateOn, 103) " & _
                                      " between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103)" & _
                                      " and ( KnetProcess like 'CAPTURED%' or KnetProcess like 'Approved%') "

                If DropDownList2.SelectedItem.Text <> "All" And DropDownList2.SelectedItem.Text <> "NULL" Then
                    sqlLocalTrans += " and tk.udf2 like '" & Trim(DropDownList2.SelectedItem.Text) & "%' "
                End If

                If Trim(MobileNOTextBox.Text) <> "" Then
                    sqlLocalTrans += "and (tk.udf3 like '" & Trim(MobileNOTextBox.Text) & "%') "
                End If
                If Trim(AmountTextBox.Text) <> "" Then
                    sqlLocalTrans += " and tk.amt like '" & Trim(AmountTextBox.Text) & "' "
                End If
                If Trim(TrackIDTextBox.Text) <> "" Then
                    sqlLocalTrans += " and tk.trackid like '" & Trim(TrackIDTextBox.Text) & "%'"
                End If
                'sqlLocalTrans += " order by tk.LastUpdateOn desc"
                Session("sqlLocalTrans") = sqlLocalTrans
                sqldatabind(sqlLocalTrans, "fail", "first")
            Else
                sqlLocalTrans = "select  tk.TrackID,isnull(tk.udf2,'Unknown') serviceCode,'No Serial' Serial,tk.udf3 Mob, tk.amt Amount,tk.knetProcess, case when CHARINDEX('|',udf4)>0 " & _
                    "then SUBSTRING(udf4,1,CHARINDEX('|',udf4)-1) " & _
                    "else udf4 end 'No.of PINS', " & _
                    "CASE WHEN CHARINDEX('|',udf4)>0 " & _
                    "THEN SUBSTRING(udf4,CHARINDEX('|',udf4)+1,len(udf4))  " & _
                    "ELSE NULL END as 'PIN Amnt', " & _
                    " tk.tdatetime TranDate,tk.LastUpdateOn ProcessDate " & _
                    " from  " & DropDownList3.SelectedItem.Text & " tk FULL OUTER JOIN PayitiSYS as t2 ON tk.trackid = t2.IsysID " & _
                    " where convert(datetime,tk.LastUpdateOn, 103) " & _
                    " between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103)" & _
                    " and (udf1 not like 'Success%' or udf1 is null) and ( KnetProcess like 'CAPTURED%' or KnetProcess like 'Approved%') "

                If DropDownList2.SelectedItem.Text <> "All" And DropDownList2.SelectedItem.Text <> "NULL" Then
                    sqlLocalTrans += " and tk.udf2 like '" & Trim(DropDownList2.SelectedItem.Text) & "%' "
                End If

                If Trim(MobileNOTextBox.Text) <> "" Then
                    sqlLocalTrans += "and (tk.udf3 like '" & Trim(MobileNOTextBox.Text) & "%') "
                End If
                If Trim(AmountTextBox.Text) <> "" Then
                    sqlLocalTrans += " and tk.amt like '" & Trim(AmountTextBox.Text) & "' "
                End If
                If Trim(TrackIDTextBox.Text) <> "" Then
                    sqlLocalTrans += " and tk.trackid like '" & Trim(TrackIDTextBox.Text) & "%'"
                End If
                'sqlLocalTrans += " order by tk.LastUpdateOn desc"
                Session("sqlLocalTrans") = sqlLocalTrans
                sqldatabind(sqlLocalTrans, "fail", "first")
            End If

            If typeS = "fail" Then

            ElseIf CheckBox2.Checked Then
                sqlLocalTrans = "select tk.TrackID,isnull(tk.udf2,'Unknown') serviceCode,'No Serial' Serial,tk.udf3 Mob, tk.amt Amount," & _
                                      " tk.knetProcess,isnull(tk.udf1,'Unprocessed') Result,tk.tdatetime TranDate,tk.LastUpdateOn ProcessDate  " & _
                                      " from  " & DropDownList3.SelectedItem.Text & " tk FULL OUTER JOIN PayitiSYS as t2 ON tk.trackid = t2.IsysID " & _
                                      " where convert(datetime,tk.LastUpdateOn, 103) " & _
                                      " between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103)" & _
                                      " and ( KnetProcess like 'CAPTURED%' or KnetProcess like 'Approved%') "

                If DropDownList2.SelectedItem.Text <> "All" And DropDownList2.SelectedItem.Text <> "NULL" Then
                    sqlLocalTrans += " and tk.udf2 like '" & Trim(DropDownList2.SelectedItem.Text) & "%' "
                End If

                If Trim(MobileNOTextBox.Text) <> "" Then
                    sqlLocalTrans += "and (tk.udf3 like '" & Trim(MobileNOTextBox.Text) & "%') "
                End If
                If Trim(AmountTextBox.Text) <> "" Then
                    sqlLocalTrans += " and tk.amt like '" & Trim(AmountTextBox.Text) & "' "
                End If
                If Trim(TrackIDTextBox.Text) <> "" Then
                    sqlLocalTrans += " and tk.trackid like '" & Trim(TrackIDTextBox.Text) & "%'"
                End If
                'sqlLocalTrans += " order by tk.LastUpdateOn desc"
                Session("sqlLocalTrans") = sqlLocalTrans
                sqldatabind(sqlLocalTrans, "fail", "first")
            Else
                sqlLocalTrans = "select tk.TrackID,isnull(tk.udf2,'Unknown') serviceCode,isnull(t3.serial,'No Serial') Serial,tk.udf3 Mob, tk.amt Amount," & _
                      " tk.knetProcess,tk.tdatetime TranDate,tk.LastUpdateOn ProcessDate " & _
                      " from  " & DropDownList3.SelectedItem.Text & " tk FULL OUTER JOIN PayitiSYS as t2 ON tk.trackid = t2.IsysID FULL OUTER JOIN PINS as t3 ON t2.OperatorOrderID = t3.ID " & _
                      " where convert(datetime,tk.LastUpdateOn, 103) " & _
                      " between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103)" & _
                      " and (udf1 not like 'Success%' or udf1 is null) and ( KnetProcess like 'CAPTURED%' or KnetProcess like 'Approved%') "

                If DropDownList2.SelectedItem.Text <> "All" And DropDownList2.SelectedItem.Text <> "NULL" Then
                    sqlLocalTrans += " and tk.udf2 like '" & Trim(DropDownList2.SelectedItem.Text) & "%' "
                End If

                If Trim(MobileNOTextBox.Text) <> "" Then
                    sqlLocalTrans += "and (tk.udf3 like '" & Trim(MobileNOTextBox.Text) & "%') "
                End If
                If Trim(AmountTextBox.Text) <> "" Then
                    sqlLocalTrans += " and tk.amt like '" & Trim(AmountTextBox.Text) & "' "
                End If
                If Trim(TrackIDTextBox.Text) <> "" Then
                    sqlLocalTrans += " and tk.trackid like '" & Trim(TrackIDTextBox.Text) & "%'"
                End If
                'sqlLocalTrans += " order by tk.LastUpdateOn desc"
                Session("sqlLocalTrans") = sqlLocalTrans
                sqldatabind(sqlLocalTrans, "fail", "first")
            End If

        Else

            If (chkCommission.Checked = True) Then
                sqlLocalTrans = "select tk.TrackID,isnull(tk.udf2,'Unknown') serviceCode, " & _
                                "tk.udf3 Mob, tk.amt Amount, isnull(tk.knetProcess,'Unprocessed') KNET,isnull(tk.udf1,'Unprocessed') Result,tk.tdatetime TranDate, " & _
                                "tk.LastUpdateOn ProcessDate,CASE WHEN CHARINDEX('|',udf4)>0 THEN SUBSTRING(udf4,CHARINDEX('|',udf4)+1,len(udf4))  " & _
                                "ELSE amt END as 'Amnt' from  " & _
                                "ThirdParty_knet_trans tk LEFT OUTER join PayitiSYS as t2 on tk.trackid = t2.IsysID " & _
                                " where convert(datetime,tk.LastUpdateOn, 103) " & _
                                " between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103)" & _
                                " and (udf1 is not null) and ( KnetProcess like 'CAPTURED%' or KnetProcess like 'Approved%') "

                If DropDownList2.SelectedItem.Text <> "All" And DropDownList2.SelectedItem.Text <> "NULL" Then
                    sqlLocalTrans += " and tk.udf2 like '" & Trim(DropDownList2.SelectedItem.Text) & "%' "
                End If

                If Trim(MobileNOTextBox.Text) <> "" Then
                    sqlLocalTrans += "and (tk.udf3 like '" & Trim(MobileNOTextBox.Text) & "%') "
                End If
                If Trim(AmountTextBox.Text) <> "" Then
                    sqlLocalTrans += " and tk.amt like '" & Trim(AmountTextBox.Text) & "' "
                End If
                If Trim(TrackIDTextBox.Text) <> "" Then
                    sqlLocalTrans += " and tk.trackid like '" & Trim(TrackIDTextBox.Text) & "%'"
                End If
                'sqlLocalTrans += " order by tk.LastUpdateOn desc"
                Session("sqlLocalTrans") = sqlLocalTrans
                sqldatabind(sqlLocalTrans, "successRI", "first")
                Exit Sub
            End If


            If CheckBox2.Checked Then
                sqlLocalTrans = "select tk.IsysID TrackID,tk.[Service] ServiceCode,isnull(pin.serial,'No Serial')  + ' ' + pin.Amount2 as Serial,tk.MobileNO,tk.Amount,tk.TranDate,tk.ProcessTranDate ProcessDate,tk.PaymentAPI,tk.ProcessTranDescription Description " & _
              " from PayitiSYS as tk LEFT JOIN PINS as pin ON tk.OperatorOrderID = pin.ID where convert(datetime,tk.TranDate, 103)" & _
              " between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) " & _
              " and tk.PaymentAPI NOT LIKE '%POS' and tk.ProcessTranDescription NOT like 'ADDCRDT'"

                If ddlServiceProvider.SelectedItem.Text <> "All" Then
                    sqlLocalTrans += " and tk.PaymentAPI like '" & ddlServiceProvider.SelectedItem.Text & "' "
                End If

                If DropDownList2.SelectedItem.Text <> "All" And DropDownList2.SelectedItem.Text <> "NULL" Then
                    sqlLocalTrans += " and tk.[Service] like '" & Trim(DropDownList2.SelectedItem.Text) & "%' "
                End If

                If Trim(MobileNOTextBox.Text) <> "" Then
                    sqlLocalTrans += "and (tk.MobileNO like '" & Trim(MobileNOTextBox.Text) & "%') "
                End If
                If Trim(AmountTextBox.Text) <> "" Then
                    sqlLocalTrans += " and tk.Amount like '" & Trim(AmountTextBox.Text) & "' "
                End If
                If Trim(TrackIDTextBox.Text) <> "" Then
                    sqlLocalTrans += " and tk.IsysID like '" & Trim(TrackIDTextBox.Text) & "%'"
                End If
                ' sqlLocalTrans += " order by  id desc"
                Session("sqlLocalTrans") = sqlLocalTrans
                sqldatabind(sqlLocalTrans, "success", "first")
            Else
                sqlLocalTrans = "select tk.IsysID TrackID,tk.[Service] ServiceCode,isnull(pin.serial,'No Serial') Serial,tk.MobileNO,tk.Amount,tk.TranDate,tk.ProcessTranDate ProcessDate,tk.PaymentAPI,tk.ProcessTranDescription Description " & _
                                " from PayitiSYS as tk LEFT JOIN PINS as pin ON tk.OperatorOrderID = pin.ID where convert(datetime,tk.TranDate, 103)" & _
                                " between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) " & _
                                " and tk.PaymentAPI NOT LIKE '%POS' and ((tk.ProcessTranDescription like '%SUCCESS%' " & _
                                " or (tk.ProcessTranDescription like '%ePayments: Your transaction has taken longer than expected%' and tk.[service] like 'VIVA%')) " & _
                                " or (tk.ProcessTranDescription like 'OK%' and tk.Service like '%-O') or (tk.[service] like 'WATANIYA-X' AND (tk.ProcessTranDescription like '%P%' or tk.ProcessTranDescription like 'Q' or tk.ProcessTranDescription like 'NO')))"

                If ddlServiceProvider.SelectedItem.Text <> "All" Then
                    sqlLocalTrans += " and tk.PaymentAPI like '" & ddlServiceProvider.SelectedItem.Text & "' "
                End If

                If DropDownList2.SelectedItem.Text <> "All" And DropDownList2.SelectedItem.Text <> "NULL" Then
                    sqlLocalTrans += " and tk.[Service] like '" & Trim(DropDownList2.SelectedItem.Text) & "%' "
                End If

                If Trim(MobileNOTextBox.Text) <> "" Then
                    sqlLocalTrans += "and (tk.MobileNO like '" & Trim(MobileNOTextBox.Text) & "%') "
                End If
                If Trim(AmountTextBox.Text) <> "" Then
                    sqlLocalTrans += " and tk.Amount like '" & Trim(AmountTextBox.Text) & "' "
                End If
                If Trim(TrackIDTextBox.Text) <> "" Then
                    sqlLocalTrans += " and tk.IsysID like '" & Trim(TrackIDTextBox.Text) & "%'"
                End If
                ' sqlLocalTrans += " order by  id desc"
                Session("sqlLocalTrans") = sqlLocalTrans
                sqldatabind(sqlLocalTrans, "success", "first")
            End If
        End If

    End Sub
   
    Public Sub counter1()
        Button2.Attributes.Add("onclick", "javascript:abct()")
        Button2.Attributes.Add("CssClass", "btn btn-info m-progress")
    End Sub

    Public Sub sqldatabind(ByVal s As String, Optional ByVal types1 As String = "", Optional ByVal type As String = "")

        Session("type") = types1
        If types1 = "success" Then
            SqlOrder = s & " order by tk.id desc"
        ElseIf types1 = "successRI" Then
            SqlOrder = s & " order by tk.LastUpdateOn desc"
        Else
            SqlOrder = s & " order by tk.LastUpdateOn desc"
        End If
        da = New SqlDataAdapter(SqlOrder, cn)
        ds = New DataSet()

        da.Fill(ds, "det")
        If CheckBox1.Checked = True Then
            GridView1.AllowPaging = True
        Else
            GridView1.AllowPaging = False
        End If
        If Request.QueryString("type") = "fail" Then
            Label8.Text = "Payment Transactions"
            GridView1.Columns(0).Visible = True

        Else
            GridView1.Columns(0).Visible = True
            Label8.Text = "Local Success Transactions"
        End If
        GridView1.DataSource = String.Empty
        GridView1.DataSource = ds.Tables("det")
        GridView1.DataBind()
        Dim totalRows As Integer
        Dim total As Double
        totalRows = GridView1.Rows.Count

        For i = 0 To totalRows - 1
            total = total + Val(GridView1.Rows(i).Cells(6).Text)
        Next

        Dim grandTotSql As String = ""
        grandTotSql = "select sum(convert(float,Amount)),count(*)  from ( " & s
        grandTotSql += " ) as temp1"
        Dim da1 As SqlDataAdapter
        Dim ds1 As DataSet

        da1 = New SqlDataAdapter(grandTotSql, cn)
        ds1 = New DataSet()
        da1.Fill(ds1, "TCount")
        Label19.Text = "GrandTotal: " & ds1.Tables("TCount").Rows(0).Item(0)
        Label20.Text = "TotalTransactions: " & ds1.Tables("TCount").Rows(0).Item(1)
        Label17.Text = "Total Amount: " & total

    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        Dim s As String
        s = Session("sqlLocalTrans")
        sqldatabind(s, Session("type"))
    End Sub

    Protected Sub CheckLinkButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Not (Session("role") = "superadmin" Or Session("role") = "cs" Or Session("role") = "operations" Or Session("role") = "accounts") Then
            alert("You are not authorized to perform this action")
            Exit Sub
        End If

        Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        Dim orderid As String = "0"
        Dim res, check As String
        check = row.Cells(1).Text
        trck = row.Cells(2).Text
        servCode = row.Cells(3).Text
        Dim transactId = row.Cells(4).Text
        Dim datetimeNow As New DateTime
        Dim db As New payitEntities()

        If (servCode.ToLower().StartsWith("zain-p") Or servCode.ToLower().StartsWith("zain-x")) Then
            Try
                Dim IsysAPI As New IsysPaymentAPIZain.Zain_Kuwait_ZDP
                Dim rows = (From c In db.PayitiSYS Order By c.ID Descending Where c.IsysID = trck Select c.TransactionID).FirstOrDefault()
                If Not rows Is Nothing And rows.HasValue Then
                    res = IsysAPI.GetRechargeStatus(rows.Value, servCode)
                    alert(res)
                Else
                    alert("Failed Transaction. Please reprocess.")
                End If
            Catch ex As Exception
                alert("Issue from Zain. Please contact operator.")
            End Try
        ElseIf (servCode.ToLower().StartsWith("wataniya")) Then
            Try
                typeS = Request.QueryString("type")
                If typeS = "fail" Then
                    datetimeNow = row.Cells(10).Text
                Else
                    datetimeNow = row.Cells(8).Text
                End If

                Dim IsysAPIOoredoo As New IsysPaymentAPIOoredoo.Ooredoo_Kuwait
                Dim rows = (From c In db.PayitiSYS Order By c.ID Descending Where c.IsysID = trck Select c)
                If (rows IsNot Nothing) Then
                    For Each item In rows
                        If (String.Compare(datetimeNow, item.ProcessTranDate) = 0) Then
                            If item.TransactionID IsNot Nothing Then
                                res = IsysAPIOoredoo.GetRechargeStatus(item.TransactionID, servCode)
                                alert(res)
                            End If
                        End If
                    Next
                Else
                    alert("Failed Transaction. Please reprocess.")
                End If
            Catch ex As Exception
                alert("Issue from Ooredoo. Please contact operator.")
            End Try
        ElseIf (servCode.ToLower().StartsWith("alghanim-y") Or servCode.ToLower().StartsWith("alamna-y") Or servCode.ToLower().StartsWith("safat-y")) Then
            Try
                Dim IsysAPIAlghanim As New IsysPaymentAPIAlghanim.Alghanim
                Dim result As New IsysPaymentAPIAlghanim.InstRespforDeals
                Dim rows = (From c In db.ThirdParty_knet_trans Order By c.myid Descending Where c.trackid = trck Select c.udf3).FirstOrDefault()
                If (rows IsNot Nothing) Then
                    result = IsysAPIAlghanim.Installments(rows, "", "")
                    If (result IsNot Nothing) Then
                        If (result.myStatus = 0) Then
                            res = "CivilID: " & rows.ToString() & ", Account No: " & result.AccountNo & ", Installment Amount: " & result.totalAmount & ", Installments: " & result.totalInstallments
                        Else
                            res = "Error retrieving data from Alghanim. Please try again"
                        End If
                        alert(res)
                    End If
                Else
                    alert("Failed Transaction. Please reprocess.")
                End If
            Catch ex As Exception
                alert("Issue from Ooredoo. Please contact operator.")
            End Try

        ElseIf (servCode.ToLower().StartsWith("intl")) Then
            Dim intl As New InternationalTopup.ServiceSoapClient()

            Try
                Dim rows = (From c In db.ThirdpartyServiceTransactions Where c.TrackId = trck Select c).FirstOrDefault()
                If Not (rows Is Nothing) Then
                    If (Not (String.IsNullOrEmpty(rows.ServiceResponse))) Then
                        alert(rows.ServiceResponse)
                    Else
                        alert("Try Processing")
                    End If
                End If
            Catch ex As Exception

            End Try
            

        Else
            alertNotify("Not enabled for this service")
        End If
    End Sub

    Protected Sub ReprocessLinkButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Not (Session("role") = "superadmin" Or Session("role") = "cs" Or Session("role") = "operations") Then
            alert("You are not authorized to perform this action")
            Exit Sub
        End If
        Dim knetCheck As Boolean = True

        If (sender.Text.ToString().Equals("Yes Reprocess")) Then
            Reprocess("")
        Else
            Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
            Dim lb As LinkButton = TryCast(sender, LinkButton)
            Dim trackIDC = lb.CommandArgument()
            'knetCheck = ConfirmKnet(trackIDC)
            knetCheck = True
            If (knetCheck = False) Then
                popup.Show()
                Exit Sub
            End If
            Reprocess(trackIDC)
        End If
    End Sub

    Public Function validateTrackID(ByVal isysID As String) As Boolean
        Dim SqlValidate As String
        Dim retValue As Boolean
        SqlValidate = "select ProcessTranDescription from PayitiSYS where isysid = " & isysID & " and ProcessTranDescription LIKE '%SUCCESS%'"

        Dim dsValidate As DataSet
        dsValidate = New DataSet()

        da = New SqlDataAdapter(SqlValidate, cn)
        da.Fill(dsValidate, "det")
        If dsValidate.Tables("det").Rows.Count > 0 Then
            retValue = True
        Else
            retValue = False
        End If
        Return retValue
    End Function

    Public Function validateTrackIDVoucher(ByVal isysID As String) As Boolean
        Dim SqlValidate As String
        Dim retValue As Boolean
        SqlValidate = "select ProcessTranDescription from PayitiSYS where isysid like '" & isysID & "'"

        Dim dsValidate As DataSet
        dsValidate = New DataSet()

        da = New SqlDataAdapter(SqlValidate, cn)
        da.Fill(dsValidate, "det")
        If dsValidate.Tables("det").Rows.Count > 0 Then
            retValue = True
        Else
            retValue = False
        End If
        Return retValue
    End Function

    Public Function GetProcessedPIN(ByVal isysID As String) As String
        If cn.State = ConnectionState.Closed Then cn.Open()
        Dim cmd As New SqlCommand("ProcessSuccessPINnew", cn)
        cmd.CommandType = System.Data.CommandType.StoredProcedure

        Dim serParm As New SqlParameter("@trackid", SqlDbType.VarChar)
        serParm.Size = 50
        serParm.Direction = ParameterDirection.Input
        serParm.Value = isysID
        cmd.Parameters.Add(serParm)

        Dim result As String
        Using rdr As SqlDataReader = cmd.ExecuteReader
            rdr.Read()
            result = "Serial: " & rdr("Serial") & ", PIN: " & rdr("PIN")
            rdr.Close()
        End Using
        cmd = Nothing
        Return result
    End Function

    Public Function GetService(ByVal isysID As String) As String
        Dim cmd As New SqlCommand("select service from PayitiSYS where isysid = " & isysID, cn)

        Dim result As String
        If cn.State = ConnectionState.Closed Then cn.Open()

        Using rdr As SqlDataReader = cmd.ExecuteReader
            rdr.Read()
            result = rdr.GetString(0)
            rdr.Close()
        End Using

        cmd = Nothing
        Return result
    End Function

    Public Function GetThirdPartyKnetTransStatus(ByVal isysID As String, _
                                                 ByRef service As String, _
                                                 ByRef amount As String, _
                                                 ByRef multipinamnt As String, _
                                                 ByRef mobileno As String, _
                                                 ByRef quantity As String) As String()

        Dim cmd As New SqlCommand("select isnull(udf1,'unprocessed'), isnull(udf2,'unknown'), ISNULL(udf3,'') Mobile, amt, CASE WHEN CHARINDEX('|',udf4)>0 THEN SUBSTRING(udf4,CHARINDEX('|',udf4)+1,len(udf4)) ELSE amt END as multipinamnt, CASE WHEN CHARINDEX('|',udf4)>0 THEN SUBSTRING(udf4,CHARINDEX('|',udf4)-1,len(udf4)) ELSE '1' END  as Quantity from " & DropDownList3.SelectedItem.Text & " where isysid like '" & isysID & "%'", cn)

        Dim result(2) As String

        If cn.State = ConnectionState.Closed Then cn.Open()

        Using rdr As SqlDataReader = cmd.ExecuteReader
            rdr.Read()

            result(0) = rdr.GetString(0)
            result(1) = rdr.GetString(1)
            service = rdr.GetString(1)
            mobileno = rdr.GetString(2)
            amount = rdr.GetString(3)
            multipinamnt = rdr.GetString(4)
            quantity = rdr.GetString(5)

            rdr.Close()
        End Using

        cmd = Nothing

        Return result
    End Function

    Public Function GetProcessDescription(ByVal isysID As String, Optional ByRef mobileNo As String = "") As String

        Dim fromdate, todate As String
        Dim result As String = String.Empty
        fromdate = Trim(FromDateTextBox.Text) & " 00:00:00"
        todate = Trim(ToDateTextBox.Text) & " 23:59:59"
        Dim _ctx As New payitEntities
        Dim row = (From d In _ctx.PayitiSYS Where d.IsysID = isysID Select d).FirstOrDefault()
        If (row IsNot Nothing) Then
            result = row.ProcessTranDescription
            mobileNo = row.MobileNo
        End If

        'Dim cmd As New SqlCommand("select ProcessTranDescription, MobileNo from PayitiSYS where convert(datetime,trandate, 103) " & _
        '                              " between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103)" & _
        '                              " and isysid like '" & isysID & "%'", cn)

        'If cn.State = ConnectionState.Closed Then cn.Open()
        'Using rdr As SqlDataReader = cmd.ExecuteReader
        '    rdr.Read()
        '    result = rdr.GetString(0)
        '    mobileNo = rdr.GetString(1)
        '    rdr.Close()
        'End Using
        'cmd = Nothing

        Return result
    End Function
   
    Private Function ProcessUnProcPIN(ByVal isysid As String, ByVal service As String, ByVal amount As String) As String
        If cn.State = ConnectionState.Closed Then cn.Open()
        Dim cmd As New SqlCommand("ProcessUnprocPin", cn)
        cmd.CommandType = System.Data.CommandType.StoredProcedure

        Dim serParm As New SqlParameter("@service", SqlDbType.VarChar)
        serParm.Size = 50
        serParm.Direction = ParameterDirection.Input
        serParm.Value = service
        cmd.Parameters.Add(serParm)

        Dim amtParm As New SqlParameter("@amt", SqlDbType.Float)
        amtParm.Direction = ParameterDirection.Input
        amtParm.Value = amount
        cmd.Parameters.Add(amtParm)

        Dim trackidParm As New SqlParameter("@trackid", SqlDbType.VarChar)
        trackidParm.Size = 50
        trackidParm.Direction = ParameterDirection.Input
        trackidParm.Value = isysid
        cmd.Parameters.Add(trackidParm)

        Dim ooid As New SqlParameter("@OOID", SqlDbType.BigInt)
        ooid.Direction = ParameterDirection.Output
        cmd.Parameters.Add(ooid)

        Dim pin As New SqlParameter("@pin", SqlDbType.VarChar)
        pin.Direction = ParameterDirection.Output
        pin.Size = 50
        cmd.Parameters.Add(pin)

        Dim r As SqlDataReader = cmd.ExecuteReader()
        'While r.NextResult()
        'End While

        'cn.Close()
        'cn.Open()
        'cmd.ExecuteNonQuery()

        r.Close()

        Dim fetchedPIN As String = cmd.Parameters("@pin").Value.ToString()

        cmd = Nothing

        Return fetchedPIN
    End Function

    Public Function getpayitisysInfo(ByVal trackid As String) As String
        Dim SqlTrackID As String
        Dim retValue As String
        SqlTrackID = "select IsysID, Service from PayitiSYS where IsysID like '" & trackid & "%'"
        Dim dsTrack As DataSet
        dsTrack = New DataSet()
        Dim da1 As New SqlDataAdapter(SqlTrackID, cn)

        ' da = New SqlDataAdapter(SqlTrackID, cn)
        da1.Fill(dsTrack, "d")
        If Not dsTrack.Tables("d").Rows.Count > 0 Then
            retValue = "NODATA"
        Else
            retValue = dsTrack.Tables("d").Rows(0).Item(0)
            'retValue = False
        End If
        Return retValue

    End Function

    Public Function getTrackIDInfo(ByVal isysID As String) As String()
        Dim SqlTrackID As String
        Dim retValue(6) As String
        SqlTrackID = "select udf2 ServiceCode,amt Amount,udf3 MobileNo,CASE WHEN CHARINDEX('|',udf4)>0 THEN SUBSTRING(udf4,CHARINDEX('|',udf4)+1,len(udf4)) " & _
                     "ELSE amt END as Denomination,tdatetime,LastUpdateOn from " & DropDownList3.SelectedItem.Text & " where TrackID like '" & isysID & "%'"

        Dim dsTrack As DataSet
        dsTrack = New DataSet()
        Dim da1 As New SqlDataAdapter(SqlTrackID, cn)

        ' da = New SqlDataAdapter(SqlTrackID, cn)
        da1.Fill(dsTrack, "d")
        If Not dsTrack.Tables("d").Rows.Count > 0 Then
            retValue(0) = "NODATA"
        Else
            retValue(0) = dsTrack.Tables("d").Rows(0).Item(0)
            retValue(1) = dsTrack.Tables("d").Rows(0).Item(1)
            retValue(2) = dsTrack.Tables("d").Rows(0).Item(2)
            retValue(3) = dsTrack.Tables("d").Rows(0).Item(3)
            retValue(3) = Math.Round(Convert.ToDecimal(retValue(3)), 3)
            retValue(4) = dsTrack.Tables("d").Rows(0).Item(4)
            retValue(5) = dsTrack.Tables("d").Rows(0).Item(5)
            'retValue = False
        End If
        Return retValue
    End Function

    Public Sub updateThirdpartyStatus(ByVal isysID As String, ByVal type As String)
        Dim SqlUpdate As String
        Dim retValue(3) As String

        Try
            If type = "" Then
                SqlUpdate = "update " & DropDownList3.SelectedItem.Text & " set udf1=NULL where trackID='" & isysID & "'"
            Else
                SqlUpdate = "update " & DropDownList3.SelectedItem.Text & " set udf1='" & type & "' where trackID='" & isysID & "'"
            End If

            Dim cmd As SqlCommand
            cmd = New SqlCommand(SqlUpdate, cn)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            cmd.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
    End Sub

    Public Sub updateThirdpartyServicenMobile(ByVal isysID As String)
        Dim SqlTrackID As String
        Dim SqlUpdate As String
        Dim retValue(2) As String

        Try
            SqlTrackID = "select pr.ServiceCode,pr.MobileNo from PayItUserInfo_Restrictions pr LEFT JOIN ThirdParty_knet_trans tk ON pr.CID=tk.TId where TrackId like '" & isysID & "%'"

            Dim dsTrack As DataSet
            dsTrack = New DataSet()
            Dim da1 As New SqlDataAdapter(SqlTrackID, cn)
            da1.Fill(dsTrack, "d")
            If dsTrack.Tables("d").Rows.Count > 0 Then

                SqlUpdate = "update " & DropDownList3.SelectedItem.Text & " set udf2='" & dsTrack.Tables("d").Rows(0).Item(0) & "',udf3='" & dsTrack.Tables("d").Rows(0).Item(1) & "' where trackID='" & isysID & "'"
                Dim cmd As SqlCommand
                cmd = New SqlCommand(SqlUpdate, cn)
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                cmd.ExecuteNonQuery()
            End If

        Catch ex As Exception
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
    End Sub

    Public Sub updatePayitiSYS(ByVal isysID As String)
        Dim SqlUpdate As String
        Try
            SqlUpdate = "update PayitiSYS set ProcessTranDescription='PENDING' where ProcessTranDescription like 'P' and isysID='" & isysID & "'"

            Dim cmd As SqlCommand
            cmd = New SqlCommand(SqlUpdate, cn)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            cmd.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
    End Sub
   
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
                      "alertify.alert('" & s & "')" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub

    Public Sub alertNotify(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'bottom-right');alertify.error('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub

    Public Sub refreshGridView()
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

        Dim sqlLocalTransFail = "select tk.TrackID,isnull(tk.udf2,'Unknown') serviceCode,tk.udf3 Mob, tk.amt Amount," & _
                                           " tk.knetProcess,isnull(tk.udf1,'Unprocessed') Result,tk.tdatetime TransDate,tk.LastUpdateOn ProcessDate  " & _
                                           " from  " & DropDownList3.SelectedItem.Text & " tk " & _
                                           " where convert(datetime,tk.LastUpdateOn, 103) " & _
                                           " between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103)" & _
                                           " and (udf1 not like 'Success%' or udf1 is null) and (KnetProcess like 'CAPTURED%' or KnetProcess like 'Approved%') "

        If DropDownList2.SelectedItem.Text <> "All" And DropDownList2.SelectedItem.Text <> "NULL" Then
            sqlLocalTransFail += " and tk.udf2 like '" & Trim(DropDownList2.SelectedItem.Text) & "%' "
        End If

        If Trim(MobileNOTextBox.Text) <> "" Then
            sqlLocalTransFail += "and (tk.udf3 like '" & Trim(MobileNOTextBox.Text) & "%') "
        End If
        If Trim(AmountTextBox.Text) <> "" Then
            sqlLocalTransFail += " and tk.amt like '" & Trim(AmountTextBox.Text) & "%' "
        End If
        If Trim(TrackIDTextBox.Text) <> "" Then
            sqlLocalTransFail += " and tk.trackid like '" & Trim(TrackIDTextBox.Text) & "%'"
        End If

        Session("sqlLocalTrans") = sqlLocalTransFail
        sqldatabind(sqlLocalTransFail, "first")
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("LocalTransactions.xls", Me.GridView1)
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
    End Sub
   
    Private Function ConfirmKnet(ByVal trackid As String) As Boolean
        TxtHidden.Text = trackid
        Dim _dc As New payitEntities
        Dim result As Boolean = False
        Dim myDate As DateTime = Convert.ToDateTime("2016-04-15 15:00:00")
        Dim check = (From c In _dc.ThirdParty_knet_trans Where c.trackid = trackid And c.tdatetime < myDate Select c).ToList()
        If (Not (check Is Nothing) AndAlso check.Count > 0) Then
            result = True
            Return result
        End If

        Dim rows = (From c In _dc.ConfirmedTpartyTransactions Where c.trackid = trackid And c.OnDate > myDate And c.Status = "CAPTURED" Select c).ToList()
        If (rows IsNot Nothing) Then
            If (rows.Count > 0) Then
                result = True
            End If
        End If
        Return result
    End Function

    Protected Sub chkReprocess_CheckedChanged(sender As Object, e As EventArgs)
        If chkReprocess.Checked Then
            CheckBox2.Checked = False
        End If
    End Sub

    Protected Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs)
        If CheckBox2.Checked Then
            chkReprocess.Checked = False
        End If
    End Sub

    Protected Sub chkCommission_CheckedChanged(sender As Object, e As EventArgs)
        If chkCommission.Checked Then
            Sql = "SELECT [Info4] ServiceCode FROM [payit].[dbo].[PayitServicesConfig]"
            da = New SqlDataAdapter(Sql, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")
            DropDownList2.DataSource = ds.Tables("deta")
            DropDownList2.DataTextField = "ServiceCode"
            DropDownList2.DataValueField = "ServiceCode"
            DropDownList2.DataBind()
        ElseIf chkCommission.Checked = False Then
            Response.Redirect(Request.RawUrl)
        End If
    End Sub

    Protected Sub btnOnay_Click(sender As Object, e As EventArgs)
        Dim result = "ForcedReprocess"
        ReprocessLinkButton_Click(result, e)
    End Sub

    Protected Sub btnRed_Click(sender As Object, e As EventArgs)

    End Sub

#Region "PRIVATE FUNCTIONS"

#Region "Reprocess"

    Public Sub Reprocess(ByVal trackIDC As String)
        If (trackIDC Is Nothing Or trackIDC = "") Then
            trackIDC = TxtHidden.Text.Trim()
        End If

        Dim Service As String = String.Empty
        Dim amount As String = String.Empty
        Dim multipinamnt As String = String.Empty
        Dim quantity As String = String.Empty
        Dim mobileno As String = String.Empty
        Dim t As String = Request.QueryString("type")

        If Nothing Is t OrElse Not t.Equals("fail") Then 'FOR SUCCESS
            Dim serviceType As String = GetService(trackIDC)
            If Not serviceType.EndsWith("-O") Then 'pre/post paid
                Dim result As String = GetProcessDescription(trackIDC)
                If Not Nothing Is result AndAlso result.Length > 0 Then
                    alert(result)
                Else
                    alert("Empty")
                End If
            Else
                Dim result As String = GetProcessedPIN(trackIDC)
                If Not Nothing Is result AndAlso result.Length > 0 Then alert(result)
            End If
            Exit Sub
        Else 'FOR FAIL
            Dim result() As String = GetThirdPartyKnetTransStatus(trackIDC, Service, amount, multipinamnt, mobileno, quantity)
            If result(1).ToLower.Equals("unknown") Then
                updateThirdpartyServicenMobile(trackIDC)
                refreshGridView()
                alert("Your transction has been updated. Please reprocess now")
            ElseIf result(0).ToLower.Equals("voided") Then
                alert("Voided transactions can't be reprocessed..")
                Exit Sub
            Else
                If Service.EndsWith("-O") Then
                    If Not Nothing Is result AndAlso result.Length > 0 Then
                        Dim VoucherResult = ProcessVoucher(trackIDC, Service, amount, multipinamnt, mobileno, quantity, result(0))
                        alert(VoucherResult)
                    End If
                ElseIf Service.ToLower().Equals("alghanim-y") Or Service.ToLower().Equals("alamna-y") Or Service.ToLower().Equals("safat-y") Then
                    If (validateTrackID(trackIDC) = True) Then
                        alert("Transaction is already processed")
                        Exit Sub
                    Else
                        Dim MerchantResult = MerchantService(trackIDC, Service)
                        alert(MerchantResult)
                    End If
                ElseIf (Service.EndsWith("-Y") Or Service.EndsWith("-Z") Or Service.Equals("Porsche")) Then
                    If validateTrackID(trackIDC) = True Then
                        alert("Transaction with this trackID is already processed..")
                        Exit Sub
                    Else
                        Dim PaymentResult = ProcessPaymentServices(trackIDC)
                        alert(PaymentResult)
                    End If
                  
                ElseIf Service.ToLower().EndsWith("-w") Then
                    If (validateTrackID(trackIDC) = True) Then
                        alert("Transaction is already processed")
                        Exit Sub
                    Else
                        Dim WalletResult = WalletReprocess(trackIDC)
                        alert(WalletResult)
                    End If

                Else
                    Try
                        If validateTrackID(trackIDC) = True Then 'Already success
                            alert("Transaction with this TrackID is already processed")
                            Exit Sub
                        Else 'Failed
                            Dim ServiceProcessResult = ProcessService(trackIDC, Service)
                            alert(ServiceProcessResult)
                        End If
                    Catch ex As Exception
                        alert("Unable to process your request. Please try again later")
                    End Try
                End If
            End If
        End If
    End Sub

#End Region

    Private Function UpdateTPServiceTransactions(trackid As String, statusDesc As String) As String
        If (Not (String.IsNullOrEmpty(trackid))) Then
            Using con As New SqlConnection(constring)
                Using cmd As New SqlCommand("UPDATE [payit].[dbo].[ThirdpartyServiceTransactions] SET [ServiceResult] = '" & statusDesc & "' WHERE TrackId = " & trackid & ")", con)
                    Using sda As New SqlDataAdapter()
                        If (con.State = ConnectionState.Closed) Then
                            con.Open()
                        End If
                        Dim insert As Integer = cmd.ExecuteNonQuery()
                        If (con.State = ConnectionState.Open) Then
                            con.Close()
                        End If
                    End Using
                End Using
            End Using
        End If
        Return "success"
    End Function

#Region "PIN Reprocess"
    Private Function ProcessVoucher(trackIDC As String, Service As String, amount As String, multipinamnt As String, mobileno As String, quantity As String, result As String) As String
        Dim PinResult As String = String.Empty
        If result.ToLower.Equals("failed") OrElse result.ToLower.Equals("unprocessed") OrElse result.ToLower.Equals("fail") OrElse validateTrackIDVoucher(trackIDC) = False Then
            Dim PIN As String = ProcessFailedPIN(trackIDC, Service, multipinamnt, mobileno, quantity)
            Dim PINalert As String = trackIDC
            Dim SMSresp As String = String.Empty
            If (PIN.Contains("NO PIN") = False) Then
                If (Not mobileno.StartsWith("965")) Then
                    mobileno = "965" + mobileno
                End If
                'Dim reference As String = SMS.GetUniqueKey(10)
                PIN = PIN + "Amount: " + amount + " KD" + "Ref:" + trackIDC + " - Payit"
                PINalert = PIN
                SMSresp = SMS.sendSMS(PIN, mobileno, "FCCKW2", trackIDC)
            End If
            refreshGridView()
            If (PIN.ToLower().Contains("no pin")) Then
                PinResult = "PIN: " & PIN
            ElseIf (SMSresp.ToLower().Equals("success")) Then
                PinResult = "SMS Sent Sucessfully to Customer"
            ElseIf (SMSresp.ToLower().Equals("failed")) Then
                PinResult = "SMS sending failed! Try Again"
            End If
        End If

        Return PinResult
    End Function

    Private Function ProcessFailedPIN(ByVal isysid As String, ByVal service As String, ByVal multipinamnt As String, ByVal mobileno As String, ByVal quantity As String) As String
        If cn.State = ConnectionState.Closed Then cn.Open()

        'If (service.ToLower.Equals("zain-o") AndAlso Convert.ToInt32(multipinamnt) > 1 AndAlso Convert.ToInt32(quantity) = 1) Then
        '    Dim api As New IsysPaymentAPIZain.Zain_Kuwait_ZDP
        '    Dim res = api.GetTelcoPIN(service, multipinamnt, mobileno, isysid, mobileno, 1, multipinamnt, transactionId)
        'End If

        Dim cmd As New SqlCommand("ProcessSuccessPIN", cn)
        cmd.CommandType = System.Data.CommandType.StoredProcedure

        Dim serParm As New SqlParameter("@service", SqlDbType.VarChar)
        serParm.Size = 50
        serParm.Direction = ParameterDirection.Input
        serParm.Value = service
        cmd.Parameters.Add(serParm)

        Dim amtParm As New SqlParameter("@AMT", SqlDbType.Float)
        amtParm.Direction = ParameterDirection.Input
        amtParm.Value = multipinamnt
        cmd.Parameters.Add(amtParm)

        Dim trackidParm As New SqlParameter("@trackid", SqlDbType.VarChar)
        trackidParm.Size = 50
        trackidParm.Direction = ParameterDirection.Input
        trackidParm.Value = isysid
        cmd.Parameters.Add(trackidParm)

        Dim ooid As New SqlParameter("@OOID", SqlDbType.VarChar)
        ooid.Size = 50
        ooid.Direction = ParameterDirection.Output
        cmd.Parameters.Add(ooid)

        Dim pinParam As New SqlParameter("@PIN", SqlDbType.VarChar)
        pinParam.Size = 50
        pinParam.Direction = ParameterDirection.Output
        cmd.Parameters.Add(pinParam)

        Dim serial As New SqlParameter("@Serialout", SqlDbType.VarChar)
        serial.Direction = ParameterDirection.Output
        serial.Size = 50
        cmd.Parameters.Add(serial)
        Dim r As SqlDataReader = cmd.ExecuteReader()

        r.Close()

        Dim fetchedPIN As String
        Dim PINText As String = ""
        fetchedPIN = cmd.Parameters("@PIN").Value.ToString()
        If (Not fetchedPIN.ToLower().Contains("no pin") And fetchedPIN <> "") Then
            Dim _PIN
            If fetchedPIN.Contains(",") And fetchedPIN <> "" Then
                _PIN = fetchedPIN.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)
            Else
                _PIN = fetchedPIN
            End If
            Dim i As Integer
            For Each row As String In _PIN
                PINText = (PINText & Convert.ToString("PIN ")) + (i + 1).ToString() + ": " + _PIN(i) + vbLf
                i += 1
            Next
        End If

        cmd = Nothing
        Dim PINresponse As String = String.Empty
        If (fetchedPIN.ToLower() <> "no pin" And fetchedPIN <> "") Then
            PINresponse = PINText
        ElseIf fetchedPIN.ToLower() = "no pin" Then
            PINresponse = fetchedPIN
        End If
        Return PINresponse

    End Function

#End Region

#Region "Payment Reprocess"
    Private Function ProcessPaymentServices(ByVal trackIDC As String) As String
        Dim commission As Double = 0.0
        Dim ptype As String = ""
        Dim com As String = ""
        Dim result As String = String.Empty

        updateThirdpartyStatus(trackIDC, "")

        Dim transactionInfo() As String = getTrackIDInfo(trackIDC)
        Dim amt As Double = transactionInfo(1)
        Dim transdate As DateTime = transactionInfo(4)
        Dim processdate As DateTime = transactionInfo(5)
        Dim payitisysinfo As String = getpayitisysInfo(trackIDC)

        If payitisysinfo.Equals("NODATA") Then
            Using con As New SqlConnection(constring)
                Using cmd As New SqlCommand("Insert into PayitiSYS ([IsysID],[MobileNo],[Service],[Amount],[TranDate],[ProcessTranDate],[ProcessTranDescription],[PaymentAPI])values ('" & trackIDC & "','" & transactionInfo(2) & "', '" & transactionInfo(0) & "','" & amt & "','" & transdate & "','" & processdate & "','SuccessRI','iSYS')", con)
                    Using sda As New SqlDataAdapter()
                        If (con.State = ConnectionState.Closed) Then
                            con.Open()
                        End If
                        Dim insert As Integer = cmd.ExecuteNonQuery()
                        If (con.State = ConnectionState.Open) Then
                            con.Close()
                        End If
                    End Using
                End Using
            End Using

            Dim rslt As String = processpayment(trackIDC, transactionInfo(0), transactionInfo(2), amt, commission, ptype, com)
            updateThirdpartyStatus(trackIDC, "SuccessRI")
            refreshGridView()

            If (rslt.Equals("Processed")) Then
                result = "Transaction Processed Successfully!"
            Else
                result = "An error occured while processing your request. Please try agan later"
            End If
        Else
            result = "Transaction Already Processed "
        End If

        Return result
    End Function

    Private Function processpayment(ByVal trackid As String, ByVal servicecode As String, ByVal referenceid As String, ByVal amount As Double, ByVal commission As Double, ByVal ptype As String, ByVal com As String) As String
        If cn.State = ConnectionState.Closed Then cn.Open()

        Dim cmd As New SqlCommand("USP_updatePaymentTables", cn)
        cmd.CommandType = System.Data.CommandType.StoredProcedure

        Dim trackidParm As New SqlParameter("@TrackID", SqlDbType.VarChar)
        trackidParm.Size = 50
        trackidParm.Direction = ParameterDirection.Input
        trackidParm.Value = trackid
        cmd.Parameters.Add(trackidParm)

        Dim serParm As New SqlParameter("@serviceCode", SqlDbType.VarChar)
        serParm.Size = 50
        serParm.Direction = ParameterDirection.Input
        serParm.Value = servicecode
        cmd.Parameters.Add(serParm)

        Dim refParm As New SqlParameter("@referenceID", SqlDbType.VarChar)
        refParm.Size = 50
        refParm.Direction = ParameterDirection.Input
        refParm.Value = referenceid
        cmd.Parameters.Add(refParm)

        Dim amtParm As New SqlParameter("@amt", SqlDbType.Float)
        amtParm.Direction = ParameterDirection.Input
        amtParm.Value = amount
        cmd.Parameters.Add(amtParm)

        Dim commissionParm As New SqlParameter("@Commission", SqlDbType.Float)
        commissionParm.Direction = ParameterDirection.Input
        commissionParm.Value = commission
        cmd.Parameters.Add(commissionParm)

        Dim ptypeParm As New SqlParameter("@ptype", SqlDbType.VarChar)
        ptypeParm.Size = 50
        ptypeParm.Direction = ParameterDirection.Input
        ptypeParm.Value = ptype
        cmd.Parameters.Add(ptypeParm)

        Dim comParm As New SqlParameter("@com", SqlDbType.VarChar)
        comParm.Size = 250
        comParm.Direction = ParameterDirection.Input
        comParm.Value = com
        cmd.Parameters.Add(comParm)

        Dim r As SqlDataReader = cmd.ExecuteReader()

        r.Close()
        Return "Processed"
    End Function

#End Region

#Region "Service Reprocess"

    Private Function ProcessService(ByVal trackIDC As String, ByVal Service As String) As String
        Dim ServiceResult As String = String.Empty
        updateThirdpartyStatus(trackIDC, "")
        Dim transInfo() As String = getTrackIDInfo(trackIDC)

        If Not transInfo(0).Equals("NODATA") Then
            Dim VWZObj As New VWZWebReference3.Service
            Dim transResult = "ERROR"
            If Service.ToUpper().EndsWith("-XR") Then
                transResult = VWZObj.ProcessTransactionSupport(transInfo(0), transInfo(3), transInfo(1), transInfo(2), trackIDC, "XSmart", "XSmart")
            Else
                transResult = VWZObj.ProcessTransactionSupport(transInfo(0), -1, transInfo(3), transInfo(2), trackIDC, "XSmart", "XSmart")
            End If
            If Not transResult.StartsWith("ERROR") Then
                updateThirdpartyStatus(trackIDC, "SUCCESSRI")
                refreshGridView()
                ServiceResult = "Your transction has been successfully processed"
            Else
                ServiceResult = "An error Occured while processing your request. Please try agan later"
            End If
        Else
            ServiceResult = "No data found for the TrackID. Please provide a valid trackID."
        End If
        Return ServiceResult
    End Function

#End Region

#Region "Wallet Reprocess"

    Private Function WalletReprocess(ByVal trackId As String) As String
        Dim result As String

        updateThirdpartyStatus(trackId, "")

        Dim transactionInfo() As String = getTrackIDInfo(trackId)
        Dim amt As Double = transactionInfo(1)
        Dim usermobile As String = transactionInfo(2)
        Dim transdate As DateTime = transactionInfo(4)
        Dim processdate As DateTime = transactionInfo(5)
        Dim wb As New WalletsEntities()
        Dim WTran As New WalletTransaction


        Dim walletTran = wb.WalletTransactions.Include("WalletAccount").Where(Function(x) x.TrackID = trackId.Trim()).FirstOrDefault()
        'Dim walletTran = (From d In wb.WalletTransactions Where d.TrackID = trackId.Trim() Select d).FirstOrDefault()
        If (walletTran Is Nothing) Then
            result = "No Wallet Account Found For This Mobile No. Please contact IT"
            Return result
        Else
            walletTran.WalletAccount.Balance += amt
            wb.SaveChanges()
            updateThirdpartyStatus(trackId, "SuccessRI")
            refreshGridView()
            result = "Your transction has been successfully processed"
        End If
       
        Return result
    End Function
#End Region

#Region "Alghanim Reprocess"

    Private Function MerchantService(ByVal trackIDC As String, ByVal Service As String) As String
        Dim ServiceResult As String = String.Empty
        updateThirdpartyStatus(trackIDC, "")
        Dim transInfo() As String = getTrackIDInfo(trackIDC)

        If Not transInfo(0).Equals("NODATA") Then
            Dim proxy As New VWZForAlghanim.ServiceSoapClient
            Dim transact = proxy.ProcessAlghanimTransactionSupport(transInfo(2), transInfo(1), 0, trackIDC, Service, "XSmart", "XSmart")
            If (transact IsNot Nothing) Then
                refreshGridView()
                ServiceResult = transact
            End If
        Else
            ServiceResult = "No data found for the TrackID. Please provide a valid trackID."
        End If
        Return ServiceResult

    End Function

#End Region

#Region "Zakat Reprocess"
    Private Function ProcessZakat(ByVal trackIDC As String) As String
        Dim commission As Double = 0.0
        Dim ptype As String = ""
        Dim com As String = ""
        Dim result As String = String.Empty

        updateThirdpartyStatus(trackIDC, "")

        Dim transactionInfo() As String = getTrackIDInfo(trackIDC)
        Dim amt As Double = transactionInfo(1)
        Dim transdate As DateTime = transactionInfo(4)
        Dim processdate As DateTime = transactionInfo(5)
        Dim payitisysinfo As String = getpayitisysInfo(trackIDC)

        If payitisysinfo.Equals("NODATA") Then
            Using con As New SqlConnection(constring)
                Using cmd As New SqlCommand("Insert into PayitiSYS ([IsysID],[MobileNo],[Service],[Amount],[TranDate],[ProcessTranDate],[ProcessTranDescription],[PaymentAPI])values ('" & trackIDC & "','" & transactionInfo(2) & "', '" & transactionInfo(0) & "','" & amt & "','" & transdate & "','" & processdate & "','SuccessRI','iSYS')", con)
                    Using sda As New SqlDataAdapter()
                        If (con.State = ConnectionState.Closed) Then
                            con.Open()
                        End If
                        Dim insert As Integer = cmd.ExecuteNonQuery()
                        If (con.State = ConnectionState.Open) Then
                            con.Close()
                        End If
                    End Using
                End Using
            End Using

            Dim rslt As String = processpayment(trackIDC, transactionInfo(0), transactionInfo(2), amt, commission, ptype, com)
            updateThirdpartyStatus(trackIDC, "SuccessRI")
            refreshGridView()

            If (rslt.Equals("Processed")) Then
                result = "Transaction Processed Successfully!"
            Else
                result = "An error occured while processing your request. Please try agan later"
            End If
        Else
            result = "Transaction Already Processed "
        End If

        Return result
    End Function

#End Region

#Region "International Recharge"

    Private Function ProcessInternational(ByVal trackId As String, ByVal service As String) As String
        Dim ServiceResult As String = String.Empty
        updateThirdpartyStatus(trackId, "")
        Dim transInfo() As String = getTrackIDInfo(trackId)

        If Not transInfo(0).Equals("NODATA") Then

        End If

        Return ServiceResult
    End Function

    Private Function GetInternationalCountry(ByVal mobileNo As String) As String
        Dim country As String = String.Empty

        If (mobileNo.StartsWith("91")) Then
            country = "India"
        ElseIf mobileNo.StartsWith("92") Then
            country = "Pakistan"
        ElseIf mobileNo.StartsWith("880") Then
            country = "Bangladesh"
        ElseIf mobileNo.StartsWith("86") Then
            country = "China"
        ElseIf mobileNo.StartsWith("20") Then
            country = "Egypt"
        ElseIf mobileNo.StartsWith("62") Then
            country = "Indonesia"
        ElseIf mobileNo.StartsWith("964") Then
            country = "Iraq"
        ElseIf mobileNo.StartsWith("962") Then
            country = "Jordan"
        ElseIf mobileNo.StartsWith("977") Then
            country = "Nepal"
        ElseIf mobileNo.StartsWith("970") Then
            country = "Palestine"
        ElseIf mobileNo.StartsWith("63") Then
            country = "Philippines"
        ElseIf mobileNo.StartsWith("94") Then
            country = "Sri Lanka"
        ElseIf mobileNo.StartsWith("963") Then
            country = "Syria"
        ElseIf mobileNo.StartsWith("90") Then
            country = "Turkey"
        ElseIf mobileNo.StartsWith("967") Then
            country = "Yemen"
        ElseIf mobileNo.StartsWith("966") Then
            country = "Saudi Arabia"
        ElseIf mobileNo.StartsWith("216") Then
            country = "Tunisia"
        ElseIf mobileNo.StartsWith("212") Then
            country = "Morocco"
        ElseIf mobileNo.StartsWith("251") Then
            country = "Ethiopia"
        ElseIf mobileNo.StartsWith("40") Then
            country = "Romania"
        ElseIf mobileNo.StartsWith("971") Then
            country = "United Arab Emirates"
        ElseIf mobileNo.StartsWith("973") Then
            country = "Bahrain"
        ElseIf mobileNo.StartsWith("374") Then
            country = "Armenia"
        Else
            country = ""
        End If

        Return country
    End Function

#End Region

#End Region

End Class


