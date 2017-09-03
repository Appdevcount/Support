Imports System.Data
Imports System.Data.SqlClient
Partial Class EnetSupport
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim da As SqlDataAdapter
    Dim ds, ds1 As DataSet
    Dim sql, sql1 As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GridView2.DataSource = Nothing
        GridView2.Enabled = False
        If Not (Session("role") = "superadmin" Or Session("role") = "cs" Or Session("role") = "accounts" Or Session("role") = "operations") Then
            Response.Redirect("~/login.aspx?ReturnURL=EnetSupport.aspx")
        End If

        cn = New SqlConnection(strConnString)
        If Not IsPostBack Then
            lblPush.Visible = False
            chkPush.Visible = False

            'ddlStatusEdit.Items.Insert(0, New ListItem("Select", "None"))
            'ddlStatusEdit.Items.Insert(1, New ListItem("Active", "1"))
            'ddlStatusEdit.Items.Insert(2, New ListItem("Inactive", "0"))
        End If

    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        If Not (Session("role") = "superadmin" Or Session("role") = "cs" Or Session("role") = "accounts" Or Session("role") = "operations") Then
            alert("Unauthorized Access")
            Exit Sub
        End If

        sql = "SELECT myid Details,company Company,Amt,ptype Payment,trackid TrackID,udf3 MobileNo, udf2,knetprocess,udf1 Result,tdatetime TranDate" & _
