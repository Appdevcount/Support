Imports Microsoft.VisualBasic

Public Class Helper
    Public Shared Function GetIPv4Address() As String
        GetIPv4Address = String.Empty

        Try
            Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
            Dim sIPAddress As String = context.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
            If String.IsNullOrEmpty(sIPAddress) Then
                GetIPv4Address = context.Request.ServerVariables("REMOTE_ADDR")
                Return GetIPv4Address
            Else
                Dim ipArray As String() = sIPAddress.Split(New [Char]() {","c})
                GetIPv4Address = ipArray(0)
                Return GetIPv4Address
            End If

        Catch ex As Exception
            GetIPv4Address = String.Empty
        End Try

        Return GetIPv4Address
    End Function
End Class
