<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ServicioTecnicoF.aspx.cs" Inherits="Gestion_Web.Formularios.OrdenReparacion.ServicioTecnicoF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12 col-xs-12">
        <div class="widget stacked">
            <div class="stat">
                <h5><i class="icon-map-marker"></i>Maestros > Orden de Reparacion > Servicio Tecnico</h5>
            </div>
            <div class="widget-header">
                <i class="icon-wrench"></i>
                <h3>Herramientas</h3>
            </div>
            <div class="widget-content">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 15%">
                            <div class="btn-group">
                                <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" visible="false" runat="server">Accion<span class="caret"></span></button>
                                <ul class="dropdown-menu">
                                </ul>
                            </div>
                        </td>
                        <td style="width: 30%">
                            <div class="col-lg-6">
                                <div class="input-group">
                                    <asp:TextBox ID="txtBusqueda" runat="server" class="form-control" placeholder="Buscar Servicio Tecnico"></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="lbBuscar" runat="server" Text="Buscar" class="btn btn-primary" OnClick="lbBuscar_Click">
                                                     <i class="shortcut-icon icon-search"></i>
                                        </asp:LinkButton>
                                    </span>
                                </div>
                            </div>
                        </td>
                        <td style="width: 40%"></td>
                        <td style="width: 5%"></td>
                        <td style="width: 30%">
                            <div class="col-lg-6">
                                <div class="input-group">
                                    <a runat="server" class="btn btn-primary" href="ServicioTecnicoABM.aspx">
                                        <i class="shortcut-icon icon-plus"></i>
                                    </a>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <div class="col-md-12 col-xs-12">
        <div class="widget stacked widget-table action-table">

            <div class="widget-header">
                <i class="icon-th-list"></i>
                <h3>Servicios Tecnicos</h3>
            </div>
            <div class="widget-content">
                <div class="panel-body">
                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                    <thead>
                                        <tr>
                                            <th>Nombre</th>
                                            <th>Direccion</th>
                                            <th>Telefono</th>
                                            <th>Observaciones</th>
                                            <th>Cliente</th>
                                            <th>Marcas</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phServicioTecnico" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div id="modalConfirmacion" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Confirmacion de Eliminacion</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <div class="form-group">
                            <div class="col-md-2">
                                <h1>
                                    <i class="icon-warning-sign" style="color: orange"></i>
                                </h1>
                            </div>
                            <div class="col-md-7">
                                <h5>
                                    <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar el servicio tecnico?" Style="text-align: center"></asp:Label>
                                </h5>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="txtMovimiento" Text="0" Style="display: none"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnSi" Text="Eliminar" class="btn btn-danger" OnClick="btnSi_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>

    <script>
        function abrirdialog(valor) {
            document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
        }
    </script>
    <script>
        //valida los campos solo numeros
        function validarNro(e) {
            var key;
            if (window.event) // IE
            {
                key = e.keyCode;
            }
            else if (e.which) // Netscape/Firefox/Opera
            {
                key = e.which;
            }

            if (key < 48 || key > 57) {
                if (key == 46 || key == 8) // Detectar . (punto) y backspace (retroceso)
                { return true; }
                else
                { return false; }
            }
            return true;
        }
    </script>

    <script>
        $(function () {

            $('.accordion').on('show', function (e) {
                $(e.target).prev('.accordion-heading').addClass('accordion-opened');
            });

            $('.accordion').on('hide', function (e) {
                $(this).find('.accordion-heading').not($(e.target)).removeClass('accordion-opened');
            });

        });
    </script>

    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>

    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>

    <%--<script src="../../Scripts/Application.js"></script>--%>

    <script src="../../Scripts/demo/notifications.js"></script>

</asp:Content>
