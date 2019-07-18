<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComprasABM.aspx.cs" Inherits="Gestion_Web.Formularios.Compras.ComprasABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <div class="col-md-12 col-xs-12">
            <div class="widget stacked">
                <div class="stat">
                    <h5><i class="icon-map-marker"></i>Compras > Compra</h5>
                </div>
                <div class="widget-header">
                    <i class="icon-pencil"></i>
                    <h3>Compras</h3>
                </div>
                <!-- /.widget-header -->

                <div class="widget-content">
                    <div class="bs-example">
                        <ul id="myTab" class="nav nav-tabs">
                            <li class="active"><a href="#home" data-toggle="tab">Detalle</a></li>
                        </ul>
                        <div class="tab-content">
                            <div class="tab-pane fade active in" id="home">
                                <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                                    <ContentTemplate>
                                        <div id="validation-form" role="form" class="form-horizontal col-md-7">
                                            <fieldset>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Empresa</label>

                                                    <div class="col-md-6">
                                                        <asp:DropDownList ID="ListEmpresa" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ListEmpresa_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:RequiredFieldValidator ControlToValidate="ListEmpresa" ID="RequiredFieldValidator18" runat="server" ErrorMessage="Seleccione Empresa" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Sucursal</label>

                                                    <div class="col-md-6">
                                                        <asp:DropDownList ID="ListSucursal" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ListSucursal_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:RequiredFieldValidator ControlToValidate="ListSucursal" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Seleccione Sucursal" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Punto de Venta</label>

                                                    <div class="col-md-6">
                                                        <asp:DropDownList ID="ListPuntoVenta" runat="server" class="form-control"></asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:RequiredFieldValidator ControlToValidate="ListPuntoVenta" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Seleccione Punto Venta" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Fecha</label>

                                                    <div class="col-md-6">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                            <asp:TextBox ID="txtFecha" runat="server" class="form-control" AutoPostBack="true" OnTextChanged="txtFecha_TextChanged"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ControlToValidate="txtFecha" ID="RequiredFieldValidator29" runat="server" ErrorMessage="Seleccione Fecha" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Tipo Compra</label>

                                                    <div class="col-md-6">
                                                        <asp:DropDownList ID="ListTipoCompra" runat="server" class="form-control">
                                                            <asp:ListItem Text="Seleccione..." Value="-1"></asp:ListItem>
                                                            <asp:ListItem Text="Gasto" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Compra mercaderia" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Bienes de uso" Value="3"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:RequiredFieldValidator ControlToValidate="ListTipoCompra" ID="RequiredFieldValidator8" runat="server" ErrorMessage="Seleccion tipo compra" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Tipo Documento</label>

                                                    <div class="col-md-6">
                                                        <asp:DropDownList ID="ListTipoDocumento" runat="server" class="form-control">
                                                            <asp:ListItem>Factura A</asp:ListItem>
                                                            <asp:ListItem>Factura B</asp:ListItem>
                                                            <asp:ListItem>Factura C</asp:ListItem>
                                                            <asp:ListItem>Factura I</asp:ListItem>
                                                            <asp:ListItem>Factura E</asp:ListItem>
                                                            <asp:ListItem>Factura M</asp:ListItem>
                                                            <asp:ListItem>Nota de Crédito A</asp:ListItem>
                                                            <asp:ListItem>Nota de Crédito B</asp:ListItem>
                                                            <asp:ListItem>Nota de Crédito C</asp:ListItem>
                                                            <asp:ListItem>Nota de Crédito E</asp:ListItem>
                                                            <asp:ListItem>Nota de Débito A</asp:ListItem>
                                                            <asp:ListItem>Nota de Débito B</asp:ListItem>
                                                            <asp:ListItem>Nota de Débito C</asp:ListItem>
                                                            <asp:ListItem>Nota de Débito E</asp:ListItem>
                                                            <asp:ListItem>Presupuesto</asp:ListItem>
                                                            <asp:ListItem>Nota de Crédito - PRP</asp:ListItem>
                                                            <asp:ListItem>Nota de Débito - PRP</asp:ListItem>
                                                            <asp:ListItem>Rendicion de ventas</asp:ListItem>
                                                            <asp:ListItem>Tiquet Factura A</asp:ListItem>
                                                            <asp:ListItem>Tiquet Factura B</asp:ListItem>
                                                            <asp:ListItem>Comprobante no fiscal</asp:ListItem>
                                                            <asp:ListItem>Liquidacion tarjeta</asp:ListItem>
                                                            <asp:ListItem>Comprobantes de retencion</asp:ListItem>
                                                            <asp:ListItem>Recibo A</asp:ListItem>
                                                            <asp:ListItem>Recibo B</asp:ListItem>
                                                            <asp:ListItem>Recibo C</asp:ListItem>
                                                            <asp:ListItem>Otros comprobantes</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:RequiredFieldValidator ControlToValidate="ListTipoDocumento" ID="RequiredFieldValidator5" runat="server" ErrorMessage="Seleccione Documento" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Numero</label>

                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="txtPVenta" runat="server" MaxLength="4" onkeypress="javascript:return validarNro(event)" onchange="completar4Ceros(this, this.value)" class="form-control"></asp:TextBox>
                                                    </div>

                                                    <div class="col-md-4">

                                                        <asp:TextBox ID="txtNumero" MaxLength="8" runat="server" onkeypress="javascript:return validarNro(event)" onchange="completar8Ceros(this, this.value)" class="form-control"></asp:TextBox>

                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:RequiredFieldValidator ControlToValidate="txtPVenta" ID="RequiredFieldValidator30" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:RequiredFieldValidator ControlToValidate="txtNumero" ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Proveedor</label>

                                                    <div class="col-md-6">
                                                        <asp:DropDownList ID="ListProveedor" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ListProveedor_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:RequiredFieldValidator ControlToValidate="ListProveedor" ID="RequiredFieldValidator32" runat="server" ErrorMessage="Seleccione Sucursal" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>
                                                </div>
                                                <asp:Panel ID="PanelCtaCtble" runat="server" Visible="false">
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Plan de cuentas:</label>
                                                        <div class="col-md-6">
                                                            <asp:TextBox ID="txtPlanCtaProv" runat="server" class="form-control" ReadOnly="true" Width="100%" />
                                                        </div>
                                                        <div class="col-md-1">
                                                            <a href="#modalCuentas" data-toggle="modal" class="btn btn-info"><span class='shortcut-icon icon-plus'></span></a>
                                                        </div>
                                                    </div>
                                                </asp:Panel>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Cuit</label>

                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="txtCuit" runat="server" class="form-control" Text="0" disabled ViewStateMode="Enabled"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCuit" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">IVA</label>

                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="txtIva" runat="server" class="form-control" disabled ViewStateMode="Enabled"></asp:TextBox>

                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:RequiredFieldValidator ControlToValidate="txtIva" ID="RequiredFieldValidator33" runat="server" ErrorMessage="Seleccione Sucursal" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">

                                                    <label for="name" class="col-md-3">Neto No Grabado</label>
                                                    <div class="col-md-6">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtNetoNoGrabado" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txtNetoNoGrabado_TextChanged"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator34" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNetoNoGrabado" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtNetoNoGrabado" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                    </div>

                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Neto 2.5</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtNeto2" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txtNeto2_TextChanged"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtIvaNeto2" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNeto2" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ControlToValidate="txtIvaNeto2" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                    </div>

                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Neto 5</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtNeto5" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txtNeto5_TextChanged"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtIvaNeto5" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNeto5" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txtIvaNeto5" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Neto 10.5</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtNeto105" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txtNeto105_TextChanged"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtIvaNeto105" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator35" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNeto105" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ControlToValidate="txtNeto105" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                    </div>

                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Neto 21</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtNeto21" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txtNeto21_TextChanged"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtIvaNeto21" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNeto21" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ControlToValidate="txtNeto21" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                    </div>

                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Neto 27</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtNeto27" Text="0" runat="server" Style="text-align: right;" class="form-control" AutoPostBack="true" OnTextChanged="txtIvaNeto27_TextChanged" onkeypress="javascript:return validarNro(event)"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtIvaNeto27" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator37" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNeto27" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ControlToValidate="txtNeto27" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Ingresos Brutos</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtPIB" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" OnTextChanged="txtPIB_TextChanged" AutoPostBack="true"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator38" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtPIB" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server" ControlToValidate="txtPIB" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Percepción IVA</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtPIva" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txtPIva_TextChanged"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator39" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtPIva" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server" ControlToValidate="txtPIva" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Impuestos Internos</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtImpuestosInternos" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="true" OnTextChanged="txtImpuestosInternos_TextChanged"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtImpuestosInternos" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator16" runat="server" ControlToValidate="txtImpuestosInternos" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Retencion de IIBB</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtRetencionIIBB" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="true" OnTextChanged="txtRetencionIIBB_TextChanged"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtRetencionIIBB" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtRetencionIIBB" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Retencion de IVA</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtRetencionIVA" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="true" OnTextChanged="txtRetencionIVA_TextChanged"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtRetencionIVA" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtRetencionIVA" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Retencion de ganancias</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtRetencionGanancias" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="true" OnTextChanged="txtRetencionGanancias_TextChanged"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtRetencionGanancias" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtRetencionGanancias" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Retencion SUSS</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtRetencionSuss" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="true" OnTextChanged="txtRetencionSuss_TextChanged"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtRetencionSuss" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator18" runat="server" ControlToValidate="txtRetencionSuss" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">ITC</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtITC" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="true" OnTextChanged="txtITC_TextChanged"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtITC" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtITC" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Tasa CO2</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtTasaCo2" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="true" OnTextChanged="txtTasaCo2_TextChanged"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtTasaCo2" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtTasaCo2" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                    </div>
                                                </div>
                                                <%--<div class="form-group">
                                                    <label for="name" class="col-md-3">Tasa Gasoil</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtTasaGasoil" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="true" OnTextChanged="txtTasaGasoil_TextChanged"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtTasaGasoil" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtTasaGasoil" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                    </div>
                                                </div>--%>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Otros</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtOtros" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro2(event)" AutoPostBack="True" OnTextChanged="txtOtros_TextChanged"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtOtros" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator17" runat="server" ControlToValidate="txtOtros" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Vencimiento</label>

                                                    <div class="col-md-6">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                            <asp:TextBox ID="txtVencimiento" runat="server" class="form-control"></asp:TextBox>
                                                        </div>

                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ControlToValidate="txtVencimiento" ID="RequiredFieldValidator42" runat="server" ErrorMessage="Seleccione Fecha" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Fecha imputacion contable</label>

                                                    <div class="col-md-6">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                            <asp:TextBox ID="txtImputacionCont" runat="server" class="form-control"></asp:TextBox>
                                                        </div>

                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ControlToValidate="txtImputacionCont" ID="RequiredFieldValidator7" runat="server" ErrorMessage="Seleccione Fecha" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Total</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtTotal" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtOtros" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtOtros" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Observaciones</label>
                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="txtObservaciones" runat="server" class="form-control" TextMode="MultiLine" Rows="6"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">No imputa en Cuenta Corriente</label>

                                                    <div class="col-md-8">
                                                        <asp:CheckBox ID="checkImputaCC" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-8">
                                                    <div class="btn-toolbar" role="toolbar">
                                                        <div class="btn-group">
                                                            <asp:Button type="button" ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" ValidationGroup="ArticuloGroup" OnClick="btnAgregar_Click" />
                                                        </div>
                                                        <div class="btn-group">
                                                            <asp:Button type="button" ID="btnAgregarPagar" runat="server" Text="Guardar y Pagar" class="btn btn-success" ValidationGroup="ArticuloGroup" OnClick="btnAgregarPagar_Click" />
                                                        </div>
                                                        <div class="btn-group">
                                                            <asp:Button type="button" ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="../Compras/ComprasF.aspx" />
                                                        </div>
                                                    </div>
                                                </div>

                                            </fieldset>

                                        </div>

                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>

                            </div>



                        </div>

                    </div>


                </div>
            </div>
        </div>
    </div>

    <div id="modalCuentas" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button id="btnCerrarCta" type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Cuentas contables</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                        <ContentTemplate>
                            <div role="form" class="form-horizontal col-md-12">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <label class="col-md-3">Nivel 1:</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListCtaContables1" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListCtaContables1_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <label class="col-md-3">Nivel 2:</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListCtaContables2" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListCtaContables2_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <label class="col-md-3">Nivel 3:</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListCtaContables3" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListCtaContables3_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <label class="col-md-3">Nivel 4:</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListCtaContables" runat="server" class="form-control" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="ListCtaContables" InitialValue="-1" ForeColor="Red" Font-Bold="true" runat="server" ValidationGroup="CtaContableGroup" />
                                        </div>
                                    </div>
                                    <asp:Label Text="0" ID="lblIdCtaContable" runat="server" Style="display: none;" />
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:LinkButton ID="lbtnAgregarMovCtaCbe" runat="server" class="btn btn-success" Text="Guardar" OnClick="lbtnAgregarMovCtaCbe_Click" ValidationGroup="CtaContableGroup" />
                                <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

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
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>

    <script src="../../Scripts/Application.js"></script>

    <script src="../../Scripts/demo/gallery.js"></script>

    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
    <script src="../../Scripts/demo/notifications.js"></script>

    <script src="../../Scripts/JSFunciones1.js"></script>

    <script>


        $(function () {
            $("#<%= txtFecha.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtVencimiento.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtImputacionCont.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $(function () {
                $("#<%= txtFecha.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

            $(function () {
                $("#<%= txtVencimiento.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

            $(function () {
                $("#<%= txtImputacionCont.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });
        }
    </script>

    <script>
        function cerrarModal2() {
            document.getElementById('btnCerrarCta').click();
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
                if (key == 46 || key == 8 || key == 44)// Detectar . (punto) y backspace (retroceso) y , (coma)
                { return true; }
                else
                { return false; }
            }
            return true;
        }
        function validarNro2(e) {
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
                if (key == 46 || key == 8 || key == 44 || key == 45)// Detectar . (punto) y backspace (retroceso) y , (coma) y -(menos)
                { return true; }
                else
                { return false; }
            }
            return true;
        }
    </script>
</asp:Content>
