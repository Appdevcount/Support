<%@ Page Title="MConnect PINS" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="MConnectPinFetch.aspx.vb" Inherits="MConnectPinFetch" %>
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
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.95;
        }

        .modalPopup
        {
            background-color:  #272b30;
            color: #f5f5f5;
            position: relative;
            overflow-y: auto;
            padding: 15px;
            border-bottom: 1px solid #1c1e22;
            width: 300px;
            max-width: 800px;
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
    <div class="wrapper">
        <!-- Content Wrapper. Contains page content -->
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <div class="content-header">
                <h4>PINS
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
                                <p>Place PIN Request</p>
                                <label>
                                    <div>
                                        <asp:Label ID="Label1" runat="server" Text="Service Name"></asp:Label>
                                    </div>
                                    <asp:DropDownList ID="ddlServType" OnSelectedIndexChanged="ddlServType_SelectedIndexChanged" runat="server" AutoPostBack="true">
                                    </asp:DropDownList>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Label2" runat="server" Text="Denomination"></asp:Label>
                                    </div>
                                    <asp:DropDownList ID="ddlDenomination" runat="server" OnSelectedIndexChanged="ddlDenomination_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </label>
                                 <label>
                                    <div>
                                        <asp:Label ID="Label4" runat="server" Text="Quantity"></asp:Label>
                                    </div>
                                     <asp:TextBox ID="txtQuantity" runat="server"></asp:TextBox>
                                    <%--<asp:DropDownList ID="ddlQuantity" runat="server" AutoPostBack="false">
                                    </asp:DropDownList>--%>
                                </label>
                                <label>
                                    <div>
                                        <asp:Label ID="Amounttxt" CssClass="text-info" runat="server" Font-Bold="true" ></asp:Label>
                                    </div>
                                    
                                </label>
                               <%-- <div class="label-group input-below">
                                    <label style="visibility: hidden">Push Notificationss</label>
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkQuantity" AutoPostBack="true" />
                                        <span class="text-default" style="color: #fff;">Other Quantity</span>
                                    </label>
                                </div>
                                <label>
                                    <div>
                                        <asp:Label ID="Label5" runat="server" Text="Quantity"></asp:Label>
                                    </div>
                                    <asp:textbox ID="txtQuantity" runat="server" AutoPostBack="false">
                                    </asp:textbox>
                                </label>--%>
                                <div class="submit-wrapper pull-right">
                                    <asp:LinkButton ID="lnkEdit" CssClass="pure-button button-success" Text="Add Order" runat="server" OnClick="Edit"></asp:LinkButton>
                                    
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
                                <h4 class="box-title">PINS Details</h4>
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
                                            <asp:BoundField DataField="Service" HeaderText="Service" SortExpression="Service" />
                                            <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                                            <asp:BoundField DataField="Amount2" HeaderText="Amount2" SortExpression="Amount2" />
                                            <asp:BoundField DataField="QuantityOrdered" HeaderText="Quantity" SortExpression="QuantityOrdered" />
                                            <asp:BoundField DataField="Status" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="Status" SortExpression="Status" />
                                            <asp:BoundField DataField="QuantityServed" HeaderText="QuantityServed" SortExpression="QuantityServed" />
                                            <asp:BoundField DataField="TransDate" HeaderText="OrderDate" SortExpression="TransDate" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>

                       <asp:Panel ID="pnlAddEdit" runat="server" CssClass="panel panel-danger modalPopup">
                           <h4>Are you sure?</h4>
                           <table class="table">
                             <%-- <thead>
                                <tr>
                                  <th>#</th>
                                  <th>First Name</th>
                                  <th>Last Name</th>
                                  <th>Username</th>
                                </tr>
                              </thead>--%>
                              <tbody>
                                <tr>
                                  <th scope="row">Service</th>
                                  <td runat="server" id="ServiceValue"></td>
                                </tr>
                                <tr>
                                  <th scope="row">Denomination</th>
                                  <td runat="server" id="DenominationValue"></td>
                                </tr>
                                <tr>
                                  <th scope="row">Amount</th>
                                  <td runat="server" id="amount2Value"></td>
                                </tr>
                                <tr>
                                  <th scope="row">Quantity</th>
                                  <td runat="server" id="QuantityValue"></td>
                                </tr>
                              </tbody>
                              
                            </table>
                           <br />
                            <div class="submit-wrapper">
                                <div class="">
                                    <asp:Button ID="btnCancel" CssClass="button-warning pure-button" runat="server" Text="Cancel" OnClientClick="return Hidepopup()" />
                                    <asp:Button ID="btnSubmit" runat="server" CssClass="pure-button button-success" Text="Add Order" />
                                </div>
                            </div>
                            <%--<div class="form-horizontal">
                                <p>Are you sure?</p>
                                <fieldset>
                                   
                                    <label>
                                        <div>
                                            <asp:Label ID="ServiceEdit" runat="server" Text="Service"></asp:Label>
                                        </div>
                                            <asp:Label ID="ServiceValue" runat="server" Text=""></asp:Label>
                                    </label>
                                    <label>
                                        <div>
                                            <asp:Label ID="Denomination" runat="server" Text="Denomination"></asp:Label>
                                        </div>
                                        <asp:Label ID="DenominationValue" runat="server" Text=""></asp:Label>
                                    </label>
                                    <label>
                                        <div>
                                            <asp:Label ID="Quantity" runat="server" Text="Quantity"></asp:Label>
                                        </div>
                                        <asp:Label ID="QuantityValue" runat="server" Text=""></asp:Label>
                                    </label>
                                    <div style="border: none;">
                                        <div class="submit-wrapper pull-right">
                                            <div class="pull-right">
                                                <asp:Button ID="btnCancel" CssClass="button-warning pure-button" runat="server" Text="Cancel" OnClientClick="return Hidepopup()" />
                                                <asp:Button ID="btnSubmit" runat="server" CssClass="pure-button button-success" Text="Add Order" />
                                            </div>
                                        </div>
                                    </div>
                                     <div style="border: none;">
                                        <div class="submit-wrapper pull-left">
                                            
                                        </div>
                                    </div>
                                </fieldset>
                            </div>--%>
                        </asp:Panel>
                        <center>
                            <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
                            <cc1:ModalPopupExtender ID="popup" runat="server" DropShadow="false"
                                PopupControlID="pnlAddEdit" TargetControlID="lnkFake"
                                BackgroundCssClass="modalBackground">
                            </cc1:ModalPopupExtender>
                        </center>

                
        </div>
    </div>
                    </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GridView1" />
                </Triggers>
            </asp:UpdatePanel>
</asp:Content>



