Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.IO
Imports System.Data

Partial Class UploadPINS
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sql, sql2, strSQL As String
    Dim dispAmnt, info1 As String
    Dim sqlLocalTrans As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString

    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=UploadPINS.aspx")
        End If

        If Not Page.IsPostBack Then
            Try
                Dim CS As String = "data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096"
                Dim con As New SqlConnection(CS)

                Using cmd As New SqlCommand("SELECT ServiceName, ServiceID FROM [payit].[dbo].[Services] WHERE ServiceName LIKE '%-O' ORDER BY ServiceName", con)
                    con.Open()
                    DropDownList1.DataSource = cmd.ExecuteReader()
                    DropDownList1.DataBind()
                    con.Close()
                End Using

                Dim sql2 = "SELECT [Vendor] FROM [payit].[dbo].[PINVendors] WHERE Status = 1 ORDER BY Vendor"
                da = New SqlDataAdapter(sql2, con)
                ds = New DataSet()
                da.Fill(ds, "data")
                ddlVendor.DataSource = ds.Tables("data")
                ddlVendor.DataTextField = "Vendor"
                ddlVendor.DataValueField = "Vendor"
                ddlVendor.DataBind()
            Catch ex As Exception
                MsgLbl.Text = ex.ToString
            End Try

        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e__1 As EventArgs) Handles Button1.Click
        Dim vendor As String = String.Empty
        Dim cn As SqlConnection = New SqlConnection(strConnString)
        If ddlVendor.SelectedItem.Text = "Select" Then
            alertme("Select Vendor")
            Return
        Else
            vendor = ddlVendor.SelectedItem.Text
        End If

        If Not FileUpload1.HasFile Then
            MsgLbl.Text = "Please Select A File"
            MsgLbl.ForeColor = System.Drawing.Color.Crimson
            Return
        End If
        
        If Not FileUpload1.FileName.EndsWith(".txt") Then
            MsgLbl.Text = "Please Upload only text file"
            MsgLbl.ForeColor = System.Drawing.Color.Red
            Return
        End If

        Dim fn As String = DateTime.Now.Day.ToString()
        fn += DateTime.Now.Month.ToString()
        fn += DateTime.Now.Year.ToString() & "."
        fn += DateTime.Now.Hour.ToString() & "_"
        fn += DateTime.Now.Minute.ToString() & "_"
        fn += DateTime.Now.Second.ToString() & "."
        fn += DropDownList1.SelectedItem.ToString
        fn += DropDownList2.SelectedItem.ToString + ".txt"
        FileUpload1.SaveAs(Server.MapPath("~/Uploads/" & fn))

        Dim strcont As String = FileUpload1.FileContent.ToString()
        Dim r As New StreamReader(FileUpload1.FileContent)
        ' string[] arrayOfStrings=null;
        Dim i As Integer = 0
        Dim listofString = New List(Of String)()
        While r.Peek() <> -1
            Dim textline As String = r.ReadLine()
            listofString.Add(textline)
            i += 1
        End While
        r.Close()

        Dim k As Integer = 0
        Dim j As Integer = 0
        While j < listofString.Count
            Dim serial As String
            Dim pins As String
            ' char[] delimit=new char[] {';'};
            ' int index= listofString[j].IndexOf(';');
            'serial=arrayOfStrings[j].IndexOf(';',index+1);
            'serial=listofString[j].Substring(0,listofString[j].IndexOf(";"));
            'pins=listofString[j].Substring(serial.Length+1);

            Dim ser As String() = listofString(j).ToString().Split(";"c)

            If ser.Length <> 2 Then
                k += 1
            Else

                Try
                    serial = ser(0)
                    pins = ser(1)

                    Using cmd As New SqlCommand("OPEN SYMMETRIC KEY [key_DataShare] DECRYPTION BY CERTIFICATE cert_keyProtection INSERT INTO DUMMYPINS([InvoiceNumber],Serial,PIN,EncryptedPIN,[Service],Amount,Status,Amount2,FLAG) values ('" & txtInvoice.Text.Trim() & "','" & serial & "','" & vendor & "',encryptbykey(key_guid('key_DataShare'),'" & pins & "'), '" & DropDownList1.SelectedItem.ToString & "', '" & DropDownList2.SelectedValue.ToString & "',0,'" & DropDownList2.SelectedItem.ToString & "',0)  CLOSE SYMMETRIC KEY [key_DataShare];", cn)
                        If (cn.State = ConnectionState.Closed) Then
                            cn.Open()
                        End If
                        cmd.ExecuteNonQuery()
                        If (cn.State = ConnectionState.Open) Then
                            cn.Close()
                        End If
                    End Using
                Catch E As Exception
                    Console.Out.WriteLine("Exception is " & E.ToString)
                    k += 1
                End Try
            End If
            j += 1
        End While

        Dim tot As Integer = j - k

        Using cmdinsert As New SqlCommand("INSERT INTO [payit].[dbo].[UploadPINSHistory]([InvoiceNumber],[Service],[Vendor],[Amount],[Info1],[UploadedPINS]) values ('" & txtInvoice.Text.Trim() & "','" & DropDownList1.SelectedItem.Text & "','" & vendor & "', '" & DropDownList2.SelectedValue.ToString() & "','" & DropDownList2.SelectedItem.Text.ToString() & "','" & tot.ToString() & "');", cn)
            If (cn.State = ConnectionState.Closed) Then
                cn.Open()
            End If
            cmdinsert.ExecuteNonQuery()
            If (cn.State = ConnectionState.Open) Then
                cn.Close()
            End If
        End Using

        MsgLbl.Text = "File Uploaded Successfully:" & tot.ToString & " pins <br/>"
        MsgLbl.Text += "Total Count : " & j.ToString() & "<br/>"
        MsgLbl.Text += "Failed Pins : " & k.ToString() & "<br/>"
        MsgLbl.ForeColor = System.Drawing.Color.Green

    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        Dim CS As String = "data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096"
        Dim con As New SqlConnection(CS)
        Using cmd As New SqlCommand("select amount,amount2 from Services s, Denominations d where s.ServiceID=d.ServiceID and d.ServiceID=" + DropDownList1.SelectedValue, con)
            con.Open()
            DropDownList2.DataSource = cmd.ExecuteReader()
            DropDownList2.DataBind()
        End Using
    End Sub

    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "popupScript", popupscript, True)
    End Sub

    Public Sub alertme(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
                      "alertify.alert('" & s & "')" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
End Class