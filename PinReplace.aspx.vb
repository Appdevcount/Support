Imports System.Data.SqlClient
Imports System.Data
Imports Data
Imports Data.payitEntities

Partial Class PinReplace
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sqlLocalTrans, Sql As String
    Dim priceTotal As Decimal = 0
    Private grdTotal As Decimal
    Dim fromdate, todate As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Dim typeS As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=PinReplace.aspx")
        End If
        If Not Page.IsPostBack Then
            Try
                cn = New SqlConnection(strConnString)
                Sql = "select ServiceName,ServiceID from services WHERE ServiceName like '%-O' order by ServiceName "
                da = New SqlDataAdapter(Sql, cn)
                ds = New DataSet()
                da.Fill(ds, "deta")
                ddlServiceName.DataSource = ds.Tables("deta")
                ddlServiceName.DataTextField = "ServiceName"
                ddlServiceName.DataValueField = "ServiceID"
                ddlServiceName.DataBind()

                ddlServiceNameEdit.DataSource = ds.Tables("deta")
                ddlServiceNameEdit.DataTextField = "ServiceName"
                ddlServiceNameEdit.DataValueField = "ServiceID"
                ddlServiceNameEdit.DataBind()
            Catch ex As Exception
            End Try
        End If
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        If Page.IsValid Then
            If Not (Session("role") = "superadmin" Or Session("role") = "operations") Then
                alert("You are not authorized to perform this action.")
                Exit Sub
            End If
            Try
                sqlLocalTrans = "OPEN SYMMETRIC KEY [key_DataShare] DECRYPTION BY CERTIFICATE cert_keyProtection " & _
                    "SELECT TOP 1 ID, Service, Pin Vendor, Amount,Amount2, Serial, Status, CONVERT(varchar(100), convert(VARCHAR,decryptbykey(EncryptedPIN))) AS DecryptedPIN, TranDate, OrderDate FROM PINS WHERE STATUS = 0"
                Dim tempSQL As String = ""
                If ddlServiceName.SelectedIndex > 0 Then
                    If tempSQL = "" Then
                        tempSQL = " AND [Service] = '" & Trim(ddlServiceName.SelectedItem.Text) & "' "
                    End If
                End If

                If ddlDenom.SelectedIndex > 0 Then
                    If tempSQL = "" Then
                        tempSQL = " AND Amount = " & Trim(ddlDenom.SelectedValue) & ""
                    Else
                        tempSQL = tempSQL & " AND Amount = " & Trim(ddlDenom.SelectedValue) & ""
                    End If
                End If

                If tempSQL <> "" Then
                    sqlLocalTrans = sqlLocalTrans & tempSQL
                End If

                Session("sql") = sqlLocalTrans
                BindGrid()
                'BindData(txtSerial.Text.Trim())
            Catch ex As Exception

            End Try
        End If
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

    Private Sub BindGrid()
        If (chkPaging.Checked) Then
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

    'Private Sub BindData(Serial As String)
    '    headinglb.InnerText = "PIN Stock History"
    '    Dim strQuery As String = ("select * FROM [payit].[dbo].[PINS_ReturnedToStock] Where Serial = '" & Serial & "'")
    '    Dim cmd As SqlCommand = New SqlCommand(strQuery)
    '    GridView2.DataSource = GetData(cmd)
    '    GridView2.DataBind()
    'End Sub

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

    Public Sub Edit(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        txtID.Text = row.Cells(0).Text

        Dim dc As New payitEntities
        Dim deno = (From d In dc.PINS Where d.ID = txtID.Text.Trim()).FirstOrDefault()
        If (deno IsNot Nothing) Then
            ddlServiceNameEdit.SelectedItem.Text = deno.Service
            ddlDenomEdit.SelectedItem.Text = deno.Amount2
            ddlAmountEdit.SelectedItem.Text = deno.Amount
            'ServiceVal.Text = "Service: " & deno.Service
            ServiceLB.Text = deno.Service
            'DenominationVal.Text = "Denomination: " & deno.Amount2
            Amount2LB.Text = deno.Amount2
            'AmountVal.Text = "Amount: " & deno.Amount
            AmountLB.Text = deno.Amount
            txtSerial.Text = deno.Serial
            txtPin.Text = row.Cells(6).Text

            SerialVal.Text = "Serial: " & deno.Serial
            SerialLb.Text = deno.Serial

            PinVal.Text = "Pin: " & row.Cells(6).Text

            If (String.IsNullOrEmpty(deno.PIN1)) Then
                VendorLB.Text = ""
            Else
                VendorLB.Text = deno.PIN1
            End If

            If (deno.OrderDate Is Nothing) Then
                OrderDateLb.Text = ""
            Else
                OrderDateLb.Text = deno.OrderDate
            End If

            If (deno.Status = True) Then
                chkStatEdit.Checked = True
            Else
                chkStatEdit.Checked = False
            End If
        End If

        popup.Show()
        alert("You can now edit")
    End Sub

    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        If Not (Session("role") = "superadmin" Or Session("role") = "operations") Then
            Response.Redirect("~/login.aspx?ReturnURL=PinStock.aspx")
        End If

        If (txtReason.Text = "" Or txtReason.Text Is Nothing) Then
            txtProcessedByError.Visible = True
            txtProcessedByError.Text = "Please Add a Reason"
            popup.Show()
            Exit Sub
        End If

        Dim catStatEdit As Integer
        If chkStatEdit.Checked = True Then
            catStatEdit = 1
        Else
            catStatEdit = 0
        End If

        Using con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand("UPDATE [payit].[dbo].[PINS] SET  Status = " & catStatEdit & "  WHERE ID = " & txtID.Text.Trim() & _
                                        ";INSERT INTO [payit].[dbo].[LogTrace] ([Page],[Service],[PaymentChannel],[status],[ChangedBy],[info1],[info2],[CreatedOn]) VALUES " & _
                                        "('PIN-Replace','Service: " & ddlServiceNameEdit.SelectedItem.Text.Trim() & " | " & SerialVal.Text.Trim() & "', ' Amount: " & ddlAmountEdit.SelectedItem.Text.Trim() & " | Denomination: " & ddlDenomEdit.SelectedItem.Text.Trim() & "', " & catStatEdit & ", '" & Session("user") & "','" & Session("ip") & "', '" & txtReason.Text.Trim() & "', getdate()) " & _
                                        ";INSERT INTO [payit].[dbo].[PINS_ReturnedToStock]([PINSID],[Serial],[Vendor],[Service],[Amount],[Amount2],[Status],[Reason],[OrderDate],[ReturnDate],[ReturnedBy]) VALUES " & _
                                        "('" & txtID.Text.Trim() & "', '" & SerialLb.Text.Trim() & "', '" & VendorLB.Text.Trim() & "', '" & ServiceLB.Text.Trim() & "', '" & AmountLB.Text.Trim() & "', '" & Amount2LB.Text.Trim() & "', " & catStatEdit & ", '" & txtReason.Text.Trim() & "', '" & OrderDateLb.Text & "', GETDATE(),'" & Session("user") & "')")
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GridView1.DataBind()
                    End Using
                End Using
            End Using
            Me.BindGrid()

            If (Not (String.IsNullOrEmpty(txtMobile.Text))) Then
                If (chkSms.Checked) AndAlso catStatEdit = 1 Then
                    Dim SMSresp As String = String.Empty
                    Dim mobileno As String = txtMobile.Text.Trim()
                    If (Not mobileno.StartsWith("965")) Then
                        mobileno = "965" + mobileno
                    End If
                    Dim PIN = "PIN: " & txtPin.Text.Trim() & ", Amount: " + ddlAmountEdit.SelectedItem.Text + " KD" + ", Ref:" + txtReason.Text.Trim() + " - Payit"

                    SMSresp = SMS.sendSMS(PIN, mobileno, "FCCKW2", txtReason.Text.Trim())
                End If
            End If
        End Using
    End Sub

    Protected Sub ddlService_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlDenom.Enabled = False
        ddlDenom.Items.Clear()

        Dim ProjectId As String = ddlServiceName.SelectedValue
        Dim ServiceName As String = ddlServiceName.SelectedItem.Text
        If ProjectId <> Nothing Then
            Dim query As String = String.Format("select Amount2, Amount " & _
                                                  "from [payit].[dbo].[Denominations] where [ServiceID] like '{0}%' order by cast(Amount as float)", ProjectId)
            BindDropDownList(ddlDenom, query, "Amount2", "Amount", "Select")
            BindDropDownList(ddlDenomEdit, query, "Amount2", "Amount", "Select")
            BindDropDownList(ddlAmountEdit, query, "Amount", "Amount", "Select")
            ddlDenom.Enabled = True
        End If

        'ddlVendor.Enabled = False
        'ddlVendor.Items.Clear()

        'If (ServiceName <> Nothing AndAlso Not (ServiceName.ToLower.Equals("all"))) Then
        '    Dim sql As String = String.Format("select DISTINCT case when CHARINDEX('-',PIN)>0 " & _
        '                                          "then SUBSTRING(PIN,1,CHARINDEX('-',PIN)-1) else isnull(PIN,'Other Vendor') end 'Vendor' " & _
        '                                          "from PINS where [Service] like '{0}%' order by Vendor", ServiceName)
        '    BindDropDownList(ddlVendor, sql, "Vendor", "Vendor", "All")
        '    ddlVendor.Enabled = True
        'End If
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
        ddl.Items.Insert(0, New ListItem(defaultText, "%"))
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("PINReplace.xls", Me.GridView1)
    End Sub

    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub

End Class