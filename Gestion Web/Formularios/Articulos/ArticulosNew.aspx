<%@ Page EnableEventValidation="false" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ArticulosNew.aspx.cs" Inherits="Gestion_Web.Formularios.Articulos.ArticulosNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="col-md-12 col-xs-12">
        <div class="widget stacked">
            <div class="stat">
                <h5><i class="icon-map-marker"></i>Maestro > Articulos > Articulos</h5>
            </div>
            <div class="widget-header">
                <i class="icon-wrench"></i>
                <h3>Herramientas</h3>
            </div>
            <!-- /widget-header -->

            <div class="widget-content">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 30%">
                            <div class="col-md-12">
                                <div class="input-group">
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="lbBuscar" OnClientClick="llenarTablaBySearch()" runat="server" Text="Buscar" class="btn btn-primary">
                                                    <i class="shortcut-icon icon-search"></i></asp:LinkButton>
                                    </span>
                                    <asp:TextBox ID="txtBusqueda" runat="server" class="form-control" placeholder="Buscar Articulo"></asp:TextBox>

                                </div>
                                <!-- /input-group -->
                            </div>
                        </td>

                        <td style="width: 40%">
                            <asp:Literal ID="LitFiltro" runat="server"></asp:Literal>
                            <asp:Literal ID="LitReferencia" runat="server" Visible="False">
                                <i class="icon-star"></i> Articulo en oferta/promoción vigente
                            </asp:Literal>

                        </td>
                        <td style="width: 2%">

                            <div class="btn-group" style="width: 100%">
                                <button type="button" class="btn btn-primary dropdown-toggle ui-tooltip" title data-original-title="Listados" data-toggle="dropdown">
                                    <i class="shortcut-icon icon-usd"></i>
                                    <i class="shortcut-icon icon-print"></i>&nbsp
                                   
                                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                    <span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu" role="menu">
                                    <li>
                                        <a href="#modalImprimirStock" data-toggle="modal" style="width: 90%">Stock Por Sucursal</a>
                                    </li>
                                    <li>
                                        <a href="#modalEtiquetas" data-toggle="modal" style="width: 90%">Etiquetas</a>
                                    </li>
                                    <li>
                                        <a href="#modalListas" data-toggle="modal" style="width: 90%">Lista de Precios</a>
                                    </li>
                                    <li>
                                        <a href="#modalStockaFecha" data-toggle="modal" style="width: 90%">Stock a Fecha</a>
                                    </li>
                                    <li>
                                        <a href="#modalStockValorizado" data-toggle="modal" style="width: 90%">Stock valorizado</a>
                                    </li>
                                    <li>
                                        <a href="#modalStockDias" data-toggle="modal" style="width: 90%">Stock a dias</a>
                                    </li>
                                    <li>
                                        <a href="#modalStockNoVendido" data-toggle="modal" style="width: 90%">Stock sin movimiento</a>
                                    </li>
                                    <li>
                                        <a href="#modalUnicoSucursal" data-toggle="modal" style="width: 90%">Stock unico sucursal</a>
                                    </li>
                                    <li>
                                        <a href="#modalMovStock" data-toggle="modal" style="width: 90%">Movimiento stock</a>
                                    </li>
                                    <li>
                                        <a href="#modalIngresosEgresosArticulos" data-toggle="modal" style="width: 90%">Ingresos/Egresos</a>
                                    </li>
                                    <li>
                                        <a href="#modalNominaDeArticulos" data-toggle="modal" style="width: 90%">Nomina de Articulos</a>
                                    </li>
                                     <li>
                                        <a href="#modalArticulosOtrosProveedores" data-toggle="modal" style="width: 90%">Articulos de Otros Proveedores</a>
                                    </li>
                                </ul>
                            </div>

                        </td>
                        <td style="width: 2%">

                            <div class="btn-group" style="width: 100%">
                                <button type="button" class="btn btn-primary dropdown-toggle ui-tooltip" title data-original-title="Precios Desactualizados x Dias " data-toggle="modal" href="#modalDeactualizados">
                                    <i class="shortcut-icon icon-minus"></i>
                                    <i class="shortcut-icon icon-usd"></i>
                                    <i class="shortcut-icon icon-calendar"></i>
                                </button>
                            </div>
                            <!-- /btn-group -->

                        </td>

                        <td style="width: 2%">

                            <div class="btn-group" style="width: 100%">
                                <button type="button" class="btn btn-primary dropdown-toggle ui-tooltip" title data-original-title="Ultima Actualizacion Precios" data-toggle="dropdown">
                                    <i class="shortcut-icon icon-usd"></i>
                                    <i class="shortcut-icon icon-calendar"></i>&nbsp
                                   
                                    <asp:Literal ID="litNumero" runat="server"></asp:Literal>
                                    <span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu" role="menu">
                                    <li>
                                        <asp:LinkButton ID="btnUltimos_1" runat="server" OnClientClick="llenarTablaByUltimosDias(this)">Ultimo Día</asp:LinkButton></li>
                                    <li>
                                        <asp:LinkButton ID="btnUltimos_2" runat="server" OnClientClick="llenarTablaByUltimosDias(this)">Ultimos 2 Días</asp:LinkButton></li>
                                    <li>
                                        <asp:LinkButton ID="btnUltimos_3" runat="server" OnClientClick="llenarTablaByUltimosDias(this)">Ultimos 3 Días</asp:LinkButton></li>
                                    <li>
                                        <asp:LinkButton ID="btnUltimos_4" runat="server" OnClientClick="llenarTablaByUltimosDias(this)">Ultimos 4 Días</asp:LinkButton></li>
                                    <li>
                                        <asp:LinkButton ID="btnUltimos_5" runat="server" OnClientClick="llenarTablaByUltimosDias(this)">Ultimos 5 Días</asp:LinkButton></li>

                                    <li>
                                        <asp:LinkButton ID="btnUltimos_6" runat="server" OnClientClick="llenarTablaByUltimosDias(this)">Ultimos 6 Días</asp:LinkButton></li>
                                    <li>
                                        <asp:LinkButton ID="btnUltimos_7" runat="server" OnClientClick="llenarTablaByUltimosDias(this)">Ultimos 7 Días</asp:LinkButton></li>

                                </ul>
                            </div>
                            <!-- /btn-group -->

                        </td>
                        <td style="width: 2%">
                            <asp:PlaceHolder ID="phActualizacionPrecios" Visible="true" runat="server">
                                <%--<div class="btn-group" style="height: 100%">                                    
                                        <a class="btn btn-primary ui-tooltip" href="#modalRecalcular" data-toggle="modal" title data-original-title="Actualizar Precios" >
                                            <i class="shortcut-icon icon-usd"></i>
                                            <i class="shortcut-icon icon-gear"></i>
                                        </a>                                
                                </div>--%>
                                <div class="btn-group" style="width: 100%">
                                    <button type="button" class="btn btn-primary dropdown-toggle ui-tooltip" title data-original-title="Actualizar Precios" data-toggle="dropdown">
                                        <i class="shortcut-icon icon-usd"></i>
                                        <i class="shortcut-icon icon-gear"></i>&nbsp
                                       
                                        <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                                        <span class="caret"></span>
                                    </button>
                                    <ul class="dropdown-menu" role="menu">
                                        <li>
                                            <a href="#modalRecalcular" data-toggle="modal">Actualizar Precios</a>
                                        </li>
                                        <li>
                                            <a href="#modalActualizarProveedor" data-toggle="modal">Actualizar Precios Proveedor</a>
                                        </li>
                                        <li>
                                            <a href="#modalArticulosDespacho" data-toggle="modal">Actualizar Articulos Despacho</a>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="btnActualizarTodo" runat="server" Visible="False" Text="recalcular desde precio venta" OnClick="btnActualizarTodo_Click"></asp:LinkButton>
                                        </li>
                                    </ul>
                                </div>
                            </asp:PlaceHolder>
                        </td>
                        <td style="width: 2%">
                            <div class="btn-group" style="height: 100%">
                                <a class="btn btn-primary ui-tooltip" data-toggle="modal" title data-original-title="Filtrar Articulos" href="#modalBusqueda" style="width: 100%">
                                    <i class="shortcut-icon icon-filter"></i>
                                </a>
                            </div>
                        </td>
                        <td style="width: 3%">
                            <a href="ArticulosABM.aspx?accion=1" class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                <i class="shortcut-icon icon-plus"></i>
                            </a>
                        </td>

                    </tr>
                </table>
            </div>
            <!-- /widget-content -->

        </div>
        <!-- /widget -->
    </div>
    <div class="col-md-12 col-xs-12">
        <div class="widget stacked widget-table action-table">

            <div class="widget-header">
                <i class="icon-bookmark"></i>
                <h3>Articulos</h3>
            </div>
            <div class="widget-content">
                <div class="panel-body">

                    <%--<div class="col-md-12 col-xs-12">--%>
                    <div class="table-responsive">
                        <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                            <thead>
                                <tr>
                                    <%--<asp:PlaceHolder ID="phColumna0" Visible="false" runat="server">--%>
                                    <th runat="server" id="ColumnaId" style="width: 0%">Id</th>
                                    <%--</asp:PlaceHolder>--%>
                                    <th runat="server" id="ColumnaCodigo" style="width: 5%">Codigo</th>
                                    <th runat="server" id="ColumnaDescripcion" style="width: 35%">Descripcion</th>
                                    <%--<asp:PlaceHolder ID="phColumna2" Visible="false" runat="server">--%>
                                    <th runat="server" id="ColumnaGrupo" style="width: 10%">Grupo</th>
                                    <%--</asp:PlaceHolder>--%>
                                    <%--<asp:PlaceHolder ID="phColumna3" Visible="false" runat="server">--%>
                                    <th runat="server" id="ColumnaSubgrupo" style="width: 10%">SubGrupo</th>
                                    <%--</asp:PlaceHolder>--%>
                                    <%--<asp:PlaceHolder ID="phColumna7" Visible="false" runat="server">
                                        <th runat="server" style="width: 10%">Presentacion</th>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="phColumna8" Visible="false" runat="server">
                                        <th runat="server" style="width: 10%">Stock</th>
                                    </asp:PlaceHolder>--%>
                                    <%--<asp:PlaceHolder ID="phColumna9" Visible="false" runat="server">--%>
                                    <th runat="server" id="ColumnaMarca" style="width: 5%">Marca</th>
                                    <%--</asp:PlaceHolder>--%>
                                    <%--<asp:PlaceHolder ID="phColumna4" Visible="false" runat="server">
                                        <th runat="server" style="width: 5%">Moneda</th>
                                    </asp:PlaceHolder>--%>
                                    <%--<asp:PlaceHolder ID="phColumna5" Visible="false" runat="server">--%>
                                    <th runat="server" id="ColumnaUltimaAct" style="width: 5%">UltimaActualizacion.</th>
                                    <%--</asp:PlaceHolder>--%>
                                    <%--<asp:PlaceHolder ID="phColumna1" Visible="false" runat="server">--%>
                                    <th runat="server" id="ColumnaProveedor" style="width: 10%">Proveedor</th>
                                    <%--</asp:PlaceHolder>--%>
                                    <%--<asp:PlaceHolder ID="phColumna10" Visible="false" runat="server">--%>
                                    <th runat="server" id="ColumnaPVenta" style="width: 5%">PVenta</th>
                                    <%--</asp:PlaceHolder>--%>
                                    <%--<asp:PlaceHolder ID="phColumna11" Visible="false" runat="server">--%>
                                    <th runat="server" id="ColumnaApareceLista" style="width: 0%">ApareceLista</th>
                                    <%--</asp:PlaceHolder>--%>
                                    <%--<asp:PlaceHolder ID="phColumna6" runat="server">
                                        <th runat="server" id="headerPrecio" style="width: 5%">P.Venta en ($)</th>
                                    </asp:PlaceHolder>--%>
                                    <%--<th runat="server" class="td-actions" style="width: 20%"></th>--%>
                                    <th runat="server" id="ColumnaBotones" style="width: 20%"></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder ID="phArticulos" runat="server"></asp:PlaceHolder>
                            </tbody>
                        </table>
                    </div>


                </div>


                <!-- /.content -->
                <div style="text-align: right; margin-bottom: 1%">
                    <asp:Button Text="Anterior" runat="server" ID="btnPrevious" OnClientClick="llenarTablaPrevious()" Style="margin-right: 1%; visibility: hidden" />
                    <asp:Button Text="Siguiente" runat="server" ID="btnNext" OnClientClick="llenarTablaNext()" Style="margin-right: 1%; visibility: hidden" />
                    <input runat="server" id="hiddenGrupoValue" type="hidden" />
                    <input runat="server" id="hiddenSubGrupoValue" type="hidden" />
                    <input runat="server" id="hiddenMarca" type="hidden" />
                    <input runat="server" id="hiddenDiasUltimaActualizacion" type="hidden" />
                    <input runat="server" id="hiddenProveedor" type="hidden" />
                    <input runat="server" id="hiddenSoloProveedorPredeterminado" type="hidden" />
                    <input runat="server" id="hiddenDescSubGrupo" type="hidden" />
                    <input runat="server" id="hiddenAccion" type="hidden" />
                    <input runat="server" id="hiddenBuscar" type="hidden" />
                    <input runat="server" id="hiddenDescripcion" type="hidden" />
                </div>
            </div>

        </div>
    </div>
    <!-- /container -->


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
                                    <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar el Articulo?" Style="text-align: center"></asp:Label>
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
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div id="modalBusqueda" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" id="btnCerrarFlitro" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Filtrar Articulos</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-4">Grupo</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListGrupo" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListGrupo_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">SubGrupo</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListSubGrupo" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Marca</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListMarca" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Buscar Proveedor</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtCodProveedor" class="form-control" runat="server"></asp:TextBox>
                                    </div>

                                    <div class="col-md-2">
                                        <asp:LinkButton ID="btnBuscarProveedor" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarProveedor_Click" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Proveedor</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListProveedor" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="DropListClientes_SelectedIndexChanged"></asp:DropDownList>
                                    </div>

                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListProveedor" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Razon Social</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListRazonSocial" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="ListRazonSocial_SelectedIndexChanged"></asp:DropDownList>
                                    </div>

                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListRazonSocial" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Precios actualizados en los ultimos</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtDiasActualizacion" TextMode="Number" runat="server" Text="0" class="form-control"></asp:TextBox>
                                    </div>

                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtDiasActualizacion" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Solo proveedor predeterminado</label>
                                    <div class="col-md-6">
                                        <asp:CheckBox ID="cbSoloProveedorPredeterminado" runat="server" />
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="btnFiltrar" OnClientClick="llenarTabla()" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" />
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div id="modalRecalcular" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanel11">
                    <ContentTemplate>
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Modificar Precio</h4>
                        </div>
                        <div class="modal-body">
                            <div role="form" class="form-horizontal col-md-12">
                                <div class="form-group">
                                    <label class="col-md-4">Porcentaje Costo</label>
                                    <div class="col-md-5">

                                        <div class="input-group">
                                            <span class="input-group-addon">%</span>
                                            <asp:TextBox ID="txtPorcentajeAumento" Style="text-align: right;" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Text="0"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button ID="btnModificarPrecio" runat="server" Text="Actualizar" class="btn btn-success" OnClick="btnModificarPrecio_Click" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Nuevo Precio Venta</label>
                                    <div class="col-md-5">

                                        <div class="input-group">
                                            <span class="input-group-addon">$</span>
                                            <asp:TextBox ID="txtPrecioVenta" Style="text-align: right;" runat="server" class="form-control" Text="0"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button ID="btnSeteaPrecioventa" runat="server" Text="Actualizar" class="btn btn-success" OnClick="btnSeteaPrecioventa_Click" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Porcentaje Precio Venta</label>
                                    <div class="col-md-5">
                                        <div class="input-group">
                                            <span class="input-group-addon">%</span>
                                            <asp:TextBox ID="txtPrecioVentaPorcentual" Style="text-align: right;" runat="server" class="form-control" Text="0"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button ID="btnSeteaPrecioventaPorcentual" runat="server" Text="Actualizar" class="btn btn-success" OnClick="btnSeteaPrecioventaPorcentual_Click" />
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div id="modalActualizarProveedor" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Actualizar Precio de Otros Proveedores</h4>
                        </div>
                        <div class="modal-body">
                            <div role="form" class="form-horizontal col-md-12">

                                <div class="form-group">
                                    <label class="col-md-4">Buscar Proveedor</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtBuscarProveedor" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:LinkButton ID="lbtnBuscarProveedorDesdeActualizarProveedor" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="lbtnBuscarProveedorDesdeActualizarProveedor_Click" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Proveedor:</label>
                                    <div class="col-md-12">
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListOtroProveedor" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListOtroProveedor" InitialValue="-1" ValidationGroup="OtrosProveedores" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Archivo:</label>
                                    <div class="col-md-8">
                                        <asp:FileUpload ID="FileUpload1" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="lblConfigCSV" runat="server" ForeColor="#999999"></asp:Label>
                                    <br />
                                    <asp:Label ID="lblFormato" runat="server" ForeColor="#999999" Text="*Los precios deben estar en formato (1234.00) sin separador de miles."></asp:Label>
                                </div>
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnActualizarOtrosProveedores" runat="server" class="btn btn-success" ValidationGroup="OtrosProveedores" Text="<span class='shortcut-icon icon-ok'></span>" OnClick="lbtnActualizarOtrosProveedores_Click"></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

    <div id="modalArticulosDespacho" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Actualizar Despacho de articulos</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel10" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-4">Archivo:</label>
                                    <div class="col-md-8">
                                        <asp:FileUpload ID="FileUploadArticulosDespacho" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label1" runat="server" ForeColor="#999999"></asp:Label>
                                    <br />
                                    <asp:Label ID="Label2" runat="server" ForeColor="#999999" Text="*Los precios deben estar en formato (1234.00) sin separador de miles."></asp:Label>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnActualizarArticulosDespacho" runat="server" class="btn btn-success" Text="<span class='shortcut-icon icon-ok'></span>" OnClick="lbtnActualizarArticulosDespacho_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modalImprimirStock" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Seleccionar Sucursal</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-4">Sucursal</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="listSucursal" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4"></label>
                                    <div class="col-md-8">

                                        <div class="input-group">
                                            <asp:CheckBox ID="CheckIncluirCeros" runat="server" Text="&nbsp INCLUIR CEROS" />
                                        </div>
                                        <div class="input-group">
                                            <asp:CheckBox ID="CheckBoxStockFaltante" runat="server" Text="&nbsp STOCK FALTANTE" />
                                        </div>
                                        <div class="input-group">
                                            <asp:CheckBox ID="CheckBoxUnicoSucursal" runat="server" Text="&nbsp STOCK UNICO SUCURSAL" />
                                        </div>
                                        <div class="input-group">
                                            <asp:CheckBox ID="CheckIncluirInactivos" runat="server" Text="&nbsp INCLUIR ARTICULOS INACTIVOS" />
                                        </div>
                                    </div>
                                </div>


                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>

                    <div class="modal-footer">
                        <asp:LinkButton ID="btnInformeStock" runat="server" Text="Generar PDF" class="btn btn-success" OnClick="btnInformeStock_Click" />
                        <asp:LinkButton ID="btnInformeStock2" runat="server" Text="Generar Excel" class="btn btn-success" OnClick="btnInformeStock2_Click" />
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div id="modalEtiquetas" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Etiquetas</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-4">Sucursal</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListSucursalEtiquetas" ValidationGroup="BusquedaEtiquetas" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListSucursalEtiquetas" InitialValue="-1" ValidationGroup="BusquedaEtiquetas" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Lista PRecios</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListListaPrecio" ValidationGroup="BusquedaEtiquetas" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListListaPrecio" InitialValue="-1" ValidationGroup="BusquedaEtiquetas" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Tipo Etiqueta</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListEtiqueta" runat="server" class="form-control">
                                            <asp:ListItem Value="1">Chicas</asp:ListItem>
                                            <asp:ListItem Value="2">Medianas</asp:ListItem>
                                            <asp:ListItem Value="3">Grandes</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4"></label>
                                    <div class="col-md-8">
                                        <div class="input-group">
                                            <asp:CheckBox ID="StockCero" runat="server" Text="&nbsp INCLUIR STOCK CERO" />
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="btnImprimirEtiqueta" runat="server" ValidationGroup="BusquedaEtiquetas" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnImprimirEtiqueta_Click" />
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div id="modalListas" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Lista de Precios</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-4">Lista de Precios</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListListaPrecios" ValidationGroup="BusquedaLista" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListListaPrecios" InitialValue="-1" ValidationGroup="BusquedaLista" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Buscar Proveedor</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtBuscarProveedorListaPrecios" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:HiddenField runat="server" ID="idProveedorHF" />
                                        <asp:LinkButton ID="LinkButton1" OnClientClick="ObtenerProveedor()" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:LinkButton ID="lbtnBuscarProveedor" OnClientClick="ObtenerProveedor()" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Proveedor</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListProveedor" ValidationGroup="BusquedaLista" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListProveedor" InitialValue="-1" ValidationGroup="BusquedaLista" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Precio Sin Iva</label>
                                    <div class="col-md-1">
                                        <asp:CheckBox ID="PrecioSinIva" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Descuento Por Cantidad</label>
                                    <div class="col-md-1">
                                        <asp:CheckBox ID="DescuentoPorCantidad" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Agrupar por ubicacion</label>
                                    <div class="col-md-1">
                                        <asp:CheckBox ID="chkUbicacion" runat="server" />
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnImprimirListaPreciosPDF" runat="server" ValidationGroup="BusquedaLista" Text="Generar PDF" class="btn btn-success" OnClientClick="AsignarProveedor()" OnClick="btnImprimirListaPrecios_Click" />
                        <asp:LinkButton ID="btnImprimirListaPreciosXLS" runat="server" ValidationGroup="BusquedaLista" Text="Generar Excel" class="btn btn-success" OnClientClick="AsignarProveedor()" OnClick="btnImprimirListaPrecios2_Click" />
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div id="modalStockaFecha" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Stock a fecha</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel5" UpdateMode="Always" runat="server">
                            <ContentTemplate>

                                <div class="form-group">
                                    <label class="col-md-4">Hasta</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtFechaHasta_St" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta_St" ValidationGroup="StockaFecha" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">
                                        Sucursal
                                   
                                    </label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListSucursal_St" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal_St" InitialValue="-1" ValidationGroup="StockaFecha" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>

                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnStockAFechaPDF" runat="server" ValidationGroup="StockaFecha" Text="Generar PDF" class="btn btn-success" OnClick="lbtnStockAFecha_Click" />
                        <asp:LinkButton ID="lbtnStockAFechaXLS" runat="server" ValidationGroup="StockaFecha" Text="Generar Excel" class="btn btn-success" OnClick="lbtnStockAFechaXLS_Click" />
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div id="modalStockValorizado" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Stock valorizado</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel6" UpdateMode="Always" runat="server">
                            <ContentTemplate>

                                <div class="form-group">
                                    <label class="col-md-4">
                                        Sucursal                                   
                                    </label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListSucursal_St2" runat="server" class="form-control" disabled></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal_St2" InitialValue="-1" ValidationGroup="StockValorizado" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Stock Detallado</label>
                                    <div class="col-md-6">
                                        <asp:CheckBox ID="cbStockDetallado" Checked="false" runat="server" />
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>

                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnStockValorizado" runat="server" ValidationGroup="StockValorizado" Text="Generar PDF" class="btn btn-success" OnClick="lbtnStockValorizado_Click" />
                        <asp:LinkButton ID="lbtnStockValorizadoXLS" runat="server" ValidationGroup="StockValorizado" Text="Generar Excel" class="btn btn-success" OnClick="lbtnStockValorizadoXLS_Click" />
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div id="modalStockDias" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Referencias</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel7" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-4">Desde</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtFechaRefDesde" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaRefDesde" ValidationGroup="ReferenciaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Hasta</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtFechaRefHasta" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaRefHasta" ValidationGroup="ReferenciaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Cant. Dias</label>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtDias" runat="server" class="form-control" TextMode="Number" Style="text-align: right;" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:CheckBox ID="chkDiasCero" runat="server" />
                                    </div>
                                    <label class="col-md-3" style="padding-left: 0%;">Incluir ceros</label>

                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtDias" ValidationGroup="ReferenciaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                    <!-- /input-group -->
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Sucursal</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListSucursalRef" runat="server" class="form-control" disabled></asp:DropDownList>
                                        <!-- /input-group -->
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursalRef" InitialValue="0" ValidationGroup="ReferenciaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-md-4">Buscar Prov.</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtProvRef" class="form-control" runat="server"></asp:TextBox>
                                    </div>

                                    <div class="col-md-2">
                                        <asp:LinkButton ID="lbtnProvRefBuscar" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="lbtnProvRefBuscar_Click" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Proveedores</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListProvRef" runat="server" class="form-control"></asp:DropDownList>
                                        <!-- /input-group -->
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListProvRef" InitialValue="0" ValidationGroup="ReferenciaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Categoria</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListCategoria" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Listas de precio</label>
                                    <asp:CheckBoxList ID="chkListListas" runat="server">
                                    </asp:CheckBoxList>
                                </div>

                                <div class="form-group" style="display: none;">
                                    <label class="col-md-4">Reporte stock no vendido</label>
                                    <div class="input-group">
                                        <asp:CheckBox ID="chkNoVendida" runat="server" />
                                    </div>
                                </div>

                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnStockDiasPDF" runat="server" Text="Generar PDF" class="btn btn-success" ValidationGroup="ReferenciaGroup" OnClick="lbtnStockDiasPDF_Click" />
                    <asp:LinkButton ID="lbtnStockDiasXLS" runat="server" Text="Exportar Excel" class="btn btn-success" ValidationGroup="ReferenciaGroup" OnClick="lbtnStockDiasXLS_Click" />
                </div>
            </div>
        </div>
    </div>
    <!-- /container -->
    <div id="modalStockNoVendido" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Stock no vendido</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel8" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-4">Desde</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtFechaDesdeNoVendido" runat="server" class="form-control"></asp:TextBox>
                                        <!-- /input-group -->
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesdeNoVendido" ValidationGroup="NoVendidaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Hasta</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtFechaHastaNoVendido" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHastaNoVendido" ValidationGroup="NoVendidaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                    <!-- /input-group -->
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Sucursal</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListSucNoVendido" runat="server" class="form-control" disabled></asp:DropDownList>
                                        <!-- /input-group -->
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucNoVendido" InitialValue="0" ValidationGroup="NoVendidaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Buscar Prov.</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtProvNoVendido" class="form-control" runat="server"></asp:TextBox>
                                    </div>

                                    <div class="col-md-2">
                                        <asp:LinkButton ID="lbtnProvNoVendido" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="lbtnProvNoVendido_Click" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Proveedores</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListProvNoVendido" runat="server" class="form-control"></asp:DropDownList>
                                        <!-- /input-group -->
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListProvNoVendido" InitialValue="0" ValidationGroup="NoVendidaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Listas de precio</label>
                                    <asp:CheckBoxList ID="chkListListasNoVendido" runat="server">
                                    </asp:CheckBoxList>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnNoVendidaPDF" runat="server" Text="Generar PDF" class="btn btn-success" ValidationGroup="NoVendidaGroup" OnClick="lbtnNoVendidaPDF_Click" />
                    <asp:LinkButton ID="lbtnNoVendidaXLS" runat="server" Text="Exportar Excel" class="btn btn-success" ValidationGroup="NoVendidaGroup" OnClick="lbtnNoVendidaXLS_Click" />
                </div>
            </div>
        </div>
    </div>

    <div id="modalUnicoSucursal" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Seleccionar Sucursal</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <div class="form-group">
                            <label class="col-md-4">Sucursal central</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="ListSucursalCentral" runat="server" class="form-control" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Sucursal a comparar</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="ListSucursalComparar" runat="server" class="form-control" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnImprimirUnicoCentral" runat="server" Text="Generar PDF" class="btn btn-success" OnClick="lbtnImprimirUnicoCentral_Click" />
                        <asp:LinkButton ID="lbtnExportarUnicoCentral" runat="server" Text="Generar Excel" class="btn btn-success" OnClick="lbtnExportarUnicoCentral_Click" />
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div id="modalNominaDeArticulos" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Nomina de Articulos</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <div class="form-group">
                            <label class="col-md-4">Incluir Art. Inactivos</label>
                            <div class="col-md-1">
                                <asp:CheckBox ID="chArtInactivos" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnNominaArticulosImprimir" runat="server" Text="Generar PDF" class="btn btn-success" OnClick="lbtnNominaArticulosImprimir_Click" />
                    <asp:LinkButton ID="lbtnNominaArticulosExportar" runat="server" Text="Generar Excel" class="btn btn-success" OnClick="lbtnNominaArticulosExportar_Click" />
                </div>
            </div>
        </div>
    </div>

    <div id="modalArticulosOtrosProveedores" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Articulos de Otros Proveedores</h4>
                </div>
                <div class="modal-body">
                    <%--<div role="form" class="form-horizontal col-md-12">
                        <div class="form-group">

                        </div>
                    </div>--%>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnArticulosOtrosProveedoresExportar" runat="server" Text="Generar Excel" class="btn btn-success" OnClick="lbtnArticulosOtrosProveedoresExportar_Click"></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

    <div id="modalMovStock" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Seleccionar Sucursal</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <div class="form-group">
                            <label class="col-md-4">Desde:</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFechaDesdeMovStock" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesdeMovStock" ValidationGroup="MovStockGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Hasta:</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFechaHastaMovStock" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHastaMovStock" ValidationGroup="MovStockGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Sucursal:</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="ListSucursalMovStock" runat="server" class="form-control" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Tipo:</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="ListTipoMovStock" runat="server" class="form-control">
                                    <asp:ListItem Text="Todos" Value="0" />
                                    <asp:ListItem Text="Inventario/Correcciones" Value="1" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnImprimirMovStock" runat="server" Text="Generar PDF" ValidationGroup="MovStockGroup" class="btn btn-success" OnClick="lbtnImprimirMovStock_Click" />
                        <asp:LinkButton Text="Generar Excel" OnClientClick="excelMovStock();" Visible="false" runat="server" class="btn btn-success" />
                        <asp:LinkButton ID="lbtnExportarMovStock" runat="server" Text="Generar Excel" ValidationGroup="MovStockGroup" class="btn btn-success" OnClick="lbtnExportarMovStock_Click" />
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div id="modalDeactualizados" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" id="btnCerrarDesactualizados" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Seleccione dias</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <div class="form-group">
                            <label class="col-md-4">Cantidad dias:</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDiasDesactualizado" runat="server" class="form-control" TextMode="Number"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtDiasDesactualizado" ValidationGroup="DesactGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnDesactualizados" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" ValidationGroup="DesactGroup" OnClientClick="llenarTablaDesactualizados()" />
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div id="modalIngresosEgresosArticulos" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Seleccionar Sucursal</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <div class="form-group">
                            <label class="col-md-4">Desde:</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDesdeIEArticulos" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtDesdeIEArticulos" ValidationGroup="IEArticulos" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Hasta:</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtHastaIEArticulos" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtHastaIEArticulos" ValidationGroup="IEArticulos" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Sucursal:</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="ListSucursalIEArticulos" runat="server" class="form-control" />
                            </div>
                        </div>

                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnIEArticulosPdf" runat="server" Text="Generar PDF" ValidationGroup="IEArticulos" class="btn btn-success" OnClick="btnIEArticulosPdf_Click" />
                        <asp:LinkButton Text="Generar Excel" OnClientClick="excelMovStock();" Visible="false" runat="server" class="btn btn-success" />
                        <asp:LinkButton ID="btnIEArticulosExcel" runat="server" Text="Generar Excel" ValidationGroup="IEArticulos" class="btn btn-success" OnClick="btnIEArticulosExcel_Click" />
                    </div>
                </div>

            </div>
        </div>
    </div>

    <script>
        function abrirdialog(valor) {
            document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
        }
    </script>

    <script>
        function pageLoad() {
            $("#<%= txtFechaHasta_St.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaRefDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaRefHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaDesdeMovStock.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaHastaMovStock.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaDesdeNoVendido.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaHastaNoVendido.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtDesdeIEArticulos.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtHastaIEArticulos.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        }
    </script>

    <script>
        var chk1 = $("#<%= CheckIncluirCeros.ClientID %>");
        var chk2 = $("#<%= CheckBoxStockFaltante.ClientID %>");
        var chk3 = $("#<%= CheckBoxUnicoSucursal.ClientID %>");

        chk1.on('change', function () {
            chk3.prop('checked', checked = false);
        });

        chk2.on('change', function () {
            chk1.prop('checked', this.checked);
            chk3.prop('checked', checked = false);
        });

        chk3.on('change', function () {
            chk1.prop('checked', checked = false);
            chk2.prop('checked', checked = false);

        });

    </script>
    <script>

        function AsignarProveedor() {
            var idProveedor = document.getElementById('<%= DropListProveedor.ClientID %>').value;
            var idProveedorHF = document.getElementById('<%= idProveedorHF.ClientID %>');

            idProveedorHF.value = idProveedor;
        };

        function ObtenerProveedor() {
            event.preventDefault();
            var descripcionProveedor = document.getElementById('<%= txtBuscarProveedorListaPrecios.ClientID %>').value;

            $.ajax({
                type: "POST",
                url: "ArticulosNew.aspx/ObtenerProveedor",
                data: '{proveedor: "' + descripcionProveedor + '"  }',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudo obtener el proveedor.");
                },
                success: OnSuccessObtenerProveedor
            });
        };

        function OnSuccessObtenerProveedor(response) {
            var controlDropListProveedor = document.getElementById('<%= DropListProveedor.ClientID %>');

            while (controlDropListProveedor.options.length > 0) {
                controlDropListProveedor.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            for (i = 0; i < obj.length; i++) {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].alias;

                controlDropListProveedor.add(option);
            }
        }

        function excelMovStock() {
            var desde = $("#<%= txtFechaDesdeMovStock.ClientID %>");
            var hasta = $("#<%= txtFechaHastaMovStock.ClientID %>");
            var suc = $("#<%= ListSucursalMovStock.ClientID %>");
            window.open("ImpresionMovStock.aspx?a=7&ex=1&fd=" + desde[0].value + "&fh=" + hasta[0].value + "&s=" + suc[0].value + "&movStk");
        };
    </script>
    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
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

    <!-- Page-Level Plugin Scripts - Tables -->
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="//cdn.datatables.net/plug-ins/1.10.9/sorting/date-eu.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>

    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

    <script>

        $(document).ready(function () {
            var inputAccion = document.getElementById('<%= hiddenAccion.ClientID%>');
            inputAccion.value = 0;
        });

        var previousPageId = 0;
        var nextPageId = 0;

        function mostrarMensaje(Boton) {
            $.msgbox("Are you sure that you want to permanently delete the selected element?", {
                type: "confirm",
                buttons: [
                    { type: "submit", value: "Yes" },
                    { type: "submit", value: "No" },
                    { type: "cancel", value: "Cancel" }
                ]
            }, function (result) {
                if (result == 'Yes') {
                    //ejecuto postback
                    __doPostBack('ctl00$MainContent$btnEliminar_2903', '');
                }
            });
        }

        function llenarTablaBySearch() {
            event.preventDefault();

            var selectedDescripcion = document.getElementById('<%= txtBusqueda.ClientID%>').value;

            var inputDescripcion = document.getElementById('<%= hiddenDescripcion.ClientID%>');
            var inputAccion = document.getElementById('<%= hiddenAccion.ClientID%>');

            inputAccion.value = 1;

            inputDescripcion.value = selectedDescripcion;

            $.ajax({
                method: "POST",
                url: "ArticulosNew.aspx/buscarArticulo",
                data: '{busqueda: "' + selectedDescripcion.toString() + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: (error) => {
                    console.log(JSON.stringify(error));
                    $.msgbox("No se pudo cargar la tabla", { type: "error" });
                },
                success: successLlenarTablaBySearch,
                complete: LimpiarTabla
            });
        }

        function llenarTablaByUltimosDias(boton) {
            event.preventDefault();
            var dias;
            dias = boton.id.split('_')[2];


            $.ajax({
                method: "POST",
                url: "ArticulosNew.aspx/cargarArticulosActualizacionPrecios",
                data: '{dias: "' + dias + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: (error) => {
                    console.log(JSON.stringify(error));
                    $.msgbox("No se pudo cargar la tabla", { type: "error" });
                },
                success: successLlenarTablaBySearch,
                complete: LimpiarTabla
            });
        }

        function llenarTablaDesactualizados() {
            event.preventDefault();
            var dias = document.getElementById('<%= txtDiasDesactualizado.ClientID%>').value;

            $.ajax({
                method: "POST",
                url: "ArticulosNew.aspx/cargarArticulosDesactualizadosPrecios",
                data: '{dias: "' + dias + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: (error) => {
                    console.log(JSON.stringify(error));
                    $.msgbox("No se pudo cargar la tabla", { type: "error" });
                },
                success: successLlenarTablaBySearch,
                complete: LimpiarTabla
            });
        }

        function llenarTabla() {
            event.preventDefault();
            previousPageId = 0;
            nextPageId = 0;

            var selectedGrupo = document.getElementById('<%= ListGrupo.ClientID%>').value;
            var selectedSubGrupo = document.getElementById('<%= ListSubGrupo.ClientID%>').value;
            var selectedProveedor = document.getElementById('<%= ListProveedor.ClientID%>').value;
            var selectedFecha = document.getElementById('<%= txtDiasActualizacion.ClientID%>').value;
            var selectedMarca = document.getElementById('<%= ListMarca.ClientID%>').value;
            var selectedDescSubGrupo = document.getElementById('<%= ListSubGrupo.ClientID%>').options[document.getElementById('<%= ListSubGrupo.ClientID%>').selectedIndex].text;
            var selectedProveedorDeterminado = document.getElementById('<%= cbSoloProveedorPredeterminado.ClientID%>').checked;
            var valueProvDet = 0;

            if (selectedProveedorDeterminado == true) {
                valueProvDet = 1;
            }
            else {
                valueProvDet = 0;
            }

            var inputGrupo = document.getElementById('<%= hiddenGrupoValue.ClientID%>');
            var inputSubGrupo = document.getElementById('<%= hiddenSubGrupoValue.ClientID%>');
            var inputMarca = document.getElementById('<%= hiddenMarca.ClientID%>');
            var inputDias = document.getElementById('<%= hiddenDiasUltimaActualizacion.ClientID%>');
            var inputProveedor = document.getElementById('<%= hiddenProveedor.ClientID%>');
            var inputSoloProvDet = document.getElementById('<%= hiddenSoloProveedorPredeterminado.ClientID%>');
            var inputDescSubGrupo = document.getElementById('<%= hiddenDescSubGrupo.ClientID%>');
            var inputAccion = document.getElementById('<%= hiddenAccion.ClientID%>');

            inputAccion.value = 2;
            inputGrupo.value = selectedGrupo;
            inputSubGrupo.value = selectedSubGrupo;
            inputMarca.value = selectedMarca;
            inputDias.value = selectedFecha;
            inputProveedor.value = selectedProveedor;
            inputSoloProvDet.value = valueProvDet;
            inputDescSubGrupo.value = selectedDescSubGrupo;


            $.ajax({
                method: "POST",
                url: "ArticulosNew.aspx/getArticulosFiltrados",
                data: '{grupo: "' + selectedGrupo + '", subgrupo: "' + selectedSubGrupo + '", proveedor: "' + selectedProveedor + '", dias: "' + selectedFecha + '", marca: "' + selectedMarca + '", descSubGrupo: "' + selectedDescSubGrupo.toString() + '", soloProveedorPredeterminado: "' + valueProvDet + '", lastPageId: "' + nextPageId + '" }',
                contentType: "application/json",
                dataType: 'json',
                error: (error) => {
                    console.log(JSON.stringify(error));
                    $.msgbox("No se pudo cargar la tabla", { type: "error" });
                },
                success: successLlenarTabla,
                complete: LimpiarTabla
            });
        }

        function llenarTablaNext() {
            event.preventDefault();

            var inputGrupo = document.getElementById('<%= hiddenGrupoValue.ClientID%>').value;
            var inputSubGrupo = document.getElementById('<%= hiddenSubGrupoValue.ClientID%>').value;
            var inputMarca = document.getElementById('<%= hiddenMarca.ClientID%>').value;
            var inputDias = document.getElementById('<%= hiddenDiasUltimaActualizacion.ClientID%>').value;
            var inputProveedor = document.getElementById('<%= hiddenProveedor.ClientID%>').value;
            var inputSoloProvDet = document.getElementById('<%= hiddenSoloProveedorPredeterminado.ClientID%>').value;
            var inputDescSubGrupo = document.getElementById('<%= hiddenDescSubGrupo.ClientID%>').value;

            $.ajax({
                method: "POST",
                url: "ArticulosNew.aspx/getArticulosFiltrados",
                data: '{grupo: "' + inputGrupo + '", subgrupo: "' + inputSubGrupo + '", proveedor: "' + inputProveedor + '", dias: "' + inputDias + '", marca: "' + inputMarca + '", descSubGrupo: "' + inputDescSubGrupo.toString() + '", soloProveedorPredeterminado: "' + inputSoloProvDet + '", lastPageId: "' + nextPageId + '" }',
                contentType: "application/json",
                dataType: 'json',
                error: (error) => {
                    console.log(JSON.stringify(error));
                    $.msgbox("No se pudo cargar la tabla", { type: "error" });
                },
                success: successLlenarTabla,
                complete: LimpiarTabla
            });
        }

        function llenarTablaPrevious() {
            event.preventDefault();

            var inputGrupo = document.getElementById('<%= hiddenGrupoValue.ClientID%>').value;
            var inputSubGrupo = document.getElementById('<%= hiddenSubGrupoValue.ClientID%>').value;
            var inputMarca = document.getElementById('<%= hiddenMarca.ClientID%>').value;
            var inputDias = document.getElementById('<%= hiddenDiasUltimaActualizacion.ClientID%>').value;
            var inputProveedor = document.getElementById('<%= hiddenProveedor.ClientID%>').value;
            var inputSoloProvDet = document.getElementById('<%= hiddenSoloProveedorPredeterminado.ClientID%>').value;
            var inputDescSubGrupo = document.getElementById('<%= hiddenDescSubGrupo.ClientID%>').value;

            $.ajax({
                method: "POST",
                url: "ArticulosNew.aspx/getArticulosFiltradosPrevious",
                data: '{grupo: "' + inputGrupo + '", subgrupo: "' + inputSubGrupo + '", proveedor: "' + inputProveedor + '", dias: "' + inputDias + '", marca: "' + inputMarca + '", descSubGrupo: "' + inputDescSubGrupo.toString() + '", soloProveedorPredeterminado: "' + inputSoloProvDet + '", lastPageId: "' + previousPageId + '" }',
                contentType: "application/json",
                dataType: 'json',
                error: (error) => {
                    console.log(JSON.stringify(error));
                    $.msgbox("No se pudo cargar la tabla", { type: "error" });
                },
                success: successLlenarTabla,
                complete: LimpiarTabla
            });
        }

        function successLlenarTabla(response) {
            event.preventDefault();

            var obj = JSON.parse(response.d);
            if (obj.length == 0) {
                //alert("Este listado no contiene mas paginas");
                return;
            }
            document.getElementById('<%= btnNext.ClientID%>').style.visibility = "visible";
            document.getElementById('<%= btnPrevious.ClientID%>').style.visibility = "visible";
            $("#dataTables-example").dataTable().fnDestroy();
            $('#dataTables-example').find("tr:gt(0)").remove();

            for (var i = 0; i < obj.length; i++) {
                $('#dataTables-example').append
                    (
                    "<tr>" +
                    "<td> " + obj[i].Id + "</td>" +
                    "<td> " + obj[i].Codigo + "</td>" +
                    "<td> " + obj[i].Descripcion + "</td>" +
                    "<td> " + obj[i].Grupo + "</td>" +
                    "<td> " + obj[i].SubGrupo + "</td>" +
                    "<td> " + obj[i].Marca + "</td>" +
                    "<td> " + obj[i].UltimaActualizacion + "</td>" +
                    "<td> " + obj[i].Proveedor + "</td>" +
                    "<td style='text-align:right'> " + "$ " + obj[i].PVenta + "</td>" +
                    "<td> " + obj[i].ApareceLista + "</td>" +
                    "<td> <a href=\"ArticulosABM.aspx?accion=2&id=" + obj[i].Id + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Ver y/o Editar\" >" +
                    "<i class=\"shortcut-icon icon-search\"></i> </a> " +

                    "<a href=\"StockF.aspx?articulo=" + obj[i].Id + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Stock\" >" +
                    "<i class=\"shortcut-icon icon-list-alt\"></i> </a> " +

                    "<a href=\"../MateriasPrimas/MateriasPrimas_Composicion.aspx?idArt=" + obj[i].Id + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Composicion\" >" +
                    "<i class=\"shortcut-icon icon-dropbox\"></i> </a> " +

                    "<a href=\"#modalConfirmacion\" class=\"btn btn-info \" data-toggle=\"modal\" OnClientClick=\"abrirdialog(" + obj[i].Id + ")\" >" +
                    "<i class=\"shortcut-icon icon-trash\"></i> </a> " +
                    "</td > " +
                    "</tr>"
                    );
                if (i == 0) {
                    previousPageId = obj[i].Id;
                }
                if (i == 49) {
                    nextPageId = obj[i].Id;
                }
            }

            $('#dataTables-example').dataTable(
                {
                    "bLengthChange": true,
                    "LengthChange": true,
                    "bFilter": false,
                    "bInfo": false,
                    "bPaginate": false,
                    "bAutoWidth": false,
                    "bStateSave": true,
                    "bSort": false,
                    "pageLength": 50,
                    "sPaginationType": "simple"
                });
        }

        function successLlenarTablaBySearch(response) {
            event.preventDefault();

            var obj = JSON.parse(response.d);
            if (obj.length == 0) {
                //alert("Este listado no contiene mas paginas");
                return;
            }

            document.getElementById('<%= btnNext.ClientID%>').style.visibility = "hidden";
            document.getElementById('<%= btnPrevious.ClientID%>').style.visibility = "hidden";

            $("#dataTables-example").dataTable().fnDestroy();
            $('#dataTables-example').find("tr:gt(0)").remove();

            for (var i = 0; i < obj.length; i++) {
                $('#dataTables-example').append
                    (
                    "<tr>" +
                    "<td> " + obj[i].Id + "</td>" +
                    "<td> " + obj[i].Codigo + "</td>" +
                    "<td> " + obj[i].Descripcion + "</td>" +
                    "<td> " + obj[i].Grupo + "</td>" +
                    "<td> " + obj[i].SubGrupo + "</td>" +
                    "<td> " + obj[i].Marca + "</td>" +
                    "<td> " + obj[i].UltimaActualizacion + "</td>" +
                    "<td> " + obj[i].Proveedor + "</td>" +
                    "<td style='text-align:right'> " + "$ " + obj[i].PVenta + "</td>" +
                    "<td> " + obj[i].ApareceLista + "</td>" +
                    "<td> <a href=\"ArticulosABM.aspx?accion=2&id=" + obj[i].Id + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Ver y/o Editar\" >" +
                    "<i class=\"shortcut-icon icon-search\"></i> </a> " +

                    "<a href=\"StockF.aspx?articulo=" + obj[i].Id + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Stock\" >" +
                    "<i class=\"shortcut-icon icon-list-alt\"></i> </a> " +

                    "<a href=\"../MateriasPrimas/MateriasPrimas_Composicion.aspx?idArt=" + obj[i].Id + "\" class=\"btn btn-info ui-tooltip\" data-toggle=\"tooltip\" title data-original-title=\"Composicion\" >" +
                    "<i class=\"shortcut-icon icon-dropbox\"></i> </a> " +

                    "<a href=\"#modalConfirmacion\" class=\"btn btn-info \" data-toggle=\"modal\" OnClientClick=\"abrirdialog(" + obj[i].Id + ")\" >" +
                    "<i class=\"shortcut-icon icon-trash\"></i> </a> " +
                    "</td > " +
                    "</tr>"
                    );
                if (i == 0) {
                    previousPageId = obj[i].Id;
                }
                if (i == 49) {
                    nextPageId = obj[i].Id;
                }
            }

            $('#dataTables-example').dataTable(
                {
                    "bLengthChange": false,
                    "LengthChange": false,
                    "bFilter": false,
                    "bInfo": false,
                    "bPaginate": false,
                    "bAutoWidth": false,
                    "bStateSave": true,
                    "bSort": false,
                    "pageLength": 50,
                    "sPaginationType": "simple"
                });
        }

        function LimpiarTabla() {
            event.preventDefault();
            document.getElementById('btnCerrarFlitro').click();
            document.getElementById('btnCerrarDesactualizados').click();
            $.ajax({
                method: "POST",
                url: "ArticulosNew.aspx/getVistaTablaArticulos",
                contentType: "application/json",
                dataType: 'json',
                error: (error) => {
                    console.log(JSON.stringify(error));
                    $.msgbox("Error al cargar la visualizacion", { type: "error" });
                },
                success: function (vista) {
                    var obj = JSON.parse(vista.d);
                    var columnaID = document.getElementById('<%= ColumnaId.ClientID%>');
                    columnaID.style.display = "none";
                    var rows = document.getElementById('dataTables-example').rows;
                    var cols = null;
                    for (var row = 0; row < rows.length; row++) {
                        cols = rows[row].cells;
                        cols[0].style.display = 'none';
                    }

                    if (obj.columnaGrupo == 0) {
                        var columnaGrupo = document.getElementById('<%= ColumnaGrupo.ClientID%>');
                        columnaGrupo.style.display = "none";
                        for (row = 0; row < rows.length; row++) {
                            cols = rows[row].cells;
                            cols[3].style.display = 'none';
                        }
                    }

                    if (obj.columnaSubGrupo == 0) {
                        var columnaSubGrupo = document.getElementById('<%= ColumnaSubgrupo.ClientID%>');
                        columnaSubGrupo.style.display = "none";
                        for (row = 0; row < rows.length; row++) {
                            cols = rows[row].cells;
                            cols[4].style.display = 'none';
                        }
                    }

                    if (obj.columnaMarca == 0) {
                        var columnaMarca = document.getElementById('<%= ColumnaMarca.ClientID%>');
                        columnaMarca.style.display = "none";
                        for (row = 0; row < rows.length; row++) {
                            cols = rows[row].cells;
                            cols[5].style.display = 'none';
                        }
                    }

                    if (obj.columnaActualizacion == 0) {
                        var columnaActualizacion = document.getElementById('<%= ColumnaUltimaAct.ClientID%>');
                        columnaActualizacion.style.display = "none";
                        for (row = 0; row < rows.length; row++) {
                            cols = rows[row].cells;
                            cols[6].style.display = 'none';
                        }
                    }

                    if (obj.columnaProveedores == 0) {
                        var columnaProveedores = document.getElementById('<%= ColumnaProveedor.ClientID%>');
                        columnaProveedores.style.display = "none";
                        for (row = 0; row < rows.length; row++) {
                            cols = rows[row].cells;
                            cols[7].style.display = 'none';
                        }
                    }

                    if (obj.columnaPrecioVentaMonedaOriginal == 0) {
                        var columnaPrecioVentaMonedaOriginal = document.getElementById('<%= ColumnaPVenta.ClientID%>');
                        columnaPrecioVentaMonedaOriginal.style.display = "none";
                        for (row = 0; row < rows.length; row++) {
                            cols = rows[row].cells;
                            cols[8].style.display = 'none';
                        }
                    }

                    var columnaAparece = document.getElementById('<%= ColumnaApareceLista.ClientID%>');
                    columnaAparece.style.display = "none";
                    for (row = 0; row < rows.length; row++) {
                        cols = rows[row].cells;
                        cols[9].style.display = 'none';
                    }
                }
            });
        }


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
                if (key == 45 || key == 46 || key == 8 || key == 44)// Detectar . (punto) y backspace (retroceso) y , (coma)
                { return true; }
                else { return false; }
            }
            return true;
        }
    </script>


</asp:Content>
