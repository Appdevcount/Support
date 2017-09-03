Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Text.RegularExpressions

Partial Class Complaints
    Inherits System.Web.UI.Page

    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim typeS As String = ""


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "cs" Or Session("role") = "qa") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=CommissionServices.aspx")
        End If

        If Session("user") = "" Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=Complaints.aspx")
        End If
        Dim sqlLocalTrans As String

        cn = New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096;Connection Timeout=120")
        If Page.IsPostBack = False Then
            sqlLocalTrans = "select top 1000 * from payitcomplaints order by id desc"

            Session("sqlLocalTrans") = sqlLocalTrans
            sqldatabind(sqlLocalTrans)
            counter1()
        End If

        If Not IsPostBack Then
            Me.BindGrid()
        End If
    End Sub

    Protected Sub Search(sender As Object, e As EventArgs)
        Me.BindGrid()
    End Sub

    Private Sub BindGrid()
        Dim constr As String = ConfigurationManager.ConnectionStrings("payitConnectionActive").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand()
                cmd.CommandText = "SELECT * FROM payitcomplaintstrack order by id desc"
                cmd.Connection = con
                Dim dt As New DataTable()
                Using sda As New SqlDataAdapter(cmd)
                    sda.Fill(dt)
                    GridView2.DataSource = dt
                    GridView2.DataBind()
                End Using
            End Using
        End Using
    End Sub

    Protected Sub OnPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        GridView2.PageIndex = e.NewPageIndex
        Me.BindGrid()
    End Sub

    ' If Not Page.IsPostBack Then
    'Dim DropDown = DropDown
    'Dim zonename As String = DropDownList.SelectedItem.Text
    'End If

    Protected Sub Link1_Click()

    End Sub
    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        counter1()
        Dim sqlLocalTrans As String

        sqlLocalTrans = "select top 1000 * from payitcomplaints order by id desc"
        Session("sqlLocalTrans") = sqlLocalTrans
        sqldatabind(sqlLocalTrans)

    End Sub

    Public Sub counter1()
        Button2.Attributes.Add("onclick", "javascript:abct()")
    End Sub

    Public Sub sqldatabind(ByVal s As String)

        'sqlLocalTrans += " order by tk.tdatetime desc"
        'Dim SqlOrder As String
        'Session("type") = types1
        'If types1 = "success" Then
        '    SqlOrder = s & " order by id desc"
        'Else
        '    SqlOrder = s & " order by tk.LastUpdateOn desc"
        'End If



        da = New SqlDataAdapter(s, cn)
        ds = New DataSet()



        da.Fill(ds, "det")

        'If CheckBox1.Checked = True Then
        '    GridView1.AllowPaging = True
        'Else
        '    GridView1.AllowPaging = False
        'End If


        GridView1.DataSource = ds.Tables("det")
        GridView1.DataBind()

    End Sub

    Public Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        Dim s As String
        s = Session("sqlLocalTrans")
        sqldatabind(s)

    End Sub
End Class
