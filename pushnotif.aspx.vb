Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Partial Class pushnotif
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sql As String
    Dim sqlLocalTrans As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Private conn As New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "accounts" Or Session("role") = "superadmin" Or Session("role") = "operations") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=pushnotif.aspx")
        End If

        If Not IsPostBack Then
            Me.BindData()
        End If

        If Not Me.IsPostBack Then
            FromDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")

            ddlStatus.Items.Insert(0, New ListItem("Select", "None"))
            ddlStatus.Items.Insert(1, New ListItem("Active", "1"))
            ddlStatus.Items.Insert(2, New ListItem("Inactive", "0"))
            If ddlStatus.Items.Count > 0 Then
                ddlStatus.SelectedIndex = 2
            End If

            ddlPlatform.Items.Insert(0, New ListItem("Select", "None"))
            ddlPlatform.Items.Insert(1, New ListItem("iOS", "iOS"))
            ddlPlatform.Items.Insert(2, New ListItem("Android", "Android"))
            ddlPlatform.Items.Insert(3, New ListItem("Both", "Both"))
            If ddlPlatform.Items.Count > 0 Then
                ddlPlatform.SelectedIndex = 3
            End If

            ddlMsgCat.Items.Insert(0, New ListItem("Select", "None"))
            ddlMsgCat.Items.Insert(1, New ListItem("VIVA", "VIVA"))
            ddlMsgCat.Items.Insert(2, New ListItem("WATANIYA", "WATANIYA"))
            ddlMsgCat.Items.Insert(3, New ListItem("ZAIN", "ZAIN"))

        End If
    End Sub
    Private Sub BindData()
        Dim strQuery As String = ("select top 1000 * from PushNotificationMessages order by ID desc")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
    End Sub

    Public Sub sqldatabind(ByVal s As String, Optional ByVal types1 As String = "", Optional ByVal type As String = "")
        da = New SqlDataAdapter()
        ds = New DataSet()
        da.Fill(ds, "det")
        GridView1.DataSource = ds.Tables("det")
        GridView1.DataBind()
    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If Not (Session("role") = "accounts" Or Session("role") = "superadmin" Or Session("role") = "operations") Then
            alert("Unauthorized to perform this action")
            Exit Sub
        End If

        If Page.IsValid Then
            Dim fromdate As String
            Label14.Visible = False
            fromdate = Trim(FromDateTextBox.Text) & " 00:00:00"

            If (ddlPlatform.SelectedItem.Value = "" Or ddlStatus.SelectedItem.Value = "") Then
                dberrorlabel.Text = "Required Values Cannot be Empty"
            Else
                Dim constring As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
                Using con As New SqlConnection(constring)

                    If (ddlStatus.Text = "Active") Then
                        ddlStatus.Text = 1
                    ElseIf (ddlStatus.Text = "Inactive") Then
                        ddlStatus.Text = 0
                    End If
                    Using cmd As New SqlCommand("INSERT INTO PushNotificationMessages (Message, Platform, Language, Application, MessageCategory, ScheduledDate, Status, StatusDescription, CreatedDate) VALUES ('" & txtMsg.Text & "','" & ddlPlatform.SelectedItem.Value & "','" & ddlMsgCat.SelectedValue.Trim() & "','PayitKuwait','','" & FromDateTextBox.Text & "'," & ddlStatus.SelectedItem.Value & ",'PENDING',GETDATE())")
                        Using sda As New SqlDataAdapter()
                            cmd.Connection = con
                            sda.SelectCommand = cmd
                            Using dt As New DataTable()
                                sda.Fill(dt)
                                GridView1.DataSource = dt
                                GridView1.DataBind()
                                BindData()
                            End Using
                        End Using
                    End Using

                End Using
            End If
            Clear()
        End If
    End Sub
    Private Function GetData(ByVal cmd As SqlCommand) As DataTable
        Dim dt As DataTable = New DataTable
        Dim con As SqlConnection = New SqlConnection(strConnString)
        Dim sda As SqlDataAdapter = New SqlDataAdapter
        cmd.Connection = con
        con.Open()
        sda.SelectCommand = cmd
        sda.Fill(dt)
        Return dt
    End Function
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        Me.BindData()
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataBind()
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("PushNotifications.xls", Me.GridView1)
    End Sub
    Protected Sub Clear()
        txtMsg.Text = String.Empty
        ddlPlatform.SelectedIndex = 0
        ddlMsgCat.SelectedIndex = 0
    End Sub
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
End Class
