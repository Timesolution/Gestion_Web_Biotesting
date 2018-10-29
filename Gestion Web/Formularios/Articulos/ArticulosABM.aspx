<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ArticulosABM.aspx.cs" Inherits="Gestion_Web.Formularios.Articulos.ArticulosABM"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Maestro > Articulos > Articulo</h5>
                        <asp:Label ID="lblFiltroAnterior" runat="server" Style="display: none;"></asp:Label>
                    </div>
                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Articulos</h3>

                    </div>
                    <!-- /.widget-header -->
                    <div class="widget-content">
                        <div class="bs-example">
                            <ul id="myTab" class="nav nav-tabs">
                                <li class="active"><a href="#home" data-toggle="tab">Detalle</a></li>
                                <li class=""><a href="#profile" data-toggle="tab" runat="server" id="linkProv" visible="false">Otros Proveedores</a></li>
                                <li class=""><a href="#Image" data-toggle="tab" runat="server" id="linkImg" visible="false">Imagenes</a></li>
                                <li class=""><a href="#Composition" data-toggle="tab" runat="server" id="linkCompo" visible="false">Composicion</a></li>
                                <li class=""><a href="#Descuentos" runat="server" id="linkDesc" visible="false" data-toggle="tab">Descuentos</a></li>
                                <li class=""><a href="#Store" runat="server" id="linkStore" visible="false" data-toggle="tab">Store</a></li>
                                <li class=""><a href="#Combustible" runat="server" id="linkCombustible" visible="false" data-toggle="tab">Datos Combustible</a></li>
                                <li class=""><a href="#Extras" runat="server" id="linkExtras" visible="false" data-toggle="tab">Datos Extra</a></li>
                                <li class=""><a href="#Costos" runat="server" id="linkCostos" visible="false" data-toggle="tab">Historial Costos</a></li>
                                <li class=""><a href="#Medidas" runat="server" id="linkMedidas" visible="false" data-toggle="tab">Cant. de venta</a></li>
                                <li class=""><a href="#Beneficios" runat="server" id="linkBeneficios" visible="false" data-toggle="tab">Sist. Beneficios</a></li>
                                <li class=""><a href="#ArticulosSucursales" runat="server" id="linkArticulosSucursales" visible="false" data-toggle="tab">Articulos Sucursales</a></li>
                                <li class=""><a href="#StockMinimoSucursales" runat="server" id="linkStockMinimoSucursales" visible="false" data-toggle="tab">Stock Minimo Sucursales</a></li>
                                <li class=""><a href="#Catalogo" runat="server" id="linkCatalogo" visible="false" data-toggle="tab">Otros </a></li>
                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane fade active in" id="home">
                                    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div id="validation-form" role="form" class="form-horizontal col-md-10">
                                                <fieldset>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Codigo Articulo</label>

                                                        <div class="col-md-4">

                                                            <asp:TextBox ID="txtCodArticulo" runat="server" class="form-control"></asp:TextBox>

                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCodArticulo" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Codigo de Barra</label>

                                                        <div class="col-md-4">

                                                            <asp:TextBox ID="txtCodigoBarra" runat="server" class="form-control" Text="0"></asp:TextBox>


                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCodigoBarra" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Descripción</label>
                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="txtDescripcion" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>

                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtDescripcion" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Proveedor</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="DropListProveedor" runat="server" class="form-control"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-1">
                                                            <a class="btn btn-info" data-toggle="modal" href="../Clientes/ClientesABM.aspx?accion=3"><i class="shortcut-icon icon-plus"></i></a>
                                                        </div>

                                                        <div class="col-md-3">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Seleccione un Proveedor" ControlToValidate="DropListProveedor" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Marca</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="DropListMarca" runat="server" class="form-control"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-1">
                                                            <a class="btn btn-info" data-toggle="modal" href="#modalMarca"><i class="shortcut-icon icon-plus"></i></a>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator33" runat="server" ErrorMessage="Seleccione una marca" ControlToValidate="DropListMarca" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Grupo</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="DropListGrupo" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="DropListGrupo_SelectedIndexChanged"></asp:DropDownList>

                                                        </div>
                                                        <div class="col-md-1">
                                                            <a class="btn btn-info" data-toggle="modal" href="#modalGrupo"><i class="shortcut-icon icon-plus"></i></a>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="Seleccione un Grupo" ControlToValidate="DropListGrupo" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Sub-Grupo</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="DropDownSubGrupo" runat="server" class="form-control"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-1">
                                                            <a class="btn btn-info" data-toggle="modal" href="#modalSubGrupo"><i class="shortcut-icon icon-plus"></i></a>
                                                        </div>

                                                        <div class="col-md-3">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="Seleccione un Sub-Grupo" ControlToValidate="DropDownSubGrupo" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Tipo</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="ListTipoDistribucion" runat="server" class="form-control">
                                                                <asp:ListItem Value="1">Distribución</asp:ListItem>
                                                                <asp:ListItem Value="2">Fabricación</asp:ListItem>
                                                            </asp:DropDownList>

                                                        </div>

                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator50" runat="server" ErrorMessage="Seleccione un tipo" ControlToValidate="ListTipoDistribucion" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Moneda de Venta</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="DropDownMonedaVent" AutoPostBack="true" runat="server" class="form-control" OnSelectedIndexChanged="DropDownMonedaVent_SelectedIndexChanged"></asp:DropDownList>

                                                        </div>

                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="Seleccione una Moneda" ControlToValidate="DropDownMonedaVent" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Porcentaje IVA</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="DropListPorcentajeIVA" runat="server" class="form-control">
                                                                <asp:ListItem Value="-1">Seleccione..</asp:ListItem>
                                                                <asp:ListItem Value="0">0%</asp:ListItem>
                                                                <asp:ListItem Value="10.5">10,5%</asp:ListItem>
                                                                <asp:ListItem Value="21">21%</asp:ListItem>
                                                            </asp:DropDownList>

                                                        </div>

                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="Seleccione un Porcentaje" ControlToValidate="DropListPorcentajeIVA" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <asp:Panel runat="server" ID="panelCosto" Visible="true">
                                                        <div class="form-group">

                                                            <label for="name" class="col-md-4">Costo</label>
                                                            <div class="col-md-4">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txtCosto" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCosto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtCosto" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                            </div>

                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Incidencia</label>

                                                            <div class="col-md-4">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">%</span>
                                                                    <asp:TextBox ID="txtIncidencia" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Text="0" Style="text-align: right;"></asp:TextBox>
                                                                </div>
                                                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtIncidencia" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtIncidencia" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                            </div>
                                                        </div>
                                                    </asp:Panel>

                                                    <asp:Panel runat="server" ID="panelCosto2" Visible="true">
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Costo Imponible</label>

                                                            <div class="col-md-4">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <input id="tCostoImponible" runat="server" class="form-control" disabled="" value="0" style="text-align: right" onkeypress="javascript:return validarNro(event)" />
                                                                </div>

                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:Literal ID="Literal2" runat="server" Text="Cotizacion: $"></asp:Literal>
                                                                <asp:Literal ID="LitCotizacionCosto" runat="server" Text="-"></asp:Literal>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="tCostoImponible" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="tCostoImponible" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Costo Iva</label>

                                                            <div class="col-md-4">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <input id="txtCostoIva" runat="server" class="form-control" disabled="" value="0" style="text-align: right" onkeypress="javascript:return validarNro(event)" />

                                                                </div>

                                                            </div>
                                                            <div class="col-md-4">
                                                            </div>
                                                            <div class="col-md-4">
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Impuestos Internos</label>

                                                            <div class="col-md-4">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">%</span>
                                                                    <asp:TextBox ID="txtImpInternos" Style="text-align: right;" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Text="0"></asp:TextBox>

                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtImpInternos" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtImpInternos" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Ingresos Brutos</label>

                                                            <div class="col-md-4">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">%</span>
                                                                    <asp:TextBox ID="txtIngBrutos" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" Text="0"></asp:TextBox>

                                                                </div>

                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtIngBrutos" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtIngBrutos" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Costo Real en $</label>

                                                            <div class="col-md-4">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <input id="tCostoReal" runat="server" class="form-control" disabled="" value="0" style="text-align: right" onkeypress="javascript:return validarNro(event)" />
                                                                    <%--                                                        <asp:TextBox ID="txtCostoReal" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Text="0" Style="text-align: right;" disabled ReadOnly="true"></asp:TextBox>--%>
                                                                </div>
                                                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                            </div>
                                                            <div class="col-md-1">
                                                                <asp:LinkButton ID="btnCalcCostos" class="btn btn-info" runat="server" Text="<span class='shortcut-icon icon-refresh'></span>" OnClick="btnCalcCostos_Click" />
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="tCostoReal" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="tCostoReal" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                            </div>
                                                        </div>

                                                        <asp:Panel ID="panelCostoRealMonedaOriginal" Visible="false" runat="server">
                                                            <div class="form-group">
                                                                <label for="name" class="col-md-4">Costo Real en Moneda Original</label>

                                                                <div class="col-md-4">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">$</span>
                                                                        <input id="tCostoRealMonedaOriginal" runat="server" class="form-control" disabled="" value="0" style="text-align: right" />

                                                                    </div>

                                                                </div>
                                                                <div class="col-md-4">
                                                                </div>
                                                                <div class="col-md-4">
                                                                </div>
                                                            </div>
                                                        </asp:Panel>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Margen</label>

                                                            <div class="col-md-4">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">%</span>
                                                                    <asp:TextBox ID="txtMargen" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Text="0" Value="0" Style="text-align: right;"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtMargen" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtMargen" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                            </div>
                                                        </div>
                                                    </asp:Panel>

                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Precio Sin IVA</label>

                                                        <div class="col-md-4">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <input id="tPrecioSinIva" runat="server" class="form-control" disabled="" value="0" style="text-align: right" onkeypress="javascript:return validarNro(event)" />

                                                            </div>

                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="tPrecioSinIva" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="tPrecioSinIva" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Precio de Venta</label>

                                                        <div class="col-md-4">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <input id="tPrecioVenta" runat="server" class="form-control" text="0" style="text-align: right" onkeypress="javascript:return validarNro(event)" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1">
                                                            <asp:LinkButton ID="lbGenerarPrecioVenta" class="btn btn-info" runat="server" Text="<span class='shortcut-icon icon-refresh'></span>" OnClick="lbGenerarPrecioVenta_Click" />
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="tPrecioVenta" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ControlToValidate="tPrecioVenta" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                        </div>
                                                    </div>

                                                    <asp:Panel ID="panelMonedaOriginal" Visible="false" runat="server">
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Precio de Venta en Moneda Original</label>

                                                            <div class="col-md-4">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">
                                                                        <asp:Literal ID="litSimboloPesos" runat="server"></asp:Literal></span>
                                                                    <input id="tPrecioVentaMonedaOriginal" runat="server" class="form-control" disabled="" value="0" style="text-align: right" />

                                                                </div>

                                                            </div>
                                                            <div class="col-md-4">
                                                            </div>
                                                            <div class="col-md-4">
                                                            </div>
                                                        </div>
                                                    </asp:Panel>


                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Stock Minimo</label>

                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="txtStock" Text="0" runat="server" class="form-control" Style="text-align: right;" TextMode="Number" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                            <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtStock" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Activo</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="DropListAparece" runat="server" class="form-control">
                                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Ubicacion</label>

                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="txtUbicacion" runat="server" class="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Presentaciones</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="ListPresentaciones" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListPresentaciones_SelectedIndexChanged">
                                                                <asp:ListItem>NO</asp:ListItem>
                                                                <asp:ListItem>SI</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <asp:Panel ID="panelPresentaciones" runat="server" Visible="false">
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Minima</label>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtPresentacionMin" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Media</label>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtPresentacionMed" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Maxima</label>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtPresentacionMax" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel runat="server" ID="panelFecha" Visible="true">
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Fecha Alta</label>

                                                            <div class="col-md-8">

                                                                <asp:TextBox ID="txtFechaAlta" runat="server" class="form-control" disabled></asp:TextBox>

                                                            </div>
                                                        </div>


                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Ultima Actualizacion</label>

                                                            <div class="col-md-8">

                                                                <asp:TextBox ID="txtUltModificacion" runat="server" name="fecha" class="form-control" disabled></asp:TextBox>


                                                            </div>
                                                        </div>


                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Modificado</label>

                                                            <div class="col-md-8">

                                                                <asp:TextBox ID="txtModificado" runat="server" name="fecha" class="form-control" disabled></asp:TextBox>


                                                            </div>
                                                        </div>
                                                    </asp:Panel>



                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Procedencia</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="DropListPais" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListPais_SelectedIndexChanged"></asp:DropDownList>

                                                        </div>

                                                        <div class="col-md-1">
                                                            <a class="btn btn-info" data-toggle="modal" href="#modalPais"><i class="shortcut-icon icon-plus"></i></a>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ErrorMessage="Seleccione un Pais" ControlToValidate="DropListPais" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <asp:Panel ID="panelDespacho" runat="server" Visible="false">
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Fecha despacho</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtFechaDespacho" runat="server" class="form-control" Text="01/01/2001"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Numero despacho</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtNumeroDespacho" runat="server" class="form-control" Text="0"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Numero lote</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtLote" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Fecha vencimiento</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtVencimiento" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Sub Lista</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="DropListSubLista" runat="server" class="form-control"></asp:DropDownList>

                                                        </div>

                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ErrorMessage="Seleccione una SubLista" ControlToValidate="DropListSubLista" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-4">Observacion</label>
                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="txtObservacion" runat="server" TextMode="MultiLine" Rows="4" class="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-4">Alerta</label>
                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="txtAlerta" runat="server" TextMode="MultiLine" Rows="4" class="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                </fieldset>

                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="lbGenerarPrecioVenta" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <div class="col-md-8">
                                        <asp:Button ID="btnAgregar" runat="server" Text="Guardar" Visible="false" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="ArticuloGroup" />
                                        <asp:Button ID="btnAgregarSig" runat="server" Text="Guardar y Siguiente" Visible="false" class="btn btn-success" OnClick="btnAgregarSig_Click" ValidationGroup="ArticuloGroup" />
                                        <asp:Button ID="btnDuplicar" runat="server" Text="Duplicar" Visible="false" class="btn btn-success" OnClick="btnDuplicar_Click" ValidationGroup="ArticuloGroup" />
                                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="../Articulos/Articulos.aspx" />
                                    </div>
                                </div>
                                <%--Otros Proveedores--%>
                                <div class="tab-pane fade" id="profile">
                                    <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div class="widget-content">
                                                <div class="col-md-12" style="padding-right: 0px; padding-left: 0px;">
                                                    <div class="widget stacked widget-table">

                                                        <div class="widget-header">
                                                            <span class="icon-group"></span>
                                                            <h3>Otros Proveedores</h3>
                                                        </div>
                                                        <!-- .widget-header -->

                                                        <div class="widget-content">
                                                            <table class="table table-bordered table-striped">

                                                                <thead>
                                                                    <tr>
                                                                        <th style="width: 15%">Proveedor</th>
                                                                        <th style="width: 5%">Codigo Proveedor</th>
                                                                        <th style="width: 10%; text-align: right;">Precio</th>
                                                                        <th style="width: 5%">% Iva</th>
                                                                        <th style="width: 5%">Moneda</th>
                                                                        <th style="width: 5%; text-align: right;">Dto Final</th>
                                                                        <th style="width: 10%; text-align: right;">Pr. Final</th>
                                                                        <th style="width: 10%; text-align: right;">Pr. Final IVA</th>
                                                                        <th style="width: 10%; text-align: right;">Final Pesos</th>
                                                                        <th style="width: 10%">Actualizacion</th>
                                                                        <th style="width: 15%">Predeterminado</th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:PlaceHolder ID="phProveedorArticulo" runat="server"></asp:PlaceHolder>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                        <!-- .widget-content -->
                                                    </div>

                                                </div>

                                                <asp:Button ID="btnSetearProv" runat="server" class="btn btn-success" Text="Predeterminar" OnClick="btnSetearProv_Click"></asp:Button>
                                            </div>

                                            <div class="widget-content">
                                                <div role="form" class="form-horizontal col-md-10">

                                                    <br />
                                                    <fieldset>
                                                        <div class="form-group">
                                                            <label for="validateSelect" class="col-md-4">Proveedor</label>
                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="DropListProveedores2" runat="server" class="form-control">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-1">
                                                                <a class="btn btn-info" data-toggle="modal" href="../Clientes/ClientesABM.aspx?accion=3"><i class="shortcut-icon icon-plus"></i></a>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="Seleccione un proveedor" ControlToValidate="DropListProveedores2" InitialValue="-1" ValidationGroup="ProveedorArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Codigo Proveedor</label>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtCodigoProveedor" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCodigoProveedor" ValidationGroup="ProveedorArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="validateSelect" class="col-md-4">Moneda</label>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList ID="DropListMoneda2" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="DropListMoneda2_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:Literal ID="Literal1" runat="server" Text="Cotizacion: $"></asp:Literal>
                                                                <asp:Literal ID="ltCotizacion" runat="server" Text="-"></asp:Literal>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="DropListMoneda2" InitialValue="-1" ValidationGroup="ProveedorArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Precio</label>

                                                            <div class="col-md-4">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txtPrecio" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtPrecio" ValidationGroup="ProveedorArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Descuento 1</label>

                                                            <div class="col-md-2">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">%</span>
                                                                    <asp:TextBox ID="txtDescuento" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ErrorMessage="*" ControlToValidate="txtDescuento" ValidationGroup="ProveedorArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>

                                                            <%--<label for="name" class="col-md-2">Descripcion</label>--%>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtDescripcionDesc" runat="server" class="form-control" placeholder="Descripcion"></asp:TextBox>
                                                            </div>


                                                        </div>

                                                        <%--<div class="form-group">
                                                            <label for="name" class="col-md-4">Descripcion descuento</label>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtDescripcionDesc" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCodigoProveedor" ValidationGroup="ProveedorArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>--%>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Descuento 2</label>

                                                            <div class="col-md-2">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">%</span>
                                                                    <asp:TextBox ID="txtDescuento2" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ErrorMessage="*" ControlToValidate="txtDescuento2" ValidationGroup="ProveedorArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>

                                                            <%--<label for="name" class="col-md-2">Descripcion</label>--%>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtDescripcionDesc2" runat="server" class="form-control" placeholder="Descripcion"></asp:TextBox>
                                                            </div>

                                                        </div>

                                                        <%--<div class="form-group">
                                                            <label for="name" class="col-md-4">Descripcion descuento</label>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtDescripcionDesc2" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCodigoProveedor" ValidationGroup="ProveedorArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>--%>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Descuento 3</label>

                                                            <div class="col-md-2">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">%</span>
                                                                    <asp:TextBox ID="txtDescuento3" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ErrorMessage="*" ControlToValidate="txtDescuento3" ValidationGroup="ProveedorArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>

                                                            <%--<label for="name" class="col-md-2">Descripcion</label>--%>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtDescripcionDesc3" runat="server" class="form-control" placeholder="Descripcion"></asp:TextBox>
                                                            </div>

                                                        </div>

                                                        <%--<div class="form-group">
                                                            <label for="name" class="col-md-4">Descripcion descuento</label>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtDescripcionDesc3" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator37" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCodigoProveedor" ValidationGroup="ProveedorArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>--%>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Descuento Final</label>

                                                            <div class="col-md-2">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">%</span>
                                                                    <asp:TextBox ID="txtDescuentoFinal" runat="server" class="form-control" disabled></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-1">
                                                                <asp:LinkButton ID="btnCalcularDesc" class="btn btn-info" runat="server" Text="<span class='shortcut-icon icon-refresh'></span>" OnClick="btnCalcularDesc_Click" />
                                                            </div>

                                                        </div>


                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Precio Final</label>

                                                            <div class="col-md-4">

                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txtPrecioFinal" runat="server" class="form-control" disabled onkeypress="javascript:return validarNro(event)" Style="text-align: right"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtPrecioFinal" ValidationGroup="ProveedorArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Precio Pesos</label>

                                                            <div class="col-md-4">

                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txtPrecioPesos" runat="server" class="form-control" disabled onkeypress="javascript:return validarNro(event)" Style="text-align: right"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtPrecioPesos" ValidationGroup="ProveedorArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>


                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Fecha Actualizacion</label>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtFechaAct" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Style="text-align: right"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator34" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtFechaAct" ValidationGroup="ProveedorArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="validateSelect" class="col-md-4">Porcentaje IVA</label>
                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="DropListIva" runat="server" class="form-control">
                                                                    <asp:ListItem Value="-1">Seleccione..</asp:ListItem>
                                                                    <asp:ListItem Value="0.00">0%</asp:ListItem>
                                                                    <asp:ListItem Value="10.50">10,5%</asp:ListItem>
                                                                    <asp:ListItem Value="21.00">21%</asp:ListItem>
                                                                </asp:DropDownList>

                                                            </div>

                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator35" runat="server" ErrorMessage="Seleccione un Porcentaje" ControlToValidate="DropListIVA" InitialValue="-1" ValidationGroup="ProveedorArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <div class="col-md-4">
                                                                <asp:Button ID="btnAgregarProvArt" runat="server" Text="Agregar" class="btn btn-success" ValidationGroup="ProveedorArticulo" OnClick="btnAgregarProvArt_Click" />
                                                            </div>
                                                        </div>

                                                    </fieldset>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnAgregarProvArt" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <%--Fin otros proveedores--%>
                                <%-- Imagenes --%>
                                <div class="tab-pane fade" id="Image">
                                    <asp:UpdatePanel ID="UpdatePanelMedidas" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div class="col-md-12 col-xs-12">
                                                <div class="widget stacked">

                                                    <div class="widget-header">
                                                        <i class="icon-wrench"></i>
                                                        <h3>Herramientas</h3>
                                                    </div>
                                                    <!-- /widget-header -->

                                                    <div class="widget-content">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 90%"></td>
                                                                <td style="width: 5%">
                                                                    <div class="shortcuts" style="height: 100%">



                                                                        <a class="btn btn-success" data-toggle="modal" href="#modalImagen" style="width: 100%">
                                                                            <i class="shortcut-icon icon-plus"></i>
                                                                        </a>
                                                                    </div>
                                                                </td>
                                                                <td style="width: 5%">
                                                                    <div class="shortcuts" style="height: 100%">
                                                                        <asp:LinkButton ID="lbtnAgregar" runat="server" Text="<span class='shortcut-icon icon-trash'></span>" class="btn btn-success" Style="width: 100%" OnClick="lbtnAgregar_Click1" />

                                                                    </div>
                                                                </td>
                                                            </tr>

                                                        </table>
                                                    </div>
                                                    <!-- /widget-content -->

                                                </div>
                                                <!-- /widget -->
                                            </div>
                                            <div class="col-md-12 col-xs-12">

                                                <div class="widget stacked">

                                                    <div class="widget-header">
                                                        <i class="icon-th-large"></i>
                                                        <h3>Galeria de Imagenes</h3>
                                                    </div>
                                                    <!-- /widget-header -->

                                                    <div class="widget-content">

                                                        <ul class="gallery-container">

                                                            <table class="table table-bordered table-striped" id="tbImagenes">
                                                                <thead>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:PlaceHolder ID="phImagenesArticulos" runat="server"></asp:PlaceHolder>
                                                                </tbody>
                                                            </table>

                                                        </ul>

                                                    </div>
                                                    <!-- /widget-content -->

                                                </div>

                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <%-- Fin Imagenes --%>
                                <%--Composicion--%>
                                <div class="tab-pane fade" id="Composition">
                                    <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                                        <ContentTemplate>

                                            <div class="widget-content">
                                                <div role="form" class="form-horizontal col-md-10">

                                                    <br />
                                                    <fieldset>
                                                        <div class="form-group">
                                                            <label for="validateSelect" class="col-md-4">Busqueda</label>
                                                            <div class="col-md-4">
                                                                <div class="input-group">
                                                                    <asp:TextBox ID="txtBusqueda" runat="server" class="form-control" placeholder="Buscar Articulo"></asp:TextBox>
                                                                    <span class="input-group-btn">
                                                                        <asp:LinkButton ID="lbBuscar" runat="server" Text="Buscar" class="btn btn-primary" OnClick="lbBuscar_Click" ValidationGroup="BusquedaArticuloCompuesto">
                                                                            <i class="shortcut-icon icon-search"></i></asp:LinkButton>
                                                                    </span>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Ingrese un Codigo" ControlToValidate="txtBusqueda" ValidationGroup="BusquedaArticuloCompuesto" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Articulos</label>
                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="DropListArticulosComp" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListArticulosComp_SelectedIndexChanged"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ErrorMessage="*" ControlToValidate="DropListArticulosComp" InitialValue="-1" ValidationGroup="ArticuloCompuesto" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Descripcion</label>

                                                            <div class="col-md-8">
                                                                <asp:TextBox ID="txtDescripcionArticulo" runat="server" class="form-control" Disabled=""></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Cantidad</label>

                                                            <div class="col-md-3">


                                                                <asp:TextBox ID="txtCantidadArticulo" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Style="text-align: right"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-1">
                                                                <asp:LinkButton ID="lbAgregarArticuloComp" runat="server" Text="Buscar" class="btn btn-success" OnClick="lbAgregarArticuloComp_Click" ValidationGroup="ArticuloCompuesto">
                                                                            <i class="shortcut-icon icon-ok"></i></asp:LinkButton>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCantidadArticulo" ValidationGroup="ArticuloCompuesto" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">

                                                            <div class="col-md-4">
                                                                <asp:TextBox runat="server" ID="txtArticuloCompuesto" Visible="false"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <asp:Label runat="server" ID="lbAvisoCompuesto" class="alert alert-danger alert-dismissable" Visible="false"></asp:Label>
                                                            </div>
                                                        </div>

                                                    </fieldset>
                                                </div>
                                            </div>

                                            <div class="widget-content">
                                                <div class="col-md-12">
                                                    <div class="widget stacked widget-table">

                                                        <div class="widget-header">
                                                            <span class="icon-bookmark"></span>
                                                            <h3>Composicion</h3>
                                                        </div>
                                                        <!-- .widget-header -->

                                                        <div class="widget-content">
                                                            <table class="table table-bordered table-striped">

                                                                <thead>
                                                                    <tr>
                                                                        <th>Codigo Articulo</th>
                                                                        <th>Descripcion</th>
                                                                        <th>Cantidad</th>
                                                                        <th>Costo</th>
                                                                        <th></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:PlaceHolder ID="phArticulosCompuestos" runat="server"></asp:PlaceHolder>
                                                                </tbody>

                                                            </table>

                                                        </div>
                                                        <!-- .widget-content -->

                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-2">Costo Total:</label>
                                                        <div class="col-md-2">
                                                            <asp:TextBox runat="server" ID="txtCostoTotalComposicion" class="form-control" disabled Style="text-align: right;"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-1">
                                                            <asp:LinkButton ID="lbtnCostocompuesto" runat="server" Text="Buscar" class="btn btn-success ui-tooltip" title data-original-title="Asignar Costo" OnClick="lbtnCostocompuesto_Click">
                                                                    <i class="shortcut-icon icon-ok"></i>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="lbAgregarArticuloComp" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="lbBuscar" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <%--Fin Composicion--%>
                                <%--Descuentos--%>
                                <div class="tab-pane fade" id="Descuentos">
                                    <asp:UpdatePanel ID="UpdatePanel5" UpdateMode="Always" runat="server">
                                        <ContentTemplate>

                                            <div class="widget-header">
                                                <span class="icon-bookmark"></span>
                                                <h3>Descuentos</h3>
                                            </div>
                                            <div class="widget-content">
                                                <div role="form" class="form-horizontal col-md-12">
                                                    <br />
                                                    <fieldset>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-2">Desde:</label>
                                                            <div class="col-md-2">
                                                                <asp:TextBox ID="txtCantDesde" runat="server" class="form-control" placeholder="Cantidad" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator37" runat="server" ErrorMessage="*" ControlToValidate="txtCantDesde" ValidationGroup="DescuentosArt" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-2">Hasta:</label>
                                                            <div class="col-md-2">
                                                                <asp:TextBox ID="txtCantHasta" runat="server" class="form-control" placeholder="Cantidad" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator38" runat="server" ErrorMessage="*" ControlToValidate="txtCantHasta" ValidationGroup="DescuentosArt" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-2">Descuento:</label>
                                                            <div class="col-md-2">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">%</span>
                                                                    <asp:TextBox ID="txtDescuentoCantidad" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-1">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server" ErrorMessage="*" ControlToValidate="txtDescuentoCantidad" ValidationGroup="DescuentosArt" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:LinkButton ID="lbtnAgregarDto" runat="server" class="btn btn-success" ValidationGroup="DescuentosArt" OnClick="lbtnAgregarDto_Click">
                                                                    <i class="shortcut-icon icon-ok"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>

                                                    </fieldset>
                                                </div>
                                            </div>

                                            <div class="widget-content">
                                                <div class="col-md-12">
                                                    <div class="widget stacked widget-table">

                                                        <div class="widget-header">
                                                            <span class="icon-bookmark"></span>
                                                            <h3>Descuentos</h3>
                                                        </div>
                                                        <!-- .widget-header -->

                                                        <div class="widget-content">
                                                            <table class="table table-bordered table-striped">

                                                                <thead>
                                                                    <tr>
                                                                        <th>Codigo Articulo</th>
                                                                        <th>Desde</th>
                                                                        <th>Hasta</th>
                                                                        <th>Descuento</th>
                                                                        <th></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:PlaceHolder ID="phDescuentos" runat="server"></asp:PlaceHolder>
                                                                </tbody>

                                                            </table>

                                                        </div>
                                                        <!-- .widget-content -->

                                                    </div>
                                                </div>
                                            </div>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <%--Fin Descuentos--%>
                                <%--Store--%>
                                <div class="tab-pane fade" id="Store">
                                    <asp:UpdatePanel ID="UpdatePanelStore" UpdateMode="Always" runat="server">
                                        <ContentTemplate>

                                            <div class="widget-content">
                                                <div role="form" class="form-horizontal col-md-10">

                                                    <br />
                                                    <fieldset>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Aparece en Store</label>

                                                            <div class="col-md-3">
                                                                <asp:DropDownList ID="ListApareceStore" class="form-control" runat="server" OnSelectedIndexChanged="ListApareceStore_SelectedIndexChanged" AutoPostBack="True">
                                                                    <asp:ListItem>NO</asp:ListItem>
                                                                    <asp:ListItem>SI</asp:ListItem>
                                                                </asp:DropDownList>

                                                            </div>
                                                        </div>


                                                        <asp:UpdatePanel ID="UpdatePanelStoresArticulos" UpdateMode="Always" runat="server" Visible="false">
                                                            <ContentTemplate>
                                                                <div class="form-group">
                                                                    <label for="name" class="col-md-4">Store</label>

                                                                    <div class="col-md-3">
                                                                        <asp:DropDownList ID="ListStores" class="form-control" runat="server">
                                                                        </asp:DropDownList>

                                                                    </div>
                                                                    <div class="col-md-3">
                                                                        <asp:LinkButton ID="btnAgregarStores" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregarStores_Click" />
                                                                    </div>
                                                                </div>

                                                                <div class="form-group">
                                                                    <label for="name" class="col-md-4"></label>

                                                                    <div class="col-md-3">
                                                                        <asp:ListBox ID="ListBoxStores" runat="server" class="form-control"></asp:ListBox>
                                                                    </div>
                                                                    <div class="col-md-3">
                                                                        <asp:LinkButton ID="btnQuitarStore" runat="server" Text="<span class='shortcut-icon icon-trash'></span>" class="btn btn-danger" OnClick="btnQuitarStore_Click" />
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Oferta</label>

                                                            <div class="col-md-3">
                                                                <asp:DropDownList ID="ListOferta" class="form-control" runat="server">
                                                                    <asp:ListItem Text="NO" Value="0" />
                                                                    <asp:ListItem Text="SOLO STORE" Value="1" />
                                                                    <asp:ListItem Text="SOLO GESTION" Value="3" />
                                                                    <asp:ListItem Text="AMBOS" Value="2" />
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Precio Oferta</label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txtPrecioOferta" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Desde</label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                                    <asp:TextBox ID="txtDesde" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                    <asp:TextBox ID="txtDesdeHora" TextMode="Time" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Hasta</label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                                    <asp:TextBox ID="txtHasta" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                    <asp:TextBox ID="txtHastaHora" TextMode="Time" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>


                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Destacado</label>

                                                            <div class="col-md-3">
                                                                <asp:DropDownList ID="ListDestacado" class="form-control" runat="server">
                                                                    <asp:ListItem>NO</asp:ListItem>
                                                                    <asp:ListItem>SI</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>


                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Novedades</label>

                                                            <div class="col-md-3">
                                                                <asp:DropDownList ID="ListNovedades" class="form-control" runat="server">
                                                                    <asp:ListItem>NO</asp:ListItem>
                                                                    <asp:ListItem>SI</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Especificaciones</label>
                                                            <div class="col-md-6">
                                                                <asp:TextBox ID="txtEspecificacionStore" runat="server" class="form-control" TextMode="MultiLine" Rows="4" />
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <asp:Button ID="btnAgregarStore" ValidationGroup="store" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarStore_Click" />
                                                        </div>

                                                    </fieldset>
                                                </div>
                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <%--Fin Store--%>
                                <div class="tab-pane fade" id="Combustible">
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <div class="widget-content">
                                                <div class="col-md-12" style="padding-right: 0px; padding-left: 0px;">
                                                    <div class="widget stacked widget-table">
                                                        <table class="table table-bordered table-striped">

                                                            <thead>
                                                                <tr>
                                                                    <th style="width: 25%">Proveedor</th>
                                                                    <th style="width: 10%; text-align: right;">ICL</th>
                                                                    <th style="width: 10%; text-align: right;">IDC</th>
                                                                    <th style="width: 10%; text-align: right;">Tasa Vial</th>
                                                                    <th style="width: 10%; text-align: right;">Tasa Municipal</th>
                                                                    <th style="width: 15%;">Fecha alta</th>
                                                                    <th style="width: 5%"></th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:PlaceHolder ID="phDatosCombustible" runat="server"></asp:PlaceHolder>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="widget-content">
                                                <div role="form" class="form-horizontal col-md-10">
                                                    <fieldset>
                                                        <div class="form-group">
                                                            <label class="col-md-4">Proveedor</label>
                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="ListProveedorCombustible" runat="server" class="form-control" />
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator39" runat="server" ErrorMessage="Seleccione un proveedor" ControlToValidate="ListProveedorCombustible" InitialValue="-1" ValidationGroup="CombustibleGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-md-4">ICL ($ x Lt.)</label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txtCombustibleITC" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Style="text-align: right;" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ErrorMessage="*" ControlToValidate="txtCombustibleITC" ValidationGroup="CombustibleGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-md-4">IDC ($ x Lt.)</label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txtCombustibleTasaHidrica" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Style="text-align: right;" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ErrorMessage="*" ControlToValidate="txtCombustibleTasaHidrica" ValidationGroup="CombustibleGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-md-4">Tasa Vial ($ x Lt.)</label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txtCombustibleTasaVial" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Style="text-align: right;" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator42" runat="server" ErrorMessage="*" ControlToValidate="txtCombustibleTasaVial" ValidationGroup="CombustibleGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-md-4">Tasa Municipal ($ x Lt.)</label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txtCombustibleTasaMunicipal" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Style="text-align: right;" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator43" runat="server" ErrorMessage="*" ControlToValidate="txtCombustibleTasaMunicipal" ValidationGroup="CombustibleGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-md-4">
                                                                <asp:Button ID="btnAgregarDatosCombustible" runat="server" Text="Agregar" class="btn btn-success" ValidationGroup="CombustibleGroup" OnClick="btnAgregarDatosCombustible_Click" />
                                                            </div>
                                                        </div>
                                                    </fieldset>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <%--Fin Combustible--%>
                                <%--Costos--%>
                                <div class="tab-pane fade" id="Costos">
                                    <asp:UpdatePanel ID="UpdatePanel7" UpdateMode="Always" runat="server">
                                        <ContentTemplate>

                                            <div class="widget-header">
                                                <span class="icon-calendar"></span>
                                                <h3>Historico</h3>
                                            </div>
                                            <div class="widget-content">
                                                <div class="col-md-12">
                                                    <div class="widget stacked widget-table">

                                                        <div class="widget-header">
                                                            <span class="icon-usd"></span>
                                                            <h3>Costos</h3>
                                                        </div>
                                                        <!-- .widget-header -->

                                                        <div class="widget-content">
                                                            <table class="table table-bordered table-striped">

                                                                <thead>
                                                                    <tr>
                                                                        <th>Proveedor</th>
                                                                        <th>Fecha Act.</th>
                                                                        <th style="text-align: right;">Costo</th>
                                                                        <th>Moneda</th>
                                                                        <th style="text-align: right;">%</th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:PlaceHolder ID="phCostos" runat="server"></asp:PlaceHolder>
                                                                </tbody>

                                                            </table>

                                                        </div>
                                                        <!-- .widget-content -->

                                                    </div>
                                                </div>
                                            </div>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <%--Fin Descuentos--%>
                                <div class="tab-pane fade" id="Extras">
                                    <asp:UpdatePanel ID="UpdatePanelExtras" runat="server">
                                        <ContentTemplate>
                                            <div class="widget-content">
                                                <div role="form" class="form-horizontal col-md-10">
                                                    <fieldset>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">¿Pedir datos extra?</label>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList ID="ListSiDatosExtra" class="form-control" runat="server">
                                                                    <asp:ListItem Text="SELECCIONE..." Value="-1" />
                                                                    <asp:ListItem Text="NO" Value="0" />
                                                                    <asp:ListItem Text="SI" Value="1" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-1">
                                                                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="ListSiDatosExtra" InitialValue="-1" runat="server" ForeColor="Red" Font-Bold="true" ValidationGroup="ExtrasGroup" />
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:LinkButton ID="btnAgregarSiDatosExtra" Text="<i class='icon icon-ok'></i>" runat="server" OnClick="btnAgregarSiDatosExtra_Click" class="btn btn-success" ValidationGroup="ExtrasGroup" />
                                                            </div>
                                                        </div>
                                                    </fieldset>
                                                </div>
                                            </div>
                                            <br />
                                            <asp:Panel ID="PanelDatosExtra" runat="server" Visible="false">
                                                <div class="widget-header">
                                                    <span class="icon icon-wrench"></span>
                                                    <h3>Datos Extras</h3>
                                                </div>
                                                <!-- .widget-header -->
                                                <div class=" widget-content" style="width: 100%">
                                                    <div class="btn-group pull-right" style="height: 100%">
                                                        <a class="btn btn-primary ui-tooltip" data-toggle="modal" title data-original-title="Filtrar" href="#modalBusqueda" style="width: 100%">
                                                            <i class="shortcut-icon icon-filter"></i>
                                                        </a>
                                                    </div>
                                                </div>

                                                <table class="table table-striped table-bordered">
                                                    <thead>
                                                        <tr>
                                                            <th>Fecha</th>
                                                            <th>Documento</th>
                                                            <th>Datos</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:PlaceHolder ID="phDatosExtras" runat="server"></asp:PlaceHolder>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <%--Medidas--%>
                                <div class="tab-pane fade" id="Medidas">
                                    <asp:UpdatePanel ID="UpdatePanel9" UpdateMode="Always" runat="server">
                                        <ContentTemplate>

                                            <div class="widget-header">
                                                <span class="icon-bookmark"></span>
                                                <h3>Venta x cantidad</h3>
                                            </div>
                                            <div class="widget-content">
                                                <div role="form" class="form-horizontal col-md-12">
                                                    <br />
                                                    <fieldset>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-2">Nombre:</label>
                                                            <div class="col-md-2">
                                                                <asp:TextBox ID="txtMedidaNombre" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator45" runat="server" ErrorMessage="*" ControlToValidate="txtMedidaNombre" ValidationGroup="MedidaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-2">Codigo Barra:</label>
                                                            <div class="col-md-2">
                                                                <asp:TextBox ID="txtCodigoBarraMedida" onkeypress="javascript:return validarNro(event)" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator51" runat="server" ErrorMessage="*" ControlToValidate="txtCodigoBarraMedida" ValidationGroup="MedidaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-2">Cantidad:</label>
                                                            <div class="col-md-2">
                                                                <asp:TextBox ID="txtMedidaCantidad" runat="server" class="form-control" placeholder="Cantidad" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-1">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator46" runat="server" ErrorMessage="*" ControlToValidate="txtMedidaCantidad" ValidationGroup="MedidaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                <span class="ui-popover" data-container="body" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Es la cantidad por la cual se multiplicara el articulo al momento de carga en la factura." title="" data-original-title="Ayuda">
                                                                    <i class="shortcut-icon fa fa-question-circle"></i>
                                                                </span>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:LinkButton ID="lbtnAgregarMedida" runat="server" class="btn btn-success" ValidationGroup="MedidaGroup" OnClick="lbtnAgregarMedida_Click">
                                                                    <i class="shortcut-icon icon-ok"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>

                                                    </fieldset>
                                                </div>
                                            </div>

                                            <div class="widget-content">
                                                <div class="col-md-12">
                                                    <div class="widget stacked widget-table">
                                                        <div class="widget-content">
                                                            <table class="table table-bordered table-striped">
                                                                <thead>
                                                                    <tr>
                                                                        <th>Codigo Articulo</th>
                                                                        <th>Nombre</th>
                                                                        <th>Codigo Barra</th>
                                                                        <th>Cantidad</th>
                                                                        <th></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:PlaceHolder ID="phMedidas" runat="server"></asp:PlaceHolder>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                        <!-- .widget-content -->
                                                    </div>
                                                </div>
                                            </div>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <%--Fin Medidas--%>
                                <%--Beneficios--%>
                                <div class="tab-pane fade" id="Beneficios">
                                    <asp:UpdatePanel ID="UpdatePanelBeneficios" UpdateMode="Always" runat="server">
                                        <ContentTemplate>

                                            <div class="widget-header">
                                                <span class="icon-bookmark"></span>
                                                <h3>Sistema Beneficios</h3>
                                            </div>
                                            <div class="widget-content">
                                                <div role="form" class="form-horizontal col-md-12">
                                                    <br />
                                                    <fieldset>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-2">¿Beneficios?:</label>
                                                            <div class="col-md-2">
                                                                <asp:DropDownList ID="ListSiBeneficios" runat="server" class="form-control">
                                                                    <asp:ListItem Text="Seleccione..." Value="-1" />
                                                                    <asp:ListItem Text="NO" Value="0" />
                                                                    <asp:ListItem Text="SI" Value="1" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator47" runat="server" ErrorMessage="*" InitialValue="-1" ControlToValidate="ListSiBeneficios" ValidationGroup="BeneficiosGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-2">¿Destacado?:</label>
                                                            <div class="col-md-2">
                                                                <asp:DropDownList ID="ListSiDestacadoBeneficios" runat="server" class="form-control">
                                                                    <asp:ListItem Text="Seleccione..." Value="-1" />
                                                                    <asp:ListItem Text="NO" Value="0" />
                                                                    <asp:ListItem Text="SI" Value="1" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator49" runat="server" ErrorMessage="*" InitialValue="-1" ControlToValidate="ListSiDestacadoBeneficios" ValidationGroup="BeneficiosGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-2">¿Oferta?:</label>
                                                            <div class="col-md-2">
                                                                <asp:DropDownList ID="ListSiOfertaBeneficios" runat="server" class="form-control">
                                                                    <asp:ListItem Text="Seleccione..." Value="-1" />
                                                                    <asp:ListItem Text="NO" Value="0" />
                                                                    <asp:ListItem Text="SI" Value="1" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator48" runat="server" ErrorMessage="*" InitialValue="-1" ControlToValidate="ListSiOfertaBeneficios" ValidationGroup="BeneficiosGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-2">Desde</label>
                                                            <div class="col-md-2">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                                    <asp:TextBox ID="txtBeneficiosDesde" runat="server" class="form-control" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-2">Hasta</label>
                                                            <div class="col-md-2">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                                    <asp:TextBox ID="txtBeneficiosHasta" runat="server" class="form-control" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-2">Puntos:</label>
                                                            <div class="col-md-2">
                                                                <asp:TextBox ID="txtBeneficiosPuntos" runat="server" class="form-control" Text="0" onkeypress="javascript:return validarNro(event)" />
                                                            </div>
                                                            <div class="col-md-1">
                                                                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtBeneficiosPuntos" runat="server" Font-Bold="true" ForeColor="Red" ValidationGroup="BeneficiosGroup" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-2">Especificaciones:</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtBeneficiosEspecificaciones" runat="server" class="form-control" TextMode="MultiLine" Rows="4" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-md-2">
                                                                <asp:LinkButton ID="lbtnAgregarBeneficios" runat="server" class="btn btn-success" Text="Guardar" ValidationGroup="BeneficiosGroup" OnClick="lbtnAgregarBeneficios_Click"></asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </fieldset>
                                                </div>
                                            </div>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <%--Fin Beneficios--%>

                                <%-- Articulos sucursales --%>
                                <div class="tab-pane fade" id="ArticulosSucursales">
                                    <asp:UpdatePanel ID="UpdatePanelArticulos_Sucursales" UpdateMode="Always" runat="server">
                                        <ContentTemplate>

                                            <div class="col-md-12 col-xs-12">
                                                <div class="widget stacked">

                                                    <div class="widget-header">
                                                        <i class="icon-wrench"></i>
                                                        <h3>Articulos Sucursales</h3>
                                                    </div>
                                                    <!-- /widget-header -->

                                                    <div class="widget-content">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 90%">
                                                                    <label for="name" class="col-md-3">Sucursal</label>
                                                                    <div class="col-md-5">
                                                                        <asp:DropDownList ID="ListSucursales" class="form-control" runat="server"></asp:DropDownList>
                                                                    </div>

                                                                    <asp:LinkButton ID="btnAgregarSucursal" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregarSucursal_Click" />

                                                                </td>

                                                            </tr>
                                                            <tr>
                                                                <td style="width: 90%">
                                                                    <label class="col-md-3"></label>
                                                                    <div class="col-md-5">
                                                                        <asp:ListBox ID="ListBoxSucursales" runat="server" class="form-control"></asp:ListBox>
                                                                    </div>

                                                                    <asp:LinkButton ID="btnQuitarSucursal" runat="server" Text="<span class='shortcut-icon icon-trash'></span>" class="btn btn-danger" OnClick="btnQuitarSucursal_Click" />

                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <!-- /widget-content -->
                                                </div>
                                                <!-- /widget -->
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <%-- Fin Articulos sucursales --%>

                                <%-- Stock minimo sucursales --%>
                                <div class="tab-pane fade" id="StockMinimoSucursales">
                                    <asp:UpdatePanel ID="UpdatePanelStockMinimoSucursales" UpdateMode="Always" runat="server">
                                        <ContentTemplate>

                                            <div class="widget-content">
                                                <div role="form" class="form-horizontal col-md-10">

                                                    <br />
                                                    <fieldset>
                                                        <div class="form-group">
                                                            <label for="validateSelect" class="col-md-4">Sucursales</label>
                                                            <div class="col-md-5">
                                                                <asp:DropDownList ID="ListSucursalesStockMinimo" class="form-control" runat="server"></asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Stock Minimo</label>

                                                            <div class="col-md-3">
                                                                <asp:TextBox ID="txtStockMinimoSucursal" runat="server" class="form-control" TextMode="Number"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-1">
                                                                <asp:LinkButton ID="lbtnAgregarStockMinimoSuc" runat="server" Text="Buscar" class="btn btn-success" OnClick="lbtnAgregarStockMinimoSuc_Click" ValidationGroup="StockMinimo">
                                                                <i class="shortcut-icon icon-ok"></i></asp:LinkButton>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator56" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtStockMinimoSucursal" ValidationGroup="StockMinimo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>

                                                    </fieldset>
                                                </div>
                                            </div>

                                            <div class="widget-content">
                                                <div class="col-md-12">
                                                    <div class="widget stacked widget-table">

                                                        <div class="widget-header">
                                                            <span class="icon-bookmark"></span>
                                                            <h3>Stock minimo por sucursales</h3>
                                                        </div>
                                                        <!-- .widget-header -->

                                                        <div class="widget-content">
                                                            <table class="table table-bordered table-striped">

                                                                <thead>
                                                                    <tr>
                                                                        <th>Codigo Articulo</th>
                                                                        <th>Sucursal</th>
                                                                        <th>Stock Minimo</th>
                                                                        <th></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:PlaceHolder ID="phStockMinimoSucursal" runat="server"></asp:PlaceHolder>
                                                                </tbody>

                                                            </table>

                                                        </div>
                                                        <!-- .widget-content -->

                                                    </div>
                                                </div>
                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <%-- Fin Stock minimo sucursales --%>

                                <%-- Catalogo --%>
                                <div class="tab-pane fade" id="Catalogo">
                                    <asp:UpdatePanel ID="UpdatePanelCatalogo" UpdateMode="Always" runat="server">
                                        <ContentTemplate>

                                            <div class="col-md-12 col-xs-12">
                                                <div class="widget stacked">

                                                    <div class="widget-header">
                                                        <i class="icon-wrench"></i>
                                                        <h3>Herramientas</h3>
                                                    </div>
                                                    <!-- /widget-header -->

                                                    <div class="widget-content">
                                                        <table style="width: 100%">

                                                            <tr>
                                                                <td style="width: 90%">
                                                                    <label class="col-md-3">Catalogo</label>
                                                                    <div class="col-md-7">
                                                                        <asp:TextBox ID="txtCatalogo" runat="server" class="form-control"></asp:TextBox>
                                                                    </div>
                                                                    <div class="col-md-2">
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator52" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtCatalogo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="CatalogoGroup"></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </td>
                                                                <td style="width: 5%">
                                                                    <div class="shortcuts" style="height: 100%">
                                                                        <asp:LinkButton ID="lbtnAgregarCatalogo" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnAgregarCatalogo_Click" Style="width: 100%" ValidationGroup="CatalogoGroup" />
                                                                    </div>
                                                                </td>
                                                                <td style="width: 5%">
                                                                    <div class="shortcuts" style="height: 100%">
                                                                        <asp:LinkButton ID="lbtnEliminarCatalogo" runat="server" Text="<span class='shortcut-icon icon-trash'></span>" class="btn btn-danger" OnClick="lbtnEliminarCatalogo_Click" Style="width: 100%" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 90%">
                                                                    <label class="col-md-3">Codigo COT</label>
                                                                    <div class="col-md-7">
                                                                        <asp:TextBox ID="txtCodCot" runat="server" class="form-control"></asp:TextBox>
                                                                    </div>
                                                                    <div class="col-md-2">
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator53" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtCodCot" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="CodCotGroup"></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </td>
                                                                <td style="width: 5%">
                                                                    <div class="shortcuts" style="height: 100%">
                                                                        <asp:LinkButton ID="btnAgregarCodCot" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregarCodCot_Click" Style="width: 100%" ValidationGroup="CodCotGroup" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 90%">
                                                                    <label class="col-md-3">Aparece en Lista</label>
                                                                    <div class="col-md-4">
                                                                        <asp:CheckBox ID="chkApareceLista" runat="server" Checked="true" />
                                                                    </div>
                                                                </td>
                                                                <td style="width: 5%">
                                                                    <div class="shortcuts" style="height: 100%">
                                                                        <asp:LinkButton ID="lbtnApareceLista" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnApareceLista_Click" Style="width: 100%" ValidationGroup="ApareceListaGroup" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <!-- /widget-content -->
                                                </div>
                                                <!-- /widget -->
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <%-- Fin Catalogo --%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--roww--%>


    <!-- sample modal content -->
    <div id="modalGrupo" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>


                    <div class="modal-header">
                    </div>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Agregar Grupo</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="validateSelect" class="col-md-4">Grupo</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtGrupo" runat="server" class="form-control"></asp:TextBox>
                        </div>

                    </div>

                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnAgregarGrupo" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregarGrupo_Click" />
                    <%--                        <asp:Button ID="btnAgregarGrupo" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarGrupo_Click" />--%>
                </div>
            </div>
        </div>
    </div>
    <%--Fin modalGrupo--%>

    <!-- sample modal content -->
    <div id="modalPais" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Agregar Pais</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="validateSelect" class="col-md-4">Pais</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtPais" runat="server" class="form-control"></asp:TextBox>
                        </div>

                    </div>

                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnAgregarPais" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnAgregarPais_Click" />
                    <%--                        <asp:Button ID="btnAgregarGrupo" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarGrupo_Click" />--%>
                </div>

            </div>
        </div>
    </div>
    <%--Fin modalGrupo--%>

    <div id="modalSubGrupo" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Agregar SubGrupo</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="validateSelect" class="col-md-4">Grupo</label>
                        <div class="col-md-4">
                            <asp:DropDownList ID="DropListGrupo2" runat="server" class="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="TxtSubGrupo" runat="server" class="form-control"></asp:TextBox>
                        </div>

                    </div>


                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnAgregarSubGrupo" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregarSubgrupo_Click" />

                    <%--                        <asp:Button ID="btnAgregarSubgrupo" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarSubgrupo_Click" />--%>
                </div>
            </div>
        </div>
    </div>
    <%--Fin modalSubGrupo--%>

    <div id="modalMarca" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Agregar Marca</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="validateSelect" class="col-md-4">Marca</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtMarcaNueva" runat="server" class="form-control"></asp:TextBox>
                        </div>

                    </div>

                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnAgregarMarca" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnAgregarMarca_Click" />
                    <%--                        <asp:Button ID="btnAgregarGrupo" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarGrupo_Click" />--%>
                </div>

            </div>
        </div>
    </div>
    <%--Fin modalMarca--%>

    <div id="modalImagen" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">

            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Agregar Imagen</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="validateSelect" class="col-md-4">Imagen</label>
                        <div class="col-md-8">
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnAgregarImagen" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnAgregarImagen_Click" />

                    <%--                        <asp:Button ID="btnAgregarSubgrupo" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarSubgrupo_Click" />--%>
                </div>
            </div>

        </div>

    </div>
    <%--Fin modalSubGrupo--%>

    <div id="modalBusqueda" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:UpdatePanel ID="UpdatePanel8" UpdateMode="Always" runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <button id="btnCerrarBusqueda" type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Filtrar </h4>
                        </div>
                        <div class="modal-body">
                            <div role="form" class="form-horizontal col-md-12">

                                <div class="form-group">
                                    <label class="col-md-4">Fecha Desde</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtFechaDesdeExtra" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Fecha Hasta</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtFechaHastaExtra" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Sucursal</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListSucursalesExtra" runat="server" class="form-control"></asp:DropDownList>
                                    </div>

                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator44" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListSucursalesExtra" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                            </div>
                            <div class="modal-footer">
                                <%--<asp:UpdatePanel ID="UpdatePanelBusqueda" runat="server">
                            <ContentTemplate>--%>
                                <asp:LinkButton ID="btnFiltrar" ValidationGroup="BusquedaGroup" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnFiltrar_Click" />
                                <%--</ContentTemplate>
                        </asp:UpdatePanel>       --%>
                            </div>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>

        </div>
    </div>
    <%--    </div>--%>
    <%-- Fin modalBusqueda --%>


    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
    <%--<script src="../../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>--%>

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <script src="../../Scripts/libs/bootstrap.min.js"></script>

    <script src="../../Scripts/plugins/hoverIntent/jquery.hoverIntent.minified.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>

    <script src="../../Scripts/Application.js"></script>

    <script src="../../Scripts/demo/gallery.js"></script>

    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
    <script src="../../Scripts/demo/notifications.js"></script>

    <script>
        function cerrarModalBusqueda() {
            document.getElementById('btnCerrarBusqueda').click();
        }
        function clickTab() {
            document.getElementById("<%= this.linkExtras.ClientID %>").click();
        }

    </script>
    <script>

        $(function () {
            $("#<%= txtDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaAct.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaDespacho.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaDesdeExtra.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaHastaExtra.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtBeneficiosDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtBeneficiosHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

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

</asp:Content>
