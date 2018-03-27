<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="addprogress.aspx.cs" Inherits="ComeBack_2._0.addprogress" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="content-wrapper">
         <div class="container">
        <div class="row pad-botm">
            <div class="col-md-12">
                <h4 class="header-line">Progress Pictures</h4>
                
                            </div>

        </div>
             <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-12">
               <div class="panel panel-info">
                        <div class="panel-heading">
                           Log your progress
                        </div>
                        <div class="panel-body">
                            <div role="form">
                                        <div class="form-group">
                                            <label>Upload an image:</label>
                                            <asp:FileUpload ID="FileInput" runat="server" />
                                           
                                        </div>
                                 
                                  <div class="form-group">
                                           
                                            <asp:Button ID="cmdUpload" Text="Upload File" runat="server" onClick="cmdUpload_Click" />
                                            
                              
                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-info" Text="Add Image" OnClick="btnSubmit_Click"/>
                                       <asp:HiddenField ID="lblfilename" runat="server" />
                                    </div>
                            </div>
                        </div>
                            </div>
                 </div>
                 </div>
             </div>
         </div>
</asp:Content>
