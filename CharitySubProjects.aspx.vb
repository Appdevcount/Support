Imports System.Data.SqlClient
Imports System.Data
Imports Data

Partial Class CharitySubProjects
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim da As SqlDataAdapter
    Dim ds, ds1 As DataSet
    Dim db As New payitEntities
    Dim sql As String
    Dim s As String
    Dim username As String = String.Empty
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "thirdparty") Then
            alert("You do not have authorization to access this page. Contact Administrator for support.")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=CharitySubProjects.aspx")
        End If
        If Session("role") = "superadmin" Or Session("role") = "accounts" Then
            username = ""
        ElseIf Session("user").ToString.ToLower() = "turathislamy" Then
            username = "Turath-C"
        Else
            username = Session("user")
        End If
        If Not Page.IsPostBack Then
            Try
                Me.BindData()
                cn = New SqlConnection(strConnString)

                sql = "Select DISTINCT Service from AlrahmaCharity Where Service like '" & username & "%' "
                da = New SqlDataAdapter(sql, cn)
                ds = New DataSet()
                da.Fill(ds, "deta")
                ddlCharityServiceName.DataSource = ds.Tables("deta")
                ddlCharityServiceName.DataTextField = "Service"
                ddlCharityServiceName.DataValueField = "Service"
                ddlCharityServiceName.DataBind()
                ddlCharityServiceName.Items.Insert(0, New ListItem("Select", ""))

                Dim query = "select ID, ProjectName from AlrahmaCharity where Service like '" & username & "%'"
                da = New SqlDataAdapter(query, cn)
                ds = New DataSet()
                da.Fill(ds, "data")
                ddlProjectNameEdit.DataSource = ds.Tables("data")
                ddlProjectNameEdit.DataTextField = "ProjectName"
                ddlProjectNameEdit.DataValueField = "ID"
                ddlProjectNameEdit.DataBind()
                ddlProjectNameEdit.Items.Insert(0, New ListItem("Select", ""))
            Catch ex As Exception
                dberrorlabel.Text = ex.ToString
            End Try

        End If
    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If Not (Session("role") = "superadmin" Or Session("role") = "operations" Or Session("role") = "thirdparty") Then
            alert("You do not have authorization to access this page. Contact Administrator for support.")
            Exit Sub
        End If
        If Page.IsValid Then
            Dim cn As SqlConnection = New SqlConnection(strConnString)

            If (ddlProjectName.SelectedItem.Text = "Select" Or charitySubName.Text = "" Or charitySubDesc.Text = "" Or charitySubPriority.Text = "") Then
                alert("Please fill all fields!")
            Else
                Dim ProjectType As Integer
                ProjectType = 2

                Dim catStat As String
                If chkStatus.Checked = True Then
                    catStat = 1
                Else
                    catStat = 0
                End If
                Dim sql1 As String = "SELECT * FROM CharitySubProjects WHERE SubprojectName=@SubprojectName"
                Dim cmd1 As SqlCommand = New SqlCommand(sql1, cn)
                cmd1.Connection = cn
                cn.Open()
                cmd1.Parameters.AddWithValue("@SubprojectName", charitySubName.Text)
                Using reader As SqlDataReader = cmd1.ExecuteReader()
                    If reader.HasRows Then
                        ' Record already exists
                        alert("Sub Project Already Exist!")
                    Else
                        ' Record does not exist, add them
                        Dim cmd As SqlCommand = New SqlCommand("INSERT INTO CharitySubProjects ([ProjectType],[ProjectID],[SubprojectName],[Description],[Status],[Priority]) VALUES (" & ProjectType & ",'" & ddlProjectName.SelectedValue & "','" & charitySubName.Text.Trim() & "','" & charitySubDesc.Text.Trim() & "','" & catStat & "','" & charitySubPriority.Text & "')")
                        cmd.Connection = cn
                        Dim insert As Integer = cmd.ExecuteNonQuery()
                        If (insert = 1) Then
                            alert("Sub Project Successfully Added!")
                        Else
                            alert("Error! Try Again")
                        End If
                    End If
                End Using
                cn.Close()
                Clear()
                Me.BindData()
            End If
        End If

    End Sub
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Protected Sub Clear()
        charitySubName.Text = String.Empty
        charitySubDesc.Text = String.Empty
        charitySubPriority.Text = String.Empty
        chkStatus.Checked = False
    End Sub
    Private Sub BindDropDownList(ddl As DropDownList, query As String, text As String, value As String, defaultText As String)
        Dim cmd As New SqlCommand(query)
        Using con As New SqlConnection(strConnString)
            Using sda As New SqlDataAdapter()
                cmd.Connection = con
                con.Open()
                ddl.DataSource = cmd.ExecuteReader()
                ddl.DataTextField = text
                ddl.DataValueField = value
                ddl.DataBind()
                con.Close()
            End Using
        End Using
        ddl.Items.Insert(0, New ListItem(defaultText, "0"))
    End Sub
    Protected Sub ddlCharityServiceName_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlProjectName.Enabled = False
        ddlProjectName.Items.Clear()
        ddlProjectName.Items.Insert(0, New ListItem("Select Project", "0"))
        Dim serviceId As String = ddlCharityServiceName.SelectedItem.Text
        If serviceId <> Nothing Then
            Dim query As String = String.Format("select ID, ProjectName from AlrahmaCharity where Service like '{0}%'", serviceId)
            BindDropDownList(ddlProjectName, query, "ProjectName", "ID", "Select")
            ddlProjectName.Enabled = True
        End If
        If (ddlCharityServiceName.SelectedItem.Text = "Select") Then
            ddlProjectName.Enabled = False
            ddlProjectName.Items.Clear()
        End If
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
    Private Sub BindData()
        Dim strQuery As String = ("SELECT csp.ID, ac.ProjectName, csp.SubprojectName, csp.Description, csp.Status,csp.Priority, ac.Service " & _
                                  "FROM [payit].[dbo].[CharitySubProjects] csp " & _
                                  "LEFT JOIN payit.dbo.AlrahmaCharity ac on csp.ProjectID = ac.ID " & _
                                  " Where ac.Service like '" & username & "%' order by csp.Priority desc")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
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
        Dim rows = (From c In db.CharitySubProjects Where c.ID = txtID.Text.Trim() Select c).FirstOrDefault()

        ddlProjectNameEdit.SelectedValue = rows.ProjectID
        txtSubProjectNameEdit.Text = rows.SubprojectName
        txtSubProjectDescEdit.Text = rows.Description
        txtPriorityEdit.Text = rows.Priority
        chkStatEdit.Checked = rows.Status
        popup.Show()
        alert("You can now edit")
    End Sub
    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        Dim catStatEdit As Integer
        If (txtSubProjectNameEdit.Text = "") Then
            alert("Please fill all fields!")

        Else
            If chkStatEdit.Checked = True Then
                catStatEdit = 1
            Else
                catStatEdit = 0
            End If

            Using con As New SqlConnection(strConnString)
                Using cmd As New SqlCommand("UPDATE [payit].[dbo].[CharitySubProjects] SET [SubprojectName] = '" & txtSubProjectNameEdit.Text.Trim() & "', [Description] = '" & txtSubProjectDescEdit.Text.Trim() & "', Priority = " & txtPriorityEdit.Text.Trim() & ", Status = " & catStatEdit & " WHERE ID = " & txtID.Text.Trim())
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
            Using cmd As New SqlCommand("DELETE FROM [payit].[dbo].[CharitySubProjects] WHERE ID = " & txtID.Text.Trim())
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
        alert("Deleted SubProject")
    End Sub
    Protected Sub TryAgain(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect(Request.RawUrl)
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("CharitySubProjects.xls", Me.GridView1)
    End Sub
End Class
