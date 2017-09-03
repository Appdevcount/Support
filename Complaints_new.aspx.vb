Imports System.Data
Imports System.Drawing
Imports Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Text.RegularExpressions

Partial Class Complaints_new
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim ds As New DataSet
    Dim da As New SqlDataAdapter
    Dim sqlLocalTrans As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
    Dim typeS As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "cs" Or Session("role") = "operations" Or Session("role") = "accounts" Or Session("role") = "qa") Then
            alert("Unauthorized Access")
            Dim returnUrl = Server.UrlEncode(Request.Url.PathAndQuery)
            Response.Redirect("~/login.aspx?ReturnURL=Complaints_new.aspx")
        End If
        'alertify("Unauthorized Access")
        If Not Me.IsPostBack Then
            Me.BindData()
            'Me.BindGrid(applist.SelectedItem.Text)
            'Dim query As String = "SELECT top 0 Status FROM payitcomplaints"
            'Using con As New SqlConnection(strConnString)
            '    Using cmd As New SqlCommand(query)
            '        cmd.CommandType = CommandType.Text
            '        cmd.Connection = con
            '        con.Open()
            '        Using sdr As SqlDataReader = cmd.ExecuteReader()
            '            While sdr.Read()
            '                Dim item As New ListItem()
            '                item.Text = sdr("Status").ToString()
            '                ddlStatus.Items.Add(item)
            '            End While
            '        End Using
            '        con.Close()
            '    End Using
            'End Using
            ddlStatus.Items.Insert(0, New ListItem("Select", "Select"))
            ddlStatus.Items.Insert(1, New ListItem("Pending", "Pending"))
            ddlStatus.Items.Insert(2, New ListItem("FollowUp", "FollowUp"))
            ddlStatus.Items.Insert(3, New ListItem("Closed", "Closed"))

            'ddlProcessed.Items.Insert(0, New ListItem("Select", "Select"))
            'ddlProcessed.Items.Insert(1, New ListItem("YNA", "YNA"))
            'ddlProcessed.Items.Insert(2, New ListItem("MARILOU", "MARILOU"))
            'ddlProcessed.Items.Insert(3, New ListItem("SAAD", "SAAD"))
            'ddlProcessed.Items.Insert(4, New ListItem("AYMAN", "AYMAN"))
            'ddlProcessed.Items.Insert(5, New ListItem("HOSSAM", "HOSSAM"))
            'ddlProcessed.Items.Insert(6, New ListItem("MAROOF", "MAROOF"))
            'ddlProcessed.Items.Insert(7, New ListItem("M.NOUR", "M.NOUR"))

        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        If (applist.SelectedValue.Equals("app")) Then
            sqlLocalTrans = "select top 1000 * from payitcomplaints"
            Dim tempSQL As String = ""
            If DropDownStatus.SelectedItem.Text <> "All" And DropDownStatus.SelectedItem.Text <> "NULL" Then
                tempSQL = "Status like '" & Trim(DropDownStatus.SelectedItem.Text) & "' "
            End If

            If Trim(TextMobile.Text) <> "" And Trim(TextMobile.Text) <> "NULL" Then
                If tempSQL = "" Then
                    tempSQL = "MobileNo like '" & Trim(TextMobile.Text) & "' "
                Else
                    tempSQL = tempSQL & " and MobileNo like '" & Trim(TextMobile.Text) & "' "
                End If
            End If
            If Trim(TextID.Text) <> "" And Trim(TextID.Text) <> "NULL" Then
                If tempSQL = "" Then
                    tempSQL = "ID=" & Trim(TextID.Text)
                Else
                    tempSQL = tempSQL & " and ID= " & Trim(TextID.Text)
                End If
            End If
            If tempSQL <> "" Then
                sqlLocalTrans = sqlLocalTrans & " where " & tempSQL & " order by id desc"
            Else
                sqlLocalTrans = sqlLocalTrans & " order by id desc"
            End If
            alert("Searching")
            Dim cmd As SqlCommand = New SqlCommand(sqlLocalTrans)
            GridView1.DataSource = GetData(cmd)
            GridView1.DataBind()

        ElseIf (applist.SelectedValue.Equals("fb")) Then
            Dim fbfeed As New SocialFeed.SocialFeed()
            Dim Tickets = fbfeed.GetTickets("", "")
            If (Tickets.Length > 0) Then
                For Each item As SocialFeed.TicketsFeed In Tickets
                    Dim saveTicket = SaveTickets(item)
                Next
            End If

            sqlLocalTrans = "select top 1000 * from [payit].[dbo].[PayitComplaintsSocial]"
            Dim tempSQL As String = ""
            If DropDownStatus.SelectedItem.Text <> "All" And DropDownStatus.SelectedItem.Text <> "NULL" Then
                tempSQL = "Status like '" & Trim(DropDownStatus.SelectedItem.Text) & "' "
            End If

            If Trim(TextMobile.Text) <> "" And Trim(TextMobile.Text) <> "NULL" Then
                If tempSQL = "" Then
                    tempSQL = "MobileNo like '" & Trim(TextMobile.Text) & "' "
                Else
                    tempSQL = tempSQL & " and MobileNo like '" & Trim(TextMobile.Text) & "' "
                End If
            End If
            If Trim(TextID.Text) <> "" And Trim(TextID.Text) <> "NULL" Then
                If tempSQL = "" Then
                    tempSQL = "ID=" & Trim(TextID.Text)
                Else
                    tempSQL = tempSQL & " and ID= " & Trim(TextID.Text)
                End If
            End If
            If tempSQL <> "" Then
                sqlLocalTrans = sqlLocalTrans & " where " & tempSQL & " order by id desc"
            Else
                sqlLocalTrans = sqlLocalTrans & " order by id desc"
            End If
            Dim cmd As SqlCommand = New SqlCommand(sqlLocalTrans)
            GridView1.DataSource = GetData(cmd)
            GridView1.DataBind()
        End If
    End Sub
   
    Private Sub BindGrid(app As String)
        Dim compid As Integer
        Dim constring As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString
        If (app.Equals("app")) Then
            If Not (Label6.Value = "") Then
                compid = Convert.ToInt32(Label6.Value)
            End If
        Else
            If Not (Label6.Value = "") Then
                compid = Convert.ToInt32(Label6.Value)
            Else
                compid = 1
            End If
            Dim GetReplies As New SocialFeed.SocialFeed()
            Dim saveReplies As String
            Dim _dc As New payitEntities
            Dim rows = (From c In _dc.PayitComplaintsSocials Where c.ID = compid Select c.UDID_ID).SingleOrDefault()
            Dim Replies = GetReplies.ReplyThread(rows, "", "")
            If (Replies.Length > 0) Then
                For Each item As SocialFeed.FBComment In Replies
                    saveReplies = SaveTicketsReply(item, compid)
                Next
            End If
        End If

        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("SELECT * from payitcomplaintstrack WHERE ComplaintID=" & compid)
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GridView2.DataSource = dt
                        GridView2.DataBind()
                    End Using
                End Using
            End Using
        End Using

    End Sub

    Private Sub BindData()
        Dim strQuery As String = ("select top 1000 * from payitcomplaints order by id desc")
        Dim cmd As SqlCommand = New SqlCommand(strQuery)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim sqlLocalTrans As String
        sqlLocalTrans = "select top 1000 * from payitcomplaints order by id desc"
        Session("sqlLocalTrans") = sqlLocalTrans
        Dim cmd As SqlCommand = New SqlCommand(sqlLocalTrans)
        GridView1.DataSource = GetData(cmd)
        GridView1.DataBind()
    End Sub
    Public Sub sqldatabind(ByVal s As String, Optional ByVal types1 As String = "", Optional ByVal type As String = "")
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
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        Me.BindData()
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataBind()
    End Sub
    Public Sub LinkButton1_Click(sender As Object, e As EventArgs)
        Dim Lnk As LinkButton = DirectCast(sender, LinkButton)
        If (applist.SelectedValue.Equals("app")) Then
            Label6.Value = Lnk.Text
            BindGrid("app")
            LinkButton1_ModalPopupExtender.Show()
        Else
            Label6.Value = Lnk.Text
            BindGrid("notapp")
            LinkButton1_ModalPopupExtender.Show()
        End If


    End Sub
    Protected Sub Edit(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        txtID.ReadOnly = True
        txtID.Text = row.Cells(9).Text
        If (applist.SelectedValue.Equals("app")) Then
            If row.Cells(6).Text = "NO" Or row.Cells(6).Text = "&nbsp;" Then
                ddlStatus.SelectedValue = "Select"
            Else
                ddlStatus.SelectedValue = row.Cells(6).Text
            End If
            txtComments.Text = row.Cells(7).Text.Replace("&nbsp;", "")
            'If row.Cells(8).Text = "NO" Or row.Cells(8).Text = "&nbsp;" Then
            '    ddlProcessed.SelectedValue = "Select"
            'Else
            '    ddlProcessed.SelectedValue = row.Cells(8).Text
            'End If
        Else
            Dim _dc As New payitEntities
            Dim rows = (From c In _dc.PayitComplaintsSocials Where c.ID = txtID.Text.Trim() Select c.UDID_ID).SingleOrDefault()
            If (rows IsNot Nothing) Then
                txtUID.Text = rows.Value
            Else
                txtUID.Text = ""
            End If

            If row.Cells(6).Text = "NO" Or row.Cells(6).Text = "&nbsp;" Then
                ddlStatus.SelectedValue = "Select"
            Else
                ddlStatus.SelectedValue = row.Cells(6).Text
            End If

            txtComments.Text = row.Cells(7).Text.Replace("&nbsp;", "")

            'If row.Cells(8).Text = "NO" Or row.Cells(8).Text = "&nbsp;" Then
            '    ddlProcessed.SelectedValue = "Select"
            'Else
            '    ddlProcessed.SelectedValue = row.Cells(8).Text
            'End If
        End If


        popup.Show()
    End Sub
    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        If Not (Session("role") = "superadmin" Or Session("role") = "cs" Or Session("role") = "operations") Then
            alert("Unauthorized Access")
            Exit Sub
        End If

        If (ddlStatus.SelectedIndex = 0) Then
            txtProcessedByError.Visible = True
            txtProcessedByError.Text = "Please select Status"
            popup.Show()
            'alert("Please select Processed By")
            Exit Sub
        End If

        'If (ddlProcessed.SelectedIndex = 0) Then
        '    txtProcessedByError.Visible = True
        '    txtProcessedByError.Text = "Please select Processed By"
        '    popup.Show()
        '    'alert("Please select Processed By")
        '    Exit Sub
        'End If
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "AddUpdateCustomer"
        cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedItem.Text)
        cmd.Parameters.AddWithValue("@Comments", txtComments.Text)
        cmd.Parameters.AddWithValue("@Processed_By", Session("user"))
        cmd.Parameters.AddWithValue("@ID", Integer.Parse(txtID.Text))
        GridView1.DataSource = Me.GetData(cmd)
        GridView1.DataBind()

        If (applist.SelectedItem.Text.Equals("app")) Then
            Dim TicketStatus As New SocialFeed.SocialFeed()
            'Dim _dc As New payitEntities
            'Dim rows = (From c In _dc.PayitComplaintsSocials Where c.ID = txtID.Text.Trim() Select c.UDID_ID).SingleOrDefault()
            Dim status = TicketStatus.SaveReply(txtUID.Text, txtComments.Text.Trim(), "", "")
        End If
    End Sub
    Private Function SaveTickets(ticket As SocialFeed.TicketsFeed) As String
        Dim result As String = ""
        Try
            If (ticket IsNot Nothing) Then
                Dim db As New payitEntities()
                Dim socialcomplaint As New PayitComplaintsSocial
                socialcomplaint.UDID_ID = ticket.ReferenceID
                socialcomplaint.Msg = ticket.ActualContent
                socialcomplaint.Name = ticket.CommentFromName
                socialcomplaint.Status = "NO"
                socialcomplaint.Datetime = ticket.ActualFeedDate
                db.PayitComplaintsSocials.Add(socialcomplaint)
                db.SaveChanges()
            End If
        Catch ex As Exception

        End Try
        Return result
    End Function
    Private Function SaveTicketsReply(ticket As SocialFeed.FBComment, payitsocialID As Int32) As String
        Dim result As String = ""
        Try
            If (ticket IsNot Nothing) Then
                Dim db As New payitEntities()
                Dim socialcomplaint As New payitcomplaintstrack
                socialcomplaint.ComplaintID = payitsocialID
                socialcomplaint.Comments = ticket.Message
                socialcomplaint.Processed_By = ticket.Commentfromname
                socialcomplaint.Status = "NO"
                socialcomplaint.Created = ticket.CommentCreatedDate
                db.payitcomplaintstracks.Add(socialcomplaint)
                db.SaveChanges()
            End If
        Catch ex As Exception

        End Try
        Return result
    End Function
    Public Sub alert(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
            "alertify.set('notifier','position', 'top-right');alertify.notify('" & s & "', 'info', 5, function(){  console.log('dismissed'); });" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub

    Public Sub alertify(ByVal s As String)
        Dim popupscript As String
        popupscript = "<script language='javascript'>" & _
                      "alertify.alert('" & s & "')" & _
                      "</script>"
        ClientScript.RegisterStartupScript(Page.GetType, "popupScript", popupscript)
    End Sub
End Class
