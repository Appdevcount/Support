Imports System.Data.SqlClient
Imports System.Data
Imports Data
Imports Data.payitEntities

Partial Class PINPriority
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
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=PINPriority.aspx")
        End If

        If Not IsPostBack Then

            Me.BindData()
            cn = New SqlConnection(strConnString)
            Sql = "Select [ServiceName],[ServiceID] from [Services] WHERE ServiceName like '%-o' order by ServiceName"
            da = New SqlDataAdapter(Sql, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")
            ddlService.DataSource = ds.Tables("deta")
            ddlService.DataTextField = "ServiceName"
            ddlService.DataValueField = "ServiceID"
            ddlService.DataBind()
            ddlService.Items.Insert(0, New ListItem("Select", "None"))

            Dim sql2 = "SELECT [Vendor] FROM [payit].[dbo].[PINVendors] where Status = 1 ORDER BY Vendor"
            da = New SqlDataAdapter(sql2, cn)
            ds = New DataSet()
            da.Fill(ds, "data")
            ddlPriority.DataSource = ds.Tables("data")
            ddlPriority.DataTextField = "Vendor"
            ddlPriority.DataValueField = "Vendor"
            ddlPriority.DataBind()
            ddlPriority.Items.Insert(0, New ListItem("Select Priority", "None"))


            'ddlPriority.Items.Insert(0, New ListItem("Select Priority", "None"))
            'ddlPriority.Items.Insert(1, New ListItem("MConnect", "MConnect"))
            'ddlPriority.Items.Insert(2, New ListItem("Blink", "Blink"))
            'ddlPriority.Items.Insert(3, New ListItem("Cameo", "Cameo"))
            'ddlPriority.Items.Insert(4, New ListItem("Dana", "Dana"))
            'ddlPriority.Items.Insert(5, New ListItem("Kuwait Star", "Kuwait Star"))


            ddlPriorityEdit.Items.Insert(0, New ListItem("Select Priority", "None"))
            ddlPriorityEdit.Items.Insert(1, New ListItem("Blink", "Blink"))
            ddlPriorityEdit.Items.Insert(2, New ListItem("Cameo", "Cameo"))
            ddlPriorityEdit.Items.Insert(3, New ListItem("Dana", "Dana"))
            ddlPriorityEdit.Items.Insert(4, New ListItem("ENet", "ENet"))
            ddlPriorityEdit.Items.Insert(5, New ListItem("HA-OG", "HA-OG"))
            ddlPriorityEdit.Items.Insert(6, New ListItem("Kuwait Star", "Kuwait Star"))
            ddlPriorityEdit.Items.Insert(7, New ListItem("MConnect", "MConnect"))

        End If
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        If Page.IsValid Then
            If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations") Then
                alert("You are not authorized to perform this action.")
                Exit Sub
            End If

            Dim cn As SqlConnection = New SqlConnection(strConnString)
            If (ddlService.SelectedItem.Text = "Select" Or ddlPriority.SelectedItem.Text = "Select Priority") Then
                alert("Please fill all fields!")
                'dberrorlabel.Text = "Please Fill all fields!"
            Else
                Dim catStat As String
                If chkStatus.Checked = True Then
                    catStat = 1
                Else
                    catStat = 0
                End If
                Dim sql1 As String = "SELECT * FROM PINPriority WHERE [Service] like @service and [Denomination] like @denomination"
                Dim db As New payitEntities()

                Dim cmd1 As SqlCommand = New SqlCommand(sql1, cn)
                cmd1.Connection = cn
                cn.Open()
                cmd1.Parameters.AddWithValue("@service", ddlService.SelectedItem.Text)
                cmd1.Parameters.AddWithValue("@denomination", ddlDenomination.SelectedValue)
                Using reader As SqlDataReader = cmd1.ExecuteReader()
                    If reader.HasRows Then
                        ' Record already exists
                        alert("Priority Already Exist!")
                    Else
                        ' Record does not exist, add them
                        Dim cmd As SqlCommand = New SqlCommand("INSERT INTO PINPriority (Service,Denomination,CreatedDate,Status,Priority) VALUES ('" & ddlService.SelectedItem.Text.Trim() & "','" & ddlDenomination.SelectedValue.Trim() & "',GETDATE()," & catStat & ",'" & ddlPriority.SelectedItem.Text & "')")
                        cmd.Connection = cn
                        Dim insert As Integer = cmd.ExecuteNonQuery()
                        If (insert = 1) Then
                            alert("Priority Successfully Assigned!")
                        Else
                            alert("Error! Try Again")
                        End If
                    End If
                End Using
                cn.Close()
                Clear()
                GridView1.DataBind()
                Me.BindData()
            End If
        End If
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
        ddl.Items.Insert(0, New ListItem(defaultText, "0"))
    End Sub
    Protected Sub ddlService_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlDenomination.Enabled = False
        ddlDenomination.Items.Clear()
        Dim serviceId As String = ddlService.SelectedValue
        If serviceId <> Nothing Then
            Dim query As String = String.Format("SELECT [Amount2],[Amount] from [Denominations] where [ServiceID] = '{0}' ORDER BY convert(float,Amount) ", serviceId)
            BindDropDownList(ddlDenomination, query, "Amount2", "Amount", "Select Denomination")
            ddlDenomination.Enabled = True
        End If
    End Sub
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Protected Sub Clear()
        ddlService.SelectedIndex = 0
        ddlPriority.SelectedIndex = 0
        ddlDenomination.Items.Clear()
        chkStatus.Checked = False
    End Sub
    Private Sub BindData()
        Dim strQuery As String = ("select TOP 1000 ID, Service, Denomination, Priority, CreatedDate, Status from [PINPriority] order by ID desc")
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

        If row.Cells(1).Text = "None" Or row.Cells(1).Text = "Select" Or row.Cells(1).Text = "&nbsp;" Or row.Cells(1).Text = "" Then
            ServiceVal.Text = "No Service"
            ServiceVal.Font.Bold = True
        Else
            ServiceVal.Text = "Service: " & row.Cells(1).Text
            ServiceVal.CssClass = "text-info"
            ServiceVal.Font.Bold = True
        End If

        If row.Cells(2).Text = "None" Or row.Cells(2).Text = "Select Denomination" Or row.Cells(2).Text = "&nbsp;" Or row.Cells(2).Text = "" Then
            DenominationVal.Text = "No Denomination"
            DenominationVal.Font.Bold = True
        Else
            DenominationVal.Text = "Denomination: " & row.Cells(2).Text
            DenominationVal.CssClass = "text-info"
            DenominationVal.Font.Bold = True
        End If

        If row.Cells(3).Text = "None" Or row.Cells(3).Text = "Select Priority" Or row.Cells(3).Text = "&nbsp;" Or row.Cells(3).Text = "" Then
            ddlPriorityEdit.SelectedIndex = 0
        Else
            ddlPriorityEdit.SelectedValue = row.Cells(3).Text
        End If

        If row.Cells(5).Text = "True" Then
            chkStatEdit.Checked = True
        Else
            chkStatEdit.Checked = False
        End If

      
        'txtPriority.Text = row.Cells(7).Text
        popup.Show()
        alert("You can now edit")
    End Sub
    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        Dim catStatEdit As Integer
        If (ddlPriorityEdit.SelectedIndex = 0) Then
            alert("Please fill all fields!")
            'dberrorlabel.Text = "Please Fill all fields!"
        Else
            If chkStatEdit.Checked = True Then
                catStatEdit = 1
            Else
                catStatEdit = 0
            End If

            Using con As New SqlConnection(strConnString)
                Using cmd As New SqlCommand("UPDATE [payit].[dbo].[PINPriority] SET [Priority] = '" & ddlPriorityEdit.SelectedItem.Text & "', Status = " & catStatEdit & "  WHERE ID = " & txtID.Text.Trim())
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
                alert("Priority Updated Successfully")
            End Using
        End If
    End Sub
    Protected Sub Delete(ByVal sender As Object, ByVal e As EventArgs)
        Using con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand("DELETE FROM [payit].[dbo].[PINPriority] WHERE ID = " & txtID.Text.Trim())
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
        alert("Deleted Priority")
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("PINPriority.xls", Me.GridView1)
    End Sub

    Protected Sub btnSubmit_Load(sender As Object, e As EventArgs) Handles btnSubmit.Load

    End Sub
End Class
