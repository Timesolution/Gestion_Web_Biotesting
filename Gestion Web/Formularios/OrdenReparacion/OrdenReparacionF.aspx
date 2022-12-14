<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrdenReparacionF.aspx.cs" Inherits="Gestion_Web.Formularios.OrdenReparacion.OrdenReparacionF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div>
            <div class="col-md-12 col-xs-12 hidden-print">
                <div class="widget stacked">

                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Ventas > Ordenes de Reparacion</h5>
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
                                                <asp:LinkButton ID="lbtnAnular" Visible="false" runat="server" data-toggle="modal" href="#modalConfirmacion">Anular</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lbtnEtiqueta" runat="server" OnClick="lbtnEtiqueta_Click">Imprimir Etiqueta</asp:LinkButton>
                                            </li>
                                            <asp:PlaceHolder ID="ltbnsServiceOficial" Visible="false" runat="server">
                                            <li class="dropdown-submenu">
                                                <a tabindex="-1" href="#">A Service Oficial</a>
                                                <ul class="dropdown-menu">
                                                    <li>
                                                        <asp:LinkButton ID="lbtnSeleccionarServiceOficial" runat="server" data-toggle="modal" href="#modalServiceOficial">Seleccionar Service Oficial</asp:LinkButton>
                                                        <asp:LinkButton ID="lbtnAsignarServiceOficial" runat="server" data-toggle="modal" href="#modalAsignarServiceOficial">Asignar OR a Service Oficial</asp:LinkButton>
                                                        <asp:LinkButton ID="lbtnEnviarASucursalGarantias" runat="server" data-toggle="modal" href="#modalEnviarASucursalGarantias">Reparado - Enviar a Garantias</asp:LinkButton>
                                                        <asp:LinkButton ID="lbtnDevolverSucOrigen" runat="server" data-toggle="modal" href="#modalDevolucionASucursal">Devolver a Sucursal Origen</asp:LinkButton>
                                                        <asp:LinkButton ID="lbtnRecibidoSucOrigen" runat="server" data-toggle="modal" href="#modalSucOrigenRecibeMercaderia">Recibido en sucursal origen</asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </li>
                                            </asp:PlaceHolder>
                                            <li>
                                                <asp:LinkButton ID="lbtnDevolucionProveedor" Visible="false" runat="server" data-toggle="modal" href="#modalDevolucionProveedor">Devolucion a proveedor</asp:LinkButton>
                                            </li>
                                            <asp:PlaceHolder ID="lbtnsRepararLocalmente" Visible="false" runat="server">
                                            <li class="dropdown-submenu">
                                                <a tabindex="-1" href="#">Reparar localmente</a>
                                                <ul class="dropdown-menu">
                                                    <li>
                                                        <asp:LinkButton ID="lbtnEnviarARepararLocalmente" runat="server" data-toggle="modal" href="#modalEnviarARepararLocalmente">Enviar a sucursal de reparacion</asp:LinkButton>
                                                        <asp:LinkButton ID="lbtnRepararLocalmente" runat="server" data-toggle="modal" href="#modalEnReparacion">En Reparacion</asp:LinkButton>
                                                        <asp:LinkButton ID="lbtnReparado" runat="server" data-toggle="modal" href="#modalReparado">Reparado</asp:LinkButton>
                                                        <asp:LinkButton ID="lbtnDevolverASucursal" runat="server" data-toggle="modal" href="#modalDevolucionASucursal">Devolver a Sucursal Origen</asp:LinkButton>
                                                        <asp:LinkButton ID="lbtnSucOrigenRecibeProducto" runat="server" data-toggle="modal" href="#modalSucOrigenRecibeMercaderia">Recibido en sucursal origen</asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </li>
                                            </asp:PlaceHolder>
                                            <asp:PlaceHolder ID="lbtnsFinalizarOR" Visible="false" runat="server">
                                            <li class="dropdown-submenu">
                                                <a tabindex="-1" href="#">Finalizar Orden Reparacion</a>
                                                <ul class="dropdown-menu">
                                                    <li>
                                                        <asp:LinkButton ID="lbtnRetiraCliente" runat="server" data-toggle="modal" href="#modalRetiraCliente">Retira Cliente</asp:LinkButton>
                                                        <asp:LinkButton ID="lbtnFinalizada" runat="server" data-toggle="modal" href="#modalFinalizada">Finalizada</asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </li>
                                            </asp:PlaceHolder>
                                            <li>
                                                <asp:LinkButton ID="lbtnEnviarSMS" Visible="false" runat="server" OnClick="lbtnEnviarSMS_Click">Avisar al Cliente</asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                </td>
                                <td style="width: 65%"></td>
                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">
                                        <a class="btn btn-primary" data-toggle="modal" href="#modalNro" style="width: 100%">
                                            <i class="shortcut-icon icon-search"></i>
                                        </a>
                                    </div>
                                </td>
                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">
                                        <a class="btn btn-primary" data-toggle="modal" href="#modalBusqueda" style="width: 100%">
                                            <i class="shortcut-icon icon-filter"></i>
                                        </a>
                                    </div>
                                </td>
                                <%--<td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">
                                        <a href="OrdenReparacionABM.aspx" class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                            <i class="shortcut-icon icon-plus"></i>
                                        </a>
                                    </div>
                                </td>--%>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div class="col-md-12 col-xs-12">
                <div class="widget widget-table">

                    <div class="widget-header">
                        <i class="icon-th-list" style="width: 2%"></i>
                        <h3 style="width: 75%">Ordenes de Reparacion</h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">
                            <div class="table-responsive">
                                <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalRemitoDetalle">Agregar Tipo Cliente</a>
                                <table class="table table-striped table-bordered table-hover" id="dataTablesR-example">
                                    <thead>
                                        <tr>
                                            <th>Fecha</th>
                                            <th>Numero Orden</th>
                                            <th>Numero Serie</th>
                                            <th>Sucursal</th>
                                            <th>PRP/Factura</th>
                                            <th>Fecha Compra</th>
                                            <th>Cliente</th>
                                            <th>Plazo</th>
                                            <th>Tope Reparacion</th>
                                            <th>Estado</th>
                                            <th>Barra de progreso</th>
                                            <th>Sucursal OR</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phOrdenReparacion" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                            </div>
                        </div>
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
                                        <asp:Label runat="server" ID="Label1" Text="Esta seguro que desea eliminar las ordenes de reparacion seleccionadas?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>

                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="TextBox1" Text="0" Style="display: none"></asp:TextBox>
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

        <div id="modalDevolucionProveedor" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Confirmacion de Devolucion a Proveedor</h4>
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
                                        <asp:Label runat="server" ID="Label3" Text="Esta seguro que desea enviar la orden de reparacion al proveedor?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>

                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="TextBox3" Text="0" Style="display: none"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnSiDevolucionProveedor" Text="Enviar" class="btn btn-success" OnClick="btnSiDevolucionProveedor_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalEnviarARepararLocalmente" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Confirmacion de Envio a Reparacion</h4>
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
                                        <asp:Label runat="server" ID="Label2" Text="Esta seguro que desea enviar la orden de reparacion a reparacion local?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>

                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="TextBox2" Text="0" Style="display: none"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnSiEnviarAReparacionLocalmente" Text="Enviar" class="btn btn-success" OnClick="btnSiEnviarAReparacionLocalmente_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalEnviarASucursalGarantias" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Confirmacion de Envio a Sucursal Garantias</h4>
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
                                        <asp:Label runat="server" ID="Label10" Text="Esta seguro que desea enviar el articulo a la sucursal de garantias?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>

                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="TextBox10" Text="0" Style="display: none"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnEnviarASucursalGarantias" Text="Enviar" class="btn btn-success" OnClick="btnEnviarASucursalGarantias_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalRetiraCliente" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Confirmacion Retira Cliente</h4>
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
                                        <asp:Label runat="server" ID="Label7" Text="El producto sera retirado por el cliente?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>

                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="TextBox7" Text="0" Style="display: none"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="name" class="col-md-4">Observacion Articulo</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtObservacionRetiraCliente" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtObservacionRetiraCliente" ValidationGroup="ObservacionGrupoRetiraCliente" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel5" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btnRetiraCliente" Text="Si" class="btn btn-success" ValidationGroup="ObservacionGrupoRetiraCliente" OnClick="btnRetiraCliente_Click" />
                                    <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalFinalizada" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Confirmacion Finalizada</h4>
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
                                        <asp:Label runat="server" ID="Label8" Text="Finalizar orden de reparacion?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>

                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="TextBox8" Text="0" Style="display: none"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="name" class="col-md-4">Observacion Articulo</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtObservacionFinalizada" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtObservacionFinalizada" ValidationGroup="ObservacionGrupoFinalizada" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnFinalizada" Text="Si" class="btn btn-success" ValidationGroup="ObservacionGrupoFinalizada" OnClick="btnFinalizada_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalEnReparacion" class="modal fade" tabindex="-1" role="dialog">
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
                                        <asp:Label runat="server" ID="Label4" Text="Esta seguro que desea cambiar el estado de la orden de reparacion a 'En Reparacion'?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>

                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="TextBox4" Text="0" Style="display: none"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnSiReparacionLocalmente" Text="Enviar" class="btn btn-success" OnClick="btnSiReparacionLocalmente_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalReparado" class="modal fade" tabindex="-1" role="dialog">
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
                                        <asp:Label runat="server" ID="Label5" Text="Esta seguro que desea cambiar el estado de la orden a 'Reparado' ?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>

                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="TextBox5" Text="0" Style="display: none"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnReparado" Text="Enviar" class="btn btn-success" OnClick="btnReparado_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalSucOrigenRecibeMercaderia" class="modal fade" tabindex="-1" role="dialog">
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
                                        <asp:Label runat="server" ID="Label9" Text="Producto recibido en la sucursal de origen?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>

                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="TextBox9" Text="0" Style="display: none"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnSucOrigenRecibeMercaderia" Text="Si" class="btn btn-success" OnClick="btnSucOrigenRecibeMercaderia_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalDevolucionASucursal" class="modal fade" tabindex="-1" role="dialog">
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
                                        <asp:Label runat="server" ID="Label6" Text="Esta seguro que desea enviar al producto a la sucursal de origen?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>

                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="TextBox6" Text="0" Style="display: none"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnDevolverASucursalOrigen" Text="Enviar" class="btn btn-success" OnClick="btnDevolverASucursalOrigen_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalAsignarServiceOficial" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Asignar OR al Service Oficial</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-4">Fecha a Reparar en Service Oficial</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtFechaReparar" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaReparar" ValidationGroup="ServiceOficial" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Numero Orden Reparacion</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNumOrdenReparacion" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtNumOrdenReparacion" ValidationGroup="ServiceOficial" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Plazo estimado de reparacion</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPlazoEstimadoReparacion" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtPlazoEstimadoReparacion" ValidationGroup="ServiceOficial" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnAgregarOrdenReparacionServicioTecnico" Text="Guardar" class="btn btn-success" ValidationGroup="ServiceOficial" OnClick="btnAgregarOrdenReparacionServicioTecnico_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                        </div>
                        <%--<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                
                            </ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="modalServiceOficial" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog" style="width: 60%;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Elegir Service Oficial</h4>
                </div>
                <div class="modal-body">


                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel7" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnBuscarServicioTecnico">
                                    <div class="form-group">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Buscar Servicio Tecnico</label>
                                                <asp:Label runat="server" ID="lblIdServicioTecnico" Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="lblIdOrdenReparacion" Visible="false"></asp:Label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtServicioTecnico" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:LinkButton ID="btnBuscarServicioTecnico" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarServicioTecnico_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <table class="table table-striped table-bordered">
                                        <thead>
                                            <tr>
                                                <th>Nombre</th>
                                                <th>Direccion</th>
                                                <th>Observaciones</th>
                                                <th>Marcas</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:PlaceHolder ID="phServicioTecnico" runat="server"></asp:PlaceHolder>
                                        </tbody>
                                    </table>

                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnGuardarServiceOficial" Text="Guardar" class="btn btn-success" OnClick="btnGuardarServiceOficial_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                    </div>

                </div>
            </div>
        </div>
    </div>

    <div id="modalNro" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:Panel ID="p" runat="server" DefaultButton="btnBuscarNumeros">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Busqueda</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-4">N° de Orden</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNumeroOrden" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnBuscarNumeros" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscarNumeros_Click" />
                    </div>
                </asp:Panel>
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
                    <asp:UpdatePanel runat="server" ID="asdasd">
                        <ContentTemplate>
                            <div role="form" class="form-horizontal col-md-12">
                                <div class="form-group">
                                    <label class="col-md-4">Desde</label>
                                    <div class="col-md-4">

                                        <asp:TextBox ID="txtFechaDesde" runat="server" class="form-control"></asp:TextBox>

                                        <!-- /input-group -->
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
                                    <!-- /input-group -->

                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Sucursal</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                                        <!-- /input-group -->
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Sucursal OR</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListSucursalOR" runat="server" class="form-control"></asp:DropDownList>
                                        <!-- /input-group -->
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursalOR" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Cod Cliente</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtCodCliente" class="form-control" runat="server"></asp:TextBox>
                                    </div>

                                    <div class="col-md-2">
                                        <asp:LinkButton ID="btnBuscarCod" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCod_Click" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Cliente</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListClientes" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListClientes" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Estado</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListEstados" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEstados" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>                                
                                <div class="form-group">
                                    <label class="col-md-4">Desc Articulo</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtDescArticulo" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:LinkButton ID="btnBuscarCodArt" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCodArt_Click" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Articulo</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListArticulo" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListArticulo" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" OnClick="lbtnBuscar_Click" class="btn btn-success" ValidationGroup="BusquedaGroup" />
                </div>
            </div>
        </div>
    </div>

    <link href="../../css/pages/reports.css" rel="stylesheet">

    <%--<script>

            function abrirdialog() {
                document.getElementById('abreDialog').click();
            }

        </script>--%>

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
    <script src="../../js/daypilot-modal-2.0.js"></script>
    <script>


        $(function () {
            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        function openModalAgregarServiceOficial() {
            $('#modalServiceOficial').modal('show');
        }

        $(function () {
            $("#<%= txtFechaReparar.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

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
                    if (key == 8)// Detectar . (punto) y backspace (retroceso) y , (coma)
                    { return true; }
                    else { return false; }
                }
                return true;
            }

    </script>

    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

</asp:Content>
