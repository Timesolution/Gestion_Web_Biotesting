<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HistorialSMS.aspx.cs" Inherits="Gestion_Web.Formularios.Herramientas.HistorialSMS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div class="container">
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Valores > Mayor</h5>
                    </div>

                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>

                    <div class="widget-content">
                        <div class="shortcuts col-lg-10"></div>
                        <div class="shortcuts col-lg-1">
                            <a class="btn btn-primary" data-toggle="modal" data-placement="top" title="Tooltip on top" href="#modalBusqueda" style="width: 100%">
                                <i class="shortcut-icon icon-filter"></i>
                            </a>
                        </div>
                    </div>
                </div>
                <div class="widget stacked widget-table action-table">
                    <div class="widget-header">
                        <i class="icon-bookmark"></i>
                        <h3>SMS</h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">
                            <div class="table-responsive">
                                <table class="table table-striped table-bordered table-sm" id="tablaSMS" style="width: 100%">
                                    <thead>
                                        <tr>
                                            <th>Fecha</th>
                                            <th>Alias Cliente</th>
                                            <th>Titulo</th>
                                            <th>Mensaje</th>
                                            <th>Celular</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phMotivos" runat="server"></asp:PlaceHolder>
                                    </tbody>
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
                                    <asp:TextBox ID="txtFechaDesde" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Hasta</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtFechaHasta" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnFiltrar" href="#" OnClientClick="Filtrar()" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" AutoPostBack="false" />
                    </div>
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
        function pageLoad() {
            var fechaActual = ObtenerFechaActual();
            controlTxtFechaDesde = document.getElementById('<%= txtFechaDesde.ClientID %>');
            controlTxtFechaHasta = document.getElementById('<%= txtFechaHasta.ClientID %>');
            controlTxtFechaDesde.value = fechaActual;
            controlTxtFechaHasta.value = fechaActual;

            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        }

        function Filtrar(obj) {
            var valorTxtFechaDesde = controlTxtFechaDesde.value;
            var valorTxtFechaHasta = controlTxtFechaHasta.value;
            var fechaDesde = InvertirDiaPorMes(valorTxtFechaDesde);
            var fechaHasta = InvertirDiaPorMes(valorTxtFechaHasta);

            $(obj).attr('disabled', 'disabled');

            $.ajax({
                type: "POST",
                url: "HistorialSMS.aspx/Filtrar",
                data: '{ fechaDesde: "' + fechaDesde.toUTCString() + '", fechaHasta: "' + fechaHasta.toUTCString() + '" }',
                contentType: "application/json",
                dataType: 'json',
                error: (error) => {
                    console.log(JSON.stringify(error));
                    $.msgbox("No se pudo filtrar correctamente!", { type: "error" });
                    $(obj).removeAttr('disabled');
                }
                ,
                success: OnSuccessFiltro
            });
        }

        function OnSuccessFiltro(response) {
            var controlBotonFiltrar = document.getElementById('<%= lbtnFiltrar.ClientID %>');

            var data = response.d;
            var obj = JSON.parse(data);

            document.getElementById('btnCerrarModalBusqueda').click();

            for (var i = 0; i < obj.length; i++) {
                $('#tablaMayor').append(
                    "<tr>" +
                    "<td> " + obj[i].Fecha + "</td>" +
                    "<td> " + obj[i].TipoMovimiento + "</td>" +
                    "<td> " + obj[i].NumeroDocumento + "</td>" +
                    '<td style="text-align:right">' + obj[i].Debito + "</td>" +
                    '<td style="text-align:right">' + obj[i].Credito + "</td>" +
                    "<td> " + obj[i].Empresa + "</td>" +
                    "<td> " + obj[i].Sucursal + "</td>" +
                    '<td style="text-align:right">' + obj[i].PuntoDeVenta + "</td>" +
                    "<td> " + obj[i].Nivel1 + "</td>" +
                    "<td> " + obj[i].Nivel2 + "</td>" +
                    "<td> " + obj[i].Nivel3 + "</td>" +
                    "<td> " + obj[i].Nivel4 + "</td>" +
                    "<td> " + obj[i].Nivel5 + "</td>" +
                    "</tr> ");
            };
            $(controlBotonFiltrar).removeAttr('disabled');
        }
    </script>
</asp:Content>


