Imports System.Data.SqlClient
Imports System.Data
Imports Data
Imports Data.payitEntities

Partial Class PinStock
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
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "cs" Or Session("role") = "operations") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=PinStock.aspx")
        End If
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        If Page.IsValid Then
            If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations") Then
                alert("You are not authorized to perform this action.")
                Exit Sub
            End If
            Try
                If (Not (String.IsNullOrEmpty(txtSerial.Text))) Then
                    sqlLocalTrans = "OPEN SYMMETRIC KEY [key_DataShare] DECRYPTION BY CERTIFICATE cert_keyProtection " & _
                    "SELECT ID, Service, Pin AS Vendor, Amount, Amount2 AS Denomination, Serial, Status, CONVERT(varchar(100), convert(VARCHAR,decryptbykey(EncryptedPIN))) AS DecryptedPIN, TranDate, OrderDate FROM PINS WHERE "
                    Dim tempSQL As String = ""
                    If txtSerial.Text IsNot Nothing Or txtSerial.Text <> "" Then
                        If tempSQL = "" Then
                            tempSQL = " [Serial] = '" & Trim(txtSerial.Text) & "' "
                        End If
                    End If

                    If tempSQL <> "" Then
                        sqlLocalTrans = sqlLocalTrans & tempSQL
                    End If

                    Session("sql") = sqlLocalTrans
                    BindGrid()
                    BindData(txtSerial.Text.Trim())
                End If
                
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

    Private Sub BindData(Serial As String)
        headinglb.InnerText = "PIN Stock History"
        Dim strQuery As String = ("select * FROM [payit].[dbo].[PINS_ReturnedToStock] Where Serial = '" & Serial & "'")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView2.DataSource = GetData(cmd)
        GridView2.DataBind()
    End Sub

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

    Protected Sub Edit(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        txtID.Text = row.Cells(0).Text

        Dim dc As New payitEntities
        Dim deno = (From d In dc.PINS Where d.ID = txtID.Text.Trim()).FirstOrDefault()
        If (deno IsNot Nothing) Then
            ServiceVal.Text = "Service: " & deno.Service
            ServiceLB.Text = deno.Service
            DenominationVal.Text = "Denomination: " & deno.Amount2
            Amount2LB.Text = deno.Amount2
            AmountVal.Text = "Amount: " & deno.Amount
            AmountLB.Text = deno.Amount
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
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations") Then
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
                                        "('PIN-StockReturn','" & ServiceVal.Text.Trim() & " | " & SerialVal.Text.Trim() & "', '" & AmountVal.Text.Trim() & " | " & DenominationVal.Text.Trim() & "', " & catStatEdit & ", '" & Session("user") & "','" & Request.UserHostAddress & "', '" & txtReason.Text.Trim() & "', getdate()) " & _
                                        "INSERT INTO [payit].[dbo].[PINS_ReturnedToStock]([PINSID],[Serial],[Vendor],[Service],[Amount],[Amount2],[Status],[Reason],[OrderDate],[ReturnDate],[ReturnedBy]) VALUES " & _
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
            BindData(SerialLb.Text.Trim())
        End Using
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("PINStock.xls", Me.GridView1)
    End Sub

    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub

End Class
