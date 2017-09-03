Imports System.Data.SqlClient

Partial Class CashUReturnURLTesting
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Page.Response.ContentType = "text/xml"
        'Dim reader As New System.IO.StreamReader(Page.Request.InputStream)
        'Dim data As String = reader.ReadToEnd

        'Label3.Text = data
        'reader.Close()


        'Dim headers As NameValueCollection = Request.Headers

        'For i = 0 To headers.Count - 1
        '    Dim key As String = headers.GetKey(i)
        '    Dim value As String = headers.Get(i)

        '    Response.Write(key & "=" & value & "<br/>")
        'Next

        'Dim deviceModel As String = Request.Headers("User-Agent")
        'Dim deviceModels() As String = deviceModel.Split(";", 10, StringSplitOptions.RemoveEmptyEntries)
        ''Dim deviceModel As String = Request.Headers("User-Agent")
        ''Response.Write(deviceModels(4))
        'Response.Write(deviceModel)


        'Page.Response.ContentType = "text/html"
        'Dim reader As New System.IO.StreamReader(Page.Request.InputStream)
        'Response.Write(reader.ReadToEnd)

        'Dim cmd As New SqlCommand
        'Dim connection As New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")

        'If connection.State = Data.ConnectionState.Closed Then connection.Open()
        Try


            'cmd.Connection = connection
            'cmd.CommandText = "CashUReturnURL_INSERT"
            'cmd.CommandType = Data.CommandType.StoredProcedure
            'cmd.Parameters.AddWithValue("lang", Request.Params("language"))
            'cmd.Parameters.AddWithValue("amount", Double.Parse(Request.Params("amount")))
            'cmd.Parameters.AddWithValue("netamount", Double.Parse(Request.Params("netamount")))
            'cmd.Parameters.AddWithValue("currency", Request.Params("currency"))
            'cmd.Parameters.AddWithValue("sessionid", Request.Params("session_id"))
            'cmd.Parameters.AddWithValue("servicecode", Request.Params("txt1"))
            'cmd.Parameters.AddWithValue("referenceid", Request.Params("trn_id"))
            'cmd.Parameters.AddWithValue("trndate", Request.Params("trnDate"))
            'cmd.Parameters.AddWithValue("service", Request.Params("servicesName"))
            'cmd.ExecuteNonQuery()
        Catch ex As Exception

        End Try
    End Sub
End Class
