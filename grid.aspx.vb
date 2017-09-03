Imports System.Data
Imports System.Data.SqlClient
Partial Class grid
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim da As SqlDataAdapter
    Dim ds, ds1 As DataSet
    Dim reader As SqlDataReader
    Dim sql As String
    Public Shared service, knet, result, mobile, trackid, refid, amount As String
    Dim s As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            ddlProcess.Items.Insert(0, New ListItem("Select", "Select"))
            ddlProcess.Items.Insert(1, New ListItem("CAPTURED", "CAPTURED"))
            ddlProcess.Items.Insert(2, New ListItem("NOT CAPTURED", "NOT CAPTURED"))
            ddlProcess.Items.Insert(3, New ListItem("APPROVED", "APPROVED"))
            ddlProcess.Items.Insert(4, New ListItem("VOIDED", "VOIDED"))

            ddlResult.Items.Insert(0, New ListItem("Select", "NULL"))
            ddlResult.Items.Insert(1, New ListItem("SUCCESS", "Success"))
            ddlResult.Items.Insert(2, New ListItem("FAIL", "Fail"))
            ddlResult.Items.Insert(3, New ListItem("REFUND", "Successrfnd"))
            ddlResult.Items.Insert(4, New ListItem("VOIDED", "Voided"))
            ddlResult.Items.Insert(5, New ListItem("PAYIT-VPN", "SuccessDealer"))
            ddlResult.Items.Insert(6, New ListItem("REFUND-ONLINE", "RefundOnline"))
            ddlResult.Items.Insert(7, New ListItem("REFUND-CASH", "RefundCash"))
            ddlResult.Items.Insert(8, New ListItem("REFUND-REQ", "RefundRequest"))

            cn = New SqlConnection(strConnString)
            sql = "SELECT ServiceCode from PayitServices"
            da = New SqlDataAdapter(sql, cn)
            ds = New DataSet()
            da.Fill(ds, "deta")
            ddlService.DataSource = ds.Tables("deta")
            ddlService.DataTextField = "ServiceCode"
            ddlService.DataValueField = "ServiceCode"
            ddlService.DataBind()
            ddlService.Items.Insert(0, New ListItem("Select", "Select"))
            ddlService.Items.Insert(1, New ListItem("INTL", "INTL"))

        End If
        cn = New SqlConnection(strConnString)
        If Request.QueryString("qi") = "Intl" Then
            sql = "SELECT myid ID,usern UserName, Company,amt Amount,ptype PayType,TrackID,PayID,RefID,destmob MobileNo,KnetProcess,TopupAmt,TopupResult,topuprawresponse Error,tdatetime TDate from topup_trans where myid like '" & Request.QueryString("id") & "' order by myid desc"

        Else
            sql = "SELECT myid ID, Company,PayID,RefID,Amt,Ptype,TrackId,udf3 MobileNo,KnetProcess,udf1 Result,udf2 ServiceCode,tdatetime TDate from thirdparty_knet_trans where myid like '" & Request.QueryString("id") & "' order by myid desc"
        End If


        da = New SqlDataAdapter(sql, cn)
        ds = New DataSet()
        da.Fill(ds, "deta")
        If Not Me.IsPostBack Then
            If ds.Tables.Count > 0 Then
                'Dim s As String = ds.Rows(1)("mycolumn1").ToString()
                For Each myRow As DataRow In ds.Tables(0).Rows
                    If Not IsDBNull(myRow("refid")) AndAlso myRow("refid") <> Nothing Then
                        TextID.Text = myRow("refid").ToString()
                        refid = myRow("refid").ToString().Trim()
                    End If
                    If Not IsDBNull(myRow("MobileNo")) AndAlso myRow("MobileNo") <> Nothing Then
                        TextMobile.Text = myRow("MobileNo").ToString()
                        mobile = myRow("MobileNo").ToString().Trim()
                    End If

                    If Not IsDBNull(myRow("amt")) AndAlso myRow("amt") <> Nothing Then
                        TextAmnt.Text = myRow("amt").ToString()
                        amount = myRow("amt").ToString()
                    End If

                    If Not IsDBNull(myRow("trackid")) AndAlso myRow("trackid") <> Nothing Then
                        TextHidden.Text = myRow("trackid").ToString()
                        trackid = myRow("trackid").ToString()
                    End If
                    If Not IsDBNull(myRow("KnetProcess")) AndAlso myRow("KnetProcess") <> Nothing Then
                        ddlProcess.SelectedValue = myRow("KnetProcess").ToString()
                        knet = myRow("KnetProcess").ToString()
                    Else
                        ddlProcess.SelectedValue = "Select"
                    End If
                    If Not IsDBNull(myRow("Result")) AndAlso myRow("Result") <> "" Then
                        If myRow("Result").ToString().ToLower().Equals("success") Then
                            ddlResult.SelectedValue = "Success"
                        ElseIf myRow("Result").ToString().ToLower().Contains("fail") Then
                            ddlResult.SelectedValue = "Fail"
                        ElseIf myRow("Result").ToString().ToLower().Contains("rfnd") Then
                            ddlResult.SelectedValue = "Successrfnd"
                        ElseIf myRow("Result").ToString().ToLower().Contains("void") Then
                            ddlResult.SelectedValue = "Voided"
                        ElseIf myRow("Result").ToString().ToLower().Contains("vpn") Then
                            ddlResult.SelectedValue = "SuccessDealer"
                        ElseIf myRow("Result").ToString().ToLower().Contains("online") Then
                            ddlResult.SelectedValue = "RefundOnline"
                        ElseIf myRow("Result").ToString().ToLower().Contains("cash") Then
                            ddlResult.SelectedValue = "RefundCash"
                        ElseIf myRow("Result").ToString().ToLower().Contains("request") Then
                            ddlResult.SelectedValue = "RefundRequest"
                        End If
                        result = myRow("Result").ToString()
                    End If

                    If Not IsDBNull(myRow("ServiceCode")) AndAlso myRow("ServiceCode") <> Nothing Then
                        ddlService.SelectedValue = myRow("ServiceCode").ToString()
                        service = myRow("ServiceCode").ToString()
                    Else
                        ddlService.SelectedValue = "Select"
                    End If

                Next
            End If
            Try
                Label1.Text = ddlService.SelectedItem.Text + ": " + TextID.Text
            Catch ex As Exception

            End Try
        End If
        'GridView1.DataSource = ds.Tables("deta")
        'GridView1.DataBind()
        DetailsView1.DataSource = ds.Tables("deta")
        DetailsView1.DataBind()
    End Sub
    Protected Sub DetailsView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles DetailsView1.DataBound
        If Request.QueryString("qi") = "Intl" Then
            Dim a As String()

            a = DetailsView1.Rows(12).Cells(1).Text.Split("|")
      
            If a(0) = "&nbsp;" Or a.Length < 3 Then
                DetailsView1.Rows(12).Cells(1).Text = ""
            Else
                DetailsView1.Rows(12).Cells(1).Text = a(3).Replace("error_txt=", "")
            End If
        End If
    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs)
        If Page.IsValid Then

            If TextID.Text = "" Or ddlProcess.SelectedItem.Value = "Select" Or ddlResult.SelectedItem.Value = "Select" Then
                alert("Required Values Cannot be Empty")
            Else
                Dim conn As SqlConnection
                conn = New SqlConnection(strConnString)

                sql = "UPDATE thirdparty_knet_trans SET refid=" & TextID.Text & ", KnetProcess='" & ddlProcess.SelectedItem.Text & "',udf1='" & ddlResult.SelectedValue & "' WHERE myid=" & Request.QueryString("id") & _
                      ";UPDATE [payit].[dbo].[ThirdpartyServiceTransactions] SET [ServiceResult] = '" & ddlResult.SelectedValue & "', [PaymentResult]='" & ddlProcess.SelectedItem.Text & "' WHERE Trackid= " & TextHidden.Text & _
                      ";INSERT INTO LogTrace (Page,Service,PaymentChannel,info2,ChangedBy,CreatedOn,info1) VALUES('Particular-Detail','" & service & " | " & ddlService.SelectedItem.Text & " - KnetProcess: " & knet & " | " & ddlProcess.SelectedItem.Text & "','Trackid: " & trackid & " | " & TextHidden.Text & "','Result: " & result & " | " & ddlResult.SelectedItem.Text & "','" & Session("user") & "',getdate(),'" & Request.UserHostAddress & "')" & _
                      ";SELECT myid ID, Company,PayID,RefID,Amt,Ptype,TrackId,udf3 MobileNo,KnetProcess,udf1 Result,tdatetime TDate from thirdparty_knet_trans where myid like '" & Request.QueryString("id") & "' order by myid desc"
                Try
                    conn.Open()
                    da = New SqlDataAdapter(sql, conn)
                    ds = New DataSet()
                    da.Fill(ds, "deta")
                    DetailsView1.DataSource = ds.Tables("deta")
                    DetailsView1.DataBind()
                    alert("Updated")
                Catch
                    alert("Error Updaing  Record!")
                Finally
                    conn.Close()
                End Try

            End If

        End If
    End Sub
    Protected Sub btnSubmit2_Click(sender As Object, e As EventArgs)
        If Page.IsValid Then
            If TextMobile.Text = "" Or ddlService.SelectedValue = "Select" Or TextAmnt.Text = "" Then
                alert("Required Values Cannot be Empty")
            Else
                Dim conn As SqlConnection
                
                Dim connectionString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString

                conn = New SqlConnection(strConnString)
                sql = "UPDATE thirdparty_knet_trans SET udf1=NULL,udf3='" & TextMobile.Text & "',udf2='" & ddlService.SelectedItem.Text & "',amt='" & TextAmnt.Text & "' WHERE trackid=" & TextHidden.Text & _
                      ";UPDATE payitisys SET ProcessTranDescription='Processedto" & TextMobile.Text & "', [Service] = '" & ddlService.SelectedItem.Text & "', [Amount]= " & TextAmnt.Text.Trim() & " WHERE isysid=" & TextHidden.Text & _
                      ";UPDATE [payit].[dbo].[ThirdpartyServiceTransactions] SET [Service] = '" & ddlService.SelectedItem.Text & "', [Amount]= " & TextAmnt.Text.Trim() & " WHERE trackid=" & TextHidden.Text & _
                      ";INSERT INTO LogTrace (Page,Service,PaymentChannel,info2,ChangedBy,CreatedOn,info1) VALUES('Particular-Detail','" & service & " | " & ddlService.SelectedItem.Text & " - KnetProcess: " & knet & " | " & ddlProcess.SelectedItem.Text & "','Trackid: " & trackid & " | " & TextHidden.Text & "','Mobile: " & mobile & " | " & TextMobile.Text & " - Amnt: " & amount & " | " & TextAmnt.Text & "','" & Session("user") & "',getdate(),'" & Request.UserHostAddress & "')" & _
                      ";SELECT myid ID, Company,PayID,RefID,Amt,Ptype,TrackId,udf3 MobileNo,KnetProcess,udf1 Result,tdatetime TDate from thirdparty_knet_trans where myid like '" & Request.QueryString("id") & "' order by myid desc"
                Try
                    conn.Open()
                    da = New SqlDataAdapter(sql, conn)
                    ds = New DataSet()
                    da.Fill(ds, "deta")
                    DetailsView1.DataSource = ds.Tables("deta")
                    DetailsView1.DataBind()
                    alert("Updated")
                Catch
                    alert("Error Updaing  Record!")
                Finally
                    conn.Close()
                End Try
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
End Class
