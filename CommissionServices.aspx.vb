Imports System.Data.SqlClient
Imports System.Data
Partial Class CommissionServices
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sql, sql2, strSQL As String
    Dim dispAmnt, info1 As String
    Dim sqlLocalTrans As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "cs" Or Session("role") = "operations" Or Session("role") = "qa") Then
            alert("Unauthorized Access")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=CommissionServices.aspx")
        End If

        If Not IsPostBack Then
            Me.BindData()
            GridView1.DataBind()
            cn = New SqlConnection(strConnString)
            sql = "SELECT DISTINCT ServiceCode FROM [payit].[dbo].[ServicesAndPayments]"
            da = New SqlDataAdapter(sql, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")
            ddlServType.DataSource = ds.Tables("deta")
            ddlServType.DataTextField = "ServiceCode"
            ddlServType.DataValueField = "ServiceCode"
            ddlServType.DataBind()
            ddlServType.Items.Insert(0, New ListItem("Select", "None"))

            ddlServiceEdit.DataSource = ds.Tables("deta")
            ddlServiceEdit.DataTextField = "ServiceCode"
            ddlServiceEdit.DataValueField = "ServiceCode"
            ddlServiceEdit.DataBind()
            ddlServiceEdit.Items.Insert(0, New ListItem("Select", "None"))

            sql2 = "SELECT DISTINCT PaymentName FROM [payit].[dbo].[ServicesAndPayments]"
            da = New SqlDataAdapter(sql2, cn)
            ds = New DataSet()
            da.Fill(ds, "data")
            ddlPaymentCode.DataSource = ds.Tables("data")
            ddlPaymentCode.DataTextField = "PaymentName"
            ddlPaymentCode.DataValueField = "PaymentName"
            ddlPaymentCode.DataBind()
            ddlPaymentCode.Items.Insert(0, New ListItem("Select Payment", "None"))

            ddlPaymentEdit.DataSource = ds.Tables("data")
            ddlPaymentEdit.DataTextField = "PaymentName"
            ddlPaymentEdit.DataValueField = "PaymentName"
            ddlPaymentEdit.DataBind()
            ddlPaymentEdit.Items.Insert(0, New ListItem("Select Payment", "None"))

            ddlCommType.Items.Insert(0, New ListItem("Select", "None"))
            ddlCommType.Items.Insert(1, New ListItem("FIXED", "FIXED"))
            ddlCommType.Items.Insert(2, New ListItem("PERCENT", "PERCENT"))

            ddlCommTypeEdit.Items.Insert(0, New ListItem("Select", "None"))
            ddlCommTypeEdit.Items.Insert(1, New ListItem("FIXED", "FIXED"))
            ddlCommTypeEdit.Items.Insert(2, New ListItem("PERCENT", "PERCENT"))

            ddlThreshold.Items.Insert(0, New ListItem("Select", "None"))
            ddlThreshold.Items.Insert(1, New ListItem("LESS THAN", "1"))
            ddlThreshold.Items.Insert(2, New ListItem("GREATER THAN", "2"))

            ddlThresholdEdit.Items.Insert(0, New ListItem("Select", "None"))
            ddlThresholdEdit.Items.Insert(1, New ListItem("LESS THAN", "1"))
            ddlThresholdEdit.Items.Insert(2, New ListItem("GREATER THAN", "2"))

        End If
    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If Not (Session("role") = "superadmin" Or Session("role") = "operations" Or Session("role") = "accounts" Or Session("role") = "qa") Then
            alert("Unauthorized Access")
            Exit Sub
        End If

        If Page.IsValid Then
            Dim cn As SqlConnection = New SqlConnection(strConnString)
            If (ddlServType.SelectedIndex = 0 Or ddlPaymentCode.SelectedIndex = 0 Or txtCommission.Text = "" Or ddlThreshold.SelectedIndex = 0) Then
                alert("Please fill all fields!")
                'dberrorlabel.Text = "Please Fill all fields!"
            Else
                Dim catStat, chkMan As String
                If chkStatus.Checked = True Then
                    catStat = 1
                Else
                    catStat = 0
                End If
                If chkMandatory.Checked = True Then
                    chkMan = 1
                Else
                    chkMan = 0
                End If
                Dim sql1 As String = "SELECT * FROM [PayitServicesPaymentsCommissions] WHERE ServiceCode=@ServiceCode and PaymentCode=@PaymentCode and [CommissionType]=@CommissionType and [CommissionValue]=@commVal and [Threshold]=@Threshold and [ThresholdType]=@ThresholdType"
                Dim cmd1 As SqlCommand = New SqlCommand(sql1, cn)
                cmd1.Connection = cn
                cn.Open()
                cmd1.Parameters.AddWithValue("@ServiceCode", ddlServType.SelectedItem.Text)
                cmd1.Parameters.AddWithValue("@PaymentCode", ddlPaymentCode.SelectedItem.Text)
                cmd1.Parameters.AddWithValue("@CommissionType", ddlCommType.SelectedItem.Text)
                cmd1.Parameters.AddWithValue("@commVal", txtCommission.Text)
                cmd1.Parameters.AddWithValue("@Threshold", txtThreshold.Text)
                cmd1.Parameters.AddWithValue("@ThresholdType", ddlThreshold.SelectedValue)
                Using reader As SqlDataReader = cmd1.ExecuteReader()
                    If reader.HasRows Then
                        ' Record already exists
                        alertme("Commission Already exists for:<br><b> " & ddlServType.SelectedItem.Text & " - " & ddlPaymentCode.SelectedItem.Text & "</b><br> CommissionType: <b>" & ddlCommType.SelectedItem.Text & "</b><br> ThresholdType:<b> " & ddlThreshold.SelectedItem.Text & "</b><br> ThresholdValue: <b>" & txtThreshold.Text & "</b>")
                    Else
                        ' Record does not exist, add them
                        Dim cmd As SqlCommand = New SqlCommand("INSERT INTO [payit].[dbo].[PayitServicesPaymentsCommissions]([ServiceCode],[PaymentCode],[CommissionType],[CommissionValue],[Threshold],[ThresholdType],[CreatedDate],[Status],[IsMandatory],[CommissionMsg]) VALUES ('" & ddlServType.SelectedItem.Text & "','" & ddlPaymentCode.SelectedItem.Text & "','" & ddlCommType.SelectedItem.Text & "'," & txtCommission.Text.Trim() & ",'" & txtThreshold.Text & "','" & ddlThreshold.SelectedValue & "',getdate()," & catStat & "," & chkMan & ",'" & txtCommissionMessage.Text & "')")
                        cmd.Connection = cn
                        Dim insert As Integer = cmd.ExecuteNonQuery()
                        If (insert = 1) Then
                            alert("Commission successfully added for:" & ddlServType.SelectedItem.Text & " - " & ddlPaymentCode.SelectedItem.Text & "!")
                        Else
                            alertme("Error! Try Again")
                        End If
                    End If
                End Using
                cn.Close()
                Clear()
                Me.BindData()
            End If
        End If
    End Sub
    Protected Sub Clear()
        txtCommission.Text = String.Empty
        txtThreshold.Text = String.Empty
        ddlServType.SelectedIndex = 0
        ddlCommType.SelectedIndex = 0
        ddlPaymentCode.SelectedIndex = 0
        ddlThreshold.SelectedIndex = 0
        chkStatus.Checked = False
        chkMandatory.Checked = False
        txtCommissionMessage.Text = String.Empty
    End Sub
    Private Sub BindData()
        Dim strQuery As String = ("select TOP 1000 CommissionID, ServiceCode, PaymentCode, CommissionType, CommissionValue, Threshold, ThresholdType, Status, IsMandatory, [CommissionMsg] from [PayitServicesPaymentsCommissions] order by CommissionID desc")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        Me.BindData()
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataBind()
    End Sub
    Public Sub sqldatabind(ByVal s As String, ByVal e As GridViewRowEventArgs, Optional ByVal types1 As String = "", Optional ByVal type As String = "")
        da = New SqlDataAdapter()
        ds = New DataSet()
        da.Fill(ds, "det")
        GridView1.DataSource = ds.Tables("det")
        GridView1.DataBind()
    End Sub
    Private Function GetData(ByVal cmd As SqlCommand) As DataTable
        Dim dt As DataTable = New DataTable
        Dim con As SqlConnection = New SqlConnection(strConnString)
        Dim sda As SqlDataAdapter = New SqlDataAdapter
        cmd.Connection = con
        con.Open()
        sda.SelectCommand = cmd
        sda.Fill(dt)
        Return dt
    End Function
    Protected Sub Edit(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        txtID.Text = row.Cells(0).Text

        If row.Cells(1).Text = "None" Or row.Cells(1).Text = "Select" Or row.Cells(1).Text = "&nbsp;" Or row.Cells(1).Text = "" Then
            ddlServiceEdit.SelectedIndex = 0
        ElseIf (row.Cells(1).Text.ToLower.Contains("&amp;")) Then
            Dim service = row.Cells(1).Text.Replace("&amp;", "&")
            ddlServiceEdit.SelectedValue = service
        Else
            ddlServiceEdit.SelectedValue = row.Cells(1).Text
        End If

        If row.Cells(2).Text = "None" Or row.Cells(2).Text = "Select Payment" Or row.Cells(2).Text = "&nbsp;" Or row.Cells(2).Text = "" Then
            ddlPaymentEdit.SelectedIndex = 0
        Else
            ddlPaymentEdit.SelectedValue = row.Cells(2).Text
        End If

        If row.Cells(3).Text = "None" Or row.Cells(3).Text = "Select" Or row.Cells(3).Text = "&nbsp;" Or row.Cells(3).Text = "" Then
            ddlCommTypeEdit.SelectedIndex = 0
        Else
            ddlCommTypeEdit.SelectedValue = row.Cells(3).Text
        End If

        If row.Cells(4).Text = "None" Or row.Cells(4).Text = "&nbsp;" Or row.Cells(4).Text = "" Then
            txtCommissionEdit.Text = ""
        Else
            txtCommissionEdit.Text = row.Cells(4).Text
        End If
        If row.Cells(5).Text = "None" Or row.Cells(5).Text = "&nbsp;" Or row.Cells(5).Text = "" Then
            txtThresholdEdit.Text = ""
        Else
            txtThresholdEdit.Text = row.Cells(5).Text
        End If
        If row.Cells(6).Text = "None" Or row.Cells(6).Text = "Select" Or row.Cells(6).Text = "&nbsp;" Or row.Cells(6).Text = "" Then
            ddlThresholdEdit.SelectedIndex = 0
        Else
            ddlThresholdEdit.SelectedValue = row.Cells(6).Text
        End If

        If row.Cells(9).Text = "True" Then
            chkStatEdit.Checked = True
        Else
            chkStatEdit.Checked = False
        End If

        If row.Cells(10).Text = "True" Then
            chkManEdit.Checked = True
        Else
            chkManEdit.Checked = False
        End If

        If row.Cells(11).Text = "None" Or row.Cells(11).Text = "&nbsp;" Or row.Cells(11).Text = "" Then
            txtCommMsgEdit.Text = ""
        Else
            txtCommMsgEdit.Text = row.Cells(11).Text
        End If
        'txtPriority.Text = row.Cells(7).Text
        popup.Show()
        alert("You can now edit")
    End Sub
    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        Dim catStatEdit, chkMandatoryEdit As Integer
        If (ddlServiceEdit.SelectedIndex = 0 Or ddlPaymentEdit.SelectedIndex = 0 Or txtCommissionEdit.Text = "") Then
            alert("Please fill all fields!")
            'dberrorlabel.Text = "Please Fill all fields!"
        Else
            If chkStatEdit.Checked = True Then
                catStatEdit = 1
            Else
                catStatEdit = 0
            End If

            If chkManEdit.Checked = True Then
                chkMandatoryEdit = 1
            Else
                chkMandatoryEdit = 0
            End If

            Using con As New SqlConnection(strConnString)
                Using cmd As New SqlCommand("UPDATE [payit].[dbo].[PayitServicesPaymentsCommissions] SET [ServiceCode] = '" & ddlServiceEdit.SelectedItem.Text & "', [PaymentCode] = '" & ddlPaymentEdit.SelectedItem.Text & "', [CommissionType] = '" & ddlCommTypeEdit.SelectedItem.Text & "', [CommissionValue] = " & txtCommissionEdit.Text & ", [Threshold] = '" & txtThresholdEdit.Text & "', [ThresholdType] = " & ddlThresholdEdit.SelectedValue & ", Status = " & catStatEdit & ", [IsMandatory] = " & chkMandatoryEdit & ", [CommissionMsg]= '" & txtCommMsgEdit.Text & "'  WHERE CommissionID = " & txtID.Text.Trim())
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridView1.DataBind()
                        End Using
                    End Using
                End Using
                Me.BindData()
                alert("Updated Successfully")
            End Using
        End If
    End Sub
    Protected Sub Delete(ByVal sender As Object, ByVal e As EventArgs)
        Using con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand("DELETE FROM [payit].[dbo].[PayitServicesPaymentsCommissions] WHERE CommissionID = " & txtID.Text.Trim())
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GridView1.DataBind()
                    End Using
                End Using
            End Using
            Me.BindData()
        End Using
        alert("Deleted Commission")
    End Sub
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Public Sub alertme(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
                      "alertify.alert('" & s & "')" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("CommissionServices.xls", Me.GridView1)
    End Sub
End Class
