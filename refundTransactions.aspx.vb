Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Partial Class refundTransactions
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sql As String
    Dim sqlLocalTrans As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Private conn As New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "cs" Or Session("role") = "accounts" Or Session("role") = "operations") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=refundTransactions.aspx")
        End If
        
        cn = New SqlConnection(strConnString)
        If Not IsPostBack Then
            sql = "Select ServiceCode from PayitServices where (Type='PIN' or Type='Payment' or Type='Exchange' or Type='Recharge' or Type='ZakatProject') And (ServiceCode NOT LIKE 'iTunesB-O%') order by ServiceName"
            da = New SqlDataAdapter(sql, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")
            ddlService.DataSource = ds.Tables("deta")
            ddlService.DataTextField = "ServiceCode"
            ddlService.DataValueField = "ServiceCode"
            ddlService.DataBind()
            ddlService.Items.Insert(0, New ListItem("All", "All"))

            ddlRefundType.Items.Insert(0, New ListItem("All", "All"))
            ddlRefundType.Items.Insert(1, New ListItem("REFUND", "Successrfnd"))
            ddlRefundType.Items.Insert(2, New ListItem("VOIDED", "Voided"))
            ddlRefundType.Items.Insert(3, New ListItem("PAYIT-VPN", "SuccessDealer"))
            ddlRefundType.Items.Insert(4, New ListItem("REFUND-ONLINE", "RefundOnline"))
            ddlRefundType.Items.Insert(5, New ListItem("REFUND-CASH", "RefundCash"))
            ddlRefundType.Items.Insert(6, New ListItem("REFUND-REQ", "RefundRequest"))

            Label14.Visible = False
            Label15.Visible = False
            Label17.Text = ""

            FromDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")
            ToDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")

        End If
    End Sub
    Private Sub BindData()
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
        Dim strQuery As String = "select tk.TrackID,isnull(tk.udf2,'Unknown') serviceCode,t3.serial,tk.udf3 Mob, tk.amt Amount," & _
                                      " tk.knetProcess,isnull(tk.udf1,'Unprocessed') Result,tk.tdatetime TransDate,tk.LastUpdateOn ProcessDate  " & _
                                      " from  " & DropDownList3.SelectedItem.Text & " tk FULL OUTER JOIN PayitiSYS as t2 ON tk.trackid = t2.IsysID FULL OUTER JOIN PINS as t3 ON t2.OperatorOrderID = t3.ID " & _
                                      " where tk.LastUpdateOn " & _
                                      " between '" & fromdate & "' and '" & todate & "' " & _
                                      " and ( KnetProcess like 'CAPTURED%' or KnetProcess like 'Approved%' or KnetProcess like 'Voided%') and (udf1 like 'Successrfnd%' or udf1 like 'refund%' or udf1 like 'Voided%')"
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()

        Dim totalRows As Integer
        Dim total As Double
        totalRows = GridView1.Rows.Count

        For i = 0 To totalRows - 1
            total = total + Val(GridView1.Rows(i).Cells(4).Text)
        Next
        Label17.Text = "Total Amount: " & total
    End Sub
    Public Sub sqldatabind(ByVal s As String, Optional ByVal types1 As String = "", Optional ByVal type As String = "")
        Dim SqlOrder As String
        Session("type") = types1
        If types1 = "success" Then
            'Dim s2 As String = " union all " & s
            's2 = s2.Replace("PayitiSYS", "PayitiSYS20140403")
            'SqlOrder = s & s2 & " order by id desc"
            SqlOrder = s & " order by tk.id desc"
        Else
            SqlOrder = s & " order by tk.LastUpdateOn desc"
        End If
        da = New SqlDataAdapter(SqlOrder, cn)
        ds = New DataSet()
        da.Fill(ds, "det")
       
        GridView1.DataSource = String.Empty
        GridView1.DataSource = ds.Tables("det")
        GridView1.DataBind()
        Dim totalRows As Integer
        Dim total As Double
        totalRows = GridView1.Rows.Count

        For i = 0 To totalRows - 1
            total = total + Val(GridView1.Rows(i).Cells(6).Text)
        Next

        'If type = "first" And ds.Tables("det").Rows.Count > 50 Then

        Dim grandTotSql As String = ""
        grandTotSql = "select sum(convert(float,Amount)),count(*)  from ( " & s
        grandTotSql += " ) as temp1"
        Dim da1 As SqlDataAdapter
        Dim ds1 As DataSet

        da1 = New SqlDataAdapter(grandTotSql, cn)
        ds1 = New DataSet()
        da1.Fill(ds1, "TCount")
        Label19.Text = "GrandTotal: " & ds1.Tables("TCount").Rows(0).Item(0)
        Label20.Text = "TotalTransactions: " & ds1.Tables("TCount").Rows(0).Item(1)

        'End If
        Label17.Text = "Total Amount: " & total

    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
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
        If Page.IsValid Then
            sqlLocalTrans = "select tk.TrackID,isnull(tk.udf2,'Unknown') serviceCode,tk.udf3 Mob, tk.amt Amount," & _
                                     " tk.knetProcess,isnull(tk.udf1,'Unprocessed') Result,tk.tdatetime TransDate,tk.LastUpdateOn ProcessDate " & _
                                     " from " & DropDownList3.SelectedItem.Text & " tk " & _
                                     " where tk.LastUpdateOn " & _
                                     " between '" & fromdate & "' and '" & todate & "' " & _
                                     " and (KnetProcess like 'CAPTURED%' or KnetProcess like 'Approved%') "

            If ddlService.SelectedIndex <> "0" Then
                sqlLocalTrans += " and tk.udf2 like '" & Trim(ddlService.SelectedItem.Text) & "' "
            End If

            If ddlRefundType.SelectedIndex <> "0" Then
                sqlLocalTrans += " and (udf1 like '" & Trim(ddlRefundType.SelectedItem.Value) & "%') "
            Else
                sqlLocalTrans += " and (udf1 like 'successrfnd' or udf1 like 'refund%' or udf1 like 'voided') "
            End If

            If Trim(txtMobile.Text) <> "" Then
                sqlLocalTrans += "and (tk.udf3 like '" & Trim(txtMobile.Text) & "') "
            End If

            If Trim(txtTrackID.Text) <> "" Then
                sqlLocalTrans += " and tk.trackid like '" & Trim(txtTrackID.Text) & "'"
            End If
            'sqlLocalTrans += " order by tk.LastUpdateOn desc"

            Session("sql") = sqlLocalTrans
            BindGrid()

        End If
    End Sub

    Private Sub BindGrid()
        If (chkStatus.Checked = True) Then
            GridView1.AllowPaging = False
        Else
            GridView1.AllowPaging = True
        End If
        Dim query As String = Session("sql") + " order by tk.LastUpdateOn desc"
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

                Dim totalRows As Integer
                Dim total As Double
                totalRows = GridView1.Rows.Count

                For i = 0 To totalRows - 1
                    total = total + Val(GridView1.Rows(i).Cells(3).Text)
                Next
                Dim grandTotSql As String = ""
                grandTotSql = "select sum(convert(float,Amount)),count(*)  from ( " & Session("sql")
                grandTotSql += " ) as temp1"
                Dim da1 As SqlDataAdapter
                Dim ds1 As DataSet

                da1 = New SqlDataAdapter(grandTotSql, cn)
                ds1 = New DataSet()
                da1.Fill(ds1, "TCount")
                Label19.Text = "GrandTotal: " & ds1.Tables("TCount").Rows(0).Item(0)
                Label20.Text = "TotalTransactions: " & ds1.Tables("TCount").Rows(0).Item(1)

                'Label17.Text = "Total Amount: " & total

            End Using
        End Using
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
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        GridView1.PageIndex = e.NewPageIndex
        Me.BindGrid()
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("PayitRefunds.xls", Me.GridView1)
    End Sub
End Class
