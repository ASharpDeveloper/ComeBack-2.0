<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="ComeBack_2._0.loginaspx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="content-wrapper">
         <div class="container">
        <div class="row pad-botm">
            <div class="col-md-12">
                <h4 class="header-line">Sign In / Register</h4>
                
                            </div>

        </div>
             <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-12">
               <div class="panel panel-info">
                        <div class="panel-heading">
                           SIGN IN
                        </div>
                        <div class="panel-body">
                            <div role="form">
                                        <div class="form-group">
                                            <label>Enter Name</label>
                                            <asp:TextBox ID="txtusernameLogin" CssClass="form-control" runat="server"></asp:TextBox>
                                           
                                        </div>
                                 <div class="form-group">
                                            <label>Enter Password</label>
                                     <asp:TextBox ID="txtpasswordLogin" Type="password" CssClass="form-control" runat="server"></asp:TextBox>
                                    
                                  </div>
                                 
                                <asp:Button ID="btnsignin" CssClass="btn btn-info" runat="server" Text="Sign In" OnClick="btnsignin_Click"/> 
                                <asp:Label ID="lblerrorlogin" runat="server" Visible="false" Text="Label"></asp:Label>
                                    </div>
                            </div>
                        </div>
                            </div>
<div class="col-md-6 col-sm-6 col-xs-12">
               <div class="panel panel-danger">
                        <div class="panel-heading">
                           SIGN UP
                        </div>
                        <div class="panel-body">
                            <div role="form">
                                        
                                 <div class="form-group">
                                            <label>Enter Username</label>
                                            
                                     <asp:TextBox ID="txtusername" class="form-control" runat="server"></asp:TextBox>
                                     
                                        </div>
                                            <div class="form-group">
                                            <label>Enter Password</label>
                                                <asp:TextBox ID="txtpassword" CssClass="form-control" runat="server"></asp:TextBox>
                                    
                                        </div>
                                <div class="form-group">
                                           
                                        </div>
                                <asp:Button ID="btnsignup" runat="server" CssClass="btn btn-danger" Text="Sign Up" OnClick="btnsignup_Click" />
                                        
                                <asp:Label ID="lblerror" runat="server" Visible="false" Text="Label"></asp:Label>
                                    </div>
                            </div>
                        </div>
                            </div>
        </div>
             </div>
         </div>
</asp:Content>
