Imports System.Data.SqlClient
Imports System.Data

Partial Class zakatSummary
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sqlLocalTrans, Sql As String
    Private grdTotal As Decimal
    Dim fromdate, todate As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "cs" Or Session("role") = "operations" Or Session("role") = "thirdparty") Then
            Response.Redirect("~/login.aspx?ReturnURL=zakatSummary.aspx")
        End If

        If Not Page.IsPostBack Then
            FromDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")
            ToDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")
            Label17.Text = String.Empty
            Try
                cn = New SqlConnection(strConnString)
                Sql = "select [ZakatProjectName], [ID] FROM [payit].[dbo].[ZakatProjects]"
                da = New SqlDataAdapter(Sql, cn)
                ds = New DataSet()
                da.Fill(ds, "deta")
                ddlServiceName.DataSource = ds.Tables("deta")
                ddlServiceName.DataTextField = "ZakatProjectName"
                ddlServiceName.DataValueField = "ID"
                ddlServiceName.DataBind()
                ddlServiceName.Items.Insert(0, New ListItem("All", "None"))
            Catch ex As Exception
                dberrorlabel.Text = ex.ToString
            End Try
        End If
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        fromdate = Trim(FromDateTextBox.Text) & " 00:00:00"
        todate = Trim(ToDateTextBox.Text) & " 23:59:59"
        sqlLocalTrans = "SELECT zp.ZakatProjectName,SUM(Convert(float,zpt.[Amount])) as Amount,Count(zpt.[ID]) as Transactions FROM [payit].[dbo].[ZakatProjectsTransactions] zpt  LEFT JOIN payit.dbo.ZakatProjects zp on zpt.ZakatProjectID = zp.ID where zpt.[CreatedDate] between '" & fromdate & "' and '" & todate & "' and (zpt.TransactionStatus like 'SUCCESS%')"
        Dim tempSQL As String = ""
        If ddlServiceName.SelectedItem.Text <> "All" And ddlServiceName.SelectedItem.Value <> "None" Then
            If tempSQL = "" Then
                tempSQL = "zpt.[ZakatProjectID] like '" & Trim(ddlServiceName.SelectedItem.Value) & "' "
            Else
                tempSQL = tempSQL & " and zpt.[ZakatProjectID] like '" & Trim(ddlServiceName.SelectedItem.Value) & "' "
            End If
        End If
        If tempSQL <> "" Then
            sqlLocalTrans = sqlLocalTrans & " and " & tempSQL & " Group by zp.ZakatProjectName,zpt.[ZakatProjectID] order by zpt.[ZakatProjectID]"
        Else
            sqlLocalTrans = sqlLocalTrans & " Group by zp.ZakatProjectName,zpt.[ZakatProjectID] order by zpt.[ZakatProjectID]"
        End If
        Dim cmd As SqlCommand = New SqlCommand(sqlLocalTrans)
        Session("sqlLocalTrans") = sqlLocalTrans
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
    Dim priceTotal As Decimal = 0
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    priceTotal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, _
        '         "Amount"))
        '    Label17.Text = "Total Amount:" & priceTotal.ToString("N2")
        'End If
    End Sub
    Private Sub BindData()
        Dim strQuery As String = ("SELECT zp.ZakatProjectName,zpt.[Amount],Count(zpt.[ID]) as Transactions FROM [payit].[dbo].[ZakatProjectsTransactions] zpt  LEFT JOIN payit.dbo.ZakatProjects zp on zpt.ZakatProjectID = zp.ID where (zpt.TransactionStatus like 'SUCCESS%') Group by zp.ZakatProjectName,zpt.[ZakatProjectID],Amount order by zpt.[ZakatProjectID]")
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
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("zakatSummary.xls", Me.GridView1)
    End Sub
End Class
