Imports System.Data.SqlClient
Imports System.Data

Partial Class ZakatSubProjects
    Inherits System.Web.UI.Page
    Dim cn As SqlConnection
    Dim da As SqlDataAdapter
    Dim ds, ds1 As DataSet

    Dim sql As String
    Dim s As String
    Dim strConnString As String = ConfigurationManager.ConnectionStrings("payitconnectionActive").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("role") = "superadmin" Or Session("role") = "accounts" Or Session("role") = "operations" Or Session("role") = "thirdparty") Then
            alert("Unauthorized Access")
            Response.Redirect("~/login.aspx?ReturnURL=ZakatSubProjects.aspx")
        End If

        If Not Page.IsPostBack Then
            Try
                cn = New SqlConnection(strConnString)
                Dim username As String = Session("user").ToString()
                sql = "select ID,ZakatProjectName from ZakatProjects "
                da = New SqlDataAdapter(sql, cn)
                ds = New DataSet()
                da.Fill(ds, "deta")
                ddlProjectName.DataSource = ds.Tables("deta")
                ddlProjectName.DataTextField = "ZakatProjectName"
                ddlProjectName.DataValueField = "ID"
                ddlProjectName.DataBind()
                ddlProjectName.Items.Insert(0, New ListItem("Select", ""))
            Catch ex As Exception
                dberrorlabel.Text = ex.ToString
            End Try
        End If
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If Page.IsValid Then
            Dim cn As SqlConnection = New SqlConnection(strConnString)

            If (zakSubName.Text = "" Or zakSubPriority.Text = "") Then
                alert("Please fill all fields!")
                'dberrorlabel.Text = "Please Fill all fields!"
            Else
                Dim ProjectType As Integer
                ProjectType = 1

                Dim catStat As String
                If chkStatus.Checked = True Then
                    catStat = 1
                Else
                    catStat = 0
                End If
                Dim sql1 As String = "SELECT * FROM CharitySubProjects WHERE SubprojectName=@SubprojectName"
                Dim cmd1 As SqlCommand = New SqlCommand(sql1, cn)
                cmd1.Connection = cn
                cn.Open()
                cmd1.Parameters.AddWithValue("@SubprojectName", zakSubName.Text)
                Using reader As SqlDataReader = cmd1.ExecuteReader()
                    If reader.HasRows Then
                        ' Record already exists
                        alert("Records Already Exist!")
                    Else
                        ' Record does not exist, add them
                        Dim cmd As SqlCommand = New SqlCommand("INSERT INTO CharitySubProjects ([ProjectType],[ProjectID],[SubprojectName],[Description],[Status],[Priority]) VALUES ('" & ProjectType & "','" & ddlProjectName.SelectedValue & "','" & zakSubName.Text.Trim() & "','" & zakSubDesc.Text.Trim() & "','" & catStat & "','" & zakSubPriority.Text & "')")
                        cmd.Connection = cn
                        Dim insert As Integer = cmd.ExecuteNonQuery()
                        If (insert = 1) Then
                            alert("Records Successfully Added!")
                        Else
                            alert("Error! Try Again")
                        End If
                    End If
                End Using
                cn.Close()
                Clear()
                GridView1.DataSourceID = "SqlDataSource1"
                GridView1.DataBind()
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
    Protected Sub Clear()
        zakSubName.Text = String.Empty
        zakSubDesc.Text = String.Empty
        zakSubPriority.Text = String.Empty
        chkStatus.Checked = False
    End Sub
End Class
