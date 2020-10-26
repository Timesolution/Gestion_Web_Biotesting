<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Soporte.aspx.cs" Inherits="Gestion_Web.Formularios.Herramientas.Soporte" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .faq-number {
            width: 42px;
            height: 42px;
            font-size: 18px;
            font-weight: 600;
            text-align: center;
            line-height: 35px;
            color: #FFF;
            background: #F90;
            border: 3px solid #FFF;
            box-shadow: 1px 1px 3px rgba(0,0,0,.4);
            border-radius: 100px;
            text-shadow: 1px 1px 2px rgba(0,0,0,.4);
        }

        .faq-list {
            padding: 0;
            margin: 3em 0 0;
            list-style: none;
        }

            .faq-list li {
                display: table;
                margin-bottom: 2em;
            }

        .faq-icon {
            display: table-cell;
            padding-right: 1.25em;
            vertical-align: top;
        }

        .faq-text {
            display: table-cell;
            vertical-align: top;
        }
    </style>
    <div class="main">
        <div class="container">
            <div class="row">
                <div class="col-md-8">
                    <div class="widget stacked">
                        <div class="widget-header">
                            <i class="icon-pushpin"></i>
                            <h3>Soporte mediante Ticket</h3>
                        </div>
                        <!-- /widget-header -->
                        <div class="widget-content">
                            <h3>Pasos para informarnos de un problema con el Sistema</h3>
                            <br>
                            <div class="faq-container">
                                <ol class="faq-list">
                                    <li id="faq-1">
                                        <div class="faq-icon">
                                            <div class="faq-number">1</div>
                                        </div>
                                        <div class="faq-text">
                                            <h4>Ir al Sitio de Tickets</h4>
                                            <p>Dirigase al siguiente sitio <a href="https://timesolution.freshdesk.com/" target="_blank">FreshDesk</a> para generar un Ticket.</p>
                                        </div>
                                    </li>

                                    <li id="faq-2">
                                        <div class="faq-icon">
                                            <div class="faq-number">2</div>
                                        </div>
                                        <div class="faq-text">
                                            <h4>Iniciar Sesion</h4>
                                            <p><strong>No es necesario Iniciar Sesion/Registrarse</strong>, pero nos ayudaria a establecer una comunicacion mas estrecha con usted agendando datos de su contacto para reconocer a que empresaa/cliente pertenece.</p>
                                        </div>
                                    </li>

                                    <li id="faq-3">
                                        <div class="faq-icon">
                                            <div class="faq-number">3</div>
                                        </div>
                                        <div class="faq-text">
                                            <h4>Desarrollo del Ticket</h4>
                                            <p>Escriba su correo electronico y por favor haga uso del siguiente <strong>modelo</strong> para el Ticket:</p>
                                            <div class="faq-toc">
                                                <ol>
                                                    <li>1. Asunto del Ticket: Nombre de su empresa + "- Problema con el Sistema"</li>
                                                    <li>2. Detalle del Problema: Diganos el #Numero de Error que le muestra el mensaje del sistema y cuentenos un poco los detalles,por ejemplo que es lo que quiso hacer, el filtro que uso, la opcion que clickeo, etc.</li>
                                                    <li>3. URL: (dentro del cuerpo del mensaje) Copie y pegue la Direccion URL de la barra superior de su navegador del sitio en donde se produjo el inconveniente.</li>
                                                    <li>4. Adjuntar Archivo: En caso de que lo desee, podra mandarnos una captura de pantalla del sitio en donde se produjo su inconveniente sin que se visualize el mensaje de error titulado 'Ups!'.</li>
                                                </ol>
                                            </div>
                                        </div>
                                    </li>

                                    <li id="faq-4">
                                        <div class="faq-icon">
                                            <div class="faq-number">Ex.</div>
                                        </div>
                                        <div class="faq-text">
                                            <h3>Nuevo de Desarrollo</h3>
                                            <p>En caso de que no sea un inconveniente y desee contactarnos por un nuevo desarrollo, siga el siguiente modelo para el mensaje del cuerpo.</p>
                                            <div class="faq-toc">
                                                <ol>
                                                    <li>1. Asunto del Ticket: Nombre de su empresa + "- Peticion de Nuevo Desarollo"</li>
                                                    <li>2. Detalle del Desarrollo: Escribanos en detalle la solicitud de la nueva funcionalidad que requiera. Por ejemplo, un nuevo filtro de Clientes mediante el DNI.</li>
                                                </ol>
                                            </div>
                                        </div>
                                    </li>
                                </ol>
                            </div>
                        </div>
                        <!-- /widget-content -->
                    </div>
                    <!-- /widget -->
                </div>
                <!-- /span8 -->

                <div class="col-md-4">
                    <!-- /widget -->

                    <div class="widget stacked widget-box">

                        <div class="widget-header">
                            <i class="icon-pushpin"></i>
                            <h3>Otro medios de Contacto</h3>
                        </div>
                        <!-- /widget-header -->

                        <div class="widget-content">
                            <p style="text-align: justify">Si necesita <strong style="color: black">atencion urgente</strong>, tambien podra contactarse mediante estos medios siguiendo el mismo modelo de desarrollo del problema planteado en la seccion de la izquierda:</p>
                            <div class="faq-toc" style="text-align: justify">
                                <ol>
                                    <li><strong style="color: black">E-mail</strong>: soporte@timesolution.com.ar</li>
                                    <li><strong style="color: black">WhatsApp</strong>: +54 9 11 3782-0435</li>
                                </ol>
                            </div>
                        </div>
                        <!-- /widget-content -->
                    </div>
                    <!-- /widget -->

                </div>
                <!-- /span4 -->



            </div>
            <!-- /row -->


        </div>
        <div class="container">
            <div class="row">
                <div class="span12">
                    <div class="widget stacked">
                        <div class="widget-header">
                            <i class="icon-th-large"></i>
                            <h3>Herramientas</h3>
                        </div>
                        <!-- /widget-header -->

                        <div class="widget-content">

                            <div class="pricing-header" style="margin-bottom: .65em; font-size: 40px; font-weight: 600; text-align: center;">
                                <h1>Nuestras Herramientas de Atencion Virtual</h1>
                            </div>
                            <!-- /.pricing-header -->
                            <div class="pricing-plans plans-4">

                                <div class="plan-container">
                                    <div class="plan stacked">
                                        <div class="plan-header">
                                            <div class="plan-title">
                                                TeamViewer	        		
                                            </div>
                                            <!-- /plan-title -->
                                            <div class="plan-price">
                                                <span class="note"></span><i class="fas fa-life-ring"></i>
                                            </div>
                                            <!-- /plan-price -->

                                        </div>
                                        <!-- /plan-header -->

                                        <div class="plan-features">
                                            <ul>
                                                <li style="text-align: justify">Este programa es util para conectarnos a su PC de forma remota y poder visualizar lo mismo que usted. Para descargarlo haca click en el enlace debajo.</li>
                                            </ul>
                                        </div>
                                        <!-- /plan-features -->

                                        <div class="plan-actions" style="">
                                            <a href="https://get.teamviewer.com/t7q7ums" class="btn" target="_blank">Descargar</a>
                                        </div>
                                        <!-- /plan-actions -->

                                    </div>
                                </div>
                                <div class="plan-container">
                                    <div class="plan stacked orange">
                                        <div class="plan-header">
                                            <div class="plan-title">
                                                AnyDesk	        		
                                            </div>
                                            <!-- /plan-title -->
                                            <div class="plan-price">
                                                <span class="note"></span><i class="fas fa-life-ring"></i>
                                            </div>
                                            <!-- /plan-price -->

                                        </div>
                                        <!-- /plan-header -->

                                        <div class="plan-features">
                                            <ul>
                                                <li style="text-align: justify">Este es programa es el <strong style="color: black">principal</strong> que utilizamos para conectarnos a su PC de forma remota y poder visualizar lo mismo que usted. Para descargarlo haca click en el enlace debajo.</li>
                                            </ul>
                                        </div>
                                        <!-- /plan-features -->

                                        <div class="plan-actions" style="">
                                            <a href="https://anydesk.com/es" class="btn" target="_blank">Descargar</a>
                                        </div>
                                        <!-- /plan-actions -->

                                    </div>
                                    <!-- /plan -->
                                </div>
                                <!-- /plan-container -->


                                <!-- /pricing-plans -->

                                <div class="clear"></div>
                                <br>
                                <br>
                            </div>
                            <!-- /widget-content -->
                        </div>
                        <!-- /widget -->

                    </div>
                    <!-- /span12 -->


                </div>
                <!-- /row -->

            </div>
        </div>
        
    </div>
    <%--<div class="container">
        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked">

                    <div class="widget-header">
                        <i class="icon-th-large"></i>
                        <h3>Soporte</h3>
                    </div>
                    <!-- /widget-header -->

                    <div class="widget-content">

                        <div class="pricing-header" style="margin-bottom: .65em; font-size: 40px; font-weight: 600; text-align: center;">
                            <h1>Time Solution</h1>

                        </div>
                        <!-- /.pricing-header -->

                        <div class="pricing-plans plans-4">

                            <div class="plan-container">
                                <div class="plan stacked">
                                    <div class="plan-header">
                                        <div class="plan-title">
                                            Soporte        		
                                        </div>
                                        <div class="plan-price">
                                            <span class="note"></span><i class="fas fa-life-ring"></i>
                                        </div>
                                    </div>
                                    <div class="plan-features">
                                        <ul>
                                            <li><strong>Si necesita ayuda ingrese </strong><a href="http://timesolution.freshdesk.com/" target="_blank">aqui</a></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>

                            <div class="plan-container">
                                <div class="plan stacked">
                                    <div class="plan-header">

                                        <div class="plan-title">
                                            Contacto	        		
                                        </div>
                                        <!-- /plan-title -->

                                        <div class="plan-price">
                                            <span class="note"></span><i class="icon-user"></i>
                                        </div>
                                        <!-- /plan-price -->

                                    </div>
                                    <!-- /plan-header -->

                                    <div class="plan-features">
                                        <ul>
                                            <li>WhatsApp: +54 11 3782-0435</li>
                                            <li>Email:<a href="#"> soporte@timesolution.com.ar</a></li>
                                        </ul>
                                    </div>
                                    <!-- /plan-features -->


                                </div>
                                <!-- /plan -->
                            </div>
                            <!-- /plan-container -->

                            <!-- <div class="plan-container">
                                <div class="plan stacked">
                                    <div class="plan-header">

                                        <div class="plan-title">
                                            Dirección	        		
                                        </div> -->
                            <!-- /plan-title -->

                            <!-- <div class="plan-price">
                                            <span class="note"></span><i class="icon-map-marker"></i>
                                        </div> -->
                            <!-- /plan-price -->

                            <!-- </div> -->
                            <!-- /plan-header -->

                            <!-- <div class="plan-features">
                                        <ul>
                                            <li>Avenida del Puerto 215</li>
                                            <li>Piso 3 - Oficina 314</li>
                                            <li>Código Postal 1670</li>
                                            <li>Rincon de Milberg - Tigre  Buenos Aires</li>
                                        </ul>
                                    </div> -->
                            <!-- /plan-features -->


                            <!-- </div> -->
                            <!-- /plan -->
                            <!-- </div> -->
                            <!-- /plan-container -->

                            <!-- <div class="plan-container">
                                <div class="plan stacked">
                                    <div class="plan-header">

                                        <div class="plan-title">
                                            WhatsApp	        		
                                        </div> -->
                            <!-- /plan-title -->

                            <!-- <div class="plan-price">
                                            <span class="note"></span><i class="fab fa-whatsapp"></i>
                                        </div> -->
                            <!-- /plan-price -->

                            <!-- </div> -->
                            <!-- /plan-header -->

                            <!-- <div class="plan-features">
                                        <ul>
                                            <li>Tel: +54 11 3782-0435</li>
                                        </ul>
                                    </div> -->
                            <!-- /plan-features -->


                            <!-- </div> -->
                            <!-- /plan -->
                            <!-- </div> -->
                            <!-- /plan-container -->

                            <div class="plan-container">
                                <div class="plan stacked">
                                    <div class="plan-header">

                                        <div class="plan-title">
                                            TeamViewer	        		
                                        </div>
                                        <!-- /plan-title -->

                                        <div class="plan-price">
                                            <span class="note"></span><i class="fas fa-life-ring"></i>
                                        </div>
                                        <!-- /plan-price -->

                                    </div>
                                    <!-- /plan-header -->

                                    <div class="plan-features">
                                        <ul>
                                            <li><strong>Descargar: </strong><a href="https://get.teamviewer.com/t7q7ums" target="_blank">Aquí</a></li>
                                        </ul>
                                    </div>
                                    <!-- /plan-features -->


                                </div>
                                <!-- /plan -->

                            </div>
                            <!-- /plan-container -->
                            <div class="plan-container">
                                <div class="plan stacked">
                                    <div class="plan-header">
                                        <div class="plan-title">
                                            AnyDesk	        		
                                        </div>
                                        <div class="plan-price">
                                            <span class="note"></span><i class="fas fa-life-ring"></i>
                                        </div>
                                    </div>
                                    <div class="plan-features">
                                        <ul>
                                            <li><strong>Descargar: </strong><a href="https://anydesk.com/es" target="_blank">Aquí</a></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>

                        </div>

                    </div>
                    <!-- /pricing-plans -->

                    <div class="clear"></div>

                    <br />
                    <br />
                    <hr />
                    <br />
                    <br />
                </div>
                <!-- /widget-content -->
            </div>
            <!-- /widget -->
        </div>
        <!-- /span12 -->
    </div>--%>
    <!-- /row -->
    <!-- /container -->
    <link href="https://use.fontawesome.com/releases/v5.0.8/css/all.css" rel="stylesheet">
</asp:Content>
