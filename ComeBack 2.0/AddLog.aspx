<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AddLog.aspx.cs" Inherits="ComeBack_2._0.AddLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="content-wrapper">
         <div class="container">
        <div class="row pad-botm">
            <div class="col-md-12">
                <h4 class="header-line">Excercise Log</h4>
                
                            </div>

        </div>
             <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-12">
               <div class="panel panel-info">
                        <div class="panel-heading">
                           Log your Exercise
                        </div>
                        <div class="panel-body">
                            <div role="form">
                                        <div class="form-group">
                                            <label>Session Name</label>
                                            <asp:textbox  ID="txtsession" class="form-control" runat="server"></asp:textbox>
                                           
                                        </div>
                                 
                                  <div class="form-group">
                                            <label>Exercise 1:</label>
                                            <asp:textbox  ID="exercise1" class="form-control" runat="server"></asp:textbox>
                                            
                                        </div>
                                <div class="form-group">
                                            <label>Exercise 2:</label>
                                            <asp:textbox  ID="exercise2" class="form-control" runat="server"></asp:textbox>
                                          
                                        </div>
                                <div class="form-group">
                                            <label>Exercise 3:</label>
                                            <asp:textbox  ID="exercise3" class="form-control" runat="server"></asp:textbox>
                                           
                                        </div>
                                <div class="form-group">
                                            <label>Exercise 4:</label>
                                            <asp:textbox  ID="exercise4" class="form-control" runat="server"></asp:textbox>
                                          
                                        </div>
                                <div class="form-group">
                                            <label>Exercise 5:</label>
                                            <asp:textbox  ID="exercise5" class="form-control" runat="server"></asp:textbox>
                                           
                                        </div>
                                 <div class="form-group">
                                            <label>How much did you weigh after this workout? (KG):</label>
                                            <asp:textbox  ID="txtweight" class="form-control" runat="server"></asp:textbox>
                                           
                                        </div>
                                 <hr />
                                 <div class="form-group">
                                            <label>Rate your Exercise:</label>
                                     <asp:DropDownList ID="DropDownRating" class="form-control" runat="server"></asp:DropDownList>
                                            <p class="help-block">Select 1-10 from list!</p>
                                        </div>
                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-info" Text="Add Log" OnClick="btnSubmit_Click"/>

                                    </div>
                            </div>
                        </div>
                            </div>
                 </div>
             </div>
         </div>
</asp:Content>
