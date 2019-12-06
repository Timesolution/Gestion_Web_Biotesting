<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NotificacionesF.aspx.cs" Inherits="Gestion_Web.Formularios.Herramientas.NotificacionesF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <div class="col-md-12 col-xs-12">
            <div class="widget stacked">
                <div class="stat">
                    <h5><i class="icon-map-marker"></i>Herramientas > Notificaciones</h5>
                </div>
                <div class="widget-header">
                    <i class="icon-wrench"></i>
                    <h3>Herramientas</h3>
                </div>
                <div class="widget-content">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 98%">
                            </td>
                            <td>
                                <div class="shortcuts" style="height: 100%">
                                    <a class="btn btn-primary" data-toggle="modal" data-placement="top" title="Tooltip on top" href="#modalBusqueda" style="width: 100%">
                                        <i class="shortcut-icon icon-filter"></i>
                                    </a>
                                </div>
                            </td>
                            <td>
                                <div class="shortcuts" style="height: 100%">
                                    <a class="btn btn-primary" href="NotificacionesABM.aspx" style="width: 100%">
                                        <i class="shortcut-icon icon-plus"></i>
                                    </a>
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
                    <i class="icon-money" style="width: 2%"></i>
                    <h3 style="width: 75%">Notificaciones</h3>
                </div>
                <div class="widget-content">
                    <div class="panel-body">
                        <div class="table-responsive">
                            <table class="table table-striped table-bordered table-hover" id="tablaNotificaciones">
                                <thead>
                                    <tr>
                                        <th style="width: 5%">Fecha</th>
                                        <th style="width: 5%">Campaña</th>
                                        <th style="width: 5%">Titulo</th>
                                        <th style="width: 15%">Mensaje</th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modalBusqueda" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnCerrarModalBusqueda" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Busqueda</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <div class="form-group">
                            <label class="col-md-4">Desde</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtFechaDesde" onchange="javascript:return ComprobacionFechaDesde()" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Hasta</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtFechaHasta" onchange="javascript:return ComprobacionFechaHasta()" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnBuscar" href="#" OnClientClick="Filtrar(this)" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success"/>
                </div>
            </div>
        </div>
    </div>

    <script src="../../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <link href="../../../css/pages/reports.css" rel="stylesheet">
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="//cdn.datatables.net/plug-ins/1.10.9/sorting/date-eu.js"></script>
    <script src="../../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />
    <script src="../../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
    <script src="../../../scripts/demo/notifications.js"></script>
    <script src="../Vendedores/Comisiones/Comisiones.js" type="text/javascript"></script>

    <script>
        $(function () {
            var fechaActual = ObtenerFechaActual();

            var controlFechaHasta = document.getElementById('<%= txtFechaHasta.ClientID %>');
            var controlFechaDesde = document.getElementById('<%= txtFechaDesde.ClientID %>');

            controlFechaDesde.value = fechaActual;
            controlFechaHasta.value = fechaActual;

            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        function ComprobacionFechaHasta() {
            var fechaActual = ObtenerFechaActual();

            var controlFechaHasta = document.getElementById('<%= txtFechaHasta.ClientID %>');
            var controlFechaDesde = document.getElementById('<%= txtFechaDesde.ClientID %>');

            var pattern = /^([0-9]{2})\/([0-9]{2})\/([0-9]{4})$/;

            //si es invalida
            if (!pattern.test(controlFechaHasta.value)) {
                controlFechaHasta.value = fechaActual;
                return false;
            }

            //si hasta es mas chico que desde
            var desde = InvertirDiaPorMes(controlFechaDesde.value);
            var hasta = InvertirDiaPorMes(controlFechaHasta.value);
            var hoy = InvertirDiaPorMes(fechaActual);
            if (desde > hasta) {
                controlFechaHasta.value = fechaActual;
                controlFechaDesde.value = fechaActual;
                return false;
            }

            //si hasta es mas grande que hoy
            if (hasta > hoy) {
                controlFechaHasta.value = fechaActual;
                return false;
            }
        };

        function ComprobacionFechaDesde() {
            var fechaActual = ObtenerFechaActual();

            var controlFechaHasta = document.getElementById('<%= txtFechaHasta.ClientID %>');
            var controlFechaDesde = document.getElementById('<%= txtFechaDesde.ClientID %>');

            var pattern = /^([0-9]{2})\/([0-9]{2})\/([0-9]{4})$/;

            //si es invalida
            if (!pattern.test(controlFechaDesde.value)) {
                controlFechaDesde.value = fechaActual;
                return false;
            }

            var desde = InvertirDiaPorMes(controlFechaDesde.value);
            var hasta = InvertirDiaPorMes(controlFechaHasta.value);
            //si es mas grande que la fecha hasta
            if (desde > hasta) {
                controlFechaDesde.value = fechaActual;
                controlFechaHasta.value = fechaActual;
                return false;
            }
        };
        function Filtrar(obj)
        {
            var valorTxtFechaDesde = document.getElementById('<%= txtFechaDesde.ClientID %>').value;
            var valorTxtFechaHasta = document.getElementById('<%= txtFechaHasta.ClientID %>').value;

            var fechaDesde = InvertirDiaPorMes(valorTxtFechaDesde);
            var fechaHasta = InvertirDiaPorMes(valorTxtFechaHasta);
            $(obj).attr('disabled', 'disabled');

            $.ajax({
                type: "POST",
                url: "NotificacionesF.aspx/Filtrar",
                data: '{ fechaDesde: "' + fechaDesde.toUTCString() + '", fechaHasta: "' + fechaHasta.toUTCString() + '" }',
                contentType: "application/json",
                dataType: 'json',
                error: (error) =>
                {
                    $.msgbox("No se pudo filtrar correctamente!", {type: "error"});
                    $(obj).removeAttr('disabled');
                }
                ,
                success: OnSuccessFiltro
            });            
        }

        function OnSuccessFiltro(response)
        {
            var data = response.d;
            var obj = JSON.parse(data);

            document.getElementById('btnCerrarModalBusqueda').click();
            $("#tablaNotificaciones").dataTable().fnDestroy();
            $('#tablaNotificaciones').find("tr:gt(0)").remove();

            var totalNeto = 0;
            var total = 0;

            for (var i = 0; i < obj.length; i++) {
                $('#tablaNotificaciones').append(
                    "<tr>" +
                    "<td> " + obj[i].Fecha + "</td>" +
                    "<td> " + obj[i].Campania + "</td>" +
                    "<td> " + obj[i].Titulo + "</td>" +
                    "<td> " + obj[i].Contenido + "</td>" +
                    "</tr> ");
            };

            $(controlBotonFiltrar).removeAttr('disabled');
        }
    </script>

</asp:Content>
