<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PedidosP.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.PedidosP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div class="col-md-12 col-xs-12 hidden-print">
            <div class="widget stacked">

                <div class="stat">
                    <h5><i class="icon-map-marker"></i>&nbsp Ventas > Pedidos</h5>
                </div>

                <div class="widget-header">
                    <i class="icon-wrench"></i>
                    <h3>Herramientas</h3>
                </div>
                <!-- /widget-header -->

                <div class="widget-content">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 20%">
                                <asp:PlaceHolder ID="phAcciones" runat="server">
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion    <span class="caret"></span></button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <asp:LinkButton ID="lbtnRemitir" runat="server" OnClick="lbtnRemitir_Click" Visible="true">Remitir</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lbtnAnular" runat="server" data-toggle="modal" href="#modalConfirmacion">Anular</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnAutorizar" runat="server" OnClick="btnAutorizar_Click">Autorizar</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lbtnFacturar" runat="server" OnClick="btnFacturar_Click" Visible="false">Facturar</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lbtnFacturarFamilia" runat="server" OnClick="lbtnFacturarFamilia_Click" Visible="false">Facturar por Grupo</asp:LinkButton>
                                            </li>
                                            <li>
                                                <a data-toggle="modal" href="#modalBultos">Impresion Bultos</a>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnModula" runat="server" OnClick="btnModula_Click">Enviar Modula</asp:LinkButton>
                                            </li>
                                            <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Consolidados</a>
                                                <ul class="dropdown-menu">
                                                    <li>
                                                        <asp:LinkButton ID="ltbnConsolidados" runat="server" OnClick="ltbnConsolidados_Click">
                                                                    <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                                    &nbsp Exportar
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lbtnConsolidadosPDF" runat="server" OnClick="lbtnConsolidadosPDF_Click">
                                                                    <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                                    &nbsp Imprimir
                                                        </asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </li>
                                        </ul>
                                    </div>
                                </asp:PlaceHolder>
                                <!-- /btn-group -->
                            </td>



                            <td style="width: 60%">
                                <h5>
                                    <asp:Label runat="server" ID="lblParametros" Text="" ForeColor="#cccccc"></asp:Label>
                                </h5>
                            </td>
                            <td style="width: 5%">
                                <div class="shortcuts" style="height: 100%">
                                    <a class="btn btn-primary" data-toggle="modal" href="#modalNro" style="width: 100%">
                                        <i class="shortcut-icon icon-search"></i>
                                    </a>
                                </div>
                            </td>
                            <td>
                                <div class="btn-group" style="width: 100%">
                                    <button type="button" class="btn btn-primary dropdown-toggle ui-tooltip" title data-original-title="Listados" data-toggle="dropdown" style="width: 100%">
                                        <i class="shortcut-icon icon-print"></i>&nbsp
                                            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                        <span class="caret"></span>
                                    </button>
                                    <ul class="dropdown-menu" role="menu">
                                        <li>
                                            <a href="#modalCantidadPendiente" data-toggle="modal">Cantidad en Pendientes</a>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="LinkButton1" runat="server" Text="Exportar pedidos" Style="width: 100%" OnClick="LinkButton1_Click" />
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbtnExportarFamilia" runat="server" Text="Exportar pedidos por grupo" Style="width: 100%" OnClick="lbtnExportarFamilia_Click" />
                                        </li>
                                    </ul>
                                </div>
                            </td>
                            <td style="width: 5%">
                                <div class="shortcuts" style="height: 100%">
                                    <a class="btn btn-primary" data-toggle="modal" href="#modalFamilia" style="width: 100%">
                                        <i class="shortcut-icon icon-group"></i>
                                    </a>
                                </div>
                            </td>
                            <td style="width: 5%" >
                                <div class="shortcuts" style="height: 100%">
                                    <a class="btn btn-primary" data-toggle="modal" href="#modalBusqueda" style="width: 100%" runat="server" id="iconoBusqueda">
                                        <i class="shortcut-icon icon-filter"></i>
                                    </a>
                                </div>
                            </td>
                            <td style="width: 5%">
                                <div class="shortcuts" style="height: 100%">
                                    <a href="ABMPedidos.aspx" class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                        <i class="shortcut-icon icon-plus"></i>
                                    </a>
                                </div>
                            </td>


                        </tr>

                    </table>
                </div>
                <!-- /widget-content -->

            </div>
            <!-- /widget -->
        </div>

        <div class="col-md-12">
            <div class="widget big-stats-container stacked">
                <div class="widget-content">
                    <asp:PlaceHolder ID="phSaldo" runat="server" Visible="true">
                        <div id="big_stats" class="cf">
                            <div class="stat">
                                <h4>Saldo</h4>
                                <asp:Label ID="lblSaldo" runat="server" Text="" class="value" Visible="false"></asp:Label>
                            </div>
                            <!-- .stat -->
                        </div>
                    </asp:PlaceHolder>
                </div>
                <!-- /widget-content -->
            </div>
            <!-- /widget -->
        </div>

        <!-- /span12 -->
        <div class="col-md-12 col-xs-12">
            <div class="widget widget-table">

                <div class="widget-header">
                    <i class="icon-th-list" style="width: 2%"></i>
                    <h3 style="width: 75%">Pedidos
                    </h3>
                    <%--<h3>
                            <asp:Label ID="lblSaldo" runat="server" Style="text-align: end" Text="" ForeColor="#0099ff" Font-Bold="true"></asp:Label>
                        </h3>--%>
                </div>
                <div class="widget-content">
                    <div class="panel-body">

                        <div class="table-responsive">
                            <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalPedidoDetalle">Agregar Tipo Cliente</a>
                            <table class="table table-striped table-bordered table-hover" id="dataTablesP-example">
                                <thead>
                                    <tr>
                                        <th>Fecha</th>
                                        <th>Numero</th>
                                        <th>Razon</th>
                                        <th>Total</th>
                                        <th>Estado</th>
                                        <th></th>
                                    </tr>

                                </thead>
                                <tbody>
                                    <asp:PlaceHolder ID="phPedidos" runat="server"></asp:PlaceHolder>
                                </tbody>
                            </table>

                        </div>
                    </div>


                    <!-- /.content -->

                </div>

            </div>
        </div>

        <div id="modalBusqueda" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Busqueda</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-md-3">
                                            Fecha Pedido
                                        </label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaDesde" placeholder="Desde" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaHasta" placeholder="Hasta" runat="server" class="form-control"></asp:TextBox>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RadioButton ID="RadioFechaPedido" Checked="true" runat="server" GroupName="fecha" />
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">
                                            Fecha Entrega
                                        </label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaEntregaDesde" placeholder="Desde" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaEntregaHasta" placeholder="Hasta" runat="server" class="form-control"></asp:TextBox>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RadioButton ID="RadioFechaEntrega" runat="server" GroupName="fecha" />
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaEntregaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaEntregaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-md-3">Tipo Entrega</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListTipoEntrega" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListTipoEntrega" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Sucursal</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Cliente</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtCodCliente" class="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <asp:LinkButton ID="btnBuscarCod" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCod_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Cliente</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListClientes" runat="server" class="form-control" OnSelectedIndexChanged="DropListClientes_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListClientes" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <%-- <div class="form-group">
                                        <label class="col-md-3">Descripcion Articulo</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtDescripcionArticulo" class="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <asp:LinkButton ID="lbtnBuscarArticulo" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="lbtnBuscarArticulo_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Articulo</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListArticulos" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListArticulos" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>--%>
                                    <div class="form-group">
                                        <label class="col-md-3">Vendedor</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListVendedor" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ListVendedor_SelectedIndexChanged"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListVendedor" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Estado</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListEstado" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEstado" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" ValidationGroup="BusquedaGroup" />
                    </div>
                </div>

            </div>
        </div>

        <div id="modalNro" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Busqueda</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-4">N° Cliente</label>
                                <div class="col-md-4">

                                    <asp:TextBox ID="txtCodigoCliente" runat="server" class="form-control"></asp:TextBox>

                                    <!-- /input-group -->
                                </div>

                            </div>
                            <div class="form-group">
                                <label class="col-md-4">N° Pedido</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNumeroPedido" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <%--<div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>--%>
                                <!-- /input-group -->

                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Buscar por observacion</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtObservacion" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnBuscarNumeros" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscarNumeros_Click" />
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
                                        <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar los pedidos seleccionados?" Style="text-align: center"></asp:Label>
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

                            <%--                        <asp:LinkButton ID="LinkButton1" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" ValidationGroup="BusquedaGroup" />--%>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <div id="modalCantidadPendiente" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Reporte cantidad</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-md-4">Desde</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtFechaDesdeCantidad" runat="server" class="form-control" />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtFechaDesdeCantidad" runat="server" ValidationGroup="CantidadGroup" ForeColor="Red" Font-Bold="true" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Hasta</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtFechaHastaCantidad" runat="server" class="form-control" />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtFechaHastaCantidad" runat="server" ValidationGroup="CantidadGroup" ForeColor="Red" Font-Bold="true" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Sucursal</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListSucursalCantidad" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ErrorMessage="*" InitialValue="-1" ControlToValidate="ListGruposCantidad" runat="server" ValidationGroup="CantidadGroup" ForeColor="Red" Font-Bold="true" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Grupo</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListGruposCantidad" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="ListGruposCantidad" runat="server" ValidationGroup="CantidadGroup" ForeColor="Red" Font-Bold="true" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Cliente</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtClienteCantidad" class="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <asp:LinkButton ID="btnBuscarClienteCantidad" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarClienteCantidad_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Clientes</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListClientesCantidad" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="ListClientesCantidad" runat="server" ValidationGroup="CantidadGroup" ForeColor="Red" Font-Bold="true" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Proveedor</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtProveedorCantidad" class="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <asp:LinkButton ID="btnBuscarProveedorCantidad" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarProveedorCantidad_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Proveedores</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListProveedoresCantidad" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="ListProveedoresCantidad" runat="server" ValidationGroup="CantidadGroup" ForeColor="Red" Font-Bold="true" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Descripcion Articulo</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtArticuloCantidad" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:LinkButton ID="lbtnArticuloCantidad" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="lbtnBuscarArticulo_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Articulo</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListArticulosCantidad" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="DropListArticulosCantidad" runat="server" ValidationGroup="CantidadGroup" ForeColor="Red" Font-Bold="true" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Zona Entrega</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListZonaEntregaCantidad" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="lbtnImprimirCantidadNeto" runat="server" class="btn btn-success" Text="Imprimir" OnClick="lbtnImprimirCantidadNeto_Click" ValidationGroup="CantidadGroup" />
                            <asp:LinkButton ID="lbtnExportarCantidadNeto" runat="server" class="btn btn-success" Text="Exportar" OnClick="lbtnExportarCantidadNeto_Click" ValidationGroup="CantidadGroup" />

                           
                        </div>
                        <div class="modal-footer">
                             <asp:LinkButton ID="lbtnImprimirPedCliente" runat="server" class="btn btn-success" Text="Imprimir Por Clientes" OnClick="lbtnImprimirPedCliente_Click" ValidationGroup="CantidadGroup" />
                            </div>
                    </div>

                </div>
            </div>
        </div>

        <div id="modalBultos" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Bultos pedido</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-md-4">Cantidad:</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtCantidadBultos" runat="server" class="form-control" TextMode="Number" />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtCantidadBultos" runat="server" ValidationGroup="BultosGroup" ForeColor="Red" Font-Bold="true" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="lbtnCargarBultos" runat="server" class="btn btn-success" Text="Cargar" OnClick="lbtnCargarBultos_Click" ValidationGroup="BultosGroup" />
                            <asp:LinkButton ID="lbtnCargarImprimirBultos" runat="server" class="btn btn-success" Text="Cargar e Imprimir" OnClick="lbtnCargarImprimirBultos_Click" ValidationGroup="BultosGroup" />
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <div id="modalFamilia" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Busqueda</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-md-3">Desde</label>
                                        <div class="col-md-6">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                <asp:TextBox ID="txtFechaDesdeFamilia" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesdeFamilia" ValidationGroup="BusquedaFamiliaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Hasta</label>
                                        <div class="col-md-6">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                <asp:TextBox ID="txtFechaHastaFamilia" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHastaFamilia" ValidationGroup="BusquedaFamiliaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-md-3">Cliente</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtClienteFamilia" class="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <asp:LinkButton ID="lbtnBuscarClienteFamilia" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="lbtnBuscarClienteFamilia_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Cliente</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListClientesFamilia" runat="server" class="form-control" AutoPostBack="True"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListClientesFamilia" InitialValue="-1" ValidationGroup="BusquedaFamiliaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    
                                    <div class="form-group">
                                        <label class="col-md-3">Estado</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListEstadoFamilia" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEstadoFamilia" InitialValue="-1" ValidationGroup="BusquedaFamiliaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnBuscarPedidoFamilia" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnBuscarPedidoFamilia_Click" ValidationGroup="BusquedaFamiliaGroup" />
                    </div>
                </div>

            </div>
        </div>
    </div>
    <%--Fin modalGrupo--%>

    <script>

        function abrirdialog(value) {
            "window.open('ImpresionPedido.aspx?Pedido=" + value + "', 'fullscreen', 'top=0,left=0,width='+(screen.availWidth)+',height ='+(screen.availHeight)+',fullscreen=yes,toolbar=0 ,location=0,directories=0,status=0,menubar=0,resiz able=0,scrolling=0,scrollbars=0');"
        }

    </script>

    <!-- Page-Level Plugin Scripts - Tables -->
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

    <link href="../../css/pages/reports.css" rel="stylesheet">
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

    <%--<script src="//code.jquery.com/jquery-1.9.1.js"></script>--%>
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

    <script>


        $(function () {
            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaDesdeCantidad.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaHastaCantidad.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaEntregaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaEntregaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaDesdeFamilia.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaHastaFamilia.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

    </script>


</asp:Content>
