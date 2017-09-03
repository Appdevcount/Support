Imports System.Data.SqlClient
Imports System.Data

Partial Class alRahmaSummary
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sqlLocalTrans, Sql As String
    Dim priceTotal As Decimal = 0
    Private grdTotal As Decimal
    Dim fromdate, todate As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "thirdparty" Or Session("role") = "cs" Or Session("role") = "qa") Then
            alert("Unauthorized Access")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=alRahmaSummary.aspx")
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
            Label17.Text = String.Empty
            Try
                cn = New SqlConnection(strConnString)
                If (username.Equals("Turath-C")) Then
                    Sql = "select DISTINCT Service from CharityUserServiceConfig where Service like '" & username & "'"
                Else
                    Sql = "select DISTINCT Service from CharityUserServiceConfig where Username like '" & username & "'"
                End If
                da = New SqlDataAdapter(Sql, cn)
                ds = New DataSet()
                da.Fill(ds, "deta")
                ddlServiceName.DataSource = ds.Tables("deta")
                ddlServiceName.DataTextField = "Service"
                ddlServiceName.DataValueField = "Service"
                ddlServiceName.DataBind()
                ddlServiceName.Items.Insert(0, New ListItem("Select", "None"))
            Catch ex As Exception
                dberrorlabel.Text = ex.ToString
            End Try
        End If
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If (ddlServiceName.SelectedItem.Text = "Select") Then
            alert("Please Select a Service Name before searching")
            Exit Sub
        End If
        fromdate = Trim(FromDateTextBox.Text) & " 00:00:00"
        todate = Trim(ToDateTextBox.Text) & " 23:59:59"
        sqlLocalTrans = "SELECT zpt.Service, zp.ProjectName,SUM(Convert(float,zpt.[Amount])) as Amount,Count(zpt.[ID]) as Transactions FROM [payit].[dbo].[AlrahmaCharityTransactions] zpt  LEFT JOIN payit.dbo.AlrahmaCharity zp on zpt.ProjectID = zp.ID where zpt.[CreatedDate] between '" & fromdate & "' and '" & todate & "' and (zpt.TransactionStatus like 'SUCCESS%')"
        Dim tempSQL As String = ""
        If ddlProjectName.SelectedItem.Text <> "All" And ddlProjectName.SelectedItem.Value <> "None" Then
            If tempSQL = "" Then
                tempSQL = "zpt.[ProjectID] like '" & Trim(ddlProjectName.SelectedItem.Value) & "' "
            Else
                tempSQL = tempSQL & " and zpt.[ProjectID] like '" & Trim(ddlProjectName.SelectedItem.Value) & "' "
            End If
        End If
        If ddlServiceName.SelectedItem.Text <> "Select" And ddlServiceName.SelectedItem.Value <> "None" Then
            If tempSQL = "" Then
                tempSQL = "zpt.[Service] like '" & Trim(ddlServiceName.SelectedItem.Value) & "' "
            Else
                tempSQL = tempSQL & " and zpt.[Service] like '" & Trim(ddlServiceName.SelectedItem.Value) & "' "
            End If
        End If
        If tempSQL <> "" Then
            sqlLocalTrans = sqlLocalTrans & " and " & tempSQL & " Group by zpt.Service,zp.ProjectName,zpt.[ProjectID] order by zpt.[ProjectID]"
        Else
            sqlLocalTrans = sqlLocalTrans & " Group by zpt.Service,zp.ProjectName,zpt.[ProjectID] order by zpt.[ProjectID]"
        End If
        Dim cmd As SqlCommand = New SqlCommand(sqlLocalTrans)
        Session("sqlLocalTrans") = sqlLocalTrans
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
        GridView1.EmptyDataText = "No Records Found"
        serviceLabel.Text = ddlServiceName.SelectedItem.Text + " Project Details"
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
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    priceTotal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, _
        '         "Amount"))
        '    Label17.Text = "Total Amount:" & priceTotal.ToString("N2")
        'End If
    End Sub
    Private Sub BindData()
        Dim strQuery As String = ("SELECT zpt.Service,zp.ProjectName,zpt.[Amount],Count(zpt.[ID]) as Transactions FROM [payit].[dbo].[AlrahmaCharityTransactions] zpt  LEFT JOIN payit.dbo.AlrahmaCharity zp on zpt.ProjectID = zp.ID where (zpt.TransactionStatus like 'SUCCESS%') AND (Service like '" & ddlServiceName.SelectedItem.Text & "') Group by zpt.Service,zp.ProjectName,zpt.[ProjectID],Amount order by zpt.[ProjectID]")
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
        ddlProjectName.Enabled = False
        ddlProjectName.Items.Clear()
        ddlProjectName.Items.Insert(0, New ListItem("Select Project", "0"))
        Dim serviceId As String = ddlServiceName.SelectedItem.Text
        If serviceId <> Nothing Then
            Dim query As String = String.Format("SELECT ID, ProjectName from AlrahmaCharity where Service like '{0}%'", serviceId)
            'Dim query As String = String.Format("SELECT distinct Amount2 + SPACE(1) + ' - ' + SPACE(1) + Amount as DispAmt from Denominations Where ServiceID = {0}", serviceId)
            BindDropDownList(ddlProjectName, query, "ProjectName", "ID", "All")
            ddlProjectName.Enabled = True
        End If
        If (ddlServiceName.SelectedItem.Text = "Select") Then
            ddlProjectName.Enabled = False
            ddlProjectName.Items.Clear()
        End If
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("Summary.xls", Me.GridView1)
    End Sub
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 2, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Protected Sub TryAgain(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect(Request.RawUrl)
    End Sub
End Class
