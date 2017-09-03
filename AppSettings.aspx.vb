Imports System.Data.SqlClient
Imports System.Data
Imports Data

Partial Class AppSettings
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sql, sql2, strSQL As String
    Dim dispAmnt, info1 As String
    Dim sqlLocalTrans As String
    Dim db As New payitEntities()
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations") Then
            alert("Unauthorized Access")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=AppSettings.aspx")
        End If

        If Not IsPostBack Then
            ViewState("Filter") = "ALL"
            BindGrid()
            btnConfig.Visible = False
            cn = New SqlConnection(strConnString)
            sql = "SELECT DISTINCT myid,auser FROM [payit].[dbo].[users] WHERE acc_status like 'Active' order by myid desc"
            da = New SqlDataAdapter(sql, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")
            ddlUser.DataSource = ds.Tables("deta")
            ddlUser.DataTextField = "auser"
            ddlUser.DataValueField = "myid"
            ddlUser.DataBind()
            ddlUser.Items.Insert(0, New ListItem("Select", "None"))

            ddlUserEdit.DataSource = ds.Tables("deta")
            ddlUserEdit.DataTextField = "auser"
            ddlUserEdit.DataValueField = "myid"
            ddlUserEdit.DataBind()
            ddlUserEdit.Items.Insert(0, New ListItem("Select", "None"))

            Dim sql2 = (From c In db.PayitServices Order By c.ServiceCode Descending Select New With {Key .ServiceCode = c.ServiceCode, Key .ID = c.ID}).ToList()
            ddlServType.DataSource = sql2
            ddlServType.DataTextField = "ServiceCode"
            ddlServType.DataValueField = "ID"
            ddlServType.DataBind()
            ddlServType.Items.Insert(0, New ListItem("Select", "None"))

            ddlServiceEdit.DataSource = sql2
            ddlServiceEdit.DataTextField = "ServiceCode"
            ddlServiceEdit.DataValueField = "ID"
            ddlServiceEdit.DataBind()
            ddlServiceEdit.Items.Insert(0, New ListItem("Select", "None"))

            Dim sql3 = "SELECT DISTINCT u.auser, u.myid FROM users u RIGHT JOIN  " & _
                        "[payit].[dbo].[AppUserServices] a on u.myid = a.UsersID group by u.auser, a.CreatedDate, u.myid " & _
                        "order by u.myid desc"
            da = New SqlDataAdapter(sql3, cn)
            ds = New DataSet()
            da.Fill(ds, "datq")
            ddlConfig.DataSource = ds.Tables("datq")
            ddlConfig.DataTextField = "auser"
            ddlConfig.DataValueField = "myid"
            ddlConfig.DataBind()
            ddlConfig.Items.Insert(0, New ListItem("Select", "None"))

        End If
    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations") Then
            alert("Unauthorized Access")
            Exit Sub
        End If

        If Page.IsValid Then
            Dim cn As SqlConnection = New SqlConnection(strConnString)
            If (ddlServType.SelectedIndex = 0 Or ddlUser.SelectedIndex = 0 Or txtReason.Text = "") Then
                alert("Please fill all fields!")
            Else
                Dim catStat As Integer
                If chkStatus.Checked = True Then
                    catStat = 1
                Else
                    catStat = 0
                End If

                Dim sql1 As String = "SELECT * FROM [payit].[dbo].[AppUserServices] WHERE [ServiceID]=@ServiceCode and [UsersID]=@User"
                Dim cmd1 As SqlCommand = New SqlCommand(sql1, cn)
                cmd1.Connection = cn
                cn.Open()
                cmd1.Parameters.AddWithValue("@ServiceCode", ddlServType.SelectedValue)
                cmd1.Parameters.AddWithValue("@User", ddlUser.SelectedValue)

                Using reader As SqlDataReader = cmd1.ExecuteReader()
                    If reader.HasRows Then
                        ' Record already exists
                        alertme("Configuration Already exists for:<br><b> " & ddlServType.SelectedItem.Text & " - " & ddlUser.SelectedItem.Text & "</b>")
                    Else
                        ' Record does not exist, add them
                        Dim recordDate As DateTime = DateTime.Now
                        Dim cmd As SqlCommand = New SqlCommand("INSERT INTO [payit].[dbo].[AppUserServices]([ServiceID],[UsersID],[CreatedDate],[Status]) VALUES ('" & ddlServType.SelectedValue & "','" & ddlUser.SelectedValue & "','" & recordDate & "'," & catStat & ");" & _
                                                               "INSERT INTO LogTrace (Page,Service,PaymentChannel,info2,ChangedBy,CreatedOn,info1) VALUES('ServiceStatus','" & ddlServType.SelectedItem.Text & "','AppUser: " & ddlUser.SelectedItem.Text & ", Status: " & catStat & "','" & txtReason.Text & "','" & Session("user") & "',getdate(),'" & Session("ip") & "')")
                        cmd.Connection = cn
                        Dim insert As Integer = cmd.ExecuteNonQuery()
                        If (insert = 1) Then
                            alert("Configuration successfully added for:" & ddlServType.SelectedItem.Text & " - " & ddlUser.SelectedItem.Text & "!")
                        Else
                            alertme("Error! Try Again")
                        End If
                    End If
                End Using
                cn.Close()
                Clear()
                Me.BindGrid()
            End If
        End If
    End Sub

    Protected Sub btnConfig_Click(sender As Object, e As EventArgs) Handles btnConfig.Click
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations") Then
            alert("Unauthorized Access")
            Exit Sub
        End If

        If Page.IsValid Then
            Dim cn As SqlConnection = New SqlConnection(strConnString)

            Dim sql1 As String = "SELECT * FROM [payit].[dbo].[AppUserServices] WHERE [ServiceID]=@ServiceCode and [UsersID]=@User"
            Dim cmd1 As SqlCommand = New SqlCommand(sql1, cn)
            cmd1.Connection = cn
            cn.Open()
            cmd1.Parameters.AddWithValue("@ServiceCode", ddlServType.SelectedValue)
            cmd1.Parameters.AddWithValue("@User", ddlUser.SelectedValue)

            Using reader As SqlDataReader = cmd1.ExecuteReader()
                If reader.HasRows Then
                    ' Record already exists
                    alertme("Configuration Already exists for:<br><b> " & ddlServType.SelectedItem.Text & " - " & ddlUser.SelectedItem.Text & "</b>")
                Else
                    ' Record does not exist, add them
                    Dim recordDate As DateTime = DateTime.Now
                    Dim cmd As SqlCommand = New SqlCommand("INSERT INTO [payit].[dbo].[AppUserServices]([ServiceID],[UsersID],[CreatedDate],[Status])" & _
                                                            " SELECT [ServiceID]," & ddlUser.SelectedValue & ",GETDATE(),[Status] " & _
                                                            " FROM [payit].[dbo].[AppUserServices] AS t1 WHERE t1.UsersID = @User AND t1.ServiceID NOT IN " & _
                                                            " (SELECT ServiceID FROM [payit].[dbo].[AppUserServices] WHERE UsersID = " & ddlUser.SelectedValue & ")")
                    cmd.Connection = cn
                    cmd.Parameters.AddWithValue("@User", ddlConfig.SelectedValue)
                    Dim insert As Integer = cmd.ExecuteNonQuery()
                    If (insert = 1) Then
                        alert("Configuration successfully added for:" & ddlConfig.SelectedItem.Text & "")
                    Else
                        alertme("Error! Try Again")
                    End If
                End If
            End Using
            cn.Close()
            Clear()
            Me.BindGrid()

        End If
    End Sub

    Protected Sub Clear()
        ddlServType.SelectedIndex = 0
        ddlUser.SelectedIndex = 0
        chkStatus.Checked = False
    End Sub
    Private Sub BindGrid()
        Dim dt As New DataTable()

        Dim con As New SqlConnection(strConnString)
        Dim sda As New SqlDataAdapter()
        Dim cmd As New SqlCommand("FilterAppUsersForCMS")
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@Filter", ViewState("Filter"))
        cmd.Connection = con
        sda.SelectCommand = cmd
        sda.Fill(dt)
        GridView1.DataSource = dt
        GridView1.DataBind()
        Dim ddlCountry As DropDownList = DirectCast(GridView1.HeaderRow _
                .FindControl("ddlCountry"), DropDownList)
        Me.BindCountryList(ddlCountry)
    End Sub
    Private Sub BindCountryList(ByVal ddlCountry As DropDownList)
        'Dim Connect As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
        Dim Connect = New SqlConnection(strConnString)
        Dim sqlme = "SELECT DISTINCT myid,auser FROM [payit].[dbo].[users] WHERE acc_status like 'Active' order by myid desc"
        da = New SqlDataAdapter(sqlme, Connect)
        ds = New DataSet()
        da.Fill(ds, "detada")
        ddlCountry.DataSource = ds.Tables("detada")
        ddlCountry.DataTextField = "auser"
        ddlCountry.DataValueField = "myid"
        ddlCountry.DataBind()
        ddlCountry.Items.FindByValue(ViewState("Filter").ToString()).Selected = True
    End Sub
    Private Sub BindData()
        Dim strQuery As String = ("Select a.ID,a.UsersID,a.ServiceID, u.auser AppUser, p.ServiceCode Service, a.Status, a.CreatedDate FROM [payit].[dbo].[AppUserServices] AS a LEFT JOIN PayitServices p on a.ServiceID = p.ID Left Join users u on a.UsersID = u.myid " & _
                                    "order by a.ID desc")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
    End Sub
    'Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
    '    Me.BindData()
    '    GridView1.PageIndex = e.NewPageIndex
    '    GridView1.DataBind()
    'End Sub
    Protected Sub OnPaging(sender As Object, e As GridViewPageEventArgs)
        GridView1.PageIndex = e.NewPageIndex
        Me.BindGrid()
    End Sub
    Protected Sub CountryChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim ddlCountry As DropDownList = DirectCast(sender, DropDownList)
        ViewState("Filter") = ddlCountry.SelectedValue
        Me.BindGrid()
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
        txtID.Text = row.Cells(1).Text

        If row.Cells(2).Text = "None" Or row.Cells(2).Text = "Select" Or row.Cells(2).Text = "&nbsp;" Or row.Cells(2).Text = "" Then
            ddlUserEdit.SelectedIndex = 0
        Else
            ddlUserEdit.SelectedValue = row.Cells(2).Text
        End If

        If row.Cells(3).Text = "None" Or row.Cells(3).Text = "Select" Or row.Cells(3).Text = "&nbsp;" Or row.Cells(3).Text = "" Then
            ddlServiceEdit.SelectedIndex = 0
        Else
            ddlServiceEdit.SelectedValue = row.Cells(3).Text
        End If

        If row.Cells(4).Text = "True" Then
            chkStatEdit.Checked = True
        Else
            chkStatEdit.Checked = False
        End If
        'txtPriority.Text = row.Cells(7).Text
        popup.Show()
        alert("You can now edit")
    End Sub
    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        Dim catStatEdit As Integer
        If (ddlServiceEdit.SelectedIndex = 0) Then
            alert("Please fill all fields!")
            'dberrorlabel.Text = "Please Fill all fields!"
        Else
            If chkStatEdit.Checked = True Then
                catStatEdit = 1
            Else
                catStatEdit = 0
            End If

            Using con As New SqlConnection(strConnString)
                Using cmd As New SqlCommand("UPDATE [payit].[dbo].[AppUserServices] SET [ServiceID] = '" & ddlServiceEdit.SelectedValue & "', [UsersID] = '" & ddlUserEdit.SelectedValue & "', Status = " & catStatEdit & "  WHERE ID = " & txtID.Text.Trim() & ";" & _
                                            "INSERT INTO LogTrace (Page,Service,PaymentChannel,info2,ChangedBy,CreatedOn,info1) VALUES('ServiceStatus','" & ddlServiceEdit.SelectedItem.Text & "','AppUser: " & ddlUserEdit.SelectedItem.Text & ", Status: " & catStatEdit & "','" & txtReasonEdit.Text & "','" & Session("user") & "',getdate(),'" & Session("ip") & "')")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridView1.DataBind()
                        End Using
                    End Using
                End Using
                Me.BindGrid()
                alert("Updated Successfully")
            End Using
        End If
    End Sub
    Protected Sub Delete(ByVal sender As Object, ByVal e As EventArgs)
        Using con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand("DELETE FROM [payit].[dbo].[AppUserServices] WHERE ID = " & txtID.Text.Trim())
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GridView1.DataBind()
                    End Using
                End Using
            End Using
            Me.BindGrid()
        End Using
        alert("Deleted Config")
    End Sub
    Protected Sub TryAgain(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect(Request.RawUrl)
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
        ExportToExcel.Export("UserConfig.xls", Me.GridView1)
    End Sub
    Protected Sub ddlConfig_SelectedIndexChanged(sender As Object, e As EventArgs)
        If (ddlConfig.Items.Count > 0) Then
            If (ddlConfig.SelectedIndex > -1) Then
                If (ddlServType.SelectedIndex > 0) Then
                    ddlServType.SelectedIndex = 0
                End If

                btnSubmit.Visible = False
                btnConfig.Visible = True
                lblUser.Text = "Assign To"
                lblUser.CssClass = "button-primary pure-button"
                hideDiv.Visible = False
            End If
        End If
    End Sub
End Class
