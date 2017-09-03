Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
'Imports System.Web.Mail

Imports System
Partial Class IntlTopUpComplaints
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim da, da1 As SqlDataAdapter
    Dim ds, ds1 As DataSet
    Dim mid As String
    Dim cmd As SqlCommand
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Then
            Label9.Text = "ERROR: Invalid TrackID/MobileNo"
            Exit Sub
        Else


            Dim sql As String = ""
            'sql = "SELECT myid,company,amt,pamt,ptype,trackid,payid,knetresult,destmob,topupamt,topupresult,tdatetime from topup_trans where convert(datetime, tdatetime,103) between convert(datetime, '" & TextBox1.Text & "',103) and  convert(datetime, '" & TextBox2.Text & "',103) and rtrim(ltrim(company)) like '" & Trim(DropDownList1.Text) & "'order by myid desc"
            If DropDownList1.SelectedItem.Text = "TrackID" Then
                sql = "SELECT myid,company,amt,pamt,ptype,trackid,knetprocess,destmob,topupamt,topupresult,tdatetime from topup_trans where trackid=" & TextBox1.Text & " order by myid desc"
            ElseIf DropDownList1.SelectedItem.Text = "PayId" Then
                sql = "SELECT myid,company,amt,pamt,ptype,trackid,knetprocess,destmob,topupamt,topupresult,tdatetime from topup_trans where payid=" & TextBox1.Text & " order by myid desc"
            ElseIf DropDownList1.SelectedItem.Text = "DestNumber" Then
                sql = "SELECT myid,company,amt,pamt,ptype,trackid,knetprocess,destmob,topupamt,topupresult,tdatetime from topup_trans where destmob=" & TextBox1.Text & " order by myid desc"
            ElseIf DropDownList1.SelectedItem.Text = "TopupAmt" Then
                sql = "SELECT myid,company,amt,pamt,ptype,trackid,knetprocess,destmob,topupamt,topupresult,tdatetime from topup_trans where topupamt=" & TextBox1.Text & " order by myid desc"
            ElseIf DropDownList1.SelectedItem.Text = "Amount" Then
                sql = "SELECT myid,company,amt,pamt,ptype,trackid,knetprocess,destmob,topupamt,topupresult,tdatetime from topup_trans where amt=" & TextBox1.Text & " order by myid desc"
            ElseIf DropDownList1.SelectedItem.Text = "Date" Then
                sql = "SELECT myid,company,amt,pamt,ptype,trackid,knetprocess,destmob,topupamt,topupresult,tdatetime from topup_trans where tdatetime=" & TextBox1.Text & " order by myid desc"
            End If
            sqldatabind(sql)
            Session("sql") = sql
        End If
    End Sub
    Public Sub sqldatabind(ByVal s As String)
        Try

            da = New SqlDataAdapter(s, cn)
            ds = New DataSet()
            da.Fill(ds, "pubnewstab")
            GridView1.DataSource = ds.Tables("pubnewstab")
            GridView1.DataBind()
        Catch ex As Exception
            Label9.Text = "ERROR: Invalid TrackID/MobileNo"
        End Try
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        Dim s As String
        s = Session("sql")
        sqldatabind(s)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cn = New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")

    End Sub

    'Protected Sub lnkCustDetails_Click(ByVal sender As Object, ByVal e As EventArgs)


    'End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand


        mid = e.CommandArgument

        If e.CommandName = "SendMail" Then

            Dim selectFromDBres As String
            selectFromDBres = selectFromDB(mid)
            If selectFromDBres = "False" Then
                Dim i As Integer
                Dim DestMobileNumber, TrackID, TransactionDate, TransferToResponse As String
                For i = 0 To GridView1.Rows.Count - 1
                    If GridView1.Rows(i).Cells(1).Text = mid Then
                        'SELECT myid,company,amt,pamt,ptype,trackid,payid,knetprocess,destmob,
                        'topupamt,topupresult,tdatetime from topup_trans where amt=" & TextBox1.Text & " order by myid desc
                        DestMobileNumber = GridView1.Rows(i).Cells(7).Text
                        TrackID = GridView1.Rows(i).Cells(5).Text
                        TransactionDate = GridView1.Rows(i).Cells(10).Text
                        TransferToResponse = getTransRawResponse(mid)
                        If insertIntoDB(mid, DestMobileNumber, TrackID, TransactionDate, TransferToResponse) = "OK" Then
                            If sendMail(TransferToResponse, TrackID) = "OK" Then
                                Label9.Text = "Mail Sent Successfully"
                                Exit Sub
                            Else
                                Label9.Text = "ERROR: Mail Sending Failed."
                                Exit Sub
                            End If

                        Else
                            Label9.Text = "ERROR: Unable To process your request. Please check all the fields."
                        End If
                    End If
                Next
            ElseIf selectFromDBres = "Submitted" Then
                Label9.Text = "ERROR: Complaint Already Submitted to TransferTo Company."
                Exit Sub
            ElseIf selectFromDBres = "" Then
                Label9.Text = "ERROR: Complaint Already Processed"
                Exit Sub
            End If
        ElseIf e.CommandName = "UpdateDB" Then

        End If

    End Sub
    'Public Function sendMail(ByVal msgBody As String, ByVal TrakID As String) As String
    '    Dim emailTo, emailFrom, emailSubject, messageBody, ServerName, result As String
    '    Dim mesg As New MailMessage

    '    emailTo = "suma.asam@gmail.com"
    '    emailFrom = "j.suma@isys.mobi"
    '    emailSubject = "Pay-it Complaint: " & TrakID
    '    messageBody = "Dear Support," & ControlChars.NewLine & "Below is the response which we got when we tried process as transaction. " & _
    '                   "Can you please investagate why the transaction failed. And please take appropriate actions to make the transaction Successfull." & _
    '                   "The Response :" & _
    '                   ControlChars.NewLine & msgBody

    '    ServerName = "smtpout.secureserver.net"


    '    Try

    '        Dim smtpObj As New SmtpClient(ServerName, 25)

    '        Dim mycache As System.Net.NetworkCredential = New System.Net.NetworkCredential("j.suma@isys.mobi", "12345")
    '        smtpObj.Credentials = mycache


    '        smtpObj.Send(emailFrom, emailTo, emailSubject, messageBody)
    '        smtpObj = Nothing
    '        result = "OK"
    '    Catch ex As Exception
    '        result = "ERROR: " & ex.Message
    '    End Try
    '    Return result
    'End Function
    Public Function UpdateStatus(ByVal ID As String, ByVal ack As String) As String
        Dim sql As String


        sql = "Update [TopUpComplaints] set status='Processed',TransferToComplaintResponse='" & ack & "'" & _
              "where ID=" & ID & ""
        Try
            cmd = New SqlCommand(sql, cn)
            cn.Open()
            cmd.ExecuteNonQuery()
            cn.Close()
            Return "OK"
        Catch ex As Exception
            Return "ERROR: " & ex.Message
        End Try

    End Function
    Public Function sendMail(ByVal msgBody As String, ByVal TrakID As String) As String
        Dim emailTo, emailFrom, emailSubject, messageBody, ServerName, result As String
        Dim mesg As New MailMessage

        emailTo = "" 'Pending Transfer TO Email ID
        emailFrom = "h.alakkad@isys.mobi"
        emailSubject = "Pay-it Complaint: " & TrakID
        messageBody = "Dear Support," & ControlChars.NewLine & ControlChars.NewLine & "Please Confirm below Transaction and take appropriate actions to make the transaction Successfull." & _
                       ControlChars.NewLine & ControlChars.NewLine & "Transaction :" & _
                       ControlChars.NewLine & msgBody & ControlChars.NewLine & ControlChars.NewLine & "Thank you," & _
                       ControlChars.NewLine & "Isys Support"

        ServerName = "smtpout.secureserver.net"
        Dim s As New MailMessage
        s.CC.Add("support@isys.mobi")
        s.Body = messageBody
        s.To.Add(emailTo)
        s.From = New MailAddress(emailFrom)
        s.Subject = emailSubject
        s.IsBodyHtml = False
        s.Priority = MailPriority.High


        Try

            Dim smtpObj As New SmtpClient(ServerName, 25)

            Dim mycache As System.Net.NetworkCredential = New System.Net.NetworkCredential("h.alakkad@isys.mobi", "12345")
            smtpObj.Credentials = mycache


            ' smtpObj.Send(emailFrom, emailTo, emailSubject, messageBody)
            smtpObj.Send(s)

            smtpObj = Nothing
            result = "OK"
        Catch ex As Exception
            result = "ERROR: " & ex.Message
        End Try
        Return result
    End Function
    Public Function insertIntoDB(ByVal TopUpTransID As String, ByVal DestMobileNumber As String, _
                                 ByVal TrackID As String, ByVal TransactionDate As String, _
                                 ByVal TransferToResponse As String) As String
        Dim sql As String


        sql = "INSERT INTO [TopUpComplaints] " & _
           "([TopUpTransID],[DestMobileNumber] ,[TrackID],[TransactionDate],[ComplaintDate]," & _
           " [TransferToResponse],[Status]) " & _
           " VALUES(" & _
           " " & TopUpTransID & "," & DestMobileNumber & "," & TrackID & ",'" & TransactionDate & "','" & Date.Now & "'," & _
           "'" & TransferToResponse & "','Submitted'" & _
           ")"

        Try
            cmd = New SqlCommand(sql, cn)
            cn.Open()
            cmd.ExecuteNonQuery()
            cn.Close()
            Return "OK"
        Catch ex As Exception
            Return "ERROR: " & ex.Message
        End Try

    End Function


    Public Function selectFromDB(ByVal myid As String) As String
        Dim sql As String
        sql = "SELECT status FROM [TopUpComplaints] where id=" & myid & ""
        da1 = New SqlDataAdapter(sql, cn)
        ds1 = New DataSet()
        da1.Fill(ds1, "Details")

        If ds1.Tables("Details").Rows.Count > 0 Then
            Return ds1.Tables("Details").Rows(0).Item(0)
        Else
            Return "False"
        End If

    End Function
    Public Function getTransRawResponse(ByVal TransID As String) As String
        Dim sqlget As String
        sqlget = "SELECT topuprawresponse FROM topup_trans where myid=" & TransID & ""
        da1 = New SqlDataAdapter(sqlget, cn)
        ds1 = New DataSet()
        da1.Fill(ds1, "sqlget")

        If ds1.Tables("sqlget").Rows.Count > 0 Then
            Return ds1.Tables("sqlget").Rows(0).Item(0)
        Else
            Return "False"
        End If
    End Function
End Class
