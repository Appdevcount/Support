Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Configuration
Imports System.Net
Imports Newtonsoft.Json
Imports System.Reflection
Imports System.IO
Imports System.Security.Cryptography

Public Class SMS
    Public Shared Function GetUniqueKey(KeyLength As Integer) As String
        Dim a As [String] = "123456789"
        Dim chars As [Char]() = New [Char]((a.Length) - 1) {}
        chars = a.ToCharArray()
        Dim data As Byte() = New Byte((KeyLength) - 1) {}
        Dim crypto As New RNGCryptoServiceProvider()
        crypto.GetNonZeroBytes(data)
        Dim result As New StringBuilder(KeyLength)
        For Each b As Byte In data
            result.Append(chars(b Mod (chars.Length)))
        Next
        Return result.ToString()
    End Function
    Public Shared Function sendSMS(message As String, mobilenumber As String, providercode As String, reference As String) As String
        Dim t As String = sendSMSToCore(message, mobilenumber, providercode, reference)
        Return t
    End Function

    Public Shared Function sendSMSToCore(message As String, mobilenumber As String, providercode As String, reference As String) As String
        Dim ri = New SMSObj()
        Dim response As Object
        Dim s As String
        Dim dataObjects As New SMSrespObj()
        ri.message = message
        ri.mobileNumber = mobilenumber
        ri.providerCode = providercode

        ri.referenceID = reference

        Dim status As Object
        Dim request As WebRequest = WebRequest.Create("https://ww1.payit.mobi/PayitKuwait/PayitKuwaitV4/api/API/sendSMS?msisdn=" + mobilenumber + "&message=" + message.Trim() + "")
        request.Method = "POST"
        request.ContentLength = 0
        response = request.GetResponse()
        Try
            response = request.GetResponse()
            status = DirectCast(response, HttpWebResponse).StatusCode
        Catch we As WebException
            status = DirectCast(we.Response, HttpWebResponse).StatusCode
        End Try

        Dim webResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        If webResponse.StatusCode <> HttpStatusCode.OK Then
        End If

        Using reader As New StreamReader(webResponse.GetResponseStream())
            s = reader.ReadToEnd()
            reader.Close()
            response = s
        End Using

        If webResponse.StatusCode = HttpStatusCode.OK Then
            response = "success"
        Else
            response = "failed"
        End If
        Return response.ToString()
    End Function

    Public Shared Function sendSMSThroughFCC(message As String, mobilenumber As String, providercode As String, reference As String) As String
        Dim dataObjects As New SMSrespObj()
        Dim result As String
        Dim FCC = New FCCSMSService.SmsService()
        FCC.Url = "https://secure.future-club.com/BulkSMSWebSrv/SmsService.asmx"
        FCC.Timeout = 20000
        Dim response As String = FCC.SendSMS(mobilenumber, message, "l", "Pay-it", "Payit", "123456", _
            "1031", "")

        If response.StartsWith("00") Then
            dataObjects.Status = "0"
            dataObjects.StatusDescription = "Success"
            result = "success"
        Else
            dataObjects.StatusDescription = "Fail"
            dataObjects.Status = "1"
            result = "failed"
        End If

        Return result
    End Function

    Public Class SMSrespObj
        Public Property Status() As String
            Get
                Return m_Status
            End Get
            Set(value As String)
                m_Status = value
            End Set
        End Property
        Private m_Status As String
        Public Property StatusDescription() As String
            Get
                Return m_StatusDescription
            End Get
            Set(value As String)
                m_StatusDescription = value
            End Set
        End Property
        Private m_StatusDescription As String
        Public Property ResponseReference() As String
            Get
                Return m_ResponseReference
            End Get
            Set(value As String)
                m_ResponseReference = value
            End Set
        End Property
        Private m_ResponseReference As String
    End Class
    Public Class SMSObj
        Public Property mobileNumber() As String
            Get
                Return m_mobileNumber
            End Get
            Set(value As String)
                m_mobileNumber = value
            End Set
        End Property
        Private m_mobileNumber As String
        Public Property providerCode() As String
            Get
                Return m_providerCode
            End Get
            Set(value As String)
                m_providerCode = value
            End Set
        End Property
        Private m_providerCode As String
        Public Property referenceID() As String
            Get
                Return m_referenceID
            End Get
            Set(value As String)
                m_referenceID = value
            End Set
        End Property
        Private m_referenceID As String
        Public Property message() As String
            Get
                Return m_message
            End Get
            Set(value As String)
                m_message = value
            End Set
        End Property
        Private m_message As String
    End Class
End Class
