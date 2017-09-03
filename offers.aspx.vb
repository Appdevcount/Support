Imports System.Data
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Configuration

Public Class offers
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sql, strSQL As String
    Dim update2 As Integer = 0, update3 As Integer = 0
    Dim dispAmnt, info1, resource As String
    Dim sqlLocalTrans As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Private conn As New SqlConnection("data source=ISYSSERVER87\payit;initial catalog=payit;password=shareef;persist security info=True;user id=sa;workstation id=ISYSSERVER3;packet size=4096")
    Public Class CATEGOREIS
        Dim OFFER As String = "Offer"
        Dim GREETING As String = "Greeting"
        Dim MESSAGE As String = "Message"
    End Class
    Public Class APPLCIATIONS
        Public PAYIT_KUWAIT As String = "PayitKuwait"
        Dim PAYIT_GlOBAL As String = "PayitGlobal"
    End Class
    Public Class PLATFORMS
        Dim iOS As String = "iOS"
        Dim ANDROID As String = "Android"
        Dim BOTH As String = "Both"
    End Class
    Public Class STATUSDESC
        Dim PENDING As String = "PENDING"
        Dim FAILED As String = "FAILED"
        Dim SUCCESS As String = "SUCCESS"
        Dim PROCESSING As String = "PROCESSING"
        Dim REPROCESSING As String = "REPROCESSING"
    End Class
    Public Class LANGUAGES
        Dim ARABIC As String = "ar"
        Dim ENGLISH As String = "en"
        Dim NONE As String = "none"
    End Class
    Private Sub BindDropDownList(ddl As DropDownList, query As String, text As String, value As String, defaultText As String)
        Dim conString As String = ConfigurationManager.ConnectionStrings("payitConnectionActive").ConnectionString
        Dim cmd As New SqlCommand(query)
        Using con As New SqlConnection(conString)
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "qa") Then
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=offers.aspx")
        End If

        If Not IsPostBack Then
            Me.BindData()
            GridView1.DataBind()
        End If
        If Not IsPostBack Then
            FromDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")
            ToDateTextBox.Text = Format(Date.Today.Date, "yyyy-MM-dd")

            If (TxtUpdate3.Text = "3") Then
                btnQ2.Visible = True
            ElseIf (TxtUpdate2.Text = "2") Then
                btnQ3.Visible = True
            End If

            cn = New SqlConnection(strConnString)

            sql = "Select ServiceName, ServiceID from Services order by ServiceName"

            da = New SqlDataAdapter(sql, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")

            ddlServiceName.DataSource = ds.Tables("deta")
            ddlServiceName.DataTextField = "ServiceName"
            ddlServiceName.DataValueField = "ServiceID"
            ddlServiceName.DataBind()
            ddlServiceName.Items.Insert(0, New ListItem("None", "None"))

            ddlType.Items.Insert(0, New ListItem("Select", "None"))
            ddlType.Items.Insert(1, New ListItem("Text", "Text"))
            ddlType.Items.Insert(2, New ListItem("Image", "Image"))
            ddlType.Items.Insert(3, New ListItem("Web", "Web"))

            ddlStatus.Items.Insert(0, New ListItem("Select", "None"))
            ddlStatus.Items.Insert(1, New ListItem("Active", "1"))
            ddlStatus.Items.Insert(2, New ListItem("Inactive", "0"))

            ddlServType.Items.Insert(0, New ListItem("Select", "None"))
            ddlServType.Items.Insert(1, New ListItem("Offer", "Offer"))
            ddlServType.Items.Insert(2, New ListItem("Greeting", "Greeting"))
          
            ddlPlatform.Items.Insert(0, New ListItem("Select", "None"))
            ddlPlatform.Items.Insert(1, New ListItem("iOS", "iOS"))
            ddlPlatform.Items.Insert(2, New ListItem("Android", "Android"))
            ddlPlatform.Items.Insert(3, New ListItem("Both", "Both"))
            If ddlPlatform.Items.Count > 0 Then
                ddlPlatform.SelectedIndex = 3
            End If

        End If
    End Sub
    Protected Sub ddlServType_SelectedIndexChanged(sender As Object, e As EventArgs)
        If (ddlServType.SelectedItem.Value = "Greeting") Then
            Label5.Text = "Title"
            ddlType.SelectedIndex = 1
            ddlServType.SelectedItem.Text = "Greeting"
            ddlServType.SelectedItem.Value = "Greeting"
            LabelServ.Visible = "false"
            ddlServiceName.Visible = "false"
            ddlServiceName.SelectedIndex = 0
            LabelDispAmt.Visible = "False"
            ddlDispAmt.Visible = "False"
            ddlDispAmt.SelectedValue = "0"
            dvDesc.Visible = "true"
            LabelOfferVal.Visible = "false"
            txtOfferVal.Visible = "false"
            txtOfferVal.Text = ""
            dvWebLink.Visible = False
            txtWebLink.Text = ""
            fileUp.Visible = "True"
            LabelType.Visible = "true"
            ddlType.Visible = "true"
            lblMultiple.Visible = False
            lblSingle.Visible = False
            btnSubmit.Text = "Add Greeting"
        ElseIf (ddlServType.SelectedItem.Value = "Offer") Then
            Label5.Text = "Offer Title"
            LabelType.Visible = "false"
            ddlType.Visible = "false"
            ddlType.SelectedIndex = 0
            ddlServType.SelectedItem.Text = "Offer"
            ddlServType.SelectedItem.Value = "Offer"
            dvDesc.Visible = False
            TextMsg.Text = ""
            dvWebLink.Visible = False
            txtWebLink.Text = ""
            fileUp.Visible = "false"
            LabelServ.Visible = "true"
            ddlServiceName.Visible = "true"
            LabelDispAmt.Visible = "true"
            ddlDispAmt.Visible = "true"
            LabelOfferVal.Visible = "true"
            txtOfferVal.Visible = "true"
            lblMultiple.Visible = True
            lblSingle.Visible = True
            btnSubmit.Text = "Add Offer"
        Else
            LabelType.Text = "Type"
            btnSubmit.Text = "Add Offer"
        End If
    End Sub
    Protected Sub ddlServiceName_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlDispAmt.Enabled = False
        ddlDispAmt.Items.Clear()
        ddlDispAmt.Items.Insert(0, New ListItem("Select Amount", "0"))
        Dim serviceId As String = ddlServiceName.SelectedItem.Text
        If serviceId <> Nothing Then
            Dim query As String = String.Format("SELECT distinct d.Amount2 + '-' + d.Amount as DispAmt from Denominations d LEFT JOIN payit.dbo.Services s on d.ServiceID=s.ServiceID where S.ServiceName like '{0}%'", serviceId)
            'Dim query As String = String.Format("SELECT distinct Amount2 + SPACE(1) + ' - ' + SPACE(1) + Amount as DispAmt from Denominations Where ServiceID = {0}", serviceId)
            BindDropDownList(ddlDispAmt, query, "DispAmt", "DispAmt", "Select Amount")
            ddlDispAmt.Enabled = True
        End If
    End Sub
    Protected Sub ddlType_SelectedIndexChanged(sender As Object, e As EventArgs)
        If (ddlType.SelectedItem.Value = "Text") Then
            dvWebLink.Visible = False
            fileUp.Visible = False
        ElseIf (ddlType.SelectedItem.Text = "Web") Then
            dvWebLink.Visible = True
            fileUp.Visible = True
        ElseIf (ddlType.SelectedItem.Text = "Image") Then
            dvWebLink.Visible = False
            fileUp.Visible = True
        End If
    End Sub
    Protected Sub ddlDispAmt_SelectedIndexChanged(sender As Object, e As EventArgs)
    End Sub
    Private Sub BindData()
        Dim strQuery As String = ("select top 1000 * from Promos order by id desc")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
    End Sub
    Public Sub sqldatabind()
        da = New SqlDataAdapter()
        ds = New DataSet()
        da.Fill(ds, "det")
        GridView1.DataSource = ds.Tables("det")
        GridView1.DataBind()
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        Me.BindData()
        GridView1.PageIndex = e.NewPageIndex
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
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "qa") Then
            alert("Not Authorized to perform this action")
            Exit Sub
        End If

        Dim query, webLink As String
        If Page.IsValid Then
            Dim isTest, isSingleOfferV1, isSingleOfferV2, InternalOffer As Integer
            If chkTest.Checked = True Then
                isTest = 1
            Else
                isTest = 0
            End If

            If chkIntOffer.Checked = True Then
                InternalOffer = 1
            Else
                InternalOffer = 0
            End If

            If chkSingleV1.Checked = True Then
                isSingleOfferV1 = 1
            Else
                isSingleOfferV1 = 0
            End If

            If chkSingleV2.Checked = True Then
                isSingleOfferV2 = 1
            Else
                isSingleOfferV2 = 0
            End If

            Dim fn As String = String.Empty
            Dim info2, pushMsg As String
            If chkHome.Checked = True Then
                info2 = "True"
            Else
                info2 = "False"
            End If
            pushMsg = txtPushMsg.Text

            If (txtWebLink.Text = "" Or txtWebLink.Text = "&nbsp;" Or txtWebLink.Text = Nothing) Then
                webLink = ""
            Else
                webLink = txtWebLink.Text
            End If
           
            Dim value As Double = 0
            If (ddlDispAmt.Items.Count > 0) Then
                Dim s As String = ddlDispAmt.SelectedValue
                Dim parts As String() = s.Split(New Char() {"-"c})
                dispAmnt = Trim(parts(0))
                info1 = Trim(parts(1))
                If (info1 <> Nothing) Then
                    value = Double.Parse(info1)
                End If
            End If

            Dim quantity1 As String = String.Empty
            Dim quantity2 As String = String.Empty
            Dim quantity3 As String = String.Empty

            Dim originalValue1 As Double = 0.0
            Dim originalValue2 As Double = 0.0
            Dim originalValue3 As Double = 0.0
            If (chkSingleV2.Checked = True) Then
                quantity1 = "1"
                originalValue1 = value
            End If
            If (chkQ2.Checked = True) Then
                quantity2 = "2"
                originalValue2 = value * 2
            End If
            If (chkQ3.Checked = True) Then
                quantity3 = "3"
                originalValue3 = value * 3
            End If

            If FileUpload1.HasFile = True Then
                Dim validFileTypes As String() = {"png", "jpg", "jpeg", "mp4", "gif", "avi", "nothing"}
                Dim ext As String = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName)
                Dim isValidFile As Boolean = False
                Dim fileChk As Boolean = FileUpload1.HasFile
                For i As Integer = 0 To validFileTypes.Length - 1
                    If ext = "." & validFileTypes(i) Then
                        isValidFile = True
                        Exit For
                    End If
                Next
                If Not isValidFile Then
                    dberrorlabel.Text = "Invalid File. Please upload a File with extension " & String.Join(",", validFileTypes)
                ElseIf IsValid = True Then
                    fn = DateTime.Now.Day.ToString() & "-"
                    fn += DateTime.Now.Month.ToString() & "-"
                    fn += DateTime.Now.Year.ToString() & "."
                    fn += DateTime.Now.Hour.ToString() & "_"
                    fn += DateTime.Now.Minute.ToString() & "_"
                    fn += DateTime.Now.Second.ToString() + ext
                    FileUpload1.SaveAs("C:/inetpub/wwwroot/PayitAds/AdsImages/Promos/Greetings/" & fn)
                    'C:\inetpub\wwwroot\PayitAds\AdsImages\Promos\Greetings
                    dberrorlabel.ForeColor = Drawing.Color.Green
                    dberrorlabel.Text = "File uploaded successfully."
                End If
            End If

            urlHidden.Text = "https://www.pay-it.mobi/PayitAds/AdsImages/Promos/Greetings/" & fn
            Dim fromdate, todate As String
            If Trim(FromDateTextBox.Text) = "" Or Trim(ToDateTextBox.Text) = "" Then
                Label14.Visible = True
                Label15.Visible = True
                Exit Sub
            Else
                Label14.Visible = False
                Label15.Visible = False
                fromdate = Trim(FromDateTextBox.Text) & " 00:00:00"
                todate = Trim(ToDateTextBox.Text) & " 23:58:59"
            End If

            If (ddlType.SelectedItem.Text = "Web") Then
                If (txtWebLink.Text = "") Then
                    dberrorlabel.Text = "WebLink can't be empty"
                End If
            End If

            If (ddlStatus.SelectedItem.Value = "None") Then
                dberrorlabel.Text = "Required Values Cannot be Empty"
            Else
                Dim constring As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
                If (ddlStatus.Text = "True") Then
                    ddlStatus.Text = 1
                ElseIf (ddlStatus.Text = "False" Or ddlStatus.Text = "None") Then
                    ddlStatus.Text = 0
                End If

                If (txtID.Text <> Nothing) Then
                    query = "UPDATE Promos SET Title = '" & txtTitle.Text.Trim() & "', Type = '" & ddlType.SelectedItem.Value & "', ServiceType = '" & ddlServType.SelectedItem.Text & "', Description = '" & TextMsg.Text.Trim() & "', StartDate = '" & fromdate & "', EndDate = '" & todate & "', ServiceCode = '" & ddlServiceName.SelectedItem.Text & "', Status = " & ddlStatus.SelectedItem.Value & ", DisplayAmount = '" & dispAmnt.Trim() & "', OfferValue = '" & txtOfferVal.Text.Trim() & "', info1 = '" & info1.Trim() & "', info2 = '" & info2 & "', info3 = '" & webLink & "', isTesting = " & isTest & ", isSingleOffer=" & isSingleOfferV1 & ", isSingleOfferV2=" & isSingleOfferV2 & ", ShowInService = " & InternalOffer & " WHERE ID = " & txtID.Text.Trim()
                ElseIf (txtWebLink.Text <> Nothing Or txtWebLink.Text <> "") Then
                    query = "INSERT INTO Promos (Title, Type, ServiceType, Description, StartDate, EndDate, CreatedDate, ServiceCode, Url, Status, DisplayAmount, OfferValue, Info1, Info2, Info3, isSingleOffer, isSingleOfferV2, ShowInService, isTesting) OUTPUT inserted.ID VALUES ('" & txtTitle.Text.Trim() & "','" & ddlType.SelectedItem.Value & "','" & ddlServType.SelectedItem.Text & "','" & TextMsg.Text.Trim() & "','" & fromdate & "','" & todate & "',GETDATE(),'" & ddlServiceName.SelectedItem.Text & "',@Url," & ddlStatus.SelectedItem.Value & ",'" & dispAmnt & "','" & txtOfferVal.Text.Trim() & "', '" & info1 & "', '" & info2 & "', '" & txtWebLink.Text.Trim() & "'," & isSingleOfferV1 & "," & isSingleOfferV2 & ", " & InternalOffer & ", " & isTest & ")" & _
                            ";INSERT INTO LogTrace (Page,Service,PaymentChannel,info2,ChangedBy,CreatedOn,info1) VALUES('Offers','ServiceType: " & ddlServType.SelectedItem.Text & " | " & txtTitle.Text.Trim() & "','Service: " & ddlServiceName.SelectedItem.Text & "','Status: " & ddlStatus.SelectedItem.Text & "','" & Session("user") & "',getdate(),'" & Request.UserHostAddress & "')"
                Else
                    query = "INSERT INTO Promos (Title, Type, ServiceType, Description, StartDate, EndDate, CreatedDate, ServiceCode, Url, Status, DisplayAmount, OfferValue, Info1, Info2, isSingleOffer, isSingleOfferV2, ShowInService, isTesting) OUTPUT inserted.ID VALUES ('" & txtTitle.Text.Trim() & "','" & ddlType.SelectedItem.Value & "','" & ddlServType.SelectedItem.Text & "','" & TextMsg.Text.Trim() & "','" & fromdate & "','" & todate & "',GETDATE(),'" & ddlServiceName.SelectedItem.Text & "',@Url," & ddlStatus.SelectedItem.Value & ",'" & dispAmnt & "','" & txtOfferVal.Text.Trim() & "', '" & info1 & "', '" & info2 & "'," & isSingleOfferV1 & ", " & isSingleOfferV2 & ", " & InternalOffer & "," & isTest & ")" & _
                            ";INSERT INTO LogTrace (Page,Service,PaymentChannel,info2,ChangedBy,CreatedOn,info1) VALUES('Offers','ServiceType: " & ddlServType.SelectedItem.Text & " | " & txtTitle.Text.Trim() & "','Service: " & ddlServiceName.SelectedItem.Text & "','Status: " & ddlStatus.SelectedItem.Text & "','" & Session("user") & "',getdate(),'" & Request.UserHostAddress & "')"
                End If

                Dim ID As Integer
                If chkPush.Checked = True Then
                    Using conn As New SqlConnection(strConnString)
                        Using cmd As New SqlCommand(query, conn)
                            If FileUpload1.HasFile = True Then
                                cmd.Parameters.AddWithValue("@Url", "https://www.pay-it.mobi/PayitAds/AdsImages/Promos/Greetings/" & fn)
                            Else
                                cmd.Parameters.AddWithValue("@Url", "")
                            End If
                            cmd.CommandType = CommandType.Text
                            cmd.CommandText = query
                            If (conn.State = ConnectionState.Closed) Then
                                conn.Open()
                            End If
                            ID = cmd.ExecuteScalar()

                            dberrorlabel.Text = "Record Successfully Added!"
                        End Using
                    End Using

                    Using cmd2 As New SqlCommand
                        cmd2.CommandType = CommandType.StoredProcedure
                        cmd2.CommandText = "InsertPustNotiications"
                        cmd2.Parameters.AddWithValue("@Message", txtPushMsg.Text)
                        cmd2.Parameters.AddWithValue("@Platform", ddlPlatform.SelectedItem.Text)
                        cmd2.Parameters.AddWithValue("@Application", "PayitKuwait")
                        cmd2.Parameters.AddWithValue("@Reference", ddlServiceName.SelectedItem.Text)
                        cmd2.Parameters.AddWithValue("@MessageCategory", ddlServType.SelectedItem.Text)
                        cmd2.Parameters.AddWithValue("@ScheduledDate", fromdate)
                        cmd2.Parameters.AddWithValue("@Status", ddlStatus.SelectedItem.Value)
                        cmd2.Parameters.AddWithValue("@TableReferenceID", ID)

                        Using sda As New SqlDataAdapter()
                            cmd2.Connection = conn
                            sda.SelectCommand = cmd2
                            Using dt As New DataTable()
                                sda.Fill(dt)
                            End Using
                        End Using
                    End Using
                Else
                    Using conn As New SqlConnection(strConnString)
                        Using cmd As New SqlCommand(query, conn)
                            If FileUpload1.HasFile = True Then
                                cmd.Parameters.AddWithValue("@Url", "https://www.pay-it.mobi/PayitAds/AdsImages/Promos/Greetings/" & fn)
                            Else
                                cmd.Parameters.AddWithValue("@Url", "")
                            End If
                            cmd.CommandType = CommandType.Text
                            cmd.CommandText = query
                            If (conn.State = ConnectionState.Closed) Then
                                conn.Open()
                            End If
                            ID = cmd.ExecuteScalar()
                            conn.Close()
                        End Using
                    End Using
                End If
                If (ddlServType.SelectedItem.Text.ToLower() = "offer") Then
                    Using connect As New SqlConnection(constring)
                        Dim multiQuery As String = String.Empty
                        If (chkSingleV2.Checked AndAlso txtID.Text <> Nothing) Then
                            multiQuery = "UPDATE [PromoDetails] SET [Quantity] = 1, [DisplayAmount] = @DisplayAmnt,[OfferValue] = '" & txtOfferVal.Text.Trim() & "',[OriginalValue]='" & originalValue1 & "',[Status]=@Status WHERE [PromoID] = @promoID and [Quantity] = 1"
                        ElseIf chkSingleV2.Checked Then
                            multiQuery = "INSERT INTO [PromoDetails] ([PromoID],[Quantity],[DisplayAmount],[OfferValue],[OriginalValue],[Status],[CreatedDate])" & _
                                         " VALUES (@promoID,'1',@DisplayAmnt, '" & txtOfferVal.Text.Trim() & "', '" & originalValue1 & "',@Status,getdate())"
                        End If
                        If (chkMultiple.Checked = True) Then
                            If (chkQ2.Checked AndAlso txtID.Text <> Nothing AndAlso TxtUpdate2.Text = "2") Then
                                multiQuery = multiQuery & ";UPDATE [PromoDetails] SET [Quantity] = 2, [DisplayAmount] = @DisplayAmnt,[OfferValue] = '" & txtMOfferValQ2.Text.Trim() & "',[OriginalValue]='" & originalValue2 & "',[Status]=@Status WHERE [PromoID] = @promoID and [Quantity] = 2"
                            ElseIf chkQ2.Checked AndAlso TxtUpdate2.Text <> "2" Then
                                multiQuery = multiQuery & ";INSERT INTO [PromoDetails] ([PromoID],[Quantity],[DisplayAmount],[OfferValue],[OriginalValue],[Status],[CreatedDate])" & _
                                             " VALUES (@promoID,'2',@DisplayAmnt, '" & txtMOfferValQ2.Text.Trim() & "', '" & originalValue2 & "',@Status,getdate())"
                            End If
                            If (chkQ3.Checked AndAlso txtID.Text <> Nothing AndAlso TxtUpdate3.Text = "3") Then
                                multiQuery = multiQuery & ";UPDATE [PromoDetails] SET [Quantity] = 3, [DisplayAmount] = @DisplayAmnt,[OfferValue] = '" & txtMOfferValQ3.Text.Trim() & "',[OriginalValue]='" & originalValue3 & "',[Status]=@Status WHERE [PromoID] = @promoID and [Quantity] = 3"
                            ElseIf chkQ3.Checked AndAlso TxtUpdate3.Text <> "3" Then
                                multiQuery = multiQuery & ";INSERT INTO [PromoDetails] ([PromoID],[Quantity],[DisplayAmount],[OfferValue],[OriginalValue],[Status],[CreatedDate])" & _
                                             " VALUES (@promoID,'3',@DisplayAmnt, '" & txtMOfferValQ3.Text.Trim() & "', '" & originalValue3 & "',@Status,getdate())"
                            End If
                        End If
                        If (Not (multiQuery = "")) Then
                            Using commandOf As New SqlCommand(multiQuery, connect)
                                If (txtID.Text <> Nothing) Then
                                    commandOf.Parameters.AddWithValue("@promoID", txtID.Text.Trim())
                                Else
                                    commandOf.Parameters.AddWithValue("@promoID", ID)
                                End If

                                commandOf.Parameters.AddWithValue("@DisplayAmnt", dispAmnt)
                                commandOf.Parameters.AddWithValue("@Status", ddlStatus.SelectedItem.Value)
                                commandOf.CommandType = CommandType.Text
                                commandOf.CommandText = multiQuery

                                If (connect.State = ConnectionState.Closed) Then
                                    connect.Open()
                                End If
                                commandOf.ExecuteNonQuery()

                                If (connect.State = ConnectionState.Open) Then
                                    connect.Close()
                                End If
                            End Using
                        End If
                    End Using
            End If
            Me.BindData()
            End If
            'Response.Redirect(Request.RawUrl)
            Clear()
            alert("Offer Added Successfully")
        End If
    End Sub
    Protected Sub Edit(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        Dim dAmn As String
        txtID.Text = row.Cells(12).Text
        cn = New SqlConnection(strConnString)
        Dim que As String = "SELECT * FROM [payit].[dbo].[PromoDetails] where Quantity  not in (1) and PromoID = " & txtID.Text
        da = New SqlDataAdapter(que, cn)
        ds = New DataSet()
        da.Fill(ds, "deta")
        If ds.Tables.Count > 0 Then
            'Dim s As String = ds.Rows(1)("mycolumn1").ToString()
            For Each myRow As DataRow In ds.Tables(0).Rows
                If (Not IsDBNull(myRow("Quantity")) AndAlso myRow("Quantity") <> Nothing) Then
                    If (myRow("Quantity").ToString.Equals("2")) Then
                        chkMultiple.Checked = True
                        chkQ2.Checked = True
                        txtMOfferValQ2.Text = myRow("OfferValue").ToString()
                        divMultiple.Attributes("style") = "display:block;"
                        divQuantity2.Attributes("style") = "display:block;"
                        TxtUpdate2.Text = 2
                        btnQ2.Visible = True
                    End If
                    If (myRow("Quantity").ToString.Equals("3")) Then
                        chkMultiple.Checked = True
                        chkQ3.Checked = True
                        txtMOfferValQ3.Text = myRow("OfferValue").ToString()
                        divMultiple.Attributes("style") = "display:block;"
                        divQuantity3.Attributes("style") = "display:block;"
                        TxtUpdate3.Text = 3
                        btnQ3.Visible = True
                    End If
                End If
            Next
        End If

        lblHeading.Text = "Edit Offer"
        btnSubmit.Text = "Save Offer"

        txtTitle.Text = row.Cells(0).Text.Replace("&nbsp;", "")
        TextMsg.Text = row.Cells(1).Text.Replace("&nbsp;", "")
        FromDateTextBox.Text = Format("yyyy-MM-dd")
        ToDateTextBox.Text = Format("yyyy-MM-dd")
        FromDateTextBox.Text = row.Cells(2).Text
        ToDateTextBox.Text = row.Cells(3).Text
        'txtPushMsg.Text = row.Cells(0).Text.Trim() + ": " + row.Cells(1).Text.Trim()
        txtOfferVal.Text = row.Cells(18).Text.Trim()

        If row.Cells(7).Text = "None" Or row.Cells(7).Text = "&nbsp;" Then
            ddlServType.SelectedValue = "None"
        Else
            ddlServType.SelectedItem.Text = row.Cells(7).Text
        End If

        If row.Cells(4).Text = "&nbsp;" Or row.Cells(4).Text = "" Then
            ddlServiceName.SelectedIndex = 0
        ElseIf (row.Cells(4).Text.ToString().ToUpper().Trim() <> Nothing AndAlso row.Cells(4).Text.ToString().ToUpper().Trim() <> "" AndAlso row.Cells(4).Text.ToString().ToUpper().Trim() <> "NULL") Then
            ddlServiceName.SelectedItem.Text = row.Cells(4).Text
            'ddlServiceName_SelectedIndexChanged(sender, e)
        ElseIf (row.Cells(4).Text Is DBNull.Value) Then
            ddlServiceName.SelectedItem.Text = "None"
        End If

        If ddlServType.SelectedItem.Text = "Offer" Then
            Label5.Text = "Offer Title"
            LabelType.Visible = "false"
            ddlType.Visible = "false"
            ddlType.SelectedIndex = 0
            dvDesc.Visible = "false"
            TextMsg.Text = ""
            fileUp.Visible = "false"
            LabelServ.Visible = "true"
            ddlServiceName.Visible = "true"
            LabelDispAmt.Visible = "true"
            ddlDispAmt.Visible = "true"
            LabelOfferVal.Visible = "true"
            txtOfferVal.Visible = "true"
            dvWebLink.Visible = False
            txtWebLink.Text = ""
            txtPushMsg.Text = row.Cells(0).Text.Trim()
            TextMsg.Text = ""
            dvDesc.Visible = False
            lblMultiple.Visible = True
            lblSingle.Visible = True
        ElseIf ddlServType.SelectedItem.Text = "Greeting" Then
            Label5.Text = "Greeting Title"
            ddlType.SelectedIndex = 1
            LabelServ.Visible = "false"
            ddlServiceName.Visible = "false"
            ddlServiceName.SelectedIndex = 0
            LabelDispAmt.Visible = "False"
            ddlDispAmt.Visible = "False"
            ddlDispAmt.SelectedValue = "0"
            dvDesc.Visible = "true"
            LabelOfferVal.Visible = "false"
            txtOfferVal.Visible = "false"
            txtOfferVal.Text = ""
            fileUp.Visible = "True"
            LabelType.Visible = "true"
            ddlType.Visible = "true"
            dvWebLink.Visible = False
            txtPushMsg.Text = row.Cells(0).Text.Trim() + ": " + row.Cells(1).Text.Trim()
            ddlDispAmt.SelectedValue = "0"
            dvAmnt.Visible = False
            lblMultiple.Visible = False
            lblSingle.Visible = False
        End If

        If row.Cells(9).Text = "None" Or row.Cells(9).Text = "&nbsp;" Or row.Cells(9).Text = "" Then
            ddlStatus.SelectedValue = "None"
        ElseIf (row.Cells(9).Text = "True") Then
            ddlStatus.SelectedIndex = 1
        ElseIf (row.Cells(9).Text = "False") Then
            ddlStatus.SelectedIndex = 2
        End If

        If row.Cells(10).Text = "None" Or row.Cells(10).Text = "&nbsp;" Then
            ddlType.SelectedValue = "None"
        Else
            ddlType.SelectedValue = row.Cells(10).Text
        End If
        If (ddlType.SelectedItem.Text = "Web") Then
            dvWebLink.Visible = True
            txtWebLink.Visible = True
        End If
        If (ddlServType.SelectedItem.Text = "Offer") Then
            If (ddlServiceName.SelectedItem.Text <> "None" Or ddlServiceName.SelectedItem.Text <> "&nbsp;") Then
                ddlServiceName_SelectedIndexChanged(sender, e)
            End If
        End If

        dAmn = row.Cells(13).Text.Trim + "-" + row.Cells(14).Text.Trim
        If (ddlServiceName.SelectedItem.Text = "None") Then
            ddlDispAmt.Enabled = False
            ddlDispAmt.Items.Clear()
        Else
            If row.Cells(13).Text = "None" Or row.Cells(13).Text = "&nbsp;" Then
                ddlDispAmt.Enabled = False
                ddlDispAmt.Items.Clear()
            Else
                ddlDispAmt.SelectedValue = dAmn
            End If
        End If
        If row.Cells(11).Text = "None" Or row.Cells(11).Text = "&nbsp;" Then
            urlHidden.Text = ""
        Else
            urlHidden.Text = row.Cells(11).Text
        End If

        If row.Cells(15).Text = "None" Or row.Cells(15).Text = "&nbsp;" Then
            chkHome.Checked = False
        Else
            chkHome.Checked = row.Cells(15).Text.TrimEnd()
        End If

        If row.Cells(16).Text = "None" Or row.Cells(16).Text = "&nbsp;" Then
            txtWebLink.Text = ""
        Else
            txtWebLink.Text = row.Cells(16).Text
        End If

        If row.Cells(17).Text = "" Or row.Cells(17).Text = "&nbsp;" Then
            chkTest.Checked = False
        Else
            chkTest.Checked = row.Cells(17).Text.TrimEnd()
        End If

        If row.Cells(19).Text = "" Or row.Cells(19).Text = "&nbsp;" Then
            chkSingleV1.Checked = False
        Else
            chkSingleV1.Checked = row.Cells(19).Text.TrimEnd()
        End If

        If row.Cells(20).Text = "" Or row.Cells(20).Text = "&nbsp;" Then
            chkSingleV2.Checked = False
        Else
            chkSingleV2.Checked = row.Cells(20).Text.TrimEnd()
        End If

        If row.Cells(21).Text = "" Or row.Cells(21).Text = "&nbsp;" Then
            chkIntOffer.Checked = False
        Else
            chkIntOffer.Checked = row.Cells(21).Text.TrimEnd()
        End If

    End Sub
    Protected Sub ddlServiceNameEdit_SelectedIndexChanged(sender As Object, e As EventArgs)
    End Sub
    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ExportToExcel.Export("offers.xls", Me.GridView1)
    End Sub
    Protected Sub txtOfferVal_TextChanged(sender As Object, e As EventArgs)
        If (ddlDispAmt.SelectedValue = "0" And ddlServiceName.SelectedIndex = 0) AndAlso (txtOfferVal.Text <> "") Then
            dberrorlabel.Text = "Please Select Service, DisplayAmount"
            Exit Sub
        End If
    End Sub
    Protected Sub Clear()
        txtOfferVal.Text = String.Empty
        ddlServType.SelectedIndex = 0
        ddlServiceName.SelectedIndex = 0
        ddlType.SelectedIndex = 0
        txtTitle.Text = String.Empty
        TextMsg.Text = String.Empty
        txtOfferVal.Text = String.Empty
        txtWebLink.Text = String.Empty
        ddlStatus.SelectedIndex = 0
        chkHome.Checked = False
        chkPush.Checked = False
        chkMultiple.Checked = False
        chkQ2.Checked = False
        chkQ3.Checked = False
        chkSingleV1.Checked = False
        chkSingleV2.Checked = False
        chkTest.Checked = False
        chkIntOffer.Checked = False
        ddlDispAmt.Enabled = False
        ddlDispAmt.Items.Clear()
    End Sub
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
                                              "alertify.alert('" & s & "')" & _
                                              "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
    Public Sub Disable(ByVal sender As Object, ByVal e As EventArgs)
        Dim quantity As Integer
        If (sender.Text <> Nothing And sender.Text = "DisableQ2") Then
            quantity = 2
        ElseIf (sender.Text <> Nothing And sender.Text = "DisableQ3") Then
            quantity = 3
        End If
        cn = New SqlConnection(strConnString)
        Dim que As String = "UPDATE [payit].[dbo].[PromoDetails] SET Status = @Status, Info1=@info1, Info2=@info2 where Quantity=@quantity and PromoID =@promoID"
        Using commandOf As New SqlCommand(que, cn)
            commandOf.Parameters.AddWithValue("@promoID", txtID.Text.Trim())
            commandOf.Parameters.AddWithValue("@quantity", quantity)
            commandOf.Parameters.AddWithValue("@Status", False)
            commandOf.Parameters.AddWithValue("@info1", DateTime.Now)
            commandOf.Parameters.AddWithValue("@info2", Session("user"))
            commandOf.CommandType = CommandType.Text
            commandOf.CommandText = que

            If (cn.State = ConnectionState.Closed) Then
                cn.Open()
            End If
            Dim result As Integer = commandOf.ExecuteNonQuery()

            If (cn.State = ConnectionState.Open) Then
                cn.Close()
            End If
            If (result = 1) Then
                Clear()
                alert("Offer Disabled Successfully")
            Else
                alert("Error disabling offer. Try again")
            End If
        End Using
    End Sub
    Protected Sub btnSubmit_Load(sender As Object, e As EventArgs) Handles btnSubmit.Load
    End Sub
End Class