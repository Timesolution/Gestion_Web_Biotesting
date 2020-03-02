<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Error404.aspx.cs" Inherits="Gestion_Web.Error404" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div class="container">
	
	<div class="row">
		
		<div class="col-md-12">
			
			<div class="error-container">
				<h1>Oops!</h1>
				
				<h2>404 Not Found</h2>
				
				<div class="error-details">
					Ah acurrido un error en la Aplicacion
					
				</div> <!-- /error-details -->
				
				<div class="error-actions">
					<a href="/Default.aspx" class="btn btn-primary btn-lg">
						<i class="icon-chevron-left"></i>
						&nbsp;
						Volver a Inicio						
					</a>
					
					<%--<a href="./faq.html" class="btn btn-default btn-lg">
						<i class="icon-envelope"></i>
						&nbsp;
						Contact Support						
					</a>--%>
					
				</div> <!-- /error-actions -->
							
			</div> <!-- /error-container -->			
			
		</div> <!-- /span12 -->
		
	</div> <!-- /row -->
	
</div> <!-- /container -->
    </div>
</asp:Content>