" from " & DropDownList3.SelectedItem.Text & " where ("
        Dim trackSql, paySql, MobSql As String
        MobSql = " udf3 like '" & TextBox1.Text & "%'"
        paySql = " PayID like '" & TextBox3.Text & "'"
        trackSql = " trackid='" & TextBox2.Text & "'"
        Dim trackID, mob, payID As String
        trackID = Trim(TextBox2.Text)
        mob = Trim(TextBox1.Text)
        payID = Trim(TextBox3.Text)

        If mob <> "" And trackID <> "" And payID <> "" Then
            sql += MobSql & " and " & trackSql & " and " & paySql
        ElseIf mob <> "" And trackID = "" And payID = "" Then
            sql += MobSql
        ElseIf mob = "" And trackID <> "" And payID = "" Then
            sql += trackSql
        ElseIf mob = "" And trackID = "" And payID <> "" Then
            sql += paySql
        ElseIf mob <> "" And trackID <> "" And payID = "" Then
            sql += MobSql & " and " & trackSql
        ElseIf mob = "" And trackID <> "" And payID <> "" Then
            sql += trackSql & " and " & paySql
        ElseIf mob <> "" And trackID = "" And payID <> "" Then
            sql += MobSql & " and " & paySql
        Else
            Exit Sub
        End If

        sql += ") order by myid desc"
        sqldatabind(sql)
        Session("sql") = sql

        If (chkPush.Checked = True) Then
            trackID = Trim(TextBox2.Text)
            trackSql = " trackid like '" & TextBox2.Text & "'"
            If (TextBox2.Text <> "") Then
                'sql3 = "Insert into ThirdParty_knet_trans Select company,Tuser,Tpass,Tresponse_link,TId,IsysId,amt,ptype,trackid,payid,transid,refid,postdate,knetprocess,rawresponse,errmsg,udf1,udf2,udf3,udf4,udf5,tdatetime,LastUpdateOn from " & DropDownList3.SelectedItem.Text.Trim() & " where" & trackSql & ";DELETE FROM " & DropDownList3.SelectedItem.Text.Trim() & " where" & trackSql & ";"
                Using con As New SqlConnection(strConnString)
                    Using cmd As New SqlCommand("Insert into ThirdParty_knet_trans Select company,Tuser,Tpass,Tresponse_link,TId,IsysId,amt,ptype,trackid,payid,transid,refid,postdate,knetprocess,rawresponse,errmsg,udf1,udf2,udf3,udf4,udf5,tdatetime,LastUpdateOn from " & DropDownList3.SelectedItem.Text.Trim() & " where" & trackSql & ";DELETE FROM " & DropDownList3.SelectedItem.Text.Trim() & " where" & trackSql & ";INSERT INTO LogTrace (Page,Service,PaymentChannel,status,info2,ChangedBy,CreatedOn,info1) VALUES ('EnetSupport','inserted into ThirdParty_Knet_Trans','Trackid: " & trackID & "','','Mobile: " & TextBox1.Text & "','" & Session("user") & "',getdate(),'" & Request.UserHostAddress & "')")
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
                            If (insert = 3) Then
                                alert("Inserted to Table. Please search again selecting ThirdParty_Knet_Trans table")
                            Else
                                alert("Failed to Insert. Try again")
                            End If
                            If (con.State = ConnectionState.Open) Then
                                con.Close()
                            End If
                        End Using
                    End Using
                End Using
            End If
        End If
    End Sub

    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        Dim a As String = ""
        Dim i As Integer
        For i = 0 To GridView1.Rows.Count - 1
            a = GridView1.Rows(i).Cells(0).Text
            GridView1.Rows(i).Cells(0).Text = "<a href= 'grid.aspx?id=" & a & "'>" & "Details" & "</a>"

            'Added below after credit card integration in BlackBerry/HTML5 version  
            If GridView1.Rows(i).Cells(7).Text = "Approved" Then
                GridView1.Rows(i).Cells(3).Text = "CreditCard"
            End If

        Next
    End Sub

    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If GridView1.Columns.Count < 1 Then
            GridView2.DataSource = Nothing
            GridView2.Enabled = False
        End If
        Dim DS As DataSet
        Dim conn As SqlConnection
        Dim da As SqlDataAdapter
        conn = New SqlConnection(strConnString)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim cell As TableCell = e.Row.Cells(6)
            Dim service As String = String.Format(cell.Text)
            If service.ToLower().EndsWith("-o") And service <> "" Then
                If (TextBox2.Text <> "") Then
                    '    sql1 = "select ProcessTranDescription FROM [payit].[dbo].[PayitiSYS] Where IsysID = " & TextBox2.Text & " " & _
                    '            " OPEN SYMMETRIC KEY [key_DataShare] DECRYPTION BY CERTIFICATE cert_keyProtection SELECT ID, Service, Amount2 Amount,Serial,Status,CONVERT(varchar(100), convert(VARCHAR,decryptbykey(EncryptedPIN))) AS DecryptedPIN FROM PINS where ID = @var2 order by serial CLOSE SYMMETRIC KEY [key_DataShare]"
                    '    sqldatabind1(sql1)
                    '    Session("sql1") = sql1
                    'Else
                    Try
                        If conn.State = ConnectionState.Closed Then
                            conn.Open()
                        End If
                        da = New SqlDataAdapter("ProcessSuccessPINnew", conn)
                        da.SelectCommand.CommandType = CommandType.StoredProcedure
                        da.SelectCommand.Parameters.Add(New SqlParameter("@trackid", SqlDbType.VarChar, 50))
                        da.SelectCommand.Parameters("@trackid").Value = TextBox2.Text.Trim()
                        DS = New DataSet()
                        da.Fill(DS, "pins")
                        GridView2.Enabled = True
                        GridView2.DataSource = DS.Tables("pins")
                        GridView2.EmptyDataText = "No PINS Found"
                        GridView2.DataBind()
                    Catch ex As Exception
                        Console.WriteLine("Error" & ex.ToString)
                    Finally
                        If conn.State = ConnectionState.Open Then
                            conn.Close()
                        End If
                    End Try

                End If
            ElseIf service.ToLower().Equals("intl") And service <> "" Then
                If (TextBox2.Text <> "") Then
                    Try
                        If conn.State = ConnectionState.Closed Then
                            conn.Open()
                        End If
                        Dim query As String = "Select [MobileNo],[ServiceCode],[Amount],[Info1] LocalMobile From [payit].[dbo].[PayItUserRestrictions] where [TrackID] like @trackid"
                        da = New SqlDataAdapter(query, conn)
                        da.SelectCommand.CommandType = CommandType.Text
                        da.SelectCommand.Parameters.Add(New SqlParameter("@trackid", SqlDbType.VarChar, 50))
                        da.SelectCommand.Parameters("@trackid").Value = TextBox2.Text.Trim()
                        DS = New DataSet()
                        da.Fill(DS, "pins")
                        GridView2.Enabled = True
                        GridView2.DataSource = DS.Tables("pins")
                        GridView2.EmptyDataText = "No Details Found"
                        GridView2.DataBind()
                    Catch ex As Exception
                        Console.WriteLine("Error" & ex.ToString)
                    Finally
                        If conn.State = ConnectionState.Open Then
                            conn.Close()
                        End If
                    End Try
                End If
            ElseIf service.ToLower().Equals("viber-xr") And service <> "" Then
                If (TextBox2.Text <> "") Then
                    Try
                        If conn.State = ConnectionState.Closed Then
                            conn.Open()
                        End If
                        Dim query As String = "Select Convert(varchar(50),OperatorOrderID) + ' USD' as Denomination From [payit].[dbo].[PayItiSYS] where [IsysID] = @trackid"
                        da = New SqlDataAdapter(query, conn)
                        da.SelectCommand.CommandType = CommandType.Text
                        da.SelectCommand.Parameters.Add(New SqlParameter("@trackid", SqlDbType.VarChar, 50))
                        da.SelectCommand.Parameters("@trackid").Value = TextBox2.Text.Trim()
                        DS = New DataSet()
                        da.Fill(DS, "pins")
                        GridView2.Enabled = True
                        GridView2.DataSource = DS.Tables("pins")
                        GridView2.EmptyDataText = "No Denomination Found"
                        GridView2.DataBind()
                    Catch ex As Exception
                        Console.WriteLine("Error" & ex.ToString)
                    Finally
                        If conn.State = ConnectionState.Open Then
                            conn.Close()
                        End If
                    End Try
                End If
            Else
                GridView2.EmptyDataText = "No Details Found"
                GridView1.DataSource = Nothing
                GridView1.Enabled = False
            End If
        End If
    End Sub
    
    Public Sub sqldatabind(ByVal s As String)
        da = New SqlDataAdapter(s, cn)
        ds = New DataSet()
        da.Fill(ds, "det")
        GridView1.DataSource = ds.Tables("det")
        GridView1.DataBind()
    End Sub

    Public Sub sqldatabind1(ByVal s As String)
        da = New SqlDataAdapter(s, cn)
        ds = New DataSet()
        da.Fill(ds, "det")
        GridView2.DataSource = ds.Tables("det")
        GridView2.DataBind()
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        Dim s As String
        s = Session("sql")
        sqldatabind(s)
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("stats.xls", Me.GridView1)
    End Sub

    Protected Sub DropDownList3_SelectedIndexChanged(sender As Object, e As EventArgs)
        If DropDownList3.SelectedValue <> "ThirdParty_knet_trans" Then
            lblPush.Visible = True
            chkPush.Visible = True
            chkPush.Checked = False
        Else
            lblPush.Visible = False
            chkPush.Visible = False
            chkPush.Checked = False
        End If
    End Sub

    'Protected Sub Edit(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
    '    txtID.Text = row.Cells(1).Text
    '    Dim statusedit As String
    '    If row.Cells(4).Text = "True" Then
    '        statusedit = "Active"
    '    Else
    '        statusedit = "Inactive"
    '    End If

    '    If row.Cells(4).Text = "None" Or row.Cells(4).Text = "&nbsp;" Or row.Cells(4).Text = "" Then
    '        ddlStatusEdit.SelectedIndex = 0
    '    Else
    '        ddlStatusEdit.SelectedItem.Text = statusedit
    '    End If
    '    popup.Show()
    'End Sub
    'Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim constring As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    '    Dim catStatEdit As Integer
    '    If ddlStatusEdit.SelectedItem.Text = "Active" Then
    '        catStatEdit = 1
    '    Else
    '        catStatEdit = 0
    '    End If
    '    Using con As New SqlConnection(constring)
    '        Using cmd As New SqlCommand("UPDATE PINS SET Status = " & catStatEdit & " WHERE ID = " & txtID.Text.Trim())
    '            Using sda As New SqlDataAdapter()
    '                cmd.Connection = con
    '                sda.SelectCommand = cmd
    '                Using dt As New DataTable()
    '                    sda.Fill(dt)
    '                    GridView2.DataBind()
    '                End Using
    '            End Using
    '        End Using
    '    End Using
    'End Sub

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
                                              "alert('" & s & "')" & _
                                              "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
End Class
