Imports System.Data.SqlClient
Imports System.Data

Partial Class alRahmaTrans
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sqlLocalTrans, Sql As String
    Private grdTotal As Decimal
    Dim priceTotal As Decimal = 0
    Dim fromdate, todate As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Dim typeS As String = ""
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        fromdate = Trim(FromDateTextBox.Text) & " 00:00:00"
        todate = Trim(ToDateTextBox.Text) & " 23:59:59"
        cn = New SqlConnection(strConnString)

        If (ddlServiceName.SelectedItem.Text = "Select") Then
            alert("Please Select a Charity Name before searching")
            Exit Sub
        End If
        sqlLocalTrans = "select zpt.ID,zpt.ProjectID,zpt.MobileNumber,zpt.EmailID,zpt.Amount,zpt.CreatedDate,zpt.TransactionStatus,zpt.TrackID,zpt.Status,zpt.Service,zp.ProjectName,cp.SubprojectName from payit.dbo.AlrahmaCharityTransactions zpt LEFT JOIN payit.dbo.AlrahmaCharity zp on zpt.ProjectID = zp.ID  left join CharitySubProjects cp on zpt.SubprojectId = cp.ID where zpt.CreatedDate between '" & fromdate & "' and '" & todate & "'"
        Dim tempSQL As String = ""
        If ddlServiceName.SelectedItem.Text <> "Select" And ddlServiceName.SelectedItem.Text <> "NULL" Then
            If tempSQL = "" Then
                tempSQL = "zpt.Service like '" & Trim(ddlServiceName.SelectedItem.Text) & "' "
            Else
                tempSQL = tempSQL & " and zpt.Service like '" & Trim(ddlServiceName.SelectedItem.Text) & "' "
            End If
        End If
        If ddlAlRahma.SelectedItem.Text <> "All" And ddlAlRahma.SelectedItem.Text <> "NULL" Then
            If tempSQL = "" Then
                tempSQL = "zp.ProjectName like '" & Trim(ddlAlRahma.SelectedItem.Text) & "' "
            Else
                tempSQL = tempSQL & " and zp.ProjectName like '" & Trim(ddlAlRahma.SelectedItem.Text) & "' "
            End If
        End If
        If (Not (ddlAlRahma.SelectedItem.Text = "All")) Then
            If dddlSubProject.SelectedItem.Text <> "All" And dddlSubProject.SelectedItem.Text <> "NULL" Then
                If tempSQL = "" Then
                    tempSQL = "zpt.SubprojectId ='" & Trim(dddlSubProject.SelectedValue) & "' "
                Else
                    tempSQL = tempSQL & " and zpt.SubprojectId = '" & Trim(dddlSubProject.SelectedValue) & "' "
                End If
            End If
        End If
       
        If Trim(TextMobile.Text) <> "" And Trim(TextMobile.Text) <> "NULL" Then
            If tempSQL = "" Then
                tempSQL = "zpt.MobileNumber like '" & Trim(TextMobile.Text) & "' "
            Else
                tempSQL = tempSQL & " and zpt.MobileNumber like '" & Trim(TextMobile.Text) & "' "
            End If
        End If
        If Trim(TextID.Text) <> "" And Trim(TextID.Text) <> "NULL" Then
            If tempSQL = "" Then
                tempSQL = "zpt.TrackID= '" & Trim(TextID.Text) & "' "
            Else
                tempSQL = tempSQL & " and zpt.TrackID= '" & Trim(TextID.Text) & "' "
            End If
        End If
        If tempSQL <> "" Then
            sqlLocalTrans = sqlLocalTrans & " and " & tempSQL & " order by zpt.id desc"
        Else
            sqlLocalTrans = sqlLocalTrans & " order by zpt.id desc"
        End If
        Session("sql") = sqlLocalTrans
        BindGrid()

        Dim SqlBind As String = "Select COUNT(zpt.ID) as col, Sum(isnull(cast(zpt.Amount as float),0)) as Amt from payit.dbo.AlrahmaCharityTransactions zpt LEFT JOIN payit.dbo.AlrahmaCharity zp on zpt.ProjectID = zp.ID  where zpt.CreatedDate between '" & fromdate & "' and '" & todate & "'"
        Dim searchSQL As String = ""
        If ddlAlRahma.SelectedItem.Text <> "All" And ddlAlRahma.SelectedItem.Text <> "NULL" Then
            If searchSQL = "" Then
                searchSQL = "zp.ProjectName like '" & Trim(ddlAlRahma.SelectedItem.Text) & "' "
            Else
                searchSQL = searchSQL & " and zp.ProjectName like '" & Trim(ddlAlRahma.SelectedItem.Text) & "' "
            End If
        End If
        If ddlServiceName.SelectedItem.Text <> "Select" And ddlServiceName.SelectedItem.Text <> "NULL" Then
            If searchSQL = "" Then
                searchSQL = "zpt.Service like '" & Trim(ddlServiceName.SelectedItem.Text) & "' "
            Else
                searchSQL = searchSQL & " and zpt.Service like '" & Trim(ddlServiceName.SelectedItem.Text) & "' "
            End If
        End If

        If (Not (ddlAlRahma.SelectedItem.Text = "All")) Then
            If dddlSubProject.SelectedItem.Text <> "All" And dddlSubProject.SelectedItem.Text <> "NULL" Then
                If searchSQL = "" Then
                    searchSQL = "zpt.SubprojectId ='" & Trim(dddlSubProject.SelectedValue) & "' "
                Else
                    searchSQL = searchSQL & " and zpt.SubprojectId = '" & Trim(dddlSubProject.SelectedValue) & "' "
                End If
            End If
        End If

        If Trim(TextMobile.Text) <> "" And Trim(TextMobile.Text) <> "NULL" Then
            If searchSQL = "" Then
                searchSQL = "zpt.MobileNumber like '" & Trim(TextMobile.Text) & "' "
            Else
                searchSQL = searchSQL & " and zpt.MobileNumber like '" & Trim(TextMobile.Text) & "' "
            End If
        End If
        If Trim(TextID.Text) <> "" And Trim(TextID.Text) <> "NULL" Then
            If searchSQL = "" Then
                searchSQL = "zpt.TrackID= '" & Trim(TextID.Text) & "' "
            Else
                searchSQL = searchSQL & " and zpt.TrackID= '" & Trim(TextID.Text) & "' "
            End If
        End If
        If searchSQL <> "" Then
            SqlBind = SqlBind & " and " & searchSQL
        Else
            SqlBind = SqlBind
        End If

        Dim totalRows As Integer
        Dim total As Double
        totalRows = GridView1.Rows.Count

        For i = 0 To totalRows - 1
            total = total + Val(GridView1.Rows(i).Cells(4).Text)
        Next
        If (total <> 0) Then
            Label17.Text = "Total Amount: " & total
        Else
            Label17.Text = String.Empty
            heading.Visible = False
        End If

        Label19.Text = String.Empty
        Label20.Text = String.Empty
        da = New SqlDataAdapter(SqlBind, cn)
        ds = New DataSet()
        da.Fill(ds, "deta")
        If Me.IsPostBack Then
            If ds.Tables.Count > 0 Then
                'Dim s As String = ds.Rows(1)("mycolumn1").ToString()
                For Each myRow As DataRow In ds.Tables(0).Rows
                    If Not IsDBNull(myRow("Amt")) AndAlso myRow("Amt") <> Nothing Then
                        Label19.Text = "Grand Total: " & myRow("Amt").ToString()
                    End If
                    If Not IsDBNull(myRow("col")) AndAlso myRow("col") <> Nothing Then
                        Label20.Text = "Total Transactions: " & myRow("col").ToString()
                    End If
                Next
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "thirdparty" Or Session("role") = "cs" Or Session("role") = "qa") Then
            alert("Unauthorized Access")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=alRahmaTrans.aspx")
        End If
        Dim username As String = String.Empty
        If Session("role") = "superadmin" Or Session("role") = "accounts" Then
            username = "%"
        ElseIf Session("user").ToString.ToLower() = "turathislamy" Then
            username = "Turath-C"
        Else
            username = Session("user")
        End If

        If Not Page.IsPostBack Then
            FromDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")
            ToDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")
            Try
                cn = New SqlConnection(strConnString)
                If (username.Equals("Turath-C")) Then
                    Sql = "select DISTINCT Service from CharityUserServiceConfig where Service like '" & username & "'"
                Else
                    Sql = "select DISTINCT Service from CharityUserServiceConfig where Username like '" & username & "'"
                End If
                'Sql = "select DISTINCT [Service] FROM [payit].[dbo].[AlrahmaCharity]"
                da = New SqlDataAdapter(Sql, cn)
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

    Protected Sub ddlServiceName_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlAlRahma.Enabled = False
        ddlAlRahma.Items.Clear()
        ddlAlRahma.Items.Insert(0, New ListItem("Select Project", "0"))
        Dim serviceId As String = ddlServiceName.SelectedItem.Text
        If serviceId <> Nothing Then
            Dim query As String = String.Format("SELECT ID,ProjectName from AlrahmaCharity where Service like '{0}%'", serviceId)
            'Dim query As String = String.Format("SELECT distinct Amount2 + SPACE(1) + ' - ' + SPACE(1) + Amount as DispAmt from Denominations Where ServiceID = {0}", serviceId)
            BindDropDownList(ddlAlRahma, query, "ProjectName", "ID", "All")
            ddlAlRahma.Enabled = True
        End If
    End Sub

    'Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        ' add the UnitPrice and QuantityTotal to the running total variables
    '        priceTotal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, _
    '          "Amount"))
    '        If (priceTotal.ToString("N2") <> "" Or Not priceTotal.ToString("N2") = Nothing) Then
    '            Label17.Text = "Total:" & priceTotal.ToString("N2")
    '        Else
    '            Label17.Text = String.Empty
    '        End If
    '    End If
    'End Sub
    Private Sub BindData()
        Dim strQuery As String = ("select cp.SubprojectName, zpt.ID,zpt.ProjectID,zpt.MobileNumber,zpt.EmailID,zpt.Amount,zpt.CreatedDate,zpt.TransactionStatus,zpt.TrackID,zpt.Status,zpt.Service, zp.ProjectName from payit.dbo.AlrahmaCharityTransactions zpt LEFT JOIN payit.dbo.AlrahmaCharity zp on zpt.ProjectID = zp.ID  left join CharitySubProjects cp on zpt.SubprojectId = cp.ID order by zpt.id desc")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
    End Sub

    'Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
    '    alert("Refreshing")
    '    Dim sqlLocalTrans As String
    '    sqlLocalTrans = "select cp.SubprojectName, zpt.ID,zpt.ProjectID,zpt.MobileNumber,zpt.EmailID,zpt.Amount,zpt.CreatedDate,zpt.TransactionStatus,zpt.TrackID,zpt.Status,zpt.Service, zp.ProjectName from payit.dbo.AlrahmaCharityTransactions zpt LEFT JOIN payit.dbo.AlrahmaCharity zp on zpt.ProjectID = zp.ID  left join CharitySubProjects cp on zpt.SubprojectId = cp.ID order by zpt.id desc"
    '    Session("sqlLocalTrans") = sqlLocalTrans
    '    Dim cmd As SqlCommand = New SqlCommand(sqlLocalTrans)
    '    GridView1.DataSource = GetData(cmd)
    '    GridView1.DataBind()
    'End Sub
    Private Sub BindGrid()
        If (chkStatus.Checked = True) Then
            GridView1.AllowPaging = False
        Else
            GridView1.AllowPaging = True
        End If
        Dim query As String = Session("sql")
        Using con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand(query)
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GridView1.DataSource = dt
                        GridView1.DataBind()
                        GridView1.EmptyDataText = "No Transactions Found"
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        GridView1.PageIndex = e.NewPageIndex
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

    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 2, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("Charity_Transactions.xls", Me.GridView1)
    End Sub

    Private Sub BindDropDownListSubProject(ddl As DropDownList, query As String, text As String, value As String, defaultText As String)
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

    Protected Sub ddlAlRahma_SelectedIndexChanged(sender As Object, e As EventArgs)
        dddlSubProject.Enabled = False
        dddlSubProject.Items.Clear()
        dddlSubProject.Items.Insert(0, New ListItem("Select Project", "0"))
        Dim ProjectId As String = ddlAlRahma.SelectedValue

        If ProjectId <> Nothing Then
            Dim query As String = String.Format("SELECT ID,SubprojectName from CharitySubProjects where ProjectID like '{0}%'", ProjectId)
            'Dim query As String = String.Format("SELECT distinct Amount2 + SPACE(1) + ' - ' + SPACE(1) + Amount as DispAmt from Denominations Where ServiceID = {0}", serviceId)
            BindDropDownListSubProject(dddlSubProject, query, "SubprojectName", "ID", "All")
            dddlSubProject.Enabled = True
        End If
    End Sub

    Protected Sub TryAgain(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect(Request.RawUrl)
    End Sub
End Class
