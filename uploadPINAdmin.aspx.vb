Imports System.Data
Imports System.Data.SqlClient
Partial Class SummaryReport
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=uploadPINAdmin.aspx")
        End If

        If Not Page.IsPostBack Then

            Dim con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand("select distinct(Service),Service from dummypins where FLAG=0", con)
                con.Open()
                DropDownList1.DataSource = cmd.ExecuteReader()
                DropDownList1.DataBind()
                con.Close()
            End Using
            Using cmd As New SqlCommand("select distinct(Amount),Amount2  from dummypins where FLAG=0 and Service like '" + DropDownList1.SelectedValue & "'", con)
                con.Open()
                DropDownList2.DataSource = cmd.ExecuteReader()
                DropDownList2.DataBind()
                con.Close()
            End Using

            If (DropDownList1.SelectedIndex <> -1) Then
                Dim db As New Data.payitEntities
                Dim rows = db.DummyPINS.FirstOrDefault(Function(x) x.Service.ToUpper().Equals(DropDownList1.SelectedItem.Text.ToUpper()) AndAlso x.Amount2.ToUpper().Equals(DropDownList2.SelectedItem.Text.ToUpper()))
                If (rows IsNot Nothing) Then
                    lblDescription.Text = If(String.IsNullOrEmpty(rows.InvoiceNumber), "", rows.InvoiceNumber)
                Else
                    Label2.Visible = False
                    lblDescription.Visible = False
                End If
            End If
          
        End If
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        Dim con As New SqlConnection(strConnString)
        Using cmd As New SqlCommand("select distinct(Amount),Amount2  from dummypins where FLAG=0 and Service like '" + DropDownList1.SelectedValue & "'", con)
            con.Open()
            DropDownList2.DataSource = cmd.ExecuteReader()
            DropDownList2.DataBind()
            con.Close()
        End Using

        If (DropDownList1.SelectedIndex <> -1 AndAlso DropDownList2.SelectedIndex <> -1) Then
            Dim db As New Data.payitEntities
            Dim rows = db.DummyPINS.FirstOrDefault(Function(x) x.Service.ToUpper().Equals(DropDownList1.SelectedItem.Text.ToUpper()) AndAlso x.Amount2.ToUpper().Equals(DropDownList2.SelectedItem.Text.ToUpper()))
            If (rows IsNot Nothing) Then
                lblDescription.Text = If(String.IsNullOrEmpty(rows.InvoiceNumber), "", rows.InvoiceNumber)
            Else
                Label2.Visible = False
                lblDescription.Visible = False
            End If
        End If

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Dim con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand("OPEN SYMMETRIC KEY [key_DataShare] DECRYPTION BY CERTIFICATE cert_keyProtection SELECT ID, [InvoiceNumber], [Service], PIN as Vendor, Amount,Amount2 as Denom, Serial, CONVERT(varchar(100), convert(VARCHAR,decryptbykey(EncryptedPIN)))AS DecryptedPIN,TranDate as Uploaded  FROM DummyPins where [service] like '" & DropDownList1.SelectedItem.ToString & "' and FLAG=0 and Amount2 like '" & DropDownList2.SelectedItem.ToString & "'CLOSE SYMMETRIC KEY [key_DataShare]; ", con)
                con.Open()
                GridView1.DataSource = cmd.ExecuteReader()
                GridView1.DataBind()
                con.Close()
            End Using
        Catch ex As Exception
            'Label17.Text = "No pins Available " & ex.ToString
            'Label17.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim CS As String = "data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096"
        Dim cn As SqlConnection = New SqlConnection(strConnString)

        Try
            Dim invoiceNumber As String = String.Empty
            If (Not (String.IsNullOrEmpty(lblDescription.Text))) Then
                invoiceNumber = lblDescription.Text
            End If

            If (DropDownList1.SelectedIndex <> -1 Or DropDownList2.SelectedIndex <> -1) Then
                Dim sql1 As String = "SELECT COUNT(dp.ID) as Total from [payit].[dbo].[PINS] p INNER JOIN [payit].[dbo].[DummyPINS] dp on p.Serial = dp.Serial " & _
                                     "WHERE p.Service like @ServiceName and p.Amount2 like @Amount and dp.FLAG = 0 and p.Status = 0"
                Dim cmd1 As SqlCommand = New SqlCommand(sql1, cn)
                cmd1.Connection = cn
                If (cn.State = ConnectionState.Closed) Then
                    cn.Open()
                End If
                cmd1.Parameters.AddWithValue("@ServiceName", DropDownList1.SelectedItem.Text)
                cmd1.Parameters.AddWithValue("@Amount", DropDownList2.SelectedItem.Text)
                Using reader As SqlDataReader = cmd1.ExecuteReader()
                    If reader.HasRows Then
                        Dim duplicount As String = 0
                        While (reader.Read)
                            duplicount = reader("Total")
                        End While
                        If duplicount > 0 Then
                            alert(duplicount & " Duplicate PINS Found! Please check and re-upload", "error")
                        Else
                            ' PINS does not exist, add them
                            Using cmd As New SqlCommand("INSERT INTO PINS (SERIAL, PIN, ENCRYPTEDPIN,SERVICE,Amount,Status,Amount2)  SELECT Serial,PIN, EncryptedPIN,service,Amount,status,Amount2 FROM dummypins where flag=0 and service like '" + DropDownList1.SelectedItem.Text & "' and amount2 like '" + DropDownList2.SelectedItem.Text & "'", cn)
                                If (cn.State = ConnectionState.Closed) Then
                                    cn.Open()
                                End If
                                cmd.ExecuteReader()
                                If (cn.State = ConnectionState.Open) Then
                                    cn.Close()
                                End If
                            End Using

                            Dim strSql As String = ("SELECT COUNT(id) FROM [payit].[dbo].[DummyPINS] WHERE service like '" + DropDownList1.SelectedItem.ToString & "' and amount2 like '") + DropDownList2.SelectedItem.ToString & "' and FLAG=0 "
                            Dim count As Integer
                            Using command As New SqlCommand(strSql, cn)
                                If (cn.State = ConnectionState.Closed) Then
                                    cn.Open()
                                End If
                                count = Convert.ToInt32(command.ExecuteScalar())
                                If (cn.State = ConnectionState.Open) Then
                                    cn.Close()
                                End If
                            End Using

                            Using cmd As New SqlCommand("UPDATE [payit].[dbo].[DummyPINS] SET FLAG=1 WHERE Service like '" + DropDownList1.SelectedItem.ToString & "' and amount2 like '" + DropDownList2.SelectedItem.ToString & "' and Flag=0;" & _
                                                        " UPDATE [payit].[dbo].[UploadPINSHistory] SET IsApproved = 1 WHERE Service = '" & DropDownList1.SelectedItem.Text.Trim() & "' AND Info1 = '" & DropDownList2.SelectedItem.Text.Trim() & "' AND InvoiceNumber = '" & invoiceNumber & "' ", cn)
                                If (cn.State = ConnectionState.Closed) Then
                                    cn.Open()
                                End If
                                cmd.ExecuteNonQuery()
                                If (cn.State = ConnectionState.Open) Then
                                    cn.Close()
                                End If
                            End Using
                            alert(count & " pins uploaded", "success")
                        End If
                    End If
                End Using
                If (cn.State = ConnectionState.Open) Then
                    cn.Close()
                End If
            End If
        Catch exc As Exception

        End Try
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs)
        Dim cn As SqlConnection = New SqlConnection(strConnString)

        Try
            If (DropDownList1.SelectedIndex <> -1 Or DropDownList2.SelectedIndex <> -1) Then
                Dim count As Integer
                Using cmd As New SqlCommand("DELETE FROM [payit].[dbo].[DummyPINS] WHERE Service like '" + DropDownList1.SelectedItem.ToString & "' and amount2 like '" + DropDownList2.SelectedItem.ToString & "' and Flag=0", cn)
                    If (cn.State = ConnectionState.Closed) Then
                        cn.Open()
                    End If
                    count = cmd.ExecuteNonQuery()
                    If (cn.State = ConnectionState.Open) Then
                        cn.Close()
                    End If
                End Using
                alert(count & " PINS deleted", "notify")
            End If
        Catch exc As Exception

        End Try
    End Sub

    Public Sub alert(ByVal s As String, action As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify." & action & "('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
End Class
