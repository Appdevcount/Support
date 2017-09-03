Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO
Imports System.Net

Partial Class login_old
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim sql, sql1 As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Redirect("login.apsx")
        txtUser.Focus()
        Session("user") = String.Empty
        Session("role") = String.Empty
        cn = New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        'sql = "Select Username,Password from CMSUsers where Username like '" & txtUser.Text & "' and Password like '" & txtPass.Text & "' and Status = 1"
        Dim sql1 As String = "Select Username,Password,Role from CMSUsers where Username like @username and Password like @pass and Status =@stat"
        Dim cmd1 As SqlCommand = New SqlCommand(sql1, cn)
        cmd1.Connection = cn
        cn.Open()
        cmd1.Parameters.Add("@username", SqlDbType.VarChar).Value = txtUser.Text.Trim()
        cmd1.Parameters.Add("@pass", SqlDbType.VarChar).Value = txtPass.Text.Trim()
        cmd1.Parameters.Add("@stat", SqlDbType.Bit).Value = 1
        Using reader As SqlDataReader = cmd1.ExecuteReader()
            If reader.HasRows Then
                reader.Read()
                'alert("User: " & reader("Username") & ", Pass: " & reader("Password"))
                Dim compUser As String = String.Empty
                Dim compPassword As String = String.Empty
                Dim compRole As String = String.Empty
                compUser = reader("Username").ToString().ToLower()
                compPassword = reader("Password").ToString().ToLower()
                compRole = reader("Role").ToString().ToLower()
                If (compUser = txtUser.Text.ToLower() And compPassword = txtPass.Text.ToLower()) Then
                    Session.Add("user", Trim(txtUser.Text))
                    Session.Timeout = 180
                    'Session("user") = Trim(txtUser.Text)
                    'Session("pass") = Trim(txtPass.Text)
                    Session("role") = compRole
                    'Session("ip") = Request.UserHostName
                    'Session("pc") = Request.LogonUserIdentity.Name

                    Dim ip As String = Request.UserHostAddress
                    Session("ip") = ip

                    'Dim computer_name As String = Dns.GetHostEntry(Request.ServerVariables("LOCAL_ADDR")).HostName
                    'Session("pc") = computer_name.ToUpperInvariant()

                    Dim lastAccess As String = "UPDATE CMSUsers SET LastAccessed=@lastaccess, info2=@userip where Username like @user and Password like @passwd and Status = @status;" & _
                                               "INSERT INTO [payit].[dbo].[CMSUsersLog] ([Username],[info2],[LastAccessed]) VALUES (@user,@userip,@lastaccess);"
                    Using connect As New SqlConnection(strConnString)
                        Using command As New SqlCommand(lastAccess)
                            command.Parameters.AddWithValue("@lastaccess", DateTime.Now.ToString())
                            command.Parameters.AddWithValue("@userip", Request.UserHostAddress)
                            command.Parameters.Add("@user", SqlDbType.VarChar).Value = txtUser.Text.Trim()
                            command.Parameters.Add("@passwd", SqlDbType.VarChar).Value = txtPass.Text.Trim()
                            command.Parameters.Add("@status", SqlDbType.Bit).Value = 1

                            command.CommandType = CommandType.Text
                            command.Connection = connect
                            If connect.State = ConnectionState.Closed Then connect.Open()
                            Dim inserted As Integer = command.ExecuteNonQuery()
                            If connect.State = ConnectionState.Open Then connect.Close()
                        End Using
                    End Using
                    Dim returnUrl = Request.QueryString("ReturnURL")

                    If String.IsNullOrEmpty(returnUrl) Then
                        returnUrl = "index.aspx"
                    End If

                    Response.Redirect(returnUrl)
                Else
                    txtUser.Text = String.Empty
                    txtPass.Text = String.Empty
                    alert("Check the Username and Password")
                End If
            Else
                ' Record does not exist, add them
                Session("user") = String.Empty
                Session("pass") = String.Empty
                alert("Invalid Login. Try Again")
            End If
            reader.Close()
        End Using
        cn.Close()
        'If (TextBox1.Text = "Support" And TextBox2.Text = "Hawraa") Or (TextBox1.Text = "Afif" And TextBox2.Text = "Farah540390#") Or (TextBox1.Text = "Shareef" And TextBox2.Text = "IsysIsys") Or (TextBox1.Text = "Aymen" And TextBox2.Text = "Aymen#123") Then
        '    Session("user") = TextBox1.Text
        '    Response.Redirect("index.aspx")
        'If (TextBox1.Text = "Support" And TextBox2.Text = "Payitsup@123") Or (TextBox1.Text = "AdmiN" And TextBox2.Text = "Payit*#999#") Or (TextBox1.Text = "Shareef" And TextBox2.Text = "Isys*999#") Or (TextBox1.Text = "Aymen" And TextBox2.Text = "Aymen#123") Or (TextBox1.Text = "QA" And TextBox2.Text = "QA*123#") Or (TextBox1.Text = "AdmiN1" And TextBox2.Text = "PayitA@Min123") Or (TextBox1.Text = "AdmiN2" And TextBox2.Text = "Pay@itAD$Min589") Then
        '    Session("user") = TextBox1.Text
        '    Response.Redirect("index.aspx")
        'ElseIf (TextBox1.Text = "Ooredoo" And TextBox2.Text = "Ooredoo#589") Then
        '    Session("user") = TextBox1.Text
        '    Response.Redirect("OoredooSummaryReport.aspx")
        'ElseIf (TextBox1.Text = "zakat" And TextBox2.Text = "zakat*589#") Then
        '    Session("user") = TextBox1.Text
        '    Response.Redirect("zakatprojects.aspx")
        'Else

        'da = New SqlDataAdapter(sql, cn)
        'ds = New DataSet()
        'da.Fill(ds, "userl")

        'If ds.Tables("userl").Rows.Count > 0 Then

        '    Session("user") = Trim(txtUser.Text)
        '    Session("pass") = Trim(txtPass.Text)
        'Else
        '    alert("Invalid")
        'End If
    End Sub
    Public Sub alertPrompt(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
                      "alertify.prompt('" & s & "')" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
                      "alertify.alert('" & s & "')" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
End Class
