<%@ Page Title="Payit: AppCategorySettings" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="AppCategoriesSettings.aspx.vb" Inherits="AppCategoriesSettings" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <link rel="stylesheet" href="Stylesheet/pure.css">
    <script src="Scripts/alertify.js"></script>
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
    <link href="Stylesheet/forms.css" rel="stylesheet" />
    <link href="fonts/css/font-awesome.css" rel="stylesheet" />
    <style scoped>
        .button-success,
        .button-error,
        .button-warning,
        .button-secondary {
            color: white;
            border-radius: 4px;
            text-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
        }

        .button-success {
            background: rgb(28, 184, 65); /* this is a green */
        }

        .button-error {
            background: rgb(202, 60, 60); /* this is a maroon */
        }

        .button-warning {
            background: rgb(223, 117, 20); /* this is an orange */
        }

        .button-secondary {
            background: #B97F7F; 
        }
        .button-secondary:hover,
        .button-secondary:active,
        .button-secondary:visited,
        .button-secondary:after {
            text-decoration: none;
        }
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .modalPopup
        {
            background-color:  #272b30;
            color: #f5f5f5;
            position: relative;
            overflow-y: auto;
            padding: 15px;
            border-bottom: 1px solid #1c1e22;
            max-width: 600px;
        }
        .hiddencol
        {
            display: none;
        }
        input[type="text"]
        {
            color: darkslategray;
            font-family: sans-serif;
            box-shadow: none;
        }
        input[type="text"]:focus
        {
            outline: none;
        }
        .spinner {
          width: 40px;
          height: 40px;

          position: relative;
          margin: 30px auto;
        }

        .double-bounce1, .double-bounce2 {
          width: 100%;
          height: 100%;
          border-radius: 50%;
          background-color: #fff;
          opacity: 0.6;
          position: absolute;
          top: 0;
          left: 0;
  
          -webkit-animation: sk-bounce 2.0s infinite ease-in-out;
          animation: sk-bounce 2.0s infinite ease-in-out;
        }

        .double-bounce2 {
          -webkit-animation-delay: -1.0s;
          animation-delay: -1.0s;
        }

        @-webkit-keyframes sk-bounce {
          0%, 100% { -webkit-transform: scale(0.0) }
          50% { -webkit-transform: scale(1.0) }
        }

        @keyframes sk-bounce {
          0%, 100% { 
            transform: scale(0.0);
            -webkit-transform: scale(0.0);
          } 50% { 
            transform: scale(1.0);
            -webkit-transform: scale(1.0);
          }
        }
    </style>
   
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="wrapper">
        <!-- Content Wrapper. Contains page content -->
        <div class="content-wrapper">
             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
            <!-- Content Header (Page header) -->
            <div class="content-header">
                <ol class="pull-right">
                    <asp:LinkButton ID="LinkButton1" runat="server">Export Report To Excel</asp:LinkButton>
                </ol>
            </div>
            <div class="alert-danger">
                <asp:Label ID="dberrorlabel" Font-Size="Larger" Font-Bold="true" runat="server" Text=""></asp:Label>
            </div>
            <div class="row" id="contact">
                <div class="col-md-3"></div>
                <!-- ./col -->
                <div class="col-md-6">
                    <div class="">
                        <!-- /.box-header -->
                        <div class="form-horizontal">
                            <fieldset>
                                <p>Add AppUserCategory Configuration</p>
                                <label>
                                    <div>
                                        <a class="button-secondary pure-button button-small" role="button"> Recent Config </a>
                                    </div>
                                     <asp:DropDownList ID="ddlConfig" runat="server" OnSelectedIndexChanged="ddlConfig_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </label>

                                
                                <label>
                                    <div>
                                        <asp:Label ID="lblUser" runat="server" Text="AppUser"></asp:Label>
                                    </div>
                                    <asp:DropDownList ID="ddlUser" runat="server" AutoPostBack="false">
                                    </asp:DropDownList>
                                </label>
                                    <div id="hideDiv" runat ="server">
                                <label>
                                    <div>
                                        <asp:Label ID="Label1" runat="server" Text="Category"></asp:Label>
                                    </div>
                                    <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="false">
                                    </asp:DropDownList>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="lblPriority" runat="server" Text="Priority"></asp:Label>
                                    </div>
                                    <asp:TextBox runat="server" ID="txtPriority" CssClass="form-control" />
                                </label>
                             
                                <div class="label-group input-below">
                                    <label style="visibility: hidden">Push Notificationss</label>
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkStatus" AutoPostBack="false" />
                                        <span class="text-default" style="color: #fff;">Status</span>
                                    </label>
                                </div>

                                
                                <div class="submit-wrapper pull-right">
                                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-default btn-sm" Text="Add Configuration" />
                                </div>
                                    </div>
                                <div class="submit-wrapper pull-right">
                                    <asp:Button ID="btnConfig" runat="server" CssClass="btn btn-default btn-sm" Text="Add Recent Configuration" />
                                </div>
                                
                            </fieldset>
                        </div>
                    </div>
                    <div class="col-md-3"></div>
                </div>
            </div>
           
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="box box-success">
                                <h4 class="box-title">AppUserCategoryConfiguration Details</h4>
                                <div class="box-body">
                                    <asp:GridView ID="GridView1" CssClass="table table-bordered table-responsive" runat="server" 
                                        BackColor="#2b2028" HeaderStyle-ForeColor="White" ForeColor="White" HorizontalAlign="Center" 
                                        AllowPaging="true" OnPageIndexChanging="OnPaging" PageSize="25" AutoGenerateColumns="false">
                                        <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                        <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                        <HeaderStyle BackColor="Gray" Font-Bold="True" HorizontalAlign="Left" />
                                         <emptydatarowstyle/>
                                            <emptydatatemplate>
                                                <div class="spinner">
                                                    <div class="double-bounce1"></div>
                                                    <div class="double-bounce2"></div>
                                                </div>
                                                <div style="text-align:center"> No Data Found.<asp:LinkButton ID="lnkTry" ForeColor="SteelBlue" runat="server" Text="Try Again" OnClick="TryAgain"></asp:LinkButton> </div>
                                        </emptydatatemplate> 
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    User:
                                                    <asp:DropDownList ID="ddlCountry" runat="server"
                                                    OnSelectedIndexChanged = "CountryChanged" AutoPostBack = "true"
                                                    AppendDataBoundItems = "true">
                                                    <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("AppUser") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="ID" SortExpression="ID" />
                                            <asp:BoundField DataField="UserID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="ID" SortExpression="ID" />
                                            <asp:BoundField DataField="PayitCategoryID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="ID" SortExpression="ID" />
                                             <asp:BoundField DataField="Status" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="Status" SortExpression="Status" />
                                            <asp:BoundField DataField="AppUser" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="AppUser" SortExpression="AppUser" />
                                            <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />
                                            <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority" />
                                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                <ItemTemplate><%#IIf(Boolean.Parse(Eval("Status").ToString()), "Active", "Inactive")%></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CreatedDate" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="Date" SortExpression="Date" />
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" ForeColor="SteelBlue" runat="server" Text="Edit" OnClick="Edit"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>

                       <asp:Panel ID="pnlAddEdit" runat="server" CssClass="panel panel-danger modalPopup">
                            <div class="form-horizontal">
                                <p>Edit details</p>
                                <fieldset>
                                    <label class="hiddencol">
                                        <div>
                                            <asp:Label ID="Label7" runat="server" Text="ID"></asp:Label>
                                        </div>
                                            <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
                                    </label>
                                     <label>
                                        <div>
                                            <asp:Label ID="Label3" runat="server" Text="User"></asp:Label>
                                        </div>
                                        <asp:DropDownList runat="server" ID="ddlUserEdit" disabled="disabled"></asp:DropDownList>
                                    </label>
                                    <label>
                                        <div>
                                            <asp:Label ID="Label17" runat="server" Text="Category"></asp:Label>
                                        </div>
                                        <asp:DropDownList runat="server" ID="ddlCategoryEdit" disabled="disabled"></asp:DropDownList>
                                    </label>
                                    <label>
                                        <div>
                                            <asp:Label ID="lblPriorityEdit" runat="server" Text="Priority"></asp:Label>
                                        </div>
                                        <asp:TextBox runat="server" ID="txtPriorityEdit"></asp:TextBox>
                                    </label>
                                    <div class="label-group input-below">
                                        <label style="visibility: hidden">Push Notificationss</label>
                                        <label>
                                            <asp:CheckBox runat="server" ID="chkStatEdit" AutoPostBack="false" />
                                            <span class="text-default" style="color: #fff;">Status</span>
                                        </label>
                                    </div>
                                   
                                    <div style="border: none;">
                                        <div class="submit-wrapper pull-right">
                                           <%-- <asp:Button ID="btnDelete" CssClass="button-error pure-button pull-left" runat="server" Text="Delete" OnClick="Delete" />--%>
                                            <div class="pull-right">
                                                <asp:Button ID="btnCancel" CssClass="button-warning pure-button" runat="server" Text="Cancel" OnClientClick="return Hidepopup()" />
                                                <asp:Button ID="btnSave" CssClass="button-success pure-button" runat="server" Text="Save" OnClick="Save" />
                                            </div>
                                        </div>
                                    </div>
                                     <div style="border: none;">
                                        <div class="submit-wrapper pull-left">
                                            
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </asp:Panel>
                        <center>
                            <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
                            <cc1:ModalPopupExtender ID="popup" runat="server" DropShadow="false"
                                PopupControlID="pnlAddEdit" TargetControlID="lnkFake"
                                BackgroundCssClass="modalBackground">
                            </cc1:ModalPopupExtender>
                        </center>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GridView1" />
                    <asp:AsyncPostBackTrigger ControlID="ddlCategory" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

