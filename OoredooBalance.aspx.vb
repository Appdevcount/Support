Imports System.Data
Imports System.Data.SqlClient

Partial Class OoredooBalance
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim sqlLocalTrans As String = ""
     If DropDownList2.SelectedItem.Text = "WATANIYA-X" Then
            sqlLocalTrans = "select servicecode,PrepaidCredit from OperatorsCredit where servicecode like 'WATANIYA-X%'"
        ElseIf DropDownList2.SelectedItem.Text = "WATANIYA-P" Then
            sqlLocalTrans = "select servicecode,PostpaidCredit from OperatorsCredit where servicecode like 'WATANIYA-P%'"
        Else 'All
            'sqlLocalTrans = "select servicecode,PrepaidCredit,PostpaidCredit from OperatorsCredit where servicecode not like 'WATANIYA'"
            sqlLocalTrans = "select servicecode,PrepaidCredit,PostpaidCredit from OperatorsCredit where servicecode like 'WATAN%'"
        End If

        Session("sqlLocalTrans") = sqlLocalTrans
        sqldatabind(sqlLocalTrans)

        Dim sqlPins = "select Service,Amount Denomination,count(*) Remaining from pins " & _
                      " where status=0 and Service like 'WATAN%' group by Service,Amount order by service,Amount"
        da = New SqlDataAdapter(sqlPins, cn)
        ds = New DataSet()
        da.Fill(ds, "pins")
        GridView2.DataSource = ds.Tables("pins")
        GridView2.DataBind()


    End Sub

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

        If Not (Session("user") = "Ooredoo" Or Session("user") = "Shareef" Or Session("user") = "AdmiN" Or Session("user") = "AdmiN1" Or Session("user") = "AdmiN2") Then
            alert("You don't have privilage to access this page!!! Please contact administrator!!")
            Response.Redirect("index.aspx")
        End If
        cn = New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")
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
         If DropDownList1.SelectedItem.Text = "WATANIYA-X" Then
                SqlUpdate = "insert into PayitiSYS(MobileNo, [Service], Amount, ProcessTranDescription, PaymentAPI) " & _
                            "values('66470917', 'WATANIYA-X'," & Val(Trim(TextBox1.Text)) & ", 'ADDCRDT', '" & Session("user") & "')"
            ElseIf DropDownList1.SelectedItem.Text = "WATANIYA-P" Then
                SqlUpdate = "insert into PayitiSYS(MobileNo, [Service], Amount, ProcessTranDescription, PaymentAPI)" & _
                            "values('66470917', 'WATANIYA-P'," & Val(Trim(TextBox1.Text)) & ", 'ADDCRDT', '" & Session("user") & "')"
            End If


            Dim cmd As SqlCommand
            cmd = New SqlCommand(SqlUpdate, cn)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            cmd.ExecuteNonQuery()
            alert("Blance added successfully to " & DropDownList1.SelectedItem.Text)
        Catch ex As Exception
            alert("Please provide valid amount!!")
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
    End Sub
End Class
