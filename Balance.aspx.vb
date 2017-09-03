Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing

Partial Class Balance
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim sqlLocalTrans As String = ""

        Dim MconnectBl As String = ""

        Dim service As String = ddlService.SelectedItem.Text
        GridView2.Columns.Clear()
        GridView2.DataBind()
        If service <> "All" Then
            sqlLocalTrans = "SELECT Service, case when CHARINDEX('-',PIN)>0 " & _
                            "then SUBSTRING(PIN,1,CHARINDEX('-',PIN)-1) else isnull(PIN,'Other Vendor') end 'Vendor',Amount2 Denomination,Amount SellingPrice,count(*) Remaining " & _
                            "from pins where service like '" & service & "' and status=0 group by Service,Amount,Amount2,case when CHARINDEX('-',PIN)>0 " & _
                            "then SUBSTRING(PIN,1,CHARINDEX('-',PIN)-1) else isnull(PIN,'Other Vendor') end order by service,Amount,Amount2"
        Else
            'sqlLocalTrans = "select servicecode,PrepaidCredit,PostpaidCredit from OperatorsCredit where servicecode not like 'WATANIYA'"
            sqlLocalTrans = "select servicecode,PrepaidCredit,PostpaidCredit from OperatorsCredit;"

            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
           
            MconnectBl = "select top(1) VendorBalance  from StockMConnect order by ID desc"
            Dim cmd As SqlCommand
            cmd = New SqlCommand(MconnectBl, cn)
            MConnectBalance.Text = "MConnect Vendor Balance: " + cmd.ExecuteScalar()
            cn.Close()

            Dim sqlPins = "select Service,Amount2 Denomination,Amount SellingPrice,count(*) Remaining from pins " & _
                     " where status=0 group by Service,Amount,Amount2 order by service,Amount,Amount2"
            da = New SqlDataAdapter(sqlPins, cn)
            ds = New DataSet()
            da.Fill(ds, "pins")
            GridView2.DataSource = ds.Tables("pins")
            GridView2.DataBind()
        End If

        Session("sqlLocalTrans") = sqlLocalTrans
        sqldatabind(sqlLocalTrans)

        'Dim sqlPins = "select Service,Amount Denomination,count(*) Remaining from pins " & _
        '              " where status=0 group by Service,Amount,Amount2 order by service,Amount,Amount2"
        'da = New SqlDataAdapter(sqlLocalTrans, cn)
        'ds = New DataSet()
        'da.Fill(ds, "pins")
        'GridView2.DataSource = ds.Tables("pins")
        'GridView2.DataBind()


    End Sub
    'Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs)
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        Dim quantity As String = e.Row.Cells(3).Text

    '        For Each cell As TableCell In e.Row.Cells
    '            If quantity = "MConnect" Then
    '                cell.ForeColor = Color.Yellow
    '            End If
    '            If quantity = "Cameo" Then
    '                cell.ForeColor = Color.BlanchedAlmond
    '            End If
    '        Next
    '    End If
    'End Sub
    Public Sub sqldatabind(ByVal s As String)
        da = New SqlDataAdapter(s, cn)
        ds = New DataSet()
        da.Fill(ds, "det")
        GridView1.DataSource = ds.Tables("det")
        GridView1.DataBind()
        'Dim totalRows As Integer
        'Dim grandTotal As Double
        'totalRows = GridView1.Rows.Count
        'grandTotal = 0
        'For i = 0 To totalRows - 1
        '    grandTotal = grandTotal + Val(GridView1.Rows(i).Cells(1).Text)
        'Next
        ' Label17.Text = "Total Amount: " & grandTotal

    End Sub
    Public Sub counter1()
        Button2.Attributes.Add("onclick", "javascript:abct()")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "cs") Then
            alert("Unauthorized Access")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=Balance.aspx")
        End If

        cn = New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")
        If Not IsPostBack Then
            Dim Sql = "Select ServiceCode from PayitServices where [type] like 'PIN' and (ServiceCode NOT LIKE 'iTunesB-O%') order by ServiceCode"
            da = New SqlDataAdapter(Sql, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")
            ddlService.DataSource = ds.Tables("deta")
            ddlService.DataTextField = "ServiceCode"
            ddlService.DataValueField = "ServiceCode"
            ddlService.DataBind()
            ddlService.Items.Insert(0, New ListItem("All", "All"))
        End If
        If Page.IsPostBack = False Then
            counter1()
            If Request.QueryString("type") = "add" Then
                UpdatePanel2.Visible = True
                UpdatePanel1.Visible = False
            Else
                UpdatePanel2.Visible = False
                UpdatePanel1.Visible = True
            End If
            Label17.Text = ""
        End If
    End Sub

    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
                                              "alert('" & s & "')" & _
                                              "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim SqlUpdate As String = ""
        Dim retValue(3) As String

        Try
            If DropDownList1.SelectedItem.Text = "Zain" Then
                SqlUpdate = "insert into PayitiSYS(MobileNo, [Service], Amount, ProcessTranDescription,PaymentAPI) " & _
                            "values('97281623', 'ZAIN-X'," & Val(Trim(TextBox1.Text)) & ", 'ADDCRDT', '" & Session("user") & "')"
            ElseIf DropDownList1.SelectedItem.Text = "VIVA-X" Then
                SqlUpdate = "insert into PayitiSYS(MobileNo, [Service], Amount, ProcessTranDescription, PaymentAPI) " & _
                            "values('97281623', 'VIVA-X'," & Val(Trim(TextBox1.Text)) & ", 'ADDCRDT', '" & Session("user") & "')"
            ElseIf DropDownList1.SelectedItem.Text = "VIVA-P" Then
                SqlUpdate = "insert into PayitiSYS(MobileNo, [Service], Amount, ProcessTranDescription, PaymentAPI)" & _
                            "values('97281623', 'VIVA-P'," & Val(Trim(TextBox1.Text)) & ", 'ADDCRDT', '" & Session("user") & "')"
            ElseIf DropDownList1.SelectedItem.Text = "WATANIYA-X" Then
                SqlUpdate = "insert into PayitiSYS(MobileNo, [Service], Amount, ProcessTranDescription, PaymentAPI) " & _
                            "values('66470917', 'WATANIYA-X'," & Val(Trim(TextBox1.Text)) & ", 'ADDCRDT', '" & Session("user") & "')"
            ElseIf DropDownList1.SelectedItem.Text = "WATANIYA-P" Then
                SqlUpdate = "insert into PayitiSYS(MobileNo, [Service], Amount, ProcessTranDescription, PaymentAPI)" & _
                            "values('66470917', 'WATANIYA-P'," & Val(Trim(TextBox1.Text)) & ", 'ADDCRDT', '" & Session("user") & "')"
            End If

            SqlUpdate = SqlUpdate & ";INSERT INTO LogTrace(Page,Service,PaymentChannel,info2,ChangedBy,CreatedOn,info1) VALUES ('Add Balance','" & DropDownList1.SelectedItem.Text & "','Amount: " & Val(Trim(TextBox1.Text)) & "','" & txtReason.Text & "','" & Session("user") & "',getdate(),'" & Request.UserHostAddress & "')"

            Dim cmd As SqlCommand
            cmd = New SqlCommand(SqlUpdate, cn)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            cmd.ExecuteNonQuery()
            alert("Balance added successfully to " & DropDownList1.SelectedItem.Text)
        Catch ex As Exception
            alert("Please enter a valid amount")
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
    End Sub
End Class
