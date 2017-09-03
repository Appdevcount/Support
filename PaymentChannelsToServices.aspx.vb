Imports System.Data
Imports System.Data.SqlClient

Partial Class PaymentChannelsToServices
    Inherits System.Web.UI.Page

    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim Sql As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=PaymentChannelsToServices.aspx")
        End If

        cn = New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")
        If Not IsPostBack Then
            Sql = "Select ServiceCode from PayitServices where (ServiceCode NOT LIKE '%Almullah%' AND ServiceCode NOT LIKE '%iTunesB-O%') order by ServiceCode"
            da = New SqlDataAdapter(Sql, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")
            DropDownList1.DataSource = ds.Tables("deta")
            DropDownList1.DataTextField = "ServiceCode"
            DropDownList1.DataValueField = "ServiceCode"
            DropDownList1.DataBind()
            DropDownList1.Items.Insert(0, New ListItem("Select", "Select"))
        End If

        If Page.IsPostBack = False Then
            bindCheckBox()
        End If
    End Sub

    Public Sub updateServiceStatus(ByVal ServiceCode As String, ByVal PaymentChannel As String, ByVal status As Boolean)

        Dim SqlUpdate As String
        Try
            SqlUpdate = "update ServicesAndPayments set Status=" & Val(status) & " where ServiceCode like '" & ServiceCode & "' and PaymentName like '" & PaymentChannel & "'; INSERT INTO LogTrace  (Page,Service,PaymentChannel,status,ChangedBy,CreatedOn,info1,info2) VALUES('PaymentChannels','" & ServiceCode & "','" & PaymentChannel & "'," & Val(status) & ",'" & Session("user") & "',getdate(),'" & Request.UserHostAddress & "','" & txtReason.Text & "')"
            Dim cmd As SqlCommand
            cmd = New SqlCommand(SqlUpdate, cn)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim inset As Integer = cmd.ExecuteNonQuery()
            If (inset = 1) Then
                alert("Service Status Updated Successfully")
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
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
                      "alertify.alert('" & s & "')" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub

    Public Sub bindCheckBox()
        Dim SqlServiceStatus As String
        SqlServiceStatus = "Select Status, PaymentName from ServicesAndPayments where ServiceCode like '" & DropDownList1.SelectedItem.Text & "' "
        da = New SqlDataAdapter(SqlServiceStatus, cn)
        ds = New DataSet()
        da.Fill(ds, "status")

        For Each r In ds.Tables(0).Rows
            If r.Item(1).ToString.ToLower.Equals("knet") Then
                KnetIsActive.Checked = r.Item(0)
            ElseIf r.Item(1).ToString.ToLower.Equals("creditcard") Then
                CreditCardIsActive.Checked = r.Item(0)
            ElseIf r.Item(1).ToString.ToLower.Equals("cashu") Then
                CashUIsActive.Checked = r.Item(0)
            ElseIf r.Item(1).ToString.ToLower.Equals("amex") Then
                AMEXIsActive.Checked = r.Item(0)
            ElseIf r.Item(1).ToString.ToLower.Equals("payitcc") Then
                PayitIsActive.Checked = r.Item(0)
            ElseIf r.Item(1).ToString.ToLower.Equals("wallet") Then
                WalletIsActive.Checked = r.Item(0)
            End If
        Next

    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        bindCheckBox()
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        updateServiceStatus(DropDownList1.SelectedItem.Text, Label3.Text.Substring(0, Label3.Text.IndexOf(":") - 1), KnetIsActive.Checked)
        updateServiceStatus(DropDownList1.SelectedItem.Text, Label4.Text.Substring(0, Label4.Text.IndexOf(":") - 1), CreditCardIsActive.Checked)
        updateServiceStatus(DropDownList1.SelectedItem.Text, Label5.Text.Substring(0, Label5.Text.IndexOf(":") - 1), CashUIsActive.Checked)
        updateServiceStatus(DropDownList1.SelectedItem.Text, Label1.Text.Substring(0, Label1.Text.IndexOf(":") - 1), AMEXIsActive.Checked)
        updateServiceStatus(DropDownList1.SelectedItem.Text, Label7.Text.Substring(0, Label7.Text.IndexOf(":") - 1), PayitIsActive.Checked)
        updateServiceStatus(DropDownList1.SelectedItem.Text, Label8.Text.Substring(0, Label8.Text.IndexOf(":") - 1), WalletIsActive.Checked)
    End Sub
End Class