Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Public Class CatServConfig
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sql, sql1 As String
    Dim sqlLocalTrans As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "qa") Then
            alert("You do not have authorization to access this page. Contact Administrator for support.")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=CatServConfig.aspx")
        End If

        If Not IsPostBack Then
            cn = New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")

            sql = "Select ID, ServiceName from PayitServices"
            sql1 = "Select ID, CategoryName from PayitCategories"

            da = New SqlDataAdapter(sql, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")

            ddlService.DataSource = ds.Tables("deta")
            ddlService.DataTextField = "ServiceName"
            ddlService.DataValueField = "ID"
            ddlService.DataBind()
            ddlService.Items.Insert(0, New ListItem("Select", "None"))

            ddlServiceEdit.DataSource = ds.Tables("deta")
            ddlServiceEdit.DataTextField = "ServiceName"
            ddlServiceEdit.DataValueField = "ID"
            ddlServiceEdit.DataBind()
            ddlServiceEdit.Items.Insert(0, New ListItem("None", "None"))

            da = New SqlDataAdapter(sql1, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")
            ddlCat.DataSource = ds.Tables("deta")
            ddlCat.DataTextField = "CategoryName"
            ddlCat.DataValueField = "ID"
            ddlCat.DataBind()
            ddlCat.Items.Insert(0, New ListItem("Select", "None"))

            ddlCatEdit.DataSource = ds.Tables("deta")
            ddlCatEdit.DataTextField = "CategoryName"
            ddlCatEdit.DataValueField = "ID"
            ddlCatEdit.DataBind()
            ddlCatEdit.Items.Insert(0, New ListItem("None", "None"))

            ddlStatus.Items.Insert(0, New ListItem("Select", "None"))
            ddlStatus.Items.Insert(1, New ListItem("Active", "1"))
            ddlStatus.Items.Insert(2, New ListItem("Inactive", "0"))

            ddlStatusEdit.Items.Insert(0, New ListItem("None", "None"))
            ddlStatusEdit.Items.Insert(1, New ListItem("Active", "1"))
            ddlStatusEdit.Items.Insert(2, New ListItem("Inactive", "0"))
        End If
    End Sub
    Private Sub BindData()
        Dim strQuery As String = ("Select * from PayitServicesCategories order by id desc")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
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
    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs)
    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If Not (Session("role") = "superadmin" Or Session("role") = "operations") Then
            alert("You do not have authorization to access this page. Contact Administrator for support.")
            Exit Sub
        End If
        Dim cn As SqlConnection = New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096;MultipleActiveResultSets=true;")
        If Page.IsValid Then
            If ddlCat.SelectedItem.Text = "" Or ddlService.SelectedItem.Text = "" Or ddlStatus.SelectedItem.Text = "" Then
                alert("Please select all fields!")
                'MsgBox("Please fill-up all fields!", MsgBoxStyle.Exclamation, "Add New Record!")
            Else
                Dim sql1 As String = "SELECT * FROM PayitServicesCategories WHERE PayitCategoryID=@PayitCategoryID AND PayitServicesID=@PayitServicesID"
                Dim cmd1 As SqlCommand = New SqlCommand(sql1, cn)
                cmd1.Connection = cn
                cn.Open()
                cmd1.Parameters.AddWithValue("@PayitCategoryID", ddlCat.SelectedValue)
                cmd1.Parameters.AddWithValue("@PayitServicesID", ddlService.SelectedValue)
                Using reader As SqlDataReader = cmd1.ExecuteReader()
                    If reader.HasRows Then
                        ' Record already exists
                        alert("Records Already Exist!")
                        'MsgBox("Records Already Exist!", MsgBoxStyle.Exclamation, "Add New Record!")
                    Else
                        ' Record does not exist, add them
                        Dim cmd As SqlCommand = New SqlCommand("INSERT INTO PayitServicesCategories (PayitCategoryID,PayitServicesID,Status,CreatedDate,PayitServicePriority) VALUES ('" & ddlCat.SelectedItem.Value & "','" & ddlService.SelectedItem.Value & "'," & ddlStatus.SelectedItem.Value & ",GETDATE(),(SELECT MAX(CONVERT(int, Priority))+1 from PayitCategories WHERE ID=" & ddlCat.SelectedItem.Value & "))")
                        cmd.Connection = cn
                        Dim insert As Integer = cmd.ExecuteNonQuery()
                        If (insert = 1) Then
                            alert("Configuration Successfully Added!")
                        Else
                            alert("Error! Try Again")
                        End If
                        'MsgBox("Records Successfully Added!", MsgBoxStyle.Information, "Add New Record")
                    End If
                End Using
                cn.Close()
            End If
        End If
    End Sub
    Protected Sub Edit(ByVal sender As Object, ByVal e As EventArgs)
        Dim servEdit As String
        Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        If row.Cells(6).Text = "None" Or row.Cells(6).Text = "&nbsp;" Or row.Cells(6).Text = "" Then
            ddlStatusEdit.SelectedValue = "None"
        ElseIf (row.Cells(6).Text = "True") Then
            ddlStatusEdit.SelectedIndex = 1
        ElseIf (row.Cells(6).Text = "False") Then
            ddlStatusEdit.SelectedIndex = 2
        End If

        txtID.Text = row.Cells(8).Text

        If row.Cells(3).Text = "None" Or row.Cells(3).Text = "&nbsp;" Or row.Cells(3).Text = "" Then
            ddlServiceEdit.SelectedIndex = 0
        Else
            ddlServiceEdit.SelectedValue = row.Cells(3).Text
            servEdit = ddlServiceEdit.SelectedValue
        End If

        If row.Cells(1).Text = "None" Or row.Cells(1).Text = "&nbsp;" Then
            ddlCatEdit.SelectedIndex = 0
        Else
            ddlCatEdit.SelectedValue = row.Cells(1).Text
        End If

        txtPriority.Text = row.Cells(7).Text
        popup.Show()
    End Sub
    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        Dim constring As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("UPDATE PayitServicesCategories SET PayitCategoryID = '" & ddlCatEdit.SelectedValue & "', PayitServicesID = '" & ddlServiceEdit.SelectedValue & "', Status = " & ddlStatusEdit.SelectedItem.Value & ", PayitServicePriority = '" & txtPriority.Text.Trim() & "' WHERE ID = " & txtID.Text.Trim())
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    If (con.State = ConnectionState.Closed) Then
                        con.Open()
                    End If
                    Dim insert As Integer = cmd.ExecuteNonQuery()
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GridView1.DataBind()
                    End Using
                    If (insert = 1) Then
                        alert("Configuration Updated")
                    Else
                        alert("Error. Try Again")
                    End If
                    If (con.State = ConnectionState.Open) Then
                        con.Close()
                    End If
                End Using
            End Using
        End Using
    End Sub
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("stats.xls", Me.GridView1)
    End Sub
End Class