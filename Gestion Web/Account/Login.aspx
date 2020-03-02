<%@ Page Title="Iniciar sesión" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Gestion_Web.Account.Login" Async="true" EnableEventValidation="false"  %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <%--<h2><%: Title %>.</h2>--%>
    

    <div class="account-container stacked">
	
	<div class="content clearfix">
		
		<form action="./index.html" method="post">
		
			<h1>Iniciar Sesión</h1>		
			
			<div class="login-fields">
				
				<p>Ingrese con su usuario y contraseña:</p>
				
				<div class="field">
					<label for="username">Usuario:</label>
					<%--<input type="text" id="username" name="username" value="" placeholder="Username" class="form-control input-lg username-field" />--%>
                    <asp:TextBox ID="txtUsuario" runat="server" name="username" value="" placeholder="Username" class="form-control input-lg username-field"></asp:TextBox>
				</div> <!-- /field -->
				
				<div class="field">
					<label for="password">Contraseña:</label>
					<%--<input type="password" id="password" name="password" value="" placeholder="Password" class="form-control input-lg password-field"/>--%>
                    <asp:TextBox runat="server" ID="txtpassword" TextMode="Password" name="password" value="" placeholder="Password" class="form-control input-lg password-field" />
				</div> <!-- /password -->
				
			</div> <!-- /login-fields -->
			
			<div class="login-actions">
				
				<span class="login-checkbox">
					<%--<input id="Field" name="Field" type="checkbox" class="field login-checkbox" value="First Choice" tabindex="4" />--%>
					<asp:CheckBox ID="Field" runat="server" class="field login-checkbox" tabindex="4" />
                    <label class="choice" for="Field">Let the magic begin</label>
				</span>
                
									
				<asp:Button runat="server" OnClick="LogIn" Text="Iniciar sesión" CssClass="login-action btn btn-primary" />
                <a href="#modalReporteCliente" data-toggle="modal" style="width: 90%">Recuperar Contraseña</a>
			</div> <!-- .actions -->
			
			<%--<div class="login-social">
				<p>Sign in using social network:</p>
				
				<div class="twitter">
					<a href="#" class="btn_1">Login with Twitter</a>				
				</div>
				
				<div class="fb">
					<a href="#" class="btn_2">Login with Facebook</a>				
				</div>
			</div>--%>
			
		</form>
		
	</div> <!-- /content -->
	
</div> <!-- /account-container -->

    <div id="modalReporteCliente" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Recuperar Contraseña</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <%--<asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                            <ContentTemplate>--%>
                                <div class="form-group">
                                    <label class="col-md-4">Escriba mail para recuperar su contraseña</label>
                                    <div class="col-md-6">
                                         <asp:TextBox ID="txtMail" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            <%--</ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>--%>

                    </div>

                    <div class="modal-footer">
                        <asp:LinkButton ID="btnReporteClientes" OnClick="btnReporteClientes_Click" runat="server" Text="Enviar" class="btn btn-success"  />                        
                    </div>
                </div>

            </div>
        </div>
    </div>
        
<!-- Text Under Box -->
<%--<div class="login-extra">
	<%--Don't have an account? <a href="./signup.html">Sign Up</a><br/>
	Recordar <a href="#">Contraseña</a>
</div>--%> <!-- /login-extra -->

   <%-- <!-- Core Scripts - Include with every page -->
  
    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <script src="../../Scripts/libs/bootstrap.min.js"></script>

    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>

    <script src="../../Scripts/Application.js"></script>

    <%--<script src="../../Scripts/plugins/hoverIntent/jquery.hoverIntent.minified.js"></script>--%>

    

   <%-- <script src="../../Scripts/demo/gallery.js"></script>

      <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>--%>
    <%--<script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>--%>


    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
    <%--<script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>--%>

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <script src="../../Scripts/libs/bootstrap.min.js"></script>
        
    <script src="../../Scripts/plugins/hoverIntent/jquery.hoverIntent.minified.js"></script>
    
    <script src="../../Scripts/Application.js"></script>

    <script src="../../Scripts/demo/gallery.js"></script>

    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
    <script src="../../Scripts/demo/notifications.js"></script>



</asp:Content>
