<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CajaF.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.CajaF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        function grisarClick() {
            if (Page_ClientValidate("BusquedaGroup_2")) {
                var btn = document.getElementById("<%= this.lbtnAgregarCaja.ClientID %>");
                btn.setAttribute("disabled", "disabled");
                btn.firstChild.className = "fa fa-spinner fa-spin";
            }
        }
    </script>
    <div class="main">
        <%--<div class="container">--%>

        <div>
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Valores > Caja</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>

                    <div class="widget-content">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 10%">
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion    <span class="caret"></span></button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <asp:LinkButton ID="lbtnaTarjeta" runat="server" Visible="false" data-toggle="modal" href="#modalConfirmacion">Mover caja Efectivo - Tarjeta</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lbtnaEfectivo" runat="server" Visible="false" data-toggle="modal" href="#modalConfirmacion2">Mover caja Tarjeta - Efectivo</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lbtnExportar" runat="server" OnClick="lbtnExportar_Click">Exportar a Excel</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lbtnGastos" runat="server" OnClick="lbtnGastos_Click">Reporte Gastos</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lbtnImprimirCaja" runat="server" OnClick="lbtnImprimirCaja_Click">Imprimir Caja</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lbtnResumen" runat="server" OnClick="lbtnResumen_Click">Resumen de caja</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lbtnInformeCierres" runat="server" OnClick="lbtnInformeCierres_Click">Informe cierres de caja</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lbtnRemesa" Visible="false" runat="server" OnClick="lbtnRemesa_Click">Nota de Remesa</asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                </td>
                                <td style="width: 65%">
                                    <h5>
                                        <asp:Label runat="server" ID="lblParametros" Text="" ForeColor="#cccccc"></asp:Label>
                                    </h5>
                                </td>
                                <asp:PlaceHolder runat="server" ID="phPasarBanco">
                                    <td style="width: 5%">
                                        <div class="shortcuts" style="height: 100%">

                                            <a class="btn btn-primary ui-tooltip" href="#pasarBanco" data-toggle="modal" title data-original-title="Movimiento a Banco" style="width: 100%">
                                                <i class="shortcut-icon icon-exchange "></i>
                                            </a>
                                        </div>
                                    </td>
                                </asp:PlaceHolder>
                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">

                                        <a class="btn btn-primary ui-tooltip" href="#pasarCaja" data-toggle="modal" title data-original-title="Movimiento entre cajas" style="width: 100%">
                                            <i class="shortcut-icon icon-refresh "></i>
                                        </a>
                                    </div>
                                </td>
                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">
                                        <a class="btn btn-primary" data-toggle="modal" data-placement="top" title="Tooltip on top" href="#modalBusqueda" style="width: 100%">

                                            <i class="shortcut-icon icon-filter"></i>
                                        </a>
                                    </div>
                                </td>
                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">
                                        <asp:HiddenField ID="hiddenPermiso" runat="server" />
                                        <a class="btn btn-primary ui-tooltip" title data-original-title="Agregar Movimiento" onclick="javascript: validarPermisoAgregarMovimientoCaja()" style="width: 100%">
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


                <div class="widget big-stats-container stacked">

                    <div class="widget-content">

                        <div id="big_stats" class="cf">
                            <div class="stat">
                                <h4>Saldo</h4>
                                <asp:Label ID="lblSaldo" runat="server" Text="" class="value"></asp:Label>
                            </div>
                            <!-- .stat -->
                        </div>

                    </div>
                    <!-- /widget-content -->

                </div>
                <!-- /widget -->


            </div>
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked widget-table action-table">

                    <div class="widget-header">
                        <i class="icon-money" style="width: 2%"></i>
                        <h3 style="width: 75%">Movimientos de Caja
                                    
                        </h3>

                    </div>
                    <div class="widget-content">
                        <div class="panel-body">

                            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div class="table-responsive">
                                        <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#Comentario">Agregar Tipo Cliente</a>
                                        <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                            <thead>
                                                <tr>
                                                    <th style="width: 15%">Fecha</th>
                                                    <th style="width: 40%">Descripcion</th>
                                                    <th style="width: 15%">Importe</th>
                                                    <th style="width: 15%">Tipo</th>
                                                    <th style="width: 15%"></th>
                                                </tr>

                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="phCaja" runat="server"></asp:PlaceHolder>
                                            </tbody>
                                        </table>

                                    </div>


                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>


                        <!-- /.content -->

                    </div>

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
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-md-4">Desde</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtFechaDesde" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Hasta</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtFechaHasta" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <!-- /input-group -->

                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Sucursal</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control" OnSelectedIndexChanged="DropListSucursal_SelectedIndexChanged" disabled AutoPostBack="true"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Punto Venta</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListPuntoVenta" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListPuntoVenta" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Tipo</label>
                                        <div class="col-md-6">

                                            <asp:DropDownList ID="ListTipos" runat="server" class="form-control">
                                                <asp:ListItem Value="-1">Seleccione...</asp:ListItem>
                                                <asp:ListItem Value="0">Todos</asp:ListItem>
                                                <asp:ListItem Value="1">Efectivo</asp:ListItem>
                                                <asp:ListItem Value="2">Cheque</asp:ListItem>
                                                <asp:ListItem Value="3">Transferencia</asp:ListItem>
                                                <asp:ListItem Value="5">Tarjeta</asp:ListItem>
                                                <asp:ListItem Value="8">Credito</asp:ListItem>
                                                <asp:ListItem Value="9">Pagare</asp:ListItem>
                                            </asp:DropDownList>

                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListTipos" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Tipo Movimiento</label>
                                        <div class="col-md-6">

                                            <asp:DropDownList ID="ListMovimiento" runat="server" class="form-control">
                                                <asp:ListItem Value="-1">Seleccione...</asp:ListItem>
                                                <asp:ListItem Value="0">Todos</asp:ListItem>
                                                <asp:ListItem Value="1">Cobro</asp:ListItem>
                                                <asp:ListItem Value="2">Gasto</asp:ListItem>
                                                <asp:ListItem Value="3">Pago</asp:ListItem>
                                                <asp:ListItem Value="4">Cierre</asp:ListItem>
                                                <asp:ListItem Value="5">Diferencia Caja</asp:ListItem>
                                                <asp:ListItem Value="6">Traspaso de Caja</asp:ListItem>
                                                <asp:ListItem Value="8">Traspaso Efectivo Tarjeta</asp:ListItem>
                                            </asp:DropDownList>

                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListTipos" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
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

        <div id="modalAgregar" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Agregar</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>

                                    <div class="form-group">
                                        <label class="col-md-4">Fecha Del Movimiento</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtFechaModalAgregarCaja" runat="server" class="form-control" onkeypress="return false" onchange="javascript:return validarQueLaFechaSeaMenorOIgualALaActual(event)"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-md-4">Sucursal</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListSucursal2" disabled runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListSucursal2_SelectedIndexChanged"></asp:DropDownList>

                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup_2" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Punto Venta</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListPuntoVenta2" runat="server" class="form-control"></asp:DropDownList>

                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListPuntoVenta2" InitialValue="-1" ValidationGroup="BusquedaGroup_2" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Tipo de Movimiento</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtBusquedaTipoMov" runat="server" class="form-control" placeholder="Buscar..." />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:LinkButton ID="lbtnBuscarTipoMovNombre" runat="server" class="btn btn-info" Text="<i class='shortcut-icon icon-search'></i>" OnClick="lbtnBuscarTipoMovNombre_Click" />
                                        </div>
                                    </div>
                                           <%--<div class="form-group">
                                               <asp:Label ID="lblCajaOrigen" CssClass="col-md-4" Visible="false" Text="text" runat="server" />
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="listCajaOrigen" Visible="false" runat="server" class="form-control"></asp:DropDownList>

                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="listCajaOrigen" InitialValue="-1" ValidationGroup="BusquedaGroup_2" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>--%>
                                    <div class="form-group">
                                        <label class="col-md-4"></label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListTipos" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListTipos" InitialValue="-1" ValidationGroup="BusquedaGroup_2" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Importe</label>
                                        <div class="col-md-6">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtImporte" runat="server" class="form-control" Style="text-align: right;" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtImporte" ValidationGroup="BusquedaGroup_2" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Comentario</label>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="txtComentarios" runat="server" class="form-control" col="6" TextMode="MultiLine"></asp:TextBox>

                                        </div>

                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnAgregarCaja" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" ValidationGroup="BusquedaGroup_2" OnClick="lbtnAgregarCaja_Click" OnClientClick="grisarClick();" />
                    </div>
                </div>

            </div>
        </div>

        <div id="pasarCaja" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Movimientos entre cajas</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-md-4">Sucursal Origen</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListSucursalOrigen" disabled runat="server" class="form-control" OnSelectedIndexChanged="ListSucursalOrigen_SelectedIndexChanged"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListSucursalOrigen" InitialValue="-1" ValidationGroup="TrasGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Punto Venta Origen</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListPuntoVentaOrigen" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListPuntoVentaOrigen" InitialValue="-1" ValidationGroup="TrasGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Sucursal Destino</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListSucursalDestino" AutoPostBack="true" runat="server" class="form-control" OnSelectedIndexChanged="ListSucursalDestino_SelectedIndexChanged"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListSucursalDestino" InitialValue="-1" ValidationGroup="TrasGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Punto VentaDestino</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListPuntoVentaDestino" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListPuntoVentaDestino" InitialValue="-1" ValidationGroup="TrasGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Importe</label>
                                        <div class="col-md-6">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtImporteTraspaso" runat="server" class="form-control" Style="text-align: right;" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtImporteTraspaso" ValidationGroup="TrasGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <!-- /input-group -->
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Observacion</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtObservacionTraspaso" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                        </div>
                                        <!-- /input-group -->
                                    </div>
                                    <div class="form-group">
                                        <label for="validateSelect" class="col-md-4">Adjunta imagen</label>
                                        <div class="col-md-6">
                                            <asp:FileUpload ID="FileUpload1" runat="server" />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RegularExpressionValidator ID="uplValidator" runat="server" ControlToValidate="FileUpload1" ValidationGroup="TrasGroup" ForeColor="Red" ErrorMessage="* .jpg o .png"
                                                ValidationExpression="(.+\.([Jj][Pp][Gg])|.+\.([Pp][Nn][Gg])|.+\.([Gg][Ii][Ff]))"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>


                    <div class="modal-footer">
                        <asp:Button ID="btnAgregarTraspaso" runat="server" Text="Agregar" class="btn btn-success" ValidationGroup="TrasGroup" OnClick="btnAgregarTraspaso_Click" />
                    </div>
                </div>

            </div>
        </div>

        <div id="pasarBanco" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Movimientos a Banco</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanelMovimientoBanco" runat="server">
                                <ContentTemplate>
                                    <fieldset>
                                        <div class="form-group">
                                            <label class="col-md-4">Sucursal Origen</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListSucursalOrigenMovimientoBanco" disabled runat="server" class="form-control" OnSelectedIndexChanged="ListSucursalOrigenMovimientoBanco_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            <%--<div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListSucursalOrigenMovimientoBanco" InitialValue="-1" ValidationGroup="MovimientoBancoGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>--%>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Punto de Venta Origen</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListPuntoVentaOrigenMovimientoBanco" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                            <%-- <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListPuntoVentaOrigenMovimientoBanco" InitialValue="-1" ValidationGroup="MovimientoBancoGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>--%>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Empresa Destino</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListEmpresaDestinoMovimientoBanco" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListEmpresaDestinoMovimientoBanco_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            <%--<div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListEmpresaDestinoMovimientoBanco" InitialValue="-1" ValidationGroup="MovimientoBancoGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>--%>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Cuenta Destino</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListCuentaDestinoMovimientoBanco" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                            <%--<div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListCuentaDestinoMovimientoBanco" InitialValue="-1" ValidationGroup="MovimientoBancoGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>--%>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Importe</label>
                                            <div class="col-md-6">
                                                <div class="input-group">
                                                    <span class="input-group-addon">$</span>
                                                    <asp:TextBox ID="txtImporteMovimientoBanco" runat="server" class="form-control" Style="text-align: right;" onkeypress="javascript:return validarNro(event)" Text="0.00"></asp:TextBox>
                                                </div>
                                            </div>
                                            <%--<div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtImporteMovimientoBanco" ValidationGroup="MovimientoBancoGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>--%>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Observacion</label>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txtObservacionMovimientoBanco" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                            </div>
                                        </div>
                                        <%--<div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Adjunta imagen</label>
                                            <div class="col-md-6">
                                                <asp:FileUpload ID="FileUploadMovimientoBanco" runat="server" />
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="FileUploadMovimientoBanco" ValidationGroup="MovimientoBancoGroup" ForeColor="Red" ErrorMessage="* .jpg o .png"
                                                    ValidationExpression="(.+\.([Jj][Pp][Gg])|.+\.([Pp][Nn][Gg])|.+\.([Gg][Ii][Ff]))"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>--%>
                                    </fieldset>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAgregarMovimientoBanco" runat="server" Text="Agregar" class="btn btn-success" ValidationGroup="MovimientoBancoGroup" OnClick="btnAgregarMovimientoBanco_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalConfirmacion" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Confirmacion de Movimiento</h4>
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
                                        <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea pasar Mov. de Efectivo a Tarjeta?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>

                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="txtMovimiento" Text="0" Style="display: none"></asp:TextBox>
                                </div>
                            </div>
                            <%--                                <div class="form-group">
                                    
                                </div>--%>
                        </div>


                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnaTarjeta" Text="Confirmar" class="btn btn-info" OnClick="btnaTarjeta_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>

                            <%--                        <asp:LinkButton ID="LinkButton1" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" ValidationGroup="BusquedaGroup" />--%>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <div id="modalConfirmacion2" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Confirmacion de Movimiento</h4>
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
                                        <asp:Label runat="server" ID="Label1" Text="Esta seguro que desea pasar Mov. de Tarjeta a Efectivo?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>

                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="TextBox1" Text="0" Style="display: none"></asp:TextBox>
                                </div>
                            </div>
                            <%--                                <div class="form-group">
                                    
                                </div>--%>
                        </div>


                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnaEfectivo" Text="Confirmar" class="btn btn-success" OnClick="btnaEfectivo_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>

                            <%--                        <asp:LinkButton ID="LinkButton1" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" ValidationGroup="BusquedaGroup" />--%>
                        </div>
                    </div>

                </div>
            </div>
        </div>


        <div id="Comentario" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Comentario</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div style="display: none" class="form-group">
                                <label class="col-md-4">Numero Comentario</label>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtNumero" runat="server" class="form-control"></asp:TextBox>

                                </div>

                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Comentario</label>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtComentario2" runat="server" class="form-control" TextMode="MultiLine" Columns="6"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ControlToValidate="txtComentario2" SetFocusOnError="true" ForeColor="Red" ID="RequiredFieldValidator16" runat="server" ErrorMessage="<h3>*</h3>"></asp:RequiredFieldValidator>  --%>
                                </div>

                            </div>

                        </div>
                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="btnActualizarComentario" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnActualizarComentario_Click" />
                    </div>
                </div>

            </div>
        </div>

        <script>

            function abrirdialog(comentario, numero) {
                document.getElementById('<%= txtComentario2.ClientID %>').value = comentario;
                document.getElementById('<%= txtNumero.ClientID %>').value = numero;
                document.getElementById('abreDialog').click();
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
                    if (key == 46 || key == 8 || key == 44)// Detectar . (punto) y backspace (retroceso) y , (coma)
                    { return true; }
                    else { return false; }
                }
                return true;
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
                $("#<%= txtFechaModalAgregarCaja.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
                $("#<%= txtFechaModalAgregarCaja.ClientID %>").val(obtenerFechaActual_ddMMyyyyEnString());
            });
        </script>

        <!-- Page-Level Plugin Scripts - Tables -->
        <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
        <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
        <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

        <script src="../../js/Funciones.js"></script>
        <script src="../../js/moment/moment.js"></script>

        <script>
            function validarPermisoAgregarMovimientoCaja() {
                var hiddenPerm = document.getElementById('<%= hiddenPermiso.ClientID%>');
                var permvalue = hiddenPerm.value;
                if (permvalue == 0) {
                    //alert("Acceso Denegado!!");
                    window.location = "../../default.aspx?m=1";
                }
                else {
                    $('#modalAgregar').modal();
                }
            }

            function validarQueLaFechaSeaMenorOIgualALaActual(e) {
                var txtFechaModalAgregarCaja = $("#<%= txtFechaModalAgregarCaja.ClientID %>");

                var fechaSeleccionada = moment(txtFechaModalAgregarCaja.val(), "DD/MM/YYYY").format("MM/DD/YYYY");
                fechaSeleccionada = new Date(fechaSeleccionada);
                var hoyString = obtenerFechaActual_ddMMyyyyEnString();
                var hoy = new Date(moment(hoyString, "DD/MM/YYYY").format("MM/DD/YYYY"));

                if (fechaSeleccionada > hoy) {
                    txtFechaModalAgregarCaja.val(hoyString);
                }

                if (isNaN(fechaSeleccionada)) {
                    txtFechaModalAgregarCaja.val(hoyString);
                }
            }
        </script>

    </div>

</asp:Content>
