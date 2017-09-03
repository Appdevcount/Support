<%@ Page Title="Offers" Language="VB" MasterPageFile="~/MasterPage2.master" EnableEventValidation="false" AutoEventWireup="false" CodeFile="offers.aspx.vb" Inherits="offers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <link rel="stylesheet" href="Stylesheet/pure.css">
    <link href="Stylesheet/forms.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ctl00_ContentPlaceHolder1_ddlServType').on('change', function () {
                if (this.value == 'Offer') {
                    $('#fileUp').show();
                }
                else {
                    $('#fileUp').hide();
                }
            })
        })
    </script>
    <script type="text/javascript">
        function ShowHideDiv(chkPush) {
            var dvPush = document.getElementById("dvPush");
            dvPush.style.display = chkPush.checked ? "block" : "none";
        }
        function ShowHideDivMulti(chkMultiple) {
            var divMultiple = document.getElementById("ctl00_ContentPlaceHolder1_divMultiple");
            divMultiple.style.display = chkMultiple.checked ? "block" : "none";
        }

        function ShowHideDivQuantity2(chkQ2) {
            var divQuantity2 = document.getElementById("ctl00_ContentPlaceHolder1_divQuantity2");
            divQuantity2.style.display = chkQ2.checked ? "block" : "none";
        }

        function ShowHideDivQuantity3(chkQ3) {
            var divQuantity3 = document.getElementById("ctl00_ContentPlaceHolder1_divQuantity3");
            divQuantity3.style.display = chkQ3.checked ? "block" : "none";
        }
     
        function ShowHideDivEdit(chkPushEdit) {
            var dvPushEdit = document.getElementById("dvPushEdit");
            dvPushEdit.style.display = chkPushEdit.checked ? "block" : "none";
        }
      

    </script>
    <script type="text/javascript">
        function edValueKeyPress() {
            var edValue, s, TheTextBox;
            if ($("#ctl00_ContentPlaceHolder1_ddlServType").val() === "Offer") {
                edValue = document.getElementById("ctl00_ContentPlaceHolder1_txtTitle").value;
                s = edValue;
                if (s !== null && s !== "") {
                    $("#ctl00_ContentPlaceHolder1_txtPushMsg").val(s)
                }
            }
            if ($("#ctl00_ContentPlaceHolder1_ddlServType").val() === "Greeting") {
                edValue = document.getElementById("ctl00_ContentPlaceHolder1_txtTitle").value + ":" + document.getElementById("ctl00_ContentPlaceHolder1_TextMsg").value;
                s = edValue;
                if (s !== null && s !== "") {
                    $("#ctl00_ContentPlaceHolder1_txtPushMsg").val(s)
                }
            }
        }
        function edValueKeyPressEdit() {
            var edValue = document.getElementById('ctl00_ContentPlaceHolder1_txtTitleEdit').value + ': ' + document.getElementById('ctl00_ContentPlaceHolder1_txtDescEdit').value;
            var s = edValue;
            if (s != null && s != "") {
                var TheTextBox = document.getElementById("ctl00_ContentPlaceHolder1_txtPMEdit");
                TheTextBox.value = s;
            }
        }
    </script>
       <script type="text/javascript">
           $(document).ready(function () {
               setTimeout(function () {
                   $('#<%= dberrorlabel.ClientId%>').hide();
        }, 2000); 
    });
    </script>
    <style>
        .hide
        {
            display: none;
        }

        .show
        {
            display: block;
        }

        .hiddencol
        {
            display: none;
        }

        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .modalPopup
        {
            background-color: #3e444c;
            color: #f5f5f5;
            position: relative;
            overflow-y: auto;
            padding: 15px;
            border-bottom: 1px solid #1c1e22;
            max-width: 600px;
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
                <h4>Offers & Greetings
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
                        <div class="col-md-6">
                                <!-- /.box-header -->
                                <div class="form-horizontal">
                                    <fieldset>
                                        <asp:Label runat="server" ID="lblHeading"></asp:Label>
                                        <label class="hiddencol">
                                            <div>
                                                <asp:Label ID="lblTxt" runat="server" Text="ID"></asp:Label>
                                            </div>
                                            <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
                                        </label>

                                        <label class="hiddencol">
                                            <div>
                                                <asp:Label ID="Label9" runat="server" Text="Update2"></asp:Label>
                                            </div>
                                            <asp:TextBox ID="TxtUpdate2" runat="server"></asp:TextBox>
                                        </label>

                                        <label class="hiddencol">
                                            <div>
                                                <asp:Label ID="Label10" runat="server" Text="Update3"></asp:Label>
                                            </div>
                                            <asp:TextBox ID="TxtUpdate3" runat="server"></asp:TextBox>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label1" runat="server" Text="Service Type"></asp:Label>
                                            </div>
                                            <asp:DropDownList ID="ddlServType" runat="server" OnSelectedIndexChanged="ddlServType_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="LabelServ" runat="server" Text="Service Code"></asp:Label>
                                            </div>
                                            <asp:DropDownList ID="ddlServiceName" runat="server" OnSelectedIndexChanged="ddlServiceName_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </label>
                                        <label id="dvAmnt" runat="server">
                                            <div>
                                                <asp:Label ID="LabelDispAmt" runat="server" Text="DisplayAmnt"></asp:Label>
                                            </div>
                                            <%-- <asp:TextBox ID="txtDispAmt" runat="server"></asp:TextBox>--%>
                                            <asp:DropDownList ID="ddlDispAmt" runat="server" OnSelectedIndexChanged="ddlDispAmt_SelectedIndexChanged" AutoPostBack="false"></asp:DropDownList>
                                        </label>
                                        <label>
                                            <div id="divType">
                                                <asp:Label ID="LabelType" runat="server" Text="Type"></asp:Label>
                                            </div>
                                            <asp:DropDownList ID="ddlType" runat="server" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label5" runat="server" Text="Title"></asp:Label>
                                            </div>
                                            <asp:TextBox ID="txtTitle" CssClass="txtPM" onKeyPress="edValueKeyPress()" onKeyUp="edValueKeyPress()" runat="server"></asp:TextBox>
                                        </label>
                                        <label id="dvDesc" runat="server">
                                            <div>
                                                <asp:Label ID="Label3" runat="server" Text="Description"></asp:Label>
                                            </div>
                                            <asp:TextBox ID="TextMsg" CssClass="txtPM" onKeyPress="edValueKeyPress()" onKeyUp="edValueKeyPress()" runat="server"></asp:TextBox>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="LabelOfferVal" runat="server" Text="OfferValue"></asp:Label>
                                            </div>
                                            <asp:TextBox ID="txtOfferVal" OnTextChanged="txtOfferVal_TextChanged" runat="server"></asp:TextBox>
                                        </label>
                                        <label id="dvWebLink" runat="server">
                                            <div>
                                                <asp:Label ID="LabelWebLink" runat="server" Text="WebLink"></asp:Label>
                                            </div>
                                            <asp:TextBox ID="txtWebLink" runat="server"></asp:TextBox>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="FromDateLabel" runat="server" Text="From Date:"></asp:Label>
                                            </div>
                                            <asp:Label ID="Label14" runat="server" CssClass="label label-primary pull-right"></asp:Label>
                                            <asp:TextBox ID="FromDateTextBox" runat="server"></asp:TextBox>

                                            <%-- <cc1:CalendarExtender ID="FromDateTextBox_CalendarExtender" runat="server"
                                            Enabled="True" Format="yyyy-MM-dd" Animated="true" TargetControlID="FromDateTextBox"
                                            OnClientShowing="CurrentDateShowing" CssClass="cal_KimTheme">
                                        </cc1:CalendarExtender>--%>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="ToDateLabel" runat="server" Text="To Date:"></asp:Label>
                                            </div>
                                            <asp:Label ID="Label15" runat="server" CssClass="label label-primary pull-right"></asp:Label>
                                            <asp:TextBox ID="ToDateTextBox" runat="server" CssClass="form-control"></asp:TextBox>

                                            <%-- <cc1:CalendarExtender ID="ToDateTextBox_CalendarExtender" runat="server"
                                            Enabled="True" Format="yyyy-MM-dd" TargetControlID="ToDateTextBox"
                                            OnClientShowing="CurrentDateShowing" CssClass="cal_KimTheme">
                                        </cc1:CalendarExtender>--%>
                                        </label>
                                        <label>
                                            <div>
                                                <asp:Label ID="Label2" runat="server" Text="Status"></asp:Label>
                                            </div>
                                            <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </label>
                                        <div id="fileUp" runat="server">
                                            <label>
                                                <div>
                                                    <asp:Label ID="LabelUpload" runat="server" Text="Upload"></asp:Label>
                                                </div>
                                                <asp:FileUpload ID="FileUpload1" CssClass="col-sm-7" runat="server" ForeColor="White" />
                                                <br />

                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.jpeg|.gif|.mp4|.avi)$"
                                                    ControlToValidate="FileUpload1" runat="server" ForeColor="Red" ErrorMessage="Please select valid file."
                                                    Display="Dynamic" />
                                                <br />
                                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                                <asp:TextBox ID="urlHidden" CssClass="hiddencol" Visible="false" runat="server"></asp:TextBox>
                                            </label>
                                        </div>

                                        <label>
                                            <div>
                                                <a class="button-secondary pure-button button-small" role="button" data-toggle="collapse" href="#collapseExample" aria-expanded="true" aria-controls="collapseExample"> More </a>
                                            </div>
                                             <div class="collapse" id="collapseExample" aria-expanded="false" style="height: 0px;"> 
                                                 <br /><br />
                                        <div class="label-group input-below" id="lblSingle" runat="server">
                                            <label style="visibility: hidden">Showin HomePage</label>
                                            <label>
                                                <asp:CheckBox runat="server" ID="chkSingleV1" AutoPostBack="false" />
                                                <span class="text-default" style="color: #fff;">SingleQuantityV1</span>
                                            </label>
                                        </div>
                                        <div class="label-group input-below" id="lblSingleV2" runat="server">
                                            <label style="visibility: hidden">Showin HomePage</label>
                                            <label>
                                                <asp:CheckBox runat="server" ID="chkSingleV2" AutoPostBack="false" />
                                                <span class="text-default" style="color: #fff;">SingleQuantityV2</span>
                                            </label>
                                        </div>
                                        <div class="label-group input-below" id="lblMultiple" runat="server">
                                            <label style="visibility: hidden">Showin HomePage</label>
                                            <label>
                                                 <asp:CheckBox runat="server" ID="chkMultiple" onclick="ShowHideDivMulti(this)" AutoPostBack="false" />
                                                <%--<asp:CheckBox runat="server" ID="chkMultiple" onclick="ShowHideDivMultiNew(this)" AutoPostBack="false" />--%>
                                                <span class="text-default" style="color: #fff;">MultipleQuantity</span>
                                            </label>
                                        </div>
                                        <div class="" id="divMultiple" runat="server" style="display: none">
                                            <label>
                                                <div>
                                                    <asp:Label ID="Label4" runat="server" Text="Quantity:"></asp:Label>
                                                </div>

                                                <div class="label-group input-below" style="float: left; margin-top: 5px;">
                                                    <label>
                                                        <asp:CheckBox runat="server" ID="chkQ2" onclick="ShowHideDivQuantity2(this)" AutoPostBack="false" />
                                                        <span class="text-default" style="color: #fff;">Two</span>
                                                    </label>
                                                </div>
                                                <div class="label-group input-below" style="float: left; margin-top: 5px;">
                                                    <label>
                                                        <asp:CheckBox runat="server" ID="chkQ3" onclick="ShowHideDivQuantity3(this)" AutoPostBack="false" />
                                                        <span class="text-default" style="color: #fff;">Three</span>
                                                    </label>
                                                </div>
                                            </label>
                                        </div>
                                        <div class="well-sm">
                                            <div class=" well" id="divQuantity2" style="display: none" runat="server">
                                                <label>
                                                    <div>
                                                        <asp:Label ID="Label7" runat="server" Text="Quantity2 OfferValue "></asp:Label>
                                                    </div>
                                                    <asp:TextBox ID="txtMOfferValQ2" CssClass="txtPM" runat="server"></asp:TextBox>
                                                    <asp:Button ID ="btnQ2" CssClass="btn btn-warning btn-sm" runat="server" Visible="false" Text="DisableQ2" OnClick="Disable" />
                                                </label>
                                            </div>

                                            <div class=" well" id="divQuantity3" style="display: none" runat="server">
                                                <label>
                                                    <div>
                                                        <asp:Label ID="Label8" runat="server" Text="Quantity3 OfferValue "></asp:Label>
                                                    </div>
                                                    <asp:TextBox ID="txtMOfferValQ3" CssClass="txtPM" runat="server"></asp:TextBox>
                                                    <asp:Button ID ="btnQ3" CssClass="btn btn-warning btn-sm" runat="server" Visible="false" Text="DisableQ3" OnClick="Disable" />
                                                </label>
                                            </div>
                                        </div>
                                        <div class="label-group input-below">
                                            <label style="visibility: hidden">Showin HomePage</label>
                                            <label>
                                                <asp:CheckBox runat="server" ID="chkPush" onclick="ShowHideDiv(this)" AutoPostBack="false" />
                                                <span class="text-default" style="color: #fff;">Push Notification</span>
                                            </label>
                                        </div>
                                        <div class="well well-sm" id="dvPush" style="display: none">
                                            <label>
                                                <div>
                                                    <asp:Label ID="Label16" runat="server" Text="Platform"></asp:Label>
                                                </div>
                                                <asp:DropDownList ID="ddlPlatform" runat="server" AutoPostBack="false">
                                                </asp:DropDownList>
                                            </label>
                                            <label>
                                                <div>
                                                    <asp:Label ID="Label6" runat="server" Text="Message"></asp:Label>
                                                </div>
                                                <asp:TextBox ID="txtPushMsg" Text="" CssClass="txtPM" runat="server"></asp:TextBox>
                                            </label>
                                        </div>
                                        <div class="label-group input-below">
                                            <label style="visibility: hidden">Showin HomePage</label>
                                            <label>
                                                <asp:CheckBox runat="server" ID="chkHome" AutoPostBack="false" />
                                                <span class="text-default" style="color: #fff;">Show in HomePage</span>
                                            </label>
                                        </div>
                                        <div class="label-group input-below">
                                            <label style="visibility: hidden">Showin HomePage</label>
                                            <label>
                                                <asp:CheckBox runat="server" ID="chkIntOffer" AutoPostBack="false" />
                                                <span class="text-default" style="color: #fff;">Internal Offer</span>
                                            </label>
                                        </div>
                                        <div class="label-group input-below" style="padding-right: 10px">
                                            <label style="visibility: hidden">Showin HomePage</label>
                                            <label>
                                                <asp:CheckBox runat="server" ID="chkTest" AutoPostBack="false" />
                                                <span class="text-default" style="color: #fff;">isTesting</span>
                                            </label>
                                        </div>
                                                 </div>
                                        </label>
                                        <div class="submit-wrapper pull-right">
                                            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-default btn-sm" Text="Add Offer" />
                                        </div>
                                    </fieldset>
                                    
                                </div>
                        </div>
                        <div class="col-md-3"></div>                    
                    </div>
                    <div class="row">
                        <div class="col-lg-1"></div>
                        <div class="col-lg-10">
                            <div style="margin-bottom: 20px;"></div>
                            <div class="box box-success">
                                <h4 class="box-title">Offers & Greetings Details</h4>
                                <div class="box-body">
                                    <div class="table-responsive">
                                        <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered" HeaderStyle-ForeColor="White" AutoGenerateColumns="False" OnPageIndexChanging="OnPaging"
                                            BorderWidth="2px" CellPadding="0"
                                            HorizontalAlign="Center" PageSize="7" AllowPaging="true" DataKeyNames="ID">
                                            <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                            <HeaderStyle BackColor="Gray" Font-Bold="True" HorizontalAlign="Left" />
                                            <RowStyle HorizontalAlign="Left" />
                                            <Columns>
                                                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                                                <asp:BoundField DataField="Description" HeaderText="Description" NullDisplayText="None" SortExpression="Description" />
                                                <asp:BoundField DataField="StartDate" HeaderText="StartDate" DataFormatString="{0:yyyy-MM-dd}" SortExpression="StartDate" />
                                                <asp:BoundField DataField="EndDate" HeaderText="EndDate" DataFormatString="{0:yyyy-MM-dd}" SortExpression="EndDate" />
                                                <asp:BoundField DataField="ServiceCode" HeaderText="ServiceCode" SortExpression="ServiceCode" />
                                                <asp:BoundField DataField="Reference" HeaderText="Reference" ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" NullDisplayText="None" SortExpression="Reference" />
                                                <asp:BoundField DataField="Priority" HeaderText="Priority" ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" NullDisplayText="None" SortExpression="Priority" />
                                                <asp:BoundField DataField="ServiceType" HeaderText="Type" SortExpression="ServiceType" />
                                                <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                    <ItemTemplate><%#IIf(Boolean.Parse(Eval("Status").ToString()), "Active", "Inactive")%></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Status" HeaderText="Status" ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" SortExpression="Status" />
                                                <asp:BoundField DataField="Type" HeaderText="Type" ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" SortExpression="Type" />
                                                <asp:BoundField DataField="Url" HeaderText="Url" ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" SortExpression="Url" />
                                                <asp:BoundField DataField="ID" HeaderText="ID" ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" SortExpression="ID" />
                                                <asp:BoundField DataField="DisplayAmount" HeaderText="Denom" SortExpression="DisplayAmount" />
                                                <asp:BoundField DataField="Info1" HeaderText="Info1" ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" SortExpression="Info1" />
                                                <asp:BoundField DataField="Info2" HeaderText="Info2" ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" SortExpression="Info2" />
                                                <asp:BoundField DataField="Info3" HeaderText="Info3" ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" SortExpression="Info3" />
                                                <asp:BoundField DataField="isTesting" HeaderText="isTesting" ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" SortExpression="isTesting" />
                                                <asp:BoundField DataField="OfferValue" HeaderText="OfferValue" SortExpression="OfferValue" />
                                                <asp:BoundField DataField="IsSingleOffer" HeaderText="IsSingleV1" SortExpression="IsSingleOffer" />
                                                <asp:BoundField DataField="IsSingleOfferV2" HeaderText="IsSingleV2" SortExpression="IsSingleOffer" />
                                                <asp:BoundField DataField="ShowInService" HeaderText="InternalOffer" SortExpression="ShowInService" />
                                                <asp:BoundField DataField="CreatedDate" HeaderText="Date" SortExpression="CreatedDate" />
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
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GridView1" />
                    <asp:PostBackTrigger ControlID="btnSubmit" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <script type="text/javascript" src="Scripts/bootstrap-datepicker.js"></script>
    <script type="text/javascript">
        function pageLoad() {
            $("#ctl00_ContentPlaceHolder1_FromDateTextBox").unbind();
            $('#ctl00_ContentPlaceHolder1_FromDateTextBox').datepicker({
                format: "yyyy-mm-dd",
                todayBtn: "linked",
                todayHighlight: true,
                autoclose: true,
                multidate: false
            });
            $("#ctl00_ContentPlaceHolder1_ToDateTextBox").unbind();
            $('#ctl00_ContentPlaceHolder1_ToDateTextBox').datepicker({
                format: "yyyy-mm-dd",
                todayBtn: "linked",
                autoclose: true,
                todayHighlight: true
            });
        }
    </script>
</asp:Content>
