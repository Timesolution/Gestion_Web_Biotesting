<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MayorF.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.MayorF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label runat="server" Style="display: none" ID="lbUsuario"></asp:Label>

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
                    <a class="btn btn-primary" data-toggle="modal" data-placement="top" title="Tooltip on top" href="#modalAgregarRegistro" style="width: 100%">
                        <i class="shortcut-icon icon-plus"></i>
                    </a>
                </div>
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
                <h3>Mayor</h3>
            </div>

            <div class="widget-content">

                <div class="panel-body">
                    <div class="table-responsive">
                        <table class="table table-striped table-bordered table-sm" id="tablaMayor" style="width: 100%">
                            <thead>
                                <tr>
                                    <th>Fecha</th>
                                    <th>Origen</th>
                                    <th>Numero de Documento</th>
                                    <th>Item</th>
                                    <th>Asiento</th>
                                    <th style="text-align: right">Debito</th>
                                    <th style="text-align: right">Credito</th>
                                    <th>Empresa</th>
                                    <th>Sucursal</th>
                                    <th style="text-align: right">Punto de Venta</th>
                                    <th>Nivel 1</th>
                                    <th>Nivel 2</th>
                                    <th>Nivel 3</th>
                                    <th>Nivel 4</th>
                                </tr>
                            </thead>
                        </table>
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
                            <div class="form-group">
                                <label class="col-md-4">Empresa</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="DropListEmpresa" runat="server" class="form-control" AutoPostBack="false"></asp:DropDownList>
                                    <asp:Label runat="server" ID="lbDropListEmpresaError" Style="display: none" Text="Seleccione un item." ForeColor="Red" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Sucursal</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Punto Venta</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="DropListPuntoVenta" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Tipo de Movimiento</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="DropListTipoDeMovimiento" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                            </div>
                             <div class="form-group">
                                <label class="col-md-4">Nivel 1</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="dropListNivel1_ModalBusqueda" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Nivel 2</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="dropListNivel2_ModalBusqueda" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Nivel 3</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="dropListNivel3_ModalBusqueda" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Nivel 4</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="dropListNivel4_ModalBusqueda" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnBuscar" href="#" OnClientClick="ValidarFormulario(this)" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" AutoPostBack="false" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalAgregarRegistro" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" id="btnCerrar_ModalAgregarRegistro" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Busqueda</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-4">Fecha</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtFecha_ModalAgregarRegistro" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Empresa</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="DropListEmpresa_ModalAgregarRegistro" runat="server" class="form-control" AutoPostBack="false"></asp:DropDownList>
                                    <asp:Label runat="server" ID="lbErrorDropListEmpresa_ModalAgregarRegistro" Style="display: none" Text="Seleccione un item." ForeColor="Red" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Sucursal</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="DropListSucursal_ModalAgregarRegistro" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Punto Venta</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="dropListPuntoVenta_ModalAgregarRegistro" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Tipo de Item</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="dropList_Asientos_Item_ModalAgregarRegistro" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Tipo Movimiento</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="dropList_Mayor_TipoMovimiento" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Nivel 1</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="DropListNivel1_ModalAgregarRegistro" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Nivel 2</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="DropListNivel2_ModalAgregarRegistro" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Nivel 3</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="DropListNivel3_ModalAgregarRegistro" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Nivel 4</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="DropListNivel4_ModalAgregarRegistro" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Tipo Operacion</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="dropList_TipoOperacion_ModalAgregarRegistro" runat="server" class="form-control">
                                        <asp:ListItem Value="1">Ingreso</asp:ListItem>
                                        <asp:ListItem Value="2">Egreso</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Importe</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtImporte_ModalAgregarRegistro" onkeypress="javascript:return ValidarSoloNumeros(event)" runat="server" class="form-control"></asp:TextBox>
                                    <asp:Label runat="server" ID="lb_ImporteError_ModalAgregarRegistro" Style="display: none" Text="Ingrese un importe." ForeColor="Red" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnCrearRegistroManual" OnClick="lbtnCrearRegistroManual_Click" OnClientClick="javascript:return ValidarFormulario_ModalCrearRegistroManual()" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" AutoPostBack="false" />
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

    <%-- <style>
        .dtHorizontalVerticalExampleWrapper {
            max-width: 600px;
            margin: 0 auto;
        }

        #tablaMayor th, td {
            white-space: nowrap;
        }

        table.dataTable thead .sorting:after,
        table.dataTable thead .sorting:before,
        table.dataTable thead .sorting_asc:after,
        table.dataTable thead .sorting_asc:before,
        table.dataTable thead .sorting_asc_disabled:after,
        table.dataTable thead .sorting_asc_disabled:before,
        table.dataTable thead .sorting_desc:after,
        table.dataTable thead .sorting_desc:before,
        table.dataTable thead .sorting_desc_disabled:after,
        table.dataTable thead .sorting_desc_disabled:before {
            bottom: .5em;
        }
    </style>--%>

    <script>
        var controlDropListEmpresa;
        var controlDropListSucursal;
        var controlDropListPuntoVenta;
        var controlDropListTipoDeMovimiento;
        var controlLabelEmpresaError;
        var controlTxtFechaDesde;
        var controlTxtFechaHasta;
        var controlDropListNivel1_ModalBusqueda;
        var controlDropListNivel2_ModalBusqueda;
        var controlDropListNivel3_ModalBusqueda;
        var controlDropListNivel4_ModalBusqueda;

        var controlDropListEmpresa_ModalAgregarRegistro;
        var controlDropListSucursal_ModalAgregarRegistro;
        var controlDropListPuntoVenta_ModalAgregarRegistro;
        var controlDropListTipoDeMovimiento_ModalAgregarRegistro;
        var controlLabelEmpresaError_ModalAgregarRegistro;
        var controlLabel_ImporteError_ModalAgregarRegistro
        var controlTxtFecha_ModalAgregarRegistro;
        var controlTxtImporte_ModalAgregarRegistro;

        var dropLists = [];

        function pageLoad() {
            AsignarControles_ModalBuscar();
            AsignarControles_ModalAgregarRegistro();
        }

        function AsignarControles_ModalBuscar() {
            controlDropListEmpresa = document.getElementById('<%= DropListEmpresa.ClientID %>');
            controlDropListEmpresa.addEventListener("change", CargarSucursales);
            //controlDropListEmpresa.value = 0;

            controlLabelEmpresaError = document.getElementById('<%= lbDropListEmpresaError.ClientID %>');

            controlDropListSucursal = document.getElementById('<%= DropListSucursal.ClientID %>');
            controlDropListSucursal.addEventListener("change", CargarPuntosVenta);

            controlDropListPuntoVenta = document.getElementById('<%= DropListPuntoVenta.ClientID %>');

            controlDropListTipoDeMovimiento = document.getElementById('<%= DropListTipoDeMovimiento.ClientID %>');

            var fechaActual = ObtenerFechaActual();
            controlTxtFechaDesde = document.getElementById('<%= txtFechaHasta.ClientID %>');
            controlTxtFechaHasta = document.getElementById('<%= txtFechaDesde.ClientID %>');
            controlTxtFechaDesde.value = fechaActual;
            controlTxtFechaHasta.value = fechaActual;

            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            controlDropListNivel1_ModalBusqueda = document.getElementById('<%= dropListNivel1_ModalBusqueda.ClientID %>');
            controlDropListNivel2_ModalBusqueda = document.getElementById('<%= dropListNivel2_ModalBusqueda.ClientID %>');
            controlDropListNivel3_ModalBusqueda = document.getElementById('<%= dropListNivel3_ModalBusqueda.ClientID %>');
            controlDropListNivel4_ModalBusqueda = document.getElementById('<%= dropListNivel4_ModalBusqueda.ClientID %>');

            controlDropListNivel1_ModalBusqueda.addEventListener("change", ChangeNivel1);
            controlDropListNivel2_ModalBusqueda.addEventListener("change", CargarNivel3);
            controlDropListNivel3_ModalBusqueda.addEventListener("change", CargarNivel4);

            dropLists.push(controlDropListNivel2_ModalBusqueda);
            dropLists.push(controlDropListNivel3_ModalBusqueda);
            dropLists.push(controlDropListNivel4_ModalBusqueda);
        }

        function AsignarControles_ModalAgregarRegistro() {
            controlDropListEmpresa_ModalAgregarRegistro = document.getElementById('<%= DropListEmpresa_ModalAgregarRegistro.ClientID %>');
            controlDropListEmpresa_ModalAgregarRegistro.addEventListener("change", CargarSucursales_ModalAgregarRegistro);
            controlDropListEmpresa_ModalAgregarRegistro.value = 0;

            controlLabelEmpresaError_ModalAgregarRegistro = document.getElementById('<%= lbErrorDropListEmpresa_ModalAgregarRegistro.ClientID %>');

            controlDropListSucursal_ModalAgregarRegistro = document.getElementById('<%= DropListSucursal_ModalAgregarRegistro.ClientID %>');
            controlDropListSucursal_ModalAgregarRegistro.addEventListener("change", CargarPuntosVenta_ModalAgregarRegistro);

            controlDropListPuntoVenta_ModalAgregarRegistro = document.getElementById('<%= dropListPuntoVenta_ModalAgregarRegistro.ClientID %>');

            controlDropListTipoDeMovimiento_ModalAgregarRegistro = document.getElementById('<%= DropListTipoDeMovimiento.ClientID %>');

            var fechaActual = ObtenerFechaActual();
            controlTxtFecha_ModalAgregarRegistro = document.getElementById('<%= txtFecha_ModalAgregarRegistro.ClientID %>');
            controlTxtFecha_ModalAgregarRegistro.value = fechaActual;
            $("#<%= txtFecha_ModalAgregarRegistro.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            controlLabel_ImporteError_ModalAgregarRegistro = document.getElementById('<%= lb_ImporteError_ModalAgregarRegistro.ClientID %>');

            controlTxtImporte_ModalAgregarRegistro = document.getElementById('<%= txtImporte_ModalAgregarRegistro.ClientID %>');
        }

        function CargarSucursales() {
            $.ajax({
                type: "POST",
                url: "MayorF.aspx/ObtenerSucursalesDependiendoDeLaEmpresa",
                data: '{empresa: "' + controlDropListEmpresa.value + '"  }',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudieron cargar las sucursales.");
                },
                success: OnSuccessSucursal
            });
        }

        function OnSuccessSucursal(response) {
            while (controlDropListSucursal.options.length > 0) {
                controlDropListSucursal.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            option = document.createElement('option');
            option.value = 0;
            option.text = "Todos";
            controlDropListSucursal.add(option);

            for (i = 0; i < obj.length; i++) {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombre;

                controlDropListSucursal.add(option);
            }
            CargarPuntosVenta();
        }

        function CargarPuntosVenta() {
            var DropListSucursal = document.getElementById('<%= DropListSucursal.ClientID %>').value;

            $.ajax({
                type: "POST",
                url: "MayorF.aspx/ObtenerPuntosDeVentaDependiendoDeLaSucursal",
                data: '{sucursal: "' + DropListSucursal + '"  }',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudieron cargar los puntos de venta.");
                },
                success: OnSuccessPuntoVenta
            });
        }

        function OnSuccessPuntoVenta(response) {
            while (controlDropListPuntoVenta.options.length > 0) {
                controlDropListPuntoVenta.remove(0);
            }

            if (SiSeleccionoTodasLasSucursalesPonerTodosEnPuntosDeVenta()) {
                return;
            }

            var data = response.d;
            obj = JSON.parse(data);

            option = document.createElement('option');
            option.value = 0;
            option.text = "Todos";
            controlDropListPuntoVenta.add(option);

            for (i = 0; i < obj.length; i++) {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombre;

                controlDropListPuntoVenta.add(option);
            }
        }

        function SiSeleccionoTodasLasSucursalesPonerTodosEnPuntosDeVenta() {
            if (controlDropListSucursal.value == 0) {
                var option = document.createElement('option');
                option.value = 0;
                option.text = "Todos";

                controlDropListPuntoVenta.add(option);
                return true;
            }
        }

        function Filtrar(obj) {
            var valorTxtFechaDesde = controlTxtFechaDesde.value;
            var valorTxtFechaHasta = controlTxtFechaHasta.value;
            var valorDropListEmpresa = controlDropListEmpresa.value;
            var valorDropListSucursal = controlDropListSucursal.value;
            var valorDropListPuntoVenta = controlDropListPuntoVenta.value;
            var valorDropListTipoDeMovimiento = controlDropListTipoDeMovimiento.value;

            var fechaDesde = InvertirDiaPorMes(valorTxtFechaDesde);
            var fechaHasta = InvertirDiaPorMes(valorTxtFechaHasta);

            $(obj).attr('disabled', 'disabled');

            $.ajax({
                type: "POST",
                url: "MayorF.aspx/Filtrar",
                data: '{ fechaDesde: "' + fechaDesde.toUTCString() + '", fechaHasta: "' + fechaHasta.toUTCString() + '", idTipoMovimiento: "' + valorDropListTipoDeMovimiento + '", idEmpresa: "' + valorDropListEmpresa + '", idSucursal: "' + valorDropListSucursal + '", idPuntoVenta: "' + valorDropListPuntoVenta + '" }',
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
            var controlBotonFiltrar = document.getElementById('<%= lbtnBuscar.ClientID %>');

            var data = response.d;
            var obj = JSON.parse(data);

            document.getElementById('btnCerrarModalBusqueda').click();
            $("#tablaMayor").dataTable().fnDestroy();
            $('#tablaMayor').find("tr:gt(0)").remove();

            for (var i = 0; i < obj.length; i++) {
                $('#tablaMayor').append(
                    "<tr>" +
                    "<td> " + obj[i].Fecha + "</td>" +
                    "<td> " + obj[i].TipoMovimiento + "</td>" +
                    "<td> " + obj[i].NumeroDocumento + "</td>" +
                    "<td> " + obj[i].Asiento_Item + "</td>" +
                    "<td> " + obj[i].Asiento_Cuenta + "</td>" +
                    '<td style="text-align:right">' + obj[i].Debito + "</td>" +
                    '<td style="text-align:right">' + obj[i].Credito + "</td>" +
                    "<td> " + obj[i].Empresa + "</td>" +
                    "<td> " + obj[i].Sucursal + "</td>" +
                    '<td style="text-align:right">' + obj[i].PuntoDeVenta + "</td>" +
                    "<td> " + obj[i].Nivel1 + "</td>" +
                    "<td> " + obj[i].Nivel2 + "</td>" +
                    "<td> " + obj[i].Nivel3 + "</td>" +
                    "<td> " + obj[i].Nivel4 + "</td>" +
                    "</tr> ");
            };
            $(controlBotonFiltrar).removeAttr('disabled');

            //$('#tablaMayor').DataTable({
            //    "scrollX": true,
            //    "scrollY": 200,
            //    "bPaginate": true
            //});
        }

        function ValidarFormulario(obj) {
            var estadoDelFormulario = true;

            if (controlDropListEmpresa.value == 0) {
                controlLabelEmpresaError.style.display = "block";
                estadoDelFormulario = false;
            }
            if (estadoDelFormulario) {
                Filtrar(obj);
            }
        }

        function ChangeNivel1() {

            BorrarLosDropListDeNiveles();

            if (controlDropListNivel1_ModalBusqueda.value != 0) {
                CargarNivel2();
            }
            else {
                CargarLosDropListDeNivelesConElItem_TODOS();
            }
        }

        function BorrarLosDropListDeNiveles() {

            for (var i in dropLists) {
                while (dropLists[i].options.length > 0) {
                    dropLists[i].remove(0);
                }
            }
        }

        function CargarLosDropListDeNivelesConElItem_TODOS() {
            for (var i in dropLists) {
                var option = document.createElement('option');
                option.value = 0;
                option.text = "Todos";

                dropLists[i].add(option);
            }
        }

        function CargarNivel2() {
            $.ajax({
                type: "POST",
                url: "MayorF.aspx/ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel",
                data: '{jerarquia: "' + 2 + '", nivel: "' + parseInt(controlDropListNivel1_ModalBusqueda.value) + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudo cargar el nivel 2.");
                },
                success: OnSuccessCargarNivel2
            });
        };

        function OnSuccessCargarNivel2(response) {
            while (controlDropListNivel2_ModalBusqueda.options.length > 0) {
                controlDropListNivel2_ModalBusqueda.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            option = document.createElement('option');
            option.value = 0;
            option.text = "Todos";
            controlDropListNivel2_ModalBusqueda.add(option);

            for (i = 0; i < obj.length; i++) {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombre;

                controlDropListNivel2_ModalBusqueda.add(option);
            }
            if (controlDropListNivel1_ModalBusqueda.value == 0) {
                return;
            }
            CargarNivel3();
        }

        function CargarNivel3() {
            $.ajax({
                type: "POST",
                url: "MayorF.aspx/ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel",
                data: '{jerarquia: "' + 3 + '", nivel: "' + parseInt(controlDropListNivel2_ModalBusqueda.value) + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudo cargar el nivel 3.");
                },
                success: OnSuccessCargarNivel3
            });
        };

        function OnSuccessCargarNivel3(response) {
            while (controlDropListNivel3_ModalBusqueda.options.length > 0) {
                controlDropListNivel3_ModalBusqueda.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            option = document.createElement('option');
            option.value = 0;
            option.text = "Todos";
            controlDropListNivel3_ModalBusqueda.add(option);

            for (i = 0; i < obj.length; i++) {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombre;

                controlDropListNivel3_ModalBusqueda.add(option);
            }
            if (controlDropListNivel1_ModalBusqueda.value == 0) {
                return;
            }
            CargarNivel4();
        }

        function CargarNivel4() {
            $.ajax({
                type: "POST",
                url: "MayorF.aspx/ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel",
                data: '{jerarquia: "' + 4 + '", nivel: "' + parseInt(controlDropListNivel3_ModalBusqueda.value) + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudo cargar el nivel 4.");
                },
                success: OnSuccessCargarNivel4
            });
        }

        function OnSuccessCargarNivel4(response) {
            while (controlDropListNivel4_ModalBusqueda.options.length > 0) {
                controlDropListNivel4_ModalBusqueda.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            option = document.createElement('option');
            option.value = 0;
            option.text = "Todos";
            controlDropListNivel4_ModalBusqueda.add(option);

            for (i = 0; i < obj.length; i++) {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombre;

                controlDropListNivel4_ModalBusqueda.add(option);
            }
        }

        //modal agregar registro
        function CargarSucursales_ModalAgregarRegistro() {
            $.ajax({
                type: "POST",
                url: "MayorF.aspx/ObtenerSucursalesDependiendoDeLaEmpresa",
                data: '{empresa: "' + controlDropListEmpresa_ModalAgregarRegistro.value + '"  }',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudieron cargar las sucursales.");
                },
                success: OnSuccessSucursal_ModalAgregarRegistro
            });
        }

        function OnSuccessSucursal_ModalAgregarRegistro(response) {
            while (controlDropListSucursal_ModalAgregarRegistro.options.length > 0) {
                controlDropListSucursal_ModalAgregarRegistro.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            for (i = 0; i < obj.length; i++) {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombre;

                controlDropListSucursal_ModalAgregarRegistro.add(option);
            }
            CargarPuntosVenta_ModalAgregarRegistro();
        }

        function CargarPuntosVenta_ModalAgregarRegistro() {
            var DropListSucursal = document.getElementById('<%= DropListSucursal_ModalAgregarRegistro.ClientID %>').value;

            $.ajax({
                type: "POST",
                url: "MayorF.aspx/ObtenerPuntosDeVentaDependiendoDeLaSucursal",
                data: '{sucursal: "' + DropListSucursal + '"  }',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudieron cargar los puntos de venta.");
                },
                success: OnSuccessPuntoVenta_ModalAgregarRegistro
            });
        }

        function OnSuccessPuntoVenta_ModalAgregarRegistro(response) {
            while (controlDropListPuntoVenta_ModalAgregarRegistro.options.length > 0) {
                controlDropListPuntoVenta_ModalAgregarRegistro.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            for (i = 0; i < obj.length; i++) {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombre;

                controlDropListPuntoVenta_ModalAgregarRegistro.add(option);
            }
        }

        function ValidarFormulario_ModalCrearRegistroManual() {
            var estadoDelFormulario = true;

            if (controlDropListEmpresa_ModalAgregarRegistro.value == 0) {
                controlLabelEmpresaError_ModalAgregarRegistro.style.display = "block";
                estadoDelFormulario = false;
            }
            if (controlTxtImporte_ModalAgregarRegistro.value.length == 0 || !controlTxtImporte_ModalAgregarRegistro.value.trim()) {
                controlLabel_ImporteError_ModalAgregarRegistro.style.display = "block";
                estadoDelFormulario = false;
            }
            if (estadoDelFormulario) {
                return true;
            }
            return false;
        }

        //valida los campos solo numeros
        function ValidarSoloNumeros(e) {
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
                if (key == 8)// || key == 44) //backspace (retroceso) y , (coma)
                { return true; }
                else { return false; }
            }
            return true;
        }
    </script>
</asp:Content>
