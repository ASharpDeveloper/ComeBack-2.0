<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="ComeBack_2._0.home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="content-wrapper">
         <div class="container">
        <div class="row pad-botm">
            <div class="col-md-12">
                
                <asp:Literal ID="litHello" runat="server"></asp:Literal>
                            </div>

        </div>
             
             <div class="row">
                 
            <asp:Literal ID="LitLog" runat="server"></asp:Literal>
             </div>
                     
             
         <div class="row">

              <div class="col-md-8 col-sm-8 col-xs-12">
                    <div id="carousel-example" class="carousel slide slide-bdr" data-ride="carousel" >
                   
                    <div class="carousel-inner">
                        <div class="item active">

                            <img src="assets/img/1.jpg" alt="" />
                           
                        </div>
                        <div class="item">
                            <img src="assets/img/2.jpg" alt="" />
                          
                        </div>
                        <div class="item">
                            <img src="assets/img/3.jpg" alt="" />
                           
                        </div>
                    </div>
                    <!--INDICATORS-->
                     <ol class="carousel-indicators">
                        <li data-target="#carousel-example" data-slide-to="0" class="active"></li>
                        <li data-target="#carousel-example" data-slide-to="1"></li>
                        <li data-target="#carousel-example" data-slide-to="2"></li>
                    </ol>
                    <!--PREVIUS-NEXT BUTTONS-->
                     <a class="left carousel-control" href="#carousel-example" data-slide="prev">
    <span class="glyphicon glyphicon-chevron-left"></span>
  </a>
  <a class="right carousel-control" href="#carousel-example" data-slide="next">
    <span class="glyphicon glyphicon-chevron-right"></span>
  </a>
                </div>
              </div>
                        
                       
                 
             </div>
             <br />
             <div class="row">
             <div class="col-md-8 col-sm-8 col-xs-12">
                      <div class="panel panel-success">
                        <div class="panel-heading">
                           Schedule (<asp:Label ID="lblmonth" runat="server" Text="Label"></asp:Label>)
                        </div>
                        <div class="panel-body">
                            <div class="table-responsive">
                                <table class="table table-striped table-bordered table-hover">
                                    <thead>
                                        <tr>
                                            <th>#</th>
                                            <th>Date</th>
                                            <th>Session</th>
                                            
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>1</td>
                                            <td>Monday</td>
                                            <td>
                                                <asp:Label ID="lblmonday" runat="server" Text="Label"></asp:Label>
                                            </td>
                                            
                                        </tr>
                                        <tr>
                                            <td>2</td>
                                            <td>Tuesday</td>
                                            <td>
                                                <asp:Label ID="lbltuesday" runat="server" Text="Label"></asp:Label>
                                            </td>
                                            
                                        </tr>
                                        <tr>
                                            <td>3</td>
                                            <td>Wednesday</td>
                                            <td>
                                                <asp:Label ID="lblwednesday" runat="server" Text="Label"></asp:Label>
                                            </td>
                                          
                                        </tr>
                                         <tr>
                                            <td>4</td>
                                            <td>Thursday</td>
                                            <td>
                                                <asp:Label ID="lblthursday" runat="server" Text="Label"></asp:Label>
                                            </td>
                                          
                                        </tr>
                                        <tr>
                                            <td>5</td>
                                            <td>Friday</td>
                                            <td>
                                                <asp:Label ID="lblfriday" runat="server" Text="Label"></asp:Label>
                                            </td>
                                        
                                        </tr>
                                        <tr>
                                            <td>6</td>
                                            <td>Saturday</td>
                                            <td>
                                                <asp:Label ID="lblsaturday" runat="server" Text="Label"></asp:Label>
                                            </td>
                                      
                                        </tr>
                                          <tr>
                                            <td>7</td>
                                            <td>Sunday</td>
                                            <td>
                                                <asp:Label ID="lblsunday" runat="server" Text="Label"></asp:Label>
                                            </td>
                                         
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
             </div>
             </div>
         </div>
         </div>
</asp:Content>
