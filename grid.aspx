<%@ Page Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="grid.aspx.vb" Inherits="grid" title="Pay-it Support-Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link href="Stylesheet/mainstyle.css" rel="stylesheet" />
    <script src="Scripts/alertify.js"></script>
    <link href="Stylesheet/alertify.css" rel="stylesheet" />
    <link href="Stylesheet/themes/default.css" rel="stylesheet" />
    <br />

    <div class="container">
      <!-- Example row of columns -->
      <div class="row">
          <div class="col-md-1"></div>
          <div class="col-md-4">
              <h4><asp:Label ID="Label1" runat="server" Text="Transaction Details"
                  Font-Bold="True" Font-Size="Larger" ForeColor="White"></asp:Label></h4>
              
              <asp:DetailsView ID="DetailsView1" runat="server" CssClass="table table-bordered table-condensed" BackColor="Black"
                  BorderWidth="1px" CellPadding="2"
                  GridLines="None" Height="50px" HorizontalAlign="Center" Width="199px">
              </asp:DetailsView>
          </div>
          <div class="col-md-3">
              
              <div class="panel panel-default">
                  <div class="panel-body">
                      <div class="input-group-sm">
                          <asp:HiddenField ID="LabelProcessID" runat="server"></asp:HiddenField>

                          <div class="form-group">
                              <asp:Label ID="LabelID" CssClass="label label-default " Font-Bold="true" runat="server" Height="17px" Text="Reference ID:"></asp:Label>
                              <asp:TextBox ID="TextID" CssClass="form-control form-sm" runat="server" Height="35px" Width="130px" />
                          </div>
                          <div class="form-group">
                              <asp:Label ID="LabelProcess" CssClass="label label-default " Font-Bold="true" runat="server" Height="17px" Text="KNET Process"></asp:Label>
                              <asp:DropDownList ID="ddlProcess" runat="server" CssClass="form-control form-sm" AutoPostBack="false" Height="35px" Width="130px">
                              </asp:DropDownList>
                          </div>
                          <div class="form-group">
                              <asp:Label ID="Label3" CssClass="label label-default " Font-Bold="true" runat="server" Height="17px" Text="Result"></asp:Label>
                              <asp:DropDownList ID="ddlResult" runat="server" CssClass="form-control" AutoPostBack="false" Height="35px" Width="130px">
                              </asp:DropDownList>
                          </div>
                      </div>
                  </div>
                  <div class="panel-footer">
                      <asp:Button ID="btnSubmit" CssClass="btn btn-default btn-sm" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                  </div>
              </div>
          </div>
          <div class="col-md-3">
              
              <div class="panel panel-primary">
                  <div class="panel-body">
                      <div class="input-group-sm">
                          <asp:TextBox ID="TextHidden" CssClass="form-control" Visible="false" runat="server" />

                          <div class="form-group">
                              <asp:Label ID="Label2" CssClass="label label-warning" Font-Bold="true" runat="server" Height="17px" Text="Mobile No"></asp:Label>
                              <asp:TextBox ID="TextMobile" CssClass="form-control" runat="server" Height="35px" Width="130px" />
                          </div>
                          <div class="form-group">
                              <asp:Label ID="Label4" CssClass="label label-warning" Font-Bold="true" runat="server" Height="17px" Text="Service Code"></asp:Label>
                              <asp:DropDownList ID="ddlService" runat="server" CssClass="form-control" AutoPostBack="false" Height="35px" Width="130px">
                              </asp:DropDownList>
                          </div>
                          <div class="form-group">
                              <asp:Label ID="Label5" CssClass="label label-warning" Font-Bold="true" runat="server" Height="17px" Text="Amount"></asp:Label>
                              <asp:TextBox ID="TextAmnt" CssClass="form-control" runat="server" Height="35px" Width="130px"/>
                          </div>
                      </div>
                  </div>
                  <div class="panel-footer">
                      <asp:Button ID="btnSubmit2" CssClass="btn btn-warning btn-sm" runat="server" Text="Submit" OnClick="btnSubmit2_Click" />
                  </div>
              </div>
          </div>
          </div>
      </div>
   
    
</asp:Content>
    

