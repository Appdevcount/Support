Imports System.Data.SqlClient
Imports System.Data
Imports Data

Partial Class alRahmaCharity
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim da As SqlDataAdapter
    Dim ds, ds1 As DataSet
    Dim sql As String
    Dim db As New payitEntities
    Dim s As String
    Dim username As String = String.Empty
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "thirdparty") Then
            alert("Unauthorized Access")
            Response.Redirect("~/login.aspx?ReturnURL=alRahmaCharity.aspx")
        End If

        If Session("role") = "superadmin" Or Session("role") = "accounts" Then
            username = "%"
        ElseIf Session("user").ToString.ToLower() = "turathislamy" Then
            username = "Turath-C"
        Else
            username = Session("user")
        End If
        If Not Page.IsPostBack Then
            Try
                Me.BindData()
                cn = New SqlConnection(strConnString)
                If (username.Equals("Turath-C")) Then
                    sql = "select DISTINCT Service from CharityUserServiceConfig where Service like '" & username & "'"
                Else
                    sql = "select DISTINCT Service from CharityUserServiceConfig where Username like '" & username & "'"
                End If


                da = New SqlDataAdapter(sql, cn)
                ds = New DataSet()
                da.Fill(ds, "deta")
                ddlServiceName.DataSource = ds.Tables("deta")
                ddlServiceName.DataTextField = "Service"
                ddlServiceName.DataValueField = "Service"
                ddlServiceName.DataBind()
                ddlServiceName.Items.Insert(0, New ListItem("Select", ""))
            Catch ex As Exception
                dberrorlabel.Text = ex.ToString
            End Try
        End If
    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If Not (Session("role") = "superadmin" Or Session("role") = "operations" Or Session("role") = "accounts" Or Session("role") = "thirdparty") Then
            alert("Unauthorized Access")
            Exit Sub
        End If
        If Page.IsValid Then
            Dim cn As SqlConnection = New SqlConnection(strConnString)
            If (ddlServiceName.SelectedIndex = 0 Or zakName.Text = "" Or zakPriority.Text = "" Or zakDesc.Text = "") Then
                alert("Please fill all fields!")
                'dberrorlabel.Text = "Please Fill all fields!"
            Else
                Dim catStat As String
                If chkStatus.Checked = True Then
                    catStat = 1
                Else
                    catStat = 0
                End If
                Dim sql1 As String = "SELECT * FROM AlrahmaCharity WHERE ProjectName=@ProjectName and Service=@Service"
                Dim cmd1 As SqlCommand = New SqlCommand(sql1, cn)
                cmd1.Connection = cn
                cn.Open()
                cmd1.Parameters.AddWithValue("@ProjectName", zakName.Text)
                cmd1.Parameters.AddWithValue("@Service", ddlServiceName.SelectedItem.Text)
                Using reader As SqlDataReader = cmd1.ExecuteReader()
                    If reader.HasRows Then
                        ' Record already exists
                        alert("Records Already Exist!")
                    Else
                        ' Record does not exist, add them
                        Dim cmd As SqlCommand = New SqlCommand("INSERT INTO AlrahmaCharity (ProjectName,Description,CreatedDate,Status,Priority,Service) VALUES ('" & zakName.Text.Trim() & "','" & zakDesc.Text.Trim() & "',GETDATE()," & catStat & "," & zakPriority.Text & ",'" & ddlServiceName.SelectedItem.Text & "')")
                        cmd.Connection = cn
                        Dim insert As Integer = cmd.ExecuteNonQuery()
                        If (insert = 1) Then
                            alert("Record Successfully Added!")
                        Else
                            alert("Error! Try Again")
                        End If
                    End If
                End Using
                cn.Close()
                Clear()
                Me.BindData()
                GridView1.DataBind()
            End If
        End If
    End Sub
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.error('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Private Sub BindData()
        Dim strQuery As String = ("select TOP 1000 [ID],[ProjectName],[Description],[Status],[Priority],[Service] from [AlrahmaCharity] Where Service like '" & username & "%' order by ID desc")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        Me.BindData()
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataBind()
    End Sub
    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = False
        End If
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
    'Protected Sub Edit(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
    '    txtID.Text = row.Cells(0).Text

    '    If row.Cells(1).Text = "None" Or row.Cells(1).Text = "Select" Or row.Cells(1).Text = "&nbsp;" Or row.Cells(1).Text = "" Then
    '        ddlServiceEdit.SelectedIndex = 0
    '    ElseIf (row.Cells(1).Text.ToLower.Contains("&amp;")) Then
    '        Dim service = row.Cells(1).Text.Replace("&amp;", "&")
    '        ddlServiceEdit.SelectedValue = service
    '    Else
    '        ddlServiceEdit.SelectedValue = row.Cells(1).Text
    '    End If

    '    If row.Cells(2).Text = "None" Or row.Cells(2).Text = "Select Payment" Or row.Cells(2).Text = "&nbsp;" Or row.Cells(2).Text = "" Then
    '        ddlPaymentEdit.SelectedIndex = 0
    '    Else
    '        ddlPaymentEdit.SelectedValue = row.Cells(2).Text
    '    End If

    '    If row.Cells(3).Text = "None" Or row.Cells(3).Text = "Select" Or row.Cells(3).Text = "&nbsp;" Or row.Cells(3).Text = "" Then
    '        ddlCommTypeEdit.SelectedIndex = 0
    '    Else
    '        ddlCommTypeEdit.SelectedValue = row.Cells(3).Text
    '    End If

    '    If row.Cells(4).Text = "None" Or row.Cells(4).Text = "&nbsp;" Or row.Cells(4).Text = "" Then
    '        txtCommissionEdit.Text = ""
    '    Else
    '        txtCommissionEdit.Text = row.Cells(4).Text
    '    End If
    '    If row.Cells(5).Text = "None" Or row.Cells(5).Text = "&nbsp;" Or row.Cells(5).Text = "" Then
    '        txtThresholdEdit.Text = ""
    '    Else
    '        txtThresholdEdit.Text = row.Cells(5).Text
    '    End If
    '    If row.Cells(6).Text = "None" Or row.Cells(6).Text = "Select" Or row.Cells(6).Text = "&nbsp;" Or row.Cells(6).Text = "" Then
    '        ddlThresholdEdit.SelectedIndex = 0
    '    Else
    '        ddlThresholdEdit.SelectedValue = row.Cells(6).Text
    '    End If

    '    If row.Cells(9).Text = "True" Then
    '        chkStatEdit.Checked = True
    '    Else
    '        chkStatEdit.Checked = False
    '    End If

    '    If row.Cells(10).Text = "True" Then
    '        chkManEdit.Checked = True
    '    Else
    '        chkManEdit.Checked = False
    '    End If

    '    If row.Cells(11).Text = "None" Or row.Cells(11).Text = "&nbsp;" Or row.Cells(11).Text = "" Then
    '        txtCommMsgEdit.Text = ""
    '    Else
    '        txtCommMsgEdit.Text = row.Cells(11).Text
    '    End If
    '    'txtPriority.Text = row.Cells(7).Text
    '    popup.Show()
    '    alert("You can now edit")
    'End Sub

    'Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim catStatEdit, chkMandatoryEdit As Integer
    '    If (ddlServiceEdit.SelectedIndex = 0 Or ddlPaymentEdit.SelectedIndex = 0 Or txtCommissionEdit.Text = "") Then
    '        alert("Please fill all fields!")
    '        'dberrorlabel.Text = "Please Fill all fields!"
    '    Else
    '        If chkStatEdit.Checked = True Then
    '            catStatEdit = 1
    '        Else
    '            catStatEdit = 0
    '        End If

    '        If chkManEdit.Checked = True Then
    '            chkMandatoryEdit = 1
    '        Else
    '            chkMandatoryEdit = 0
    '        End If

    '        Using con As New SqlConnection(strConnString)
    '            Using cmd As New SqlCommand("UPDATE [payit].[dbo].[PayitServicesPaymentsCommissions] SET [ServiceCode] = '" & ddlServiceEdit.SelectedItem.Text & "', [PaymentCode] = '" & ddlPaymentEdit.SelectedItem.Text & "', [CommissionType] = '" & ddlCommTypeEdit.SelectedItem.Text & "', [CommissionValue] = " & txtCommissionEdit.Text & ", [Threshold] = '" & txtThresholdEdit.Text & "', [ThresholdType] = " & ddlThresholdEdit.SelectedValue & ", Status = " & catStatEdit & ", [IsMandatory] = " & chkMandatoryEdit & ", [CommissionMsg]= '" & txtCommMsgEdit.Text & "'  WHERE CommissionID = " & txtID.Text.Trim())
    '                Using sda As New SqlDataAdapter()
    '                    cmd.Connection = con
    '                    sda.SelectCommand = cmd
    '                    Using dt As New DataTable()
    '                        sda.Fill(dt)
    '                        GridView1.DataBind()
    '                    End Using
    '                End Using
    '            End Using
    '            Me.BindData()
    '            alert("Updated Successfully")
    '        End Using
    '    End If
    'End Sub

    'Protected Sub Delete(ByVal sender As Object, ByVal e As EventArgs)
    '    Using con As New SqlConnection(strConnString)
    '        Using cmd As New SqlCommand("DELETE FROM [payit].[dbo].[PayitServicesPaymentsCommissions] WHERE CommissionID = " & txtID.Text.Trim())
    '            Using sda As New SqlDataAdapter()
    '                cmd.Connection = con
    '                sda.SelectCommand = cmd
    '                Using dt As New DataTable()
    '                    sda.Fill(dt)
    '                    GridView1.DataBind()
    '                End Using
    '            End Using
    '        End Using
    '        Me.BindData()
    '    End Using
    '    alert("Deleted Commission")
    'End Sub
    Protected Sub Edit(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        txtID.Text = row.Cells(0).Text
        Dim rows = (From c In db.AlrahmaCharities Where c.ID = txtID.Text.Trim() Select c).FirstOrDefault()

        txtProjectNameEdit.Text = rows.ProjectName
        txtProjectDescEdit.Text = rows.Description
        txtPriorityEdit.Text = rows.Priority
        chkStatEdit.Checked = rows.Status
        popup.Show()
        alert("You can now edit")
    End Sub
    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        Dim catStatEdit As Integer
        If (txtProjectNameEdit.Text = "") Then
            alert("Please fill all fields!")
        Else
            If chkStatEdit.Checked = True Then
                catStatEdit = 1
            Else
                catStatEdit = 0
            End If

            Using con As New SqlConnection(strConnString)
                Using cmd As New SqlCommand("UPDATE [payit].[dbo].[AlrahmaCharity] SET [ProjectName] = '" & txtProjectNameEdit.Text.Trim() & "', [Description] = '" & txtProjectDescEdit.Text.Trim() & "', Priority = " & txtPriorityEdit.Text.Trim() & ", Status = " & catStatEdit & " WHERE ID = " & txtID.Text.Trim())
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
            Using cmd As New SqlCommand("DELETE FROM [payit].[dbo].[AlrahmaCharity] WHERE ID = " & txtID.Text.Trim())
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
        alert("Deleted Project")
    End Sub
    Protected Sub Clear()
        zakName.Text = String.Empty
        zakDesc.Text = String.Empty
        zakPriority.Text = String.Empty
        chkStatus.Checked = False
        ddlServiceName.SelectedIndex = 0
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("CharityProjects.xls", Me.GridView1)
    End Sub
    Protected Sub TryAgain(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect(Request.RawUrl)
    End Sub

    
End Class