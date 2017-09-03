Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Public Class CatServices
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sql As String
    Dim sqlLocalTrans As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "qa") Then
            alert("You do not have authorization to access this page. Contact Administrator for support.")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=CatServices.aspx")
        End If

        If Not IsPostBack Then
            btnEdit.Visible = False
            Me.BindData()
        End If

        If IsPostBack Then
            ClientScript.RegisterClientScriptBlock([GetType](), "IsPostBack", "var isPostBack = true;", True)
        Else
            ClientScript.RegisterClientScriptBlock([GetType](), "IsPostBack", "var isPostBack = false;", True)
        End If

        If Not IsPostBack Then
            'cn = New SqlConnection(strConnString)

            'sql = "Select DISTINCT Type from PayitServices"

            'da = New SqlDataAdapter(sql, cn)
            'ds = New DataSet()
            'da.Fill(ds, "deta")
            'ddlType.DataSource = ds.Tables("deta")
            'ddlType.DataTextField = "Type"
            'ddlType.DataBind()
            'ddlType.Items.Insert(0, New ListItem("Select", "None"))

            ddlknetPay.Items.Insert(0, New ListItem("Select", "None"))
            ddlknetPay.Items.Insert(1, New ListItem("isys", "isys"))
            ddlknetPay.Items.Insert(2, New ListItem("isys3", "isys3"))
            ddlknetPay.Items.Insert(3, New ListItem("payit", "payit"))

            ddlStatus.Items.Insert(0, New ListItem("Select", "None"))
            ddlStatus.Items.Insert(1, New ListItem("Active", "1"))
            ddlStatus.Items.Insert(2, New ListItem("Inactive", "0"))

            ddlStatusNew.Items.Insert(0, New ListItem("Select", "None"))
            ddlStatusNew.Items.Insert(1, New ListItem("Active", "1"))
            ddlStatusNew.Items.Insert(2, New ListItem("Inactive", "0"))

            ddlType.Items.Insert(0, New ListItem("Select", "None"))
            ddlType.Items.Insert(1, New ListItem("PIN", "PIN"))
            ddlType.Items.Insert(2, New ListItem("Payment", "Payment"))
            ddlType.Items.Insert(3, New ListItem("Exchange", "Exchange"))
            ddlType.Items.Insert(4, New ListItem("MyPayments", "MyPayments"))
            ddlType.Items.Insert(5, New ListItem("Recharge", "Recharge"))
            ddlType.Items.Insert(6, New ListItem("App", "App"))
            ddlType.Items.Insert(7, New ListItem("Intl", "Intl"))
            ddlType.Items.Insert(8, New ListItem("ZakatProject", "ZakatProject"))
        End If
    End Sub
    Private Sub BindData()
        Dim strQuery As String = ("Select * from PayitServices order by id desc")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
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
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If Not (Session("role") = "superadmin" Or Session("role") = "operations") Then
            alert("You do not have authorization to access this page. Contact Administrator for support.")
            Exit Sub
        End If
        If Page.IsValid Then
            Dim query As String
            Dim cn As SqlConnection = New SqlConnection(strConnString)
            If (txtServiceCode.Text = "" Or txtServiceName.Text = "" Or ddlStatus.SelectedItem.Value = "Select" Or ddlType.SelectedItem.Value = "Select") Then
                alert("Required Values Cannot be Empty")
            Else
                Dim sql1 As String = "SELECT * FROM PayitServices WHERE ServiceCode=@serCode and ServiceName=@serName"
                Dim cmd1 As SqlCommand = New SqlCommand(sql1, cn)
                cmd1.Connection = cn
                cn.Open()
                cmd1.Parameters.AddWithValue("@serCode", txtServiceCode.Text)
                cmd1.Parameters.AddWithValue("@serName", txtServiceName.Text)
                Using reader As SqlDataReader = cmd1.ExecuteReader()
                    If reader.HasRows Then
                        ' Record already exists
                        alert("Service Already Exist!")
                    Else
                        Using con As New SqlConnection(strConnString)
                            query = "INSERT INTO PayitServices (ServiceCode, ServiceName, ServiceDescription, Status, KNetPaymentTunnel, MinAmtThreshold, Logo, Type, PaymentChannels, Help, ServiceNameAR, ServiceDescriptionAR, HelpAR,ServiceOrder,StatusNew,LogoNew) VALUES (@serCode,@serName,@serDesc,@stat,,@kPay,@minAT,@logo,@type,@payChannel,@help,@serNameAR,@serDescAR,@helpAR,@serOrder,@statNew,@logoNew)"
                            Using cmd As New SqlCommand(query)
                                cmd.Parameters.AddWithValue("@serCode", txtServiceCode.Text)
                                cmd.Parameters.AddWithValue("@serName", txtServiceName.Text)
                                cmd.Parameters.AddWithValue("@serDesc", serviceDesc.Text)
                                cmd.Parameters.AddWithValue("@stat", ddlStatus.SelectedItem.Value)
                                cmd.Parameters.AddWithValue("@kPay", ddlknetPay.SelectedItem.Value)
                                cmd.Parameters.AddWithValue("@minAT", minAmnt.Text)
                                cmd.Parameters.AddWithValue("@logo", logoURL.Text)
                                cmd.Parameters.AddWithValue("@type", ddlType.SelectedItem.Text)
                                cmd.Parameters.AddWithValue("@payChannel", payChannel.Text)
                                cmd.Parameters.AddWithValue("@help", Help.Text)
                                cmd.Parameters.AddWithValue("@serNameAR", ServiceNameAR.Text)
                                cmd.Parameters.AddWithValue("@serDescAR", ServiceDescriptionAR.Text)
                                cmd.Parameters.AddWithValue("@helpAR", HelpAR.Text)
                                cmd.Parameters.AddWithValue("@serOrder", txtSerOrder.Text)
                                cmd.Parameters.AddWithValue("@statNew", ddlStatusNew.SelectedValue)
                                cmd.Parameters.AddWithValue("@logoNew", txtLogoNew.Text)
                                Using sda As New SqlDataAdapter()
                                    cmd.Connection = con
                                    sda.SelectCommand = cmd
                                    Using dt As New DataTable()
                                        sda.Fill(dt)
                                        GridView1.DataBind()
                                        Me.BindData()
                                    End Using
                                End Using
                            End Using
                        End Using
                        alert("Service Successfully Added!")
                    End If
                End Using
                cn.Close()
            End If
        End If
    End Sub
    Protected Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If Page.IsValid Then
            Dim query As String
            Dim cn As SqlConnection = New SqlConnection(strConnString)
            If (txtServiceCode.Text = "" Or txtServiceName.Text = "" Or ddlStatus.SelectedItem.Value = "Select" Or ddlType.SelectedItem.Value = "Select") Then
                alert("Required Values Cannot be Empty")
            Else
                Using con As New SqlConnection(strConnString)
                    query = "UPDATE PayitServices SET ServiceCode=@serCode,ServiceName=@serName,ServiceDescription=@serDesc,Status=@stat,KNetPaymentTunnel=@kPay,MinAmtThreshold=@minAT,Logo=@logo,Type=@type,PaymentChannels=@payChannel,Help=@help,ServiceNameAR=@serNameAR,ServiceDescriptionAR=@serDescAR,HelpAR=@helpAR,ServiceOrder=@serOrder,StatusNew=@statNew,LogoNew=@logoNew  WHERE ID = " & txtID.Text.Trim()
                    Using cmd As New SqlCommand(query)
                        cmd.Parameters.AddWithValue("@serCode", txtServiceCode.Text)
                        cmd.Parameters.AddWithValue("@serName", txtServiceName.Text)
                        cmd.Parameters.AddWithValue("@serDesc", serviceDesc.Text)
                        cmd.Parameters.AddWithValue("@stat", ddlStatus.SelectedItem.Value)
                        cmd.Parameters.AddWithValue("@kPay", ddlknetPay.SelectedItem.Value)
                        cmd.Parameters.AddWithValue("@minAT", minAmnt.Text)
                        cmd.Parameters.AddWithValue("@logo", logoURL.Text)
                        cmd.Parameters.AddWithValue("@type", ddlType.SelectedItem.Text)
                        cmd.Parameters.AddWithValue("@payChannel", payChannel.Text)
                        cmd.Parameters.AddWithValue("@help", Help.Text)
                        cmd.Parameters.AddWithValue("@serNameAR", ServiceNameAR.Text)
                        cmd.Parameters.AddWithValue("@serDescAR", ServiceDescriptionAR.Text)
                        cmd.Parameters.AddWithValue("@helpAR", HelpAR.Text)
                        cmd.Parameters.AddWithValue("@serOrder", txtSerOrder.Text)
                        cmd.Parameters.AddWithValue("@statNew", ddlStatusNew.SelectedValue)
                        cmd.Parameters.AddWithValue("@logoNew", txtLogoNew.Text)
                        Using sda As New SqlDataAdapter()
                            cmd.Connection = con
                            sda.SelectCommand = cmd
                            Using dt As New DataTable()
                                sda.Fill(dt)
                                GridView1.DataBind()
                                Me.BindData()
                            End Using
                        End Using
                    End Using
                End Using
                alert("Service Edited")
                cn.Close()
            End If
        End If
    End Sub
    Protected Sub Edit(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        lblHeading.Text = "Edit Service"
        btnSubmit.Visible = False
        btnEdit.Visible = True
        txtID.Text = row.Cells(0).Text
        txtServiceCode.Text = row.Cells(1).Text.Replace("&nbsp;", "")
        txtServiceName.Text = row.Cells(2).Text.Replace("&nbsp;", "")
        serviceDesc.Text = row.Cells(3).Text.Replace("&nbsp;", "")
        'row(4)--TemplateField
        If row.Cells(5).Text = "None" Or row.Cells(5).Text = "&nbsp;" Then
            ddlStatus.SelectedValue = "None"
        ElseIf row.Cells(5).Text = "True" Then
            ddlStatus.SelectedIndex = 1
        ElseIf row.Cells(5).Text = "False" Then
            ddlStatus.SelectedIndex = 2
        End If

        If row.Cells(6).Text = "None" Or row.Cells(6).Text = "&nbsp;" Then
            ddlknetPay.SelectedValue = "None"
        ElseIf (row.Cells(6).Text = "isys") Then
            ddlknetPay.SelectedIndex = 1
        ElseIf (row.Cells(6).Text = "isys3") Then
            ddlknetPay.SelectedIndex = 2
        End If

        minAmnt.Text = row.Cells(7).Text.Replace("&nbsp;", "")
        logoURL.Text = row.Cells(8).Text.Replace("&nbsp;", "")
        'row(9)--TemplateField
        If row.Cells(10).Text = "None" Or row.Cells(10).Text = "&nbsp;" Then
            ddlType.SelectedValue = "None"
        Else
            ddlType.SelectedItem.Value = row.Cells(10).Text
            ddlType.SelectedItem.Text = row.Cells(10).Text
        End If
        'row(11)--TemplateField
        payChannel.Text = row.Cells(12).Text.Replace("&nbsp;", "")
        Help.Text = row.Cells(13).Text.Replace("&nbsp;", "")
        'row(14)--TemplateField
        ServiceNameAR.Text = row.Cells(15).Text.Replace("&nbsp;", "")
        'row(16)--TemplateField
        ServiceDescriptionAR.Text = row.Cells(17).Text.Replace("&nbsp;", "")
        'row(18)--TemplateField
        HelpAR.Text = row.Cells(19).Text.Replace("&nbsp;", "")
        txtSerOrder.Text = row.Cells(20).Text.Replace("&nbsp;", "")

        If row.Cells(21).Text = "None" Or row.Cells(21).Text = "&nbsp;" Then
            ddlStatusNew.SelectedValue = "None"
        ElseIf row.Cells(21).Text = "True" Then
            ddlStatusNew.SelectedIndex = 1
        ElseIf row.Cells(21).Text = "False" Then
            ddlStatusNew.SelectedIndex = 2
        End If

        txtLogoNew.Text = row.Cells(22).Text.Replace("&nbsp;", "")
        alert("You can now edit the service")

    End Sub
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("Services.xls", Me.GridView1)
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        Me.BindData()
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataBind()
    End Sub
    Protected Sub GridView1_PageIndexChanged(sender As Object, e As EventArgs)

    End Sub
End Class