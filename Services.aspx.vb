
Imports System.Data
Imports System.Data.SqlClient

Partial Class Services
    Inherits System.Web.UI.Page

    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim typeS As String = ""
    Dim sqlLocalTrans As String = "SELECT * FROM PayitServices ORDER BY ID"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "cs" Or Session("role") = "operations" Or Session("role") = "accounts") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=Services.aspx")
        End If

        cn = New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")

        sqldatabind("select ServiceCode, ServiceName, Status from PayitServices where status = 1 and Type LIKE 'PIN'")

        If Page.IsPostBack = False Then
            'Label14.Visible = False
            'Label15.Visible = False
            'FromDateTextBox.Text = Format(Date.Today.Date, "dd/MM/yyyy")
            'ToDateTextBox.Text = Format(Date.Today.Date, "dd/MM/yyyy")
            'Label17.Text = ""
            'Dim typeS As String = ""
            'typeS = Request.QueryString("type")
            'If typeS = "fail" Then
            '    ddlServiceProvider.Enabled = False
            'Else
            '    ddlServiceProvider.Enabled = True
            'End If
            'counter1()
        End If

    End Sub

    Public Sub sqldatabind(ByVal s As String)

        da = New SqlDataAdapter(s, cn)
        ds = New DataSet()
        da.Fill(ds, "det")

        GridView1.AllowPaging = True
        GridView1.DataSource = ds.Tables("det")
        GridView1.DataBind()

    End Sub


    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex

        sqldatabind(sqlLocalTrans)

    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound

    End Sub
End Class
