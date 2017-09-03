<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage2.master" EnableEventValidation="false" AutoEventWireup="false" CodeFile="CatServices.aspx.vb" Inherits="CatServices" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <script src="Scripts/alertify.js"></script>
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
    <link rel="stylesheet" href="Stylesheet/pure.css">
    <link href="Stylesheet/forms.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://cdn.jsdelivr.net/jquery.simpletip/1.3.1/jquery.simpletip-1.3.1.min.js"></script>
     
    <script type="text/javascript">
        $(document).ready(function () {
           // $("#contact").hide();
            $("#aa").show();
            $("#ctl00_ContentPlaceHolder1_GridView1_ctl02_lnkEdit").click(function () {
                document.getElementById('contact').classList.add('show');
                document.getElementById('aa').classList.add('hide');
            })
            if (isPostBack) { document.getElementById('contact').classList.add('show'); document.getElementById('aa').classList.add('hide'); }
        });
    </script>
    <style>
    .tooltip
    {
        position: absolute;
        top: 0;
        left: 0;
        z-index: 3;
        display: none;
        background-color: #808080;
        color: White;
        padding: 5px;
        font-size: 10pt;
        font-family: Arial;
    }
    .hiddencol
    {
        display: none;
    }
    .trimmer
    {
       border-bottom: 1px dotted blue;
	    white-space: pre-wrap;
	    overflow: hidden;
    }
    </style>
    <script type="text/javascript">
        function showDiv() {
            document.getElementById('contact').style.display = "block";

        }
        function block_none() {
            document.getElementById('contact').classList.add('show');
           // $("#contact").show();
            document.getElementById('aa').classList.add('hide');
        }
        function block_edit() {
            document.getElementById('contact').classList.add('show');
            document.getElementById('aa').classList.add('hide');
        }
    </script>
   
    <script type="text/javascript">
        $(document).ready(function () {
            setTimeout(function () {
                $('#<%= dberrorlabel.ClientId%>').hide();
        }, 2000); //
    });
    </script>

    <script type="text/javascript">
        $(function () {
            $('[id*=GridView1] tr').each(function () {
                var toolTip = $(this).attr("title");
                $(this).find("td").each(function () {
                    $(this).simpletip({
                        content: toolTip
                    });
                });
                $(this).removeAttr("title");
            });
        });
    </script>
    <div class="wrapper">
        <!-- Content Wrapper. Contains page content -->
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <div class="content-header">
               
                <ol class="pull-right">
                    <asp:LinkButton ID="LinkButton1" runat="server">Export Report To Excel</asp:LinkButton>
                </ol>
            </div>
            <div class="content">
                <div class="alert-danger">
                    <asp:Label ID="dberrorlabel" Font-Size="Larger" Font-Bold="true" runat="server" Text=""></asp:Label>
                </div>
                <div class="row hide" id="contact" style="display:none;">
                    <div class="col-md-2"></div>
                    <!-- ./col -->
                     <div class="col-md-6 col-md-offset-3">
                        <div class="">
                           
                            <div class="form-horizontal">
                                <p><asp:Label ID="lblHeading" runat="server" Text="Add Service"></asp:Label></p>
                                <div class="form-group hiddencol">
                                    <asp:Label ID="lblID" runat="server" CssClass="col-sm-4 control-label" Text="ID"></asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtID" runat="server" CssClass="form-control" ></asp:TextBox>
                                    </div>
                                </div>
					            <label>
						            <div><asp:Label ID="Label1" runat="server" Text="Service Code"></asp:Label></div>
                                    <asp:TextBox ID="txtServiceCode" runat="server" ></asp:TextBox>
					            </label>
                                <label>
						            <div><asp:Label ID="Label4" runat="server" Text="Service Name"></asp:Label></div>
                                    <asp:TextBox ID="txtServiceName" runat="server" ></asp:TextBox>
					            </label>
                                <label>
						            <div><asp:Label ID="Label3" runat="server" Text="Service Description"></asp:Label></div>
						            <asp:TextBox ID="serviceDesc" runat="server" ></asp:TextBox>
					            </label>
                                <label>
						            <div><asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label></div>
						            <asp:DropDownList ID="ddlStatus" runat="server"  AutoPostBack="false">
                                    </asp:DropDownList>
					            </label>
                                <label>
						            <div><asp:Label ID="Label6" runat="server" Text="KNETPayTunnel"></asp:Label></div>
						            <asp:DropDownList ID="ddlknetPay" runat="server"  AutoPostBack="false">
                                    </asp:DropDownList>
					            </label>
                               <label>
						            <div><asp:Label ID="Label7" runat="server" Text="MinAmtThreshold"></asp:Label></div>
						            <asp:TextBox ID="minAmnt" runat="server" ></asp:TextBox>
					            </label>
                                <label>
						            <div><asp:Label ID="Label8" runat="server" Text="Logo URL"></asp:Label></div>
						            <asp:TextBox ID="logoURL" runat="server" ></asp:TextBox>
					            </label>
                                <label>
						            <div><asp:Label ID="Label5" runat="server" Text="Type"></asp:Label></div>
						            <asp:DropDownList ID="ddlType" runat="server"  AutoPostBack="false">
                                    </asp:DropDownList>
					            </label>
                               <label>
						            <div><asp:Label ID="Label9" runat="server" Text="Payment Channel"></asp:Label></div>
						            <asp:TextBox ID="payChannel" runat="server" ></asp:TextBox>
					            </label>
                                <label>
						            <div><asp:Label ID="Label10" runat="server" Text="Help"></asp:Label></div>
						            <asp:TextBox ID="Help" runat="server" ></asp:TextBox>
					            </label>
                                <label>
						            <div><asp:Label ID="Label11" runat="server" Text="ServiceNameAR"></asp:Label></div>
						            <asp:TextBox ID="ServiceNameAR" runat="server" ></asp:TextBox>
					            </label>
                                <label>
						            <div><asp:Label ID="Label12" runat="server" Text="ServiceDescAR"></asp:Label></div>
						            <asp:TextBox ID="ServiceDescriptionAR" runat="server" ></asp:TextBox>
					            </label>
                                <label>
						            <div><asp:Label ID="Label13" runat="server" Text="HelpAR"></asp:Label></div>
						            <asp:TextBox ID="HelpAR" runat="server" ></asp:TextBox>
					            </label>
                                <label>
						            <div><asp:Label ID="lblSerOrder" runat="server" Text="Service Order"></asp:Label></div>
						            <asp:TextBox ID="txtSerOrder" runat="server" ></asp:TextBox>
					            </label>
                                <label>
						            <div><asp:Label ID="lblStatNew" runat="server" Text="Status New"></asp:Label></div>
						            <asp:DropDownList ID="ddlStatusNew" runat="server"  AutoPostBack="false">
                                    </asp:DropDownList>
					            </label>
                                 <label>
						            <div><asp:Label ID="lblLogoNew" runat="server" Text="Logo New"></asp:Label></div>
						            <asp:TextBox ID="txtLogoNew" runat="server" ></asp:TextBox>
					            </label>
                                <div class="submit-wrapper pull-right">
                                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-default btn-sm" Text="Add Service" />
                                    <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-primary btn-sm" Text="Save Service" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1"></div>
                </div>
                <div class="row">
                    <div class="col-lg-12">
                        <div class="box box-success">
                            <a href="#" id="aa"  onclick="block_none()" class="btn btn-default btn-sm pull-right">Add Service</a>
                            <div class="box-header with-border">
                                <div style="margin-bottom:20px;"></div>
                                <h3 class="box-title">Service Details</h3>
                            </div>
                            <!-- /.box-header -->
                            <div class="box-body">
                                        
                                        <asp:GridView ID="GridView1" runat="server" BorderColor="DimGray" BorderStyle="Outset"
                                            BorderWidth="2px" CellPadding="0" Font-Bold="True" Font-Size="Smaller" ForeColor="White"
                                            HorizontalAlign="Center" PageSize="25" OnPageIndexChanging="OnPaging" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="ID">
                                            <RowStyle HorizontalAlign="Center" Font-Names="Times New Roman" />
                                            <FooterStyle BorderWidth="5px" />
                                            <PagerStyle HorizontalAlign="Center" CssClass = "GridPager" />
                                            <HeaderStyle BorderStyle="Dotted" ForeColor="#00C0C0" />
                                            <AlternatingRowStyle BackColor="#666666" BorderColor="#666666" Font-Bold="False"
                                                ForeColor="White" BorderWidth="1px" />
                                            <Columns>
                                                <asp:BoundField HeaderText="ID"
                                                    DataField="ID"
                                                    SortExpression="ID"
                                                    ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" 
                                                    HtmlEncode="true"></asp:BoundField>
                                                <asp:BoundField HeaderText="ServiceCode"
                                                    DataField="ServiceCode"
                                                    SortExpression="ServiceCode"
                                                    ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" 
                                                    HtmlEncode="true"></asp:BoundField>

                                                <asp:BoundField HeaderText="ServiceName"
                                                    DataField="ServiceName"
                                                    SortExpression="ServiceName"
                                                    HtmlEncode="true"></asp:BoundField>

                                                <asp:BoundField HeaderText="ServiceDescription"
                                                    DataField="ServiceDescription"
                                                    SortExpression="ServiceDescription"
                                                    ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" 
                                                    HtmlEncode="true"></asp:BoundField>

                                                <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                    <ItemTemplate><%#IIf(Boolean.Parse(Eval("Status").ToString()), "Active", "Inactive")%></ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField HeaderText="Status" 
                                                DataField="Status" 
                                                SortExpression="Status"
                                                HtmlEncode = "true"
                                                ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" ></asp:BoundField>

                                                <asp:BoundField HeaderText="KNetPayment"
                                                    DataField="KNetPaymentTunnel"
                                                    SortExpression="KNetPaymentTunnel"
                                                    HtmlEncode="true">
                                                </asp:BoundField>

                                                <asp:BoundField HeaderText="MinAmtThreshold"
                                                    DataField="MinAmtThreshold"
                                                    SortExpression="MinAmtThreshold"
                                                    HtmlEncode="true"></asp:BoundField>

                                                <asp:BoundField HeaderText="Logo" 
                                                    DataField="Logo"
                                                    SortExpression="Logo"
                                                    HtmlEncode = "true"
                                                    ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" ></asp:BoundField>

                                                <asp:ImageField DataImageUrlField = "Logo"
                                                 ControlStyle-Width = "50" ControlStyle-Height = "100"
                                                    Visible="false"
                                                 HeaderText = "Logo"/>
                                                
                                                <asp:BoundField HeaderText="Type"
                                                    DataField="Type"
                                                    ControlStyle-Width = "50"
                                                    SortExpression="Type"
                                                    HtmlEncode="true"></asp:BoundField>

                                                <asp:TemplateField HeaderText="PaymentChannels"  ItemStyle-Wrap="true" SortExpression="PaymentChannels">
                                                     <ItemTemplate>
                                                         <asp:Label ID="Label1" runat="server" Text='<%# Bind("PaymentChannels")%>' ToolTip ='<%# Bind("PaymentChannels") %>'></asp:Label>
                                                     </ItemTemplate>
                                                    <ControlStyle Height="50px" CssClass="trimmer" />
                                                </asp:TemplateField>

                                                <asp:BoundField HeaderText="PaymentChannels"
                                                    DataField="PaymentChannels"
                                                    ControlStyle-Width = "50"
                                                    SortExpression="PaymentChannels"
                                                    HtmlEncode="true"
                                                    ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" ></asp:BoundField>

                                                 <asp:BoundField HeaderText="Help"
                                                    DataField="Help"
                                                    ControlStyle-Width = "50"
                                                    SortExpression="Help"
                                                    HtmlEncode="true"
                                                    ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" ></asp:BoundField>

                                                <asp:TemplateField HeaderText="Help"  ItemStyle-Wrap="true" ControlStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol"  SortExpression="Help">
                                                     <ItemTemplate>
                                                         <asp:Label ID="Label2" runat="server" Text='<%# Bind("Help")%>' ToolTip ='<%# Bind("Help") %>'></asp:Label>
                                                     </ItemTemplate>
                                                    <ControlStyle Height="50px" Width="50px"/>
                                                </asp:TemplateField>

                                                 <asp:BoundField HeaderText="ServiceNameAR"
                                                    DataField="ServiceNameAR"
                                                     ControlStyle-Width = "50"
                                                    SortExpression="ServiceNameAR"
                                                    HtmlEncode="true"
                                                    ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" ></asp:BoundField>

                                                <asp:TemplateField HeaderText="ServiceNameAR" ItemStyle-Wrap="true" ControlStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" SortExpression="ServiceNameAR">
                                                     <ItemTemplate>
                                                         <asp:Label ID="lblSerNameAr" runat="server" Text='<%# Bind("ServiceNameAR")%>' ToolTip ='<%# Bind("ServiceNameAR") %>'></asp:Label>
                                                     </ItemTemplate>
                                                    <ControlStyle Height="50px" Width="50px" CssClass="trimmer" />
                                                </asp:TemplateField>

                                                <asp:BoundField HeaderText="ServiceDescriptionAR"
                                                    DataField="ServiceDescriptionAR"
                                                     ControlStyle-Width = "50"
                                                    SortExpression="ServiceDescriptionAR"
                                                    HtmlEncode="true"
                                                    ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" ></asp:BoundField>

                                                <asp:TemplateField HeaderText="ServiceDescriptionAR" ItemStyle-Wrap="true" ControlStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" SortExpression="ServiceDescriptionAR">
                                                     <ItemTemplate>
                                                         <asp:Label ID="lblSerDescAr" runat="server" Text='<%# Bind("ServiceDescriptionAR")%>' ToolTip ='<%# Bind("ServiceDescriptionAR") %>'></asp:Label>
                                                     </ItemTemplate>
                                                    <ControlStyle Height="50px" Width="50px" CssClass="trimmer" />
                                                </asp:TemplateField>

                                                <asp:BoundField HeaderText="HelpAR"
                                                    DataField="HelpAR"
                                                    SortExpression="HelpAR"
                                                    ControlStyle-Width = "50"
                                                    HtmlEncode="true"
                                                    ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" ></asp:BoundField>
                                                <asp:BoundField HeaderText="ServiceOrder"
                                                    DataField="ServiceOrder"
                                                    SortExpression="ServiceOrder"
                                                    ControlStyle-Width = "50"
                                                    HtmlEncode="true"
                                                    ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" ></asp:BoundField>
                                                <asp:BoundField HeaderText="StatusNew"
                                                    DataField="StatusNew"
                                                    SortExpression="StatusNew"
                                                    ControlStyle-Width = "50"
                                                    HtmlEncode="true"
                                                    ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" ></asp:BoundField>
                                                <asp:BoundField HeaderText="LogoNew"
                                                    DataField="LogoNew"
                                                    SortExpression="LogoNew"
                                                    ControlStyle-Width = "50"
                                                    HtmlEncode="true"
                                                    ControlStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" ></asp:BoundField>

                                                <asp:TemplateField HeaderText="HelpAR" ControlStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" SortExpression="HelpAR">
                                                     <ItemTemplate>
                                                         <asp:Label ID="lblHelpAr" CssClass="hiddencol" runat="server" Text='<%# Bind("HelpAR")%>' ToolTip ='<%# Bind("HelpAR") %>'></asp:Label>
                                                     </ItemTemplate>
                                                    <ControlStyle Height="50px" Width="50px" CssClass="trimmer" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" ForeColor="#0000EE" runat="server" Text="Edit" OnClientClick="block_edit()" OnClick="Edit"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                  
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>