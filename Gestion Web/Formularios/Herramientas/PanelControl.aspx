<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PanelControl.aspx.cs" Inherits="Gestion_Web.Formularios.Herramientas.PanelControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Herramientas > Panel de control</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Configuraciones</h3>

                    </div>
                    <!-- /.widget-header -->

                    <div class="widget-content">
                        <div class="form-horizontal col-md-12">
                            <fieldset>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Alicuota Presupuesto</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListPorcentajeIVA" runat="server" class="form-control">
                                                    <asp:ListItem Value="-1">Seleccione..</asp:ListItem>
                                                    <asp:ListItem Value="0">0%</asp:ListItem>
                                                    <asp:ListItem Value="10.5">10,5%</asp:ListItem>
                                                    <asp:ListItem Value="21">21%</asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListPorcentajeIVA" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnActualizar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregar_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Mostrar precio venta Articulos</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListPrecioArticulo" runat="server" class="form-control">
                                                    <asp:ListItem Value="-1">Seleccione..</asp:ListItem>
                                                    <asp:ListItem Value="Con Iva" Text="Con Iva"></asp:ListItem>
                                                    <asp:ListItem Value="Sin Iva" Text="Sin Iva"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListPrecioArticulo" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnActualizarPrecio" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregarPrecio_Click" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Editar Descripcion y Precio (Facturacion) :</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListEditarDescripcion" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListEditarDescripcion_SelectedIndexChanged">
                                                    <asp:ListItem Value="-1">Seleccione..</asp:ListItem>
                                                    <asp:ListItem Value="0" Text="No Editable"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Editable"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEditarDescripcion" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnEditarDescripcion" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnEditarDescripcion_Click" />
                                            </div>
                                        </div>

                                        <asp:Panel ID="panelPrecio" runat="server" Visible="false">
                                            <div class="form-group">
                                                <label for="validateSelect" class="col-md-4">Editar precio unitario (Facturacion) :</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="DropListEdicionPrecio" runat="server" class="form-control">
                                                        <asp:ListItem Value="-1">Seleccione..</asp:ListItem>
                                                        <asp:ListItem Value="0" Text="Libre"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Solo aumentar"></asp:ListItem>
                                                    </asp:DropDownList>

                                                </div>
                                                <div class="col-md-2">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEdicionPrecio" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>

                                                <div class="col-md-2">
                                                    <asp:LinkButton ID="lbtnEdicionPrecio" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnEdicionPrecio_Click" />
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Forma Pago Cta Cte Consumidor Final:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListConsumidorFinalCC" runat="server" class="form-control">
                                                    <asp:ListItem Value="-1">Seleccione..</asp:ListItem>
                                                    <asp:ListItem Value="0" Text="No Habilitado"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Habilitado"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListConsumidorFinalCC" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnConsumidorFinalCC" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnConsumidorFinalCC_Click" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Numeracion Pagos:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListNumeracionPagos" runat="server" class="form-control">
                                                    <asp:ListItem Value="-1">Seleccione..</asp:ListItem>
                                                    <asp:ListItem Value="0" Text="Manual"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Autonumerico"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListNumeracionPagos" InitialValue="-1" ValidationGroup="NumeracionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnNumeracionPagos" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnNumeracionPagos_Click" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Numeracion Cobros:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListNumeracionCobros" runat="server" class="form-control">
                                                    <asp:ListItem Value="-1">Seleccione..</asp:ListItem>
                                                    <asp:ListItem Value="0" Text="Manual"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Autonumerico"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListNumeracionCobros" InitialValue="-1" ValidationGroup="NumeracionCobGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnNumeracionCobros" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnNumeracionCobros_Click" ValidationGroup="NumeracionCobGroup" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Separador listas:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListSeparador" runat="server" class="form-control">
                                                    <asp:ListItem Value="0" Text="PuntoComa (;)"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Coma (,)"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnSeparador" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnSeparador_Click" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Egreso de stock:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListEgresoStock" runat="server" class="form-control">
                                                    <asp:ListItem Value="0" Text="Por Factura"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Por Remito"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnEgresoStock" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnEgresoStock_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Max % dto en articulo:</label>
                                            <div class="col-md-4">
                                                <div class="input-group">
                                                    <span class="input-group-addon">%</span>
                                                    <asp:TextBox ID="txtMaxDtoArticulo" runat="server" class="form-control" TextMode="Number" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnMaxDtoArticulo" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnMaxDtoArticulo_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Max % dto en factura:</label>
                                            <div class="col-md-4">
                                                <div class="input-group">
                                                    <span class="input-group-addon">%</span>
                                                    <asp:TextBox ID="txtMaxDtoFc" runat="server" class="form-control" TextMode="Number" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnMaxDtoFc" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnMaxDtoFc_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Numeracion cod. articulos:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListNumeracionArt" runat="server" class="form-control">
                                                    <asp:ListItem Value="0" Text="Manual"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Automatica"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnNumArt" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnNumArt_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Max dias sin aceptar mercaderia</label>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtMaxDiasSinAceptarMercaderia" runat="server" class="form-control" TextMode="Number" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnMaxApertura" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnMaxApertura_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Limite diferencia caja</label>
                                            <div class="col-md-4">
                                                <div class="input-group">
                                                    <span class="input-group-addon">$</span>
                                                    <asp:TextBox ID="txtLimiteDif" runat="server" class="form-control" TextMode="Number" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnLimiteDif" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnLimiteDif_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Items en cero:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListItemsEnCero" runat="server" class="form-control">
                                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnItemsEnCero" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnItemsEnCero_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Alerta precios artículos sin actualizar</label>
                                            <div class="col-md-4">
                                                <div class="input-group">
                                                    <span class="input-group-addon">Días</span>
                                                    <asp:TextBox ID="txtDiasArticulosSinActualizar" runat="server" class="form-control" TextMode="Number" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="ltbnAlertaPreciosArticulosSinActualizar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="ltbnAlertaPreciosArticulosSinActualizar_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Fecha Filtros Cuenta Corriente</label>
                                            <div class="col-md-2">
                                                <div class="input-group">
                                                    <span class="input-group-addon">CCC</span>
                                                    <asp:TextBox ID="txtFechaCuentaCorrienteCompras" runat="server" class="form-control ui-popover" data-container="body" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Fecha correspondiente al filtro de Cuenta Corriente Compras"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="input-group">
                                                    <span class="input-group-addon">CCV</span>
                                                    <asp:TextBox ID="txtFechaCuentaCorrienteVentas" runat="server" class="form-control ui-popover" data-container="body" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Fecha correspondiente al filtro de Cuenta Corriente Ventas"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnFiltroFechaCuentaCorriente" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnFiltroFechaCuentaCorriente_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Tope mínimo Retenciones:</label>
                                            <div class="col-md-4">
                                                <div class="input-group">
                                                    <span class="input-group-addon">$</span>
                                                    <asp:TextBox ID="txtTopeMinimoRetenciones" runat="server" class="form-control" TextMode="Number" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnTopeMinimoRetenciones" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnTopeMinimoRetenciones_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Redondear Precio de Venta:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListRedondearPrecioVenta" runat="server" class="form-control">
                                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnRedondearPrecioVenta" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnRedondearPrecioVenta_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Facturar PRP:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListFacturarPRP" runat="server" class="form-control">
                                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnRefacturarPRP" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnRefacturarPRP_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Sucursal Garantia</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListSucGarantia" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnSucGarantia" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnSucGarantia_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Sucursal Service Oficial</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListSucServiceOficial" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnSucServiceOficial" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnSucServiceOficial_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Estado pedidos:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListEstadoPedidos" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnEstadoIniPedidos" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnEstadoIniPedidos_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Estado pendiente refacturar:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListEstadoPendienteRefacturar" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnEstadoPendienteRefacturar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnEstadoPendienteRefacturar_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Tiempo de preparación por linea de pedido:</label>
                                            <div class="col-md-2">
                                                <div class="input-group">
                                                    <span class="input-group-addon">Minutos</span>
                                                    <asp:TextBox ID="txtMinutosLineas" runat="server" class="form-control ui-popover" data-container="body" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Minutos" MaxLength="2" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="input-group">
                                                    <span class="input-group-addon">Segundos</span>
                                                    <asp:TextBox ID="txtSegundosLineas" runat="server" class="form-control ui-popover" data-container="body" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Segundos" MaxLength="2" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnTiempoLineas" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnTiempoLineas_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Ver saldo de Cliente en Observaciones del PRP:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListVerSaldoClienteObservacionesPRP" runat="server" class="form-control">
                                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnVerSaldoClienteObservacionesPRP" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnVerSaldoClienteObservacionesPRP_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Incidencia Obligatoria:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListIncidenciaObligatoria" runat="server" class="form-control">
                                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnIncidenciaObligatoria" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnIncidenciaObligatoria_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Margen Obligatorio:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListMargenObligatorio" runat="server" class="form-control">
                                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnMargenObligatorio" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnMargenObligatorio_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Actualizar Compuestos:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListActualizarCompuestos" runat="server" class="form-control">
                                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnActualizarCompuestos" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnActualizarCompuestos_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Filtro Articulos Sucursal:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListFiltroArticulosSucursal" runat="server" class="form-control">
                                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnFiltroArticulosSucursal" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnFiltroArticulosSucursal_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Columna de Unidad Medida En Trazabilidad:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListColumnaUnidadMedidaEnTrazabilidad" runat="server" class="form-control">
                                                    <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnColumnaUnidadMedidaEnTrazabilidad" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnColumnaUnidadMedidaEnTrazabilidad_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Mostrar Alicuota IVA en Descripcion Articulos De las Facturas:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListMostrarAlicuotaIVAenDescripcionArticulosDeFacturas" runat="server" class="form-control">
                                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnMostrarAlicuotaIVAenDescripcionArticulosDeFacturas" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnMostrarAlicuotaIVAenDescripcionArticulosDeFacturas_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Modificar cantidad en Venta entre Sucursales:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListModificarCantidadEnVentaEntreSucursales" runat="server" class="form-control">
                                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnModificarCantidadEnVentaEntreSucursales" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnModificarCantidadEnVentaEntreSucursales_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">

                                            <label for="validateSelect" class="col-md-4">Observaciones predeterminadas FC para Ticket Fiscal</label>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtObservacionesFC" Style="height: 100px; width: 500px" runat="server" class="form-control" Rows="4" ToolTip="Las oraciones deben ser de 22 caracteres,cada oracion debe estar separada por '|' ."></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnObeservacionesFC" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnObeservacionesFC_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">

                                            <label for="validateSelect" class="col-md-4">Proveedor Predeterminado Importacion</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListProveedores" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-2">

                                                <asp:LinkButton ID="lbtnProveedores" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnProveedores_Click" />

                                            </div>
                                        </div>


                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-10">Personalizar tabla Articulos:</label>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnPersonalizar" runat="server" Text="<span class='fa fa-expand'></span>" data-toggle="modal" href="#modalPersonalizacion" class="btn btn-success" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-10">Personalizar tabla Cheques:</label>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnPersonalizarCheques" runat="server" Text="<span class='fa fa-expand'></span>" data-toggle="modal" href="#modalPersonalizacionCheques" class="btn btn-success" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-10">Personalizar tabla Stock:</label>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnPersonalizarStock" runat="server" Text="<span class='fa fa-expand'></span>" data-toggle="modal" href="#modalPersonalizacionStock" class="btn btn-success" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-10">Modo Seguro:</label>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnModoSeguro" runat="server" OnClick="lbtnModoSeguro_Click" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-10">Agregar ítems al Facturar o Remitir:</label>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnAgregarItemsFactura" runat="server" OnClick="lbtnAgregarItemsFactura_Click" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-10">Remitir mas de una vez un pedido:</label>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnRemitirMultiplesVeces" runat="server" OnClick="lbtnRemitirMultiplesVeces_Click" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-10">Facturar mas de una vez un pedido:</label>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnFacturarMultiplesVeces" runat="server" OnClick="lbtnFacturarMultiplesVeces_Click" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-10">Generar Pedido Stock Faltante:</label>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnGenerarPedidoStockFaltante" runat="server" OnClick="lbtnGenerarPedidoStockFaltante_Click" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-10">Sumar puntos cobro:</label>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnSumaPuntosCobro" runat="server" Text="<span class='fa fa-expand'></span>" data-toggle="modal" href="#modalSumaPuntosCobro" class="btn btn-success" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-10">Enviar mail al realizar pedido:</label>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnEnviarMailPedido" runat="server" OnClick="lbtnEnviarMailPedido_Click" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-10">Enviar mail al realizar factura:</label>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnEnviarMailFactura" runat="server" OnClick="lbtnEnviarMailFactura_Click" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-10">Facturar mas de una vez un remito:</label>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnFacturarMultiplesVecesRemito" runat="server" OnClick="lbtnFacturarMultiplesVecesRemito_Click" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-10">Mail Gestion:</label>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="LinkButton1" runat="server" Text="<span class='fa fa-expand'></span>" data-toggle="modal" href="#modalMailGestion" class="btn btn-success" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-10">Mail Estetica:</label>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="LinkButton2" runat="server" Text="<span class='fa fa-expand'></span>" data-toggle="modal" href="#modalMailEstetica" class="btn btn-success" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </fieldset>

                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>

    <div id="modalPersonalizacion" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content" style="width: 75%;">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Campos a visualizar</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-group" style="margin-bottom: 15px;">
                                    <label class="col-md-8">Proveedor</label>
                                    <div class="input-group">
                                        <asp:CheckBox ID="CheckBoxProv" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group" style="margin-bottom: 15px;">
                                    <label class="col-md-8">Grupo</label>
                                    <div class="input-group">
                                        <asp:CheckBox ID="CheckBoxGrupo" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group" style="margin-bottom: 15px;">
                                    <label class="col-md-8">Sub Grupo</label>
                                    <div class="input-group">
                                        <asp:CheckBox ID="CheckBoxSubGrupo" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group" style="margin-bottom: 15px;">
                                    <label class="col-md-8">Moneda</label>
                                    <div class="input-group">
                                        <asp:CheckBox ID="CheckBoxMoneda" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group" style="margin-bottom: 15px;">
                                    <label class="col-md-8">Ultima Act.</label>
                                    <div class="input-group">
                                        <asp:CheckBox ID="CheckBoxAct" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group" style="margin-bottom: 15px;">
                                    <label class="col-md-8">Presentaciones</label>
                                    <div class="input-group">
                                        <asp:CheckBox ID="CheckBoxPresen" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group" style="margin-bottom: 15px;">
                                    <label class="col-md-8">Stock</label>
                                    <div class="input-group">
                                        <asp:CheckBox ID="CheckBoxStock" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group" style="margin-bottom: 15px;">
                                    <label class="col-md-8">Marca</label>
                                    <div class="input-group">
                                        <asp:CheckBox ID="CheckBoxMarca" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group" style="margin-bottom: 15px;">
                                    <label class="col-md-8">Precio de Venta en Moneda Original</label>
                                    <div class="input-group">
                                        <asp:CheckBox ID="CheckBoxPrecioVentaMonedaOriginal" runat="server" />
                                    </div>
                                </div>

                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>

                    <div class="modal-footer">
                        <asp:LinkButton ID="btnGuardarPersonalizar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnGuardarPersonalizar_Click" />
                    </div>
                </div>

            </div>
        </div>
    </div>
    <div id="modalSumaPuntosCobro" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content" style="width: 75%;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Puntos por cobro</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel runat="server" UpdateMode="Always" ID="UpdatePanel3">
                            <ContentTemplate>
                                <fieldset>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-8">Puntos por cobro</label>
                                        <div class="input-group">
                                            <asp:DropDownList ID="ListPuntosCobro" runat="server" class="form-control">
                                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-8">Porcentaje Tarjeta</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtPorcentajeTarjeta" runat="server" class="form-control" TextMode="Number" />
                                            <span class="input-group-addon">%</span>

                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-8">Porcentaje Efectivo</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtPorcentajeEfectivo" runat="server" class="form-control" TextMode="Number" />
                                            <span class="input-group-addon">%</span>
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-8">Porcentaje Transferencias</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtPorcentajeTransferencia" runat="server" class="form-control" TextMode="Number" />
                                            <span class="input-group-addon">%</span>
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-8">Porcentaje Cheques</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtPorcentajeCheques" runat="server" class="form-control" TextMode="Number" />
                                            <span class="input-group-addon">%</span>
                                        </div>
                                    </div>

                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-8">Redimir Puntos 1 PTO = </label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtPesosPuntos" runat="server" class="form-control" TextMode="Number" />
                                            <span class="input-group-addon">$</span>
                                        </div>
                                    </div>
                                </fieldset>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnGuardarPuntosCobro" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnGuardarPuntosCobro_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modalPersonalizacionCheques" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content" style="width: 75%;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Visualizacion Cheques</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel runat="server" UpdateMode="Always" ID="UpdatePanelVisualizacionCheques">
                            <ContentTemplate>
                                <fieldset>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-8">Recibo de Cobro</label>
                                        <div class="input-group">
                                            <asp:CheckBox ID="CheckBoxChReciboCobro" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-8">Recibo de Pago</label>
                                        <div class="input-group">
                                            <asp:CheckBox ID="CheckBoxChReciboPago" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-8">Sucursal de Cobro</label>
                                        <div class="input-group">
                                            <asp:CheckBox ID="CheckBoxChSucCobro" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-8">Sucursal de Pago</label>
                                        <div class="input-group">
                                            <asp:CheckBox ID="CheckBoxChSucPago" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-8">Observacion</label>
                                        <div class="input-group">
                                            <asp:CheckBox ID="CheckBoxChObservacion" runat="server" />
                                        </div>
                                    </div>
                                </fieldset>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnGuardarPersonalizarCheques" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnGuardarPersonalizarCheques_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modalPersonalizacionStock" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content" style="width: 75%;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Visualizacion Stock</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel runat="server" UpdateMode="Always" ID="UpdatePanelVisualizacionStock">
                            <ContentTemplate>
                                <fieldset>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-8">Remitos Pendientes</label>
                                        <div class="input-group">
                                            <asp:CheckBox ID="CheckBoxStockRemitosP" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-8">Importaciones Pendientes</label>
                                        <div class="input-group">
                                            <asp:CheckBox ID="CheckBoxStockImportacionesP" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-8">Stock Real</label>
                                        <div class="input-group">
                                            <asp:CheckBox ID="CheckBoxStockReal" runat="server" />
                                        </div>
                                    </div>
                                </fieldset>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnGuardarPersonalizarStock" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnGuardarPersonalizarStock_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div id="modalMailGestion" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content" style="width: 75%;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Mail Gestion</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel runat="server" UpdateMode="Always" ID="UpdatePanel4">
                            <ContentTemplate>
                                <fieldset>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-4">Empresa</label>
                                        <div class="input-group col-md-8">
                                            <asp:DropDownList OnSelectedIndexChanged="ListEmpresaMailGestion_SelectedIndexChanged" AutoPostBack="true" class="form-control" runat="server" ID="ListEmpresaMailGestion"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-4">Sucursal</label>
                                        <div class="input-group col-md-8">
                                            <asp:DropDownList runat="server" class="form-control" ID="ListSucursalMailGestion" onchange="actualizarDatosMailGestion()">
                                                <asp:ListItem Text="Seleccione" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-4">Mail</label>
                                        <div class="input-group col-md-8">
                                            <asp:TextBox runat="server" class="form-control" id="txtMailGestion"/>
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-4">Password</label>
                                        <div class="input-group col-md-8">
                                            <asp:TextBox runat="server" class="form-control" id="txtPasswordMailGestion"/>
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-4">SMTP</label>
                                        <div class="input-group col-md-8">
                                            <asp:TextBox runat="server" class="form-control" id="txtSMTPMailGestion"/>
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-4">Puerto</label>
                                        <div class="input-group col-md-8">
                                            <asp:TextBox runat="server" class="form-control" id="txtPuertoMailGestion"/>
                                        </div>
                                    </div>
                                     <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-4">SSL</label>
                                        <div class="input-group col-md-8">
                                            <asp:DropDownList ID="ListSSLMailGestion" class="form-control" runat="server">
                                                <asp:ListItem Value="0" Text="Si" />
                                                <asp:ListItem Value="1" Text="No" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnGenerarDatosMailGestion" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnGenerarDatosMailGestion_Click" />
                        <asp:HiddenField runat="server"  id="hiddenMailGestion" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="modalMailEstetica" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content" style="width: 75%;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Mail Gestion</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel runat="server" UpdateMode="Always" ID="UpdatePanel5">
                            <ContentTemplate>
                                <fieldset>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-4">Empresa</label>
                                        <div class="input-group col-md-8">
                                            <asp:DropDownList runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListEmpresaMailEstetica_SelectedIndexChanged" ID="ListEmpresaMailEstetica"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-4">Sucursal</label>
                                        <div class="input-group col-md-8">
                                            <asp:DropDownList runat="server" class="form-control" onchange="actualizarDatosMailEstetica()" ID="ListSucursalMailEstetica">
                                                <asp:ListItem Text="Seleccione" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-4">Mail</label>
                                        <div class="input-group col-md-8">
                                            <asp:TextBox runat="server" class="form-control" id="txtMailEstetica"/>
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-4">Password</label>
                                        <div class="input-group col-md-8">
                                            <asp:TextBox runat="server" class="form-control" id="txtPasswordMailEstetica"/>
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-4">SMTP</label>
                                        <div class="input-group col-md-8">
                                            <asp:TextBox runat="server" class="form-control" id="txtSMPTEstetica"/>
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-4">Puerto</label>
                                        <div class="input-group col-md-8">
                                            <asp:TextBox runat="server" class="form-control" id="txtPuertoMailEstetica"/>
                                        </div>
                                    </div>
                                     <div class="form-group" style="margin-bottom: 15px;">
                                        <label class="col-md-4">SSL</label>
                                        <div class="input-group col-md-8">
                                            <asp:DropDownList ID="ListSSLMailEstetica" class="form-control" runat="server">
                                                <asp:ListItem Value="0" Text="Si" />
                                                <asp:ListItem Value="1" Text="No" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                      <div class="modal-footer">
                        <asp:LinkButton ID="btnGenerarDatosMailEstetica" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnGenerarDatosMailEstetica_Click" />
                        <asp:HiddenField runat="server"  id="hiddenMailEstetica" />                    
                      </div>
                </div>
            </div>
        </div>
    </div>



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



    <script>
        function ValidarVuelos() {

            var V = ValidateFly();
            envio(V);
            if (V == 1)
                return false;
            if (V == 0)
                return true;
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

    <script>
        function actualizarDatosMailGestion() {
            var empresa = MainContent_ListEmpresaMailGestion.value;
            var sucursal = MainContent_ListSucursalMailGestion.value.replace(',', '');
            var sistema = 0;
            var datos = empresa + ',' + sucursal + ',' + sistema;
            $.ajax({
                method: "POST",
                url: "PanelControl.aspx/ObtenerDatosMailEmpresaSucursal",
                data: '{datos: "' + datos + '" }',
                contentType: "application/json",
                dataType: 'json',
                error: (error) => {
                    console.log(JSON.stringify(error));
                    //$.msgbox("No se pudo cargar la tabla", { type: "error" });
                },
                success: cargarDatosMailGestion
            });
        }
        function cargarDatosMailGestion(response) {
            var data = response.d;
            var obj = JSON.parse(data);
            if (obj != "null") {
                MainContent_txtMailGestion.value = obj.split(',')[1];
                MainContent_txtPasswordMailGestion.value = obj.split(',')[2];
                MainContent_txtSMTPMailGestion.value = obj.split(',')[3];
                MainContent_txtPuertoMailGestion.value = obj.split(',')[4];
                MainContent_ListSSLMailGestion.value = obj.split(',')[6];
                MainContent_hiddenMailGestion.value = obj.split(',')[0];
            }
        }

        function actualizarDatosMailEstetica() {
            var empresa = MainContent_ListEmpresaMailEstetica.value;
            var sucursal = MainContent_ListSucursalMailEstetica.value.replace(',', '');
            var sistema = 1; 
            var datos = empresa + ',' + sucursal + ',' + sistema;
            $.ajax({
                method: "POST",
                url: "PanelControl.aspx/ObtenerDatosMailEmpresaSucursal",
                data: '{datos: "' + datos + '" }',
                contentType: "application/json",
                dataType: 'json',
                error: (error) => {
                    console.log(JSON.stringify(error));
                    //$.msgbox("No se pudo cargar la tabla", { type: "error" });
                },
                success: cargarLabelDatosMailEstetica
            });
        }
        function cargarLabelDatosMailEstetica(response) {
            var data = response.d;
            var obj = JSON.parse(data);
            if (obj != "null") {
                MainContent_txtMailEstetica.value = obj.split(',')[1];
                MainContent_txtPasswordMailEstetica.value = obj.split(',')[2];
                MainContent_txtSMPTEstetica.value = obj.split(',')[3];
                MainContent_txtPuertoMailEstetica.value = obj.split(',')[4];
                MainContent_ListSSLMailEstetica.value = obj.split(',')[6];
                MainContent_hiddenMailEstetica.value = obj.split(',')[0];

            }

        }
    </script>

    <script>
        $(function () {
            $("#<%= txtFechaCuentaCorrienteCompras.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });
        $(function () {
            $("#<%= txtFechaCuentaCorrienteVentas.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });
    </script>


</asp:Content>

