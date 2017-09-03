Imports Data
Imports System.Data.SqlClient
Imports System.Data
Partial Class RatingValidations
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sql, sql2, strSQL As String
    Dim dispAmnt, info1 As String
    Dim sqlLocalTrans As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Dim Config As String = String.Empty
    Dim db As New payitEntities()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "operations" Or Session("role") = "accounts" Or Session("role") = "qa") Then
            alert("Unauthorized Access")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=RatingValidations.aspx")
        End If
      
        If Not IsPostBack Then
            btnSave.Visible = False
            btnDelete.Visible = False
            Me.BindData()
            Dim sql = (From c In db.PayitServices Order By c.ServiceCode Select New With {Key .Service = c.ServiceName, Key .ID = c.ID}).ToList()

            ddlServiceType.DataSource = sql
            ddlServiceType.DataTextField = "Service"
            ddlServiceType.DataValueField = "ID"
            ddlServiceType.DataBind()
            ddlServiceType.Items.Insert(0, New ListItem("Select", "None"))
        End If
    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If Not (Session("role") = "superadmin" Or Session("role") = "operations" Or Session("role") = "accounts" Or Session("role") = "qa") Then
            alert("Unauthorized Access")
            Exit Sub
        End If

        If Page.IsValid Then
            If (txtRating Is Nothing) Then
                alert("Please enter a Rating")
            Else

                Dim catStat As String
                If chkStatus.Checked = True Then
                    catStat = 1
                Else
                    catStat = 0
                End If

                Dim rows = (From c In db.KYCRatingValidations Where c.ValidationType = 2 And c.ConfigID = ddlServiceType.SelectedValue.Trim() And c.MinAmt = txtminamt.Text.Trim() And c.MaxAmt = txtmaxamt.Text.Trim() And c.KYCRating = txtRating.Text.Trim() Select c.ID).ToList()
                If rows.Count <> 0 Then
                    ' Record already exists
                    alertme("Validation Already exists for:<br><b> " & ddlServiceType.SelectedItem.Text & " </b>")
                Else
                    ' Record does not exist, add them
                    Dim kyc As New KYCRatingValidation
                    kyc.ValidationType = 2
                    kyc.ConfigID = ddlServiceType.SelectedValue.Trim()
                    kyc.MinAmt = txtminamt.Text.Trim()
                    kyc.MaxAmt = txtmaxamt.Text.Trim()
                    kyc.CreatedOn = DateTime.Now
                    kyc.Status = catStat
                    kyc.KYCRatingService = 1
                    kyc.KYCRatingTypes = 1
                    kyc.KYCRating = txtRating.Text.Trim()
                    db.KYCRatingValidations.Add(kyc)
                    db.SaveChanges()
                End If
                Clear()
                Me.BindData()
            End If
        End If
    End Sub
    Protected Sub Clear()
        txtmaxamt.Text = String.Empty
        txtminamt.Text = String.Empty
        txtRating.Text = String.Empty
        ddlServiceType.SelectedIndex = 0
        chkStatus.Checked = False
    End Sub
    Private Sub BindData()
        Dim sqlQuery = (From c In db.KYCRatingValidations Order By c.ID Descending Select c).ToList
        GridView1.DataSource = sqlQuery
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

        Dim rows = (From c In db.KYCRatingValidations Where c.ID = txtID.Text Select c).FirstOrDefault()
        txtmaxamt.Text = rows.MaxAmt
        txtminamt.Text = rows.MinAmt
        txtRating.Text = rows.KYCRating
        ddlServiceType.SelectedValue = rows.ConfigID

        If (rows.Status = True) Then
            chkStatus.Checked = True
        Else
            chkStatus.Checked = False
        End If
        btnSubmit.Visible = False
        btnSave.Visible = True
        btnDelete.Visible = True
        alert("You can now edit")
    End Sub
    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        If (txtRating.Text = "") Then
            alert("Please fill all fields!")
        Else

            Dim status As Boolean = False
            If (chkStatus.Checked) Then
                status = True
            End If
            Dim update = db.KYCRatingValidations.SingleOrDefault(Function(x) x.ID = txtID.Text.Trim())
            If (update IsNot Nothing) Then
                update.ValidationType = 2
                update.ConfigID = ddlServiceType.SelectedValue.Trim()
                update.MinAmt = txtminamt.Text.Trim()
                update.MaxAmt = txtmaxamt.Text.Trim()
                update.Status = status
                update.KYCRating = txtRating.Text.Trim()
                update.KYCRatingTypes = 1
                update.KYCRatingService = 1
                update.UpdatedOn = DateTime.Now
                db.SaveChanges()
            End If
            Clear()
            Me.BindData()
        End If
    End Sub
    Protected Sub Delete(ByVal sender As Object, ByVal e As EventArgs)
        Dim delete = db.KYCRatingValidations.SingleOrDefault(Function(x) x.ID = txtID.Text.Trim())
        If (delete IsNot Nothing) Then
            db.KYCRatingValidations.Remove(delete)
            If (db.SaveChanges() > 0) Then
                alert("RatingRule Deleted")
            Else
                alert("Error Try Again")
            End If
        End If
        Clear()
        btnSubmit.Visible = True
        btnSave.Visible = False
        btnDelete.Visible = False
        Me.BindData()
    End Sub

    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim RuleType As TableCell = e.Row.Cells(1)
            Dim ConfigType As TableCell = e.Row.Cells(2)
            Dim LimitType As TableCell = e.Row.Cells(3)
            Dim KYCType As TableCell = e.Row.Cells(9)

            If RuleType.Text = 1 Then
                RuleType.Text = "Country"
                If (ConfigType.Text = 1) Then
                    ConfigType.Text = "Kuwait"
                End If
            ElseIf RuleType.Text = 2 Then
                RuleType.Text = "Service"
                Dim rows = (From c In db.PayitServices Where c.ID = ConfigType.Text Select c.ServiceCode).FirstOrDefault()
                ConfigType.Text = rows
            End If

            If (KYCType.Text = kyctypes.OneIn) Then
                KYCType.Text = "OneIn"
            ElseIf (KYCType.Text = kyctypes.KycRatingService) Then
                KYCType.Text = "KycRatingService"
            ElseIf (KYCType.Text = kyctypes.Both) Then
                KYCType.Text = "Both"
            End If

        End If
    End Sub

    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Public Sub alertme(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
                      "alertify.alert('" & s & "')" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("RatingValidations.xls", Me.GridView1)
    End Sub

    Private Function SaveData(ByVal kyc As KYCRatingValidation) As String
        Dim result As String = "Error"
        Dim rows = db.KYCRatingValidations.Add(kyc)
        If (db.SaveChanges() > 0) Then
            result = "Success"
        End If
        Return result
    End Function

    Public Enum kyctypes As Integer
        OneIn = 1
        KycRatingService = 2
        Both = 3
    End Enum
End Class
