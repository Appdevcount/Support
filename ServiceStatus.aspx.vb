Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.NetworkInformation

Partial Class ServiceStatus
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim Sql As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "operations" Or Session("role") = "accounts") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=ServiceStatus.aspx")
        End If

        'If Not (Session("user") = "Shareef" Or Session("user") = "AdmiN" Or Session("user") = "AdmiN1" Or Session("user") = "AdmiN2") Then
        '    alert("You don't have privilage to access this page!!! Please contact administrator!!")
        '    Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
        '    Response.Redirect("~/login.aspx?ReturnURL=ServiceStatus.aspx")
        'End If
        cn = New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")
        If Not IsPostBack Then
            Sql = "Select ServiceCode from PayitServices where (Type='PIN' or Type='Payment' or Type='Exchange' or Type='Recharge' or Type='ZakatProject') And (ServiceCode not like '%TravelMate%' and ServiceCode NOT LIKE 'iTunesB-O%') order by ServiceCode"
            da = New SqlDataAdapter(Sql, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")
            DropDownList1.DataSource = ds.Tables("deta")
            DropDownList1.DataTextField = "ServiceCode"
            DropDownList1.DataValueField = "ServiceCode"
            DropDownList1.DataBind()
            DropDownList1.Items.Insert(0, New ListItem("Select", "None"))
        End If

    End Sub
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If (txtReason.Text = Nothing Or txtReason.Text = "") Then
            alertme("Reason can't be empty")
            Exit Sub
        End If
        If (DropDownList1.SelectedItem.Text = "Select" Or DropDownList1.SelectedValue = "None") Then
            alertme("Select service and then update")
            Exit Sub
        End If
        updateServiceStatus(DropDownList1.SelectedItem.Text, isActive.Checked, isActiveNew.Checked)
    End Sub
    Public Sub updateServiceStatus(ByVal ServiceCode As String, ByVal status As Boolean, ByVal statusNew As Boolean)
        Dim SqlUpdate As String
        Dim stat, statnew As Integer
        If (isActive.Checked = True) Then
            stat = 1
        Else
            stat = 0
        End If

        If (isActiveNew.Checked = True) Then
            statnew = 1
        Else
            statnew = 0
        End If
        Try
            SqlUpdate = "update PayitServices set Status=" & stat & ", StatusNew=" & statnew & " where ServiceCode like '" & ServiceCode & "'; INSERT INTO LogTrace  (Page,Service,PaymentChannel,info2,ChangedBy,CreatedOn,info1) VALUES('ServiceStatus','" & ServiceCode & "','Status: " & stat & " | StatusNew: " & statnew & "','" & txtReason.Text & "','" & Session("user") & "',getdate(),'" & Session("ip") & "')"
            Dim cmd As SqlCommand
            cmd = New SqlCommand(SqlUpdate, cn)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim integ As Integer = cmd.ExecuteNonQuery()
            If (integ = 2) Then
                alert("Service Status Updated Successfully")
                txtReason.Text = String.Empty
            End If
        Catch ex As Exception
            alert("An error occured while updating service status !!")
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
    Public Sub alertme(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.success('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        bindCheckBox()
    End Sub
    Public Sub bindCheckBox()
        Dim SqlServiceStatus As String
        SqlServiceStatus = "Select Status, StatusNew from PayitServices where ServiceCode like '" & DropDownList1.SelectedItem.Text & "' "
        da = New SqlDataAdapter(SqlServiceStatus, cn)
        ds = New DataSet()
        da.Fill(ds, "status")

        isActive.Checked = ds.Tables("status").Rows(0).Item(0)
        isActiveNew.Checked = ds.Tables("status").Rows(0).Item(1)
        If (isActive.Checked) Then
            lblStatus.Text = "Active"
        Else
            lblStatus.Text = "Inactive"
        End If

        If (isActiveNew.Checked) Then
            lblStatusNew.Text = "Active"
        Else
            lblStatusNew.Text = "Inactive"
        End If
    End Sub
End Class
