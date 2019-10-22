<%@ Page EnableEventValidation="false" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AlertasAPP.aspx.cs" Inherits="Gestion_Web.Formularios.Reportes.AlertasAPP.AlertasAPP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div>
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Compras > Compras</h5>
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
                                        <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion<span class="caret"></span></button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <asp:LinkButton ID="btnCambiarEstado" data-toggle="modal" runat="server" href="#modalConfirmacion">Cambiar estado</asp:LinkButton>
                                                <asp:LinkButton ID="btnAdministrarBotonesAlertas" runat="server" href="ABMBotonesAlertas.aspx">Administrar Botones Alertas</asp:LinkButton>
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
                                    <div class="btn-group pull-right" style="width: 100%">
                                        <button type="button" class="btn btn-primary dropdown-toggle ui-tooltip" title data-original-title="Listados" data-toggle="dropdown">
                                            <i class="shortcut-icon icon-print"></i>&nbsp
                                       
                                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                            <span class="caret"></span>
                                        </button>
                                        <ul class="dropdown-menu dropdown-menu-right" role="menu" aria-labelledby="dropdownMenu">
                                            <li>
                                                <asp:LinkButton ID="btnExportarExcel" runat="server" OnClick="btnExportarExcel_Click">
                                                        <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                        &nbsp Exportar
                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnImprimirPDF" runat="server" OnClick="btnImprimirPDF_Click">
                                                        <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                        &nbsp Imprimir
                                                </asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                </td>

                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">
                                        <a class="btn btn-primary" data-toggle="modal" href="#modalBusqueda" style="width: 100%">
                                            <i class="shortcut-icon icon-filter"></i>
                                        </a>
                                    </div>
                                </td>
                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">
                                        <a href="ComprasABM.aspx?a=1" class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
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
                <div class="widget widget-table">
                    <div class="widget-header">
                        <i class="icon-th-list" style="width: 2%"></i>
                        <h3 style="width: 75%">Compras
                        </h3>
                        <h3>
                            <asp:Label ID="lblSaldo" runat="server" Style="text-align: right" Text="" ForeColor="#666666" Font-Bold="true"></asp:Label>
                        </h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">
                            <div class="table-responsive">
                                <table class="table table-striped table-bordered table-hover" id="tablaAlertas">
                                    <thead>
                                        <tr>
                                            <th style="width:10%">Fecha</th>
                                            <th style="width:10%">Cliente</th>
                                            <th style="width:10%">Vendedor</th>
                                            <th style="width:10%">Tipo Alerta</th>
                                            <th style="width:30%">Mensaje</th>
                                            <th style="width:10%">Estado</th>
                                            <th style="width:10%">Vencimiento</th>
                                            <th style="width:10%"></th>
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
                            <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-md-4">Fecha Alerta</label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaDesde" onchange="javascript:return ComprobacionFechaDesde()" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaHasta" onchange="javascript:return ComprobacionFechaHasta()" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Buscar Cliente</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtCodCliente" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:LinkButton ID="lbtnBuscarCliente" OnClientClick="ObtenerCliente()" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Cliente</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListCliente" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Buscar Vendedor/Distribuidor</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtVendedor" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:LinkButton ID="lbtnBuscarVendedor" OnClientClick="ObtenerVendedor()" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Vendedor/Distribuidor</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListVendedor" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Tipo Alerta</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListTipoAlerta" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Estado</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListEstadoAlerta" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnBuscar" OnClientClick="Filtrar(this)" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalConfirmacion" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" id="btnCerrarModalCambiarEstado" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Confirmacion Cambiar estado</h4>
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
                                        <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea cambiar el estado de la alerta?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="txtMovimiento" Text="0" Style="display: none"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnCambiarEstadoAlertas" Text="Aceptar" class="btn btn-success" OnClientClick="CambiarEstadoAlertas()" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <script src="../../../../Scripts/libs/jquery-1.9.1.min.js"></script>
        <script src="../../../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
        <link href="../../../../css/pages/reports.css" rel="stylesheet">
        <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
        <script src="//cdn.datatables.net/plug-ins/1.10.9/sorting/date-eu.js"></script>
        <script src="../../../../Scripts/plugins/dataTables/custom.tables.js"></script>
        <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />
        <script src="../../../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
        <script src="../../../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
        <script src="../../../../scripts/demo/notifications.js"></script>
        <script src="../../Vendedores/Comisiones/Comisiones.js" type="text/javascript"></script>

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
        </script>

        <script type="text/javascript">

            function CambiarEstadoAlertas() {
                var btnCambiarEstadoAlertas = document.getElementById("MainContent_btnCambiarEstadoAlertas");

                btnCambiarEstadoAlertas.disabled = true;
                btnCambiarEstadoAlertas.value = "Aguarde...";

                var checkedNodes = $('#tablaAlertas').find('input[type="checkbox"]:checked');

                if (checkedNodes.length <= 0) {
                    event.preventDefault();
                    $.msgbox("No hay alertas seleccionadas!", { type: "alert" });
                    btnCambiarEstadoAlertas.disabled = false;
                    btnCambiarEstadoAlertas.value = "Aceptar";
                    document.getElementById('btnCerrarModalCambiarEstado').click();
                    return false;
                }

                var idsAlertas = "";

                for (var i = 0; i < checkedNodes.length; i++) {
                    idsAlertas += checkedNodes[i].id.replace("alerta_", "") + ";";
                }

                $.ajax({
                    type: "POST",
                    url: "AlertasAPP.aspx/CambiarEstadoAlertas",
                    data: '{idsAlertas: "' + idsAlertas + '"}',
                    contentType: "application/json",
                    dataType: 'json',
                    error: function () {
                        $.msgbox("No se pudo cambiar el estado de las alertas!", { type: "error" });
                    },
                    success: OnSuccessCambiarEstado
                });
            }

            function OnSuccessCambiarEstado(response) {
                var btnFiltrar = document.getElementById("MainContent_lbtnBuscar");
                var btnCambiarEstadoAlertas = document.getElementById("MainContent_btnCambiarEstadoAlertas");

                var data = response.d;
                var obj = JSON.parse(data);

                if (obj >= 1)
                    $.msgbox("Estado de alertas cambiadas con exito!", { type: "info" });
                else
                    $.msgbox("Error cambiando estado de alertas!", { type: "error" });

                btnCambiarEstadoAlertas.disabled = false;
                btnCambiarEstadoAlertas.value = "Aceptar";
                document.getElementById('btnCerrarModalCambiarEstado').click();
                setTimeout(Filtrar(btnFiltrar));
            }

            function Filtrar(obj) {
                event.preventDefault();
                var valorTxtFechaDesde = document.getElementById('<%= txtFechaDesde.ClientID %>').value;
                var valorTxtFechaHasta = document.getElementById('<%= txtFechaHasta.ClientID %>').value;
                var valorDropListCliente = document.getElementById('<%= DropListCliente.ClientID %>').value;
                var valorDropListVendedor = document.getElementById('<%= DropListVendedor.ClientID %>').value;
                var valorDropListTipoAlerta = document.getElementById('<%= DropListTipoAlerta.ClientID %>').value;
                var valorDropListEstadoAlerta = document.getElementById('<%= DropListEstadoAlerta.ClientID %>').value;

                var fechaDesde = InvertirDiaPorMes(valorTxtFechaDesde);
                var fechaHasta = InvertirDiaPorMes(valorTxtFechaHasta);
                $(obj).attr('disabled', 'disabled');

                $.ajax({
                    type: "POST",
                    url: "AlertasAPP.aspx/Filtrar",
                    data: '{ fechaDesde: "' + fechaDesde.toUTCString() + '", fechaHasta: "' + fechaHasta.toUTCString() + '", idCliente: "' + valorDropListCliente + '", idVendedor: "' + valorDropListVendedor + '", tipoAlerta: "' + valorDropListTipoAlerta + '", estado: "' + valorDropListEstadoAlerta + '"}',
                    contentType: "application/json",
                    dataType: 'json',
                    error: (error) => {
                        $.msgbox("No se pudo filtrar correctamente!", { type: "error" });
                        $(obj).removeAttr('disabled');
                    }
                    ,
                    success: OnSuccessFiltro
                });
            }

            function OnSuccessFiltro(response) {
                var controlBotonFiltrar = document.getElementById('<%= lbtnBuscar.ClientID %>');

                var data = response.d;
                var obj = JSON.parse(data);

                $("#tablaAlertas").dataTable().fnDestroy();
                $('#tablaAlertas').find("tr:gt(0)").remove();

                for (var i = 0; i < obj.length; i++) {
                    $('#tablaAlertas').append(
                        "<tr>" +
                        "<td> " + obj[i].fecha + "</td>" +
                        "<td> " + obj[i].cliente + "</td>" +
                        "<td> " + obj[i].vendedor + "</td>" +
                        "<td> " + obj[i].tipoAlerta + "</td>" +
                        "<td> " + obj[i].mensaje + "</td>" +
                        "<td> " + obj[i].estado + "</td>" +
                        "<td> " + BarraDeProgreso(parseInt(obj[i].vencimiento)) + "</td>" +
                        "<td> " + CrearBotonesAccion(obj[i].id, obj[i].idCliente) + "</td>" +
                        "</tr> ");
                };

                $('#tablaAlertas').dataTable(
                    {
                        "bFilter": false,
                        "bInfo": false,
                        "bAutoWidth": false,
                        "bStateSave": true,
                        "pageLength": 25,
                        "columnDefs": [
                            { type: 'date-eu', targets: 5 }
                        ]
                    });

                $(controlBotonFiltrar).removeAttr('disabled');

                document.getElementById('btnCerrarModalBusqueda').click();
                document.getElementById('btnCerrarModalCambiarEstado').click();
            }

            function CrearBotonesAccion(idAlerta, idCliente) {
                var accion = "";

                accion += "<span class=\"btn btn-info\" style=\"font-size:7pt;\"><input id='alerta_" + idAlerta + "' type=\"checkbox\"></span> ";
                accion += "<a onclick=\"javascript: return IrAlCliente(" + idCliente + ");\" id=\"btnCliente" + idAlerta + "_" + idCliente + "\" class=\"btn btn-info\" style=\"text-align: right\" autopostback=\"false\"><span class=\"shortcut-icon icon-group\"></span></a>";

                return accion;
            }

            function IrAlCliente(idCliente) {
                window.location.replace("../../../Formularios/Clientes/ClientesABM.aspx?accion=2&id=" + idCliente);
            }

            function BarraDeProgreso(progreso) {
                var barraDeProgreso = "";

                if (progreso <= 70) {
                    barraDeProgreso = "<div class=\"progress\"> <div class=\"progress-bar progress-bar-success\" style=\"width: " + progreso + "% \"></div></div>";
                }
                else if (progreso > 70 && progreso < 90) {
                    barraDeProgreso = "<div class=\"progress\">";
                    barraDeProgreso += "<div class=\"progress-bar progress-bar-success\" style=\"width: 70% \"></div>";
                    barraDeProgreso += "<div class=\"progress-bar progress-bar-warning\" style=\"width: " + (progreso * 20) / 100 + "% \"></div>";
                    barraDeProgreso += "</div>";
                }
                else if (progreso >= 90 && progreso <= 100) {
                    barraDeProgreso = "<div class=\"progress\">";
                    barraDeProgreso += "<div class=\"progress-bar progress-bar-success\" style=\"width: 70% \"></div>";
                    barraDeProgreso += "<div class=\"progress-bar progress-bar-warning\" style=\"width: 20% \"></div>";
                    barraDeProgreso += "<div class=\"progress-bar progress-bar-danger\" style=\"width: " + (progreso * 10) / 100 + "% \"></div>";
                    barraDeProgreso += "</div>";
                }
                else {
                    barraDeProgreso = "<div class=\"progress\">";
                    barraDeProgreso += "<div class=\"progress-bar progress-bar-success\" style=\"width: 70% \"></div>";
                    barraDeProgreso += "<div class=\"progress-bar progress-bar-warning\" style=\"width: 20% \"></div>";
                    barraDeProgreso += "<div class=\"progress-bar progress-bar-danger\" style=\"width: 10% \"></div>";
                    barraDeProgreso += "</div>";
                }

                return barraDeProgreso;
            }

            function ObtenerCliente() {
                event.preventDefault();
                var descripcionCliente = document.getElementById('<%= txtCodCliente.ClientID %>').value;

                $.ajax({
                    type: "POST",
                    url: "AlertasAPP.aspx/ObtenerCliente",
                    data: '{cliente: "' + descripcionCliente + '"  }',
                    contentType: "application/json",
                    dataType: 'json',
                    error: function () {
                        alert("No se pudo obtener el cliente.");
                    },
                    success: OnSuccessObtenerCliente
                });
            };

            function OnSuccessObtenerCliente(response) {
                var controlDropListCliente = document.getElementById('<%= DropListCliente.ClientID %>');

                while (controlDropListCliente.options.length > 0) {
                    controlDropListCliente.remove(0);
                }

                var data = response.d;
                obj = JSON.parse(data);

                for (i = 0; i < obj.length; i++) {
                    option = document.createElement('option');
                    option.value = obj[i].id;
                    option.text = obj[i].alias;

                    controlDropListCliente.add(option);
                }
            }

            function ObtenerVendedor() {
                event.preventDefault();
                var descripcionVendedor = document.getElementById('<%= txtVendedor.ClientID %>').value;

                $.ajax({
                    type: "POST",
                    url: "AlertasAPP.aspx/ObtenerVendedor",
                    data: '{vendedor: "' + descripcionVendedor + '"  }',
                    contentType: "application/json",
                    dataType: 'json',
                    error: function () {
                        alert("No se pudo obtener el vendedor.");
                    },
                    success: OnSuccessObtenerVendedor
                });
            };

            function OnSuccessObtenerVendedor(response) {
                var controlDropListVendedor = document.getElementById('<%= DropListVendedor.ClientID %>');

                while (controlDropListVendedor.options.length > 0) {
                    controlDropListVendedor.remove(0);
                }

                var data = response.d;
                obj = JSON.parse(data);

                for (i = 0; i < obj.length; i++) {
                    option = document.createElement('option');
                    option.value = obj[i].id;
                    option.text = obj[i].nombre;

                    controlDropListVendedor.add(option);
                }
            }

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
        </script>
    </div>
</asp:Content>
