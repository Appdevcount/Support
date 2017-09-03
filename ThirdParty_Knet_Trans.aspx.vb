Imports System.Data
Imports System.Data.SqlClient
Partial Class ThirdParty_Knet_Trans
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim da As SqlDataAdapter
    Dim ds, ds1 As DataSet
    Dim sql As String

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click

        If TextBox1.Text = "" Then
            'sql = "SELECT myid,usern, company,amt,ptype,trackid,destmob,topupamt,topupresult,tdatetime from topup_trans where convert(datetime, tdatetime,103) between convert(datetime, '" & TextBox2.Text & "',103) and  convert(datetime, '" & TextBox3.Text & "',103) and rtrim(ltrim(company)) like 'Fcc' order by myid desc"
            'sql = "select company,Tuser,Tresponse_link Link,TId,ISysId,amt,ptype,trackID,payid,transid,refid," & _
            '      "tdatetime TDate,postdate,knetprocess,errmsg from ThirdParty_knet_trans where " & _
            '      " convert(datetime, tdatetime,103) between" & _
            '      " convert(datetime, '" & TextBox2.Text & "',103)" & _
            '      " and convert(datetime, '" & TextBox3.Text & "',103)" & _
            '      " order by myid desc"
            sql = "select company,Tuser,Tresponse_link Link,TId,amt,trackID,transid,refid," & _
                  "tdatetime TranDate,LastUpdateOn ProcessDatepostdate,knetprocess,errmsg from ThirdParty_knet_trans where " & _
                  " convert(datetime, LastUpdateOn,103) between" & _
                  " convert(datetime, '" & TextBox2.Text & "',103)" & _
                  " and convert(datetime, '" & TextBox3.Text & "',103)" & _
                  " order by myid desc"
        ElseIf TextBox1.Text <> "" Then
            'sql = "select company,Tuser,Tresponse_link,TId,ISysId,amt,ptype,trackID,payid,transid,refid," & _
            '       "tdatetime,postdate,knetprocess,errmsg from ThirdParty_knet_trans where (Tid like '" & TextBox1.Text & "')" & _
            '       " and convert(datetime, tdatetime,103) between" & _
            '       " convert(datetime, '" & TextBox2.Text & "',103)" & _
            '       " and convert(datetime, '" & TextBox3.Text & "',103)" & _
            '       " order by myid desc"
            sql = "select company,Tuser,Tresponse_link Link,TId,amt,trackID,transid,refid," & _
                  " tdatetime TranDate,LastUpdateOn ProcessDate,postdate,knetprocess,errmsg  from ThirdParty_knet_trans where (Tid like '" & TextBox1.Text & "')" & _
                  " and convert(datetime, LastUpdateOn,103) between" & _
                  " convert(datetime, '" & TextBox2.Text & "',103)" & _
                  " and convert(datetime, '" & TextBox3.Text & "',103)" & _
                  " order by myid desc"
       
        End If
        sqldatabind(sql)
        Session("sql") = sql

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("user") = "" Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=ThirdParty_Knet_Trans.aspx")
        End If
        cn = New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")


    End Sub

    

    Public Sub sqldatabind(ByVal s As String)
        da = New SqlDataAdapter(s, cn)
        ds = New DataSet()
        da.Fill(ds, "det")
        GridView1.DataSource = ds.Tables("det")
        GridView1.DataBind()

    End Sub


    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        Dim s As String
        s = Session("sql")
        sqldatabind(s)
    End Sub
End Class
