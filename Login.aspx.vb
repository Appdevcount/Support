Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO
Imports System.Net

Partial Class Login
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim sql, sql1 As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("user") = String.Empty
        Session("role") = String.Empty
        cn = New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.ServerClick
        'sql = "Select Username,Password from CMSUsers where Username like '" & txtUser.Text & "' and Password like '" & txtPass.Text & "' and Status = 1"
        Dim sql1 As String = "Select ID,Username,Password,Role from CMSUsers where Username like @username and Password like @pass and Status =@stat"
        Dim cmd1 As SqlCommand = New SqlCommand(sql1, cn)
        cmd1.Connection = cn
        cn.Open()
        cmd1.Parameters.Add("@username", SqlDbType.VarChar).Value = name.Value.Trim()
        cmd1.Parameters.Add("@pass", SqlDbType.VarChar).Value = txtpassword.Value.Trim()
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
                If (compUser = name.Value.ToLower() And compPassword = txtpassword.Value.ToLower()) Then
                    Session.Add("user", Trim(name.Value))

                    Session.Timeout = 180
                    Session("role") = compRole
                    Dim ip As String = Helper.GetIPv4Address()
                    Session("ip") = ip

                    Dim lastAccess As String = "UPDATE CMSUsers SET LastAccessed=@lastaccess, info2=@userip where Username like @user and Password like @passwd and Status = @status;" & _
                                               "INSERT INTO [payit].[dbo].[CMSUsersLog] ([Username],[info2],[LastAccessed]) VALUES (@user,@userip,@lastaccess);"
                    Using connect As New SqlConnection(strConnString)
                        Using command As New SqlCommand(lastAccess)
                            command.Parameters.AddWithValue("@lastaccess", DateTime.Now.ToString())
                            command.Parameters.AddWithValue("@userip", ip)
                            command.Parameters.Add("@user", SqlDbType.VarChar).Value = name.Value.Trim()
                            command.Parameters.Add("@passwd", SqlDbType.VarChar).Value = txtpassword.Value.Trim()
                            command.Parameters.Add("@status", SqlDbType.Bit).Value = 1

                            command.CommandType = CommandType.Text
                            command.Connection = connect
                            If connect.State = ConnectionState.Closed Then connect.Open()
                            Dim inserted As Integer = command.ExecuteNonQuery()
                            If connect.State = ConnectionState.Open Then connect.Close()
                        End Using
                    End Using
                    Dim returnUrl = Request.QueryString("ReturnURL")

                    If (compUser.Equals("zakat")) Then
                        returnUrl = "zakatprojects.aspx"
                    End If

                    If (compUser.Equals("zain")) Then
                        returnUrl = "OgMoneySummary.aspx"
                    End If

                    If (compUser.Equals("alghanim")) Then
                        returnUrl = "PaymentTransactions.aspx"
                    End If

                    If ((compUser.Equals("alnajat")) Or (compUser.Equals("turathislamy")) Or (compUser.Equals("alrahma"))) Then
                        returnUrl = "alrahmacharity.aspx"
                    End If

                    If String.IsNullOrEmpty(returnUrl) Then
                        returnUrl = "index.aspx"
                    End If

                    Response.Redirect(returnUrl)
                Else
                    name.Value = String.Empty
                    txtpassword.Value = String.Empty
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
