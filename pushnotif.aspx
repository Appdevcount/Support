<%@ Page Title="Push Notification" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="pushnotif.aspx.vb" Inherits="pushnotif" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="Stylesheet/pure.css">
    <link href="Stylesheet/mainstyle.css" rel="Stylesheet" />
    <link href="Stylesheet/forms.css" rel="stylesheet" />
    <script src="scripts/jquery-1.3.2.min.js" type="text/javascript"></script>
    <style>
        .gridview 
        {
            width: 100%; 
            word-wrap:break-word;
            grid-columns: fixed;
            grid-column-sizing: fixed;
            table-layout: fixed;
            overflow: hidden;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="wrapper">
        <!-- Content Wrapper. Contains page content -->
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <div class="content-header">
                <h4>Offers & Greetings
                </h4>
                <ol class="pull-right">
                    <asp:LinkButton ID="LinkButton1" runat="server">Export Report To Excel</asp:LinkButton>
                </ol>
            </div>
            <div class="">
                <div class="alert-danger">
                    <asp:Label ID="dberrorlabel" Font-Size="Larger" Font-Bold="true" runat="server" Text=""></asp:Label>
                </div>
                <div class="row" id="contact">
                    <div class="col-md-2"></div>
                    <!-- ./col -->
                    <div class="col-md-6 col-md-offset-3"> 
                        <div>
                            <!-- /.box-header -->
                            <div class="form-horizontal">
                                <fieldset>
                                    <p>Add Push Notification details</p>
                                    <label>
                                        <div>
                                        <asp:Label ID="Label1" runat="server" Text="Platform"></asp:Label></div>
                                        <asp:DropDownList ID="ddlPlatform" runat="server" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </label>

                                    <label>
                                        <div>
                                        <asp:Label ID="Label3" runat="server" Text="Service"></asp:Label></div>
                                        <asp:DropDownList ID="ddlMsgCat"  runat="server" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </label>
                                    <label>
                                        <div>
                                        <asp:Label ID="LabelMsg" runat="server" Text="Message"></asp:Label></div>
                                        <asp:TextBox ID="txtMsg" runat="server"></asp:TextBox>
                                    </label>
                                    <label>
                                        <div>
                                        <asp:Label ID="FromDateLabel" runat="server" Text="Scheduled Date"></asp:Label></div>
                                        <asp:Label ID="Label14" runat="server" CssClass="label label-primary pull-right"></asp:Label>
                                        <asp:TextBox ID="FromDateTextBox" runat="server"></asp:TextBox>

                                        <cc1:CalendarExtender ID="FromDateTextBox_CalendarExtender" runat="server"
                                            Enabled="True" Format="yyyy-MM-dd" Animated="true" TargetControlID="FromDateTextBox"
                                            OnClientShowing="CurrentDateShowing" CssClass="cal_KimTheme">
                                        </cc1:CalendarExtender>
                                    </label>
                                   
                                    <label>
                                        <div>
                                        <asp:Label ID="Label2" runat="server" Text="Status"></asp:Label></div>
                                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </label>
                                    <div style="margin-bottom:10px;"></div>
                                    <div class="submit-wrapper pull-right">
                                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-default btn-sm" Text="Add Push Notification" />
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1"></div>
                </div>
                <div class="row">
                    <div class="col-lg-12">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
             
        <asp:GridView ID="GridView1" runat="server"
        AutoGenerateColumns="false" BorderColor="DimGray" BorderStyle="Outset"
        BorderWidth="2px" CellPadding="0" Font-Bold="True" Font-Size="Smaller" ForeColor="White"
        HorizontalAlign="Center" AllowPaging="True" OnPageIndexChanging = "OnPaging" PageSize = "25" >
            <RowStyle HorizontalAlign="Center" Font-Names="Times New Roman" />
            <FooterStyle BorderWidth="5px" />
            <PagerStyle Font-Size="Larger" HorizontalAlign="Center" />
            <HeaderStyle BorderStyle="Dotted" ForeColor="#00C0C0" />
            <AlternatingRowStyle BackColor="#666666" BorderColor="#666666" Font-Bold="False"
                ForeColor="White" BorderWidth="1px" />
            <Columns>
           
                 <asp:TemplateField Visible="false">
                     <HeaderTemplate>ID</HeaderTemplate>
                    <ItemTemplate> 
                    <asp:LinkButton ID="LinkButton1" Visible="false" ForeColor="#0000EE" runat="server" Text='<%#Eval("ID")%>' value='<%#Eval("ID")%>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>                     

                    <asp:BoundField HeaderText="Message" 
                     DataField="Message" 
                     SortExpression="Message"
                     HtmlEncode = "true"></asp:BoundField>

                    <asp:BoundField HeaderText="Platform"
                     DataField="Platform" 
                     SortExpression="Platform"
                         HtmlEncode = "true"></asp:BoundField>

                    <asp:BoundField HeaderText="Language" 
                     DataField="Language" 
                     SortExpression="Language"
                         HtmlEncode = "true"></asp:BoundField>

                    <asp:BoundField HeaderText="Application" 
                     DataField="Application" 
                     SortExpression="Application"
                         HtmlEncode = "true"></asp:BoundField>

                    <asp:BoundField HeaderText="ScheduledDate" 
                     DataField="ScheduledDate" 
                     DataFormatString = "{0:dd/MM/yyyy}"
                     SortExpression="ScheduledDate"></asp:BoundField>

                    <asp:TemplateField HeaderText="Status" SortExpression="Status">
                        <ItemTemplate><%#IIf(Boolean.Parse(Eval("Status").ToString()), "Active", "Inactive")%></ItemTemplate>
                    </asp:TemplateField>

                    <%-- <asp:BoundField HeaderText="Status" 
                     DataField="Status" 
                     SortExpression="Status"
                         HtmlEncode = "true"></asp:BoundField>--%>

                    <asp:BoundField HeaderText="Reference" 
                     DataField="Reference" 
                     SortExpression="Reference"
                         HtmlEncode = "true"></asp:BoundField>

                    <%-- <asp:BoundField HeaderText="MessageCategory" 
                     DataField="MessageCategory" 
                     SortExpression="MessageCategory"
                         HtmlEncode = "true"></asp:BoundField>--%>

                   <%--<asp:TemplateField ItemStyle-Width = "30px" HeaderText = "Action">
                       <ItemTemplate>
                           <asp:LinkButton ID="lnkEdit" ForeColor="#0000EE" runat="server" Text = "Edit"></asp:LinkButton>
                       </ItemTemplate>
                    </asp:TemplateField>--%>
                    </Columns>
                    </asp:GridView>
                   
            
        
            </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID = "GridView1" />
            <asp:AsyncPostBackTrigger ControlID = "btnSubmit" />
        </Triggers>
    </asp:UpdatePanel>
                        </div>
    </div>
                 </div>
                </div>
            </div>
</asp:Content>


