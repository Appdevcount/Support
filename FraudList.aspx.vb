Imports System.Data.SqlClient
Imports System.Data
Imports Data
Imports Data.payitEntities

Partial Class FraudList
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
        If Not (Session("role") = "superadmin" Or Session("role") = "cs" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "qa") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=FraudList.aspx")
        End If
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        If Page.IsValid Then
            If Not (Session("role") = "superadmin" Or Session("role") = "cs" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "qa") Then
                alert("You are not authorized to perform this action.")
                Exit Sub
            End If
            Try
                sqlLocalTrans = "SELECT [ID],[UdidID],[Mobile],[Description],[Status],[CreatedDate],[CreatedBy] FROM [payit].[dbo].[FraudList] "
                Dim tempSQL As String = ""
                If Not (String.IsNullOrEmpty(txtMobile.Text)) Then
                    If tempSQL = "" Then
                        tempSQL = " WHERE [Mobile] = '" & Trim(txtMobile.Text) & "' "
                    End If
                End If

                If Not (String.IsNullOrEmpty(txtUdid.Text)) Then
                    If tempSQL = "" Then
                        tempSQL = " WHERE UdidID = " & Trim(txtUdid.Text) & ""
                    Else
                        tempSQL = tempSQL & " AND UdidID = " & Trim(txtUdid.Text) & ""
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

    Private Sub BindGrid()
        If (chkPaging.Checked) Then
            GridView1.AllowPaging = False
        Else
            GridView1.AllowPaging = True
        End If
        Dim query As String = Session("sql") & " ORDER BY ID DESC"
        Using con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand(query)
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GridView1.DataSource = dt
                        GridView1.DataBind()
                        GridView1.EmptyDataText = "No Fraudsters Found"
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        GridView1.PageIndex = e.NewPageIndex
        Me.BindGrid()
    End Sub

    Public Sub Edit(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        txtID.Text = row.Cells(0).Text

        Dim dc As New payitEntities
        Dim fraud = (From d In dc.FraudLists Where d.ID = txtID.Text.Trim()).FirstOrDefault()
        If (fraud IsNot Nothing) Then
            txtMobileEdit.Text = fraud.Mobile
            txtUdidEdit.Text = fraud.UdidID

            If (fraud.Status = True) Then
                txtBlockStatus.Text = "BLOCKED"
                btnSave.Visible = True
                txtReason.Visible = True
                lblReason.Visible = True
            Else
                txtBlockStatus.Text = "UNBLOCKED"
                btnSave.Visible = False
                txtReason.Visible = False
                lblReason.Visible = False
            End If
        End If

        popup.Show()
        alert("You can now edit")
    End Sub

    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        If Not (Session("role") = "superadmin" Or Session("role") = "cs" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "qa") Then
            Response.Redirect("~/login.aspx?ReturnURL=FraudList.aspx")
        End If

        If (txtReason.Text = "" Or txtReason.Text Is Nothing) Then
            txtProcessedByError.Visible = True
            txtProcessedByError.Text = "Please Add a Reason"
            popup.Show()
            Exit Sub
        End If
       
        Using con As New SqlConnection(strConnString)
            'Using cmd As New SqlCommand("UPDATE [payit].[dbo].[FraudList] SET  Status = 0  WHERE ID = " & txtID.Text.Trim() & _
            '                            ";INSERT INTO [payit].[dbo].[LogTrace] ([Page],[Service],[PaymentChannel],[status],[ChangedBy],[info1],[info2],[CreatedOn]) VALUES " & _
            '                            "('FraudControl-Unblock','Mobile: " & txtMobileEdit.Text.Trim() & " | UNBLOCKED', ' UdidID: " & txtUdidEdit.Text.Trim() & " | UNBLOCKED',1, '" & Session("user") & "','" & Session("ip") & "', '" & txtReason.Text.Trim() & "', GETDATE()) " & _
            '                            ";INSERT INTO [payit].[dbo].[FraudWhiteList]([UdidID],[Mobile],[Description],[Status],[WhiteListDate],[WhiteListedBy]) VALUES " & _
            '                            "('" & txtUdidEdit.Text.Trim() & "', '" & txtMobileEdit.Text.Trim() & "', '" & txtReason.Text.Trim() & "',1, GETDATE(),'" & Session("user") & "')")
            Using cmd As New SqlCommand("UPDATE [payit].[dbo].[FraudList] SET  Status = 0  WHERE Mobile = '" & txtMobileEdit.Text.Trim() & "' or UdidID = '" & txtUdidEdit.Text.Trim() & "' " & _
                                       ";INSERT INTO [payit].[dbo].[LogTrace] ([Page],[Service],[PaymentChannel],[status],[ChangedBy],[info1],[info2],[CreatedOn]) VALUES " & _
                                       "('FraudControl-Unblock','Mobile: " & txtMobileEdit.Text.Trim() & " | UNBLOCKED', ' UdidID: " & txtUdidEdit.Text.Trim() & " | UNBLOCKED',1, '" & Session("user") & "','" & Session("ip") & "', '" & txtReason.Text.Trim() & "', GETDATE()) " & _
                                       ";INSERT INTO [payit].[dbo].[FraudWhiteList]([UdidID],[Mobile],[Description],[Status],[WhiteListDate],[WhiteListedBy]) " & _
                                       "SELECT [UdidID],[Mobile], '" & txtReason.Text.Trim() & "', 1,GETDATE(), '" & Session("user") & "' FROM [payit].[dbo].[FraudList] WHERE Mobile = '" & txtMobileEdit.Text.Trim() & "' or UdidID = '" & txtUdidEdit.Text.Trim() & "'")
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
        End Using
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("FraudList.xls", Me.GridView1)
    End Sub

    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub

End Class