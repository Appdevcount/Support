Imports System.Data.SqlClient
Imports System.Data

Partial Class zakatprojectstrans
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sqlLocalTrans, Sql, SubProjectName As String
    Dim priceTotal As Decimal = 0
    Private grdTotal As Decimal
    Dim fromdate, todate As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Dim typeS As String = ""
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        fromdate = Trim(FromDateTextBox.Text) & " 00:00:00"
        todate = Trim(ToDateTextBox.Text) & " 23:59:59"
        cn = New SqlConnection(strConnString)
        Dim zakatSubProject As String = "Select"
        sqlLocalTrans = "select zpt.ID,zpt.ZakatProjectID,zpt.MobileNumber,zpt.EmailID,zpt.Amount,zpt.CreatedDate,zpt.TransactionStatus,zpt.TrackID,zpt.Status, zp.ZakatProjectName,cp.SubprojectName from payit.dbo.ZakatProjectsTransactions zpt LEFT JOIN payit.dbo.ZakatProjects zp on zpt.ZakatProjectID = zp.ID left join CharitySubProjects cp on zpt.SubprojectId = cp.ID  where zpt.CreatedDate between '" & fromdate & "' and '" & todate & "'"
        If (ddlzakatsub.Items.Count > 0) Then
            zakatSubProject = ddlzakatsub.SelectedItem.Text
        End If
        Dim tempSQL As String = ""
        If ddlZakat.SelectedItem.Text <> "All" And ddlZakat.SelectedItem.Text <> "NULL" Then
            If tempSQL = "" Then
                tempSQL = "zp.ZakatProjectName like '" & Trim(ddlZakat.SelectedItem.Text) & "' "
            Else
                tempSQL = tempSQL & " and zp.ZakatProjectName like '" & Trim(ddlZakat.SelectedItem.Text) & "' "
            End If
        End If
        If zakatSubProject <> "Select" And zakatSubProject <> "NULL" Then
            If tempSQL = "" Then
                tempSQL = "cp.SubprojectName like '" & Trim(ddlzakatsub.SelectedItem.Text) & "' "
            Else
                tempSQL = tempSQL & " and cp.SubprojectName like '" & Trim(ddlzakatsub.SelectedItem.Text) & "' "
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
        Dim cmd As SqlCommand = New SqlCommand(sqlLocalTrans)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
        GridView1.EmptyDataText = "No Transactions Found"

        Dim SqlBind As String = "Select COUNT(zpt.ID) as col, Sum(isnull(cast(zpt.Amount as float),0)) as Amt from payit.dbo.ZakatProjectsTransactions zpt LEFT JOIN payit.dbo.ZakatProjects zp on zpt.ZakatProjectID = zp.ID where zpt.CreatedDate  between '" & fromdate & "' and '" & todate & "'"
        Dim searchSQL As String = ""
        If ddlZakat.SelectedItem.Text <> "All" And ddlZakat.SelectedItem.Text <> "NULL" Then
            If searchSQL = "" Then
                searchSQL = "zp.ZakatProjectName like '" & Trim(ddlZakat.SelectedItem.Text) & "' "
            Else
                searchSQL = searchSQL & " and zp.ZakatProjectName like '" & Trim(ddlZakat.SelectedItem.Text) & "' "
            End If
        End If
        If zakatSubProject <> "Select" And zakatSubProject <> "NULL" Then
            If searchSQL = "" Then
                searchSQL = "cp.SubprojectName like '" & Trim(ddlzakatsub.SelectedItem.Text) & "' "
            Else
                searchSQL = searchSQL & " and cp.SubprojectName like '" & Trim(ddlzakatsub.SelectedItem.Text) & "' "
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
    Protected Sub ddlZakat_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlzakatsub.Enabled = False
        ddlzakatsub.Items.Clear()
        ' ddlzakatsub.Items.Insert(0, New ListItem("Select Project", "0"))
        Dim ProjectId As String = ddlZakat.SelectedValue
        If ProjectId <> Nothing Then
            ' Dim query As String = String.Format("SELECT ID, ProjectName from AlrahmaCharity where Service like '{0}%'", serviceId)
            Dim query As String = String.Format("select ID, SubprojectName from CharitySubProjects where [ProjectID] like '{0}'", ProjectId)
            'Dim query As String = String.Format("SELECT distinct Amount2 + SPACE(1) + ' - ' + SPACE(1) + Amount as DispAmt from Denominations Where ServiceID = {0}", serviceId)
            BindDropDownList(ddlzakatsub, query, "SubprojectName", "ID", "Select")
            ddlzakatsub.Enabled = True
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
    Protected Sub chkStatus_CheckedChanged(sender As Object, e As EventArgs)
        If IsPostBack Then
            Dim sqlLocalTrans As String
            sqlLocalTrans = "select cp.SubprojectName,zpt.ID,zpt.ZakatProjectID,zpt.MobileNumber,zpt.EmailID,zpt.Amount,zpt.CreatedDate,zpt.TransactionStatus,zpt.TrackID,zpt.Status, zp.ZakatProjectName from payit.dbo.ZakatProjectsTransactions zpt LEFT JOIN payit.dbo.ZakatProjects zp on zpt.ZakatProjectID = zp.ID left join CharitySubProjects cp on zp.ID = cp.ProjectID order by zpt.id desc"
            Session("sqlLocalTrans") = sqlLocalTrans
            Dim cmd As SqlCommand = New SqlCommand(sqlLocalTrans)
            If chkStatus.Checked = True Then
                GridView1.AllowPaging = True
                lblPaging.Text = "Disable Paging"
            Else
                GridView1.AllowPaging = False
                lblPaging.Text = "Enable Paging"
            End If
            GridView1.DataSource = GetData(cmd)
            GridView1.DataBind()
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "cs" Or Session("role") = "operations" Or Session("role") = "thirdparty") Then
            alert("Unauthorized Access")
            Response.Redirect("~/login.aspx?ReturnURL=zakatprojectstrans.aspx")
        End If
        
        If Not IsPostBack Then
            FromDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")
            ToDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")
            fromdate = Trim(FromDateTextBox.Text) & " 00:00:00"
            todate = Trim(ToDateTextBox.Text) & " 23:59:59"
            cn = New SqlConnection(strConnString)
            Sql = "Select ID,ZakatProjectName from ZakatProjects"
            da = New SqlDataAdapter(Sql, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")
            ddlZakat.DataSource = ds.Tables("deta")
            ddlZakat.DataTextField = "ZakatProjectName"
            ddlZakat.DataValueField = "ID"
            ddlZakat.DataBind()
            ddlZakat.Items.Insert(0, New ListItem("All", "None"))
        End If
    End Sub
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    priceTotal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, _
        '      "Amount"))
        '    If (priceTotal.ToString("N2") <> "" Or Not priceTotal.ToString("N2") = Nothing) Then
        '        Label17.Text = "Total:" & priceTotal.ToString("N2")
        '    Else
        '        Label17.Text = String.Empty
        '    End If
        'End If
    End Sub
    Private Sub BindData()
        Dim strQuery As String = ("select cp.SubprojectName,zpt.ID,zpt.ZakatProjectID,zpt.MobileNumber,zpt.EmailID,zpt.Amount,zpt.CreatedDate,zpt.TransactionStatus,zpt.TrackID,zpt.Status, zp.ZakatProjectName from payit.dbo.ZakatProjectsTransactions zpt LEFT JOIN payit.dbo.ZakatProjects zp on zpt.ZakatProjectID = zp.ID left join CharitySubProjects cp on zpt.SubprojectId = cp.ID  order by zpt.id desc")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
    End Sub
    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        alert("Refreshing")
        Dim sqlLocalTrans As String
        sqlLocalTrans = "select cp.SubprojectName,zpt.ID,zpt.ZakatProjectID,zpt.MobileNumber,zpt.EmailID,zpt.Amount,zpt.CreatedDate,zpt.TransactionStatus,zpt.TrackID,zpt.Status, zp.ZakatProjectName from payit.dbo.ZakatProjectsTransactions zpt LEFT JOIN payit.dbo.ZakatProjects zp on zpt.ZakatProjectID = zp.ID left join CharitySubProjects cp on zp.ID = cp.ProjectID  order by zpt.id desc"
        Session("sqlLocalTrans") = sqlLocalTrans
        Dim cmd As SqlCommand = New SqlCommand(sqlLocalTrans)
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
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 2, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("zakattrans.xls", Me.GridView1)
    End Sub
End Class
