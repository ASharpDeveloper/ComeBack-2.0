<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AddLog.aspx.cs" Inherits="ComeBack_2._0.AddLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="content-wrapper">
         <div class="container">
        <div class="row pad-botm">
            <div class="col-md-12">
                <h4 class="header-line">FORM EXAMPLES</h4>
                
                            </div>

        </div>
             <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-12">
               <div class="panel panel-info">
                        <div class="panel-heading">
                           BASIC FORM
                        </div>
                        <div class="panel-body">
                            <div role="form">
                                        <div class="form-group">
                                            <label>Session Name</label>
                                            <asp:textbox  ID="txtsession" class="form-control" runat="server"></asp:textbox>
                                            <p class="help-block">Help text here.</p>
                                        </div>
                                 
                                  <div class="form-group">
                                            <label>Exercise 1:</label>
                                            <asp:textbox  ID="exercise1" class="form-control" runat="server"></asp:textbox>
                                            <p class="help-block">Help text here.</p>
                                        </div>
                                <div class="form-group">
                                            <label>Exercise 2:</label>
                                            <asp:textbox  ID="exercise2" class="form-control" runat="server"></asp:textbox>
                                            <p class="help-block">Help text here.</p>
                                        </div>
                                <div class="form-group">
                                            <label>Exercise 3:</label>
                                            <asp:textbox  ID="exercise3" class="form-control" runat="server"></asp:textbox>
                                            <p class="help-block">Help text here.</p>
                                        </div>
                                <div class="form-group">
                                            <label>Exercise 4:</label>
                                            <asp:textbox  ID="exercise4" class="form-control" runat="server"></asp:textbox>
                                            <p class="help-block">Help text here.</p>
                                        </div>
                                <div class="form-group">
                                            <label>Exercise 5:</label>
                                            <asp:textbox  ID="exercise5" class="form-control" runat="server"></asp:textbox>
                                            <p class="help-block">Help text here.</p>
                                        </div>
                                 
                                        <button type="submit" class="btn btn-info">Send Message </button>

                                    </div>
                            </div>
                        </div>
                            </div>
                 </div>
             </div>
         </div>
</asp:Content>
