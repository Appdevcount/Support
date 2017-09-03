Imports System.Data.SqlClient
Imports System.Data

Partial Class PaymentsSummary
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
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "checkoutuser" Or Session("role") = "cs" Or Session("role") = "qa") Then
            alert("Unauthorized Access")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=PaymentsSummary.aspx")
        End If
      

        If Not Page.IsPostBack Then
            FromDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")
            ToDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")
            Label17.Text = String.Empty
            Try
                cn = New SqlConnection(strConnString)
                Sql = "select DISTINCT [ServiceName],[ServiceID] from [payit].[dbo].[Services] where ServiceId in (168,163,162)"
                da = New SqlDataAdapter(Sql, cn)
                ds = New DataSet()
                da.Fill(ds, "deta")
                ddlServiceName.DataSource = ds.Tables("deta")
                ddlServiceName.DataTextField = "ServiceName"
                ddlServiceName.DataValueField = "ServiceName"
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
        sqlLocalTrans = "SELECT Service,SUM(Convert(float,[Amount])) as Amount, Count([ID]) as Transactions FROM [payit].[dbo].[PayitiSYS] where TranDate between '" & fromdate & "' and '" & todate & "' and ([ProcessTranDescription] like 'SUCCESS') "
        Dim tempSQL As String = ""
        Dim searchService As String = String.Empty
        If ddlServiceName.SelectedItem.Text = "All" Then
            searchService = "Service in ('Alghanim-Y', 'Safat-Y', 'Alamna-Y') "
        Else
            searchService = "Service like '" & Trim(ddlServiceName.SelectedItem.Text) & "' "
        End If
        If ddlServiceName.SelectedItem.Text <> "None" Then
            If tempSQL = "" Then
                tempSQL = searchService
            Else
                tempSQL = tempSQL & " and  " & searchService
            End If
        End If
        If tempSQL <> "" Then
            sqlLocalTrans = sqlLocalTrans & " and " & tempSQL & " Group by Service"
        Else
            sqlLocalTrans = sqlLocalTrans & " Group by Service"
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
  
    Private Sub BindData()
        Dim strQuery As String = (" SELECT Service,[Amount],Count([ID]) as Transactions FROM [payit].[dbo].[PayitiSYS]  where ([ProcessTranDescription] like 'SUCCESS') AND " & _
                                 " (Service like '" & ddlServiceName.SelectedItem.Text & "') Group by Service,[ID],Amount order by [ID]")
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
        ExportToExcel.Export("PaymentsSummary.xls", Me.GridView1)
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
