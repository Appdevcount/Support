﻿<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="commission.aspx.vb" Inherits="commission" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <link rel="stylesheet" href="Stylesheet/pure.css">
    <script src="Scripts/alertify.js"></script>
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
    <link href="Stylesheet/forms.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ctl00_ContentPlaceHolder1_lblCommMsg').hide();
            $('#ctl00_ContentPlaceHolder1_lblComEdit').hide();
        }); 
        function valueChanged() {
            if ($('#ctl00_ContentPlaceHolder1_chkCommission').is(":checked"))
                $("#ctl00_ContentPlaceHolder1_lblCommMsg").hide();
            else
                $("#ctl00_ContentPlaceHolder1_lblCommMsg").show();
        }
        function valueChangedEdit() {
            if ($('#ctl00_ContentPlaceHolder1_chkComMan').is(":checked"))
                $("#ctl00_ContentPlaceHolder1_lblComEdit").hide();
            else
                $("#ctl00_ContentPlaceHolder1_lblComEdit").show();
        }
    </script>
    <style>
       .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .modalPopup
        {
            background-color:  #1c1e22;
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
            background: rgb(66, 184, 221); /* this is a light blue */
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="wrapper">
        <!-- Content Wrapper. Contains page content -->
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <div class="content-header">
                <h4>Commission and Controls
                </h4>
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
                                <p>Add Commission Details</p>
                                <label>
                                    <div>
                                        <asp:Label ID="Label1" runat="server" Text="Service Name"></asp:Label>
                                    </div>
                                    <asp:DropDownList ID="ddlServType" runat="server" AutoPostBack="false">
                                    </asp:DropDownList>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="LabelServ" runat="server" Text="Commission"></asp:Label>
                                    </div>
                                    <asp:TextBox runat="server" ID="txtCommission"></asp:TextBox>
                                </label>
                                <div class="label-group input-below">
                                    <label style="visibility: hidden">Push Notificationss</label>
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkCommission" Checked="true" AutoPostBack="false" onchange="valueChanged()" />
                                        <span class="text-default" style="color: #fff;">isCommissionMandatory</span>
                                    </label>
                                </div>
                                <label>
                                    <div>
                                        <asp:Label ID="LabelDispAmt" runat="server" Text="Commission Msg"></asp:Label>
                                    </div>
                                    <asp:TextBox ID="txtCommMsg" runat="server"></asp:TextBox>
                                </label>
                                    <span runat="server" ID="lblCommMsg" class="badge text-danger" style="font-size:small;background-color:#fff;border-radius:2px;margin-right:-18px;"> Add $$$ (To show Amount) in Commission Msg</span>
                                
                                <div class="label-group input-below" style="margin-top:5px;">
                                    <label style="visibility: hidden">Push Notificationss</label>
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkAmountEdit" AutoPostBack="false" />
                                        <span class="text-default" style="color: #fff;">isAmountEditable</span>
                                    </label>
                                </div>
                                <label>
                                    <div>
                                        <asp:Label ID="LabelType" runat="server" Text="Keypad"></asp:Label>
                                    </div>
                                    <asp:DropDownList ID="ddlKeypad" runat="server" AutoPostBack="true">
                                    </asp:DropDownList>
                                </label>
                                <div class="label-group input-below">
                                    <label style="visibility: hidden">Push Notificationss</label>
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkStatus" AutoPostBack="false" />
                                        <span class="text-default" style="color: #fff;">Status</span>
                                    </label>
                                </div>
                                <div class="submit-wrapper pull-right">
                                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-default btn-sm" Text="Add Commission" />
                                </div>
                            </fieldset>
                        </div>
                    </div>
                    <div class="col-md-3"></div>
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <div class="row">
                        <div class="col-lg-12">
                            <div class="box box-success">
                                <h4 class="box-title">Commission Details</h4>
                                <div class="box-body">
                                    <asp:GridView ID="GridView1" CssClass="table" runat="server" BorderColor="DimGray" BorderStyle="Outset"
                                        BorderWidth="2px" CellPadding="0" Font-Bold="True" Font-Size="Smaller" ForeColor="White"
                                        HorizontalAlign="Center" AllowPaging="true" OnPageIndexChanging="OnPaging" PageSize="25" AutoGenerateColumns="false">
                                        <RowStyle HorizontalAlign="Center" Font-Names="Times New Roman" />
                                        <FooterStyle BorderWidth="5px" />
                                        <PagerStyle Font-Size="Larger" HorizontalAlign="Center" />
                                        <HeaderStyle BorderStyle="Dotted" ForeColor="#00C0C0" />
                                        <AlternatingRowStyle BackColor="#666666" BorderColor="#666666" Font-Bold="False"
                                            ForeColor="White" BorderWidth="1px" />
                                        <Columns>
                                            <asp:BoundField DataField="ID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="ID" SortExpression="ID" />
                                            <asp:BoundField DataField="ServiceName" HeaderText="ServiceName" SortExpression="ServiceName" />
                                            <asp:BoundField DataField="Commission" HeaderText="Commission" SortExpression="Commission" />
                                            <%--<asp:TemplateField HeaderText="Comm Mandatory" SortExpression="isCommissionMandatory">
                                                <ItemTemplate><%#IIf(Boolean.Parse(Eval("isCommissionMandatory").ToString()), "Yes", "No")%></ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:BoundField DataField="isCommissionMandatory" HeaderText="isCommissionMandatory" SortExpression="isCommissionMandatory" />
                                            <asp:BoundField DataField="CommissionInfo" HeaderText="CommissionInfo" SortExpression="CommissionInfo" />
                                            <asp:TemplateField HeaderText="Amnt Editable" SortExpression="isAmountEditable">
                                                <ItemTemplate><%#IIf(Boolean.Parse(Eval("Status").ToString()), "Active", "Inactive")%></ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="isAmountEditable" HeaderText="isAmountEditable" SortExpression="isAmountEditable" />--%>
                                           <%-- <asp:TemplateField HeaderText="Keypad" SortExpression="KeypadType">
                                                <ItemTemplate><%#IIf(Eval("KeypadType").ToString() = 1 , "Numeric", "AlphaNumeric")%></ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:BoundField DataField="KeypadType" HeaderText="KeypadType" SortExpression="KeypadType" />
                                            <%--<asp:BoundField DataField="CreatedDate" HeaderText="Date" SortExpression="CreatedDate" />--%>
                                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                <ItemTemplate><%#IIf(Boolean.Parse(Eval("Status").ToString()), "Active", "Inactive")%></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="isCommissionMandatory" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="isCommissionMandatory" SortExpression="isCommissionMandatory" />
                                            <asp:BoundField DataField="isAmountEditable" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="isAmountEditable" SortExpression="isAmountEditable" />
                                            <asp:BoundField DataField="KeypadType" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="KeypadType" SortExpression="KeypadType" />
                                            <asp:BoundField DataField="Status" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="Status" SortExpression="Status" />
                                            <asp:BoundField DataField="PayitServiceID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="PayitServiceID" SortExpression="PayitServiceID" />
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" ForeColor="#0000EE" runat="server" Text="Edit" OnClick="Edit"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>

                       <asp:Panel ID="pnlAddEdit" runat="server" CssClass="modalPopup">
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
                                            <asp:Label ID="Label18" runat="server" Text="Commission"></asp:Label>
                                        </div>
                                        <asp:Textbox runat="server" ID="txtCommissionEdit"></asp:Textbox>
                                    </label>
                                    <div class="label-group input-below">
                                    <label style="visibility: hidden">Push Notificationss</label>
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkComMan" AutoPostBack="false" onchange="valueChangedEdit()"/>
                                        <span class="text-default" style="color: #fff;">isCommissionMandatory</span>
                                    </label>
                                </div>
                                <label>
                                    <div>
                                        <asp:Label ID="Label2" runat="server" Text="Commission Msg"></asp:Label>
                                    </div>
                                    <asp:TextBox ID="txtComMsgEdit" runat="server"></asp:TextBox>
                                    <%--<span runat="server" ID="lblComEdit" class="label label-danger" style="font-size:small">Add $$$ (To show Amount) in Commission Msg</span>--%>
                                </label>
                                    <span runat="server" ID="lblComEdit" class="badge text-danger" style="font-size:small;background-color:#fff;border-radius:2px;margin-right:-18px;"> Add $$$ (To show Amount) in Commission Msg</span>
                                <div class="label-group input-below">
                                    <label style="visibility: hidden">Push Notificationss</label>
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkAmtEdit2" AutoPostBack="false" />
                                        <span class="text-default" style="color: #fff;">isAmountEditable</span>
                                    </label>
                                </div>
                                <label>
                                    <div>
                                        <asp:Label ID="Label3" runat="server" Text="Keypad"></asp:Label>
                                    </div>
                                    <asp:DropDownList ID="ddlKeypadEdit" runat="server" AutoPostBack="false">
                                    </asp:DropDownList>
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
                                        <asp:Button ID="btnDelete" CssClass="button-error pure-button pull-left" runat="server" Text="Delete" OnClick="Delete" />
                                        <div class="pull-right">
                                            <asp:Button ID="btnCancel" CssClass="button-warning pure-button" runat="server" Text="Cancel" OnClientClick="return Hidepopup()" />
                                            <asp:Button ID="btnSave" CssClass="button-success pure-button" runat="server" Text="Save" OnClick="Save" />
                                        </div>
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
