Imports System.Data.SqlClient
Imports System.Data
Partial Class commission
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sql, strSQL As String
    Dim dispAmnt, info1 As String
    Dim sqlLocalTrans As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Protected Sub chkCommission_CheckedChanged(sender As Object, e As EventArgs)
        If (chkCommission.Checked = True) Then
            lblCommMsg.Visible = False
        Else
            lblCommMsg.Visible = True
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "qa") Then
            alert("You do not have authorization to access this page. Contact Administrator for support.")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=commission.aspx")
        End If

        If Not IsPostBack Then
            Me.BindData()
            GridView1.DataBind()
        End If
        If Not IsPostBack Then
            cn = New SqlConnection(strConnString)
            sql = "Select ID, ServiceCode from PayitServices"
            da = New SqlDataAdapter(sql, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")
            ddlServType.DataSource = ds.Tables("deta")
            ddlServType.DataTextField = "ServiceCode"
            ddlServType.DataValueField = "ID"
            ddlServType.DataBind()
            ddlServType.Items.Insert(0, New ListItem("Select", "None"))

            ddlServiceEdit.DataSource = ds.Tables("deta")
            ddlServiceEdit.DataTextField = "ServiceCode"
            ddlServiceEdit.DataValueField = "ID"
            ddlServiceEdit.DataBind()
            ddlServiceEdit.Items.Insert(0, New ListItem("Select", "None"))

            ddlKeypad.Items.Insert(0, New ListItem("Select", "None"))
            ddlKeypad.Items.Insert(1, New ListItem("Numeric", "1"))
            ddlKeypad.Items.Insert(2, New ListItem("AlphaNumeric", "2"))
            If (ddlKeypad.Items.Count > 0) Then
                ddlKeypad.SelectedIndex = 1
            End If

            ddlKeypadEdit.Items.Insert(0, New ListItem("Select", "None"))
            ddlKeypadEdit.Items.Insert(1, New ListItem("Numeric", "1"))
            ddlKeypadEdit.Items.Insert(2, New ListItem("AlphaNumeric", "2"))
        End If
    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If Not (Session("role") = "superadmin" Or Session("role") = "operations" Or Session("role") = "accounts" Or Session("role") = "qa") Then
            alert("You do not have authorization to access this page. Contact Administrator for support.")
            Exit Sub
        End If

        If Page.IsValid Then
            'If (txtCommission.Text = "" Or txtCommMsg.Text = "") Then
            '    dberrorlabel.Text = "Please Fill all fields!"
            'MsgBox("Please fill-up all fields!", MsgBoxStyle.Exclamation, "Add New Record!")
            Dim catStat, chkCommInt, chkAmtInt As Integer
            If chkStatus.Checked = True Then
                catStat = 1
            Else
                catStat = 0
            End If
            If chkAmountEdit.Checked = True Then
                chkAmtInt = 1
            Else
                chkAmtInt = 0
            End If
            If chkCommission.Checked = True Then
                chkCommInt = 1
            Else
                chkCommInt = 0
            End If

            Dim constring As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
            Using con As New SqlConnection(constring)
                Dim quer As String = "INSERT INTO PayitServicesConfig(PayitServiceID,Commission,isCommissionMandatory,CommissionInfo,isAmountEditable,KeypadType,Status,CreatedDate,Info4) VALUES (" & ddlServType.SelectedValue & ",'" & txtCommission.Text.Trim() & "'," & chkCommInt & ",'" & txtCommMsg.Text & "'," & chkAmtInt & ",'" & ddlKeypad.SelectedValue & "'," & catStat & ",GETDATE(),'" & ddlServType.SelectedItem.Text & "')"
                Using cmd As New SqlCommand("INSERT INTO PayitServicesConfig(PayitServiceID,Commission,isCommissionMandatory,CommissionInfo,isAmountEditable,KeypadType,Status,CreatedDate,Info4) VALUES (" & ddlServType.SelectedValue & ",'" & txtCommission.Text.Trim() & "'," & chkCommInt & ",'" & txtCommMsg.Text & "'," & chkAmtInt & ",'" & ddlKeypad.SelectedValue & "'," & catStat & ",GETDATE(),'" & ddlServType.SelectedItem.Text & "')")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridView1.DataBind()
                        End Using
                    End Using
                    alert("Records Successfully Added!")
                    clear()
                End Using
            End Using
            Me.BindData()
        End If
    End Sub
    Private Sub BindData()
        Dim strQuery As String = ("SELECT TOP 1000 ps.ServiceName,psc.ID, psc.PayitServiceID,psc.Commission,psc.isCommissionMandatory,psc.CommissionInfo,psc.isAmountEditable,psc.KeypadType,psc.Status,psc.CreatedDate,psc.Info4 FROM PayitServicesConfig psc LEFT JOIN payit.dbo.PayitServices ps on psc.PayitServiceID = ps.ID order by psc.ID desc;")
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
    Protected Sub Edit(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        txtID.Text = row.Cells(0).Text

        If row.Cells(12).Text = "None" Or row.Cells(12).Text = "&nbsp;" Or row.Cells(12).Text = "" Then
            ddlServiceEdit.SelectedValue = "None"
        Else
            ddlServiceEdit.SelectedValue = row.Cells(12).Text
        End If

        If row.Cells(2).Text = "None" Or row.Cells(2).Text = "&nbsp;" Or row.Cells(2).Text = "" Then
            txtCommissionEdit.Text = ""
        Else
            txtCommissionEdit.Text = row.Cells(2).Text
        End If


        If row.Cells(8).Text = "False" Or row.Cells(8).Text = "&nbsp;" Or row.Cells(8).Text = "" Then
            chkComMan.Checked = False
        Else
            chkComMan.Checked = True
        End If

        If row.Cells(4).Text = "None" Or row.Cells(4).Text = "&nbsp;" Or row.Cells(4).Text = "" Then
            txtComMsgEdit.Text = ""
        Else
            txtComMsgEdit.Text = row.Cells(4).Text
        End If

        If row.Cells(9).Text = "True" Then
            chkAmtEdit2.Checked = True
        Else
            chkAmtEdit2.Checked = False
        End If

        If row.Cells(10).Text = "1" Then
            ddlKeypadEdit.SelectedIndex = 1
        ElseIf (row.Cells(10).Text = "2") Then
            ddlKeypadEdit.SelectedIndex = 2
        Else
            ddlKeypadEdit.SelectedIndex = 0
        End If

        If row.Cells(11).Text = "True" Then
            chkStatEdit.Checked = True
        Else
            chkStatEdit.Checked = False
        End If
        'txtPriority.Text = row.Cells(7).Text
        popup.Show()
        alert("You can now edit")
    End Sub
    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        Dim catStatEdit, chkCommIntEdit, chkAmtIntEdit As Integer
        If chkStatEdit.Checked = True Then
            catStatEdit = 1
        Else
            catStatEdit = 0
        End If
        If chkAmtEdit2.Checked = True Then
            chkAmtIntEdit = 1
        Else
            chkAmtIntEdit = 0
        End If
        If chkComMan.Checked = True Then
            chkCommIntEdit = 1
        Else
            chkCommIntEdit = 0
        End If

        Using con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand("UPDATE PayitServicesConfig SET PayitServiceID = '" & ddlServiceEdit.SelectedValue & "', Commission = '" & txtCommissionEdit.Text & "', isCommissionMandatory = " & chkCommIntEdit & ", CommissionInfo = '" & txtComMsgEdit.Text & "', isAmountEditable = " & chkAmtIntEdit & ",KeypadType = '" & ddlKeypadEdit.SelectedItem.Value & "', Status = " & catStatEdit & ", Info4 = '" & ddlServiceEdit.SelectedItem.Text & "'  WHERE ID = " & txtID.Text.Trim())
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GridView1.DataBind()
                    End Using
                End Using
            End Using
            Me.BindData()
            alert("Updated Successfully")
        End Using
    End Sub
    Protected Sub Delete(ByVal sender As Object, ByVal e As EventArgs)
        Using con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand("DELETE FROM [payit].[dbo].[PayitServicesConfig] WHERE ID = " & txtID.Text.Trim())
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GridView1.DataBind()
                    End Using
                End Using
            End Using
            Me.BindData()
        End Using
        alert("Deleted Commission")
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("PayitServicesConfig.xls", Me.GridView1)
    End Sub
    Protected Sub clear()
        txtCommission.Text = String.Empty
        txtCommMsg.Text = String.Empty
        ddlServType.SelectedIndex = 0
        chkAmountEdit.Checked = False
        ddlKeypad.SelectedIndex = 1
        chkStatus.Checked = False
    End Sub
End Class
