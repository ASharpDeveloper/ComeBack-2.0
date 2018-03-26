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
             </div>
         </div>
</asp:Content>
