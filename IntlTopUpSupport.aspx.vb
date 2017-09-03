Imports System.Data
Imports System.Data.SqlClient
Partial Class IntlTopUpSupport
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim da As SqlDataAdapter
    Dim ds, ds1 As DataSet
    Dim sql As String

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        sql = "SELECT myid Details,usern, company,amt,ptype,trackid,destmob,topupamt,topupresult,tdatetime from topup_trans where (destmob like '" & TextBox1.Text & "' or trackid like '" & TextBox2.Text & "') order by myid desc"
        sqldatabind(sql)
        Session("sql") = sql

    End Sub

    Public Sub sqldatabind(ByVal s As String)
        da = New SqlDataAdapter(s, cn)
        ds = New DataSet()
        da.Fill(ds, "deta")
        GridView1.DataSource = ds.Tables("deta")
        GridView1.DataBind()

    End Sub
    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        Dim a As String = ""
        Dim i As Integer
        For i = 0 To GridView1.Rows.Count - 1
            a = GridView1.Rows(i).Cells(0).Text
            GridView1.Rows(i).Cells(0).Text = "<a href= 'grid.aspx?id=" & a & "&qi=" & "Intl" & "'>" & "Details" & "</a>"

        Next
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        Dim s As String
        s = Session("sql")
        sqldatabind(s)
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("user") = "" Then
            Response.Redirect("login.aspx")
        End If
        cn = New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")
    End Sub
End Class
