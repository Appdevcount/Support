Imports System.Data
Imports System.Data.SqlClient
Partial Class OoredooSummaryReport
    Inherits System.Web.UI.Page

    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        counter1()
        Dim sqlLocalTrans As String
        Dim fromdate, todate As String
        If Trim(FromDateTextBox.Text) = "" Or Trim(ToDateTextBox.Text) = "" Then
            Label14.Visible = True
            Label15.Visible = True

            Exit Sub
        Else
            Label14.Visible = False
            Label15.Visible = False
            fromdate = Trim(FromDateTextBox.Text) & " 00:00:00"
            todate = Trim(ToDateTextBox.Text) & " 23:59:59"
        End If
      

        
        sqlLocalTrans = "select [Service] ServiceCode,sum(Amount) Amount,count(*) TransCount " & _
     "from PayitiSYS " & _
     "where(Convert(DateTime, TranDate, 103)) " & _
     "between convert(datetime,'" & fromdate & "', 103) and convert(datetime,'" & todate & "', 103) and id>=26 " & _
     " and PaymentAPI NOT LIKE '%POS' and (ProcessTranDescription like '%SUCCESS%' " & _
      " or  ProcessTranDescription like 'P%' or  ProcessTranDescription like 'OK%') and (Service like 'watan%' or Service like 'mynet%')" & _
     " group by [Service]"


        Session("sqlLocalTrans") = sqlLocalTrans
        sqldatabind(sqlLocalTrans)

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("user") = "" Then
            Response.Redirect("login.aspx")
        End If
        cn = New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")
        If Page.IsPostBack = False Then
            counter1()
            Label14.Visible = False
            Label15.Visible = False
            FromDateTextBox.Text = Format(Date.Today.Date, "dd/MM/yyyy")
            ToDateTextBox.Text = Format(Date.Today.Date, "dd/MM/yyyy")
            Label17.Text = ""

        End If
    End Sub
    Public Sub sqldatabind(ByVal s As String)

        Dim cmd1 As New SqlCommand(s, cn)
        cmd1.CommandTimeout = 3600

        da = New SqlDataAdapter(cmd1)
        ds = New DataSet()
        da.Fill(ds, "det")
        GridView1.DataSource = ds.Tables("det")
        GridView1.DataBind()
        Dim totalRows As Integer
        Dim grandTotal, Totaltrans As Double
        totalRows = GridView1.Rows.Count
        grandTotal = 0
        Totaltrans = 0
        For i = 0 To totalRows - 1
            grandTotal = grandTotal + Val(GridView1.Rows(i).Cells(1).Text)
            Totaltrans = Totaltrans + Val(GridView1.Rows(i).Cells(2).Text)
        Next
        Label17.Text = "Total Amount: " & grandTotal
        Label18.Text = "Total Transactions :" & Totaltrans

    End Sub

    Public Sub counter1()
        Button2.Attributes.Add("onclick", "javascript:abct()")
        GridView1.Attributes.Add("databinding", "javascript:abct()")
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        Dim s As String
        s = Session("sqlLocalTrans")
        sqldatabind(s)

    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("stats.xls", Me.GridView1)
    End Sub

    Protected Sub ddlServiceProvider_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlServiceProvider.SelectedIndexChanged

    End Sub

   
End Class
