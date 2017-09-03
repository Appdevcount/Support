Imports System.Data.SqlClient
Imports System.Data
Partial Class MConnectPinFetch
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sql, sql2, strSQL As String
    Dim category As String = String.Empty, brand As String = String.Empty, service As String = String.Empty
    Dim value As String = String.Empty, currency As String = String.Empty, denomination As String = String.Empty
    Dim brandpart As String = String.Empty
    Dim sqlLocalTrans As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "popupScript", popupscript, True)
    End Sub
    Public Sub alertme(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
                      "alertify.alert('" & s & "')" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=MConnectPinFetch.aspx")
        End If

        If Not IsPostBack Then
            Me.BindData()
            GridView1.DataBind()

            cn = New SqlConnection(strConnString)
            sql = "SELECT [ServiceCode],[ServiceID] FROM [payit].[dbo].[StockRepository] WHERE Status = 1 ORDER BY ID"
            da = New SqlDataAdapter(sql, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")
            ddlServType.DataSource = ds.Tables("deta")
            ddlServType.DataTextField = "ServiceCode"
            ddlServType.DataValueField = "ServiceID"
            ddlServType.DataBind()
            ddlServType.Items.Insert(0, New ListItem("Select", "None"))

            'ddlQuantity.Items.Insert(0, New ListItem("Select Quantity", "0"))
            'ddlQuantity.Items.Insert(1, New ListItem("20", "20"))
            'ddlQuantity.Items.Insert(2, New ListItem("30", "30"))
            'ddlQuantity.Items.Insert(3, New ListItem("40", "40"))
            'ddlQuantity.Items.Insert(4, New ListItem("50", "50"))
            'ddlQuantity.Items.Insert(5, New ListItem("60", "60"))
            'ddlQuantity.Items.Insert(6, New ListItem("70", "70"))
            'ddlQuantity.Items.Insert(7, New ListItem("80", "80"))
            'ddlQuantity.Items.Insert(8, New ListItem("90", "90"))
            'ddlQuantity.Items.Insert(9, New ListItem("100", "100"))

        End If
    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim insert As Integer = 0
        If Page.IsValid Then
            Dim cn As SqlConnection = New SqlConnection(strConnString)
            If (ddlServType.SelectedIndex = 0 Or ddlDenomination.SelectedIndex = 0 Or txtQuantity.Text = "") Then
                alertme("Please fill all fields!")
                'dberrorlabel.Text = "Please Fill all fields!"
            Else
                If (ddlServType.SelectedIndex <> 0) Then
                    Dim sqlQ As String = "SELECT [category],[brand] from [payit].[dbo].[StockRepository] Where ServiceCode like '" & ddlServType.SelectedItem.Text & "'"
                    da = New SqlDataAdapter(sqlQ, cn)
                    If (cn.State = ConnectionState.Closed) Then
                        cn.Open()
                    End If
                    ds = New DataSet()
                    da.Fill(ds, "data")
                    If Me.IsPostBack Then
                        If ds.Tables.Count > 0 Then
                            Try
                                For Each myRow As DataRow In ds.Tables(0).Rows
                                    If Not IsDBNull(myRow("category")) AndAlso myRow("category") <> Nothing Then
                                        category = myRow("category").ToString().Trim()
                                    End If
                                    If Not IsDBNull(myRow("brand")) AndAlso myRow("brand") <> Nothing Then
                                        brand = myRow("brand").ToString().Trim()
                                    End If
                                Next
                            Catch ex As Exception
                                alert(ex.Message)
                            End Try
                        End If
                    End If
                    If (cn.State = ConnectionState.Open) Then
                        cn.Close()
                    End If
                End If
                Dim Subdenomination As String = ddlDenomination.SelectedItem.Text.ToLower()

                If (ddlDenomination.Items.Count > 0) Then
                    If (Subdenomination.ToLower().Contains("months") Or Subdenomination.ToLower().Contains("point")) Then
                        denomination = ddlDenomination.SelectedItem.Text

                    Else
                        Dim s As String = ddlDenomination.SelectedItem.Text
                        Dim parts As String() = s.Split(New Char() {" "c})
                        value = Trim(parts(0))
                        currency = Trim(parts(1))
                        denomination = "$" & value.Trim()
                    End If
                End If
                service = ddlServType.SelectedItem.Text.Trim()

                Dim quantityordered As Integer = 0
                If txtQuantity.Text <> "" Then
                    quantityordered = txtQuantity.Text.Trim()
                End If
                Dim amnt As String = String.Empty
                amnt = ddlDenomination.SelectedValue.Trim()

                Dim amnt2 As String = String.Empty
                If denomination <> "" Or denomination <> Nothing Then
                    amnt2 = ddlDenomination.SelectedItem.Text.Trim()
                End If

                Using con As New SqlConnection(strConnString)
                    Dim Query As String = "INSERT INTO [payit].[dbo].[StockOrders]([Category], [Brand], [Service], [Denomination], [QuantityOrdered], [Amount], [Amount2])" & _
                                            "VALUES (@category,@brand,@service,@Denomination,@Quantity,@Amount,@Amount2)"
                    Using cmd As New SqlCommand(Query)
                        cmd.Parameters.AddWithValue("@category", category)
                        cmd.Parameters.AddWithValue("@brand", brand)
                        cmd.Parameters.AddWithValue("@service", service)
                        cmd.Parameters.AddWithValue("@Denomination", denomination)
                        cmd.Parameters.AddWithValue("@Quantity", quantityordered)
                        cmd.Parameters.AddWithValue("@Amount", amnt)
                        cmd.Parameters.AddWithValue("@Amount2", amnt2)

                        Using sda As New SqlDataAdapter()
                            cmd.Connection = con
                            If (con.State = ConnectionState.Closed) Then
                                con.Open()
                            End If
                            sda.SelectCommand = cmd
                            insert = cmd.ExecuteNonQuery()
                            If (con.State = ConnectionState.Open) Then
                                con.Close()
                            End If
                        End Using
                    End Using
                    Me.BindData()
                    If (insert = 1) Then
                        alert("Order Placed Added")
                    Else
                        alert("Error. Try Again")
                    End If
                    Clear()
                End Using
            End If
        End If
    End Sub
    Protected Sub Clear()
        ddlServType.SelectedIndex = 0
        txtQuantity.Text = ""
        ddlDenomination.Items.Clear()
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
    Protected Sub ddlServType_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlDenomination.Enabled = False
        ddlDenomination.Items.Clear()
        'ddlDenomination.Items.Insert(0, New ListItem("Select Denomination", "NULL"))
        Dim serviceId As String = ddlServType.SelectedItem.Text
        If serviceId <> Nothing Then
            Dim query As String = String.Format("SELECT sr.ServiceCode, d.Amount2 As Denomination, d.Amount from Denominations d " & _
            "left join StockRepository sr on d.ServiceID=sr.ServiceID  " & _
            "left join StockDenominations sd on sr.ID=sd.StockRepositoryID " & _
            "where sr.ServiceCode like '{0}' and d.Amount2=sd.Denomination order by cast(d.Amount as float)", serviceId)
            BindDropDownList(ddlDenomination, query, "Denomination", "Amount", "Select Denomination")
            ddlDenomination.Enabled = True
        End If
    End Sub
    Protected Sub ddlDenomination_SelectedIndexChanged(sender As Object, e As EventArgs)
        If (ddlDenomination.Items.Count > 0) Then
            Amounttxt.Text = "Amount KD: " & ddlDenomination.SelectedValue
        End If
    End Sub

    Private Sub BindData()
        Dim strQuery As String = ("select TOP 1000 ID, [Category], [Brand], [Service], [Denomination], [QuantityOrdered], [Amount], [Amount2], [QuantityServed],[Status] ,[TransDate] from [payit].[dbo].[StockOrders] order by ID desc")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        Me.BindData()
        GridView1.PageIndex = e.NewPageIndex
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
        If (ddlServType.SelectedIndex = 0 Or ddlDenomination.SelectedIndex = 0 Or txtQuantity.Text = "") Then
            alertme("Please fill all fields!")
        Else
            If Not "Select".Equals(ddlServType.SelectedItem.Text()) Then
                ServiceValue.InnerHtml = ddlServType.SelectedItem.Text()
            Else
                ServiceValue.InnerHtml = "None"
            End If

            If ddlDenomination.Items.Count > 0 Then
                DenominationValue.InnerHtml = ddlDenomination.SelectedItem.Text()
            Else
                DenominationValue.InnerHtml = "0 USD"
            End If

            If ddlDenomination.Items.Count > 0 Then
                amount2Value.InnerHtml = ddlDenomination.SelectedValue & " KWD"
            Else
                amount2Value.InnerHtml = "0 KWD"
            End If

            If txtQuantity.Text <> "" Then
                QuantityValue.InnerHtml = txtQuantity.Text()
            Else
                QuantityValue.InnerHtml = ""
            End If
            popup.Show()
        End If
    End Sub
    
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("MconnetPINS.xls", Me.GridView1)
    End Sub
End Class

