﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RetencionesF.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.RetencionesF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div class="container">

            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">

                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Ventas > Retenciones</h5>
                    </div>

                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>

                    <div class="widget-content">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 20%">
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion    <span class="caret"></span></button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <%--<asp:LinkButton ID="lbtnExportar" runat="server" OnClick="lbtnExportar_Click">Exportar reporte</asp:LinkButton>--%>
                                            </li>
                                        </ul>
                                    </div>
                                </td>
                                <td style="width: 65%">
                                    <h5>
                                        <asp:Label runat="server" ID="lblParametros" Text="" ForeColor="#cccccc"></asp:Label>
                                    </h5>
                                </td>
                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">

                                        <div class="shortcuts" style="height: 100%">
                                            <asp:LinkButton ID="lbImpresion" class="btn btn-primary" runat="server" Text="<span class='shortcut-icon icon-print'></span>" Visible="false" Style="width: 100%" />
                                        </div>
                                    </div>
                                </td>

                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">
                                        <a class="btn btn-primary" data-toggle="modal" href="#modalBusqueda" style="width: 100%">
                                            <i class="shortcut-icon icon-filter"></i>
                                        </a>
                                    </div>
                                </td>
                            </tr>

                        </table>
                    </div>
                </div>
            </div>

            <div class="col-md-12">
                <div class="widget big-stats-container stacked">
                    <div class="widget-content">
                        <div id="big_stats" class="cf">
                            <div class="stat">
                                <h4>Monto total percibido</h4>
                                <asp:Label ID="labelSaldo" runat="server" class="value"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-12 col-xs-12">
                <div class="widget widget-table">
                    <div class="widget-header">
                        <i class="icon-th-list" style="width: 2%"></i>
                        <h3 style="width: 75%">Retenciones</h3>
                        <h3>
                            <asp:Label ID="lblSaldo" runat="server" Style="text-align: right" Text="" ForeColor="#666666" Font-Bold="true"></asp:Label>
                        </h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">
                            <div class="table-responsive">
                                <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalFacturaDetalle">Agregar Tipo Cliente</a>
                                <table class="table table-striped table-bordered table-hover" id="tabla_IngresosBrutos">
                                    <thead>
                                        <tr>
                                            <th>Fecha</th>
                                            <th>Jurisdiccion</th>
                                            <th>Razon Social</th>
                                            <th>CUIT</th>
                                            <th>Nº Factura</th>
                                            <th>Neto</th>
                                            <th>Percepcion %</th>
                                            <th>Monto Percibido</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phFacturas" runat="server"></asp:PlaceHolder>
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
                        <button id="btnCerrarModalBusqueda" type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Busqueda</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-4">Desde</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtFechaDesde" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Hasta</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtFechaHasta" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Sucursal</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Provincia</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="DropListProvincias" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListProvincias" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnBuscar" href="#" OnClientClick="ObtenerRegistrosYLLenarTablaIIBB(this);" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" AutoPostBack="false" />
                    </div>
                </div>
            </div>
        </div>

        <script>

            function abrirdialog() {
                document.getElementById('abreDialog').click();
            }

        </script>
        <link href="../../css/pages/reports.css" rel="stylesheet">
        <script src="../../Scripts/plugins/dataTables/jquery-2.0.0.js"></script>
        <script src="../../Scripts/plugins/dataTables/jquery.dataTables.min.js"></script>
        <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>


        <!-- Core Scripts - Include with every page -->
        <script src="../../Scripts/jquery-1.10.2.js"></script>
        <script src="../../Scripts/bootstrap.min.js"></script>
        <script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>

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
        <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>
        <script src="../../js/Funciones.js"></script>

        <script>
            $(function () {
                $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

            $(function () {
                $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });
        </script>

        <!-- Page-Level Plugin Scripts - Tables -->
        <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
        <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
        <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

        <script>
            function pageLoad() {
                var fechaActual = ObtenerFechaActual();
                controlTxtFechaDesde = document.getElementById('<%= txtFechaDesde.ClientID %>');
                controlTxtFechaHasta = document.getElementById('<%= txtFechaHasta.ClientID %>');
                controlTxtFechaDesde.value = fechaActual;
                controlTxtFechaHasta.value = fechaActual;
            }

            function ObtenerRegistrosYLLenarTablaIIBB(obj) {
                var fechaDesde = document.getElementById('<%= txtFechaDesde.ClientID %>');
                var fechaHasta = document.getElementById('<%= txtFechaHasta.ClientID %>');
                var provinciaSeleccionada = document.getElementById('<%= DropListProvincias.ClientID %>');
                var sucursalSeleccionada = document.getElementById('<%= DropListSucursal.ClientID %>');

                $(obj).attr('disabled', 'disabled');

                $.ajax({
                    type: "POST",
                    url: "RetencionesF.aspx/TraerRegistrosDe_CuentasContables_MayorTipoMovimiento",
                    data: '{ FechaDesde: "' + fechaDesde.value + '", FechaHasta: "' + fechaHasta.value + '", Provincia: "' + provinciaSeleccionada.value + '", Sucursal: "' + sucursalSeleccionada.value + '"}',
                    contentType: "application/json",
                    dataType: 'json',
                    error: (error) => {
                        console.log(JSON.stringify(error));
                        $.msgbox("No se pudo filtrar !", { type: "error" });
                    }
                    ,
                    success: CargarTabla
                });
                return false;
            }

            function CargarTabla(response) {
                var controlBotonFiltrar = document.getElementById('<%= lbtnBuscar.ClientID %>');

                var data = response.d;
                var obj = JSON.parse(data);

                document.getElementById('btnCerrarModalBusqueda').click();

                if (obj == "-2") {
                    $.msgbox("Provincia ya existente", { type: "error" });
                    return false;
                }

                if (obj.length == 0) {
                    $.msgbox("No hay registros..", { type: "alert" });
                }

                $('#tabla_IngresosBrutos').find("tr:gt(0)").remove();

                var montoPercibidoTotal = 0;
                for (var i = 0; i < obj.length; i++) {
                    montoPercibidoTotal += parseFloat(obj[i].MontoPercepcion);

                    $('#tabla_IngresosBrutos').append(
                        "<tr>" +
                        "<td> " + obj[i].Fecha + "</td>" +
                        "<td> " + obj[i].Provincia + "</td>" +
                        "<td> " + obj[i].RazonSocial + "</td>" +
                        '<td style="text-align:right">' + obj[i].CUIT + "</td>" +
                        '<td style="text-align:right">' + obj[i].Factura + "</td>" +
                        '<td style="text-align:right">$ ' + obj[i].Neto + "</td>" +
                        '<td style="text-align:right">' + obj[i].Percepcion + "%</td>" +
                        '<td style="text-align:right">$ ' + obj[i].MontoPercepcion + "</td>" +
                        "</tr> ");
                };
                var label = document.getElementById('<%=labelSaldo.ClientID%>');
                label.textContent = "$ " + parseFloat(montoPercibidoTotal).toFixed(2);
                $(controlBotonFiltrar).removeAttr('disabled');
            }
        </script>


    </div>
</asp:Content>