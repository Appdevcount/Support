Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO

Partial Class loginnew
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim sql, sql1 As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        cn = New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")

        'Dim totalSessionBytes As Long = 0
        'Dim b As New BinaryFormatter()
        'Dim m As MemoryStream
        'For Each obj In Session
        '    m = New MemoryStream()
        '    b.Serialize(m, obj)
        '    totalSessionBytes += m.Length
        '    Response.Write("<font color=red>" & obj.ToString & "</font>")

        'Next
        'Response.Write("<font color=red>" & totalSessionBytes & "</font>")

    End Sub
    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        sql = "Select Username,Password from CMSUsers where Username like '" & txtUser.Value & "' and Password like '" & txtPass.Value & "' and Status = 1"
        da = New SqlDataAdapter(sql, cn)
        ds = New DataSet()
        da.Fill(ds, "userl")

        If ds.Tables("userl").Rows.Count > 0 Then

            Session("user") = Trim(txtUser.Value)
            Session("pass") = Trim(txtPass.Value)

            Dim returnUrl = Request.QueryString("ReturnURL")

            If String.IsNullOrEmpty(returnUrl) Then
                returnUrl = "index.aspx"
            ElseIf returnUrl = "loggedOut" Then
                alert("Successfully Logged Out")
                returnUrl = "index.aspx"
            End If
            Response.Redirect(returnUrl)
        Else
            alert("Invalid Login")
        End If
    End Sub
    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
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

        'sql = "Select Username,Password from CMSUsers where Username like '" & txtUser.Value & "' and Password like '" & txtPass.Value & "' and Status = 1"
        'da = New SqlDataAdapter(sql, cn)
        'ds = New DataSet()
        'da.Fill(ds, "userl")

        'If ds.Tables("userl").Rows.Count > 0 Then

        '    Session("user") = Trim(txtUser.Value)
        '    Session("pass") = Trim(txtPass.Value)

        '    Dim returnUrl = Request.QueryString("ReturnURL")
        '    If String.IsNullOrEmpty(returnUrl) Then
        '        returnUrl = "index.aspx"
        '    End If
        '    Response.Redirect(returnUrl)
        'Else

        '    Dim popupscript As String
        '    popupscript = "<script language='javascript'>" & _
        '                                          "alertify.alert('Invalid Login')" & _
        '                                          "</script>"
        '    ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
        'End If
    End Sub
    Protected Sub forgetLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles forgetLink.Click
        alertPrompt("Hahaha")
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
