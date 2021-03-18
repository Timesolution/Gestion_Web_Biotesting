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
                                    <th>Tipo de movimiento</th>
                                    <th>Numero de Documento</th>
                                    <th>Cliente</th>    
                                    <th style="text-align: right">Debito</th>
                                    <th style="text-align: right">Credito</th>
                                    <th style="text-align: right">Saldo Acumulado</th>
                                    <th>Nivel 1</th>
                                    <th>Nivel 2</th>
                                    <th>Nivel 3</th>
                                    <th>Nivel 4</th>
                                    <th>Nivel 5</th>
                                    <th>Observaciones</th>
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
                            <div class="form-group">
                                <label class="col-md-4">Nivel 5</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="dropListNivel5_ModalBusqueda" runat="server" class="form-control"></asp:DropDownList>
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
            <div class="modal-dialog" style="width: 100%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" id="btnCerrar_ModalAgregarRegistro" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title" style="text-align: center;">Agregar Movimiento Manual</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">

                                <div class="row col-lg-12">
                                    <div class="col-md-2">
                                        <label>Fecha</label>
                                        <asp:TextBox ID="txtFecha_ModalAgregarRegistro" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <label>Empresa</label>
                                        <asp:DropDownList ID="DropListEmpresa_ModalAgregarRegistro" runat="server" class="form-control" AutoPostBack="false"></asp:DropDownList>
                                        <asp:Label runat="server" ID="lbErrorDropListEmpresa_ModalAgregarRegistro" Style="display: none" Text="Seleccione un item." ForeColor="Red" Font-Bold="true"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <label>Sucursal</label>
                                        <asp:DropDownList ID="DropListSucursal_ModalAgregarRegistro" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <label>Punto Venta</label>
                                        <asp:DropDownList ID="dropListPuntoVenta_ModalAgregarRegistro" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                </div>

                                <hr />

                                <div class="row col-lg-12" style="display: flex; justify-content: flex-end;">
                                    <asp:TextBox ID="txtBuscarNiveles" runat="server" class="form-control" placeholder="Busqueda nivel 5" style="margin-right: 5px; max-width: 33%;"></asp:TextBox>
                                    <asp:LinkButton ID="lbtnBuscarNiveles" OnClientClick="CargarNivel5_ModalAgregarRegistro(); return false;" runat="server" Text="Buscar" class="btn btn-success" AutoPostBack="false" />
                                </div>

                                <div class="row">
                                    <div class="col-md-5ths col-xs-6">
                                        <label>Nivel 1</label>
                                        <asp:DropDownList ID="DropListNivel1_ModalAgregarRegistro" onchange="CargarNivel2_ModalAgregarRegistro()" runat="server" class="form-control"></asp:DropDownList>
                                    </div>

                                    <div class="col-md-5ths col-xs-6">
                                        <label>Nivel 2</label>
                                        <asp:DropDownList ID="DropListNivel2_ModalAgregarRegistro" onchange="CargarNivel3_ModalAgregarRegistro()" runat="server" class="form-control"></asp:DropDownList>
                                    </div>

                                    <div class="col-md-5ths col-xs-6">
                                        <label>Nivel 3</label>
                                        <asp:DropDownList ID="DropListNivel3_ModalAgregarRegistro" onchange="CargarNivel4_ModalAgregarRegistro()" runat="server" class="form-control"></asp:DropDownList>
                                    </div>

                                    <div class="col-md-5ths col-xs-6">
                                        <label>Nivel 4</label>
                                        <asp:DropDownList ID="DropListNivel4_ModalAgregarRegistro" onchange="CargarNivel5_ModalAgregarRegistro()" runat="server" class="form-control"></asp:DropDownList>
                                    </div>

                                    <div class="col-md-5ths col-xs-6">
                                        <label>Nivel 5</label>
                                        <asp:DropDownList ID="DropListNivel5_ModalAgregarRegistro" onchange="OnChangeNivel5()" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                </div>

                                <hr />

                                <div class="row col-lg-12">
                                    <div class="col-md-3">
                                        <label>Tipo Operacion</label>
                                        <asp:DropDownList ID="dropList_TipoOperacion_ModalAgregarRegistro" runat="server" class="form-control">
                                            <asp:ListItem Value="1">Haber</asp:ListItem>
                                            <asp:ListItem Value="2">Debe</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-md-3">
                                        <label>Importe</label>
                                        <asp:TextBox ID="txtImporte_ModalAgregarRegistro" onkeypress="javascript:return ValidarSoloNumeros(event)" runat="server" class="form-control"></asp:TextBox>
                                        <asp:Label runat="server" ID="lb_ImporteError_ModalAgregarRegistro" Style="display: none" Text="Ingrese un importe." ForeColor="Red" Font-Bold="true"></asp:Label>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Observaciones</label>
                                        <asp:TextBox ID="txtObservaciones_ModalAgregarRegistro" TextMode="MultiLine" Rows="5" runat="server" class="form-control"></asp:TextBox>
                                        <asp:Label runat="server" ID="Label1" Style="display: none" Text="Ingrese un importe." ForeColor="Red" Font-Bold="true"></asp:Label>
                                    </div>

                                    <div class="col-md-3">
                                        <br />
                                        <asp:LinkButton ID="lbtnCrearRegistroManual" OnClientClick="javascript: return ValidarFormulario_ModalCrearRegistroManual()" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" AutoPostBack="false" />
                                    </div>
                                </div>

                                <hr />

                                <div class="row col-lg-12">
                                    <div class="table-responsive">
                                        <table class="table table-striped table-bordered table-sm" id="tablaMayor_Temporal" style="width: 100%">
                                            <thead>
                                                <tr>
                                                    <th>Fecha</th>
                                                    <th style="text-align: right">Debito</th>
                                                    <th style="text-align: right">Credito</th>
                                                    <th>Empresa</th>
                                                    <th>Sucursal</th>
                                                    <th>Punto de Venta</th>
                                                    <th>Plan Cuenta</th>
                                                    <th>Observaciones</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                        </table>
                                        <button id="btnCrearRegistros" class="btn btn-success" style="display: none">Crear Registros</button>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <style>
        .col-xs-5ths,
        .col-sm-5ths,
        .col-md-5ths,
        .col-lg-5ths {
            position: relative;
            min-height: 1px;
            padding-right: 15px;
            padding-left: 15px;
        }

        .col-xs-5ths {
            width: 20%;
            float: left;
        }

        @media (min-width: 768px) {
            .col-sm-5ths {
                width: 20%;
                float: left;
            }
        }

        @media (min-width: 992px) {
            .col-md-5ths {
                width: 20%;
                float: left;
            }
        }

        @media (min-width: 1200px) {
            .col-lg-5ths {
                width: 20%;
                float: left;
            }
        }
    </style>

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
        var controlDropListNivel5_ModalBusqueda;

        var controlDropListNivel1_ModalAgregarRegistro;
        var controlDropListNivel2_ModalAgregarRegistro;
        var controlDropListNivel3_ModalAgregarRegistro;
        var controlDropListNivel4_ModalAgregarRegistro;
        var controlDropListNivel5_ModalAgregarRegistro;

        var controlDropListEmpresa_ModalAgregarRegistro;
        var controlDropListSucursal_ModalAgregarRegistro;
        var controlDropListPuntoVenta_ModalAgregarRegistro;
        var controlDropListTipoDeMovimiento_ModalAgregarRegistro;
        var controlLabelEmpresaError_ModalAgregarRegistro;
        var controlLabel_ImporteError_ModalAgregarRegistro
        var controlTxtFecha_ModalAgregarRegistro;
        var controlTxtImporte_ModalAgregarRegistro;
        var controlDropList_TipoOperacion_ModalAgregarRegistro;
        var controlTxtObservaciones_ModalAgregarRegistro;

        var dropLists_ModalBusqueda = [];
        var dropLists_Niveles_ModalAgregarRegistro = [];

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
            controlTxtFechaDesde = document.getElementById('<%= txtFechaDesde.ClientID %>');
            controlTxtFechaHasta = document.getElementById('<%= txtFechaHasta.ClientID %>');
            controlTxtFechaDesde.value = fechaActual;
            controlTxtFechaHasta.value = fechaActual;

            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            controlDropListNivel1_ModalBusqueda = document.getElementById('<%= dropListNivel1_ModalBusqueda.ClientID %>');
            controlDropListNivel2_ModalBusqueda = document.getElementById('<%= dropListNivel2_ModalBusqueda.ClientID %>');
            controlDropListNivel3_ModalBusqueda = document.getElementById('<%= dropListNivel3_ModalBusqueda.ClientID %>');
            controlDropListNivel4_ModalBusqueda = document.getElementById('<%= dropListNivel4_ModalBusqueda.ClientID %>');
            controlDropListNivel5_ModalBusqueda = document.getElementById('<%= dropListNivel5_ModalBusqueda.ClientID %>');

            controlDropListNivel1_ModalBusqueda.addEventListener("change", CargarNivel2);
            controlDropListNivel2_ModalBusqueda.addEventListener("change", CargarNivel3);
            controlDropListNivel3_ModalBusqueda.addEventListener("change", CargarNivel4);
            controlDropListNivel4_ModalBusqueda.addEventListener("change", CargarNivel5);

            dropLists_ModalBusqueda.push(controlDropListNivel2_ModalBusqueda);
            dropLists_ModalBusqueda.push(controlDropListNivel3_ModalBusqueda);
            dropLists_ModalBusqueda.push(controlDropListNivel4_ModalBusqueda);
            dropLists_ModalBusqueda.push(controlDropListNivel5_ModalBusqueda);
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
            controlTxtObservaciones_ModalAgregarRegistro = document.getElementById('<%= txtObservaciones_ModalAgregarRegistro.ClientID %>');

            controlDropList_TipoOperacion_ModalAgregarRegistro = document.getElementById('<%= dropList_TipoOperacion_ModalAgregarRegistro.ClientID %>');

            controlDropListNivel1_ModalAgregarRegistro = document.getElementById('<%= DropListNivel1_ModalAgregarRegistro.ClientID %>');
            controlDropListNivel2_ModalAgregarRegistro = document.getElementById('<%= DropListNivel2_ModalAgregarRegistro.ClientID %>');
            controlDropListNivel3_ModalAgregarRegistro = document.getElementById('<%= DropListNivel3_ModalAgregarRegistro.ClientID %>');
            controlDropListNivel4_ModalAgregarRegistro = document.getElementById('<%= DropListNivel4_ModalAgregarRegistro.ClientID %>');
            controlDropListNivel5_ModalAgregarRegistro = document.getElementById('<%= DropListNivel5_ModalAgregarRegistro.ClientID %>');

            dropLists_Niveles_ModalAgregarRegistro.push(controlDropListNivel1_ModalAgregarRegistro);
            dropLists_Niveles_ModalAgregarRegistro.push(controlDropListNivel2_ModalAgregarRegistro);
            dropLists_Niveles_ModalAgregarRegistro.push(controlDropListNivel3_ModalAgregarRegistro);
            dropLists_Niveles_ModalAgregarRegistro.push(controlDropListNivel4_ModalAgregarRegistro);
            dropLists_Niveles_ModalAgregarRegistro.push(controlDropListNivel5_ModalAgregarRegistro);
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

            for (var i = 0; i < obj.length; i++) {
                $('#tablaMayor').append(
                    "<tr>" +
                    "<td> " + obj[i].Fecha + "</td>" +
                    "<td> " + obj[i].TipoMovimiento + "</td>" +
                    "<td> " + obj[i].NumeroDocumento + "</td>" +
                    "<td> " + obj[i].Cliente + "</td>" +
                    '<td style="text-align:right">' + obj[i].Debito + "</td>" +
                    '<td style="text-align:right">' + obj[i].Credito + "</td>" +
                    '<td style="text-align:right">' + obj[i].SaldoAcumulado + "</td>" +
                    "<td> " + obj[i].Nivel1 + "</td>" +
                    "<td> " + obj[i].Nivel2 + "</td>" +
                    "<td> " + obj[i].Nivel3 + "</td>" +
                    "<td> " + obj[i].Nivel4 + "</td>" +
                    "<td> " + obj[i].Nivel5 + "</td>" +
                    '<td data-toggle="modal" data-placement="top" title="' + obj[i].Observaciones +'" >' + obj[i].Observaciones.substring(0,20) + "</td>" +
                    "</tr> ");
            };
            $(controlBotonFiltrar).removeAttr('disabled');
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

            for (var i in dropLists_ModalBusqueda) {
                while (dropLists_ModalBusqueda[i].options.length > 0) {
                    dropLists_ModalBusqueda[i].remove(0);
                }
            }
        }

        function CargarLosDropListDeNivelesConElItem_TODOS() {
            for (var i in dropLists_ModalBusqueda) {
                var option = document.createElement('option');
                option.value = 0;
                option.text = "Todos";

                dropLists_ModalBusqueda[i].add(option);
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

        function CargarNivel5() {
            $.ajax({
                type: "POST",
                url: "MayorF.aspx/ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel",
                data: '{jerarquia: "' + 5 + '", nivel: "' + parseInt(controlDropListNivel4_ModalBusqueda.value) + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudo cargar el nivel 5.");
                },
                success: OnSuccessCargarNivel5
            });
        }

        function OnSuccessCargarNivel5(response) {
            while (controlDropListNivel5_ModalBusqueda.options.length > 0) {
                controlDropListNivel5_ModalBusqueda.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            option = document.createElement('option');
            option.value = 0;
            option.text = "Todos";
            controlDropListNivel5_ModalBusqueda.add(option);

            for (i = 0; i < obj.length; i++) {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombre;

                controlDropListNivel5_ModalBusqueda.add(option);
            }
        }

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
            else {
                controlLabelEmpresaError_ModalAgregarRegistro.style.display = "none";
            }
            if (controlTxtImporte_ModalAgregarRegistro.value.length == 0 || !controlTxtImporte_ModalAgregarRegistro.value.trim()) {
                controlLabel_ImporteError_ModalAgregarRegistro.style.display = "block";
                estadoDelFormulario = false;
            }
            else {
                controlLabel_ImporteError_ModalAgregarRegistro.style.display = "none";
            }
            if (!VerificarSiSeleccionoTodosLosNiveles()) {
                alert('Debe seleccionar todos los niveles');
                estadoDelFormulario = false;
            }
            if (estadoDelFormulario) {
                AgregarRegistroTo_TablaTemporal();
            }
            return false;
        }

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
                if (key == 8 || key == 46)//46 = '.'
                {
                    if (key == 46 && controlTxtImporte_ModalAgregarRegistro.value.includes(".", 0)) {
                        return false;
                    }
                    return true;
                }
                else { return false; }
            }
            return true;
        }

        function AgregarRegistroTo_TablaTemporal() {
            var controlBoton = document.getElementById('<%= lbtnCrearRegistroManual.ClientID %>');

            var debe = 0;
            var haber = 0;
            if (controlDropList_TipoOperacion_ModalAgregarRegistro.value == "1") {
                haber = controlTxtImporte_ModalAgregarRegistro.value;
            }
            else {
                debe = controlTxtImporte_ModalAgregarRegistro.value;
            }

            var empresa = $("#<%= DropListEmpresa_ModalAgregarRegistro.ClientID %> option:selected").text();
            var sucursal = $('#<%= DropListSucursal_ModalAgregarRegistro.ClientID %> option:selected').text();
            var puntoDeVenta = $('#<%= dropListPuntoVenta_ModalAgregarRegistro.ClientID %> option:selected').text();
            var planCuenta = $('#<%=DropListNivel4_ModalAgregarRegistro.ClientID %> option:selected').text();

            var fecha = controlTxtFecha_ModalAgregarRegistro.value;
            var observaciones = controlTxtObservaciones_ModalAgregarRegistro.value;
            var idEmpresa = $('#<%= DropListEmpresa_ModalAgregarRegistro.ClientID %>').val();
            var idSucursal = $('#<%= DropListSucursal_ModalAgregarRegistro.ClientID %>').val();
            var idPuntoDeVenta = $('#<%= dropListPuntoVenta_ModalAgregarRegistro.ClientID %>').val();
            var idNivel1 = $('#<%= DropListNivel1_ModalAgregarRegistro.ClientID %>').val();
            var idNivel2 = $('#<%= DropListNivel2_ModalAgregarRegistro.ClientID %>').val();
            var idNivel3 = $('#<%= DropListNivel3_ModalAgregarRegistro.ClientID %>').val();
            var idNivel4 = $('#<%= DropListNivel4_ModalAgregarRegistro.ClientID %>').val();
            var idNivel5 = $('#<%= DropListNivel5_ModalAgregarRegistro.ClientID %>').val();

            var datos_DeLaFila = [fecha, debe, haber, idEmpresa, idSucursal, idPuntoDeVenta, idNivel1, idNivel2, idNivel3, idNivel4, idNivel5, observaciones].join('_');

            var table = document.getElementById("tablaMayor_Temporal");

            $('#tablaMayor_Temporal').append("<tr id=\"" + (table.rows.length - 1) + "\">" +
                "<td>" + fecha + "</td>" +
                "<td style=\"text-align:right\">" + debe + "</td>" +
                "<td style=\"text-align:right\">" + haber + "</td>" +
                "<td>" + empresa + "</td>" +
                "<td>" + sucursal + "</td>" +
                "<td>" + puntoDeVenta + "</td>" +
                "<td>" + planCuenta + "</td>" +
                "<td>" + observaciones + "</td>" +
                "<td style=\"display:none\">" + datos_DeLaFila + "</td>" +
                "<td>" +
                "<a onclick=\"javascript: return eliminarFila(this);\" id=\"btnEliminar_" + (table.rows.length - 1) + "\" class=\"btn btn-danger\" style=\"text-align: right\" autopostback=\"false\"><span class=\"shortcut-icon icon-trash\"></span></a>" +
                "</td></tr>");

            $("#btnCrearRegistros").css("display", "block");
        }

        function eliminarFila(obj) {
            var table = document.getElementById("tablaMayor_Temporal");
            var rowCount = table.rows.length;

            if (rowCount <= 1)
                alert('No se puede eliminar el encabezado');
            else {
                var id = obj.id.split('_')[1];
                var fila = document.getElementById(id);
                fila.remove();
            }

            if (table.rows.length == 1) {
                $("#btnCrearRegistros").css("display", "none");
            }
            return false;
        }

        $("#btnCrearRegistros").click(function () {
            var objeto = new Array();

            controlDropListEmpresa.value = 0;

            $("td").each(function () {
                var campo = $(this).text();
                if (campo.includes("_", 0)) {
                    objeto.push(campo);
                }
            });

            if (!VerificarSiLaSumaDeLosRegistrosDelDebeYElHaberDaCero(objeto)) {
                alert('La diferencia entre el debe y el haber debe dar "0"');
                return false;
            }

            $.ajax({
                type: "POST",
                url: "MayorF.aspx/InsertarRegistroEnLaTablaMayor_JSON",
                data: JSON.stringify({ objetos: objeto }),
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudieron crear los registros.");
                },
                success: OnSuccess_CrearRegistros
            });
        });

        function VerificarSiLaSumaDeLosRegistrosDelDebeYElHaberDaCero(objeto) {
            var debe = 0;
            var haber = 0;
            for (var i in objeto) {
                debe += parseFloat(objeto[i].split('_')[1]);
                haber += parseFloat(objeto[i].split('_')[2]);
            }
            var resultado = debe - haber;

            if (resultado == 0) {
                return true;
            }
            return false;
        }

        function OnSuccess_CrearRegistros() {
            alert("Registros creados correctamente.");
            window.location.replace("/Formularios/Valores/MayorF.aspx");

            return false;
        }

        function CargarNivel2_ModalAgregarRegistro() {
            $.ajax({
                type: "POST",
                url: "MayorF.aspx/ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel",
                data: '{jerarquia: "' + 2 + '", nivel: "' + parseInt(controlDropListNivel1_ModalAgregarRegistro.value) + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudo cargar el nivel 2.");
                },
                success: OnSuccessCargarNivel2_ModalAgregarRegistro
            });
        };

        function OnSuccessCargarNivel2_ModalAgregarRegistro(response) {
            while (controlDropListNivel2_ModalAgregarRegistro.options.length > 0) {
                controlDropListNivel2_ModalAgregarRegistro.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            MostrarAlertaDeQueNoSeEncontroElNivelSiNoHayDatos(data, 2);

            for (i = 0; i < obj.length; i++) {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombre;

                controlDropListNivel2_ModalAgregarRegistro.add(option);
            }
            if (controlDropListNivel1_ModalAgregarRegistro.value == 0) {
                return;
            }
            CargarNivel3_ModalAgregarRegistro();
        }

        function CargarNivel3_ModalAgregarRegistro() {
            $.ajax({
                type: "POST",
                url: "MayorF.aspx/ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel",
                data: '{jerarquia: "' + 3 + '", nivel: "' + parseInt(controlDropListNivel2_ModalAgregarRegistro.value) + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudo cargar el nivel 3.");
                },
                success: OnSuccessCargarNivel3_ModalAgregarRegistro
            });
        };

        function OnSuccessCargarNivel3_ModalAgregarRegistro(response) {
            while (controlDropListNivel3_ModalAgregarRegistro.options.length > 0) {
                controlDropListNivel3_ModalAgregarRegistro.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            MostrarAlertaDeQueNoSeEncontroElNivelSiNoHayDatos(data, 3);

            for (i = 0; i < obj.length; i++) {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombre;

                controlDropListNivel3_ModalAgregarRegistro.add(option);
            }
            if (controlDropListNivel1_ModalAgregarRegistro.value == 0) {
                return;
            }
            CargarNivel4_ModalAgregarRegistro();
        }

        function CargarNivel4_ModalAgregarRegistro() {
            $.ajax({
                type: "POST",
                url: "MayorF.aspx/ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel",
                data: '{jerarquia: "' + 4 + '", nivel: "' + parseInt(controlDropListNivel3_ModalAgregarRegistro.value) + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudo cargar el nivel 4.");
                },
                success: OnSuccessCargarNivel4_ModalAgregarRegistro
            });
        }

        function OnSuccessCargarNivel4_ModalAgregarRegistro(response) {
            while (controlDropListNivel4_ModalAgregarRegistro.options.length > 0) {
                controlDropListNivel4_ModalAgregarRegistro.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            MostrarAlertaDeQueNoSeEncontroElNivelSiNoHayDatos(data, 4);

            for (i = 0; i < obj.length; i++) {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombre;

                controlDropListNivel4_ModalAgregarRegistro.add(option);
            }
        }

        function CargarNivel5_ModalAgregarRegistro() {
            $.ajax({
                type: "POST",
                url: "MayorF.aspx/ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel",
                data: '{jerarquia: "' + 5 + '", nivel: "' + parseInt(controlDropListNivel4_ModalAgregarRegistro.value) + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudo cargar el nivel 5.");
                },
                success: OnSuccessCargarNivel5_ModalAgregarRegistro
            });
        }

        function OnSuccessCargarNivel5_ModalAgregarRegistro(response) {
            while (controlDropListNivel5_ModalAgregarRegistro.options.length > 0) {
                controlDropListNivel5_ModalAgregarRegistro.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            MostrarAlertaDeQueNoSeEncontroElNivelSiNoHayDatos(data, 5);

            for (i = 0; i < obj.length; i++) {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombre;

                controlDropListNivel5_ModalAgregarRegistro.add(option);
            }
        }

        function MostrarAlertaDeQueNoSeEncontroElNivelSiNoHayDatos(datos, nivel) {
            if (datos == "[]") {
                alert('No se encontraron datos del nivel ' + nivel);
            }
        }

        function VerificarSiSeleccionoTodosLosNiveles() {
            var seleccionoTodo = true;

            for (var i in dropLists_Niveles_ModalAgregarRegistro) {
                if (dropLists_Niveles_ModalAgregarRegistro[i].value == 0) {
                    seleccionoTodo = false;
                }
            }
            return seleccionoTodo;
        }


        //Buscador esto busca el ultimo nivel, cuando se selecciona 1, se setean los niveles anteriores en sus correspondientes DDL.

        function CargarNivel5_ModalAgregarRegistro() {

            var buscador = document.getElementById('<%= txtBuscarNiveles.ClientID %>');

            $.ajax({
                type: "POST",
                url: "MayorF.aspx/BuscarUltimoNivel",
                data: '{textoABuscar: "' + buscador.value + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("Hubo un error en la busqueda.");
                },
                success: OnSuccessCargarNivel5_ModalAgregarRegistro2
            });
        }

        //Cargo el nivel 5
        function OnSuccessCargarNivel5_ModalAgregarRegistro2(response) {
            while (controlDropListNivel5_ModalAgregarRegistro.options.length > 0) {
                controlDropListNivel5_ModalAgregarRegistro.remove(0);
            }

            //BorradoMasivoDeDropDownList();

            var data = response.d;
            obj = JSON.parse(data);

            MostrarAlertaDeQueNoSeEncontroElNivelSiNoHayDatos(data, 5);

            for (i = 0; i < obj.length; i++) {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombre;

                controlDropListNivel5_ModalAgregarRegistro.add(option);
            }

            CargarNivel4DesdeNivel5(controlDropListNivel5_ModalAgregarRegistro.value);
        }

        function OnChangeNivel5() {
            CargarNivel4DesdeNivel5(controlDropListNivel5_ModalAgregarRegistro.value);
        }

        //Hago la busqueda del nivel 4
        function CargarNivel4DesdeNivel5(idJerarquia5){
            $.ajax({
                type: "POST",
                url: "MayorF.aspx/ObtenerNivelAnteriorByIdJerarquia5",
                data: '{idjerarquia: "' + idJerarquia5 + '", nivel: "' + 4 + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert('No se encontraron datos del nivel ' + 4);
                },
                success: OnSuccessCargarNivel4_DesdeNivel5
            });
        }

        //cargo el nivel 4
        function OnSuccessCargarNivel4_DesdeNivel5(response) {
            while (controlDropListNivel4_ModalAgregarRegistro.options.length > 0) {
                controlDropListNivel4_ModalAgregarRegistro.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            MostrarAlertaDeQueNoSeEncontroElNivelSiNoHayDatos(data, 4);

            option = document.createElement('option');
            option.value = obj.id;
            option.text = obj.nombre;

            controlDropListNivel4_ModalAgregarRegistro.add(option);

            CargarNivel3DesdeNivel5(controlDropListNivel5_ModalAgregarRegistro.value);
        }

        //hago la busqueda del nivel 3
        function CargarNivel3DesdeNivel5(idJerarquia5) {
            $.ajax({
                type: "POST",
                url: "MayorF.aspx/ObtenerNivelAnteriorByIdJerarquia5",
                data: '{idjerarquia: "' + idJerarquia5 + '", nivel: "' + 3 + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert('No se encontraron datos del nivel ' + 3);
                },
                success: OnSuccessCargarNivel3_DesdeNivel5
            });
        }

        //Cargo el nivel 3
        function OnSuccessCargarNivel3_DesdeNivel5(response) {
            while (controlDropListNivel3_ModalAgregarRegistro.options.length > 0) {
                controlDropListNivel3_ModalAgregarRegistro.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            MostrarAlertaDeQueNoSeEncontroElNivelSiNoHayDatos(data, 3);

            option = document.createElement('option');
            option.value = obj.id;
            option.text = obj.nombre;

            controlDropListNivel3_ModalAgregarRegistro.add(option);

            CargarNivel2DesdeNivel5(controlDropListNivel5_ModalAgregarRegistro.value);
        }

        //hago la busqueda del nivel 2
        function CargarNivel2DesdeNivel5(idJerarquia5) {
            $.ajax({
                type: "POST",
                url: "MayorF.aspx/ObtenerNivelAnteriorByIdJerarquia5",
                data: '{idjerarquia: "' + idJerarquia5 + '", nivel: "' + 2 + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert('No se encontraron datos del nivel ' + 2);
                },
                success: OnSuccessCargarNivel2_DesdeNivel5
            });
        }

        //Cargo el nivel 2
        function OnSuccessCargarNivel2_DesdeNivel5(response) {
            while (controlDropListNivel2_ModalAgregarRegistro.options.length > 0) {
                controlDropListNivel2_ModalAgregarRegistro.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            MostrarAlertaDeQueNoSeEncontroElNivelSiNoHayDatos(data, 2);

            option = document.createElement('option');
            option.value = obj.id;
            option.text = obj.nombre;

            controlDropListNivel2_ModalAgregarRegistro.add(option);

            CargarNivel1DesdeNivel5(controlDropListNivel5_ModalAgregarRegistro.value);
        }

        //hago la busqueda del nivel 1
        function CargarNivel1DesdeNivel5(idJerarquia5) {
            $.ajax({
                type: "POST",
                url: "MayorF.aspx/ObtenerNivelAnteriorByIdJerarquia5",
                data: '{idjerarquia: "' + idJerarquia5 + '", nivel: "' + 1 + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert('No se encontraron datos del nivel ' + 2);
                },
                success: OnSuccessCargarNivel1_DesdeNivel5
            });
        }

        //Cargo el nivel 1
        function OnSuccessCargarNivel1_DesdeNivel5(response) {
            while (controlDropListNivel1_ModalAgregarRegistro.options.length > 0) {
                controlDropListNivel1_ModalAgregarRegistro.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            MostrarAlertaDeQueNoSeEncontroElNivelSiNoHayDatos(data, 2);

            option = document.createElement('option');
            option.value = obj.id;
            option.text = obj.nombre;

            controlDropListNivel1_ModalAgregarRegistro.add(option);

        }

        function BorradoMasivoDeDropDownList() {

            //while (controlDropListNivel5_ModalAgregarRegistro.options.length > 0) {
            //    controlDropListNivel5_ModalAgregarRegistro.remove(0);
            //}
            //while (controlDropListNivel4_ModalAgregarRegistro.options.length > 0) {
            //    controlDropListNivel4_ModalAgregarRegistro.remove(0);
            //}
            //while (controlDropListNivel3_ModalAgregarRegistro.options.length > 0) {
            //    controlDropListNivel3_ModalAgregarRegistro.remove(0);
            //}
            //while (controlDropListNivel2_ModalAgregarRegistro.options.length > 0) {
            //    controlDropListNivel2_ModalAgregarRegistro.remove(0);
            //}
            //while (controlDropListNivel1_ModalAgregarRegistro.options.length > 0) {
            //    controlDropListNivel1_ModalAgregarRegistro.remove(0);
            //}

        }


    </script>
</asp:Content>
