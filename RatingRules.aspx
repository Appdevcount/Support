﻿<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="RatingRules.aspx.vb" Inherits="RatingRules" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <link rel="stylesheet" href="Stylesheet/pure.css">
    <script src="Scripts/alertify.js"></script>
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
    <link href="Stylesheet/forms.css" rel="stylesheet" />
    <style scoped>
        .button-success,
        .button-error,
        .button-warning,
        .button-secondary
        {
            color: white;
            border-radius: 4px;
            text-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
        }

        .button-success
        {
            background: rgb(28, 184, 65); /* this is a green */
        }

        .button-error
        {
            background: rgb(202, 60, 60); /* this is a maroon */
        }

        .button-warning
        {
            background: rgb(223, 117, 20); /* this is an orange */
        }

        .button-secondary
        {
            background: rgb(66, 184, 221); /* this is a light blue */
        }

        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .modalPopup
        {
            background-color: #272b30;
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
    </style>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
            <div class="wrapper">
                <!-- Content Wrapper. Contains page content -->
                <div class="content-wrapper">
                    <!-- Content Header (Page header) -->
                    <div class="content-header">
                        <h4>KYC
                        </h4>
                        <ol class="pull-right">
                            <asp:LinkButton ID="LinkButton1" runat="server">Export Report To Excel</asp:LinkButton>
                        </ol>
                    </div>
                    <div class="alert-danger">
                        <asp:Label ID="dberrorlabel" Font-Size="Larger" Font-Bold="true" runat="server" Text=""></asp:Label>
                    </div>
                     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                    <div class="row" id="contact">
                        <div class="col-md-3"></div>
                        <!-- ./col -->
                        <div class="col-md-6">
                            <div class="">
                                <!-- /.box-header -->
                                <div class="form-horizontal">
                                    <fieldset>
                                        <p>Add Rule</p>
                                        <%-- <label>
                                    <div>
                                        <asp:Label ID="Label14" runat="server" Text="Rule Type"></asp:Label>
                                    </div>
                                    <asp:DropDownList ID="ddlRuleType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRuleType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </label>
                                 <label>
                                    <div>
                                        <asp:Label ID="Label15" runat="server" Text="Country"></asp:Label>
                                    </div>
                                    <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="false">
                                    </asp:DropDownList>
                                </label>--%>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label1" runat="server" Text="Service Name"></asp:Label>
                                            </div>
                                            <asp:DropDownList ID="ddlServiceType" runat="server" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label2" runat="server" Text="Limit Type"></asp:Label>
                                            </div>
                                            <asp:DropDownList ID="ddlLimitType" runat="server" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label4" runat="server" Text="Limit Value"></asp:Label>
                                            </div>
                                            <asp:TextBox runat="server" ID="txtLimitValue"></asp:TextBox>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="LabelDuration" runat="server" Text="Duration"></asp:Label>
                                            </div>
                                            <asp:TextBox runat="server" ID="txtDuration"></asp:TextBox>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label6" runat="server" Text="KYC Rating"></asp:Label>
                                            </div>
                                            <asp:TextBox runat="server" ID="txtRating"></asp:TextBox>
                                        </label>
                                        <%--   <label>
                                    <div>
                                        <asp:Label ID="Label5" runat="server" Text="KYCRatingType"></asp:Label>
                                    </div>
                                    <asp:DropDownList ID="ddlKYCRatingType" runat="server" AutoPostBack="false">
                                    </asp:DropDownList>
                                </label>--%>

                                        <div class="label-group input-below">
                                            <label style="visibility: hidden">Push Notificationss</label>
                                            <label>
                                                <asp:CheckBox runat="server" ID="chkStatus" AutoPostBack="false" />
                                                <span class="text-default" style="color: #fff;">Status</span>
                                            </label>
                                        </div>
                                        <div class="submit-wrapper pull-right">
                                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary btn-sm" Text="Save Rule" />
                                            <asp:Button ID="btnDelete" CssClass="button-error pure-button pull-left" runat="server" Text="Delete" OnClick="Delete" />
                                            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-default btn-sm" Text="Add Rule" />
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
                                <h4 class="box-title">Rule Details</h4>
                                <div class="box-body">
                                    <asp:GridView ID="GridView1" CssClass="table" runat="server" BorderColor="DimGray" BorderStyle="Outset"
                                        BorderWidth="2px" CellPadding="0" Font-Bold="True" Font-Size="Smaller" ForeColor="White"
                                        HorizontalAlign="Center" AllowPaging="true" OnPageIndexChanging="OnPaging" OnRowDataBound="GridView1_RowDataBound" PageSize="25" AutoGenerateColumns="false">
                                        <RowStyle HorizontalAlign="Center" Font-Names="Times New Roman" />
                                        <FooterStyle BorderWidth="5px" />
                                        <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                        <HeaderStyle BorderStyle="Dotted" ForeColor="#00C0C0" />
                                        <AlternatingRowStyle BackColor="#666666" BorderColor="#666666" Font-Bold="False"
                                            ForeColor="White" BorderWidth="1px" />
                                        <Columns>
                                            <asp:BoundField DataField="ID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="ID" SortExpression="ID" />
                                            <asp:BoundField DataField="RuleType" HeaderText="RuleType" SortExpression="RuleType" />
                                            <asp:BoundField DataField="ConfigID" HeaderText="Config" SortExpression="ConfigID" />
                                            <asp:BoundField DataField="LimitType" HeaderText="LimitType" SortExpression="LimitType" />
                                            <asp:BoundField DataField="LimitValue" HeaderText="LimitValue" SortExpression="LimitValue" />
                                            <asp:BoundField DataField="Duration" HeaderText="Duration" SortExpression="Duration" />
                                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                <ItemTemplate><%#IIf(Boolean.Parse(Eval("Status").ToString()), "Active", "Inactive")%></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Status" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="Status" SortExpression="Status" />
                                            <asp:BoundField DataField="KYCRating" HeaderText="KYCRating" SortExpression="KYCRating" />
                                            <asp:BoundField DataField="KYCRatingService" HeaderText="KYCRatingService" SortExpression="KYCRatingService" />
                                            <asp:BoundField DataField="KYCRatingTypes" HeaderText="KYCRatingTypes" SortExpression="KYCRatingTypes" />
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
                                        <asp:Label ID="Label17" runat="server" Text="Service"></asp:Label>
                                    </div>
                                    <asp:DropDownList runat="server" ID="ddlServiceEdit"></asp:DropDownList>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label3" runat="server" Text="Payment"></asp:Label>
                                    </div>
                                    <asp:DropDownList runat="server" ID="ddlPaymentEdit"></asp:DropDownList>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label8" runat="server" Text="Commission Type"></asp:Label>
                                    </div>
                                    <asp:DropDownList ID="ddlCommTypeEdit" runat="server" AutoPostBack="false">
                                    </asp:DropDownList>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label9" runat="server" Text="Commission Value"></asp:Label>
                                    </div>
                                    <asp:TextBox runat="server" ID="txtCommissionEdit"></asp:TextBox>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label10" runat="server" Text="Threshold Type"></asp:Label>
                                    </div>
                                    <asp:DropDownList ID="ddlThresholdEdit" runat="server" AutoPostBack="false">
                                    </asp:DropDownList>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label11" runat="server" Text="Threshold Value"></asp:Label>
                                    </div>
                                    <asp:TextBox runat="server" ID="txtThresholdEdit"></asp:TextBox>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label13" runat="server" Text="Comm. Msg"></asp:Label>
                                    </div>
                                    <asp:TextBox runat="server" ID="txtCommMsgEdit"></asp:TextBox>
                                </label>
                                <div class="label-group input-below">
                                    <label style="visibility: hidden">Push Notificationss</label>
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkStatEdit" AutoPostBack="false" />
                                        <span class="text-default" style="color: #fff;">Status</span>
                                    </label>
                                </div>
                                <div class="label-group input-below">
                                    <label style="visibility: hidden">Push Notificationss</label>
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkManEdit" AutoPostBack="false" />
                                        <span class="text-default" style="color: #fff;">isMandatory</span>
                                    </label>
                                </div>
                                <div style="border: none;">
                                    <div class="submit-wrapper pull-right">
                                       
                                        <div class="pull-right">
                                            <asp:Button ID="btnCancel" CssClass="button-warning pure-button" runat="server" Text="Cancel" OnClientClick="return Hidepopup()" />
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
            </Triggers>
        </asp:UpdatePanel>
    </div>
</div>
</asp:Content>


