Imports System.Data
Imports System.Data.SqlClient

Partial Class PaymentChannels
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=PaymentChannels.aspx")
        End If

        cn = New SqlConnection(strConnString)
        If Page.IsPostBack = False Then
            bindCheckBox()
        End If
    End Sub

    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
                      "alertify.alert('" & s & "')" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub

    Public Sub bindCheckBox()
        Dim SqlServiceStatus As String
        SqlServiceStatus = "Select Status from PaymentChannels where PaymentName like '" & DropDownList1.SelectedItem.Text & "' "
        da = New SqlDataAdapter(SqlServiceStatus, cn)
        ds = New DataSet()
        da.Fill(ds, "status")
        isActive.Checked = ds.Tables("status").Rows(0).Item(0)
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        bindCheckBox()
    End Sub

    Public Sub updateServiceStatus(ByVal PaymentName As String, ByVal status As Boolean)
        Dim SqlUpdate As String
        Try
            SqlUpdate = "update PaymentChannels set Status=" & Val(status) & " where PaymentName like '" & PaymentName & "'; INSERT INTO LogTrace  (Page,Service,PaymentChannel,status,ChangedBy,CreatedOn,info1,info2) VALUES('PaymentChannels','','" & PaymentName & "'," & Val(status) & ",'" & Session("user") & "',getdate(),'" & Request.UserHostAddress & "','" & txtReason.Text & "')"
            Dim cmd As SqlCommand
            cmd = New SqlCommand(SqlUpdate, cn)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim inset As Integer = cmd.ExecuteNonQuery()
            If (inset = 1) Then
                alert("Payment Channel Status Updated Successfully")
                txtReason.Text = String.Empty
            End If
        Catch ex As Exception
            alert("An error occured while updating service status")
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations") Then
            alert("You are not authorized to perform this action.")
            Exit Sub
        End If

        If (txtReason.Text = Nothing Or txtReason.Text = "") Then
            alert("Reason can't be empty")
            Exit Sub
        End If

        updateServiceStatus(DropDownList1.SelectedItem.Text, isActive.Checked)
    End Sub
End Class
