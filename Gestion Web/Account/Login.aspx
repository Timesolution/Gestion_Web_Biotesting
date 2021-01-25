<%@ Page Title="Iniciar sesión" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Gestion_Web.Account.Login" Async="true" EnableEventValidation="false" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <%--<h2><%: Title %>.</h2>--%>
    <style>
        .footer {
            position: fixed;
            left: 0;
            bottom: 0;
            width: 100%;
        }

        .lbl-neon {
            position: relative;
            display: inline-block;
            padding: 25px 0px;
            color: #505A57;
            letter-spacing: 2px;
            font-size: 12px;
            overflow: hidden;
        }

        .btn-iniciar {
            position: relative;
            font-size: 15px;
            overflow: hidden;
            border: none;
        }

            .btn-iniciar:hover {
                box-shadow: inset 0 0 10px #D7A938;
                outline-color: #F9C33F;
                outline-offset: 80px;
                text-shadow: 1px 1px 6px #FFFFFF;
            }
    </style>

    <script type="text/javascript">

        function validar2(result) {
            $.ajax({
                type: "POST",
                url: "Login.aspx/ReporteClientes_Click",
                data: '{email: "' + result + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    $.msgGrowl({
                        type: 'error'
                        , title: 'Error'
                        , text: 'Disculpe, a ocurrido un error. Contacte con el area de soporte.'
                    });
                }

            });
        }

        function OnSuccess(response) {
            if (response.d > 0) {
                $.msgGrowl({
                    type: 'success'
                    , title: 'Exito'
                    , text: 'E-mail enviado con exito. Revise su casilla por favor.'
                });
            }
            else {
                $.msgGrowl({
                    type: 'error'
                    , title: 'Error'
                    , text: 'Disculpe, ha ocurrido un error. Contacte con el area de soporte.'
                });
            }
        }

        function validar() {
            $.msgbox("Inserte una direccion de e-mail:", {
                type: "prompt"
            }, function (result) {
                if (result) {
                    $(validar2(result));
                }
            });
        }
    </script>
    <div class="account-container stacked">
        <div class="content clearfix">
            <form action="./index.html" method="post">
                <h1>Iniciar Sesión</h1>
                <div class="login-fields">
                    <p>Ingrese con su usuario y contraseña:</p>
                    <div class="field">
                        <label for="username">Usuario:</label>
                        <%--<input type="text" id="username" name="username" value="" placeholder="Username" class="form-control input-lg username-field" />--%>
                        <asp:TextBox ID="txtUsuario" runat="server" name="username" value="" placeholder="Username" CssClass="form-control input-lg username-field"></asp:TextBox>
                    </div>
                    <!-- /field -->
                    <div class="field">
                        <label for="password">Contraseña:</label>
                        <%--<input type="password" id="password" name="password" value="" placeholder="Password" class="form-control input-lg password-field"/>--%>
                        <asp:TextBox runat="server" ID="txtpassword" TextMode="Password" name="password" value="" placeholder="Password" class="form-control input-lg password-field" />
                    </div>
                    <!-- /password -->
                </div>
                <div class="login-actions">

                    <label class="lbl-neon">
                        Let the magic begin
                    </label>


                    <%-- <span class="login-checkbox">
                        <input id="Field" name="Field" type="checkbox" class="field login-checkbox" value="First Choice" tabindex="4" />
                    </span>--%>
                    <asp:Button runat="server" ID="btnIniciarSesion" OnClientClick="this.disabled = true; this.value = 'Iniciando...';" OnClick="LogIn" Text="Iniciar sesión" CssClass="login-action btn btn-primary btn-iniciar" UseSubmitBehavior="false" />
                    <div id="status">
                        <!--<img src="../images/animated-overlay.gif"-->
                    </div>
                </div>
                <div >
                        <div class="more-ot-alert" style="display:block;position:absolute;margin-left:400px;width:220px;bottom:0px;">
                            <p style="font-size: 12px">
                                Necesita ayuda? Haga click <strong><a href="../Formularios/Herramientas/Soporte.aspx">aqui</a></strong>
                            </p>
                        </div>
                </div>
                <asp:CheckBox ID="Field" runat="server" class="field login-checkbox" TabIndex="4" Visible="false" />
                <!-- /login-fields -->
                <!-- .actions -->

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
        </div>
        <!-- /content -->
    </div>
    <!-- /account-container -->
    <div class="login-extra">
        Olvido su <a href="#" id="rc" data-toggle="modal" onclick="validar()">Contraseña</a> ?
    </div>
    
        


    <%--<div id="modalReporteCliente" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Recuperar Contraseña</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <div class="form-group">
                            <label class="col-md-4">Escriba mail para recuperar su contraseña</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtMail" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnReporteClientes" OnClick="btnReporteClientes_Click" runat="server" Text="Enviar" class="btn btn-success" />
                    </div>
                </div>
            </div>
        </div>
    </div>--%>

    <!-- Text Under Box -->
    <%--<div class="login-extra">
	<%--Don't have an account? <a href="./signup.html">Sign Up</a><br/>
	Recordar <a href="#">Contraseña</a>
</div>--%>
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
    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
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
    <script type="text/javascript">
        window.onload = function () {
            openAlert();
        };
        function closeAlert() {
            setTimeout(function () {
                $(".more-ot-alert").fadeOut("fast");
            }, 100);
        }
        function openAlert() {
            $(".more-ot-alert").fadeIn("fast");
            // IE8 animation polyfill
            if ($("html").hasClass("lt-ie9")) {
                var speed = 400;
                var times = Infinity;
                var loop = setInterval(anim, Infinity);
                function anim() {
                    times--;
                    if (times === 0) { clearInterval(loop); }
                    $(".more-ot-alert").animate({ left: 450 }, speed).animate({ left: 440 }, speed);
                    //.stop( true, true ).fadeIn();
                };
                anim();
            };
        }
        $(".close-ot-alert").on("click", function () {
            closeAlert()
        });

        $(".open-ot-alert").on("click", function () {
            openAlert();
        });


        //$(document).(function (e) {
        //    if (e.keyCode == 27) { closeAlert(); }
        //    if (e.keyCode == 67) { openAlert(); } // C is for click?
        //});
    </script>
</asp:Content>
