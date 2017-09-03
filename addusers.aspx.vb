Imports System.Data.SqlClient
Imports System.Data
Partial Class addusers
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sqlLocalTrans, Sql As String
    Private grdTotal As Decimal
    Dim fromdate, todate As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Dim typeS As String = ""
   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin") Then
            alert("Unauthorized Access")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=addusers.aspx")
        End If
        
        If Not IsPostBack Then
            ddlCompany.Items.Insert(0, New ListItem("Select", "None"))
            ddlCompany.Items.Insert(1, New ListItem("Android", "Android"))
            ddlCompany.Items.Insert(2, New ListItem("iSYS", "isys"))
        End If
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Page.IsValid Then
            Dim cn As SqlConnection = New SqlConnection(strConnString)
            Dim Query As String
            If (TextUser.Text = "" Or TextPwd.Text = "") Then
                alert("Please fill all fields!")
            Else
                Dim sql1 As String = "SELECT * FROM payit.dbo.users WHERE [auser] like '@user' and apass like '@pass'"
                Dim cmd1 As SqlCommand = New SqlCommand(sql1, cn)
                cmd1.Connection = cn
                cn.Open()
                cmd1.Parameters.AddWithValue("@user", TextUser.Text)
                cmd1.Parameters.AddWithValue("@pass", TextPwd.Text)
                Using reader As SqlDataReader = cmd1.ExecuteReader()
                    If reader.HasRows Then
                        ' Record already exists
                        alert("User Already Exist!")
                    Else
                        Using con As New SqlConnection(strConnString)
                            Query = "INSERT INTO [payit].[dbo].[users] ([company],[auser],[apass],[acc_type],[acc_status],[dates],[times]) VALUES (@company,@user,@pass,@type,@status,@date,@time);" & _
                                    "INSERT INTO [PayItv2].[dbo].[ThirdPartyUsers] ([Name],[UserName],[Password],[AvailableBalance],[Status],[CreatedOn],[PaymentType]) VALUES (@name,@user,@pass,@balance,@status,@datetime,@paymentType)"
                            Using cmd As New SqlCommand(Query)
                                cmd.Parameters.AddWithValue("@company", ddlCompany.SelectedItem.Text)
                                cmd.Parameters.AddWithValue("@user", TextUser.Text)
                                cmd.Parameters.AddWithValue("@pass", TextPwd.Text)
                                cmd.Parameters.AddWithValue("@type", "COMPANY-API")
                                cmd.Parameters.AddWithValue("@status", "Active")
                                cmd.Parameters.AddWithValue("@date", DateTime.Now.Date)
                                cmd.Parameters.AddWithValue("@time", DateTime.Now.TimeOfDay)
                                cmd.Parameters.AddWithValue("@name", "iSYS")
                                cmd.Parameters.AddWithValue("@balance", 10000)
                                cmd.Parameters.AddWithValue("@datetime", "GETDATE()")
                                cmd.Parameters.AddWithValue("@paymentType", "All")
                                Using sda As New SqlDataAdapter()
                                    cmd.Connection = con
                                    sda.SelectCommand = cmd
                                    Using dt As New DataTable()
                                        sda.Fill(dt)
                                        GridView1.DataBind()
                                    End Using
                                End Using
                            End Using
                        End Using
                        alert("User Successfully Added!")
                    End If
                End Using
                cn.Close()
            End If
        End If
    End Sub
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    'Private Sub BindData()
    '    Dim strQuery As String = "select * from [payit].[dbo].[users] order by myid desc"
    '    Dim cmd As SqlCommand = New SqlCommand(strQuery)
    '    GridView1.DataSource = GetData(cmd)
    '    GridView1.DataBind()
    'End Sub
    'Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
    '    Dim sqlLocalTrans As String
    '    sqlLocalTrans = "select * from [payit].[dbo].[users] order by myid desc"
    '    Session("sqlLocalTrans") = sqlLocalTrans
    '    Dim cmd As SqlCommand = New SqlCommand(sqlLocalTrans)
    '    GridView1.DataSource = GetData(cmd)
    '    GridView1.DataBind()
    'End Sub
    'Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
    '    Me.BindData()
    '    GridView1.PageIndex = e.NewPageIndex
    'End Sub
    'Public Sub sqldatabind(ByVal s As String, ByVal e As GridViewRowEventArgs, Optional ByVal types1 As String = "", Optional ByVal type As String = "")
    '    da = New SqlDataAdapter()
    '    ds = New DataSet()
    '    da.Fill(ds, "det")
    '    GridView1.DataSource = ds.Tables("det")
    '    GridView1.DataBind()
    'End Sub
    'Private Function GetData(ByVal cmd As SqlCommand) As DataTable
    '    Dim dt As DataTable = New DataTable
    '    Dim con As SqlConnection = New SqlConnection(strConnString)
    '    Dim sda As SqlDataAdapter = New SqlDataAdapter
    '    cmd.Connection = con
    '    con.Open()
    '    sda.SelectCommand = cmd
    '    sda.Fill(dt)
    '    Return dt
    'End Function
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("users.xls", Me.GridView1)
    End Sub
End Class
